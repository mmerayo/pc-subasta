using System;
using System.Linq;
using NUnit.Framework;

namespace GamesUnitTests.Deck.Spanish
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
			Games.Deck.Spanish.Deck actual = _context.Sut;

			_context.AssertIsComplete();
		}

		private class TestContext
		{
			private Games.Deck.Spanish.Deck _sut;

			public Games.Deck.Spanish.Deck Sut
			{
				get
				{
					_sut = new Games.Deck.Spanish.Deck();
					return _sut;
				}
			}

			public void AssertIsComplete()
			{
				var actual = _sut.Cards.ToList();
				Assert.AreEqual(40,actual.Count());
				for (var i = 1; i < 13; i++)
				{
					if (i != 8 && i != 9)
					{
						Assert.IsTrue(1==actual.Count(x=>x.Number==i && x.Suit.Name.Equals("oros",StringComparison.InvariantCultureIgnoreCase)));
						Assert.IsTrue(1 == actual.Count(x => x.Number == i && x.Suit.Name.Equals("copas", StringComparison.InvariantCultureIgnoreCase)));
						Assert.IsTrue(1 == actual.Count(x => x.Number == i && x.Suit.Name.Equals("espadas", StringComparison.InvariantCultureIgnoreCase)));
						Assert.IsTrue(1 == actual.Count(x => x.Number == i && x.Suit.Name.Equals("bastos", StringComparison.InvariantCultureIgnoreCase)));
					}
				}
				
			}
		}
	}
}
