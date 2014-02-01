using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Games.Deck;
using Games.Subasta;
using Games.Subasta.GameGeneration.AI;
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

			_context.WithTrump(Oros).WithExistingCards(existingCards, 1);

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

		[Test, TestCaseSource("Can_AddDeclaration_TestCases")]
		public Declaration? Can_AddDeclaration(ICard[] existingCards, Declaration declaration)
		{

			_context.WithTrump(Oros).WithExistingCards(existingCards, 1);

			_context.Sut.Add(declaration);

			return _context.Sut.Declaration;
		}

		public static IEnumerable Can_AddDeclaration_TestCases()
		{
			yield return new TestCaseData(new Card[0], Declaration.ParejaBastos).Throws(typeof(InvalidOperationException));
			yield return
				new TestCaseData(new[] {new Card(Copas, 1), new Card(Oros, 2), new Card(Oros, 3), new Card(Oros, 5)},
				                 Declaration.ParejaBastos).Returns(Declaration.ParejaBastos);

		}


		[Test,TestCaseSource("Can_GetIsCompleted_TestCases")]
		public bool Can_GetIsCompleted(ICard[] cards,int i)
		{
			_context.WithTrump(Oros).WithExistingCards(cards, 1);
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
		public int Can_GetWinner(ICard[] cards, int firstPlayer, string trump)
		{
			_context.WithTrump(trump).WithExistingCards(cards, 1);
			Console.WriteLine("Cards:{0}", _context.Sut);
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

		[Test, TestCaseSource("Can_GetCardWinner_TestCases")]
		public ICard Can_GetCardWinner(ICard[] cards, int firstPlayer, string trump)
		{
			_context.WithTrump(trump).WithExistingCards(cards, 1);
			Console.WriteLine("Cards: {0}",_context.Sut);
			return _context.Sut.CardWinner;
		}

		public static IEnumerable Can_GetCardWinner_TestCases()
		{
			yield return new TestCaseData(new Card[0], 1, Oros).Throws(typeof(InvalidOperationException));
			yield return
				new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 1) }, 1, Oros).Returns(new Card(Oros,1));

			for (int i = 1; i <= 4; i++)
				yield return
					new TestCaseData(new[] { new Card(Copas, 4), new Card(Oros, 5), new Card(Oros, 6), new Card(Oros, 7) }, i, Oros)
						.Returns(new Card(Oros,7));

			for (int i = 1; i <= 4; i++)
				yield return
					new TestCaseData(new[] { new Card(Copas, 4), new Card(Oros, 5), new Card(Oros, 6), new Card(Oros, 7) }, i, Copas)
						.Returns(new Card(Copas,4));

			for (int i = 1; i <= 4; i++)
				yield return
					new TestCaseData(new[] { new Card(Copas, 4), new Card(Oros, 5), new Card(Espadas, 6), new Card(Oros, 7) }, i, Copas)
						.Returns(new Card(Copas,4));

			yield return
				new TestCaseData(new[] { new Card(Oros, 1), new Card(Oros, 3), new Card(Oros, 12), new Card(Oros, 11) }, 1, Oros)
					.Returns(new Card(Oros,1));
			yield return
				new TestCaseData(new[] { new Card(Oros, 3), new Card(Oros, 12), new Card(Oros, 11), new Card(Oros, 10) }, 1, Oros)
					.Returns(new Card(Oros, 3));
			yield return
				new TestCaseData(new[] { new Card(Oros, 12), new Card(Oros, 11), new Card(Oros, 10), new Card(Oros, 7) }, 1, Oros)
					.Returns(new Card(Oros, 12));
			yield return
				new TestCaseData(new[] { new Card(Oros, 11), new Card(Oros, 10), new Card(Oros, 7), new Card(Oros, 6) }, 1, Oros)
					.Returns(new Card(Oros, 11));

			//IMPORTANTE
			yield return
				new TestCaseData(new[] { new Card(Copas, 4), new Card(Bastos, 3), new Card(Espadas, 10) }, 1, Oros)
				.Returns(new Card(Copas, 4));

		}


		[Test,TestCaseSource("Can_GetPoints_TestCases")]
		public int Can_GetPoints(ICard[] cards, int i,Declaration? declaration=null)
		{
			_context
				.WithTrump(Oros)
				.WithExistingCards(cards, 1)
				.WithDeclaration(declaration);
			return _context.Sut.Points;
		}

		public static IEnumerable Can_GetPoints_TestCases()
		{
			yield return new TestCaseData(new Card[0], 1, null).Returns(0);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 1) }, 2,null).Returns(22);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 2), new Card(Oros, 1) }, 3, null).Returns(22);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 2), new Card(Oros, 3), new Card(Oros, 1) }, 4, null).Returns(32);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 10), }, 5, null).Returns(13);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 11), }, 6, null).Returns(14);
			yield return new TestCaseData(new[] { new Card(Copas, 1), new Card(Oros, 12), }, 7, null).Returns(15);
			yield return new TestCaseData(new[] { new Card(Copas, 4), new Card(Oros, 5), new Card(Oros, 6), new Card(Oros, 7) }, 8, null).Returns(0);

			var declarations = Enum.GetValues(typeof(Declaration)).Cast<Declaration>();
			foreach (var declaration in declarations)
				yield return new TestCaseData(new[] { new Card(Copas, 3), new Card(Oros, 5), new Card(Oros, 6), new Card(Oros, 7) }, 8,declaration).Returns(10+ GetDeclarationValue(declaration));
				
		}

		private static int GetDeclarationValue(Declaration declaration)
		{
			switch(declaration)
			{
				case Declaration.Reyes:
					return 120;
				case Declaration.Caballos:
					return 60;
				case Declaration.ParejaOros:
				case Declaration.ParejaCopas:
				case Declaration.ParejaEspadas:
				case Declaration.ParejaBastos:
					return 20;
				case Declaration.Cuarenta:
					return 40;
				default:
					throw new ArgumentOutOfRangeException("declaration");
			}
		}

		private class TestContext
		{
			private readonly Fixture _fixture;
			private Hand _sut;
			private ISuit _Trump=null;
			

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

			public TestContext WithTrump(string TrumpName)
			{
				_Trump = Games.Subasta.Suit.FromName(TrumpName);
				_fixture.Register<ISuit>(() => _Trump);
				_fixture.Register<ICardComparer>(() => new CardComparer(_Trump));

				return this;
			}

			public TestContext WithDeclaration(Declaration? declaration)
			{
				if(declaration.HasValue)
					Sut.Add(declaration.Value);
				return this;
			}
		}
	}
}
