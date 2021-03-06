﻿using System;
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
			_context.WithGenerateNewGameExpectations(fail).ExpectFailureWhileGenerating(fail);

			Guid result;
			Assert.That(_context.Sut.TryGenerateNewGame(out result),Is.EqualTo(!fail));
			if(!fail)
				Assert.That(result, Is.Not.EqualTo(Guid.Empty));
			else
				Assert.That(result, Is.EqualTo(Guid.Empty));
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

			public TestContext WithGenerateNewGameExpectations(bool fail)
			{
				if (!fail)
				{
					_gameExplorer.Expect(
						x =>
						x.Execute(Arg<Guid>.Is.Anything, Arg<int>.Is.Equal(1), Arg<ICard[]>.Is.Anything, Arg<ICard[]>.Is.Anything,
						          Arg<ICard[]>.Is.Anything, Arg<ICard[]>.Is.Anything, Arg<ISuit>.Is.Equal(Suit.FromName("Oros"))));
					_gameExplorer.Expect(
						x =>
						x.Execute(Arg<Guid>.Is.Anything, Arg<int>.Is.Equal(1), Arg<ICard[]>.Is.Anything, Arg<ICard[]>.Is.Anything,
						          Arg<ICard[]>.Is.Anything, Arg<ICard[]>.Is.Anything, Arg<ISuit>.Is.Equal(Suit.FromName("Copas"))));
					_gameExplorer.Expect(
						x =>
						x.Execute(Arg<Guid>.Is.Anything, Arg<int>.Is.Equal(1), Arg<ICard[]>.Is.Anything, Arg<ICard[]>.Is.Anything,
						          Arg<ICard[]>.Is.Anything, Arg<ICard[]>.Is.Anything, Arg<ISuit>.Is.Equal(Suit.FromName("Espadas"))));
					_gameExplorer.Expect(
						x =>
						x.Execute(Arg<Guid>.Is.Anything, Arg<int>.Is.Equal(1), Arg<ICard[]>.Is.Anything, Arg<ICard[]>.Is.Anything,
						          Arg<ICard[]>.Is.Anything, Arg<ICard[]>.Is.Anything, Arg<ISuit>.Is.Equal(Suit.FromName("Bastos"))));

					_suffler.Expect(x => x.Suffle(Arg<IDeck>.Is.Anything)).Return(Deck.DefaultForSubasta);
					_dataAllocator.Expect(x => x.CreateNewGame()).Return(Guid.NewGuid());
				}
				else
				{
					_dataAllocator.Expect(x => x.CreateNewGame()).Throw(new InvalidOperationException());
				}

				return this;
			}


			public TestContext ExpectFailureWhileGenerating(bool fail)
			{
				_fail = fail;
				if(!fail)
					_dataAllocator.Expect(x => x.RecordGenerationOutput(Arg<Guid>.Is.Anything, Arg<bool>.Is.Equal(true)));
				return this;
			}
		}
	}
}