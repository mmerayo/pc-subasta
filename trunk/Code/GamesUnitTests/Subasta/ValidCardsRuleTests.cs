using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Games.Deck;
using Games.Subasta;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Card = Games.Subasta.Card;

namespace GamesUnitTests.Subasta
{
	[TestFixture]
	class ValidCardsRuleTests
	{
		private const string Oros = "Oros";
		private const string Copas = "Copas";
		private const string Espadas = "Espadas";
		private const string Bastos = "Bastos";

		private TestContext _context;

		[SetUp]
		public void OnSetUp()
		{
			_context=new TestContext();
		}

		[Test,TestCaseSource("CanGetValidMoves_TestCases")]
		public ICard[] CanGetValidMoves(string trumpName, ICard[] currentHandCards, ICard[] playerCards)
		{
			_context.WithTrump(trumpName).WithCurrentHandCards(currentHandCards).WithPlayerCards(playerCards);

			return _context.Result();

		}

		public static IEnumerable CanGetValidMoves_TestCases()
		{aqui
			yield return new TestCaseData(new Card[0], 1).Returns(0);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 1) }, 2).Returns(22);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 2), new Card(Oros, 1) }, 3).Returns(22);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 2), new Card(Oros, 3), new Card(Oros, 1) }, 4).Returns(32);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 10), }, 5).Returns(13);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 11), }, 6).Returns(14);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 12), }, 7).Returns(15);
			yield return new TestCaseData(new[] { new Card(Copas, 4), new Card(Oros, 5), new Card(Oros, 6), new Card(Oros, 7) }, 8).Returns(0);

		}

		private class TestContext
		{
			private readonly Fixture _fixture;
			private ValidCardsRule _sut;
			private ISuit _trump;
			private ICard[] _playerCards;
			private IHand _hand;

			public TestContext()
			{
				_fixture=new Fixture();
				_hand = _fixture.Freeze<Hand>();
			}

			private ValidCardsRule Sut
			{
				get { return _sut??(_sut=_fixture.CreateAnonymous<ValidCardsRule>()); }
			}

			public TestContext WithTrump(string trumpName)
			{
				_trump = Games.Subasta.Suit.FromName(trumpName);
				_fixture.Register<ISuit>(() => _trump);

				return this;
			}

			public TestContext WithPlayerCards(ICard[] playerCards)
			{
				_playerCards = playerCards;
				return this;
			}

			public TestContext WithCurrentHandCards(ICard[] currentHandCards)
			{
				var playerPlays = 1;
				foreach (var existingCard in currentHandCards)
				{
					_hand.Add(playerPlays, existingCard);
					SetNextPlayer(ref playerPlays);
				}

				return this;
			}

			private void SetNextPlayer(ref int playerPlays)
			{
				if (++playerPlays > 4)
					playerPlays = 1;
			}

			public ICard[] Result()
			{
				return Sut.GetValidMoves(_playerCards, _hand);
			}
		}
	}
}
