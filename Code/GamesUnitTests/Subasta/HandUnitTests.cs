using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Games.Deck;
using Games.Subasta;
using NUnit.Framework;
using Card = Games.Subasta.Card;

namespace GamesUnitTests.Subasta
{
	[TestFixture]
	class HandUnitTests
	{
		private TestContext _context;
		
		[SetUp]
		public void SetUp()
		{
			_context=new TestContext();
		}

		[Test,TestCaseSource("Can_AddCard_TestCases")]
		public int Can_AddCard(ICard[] existingCards,ICard newCard)
		{
			_context = new TestContext();

			var target = _context.Sut;
			int playerPlays=1;
			foreach (var existingCard in existingCards)
				target.Add(playerPlays++, existingCard);

			return target.Add(playerPlays, newCard);
		}

		public static IEnumerable Can_AddCard_TestCases()
		{
			yield return new TestCaseData(new[]{new Card("Oros",1) }, 1).Returns(1);
		}

		[Test]
		public void Cant_AddCardIfHandIsComplete()
		{
			_context = new TestContext();


		}

		[Test]
		public void GetWinner_Throws_When_Incompleted()
		{
			throw new NotImplementedException();
		}

		[Test]
		public void Can_GetIsCompleted()
		{
			throw new NotImplementedException();
		}

		[Test]
		public void Can_DetermineWinner()
		{
			throw new NotImplementedException();
		}

		[Test]
		public void Can_GetPoints()
		{
			throw new NotImplementedException();
		}


		private class TestContext
		{
			public Hand Sut
			{
				get { throw new NotImplementedException(); }
			}
		}
	}
}
