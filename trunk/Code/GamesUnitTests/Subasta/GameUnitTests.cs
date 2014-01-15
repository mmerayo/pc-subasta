using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Games;
using Games.Subasta;
using Games.Subasta.Sets;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

//TODO: REMOVE
//1-Create Game
//2- Create set -> shuffle -> starts bet
namespace GamesUnitTests.Subasta
{
	[TestFixture]
	class GameUnitTests
	{
		private TestContext _context;

		[SetUp]
		public void SetUp()
		{
			_context=new TestContext();
		}

		[Test]
		public void Can_StartGame()
		{
			_context
				.WithPlayers()
				.WithInitialSet();

			var sut = _context.Sut;
			
			sut.StartNew(_context.GameConfiguration);

			_context.AssertHasPlayers();
			_context.AssertCreatesFirstSet();
			_context.AssertStartsFirstSet();
		}
		
		private class TestContext
		{
			private readonly IFixture _fixture;
			private readonly GameConfiguration _gameConfiguration;
			private Game _sut;
			private Mock<ISet> _initialSet;
			private readonly Mock<ISetFactory> _setFactory;
			public TestContext()
			{
				_fixture=new Fixture().Customize(new AutoMoqCustomization());
				_gameConfiguration=new GameConfiguration();
				_setFactory = _fixture.Freeze<Mock<ISetFactory>>();
			}

			public Game Sut
			{
				get { return _sut ?? (_sut = _fixture.CreateAnonymous<Game>()); }
			}

			public GameConfiguration GameConfiguration
			{
				get { return _gameConfiguration; }
			}

			public TestContext WithPlayers()
			{
				GameConfiguration.AddPlayer(1,CreatePlayer(false));
				GameConfiguration.AddPlayer(2, CreatePlayer(false));
				GameConfiguration.AddPlayer(3, CreatePlayer(false));
				GameConfiguration.AddPlayer(4, CreatePlayer(false));
				return this;
			}

			public TestContext WithInitialSet()
			{
				_initialSet = _fixture.CreateAnonymous<Mock<ISet>>();
				_initialSet.Setup(x => x.Start()).Verifiable();
				_setFactory.Setup(x => x.CreateNew()).Returns(_initialSet.Object).Verifiable();
				return this;
			}

			private IPlayer CreatePlayer(bool isHuman)
			{
				var result = _fixture.CreateAnonymous < Mock<IPlayer>>();
				result.SetupGet(x => x.IsHuman).Returns(isHuman);
				return result.Object;
			}

			public void AssertHasPlayers()
			{
				for(int i=0;i<4;i++)
					Assert.IsNotNull(Sut.Players[i]);
			}

			public void AssertCreatesFirstSet()
			{
				Assert.That(Sut.Sets.Count,Is.EqualTo(1));
			}

			public void AssertStartsFirstSet()
			{
				_initialSet.Verify(x=>x.Start(),Times.Once());
			}

			
		}
	}
}
