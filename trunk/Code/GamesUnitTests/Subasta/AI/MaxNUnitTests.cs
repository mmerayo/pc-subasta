using System;
using System.Collections;
using System.Collections.Generic;
using Games.Deck;
using Games.Subasta.AI;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

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
		public void CanResolveForLastHandAndFirstPlayer()
		{
			int firstPlayer = 1;//TODO: make it dynamic
			_context
				.WithOneHand()
				.WithFirstPlayer(firstPlayer);

			var target = _context.Sut;

			target.Execute(_context.InitialStatus, firstPlayer);

			for (int i = 1; i < 4;i++ )
				_context.VerifyCanGetBestMoveForPlayer(i);

			//TODO: GET THE STATUS??
		}


		private class TestContext
		{
			private MaxN _sut;
			private IFixture _fixture;
			private Status _initialStatus;
			private Games.Subasta.Deck _deck;

			public TestContext()
			{
				_fixture = new Fixture().Customize(new AutoMoqCustomization());
				_deck = _fixture.CreateAnonymous<Games.Subasta.Deck>();
				_initialStatus = new Status();
			}

			public TestContext WithFirstPlayer(int firstPlayer)
			{
				_initialStatus.SetTurn(firstPlayer);

				return this;
			}

			public TestContext WithOneHand()
			{
				_initialStatus.SetCards(1, new[] {_deck.Get(1, "Oros")});

				_initialStatus.SetCards(1, new[] {_deck.Get(2, "Oros")});

				_initialStatus.SetCards(1, new[] {_deck.Get(3, "Oros")});

				_initialStatus.SetCards(1, new[] {_deck.Get(4, "Oros")});

				return this;
			}

			public TestContext WithTwoHands()
			{
				throw new NotImplementedException();
			}

			public MaxN Sut
			{
				get { return _sut ?? (_sut = new MaxN()); }
			}

			public Status InitialStatus
			{
				get { throw new NotImplementedException(); }
			}

			public void VerifyCanGetBestMoveForPlayer(int player)
			{
				throw new NotImplementedException();
			}
		}
	}
}
