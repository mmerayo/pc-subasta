using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Games.Deck;
using Games.Subasta;
using NUnit.Framework;
using Suit = Games.Subasta.Suit;
using Card = Games.Subasta.Card;

namespace GamesUnitTests.Subasta
{
	[TestFixture]
	class CardComparerTests
	{
		private const string Oros = "Oros";
		private const string Copas = "Copas";
		private const string Espadas = "Espadas";
		private const string Bastos = "Bastos";


		public static ISuit GetSuit(string name)
		{
			return Suit.FromName(name);
		}

		public static ICard GetCard(string suit, int number)
		{
			return new Card(GetSuit(suit), number);
		}

		[Test, TestCaseSource("CanGetBest_TestCases")]
		public ICard CanGetBest(string id, ISuit trump, ICard currentWinner, ICard candidate)
		{
			Console.WriteLine("ID:{0}",id);
			var target = new CardComparer(trump);

			return target.Best(currentWinner, candidate);
		}

		public static IEnumerable CanGetBest_TestCases()
		{

			//de arrastre
			ICard expected ;
			for (int i = 2; i <= 12; i++)
			{
				if (i > 7 && i < 10) continue;
				expected = GetCard(Oros, 1);
				yield return
					new TestCaseData("100", Suit.FromName(Oros), expected, GetCard(Oros, i)).Returns(expected);
			}

			for (int i = 2; i <= 12; i++)
			{
				if (i > 7 && i < 10) continue;
				 expected = GetCard(Oros, 1);
				yield return
					new TestCaseData("200", Suit.FromName(Oros), GetCard(Oros, i), expected).Returns(expected);
			}

			//fallando
			for (int i = 1; i <= 12; i++)
			{
				if (i > 7 && i < 10) continue;
				 expected = GetCard(Oros, 1);
				yield return
					new TestCaseData("300", Suit.FromName(Oros), GetCard(Bastos, i), expected).Returns(expected);
			}


			//no asistiendo arrastre
			for (int i = 1; i <= 12; i++)
			{
				if (i > 7 && i < 10) continue;
				 expected = GetCard(Oros, 1);
				yield return
					new TestCaseData("400", Suit.FromName(Oros), expected, GetCard(Bastos, i)).Returns(expected);
			}

			//no asistiendo
			for (int i = 1; i <= 12; i++)
			{
				if (i > 7 && i < 10) continue;
				 expected = GetCard(Espadas, 1);
				yield return
					new TestCaseData("500", Suit.FromName(Oros), expected, GetCard(Bastos, i)).Returns(expected);
			}


			//cartas sin puntos

			 expected = GetCard(Bastos, 4);
			yield return
					new TestCaseData("600", Suit.FromName(Oros), expected, GetCard(Bastos, 2)).Returns(expected);
			
		}

	}
}
