using System;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;
using Subasta.Domain.Deck;
using Subasta.DomainServices.DataAccess;
using Subasta.DomainServices.Game;
using Subasta.Infrastructure.Domain;
using Subasta.Infrastructure.DomainServices.Game;

namespace Subasta.Infrastructure.UnitTests.DomainServices
{
	[TestFixture]
	internal class GameGeneratorTests
	{
		private TestContext _context;

		[SetUp]
		public void OnSetUp()
		{
			_context = new TestContext();
		}

		[Test, Theory]
		public void Can_GenerateNewGame(bool fail)
		{
			_context.WithGenerateNewGameExpectations().ExpectFailureWhileGenerating(fail);

			Guid result = _context.Sut.GenerateNewGame();

			Assert.That(result, Is.Not.Empty);
			_context.VerifySuffleWasCalled();
			_context.VerifyGameExplorerWasCalled();
			_context.VerifyGameWasCreated();
		}



		private class TestContext
		{
			private readonly IFixture _fixture;
			private IGameExplorer _gameExplorer;
			private IDeckSuffler _suffler;
			private IGameDataAllocator _dataAllocator;
			private bool _fail;

			public TestContext()
			{
				_fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
				_gameExplorer = _fixture.Freeze<IGameExplorer>();
				_suffler = _fixture.Freeze<IDeckSuffler>();
				_dataAllocator = _fixture.Freeze<IGameDataAllocator>();
			}

			public GameGenerator Sut
			{
				get { return _fixture.CreateAnonymous<GameGenerator>(); }
			}

			public void VerifySuffleWasCalled()
			{
				_suffler.VerifyAllExpectations();
			}

			public void VerifyGameExplorerWasCalled()
			{
				_gameExplorer.VerifyAllExpectations();
			}

			public void VerifyGameWasCreated()
			{
				_dataAllocator.VerifyAllExpectations();
			}

			public TestContext WithGenerateNewGameExpectations()
			{
				_gameExplorer.Expect(x => x.Execute(Arg<Status>.Is.Anything, Arg<int>.Is.Equal(1))).Repeat.Times(4);
				_suffler.Expect(x => x.Suffle(Arg<IDeck>.Is.Anything)).Return(Deck.DefaultForSubasta);
				_dataAllocator.Expect(x => x.CreateNewGame()).Return(Guid.NewGuid());

				return this;
			}


			public TestContext ExpectFailureWhileGenerating(bool fail)
			{
				_fail = fail;
				_dataAllocator.Expect(x => x.RecordGenerationOutput(Arg<Guid>.Is.Anything, Arg<bool>.Is.Equal(!fail)));
				return this;
			}
		}
	}
}