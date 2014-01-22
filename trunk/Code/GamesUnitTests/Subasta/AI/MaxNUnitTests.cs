using System;
using System.Collections;
using System.Collections.Generic;
using Games.Deck;
using Games.Subasta;
using Games.Subasta.AI;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Suit = Games.Subasta.Suit;
namespace GamesUnitTests.Subasta.AI
{
	[TestFixture]
	public class MaxNUnitTests
	{
		private const string Oros = "Oros";
		private const string Copas = "Copas";
		private const string Espadas = "Espadas";
		private const string Bastos = "Bastos";

		private Dictionary<string, ISuit> _suits;
		private TestContext _context;

		[TestFixtureSetUp]
		public void OnFixtureSetUp()
		{
			_suits = new Dictionary<string, ISuit>
				{
					{Oros, new Suit(Oros, 1)},
					{Copas, new Suit(Copas, 1)},
					{Espadas, new Suit(Espadas, 1)},
					{Bastos, new Suit(Bastos, 1)},
				};
		}

		[SetUp]
		public void OnSetUp()
		{
			_context=new TestContext();
		}

		[Test]
		public void CanResolveForFinalHand()
		{
			const int firstPlayer = 1; //TODO: make it dynamic
			_context
				.WithTrump("oros")
				.WithOneHand()
				.WithFirstPlayer(firstPlayer);

			var target = _context.Sut;

			target.Execute(_context.Status, firstPlayer);

			_context.VerifyCanGetBestMoveForPlayer(firstPlayer);

			//TODO: GET THE STATUS??
		}


		private class TestContext
		{
			private MaxN _sut;
			private IFixture _fixture;
			private Status _status;
			private Games.Subasta.Deck _deck;
			
			public TestContext()
			{
				_fixture = new Fixture().Customize(new AutoMoqCustomization());
				_fixture.Register<IValidCardsRule>(()=>new ValidCardsRule());
				_deck = _fixture.CreateAnonymous<Games.Subasta.Deck>();
				
			}

			public TestContext WithTrump(string Trump)
			{
				_fixture.Register<ICardComparer>(() => new CardComparer(Suit.FromName(Trump)));
				_status = _fixture.Freeze<Status>();
				return this;
			}

			public TestContext WithFirstPlayer(int firstPlayer)
			{
				_status.Turn=firstPlayer;

				return this;
			}

			public TestContext WithOneHand()
			{
				_status.SetCards(1, new[] {_deck.Get(1, "Oros")});

				_status.SetCards(2, new[] {_deck.Get(2, "Oros")});

				_status.SetCards(3, new[] {_deck.Get(3, "Oros")});

				_status.SetCards(4, new[] {_deck.Get(4, "Oros")});

				return this;
			}

			public MaxN Sut
			{
				get { return _sut ?? (_sut = _fixture.CreateAnonymous<MaxN>()); }
			}

			public Status Status
			{
				get { return _status; }
			}

			public void VerifyCanGetBestMoveForPlayer(int player)
			{
				throw new NotImplementedException();
			}
		}
	}
}
