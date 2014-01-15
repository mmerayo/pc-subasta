using System;
using System.Linq;
using Games.Subasta;
using NUnit.Framework;

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

		private class TestContext
		{
			private Deck _sut;

			public Deck Sut
			{
				get
				{
					_sut = new Deck();
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
		}
	}
}
