﻿using System;
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
	class HandUnitTests
	{
		private const string Oros = "Oros";
		private const string Copas = "Copas";
		private const string Espadas = "Espadas";
		private const string Bastos = "Bastos";


		private TestContext _context;
		
		[SetUp]
		public void SetUp()
		{
			_context=new TestContext();
		}

		[Test, TestCaseSource("Can_AddCard_TestCases")]
		public int Can_AddCard(ICard[] existingCards, ICard newCard)
		{

			_context.WithLeadSuit(Oros).WithExistingCards(existingCards, 1);

			int playerPlays = 1 + existingCards.Length;

			if (playerPlays > 4) playerPlays = 1;
			return _context.Sut.Add(playerPlays, newCard);
		}

		public static IEnumerable Can_AddCard_TestCases()
		{
			yield return new TestCaseData(new Card[0], new Card(Oros, 1)).Returns(1);
			yield return new TestCaseData(new[] { new Card(Oros, 1) }, new Card(Oros, 1)).Throws(typeof(InvalidOperationException));
			yield return new TestCaseData(new[] { new Card(Copas, 1) }, new Card(Oros, 1)).Returns(2);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 2) }, new Card(Oros, 1)).Returns(3);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 2), new Card(Oros, 3) }, new Card(Oros, 1)).Returns(4);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 2), new Card(Oros, 3), new Card(Oros, 5) }, new Card(Oros, 1)).Throws(typeof(InvalidOperationException));

		}

		[Test,TestCaseSource("Can_GetIsCompleted_TestCases")]
		public bool Can_GetIsCompleted(ICard[] cards,int i)
		{
			_context.WithLeadSuit(Oros).WithExistingCards(cards, 1);
			return _context.Sut.IsCompleted;
		}

		public static IEnumerable Can_GetIsCompleted_TestCases()
		{
			yield return new TestCaseData(new Card[0],1).Returns(false);
			yield return new TestCaseData(new [] { new Card(Copas, 1), new Card(Oros, 1) },1).Returns(false);
			yield return new TestCaseData(new [] { new Card(Copas, 1), new Card(Oros, 2), new Card(Oros, 1) },1).Returns(false);
			yield return new TestCaseData(new [] { new Card(Copas, 1), new Card(Oros, 2), new Card(Oros, 3), new Card(Oros, 1) },1).Returns(true);
		}

		[Test, TestCaseSource("Can_GetPlayerWinner_TestCases")]
		public int Can_GetWinner(ICard[] cards, int firstPlayer, string leadSuit)
		{
			_context.WithLeadSuit(leadSuit).WithExistingCards(cards, 1);
			return _context.Sut.PlayerWinner;
		}

		public static IEnumerable Can_GetPlayerWinner_TestCases()
		{
			yield return new TestCaseData(new Card[0], 1, Oros).Throws(typeof(InvalidOperationException));
			yield return
				new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 1) }, 2, Oros).Throws(typeof(InvalidOperationException));
			for (int i = 1; i <= 4; i++)
				yield return
					new TestCaseData(new[] { new Card(Copas, 4), new Card(Oros, 5), new Card(Oros, 6), new Card(Oros, 7) }, i, Oros)
						.Returns(4);

			for (int i = 1; i <= 4; i++)
				yield return
					new TestCaseData(new[] { new Card(Copas, 4), new Card(Oros, 5), new Card(Oros, 6), new Card(Oros, 7) }, i, Copas)
						.Returns(1);

			for (int i = 1; i <= 4; i++)
				yield return
					new TestCaseData(new[] { new Card(Copas, 4), new Card(Oros, 5), new Card(Espadas, 6), new Card(Oros, 7) }, i, Copas)
						.Returns(1);

			for (int i = 1; i <= 4; i++)
				yield return
					new TestCaseData(new[] { new Card(Copas, 4), new Card(Oros, 5), new Card(Espadas, 6), new Card(Oros, 7) }, i, Espadas)
						.Returns(3);

			yield return
				new TestCaseData(new[] { new Card(Oros, 1), new Card(Oros, 3), new Card(Oros, 12), new Card(Oros, 11) }, 1, Oros)
					.Returns(1);
			yield return
				new TestCaseData(new[] { new Card(Oros, 3), new Card(Oros, 12), new Card(Oros, 11), new Card(Oros, 10) }, 1, Oros)
					.Returns(1);
			yield return
				new TestCaseData(new[] { new Card(Oros, 12), new Card(Oros, 11), new Card(Oros, 10), new Card(Oros, 7) }, 1, Oros)
					.Returns(1);
			yield return
				new TestCaseData(new[] { new Card(Oros, 11), new Card(Oros, 10), new Card(Oros, 7), new Card(Oros, 6) }, 1, Oros)
					.Returns(1);

		}

		[Test,TestCaseSource("Can_GetPoints_TestCases")]
		public int Can_GetPoints(ICard[] cards, int i)
		{
			_context.WithLeadSuit(Oros).WithExistingCards(cards, 1);
			return _context.Sut.Points;
		}

		public static IEnumerable Can_GetPoints_TestCases()
		{
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
			private Hand _sut;
			private ISuit _leadSuit=null;

			public TestContext()
			{
				_fixture = new Fixture();
			}

			public Hand Sut
			{
				get { return _sut??(_sut=_fixture.CreateAnonymous<Hand>()); }
			}

			public TestContext WithExistingCards(IEnumerable<ICard> existingCards, int playerPlays)
			{
				foreach (var existingCard in existingCards)
				{
					Sut.Add(playerPlays, existingCard);
					SetNextPlayer(ref playerPlays);
				}

				return this;
			}

			private void SetNextPlayer(ref int playerPlays)
			{
				if (++playerPlays > 4)
					playerPlays = 1;
			}

			public TestContext WithLeadSuit(string leadSuitName)
			{
				_leadSuit = Games.Subasta.Suit.FromName(leadSuitName);
				_fixture.Register<ISuit>(() => _leadSuit);
				_fixture.Register<ICardComparer>(() => new CardComparer(_leadSuit));

				return this;
			}
		}
	}
}
