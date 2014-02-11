using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Subasta.ApplicationServices;
using Subasta.DomainServices.Game;
using Subasta.Infrastructure.ApplicationServices;

namespace Subasta.Infrastructure.UnitTests.ApplicationServices
{
	[TestFixture]
	class GameGeneratorTests
	{
		private TestContext _context;

		[SetUp]
		public void OnSetUp()
		{
			_context=new TestContext();
		}

		[Test]
		public void Can_GenerateNewGame()
		{
			_context.WithGenerateNewGameExpectations();

			Guid result=_context.Sut.GenerateNewGame();

			Assert.That(result, Is.Not.Empty);
			_context.VerifySuffleWasCalled();
			_context.VerifyGameExplorerWasCalled();
			_context.VerifyDbWasCreated();
		}

		private class TestContext
		{
			private readonly IFixture _fixture;
			private IGameExplorer _gameExplorer;
			private IDeckSuffler _suffler;
			public TestContext()
			{
				_fixture=new Fixture();
				_gameExplorer = _fixture.Freeze<IGameExplorer>();
				_suffler = _fixture.Freeze<IDeckSuffler>();
			}

			public GameGenerator Sut
			{
				get { return _fixture.CreateAnonymous<GameGenerator>(); }
			}

			public void VerifySuffleWasCalled()
			{
				throw new NotImplementedException();
			}

			public void VerifyGameExplorerWasCalled()
			{
				throw new NotImplementedException();
			}

			public void VerifyDbWasCreated()
			{
				throw new NotImplementedException();
			}

			public TestContext WithGenerateNewGameExpectations()
			{
				throw new NotImplementedException();
			}
		}
	}
}
