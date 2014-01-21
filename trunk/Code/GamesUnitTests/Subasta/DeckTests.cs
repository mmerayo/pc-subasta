using System;
using System.Linq;
using Games.Deck;
using NUnit.Framework;
using Deck = Games.Subasta.Deck;

namespace GamesUnitTests.Subasta
{
	[TestFixture]
	class DeckTests
	{
		private TestContext _context;

		[SetUp]
		public void OnSetUp()
		{
			_context= new TestContext();
		}

		[Test]
		public void Can_CreateDeck()
		{
			Deck actual = _context.Sut;

			_context.AssertIsComplete();
		}

		[Test]
		public void Can_GetCard()
		{
			Deck actual = _context.Sut;

			for (int i = 1; i < 13; i++)
				if (i != 8 && i != 9)
				{
					_context.AssertCanGet(i, "Oros");
					_context.AssertCanGet(i, "oros");
					_context.AssertCanGet(i, "Copas");
					_context.AssertCanGet(i, "copas");
					_context.AssertCanGet(i, "Espadas");
					_context.AssertCanGet(i, "espadas");
					_context.AssertCanGet(i, "Bastos");
					_context.AssertCanGet(i, "bastos");
				}

		}

		private class TestContext
		{
			private Deck _sut;

			public Deck Sut
			{
				get { return _sut ?? (_sut = new Deck()); }
			}

			public void AssertIsComplete()
			{
				var actual = _sut.Cards.Cards;
				Assert.AreEqual(40,actual.Count());
				for (var i = 1; i < 13; i++)
				{
					if (i != 8 && i != 9)
					{
						Assert.IsTrue(1 ==
						              actual.Count(
							              x => x.Number == i && x.Suit.Name.Equals("oros", StringComparison.InvariantCultureIgnoreCase)));
						Assert.IsTrue(1 ==
						              actual.Count(
							              x => x.Number == i && x.Suit.Name.Equals("copas", StringComparison.InvariantCultureIgnoreCase)));
						Assert.IsTrue(1 ==
						              actual.Count(
							              x => x.Number == i && x.Suit.Name.Equals("espadas", StringComparison.InvariantCultureIgnoreCase)));
						Assert.IsTrue(1 ==
						              actual.Count(
							              x => x.Number == i && x.Suit.Name.Equals("bastos", StringComparison.InvariantCultureIgnoreCase)));
					}
				}

			}

			public void AssertCanGet(int number, string suitName)
			{
				var target = Sut.Get(number, suitName);
				Assert.IsNotNull(target);

				Assert.AreEqual(number, target.Number);
				Assert.AreEqual(suitName.ToLower(), target.Suit.Name.ToLower());
			}
		}
	}
}
