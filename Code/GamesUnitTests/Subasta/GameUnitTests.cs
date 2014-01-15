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
			const int dealerPosition = 2;
			_context
				.WithPlayers()
				.WithInitialSet()
				.WithInitialDealer(dealerPosition);

			var sut = _context.Sut;
			
			sut.StartNew(_context.GameConfiguration);

			_context.AssertHasPlayers();
			_context.AssertCreatesFirstSet();
			_context.AssertStartsFirstSet();
			_context.AssertCurrentDealer(dealerPosition);
		}

		[TestCase(1, Result = 2)]
		[TestCase(2, Result = 3)]
		[TestCase(3, Result = 4)]
		[TestCase(4, Result = 1)]
		public int IncrementsCurrentDealerAfterSetIsCompleted(int dealerPosition)
		{
			_context
				.WithPlayers()
				.WithInitialSet()
				.WithInitialDealer(dealerPosition);

			var sut = _context.Sut;
			sut.StartNew(_context.GameConfiguration);

			_context.TriggerEndSet();

			return int.Parse(_context.Sut.CurrentDealer.Name.Substring(1, 1));
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
				GameConfiguration.AddPlayer(1,CreatePlayer(false,"P1"));
				GameConfiguration.AddPlayer(2, CreatePlayer(false, "P2"));
				GameConfiguration.AddPlayer(3, CreatePlayer(false, "P3"));
				GameConfiguration.AddPlayer(4, CreatePlayer(false, "P4"));
				
				return this;
			}

			public TestContext WithInitialDealer(int dealerPosition)
			{
				GameConfiguration.SetInitialDealer(dealerPosition);

				return this;
			}

			public TestContext WithInitialSet()
			{
				_initialSet = _fixture.CreateAnonymous<Mock<ISet>>();
				_initialSet.Setup(x => x.Run()).Verifiable();


				_setFactory.Setup(x => x.CreateNew()).Returns(_initialSet.Object).Verifiable();
				return this;
			}

			private IPlayer CreatePlayer(bool isHuman, string playerName)
			{
				var result = _fixture.CreateAnonymous < Mock<IPlayer>>();
				result.SetupGet(x => x.IsHuman).Returns(isHuman);
				result.SetupGet(x => x.Name).Returns(playerName);
				return result.Object;
			}

			public void AssertHasPlayers()
			{
				for (int i = 0; i < 4; i++)
				{
					var current = Sut.Players[i];
					Assert.IsNotNull(current);
					Assert.AreEqual(string.Format("P{0}",i+1),current.Name);
				}
			}

			public void AssertCreatesFirstSet()
			{
				Assert.That(Sut.Sets.Count,Is.EqualTo(1));
			}

			public void AssertStartsFirstSet()
			{
				_initialSet.Verify(x=>x.Run(),Times.Once());
			}


			public void AssertCurrentDealer(int dealerPosition)
			{
				Assert.AreEqual(string.Format("P{0}", dealerPosition), Sut.CurrentDealer.Name);
			}

			public void TriggerEndSet()
			{
				_initialSet.Raise(x=>x.OnCompleted+=null,_initialSet.Object);
			}
		}
	}
}
