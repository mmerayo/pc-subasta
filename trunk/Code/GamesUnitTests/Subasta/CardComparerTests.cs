using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Games.Deck;
using Games.Subasta;
using NUnit.Framework;

namespace GamesUnitTests.Subasta
{
	[TestFixture]
	class CardComparerTests
	{
		[Test, TestCaseSource()]
		public ICard CanGetBest(string id, ISuit trump, ICard currentWinner, ICard candidate)
		{
			Console.WriteLine("ID:{0}",id);
			var target = new CardComparer(trump);

			return target.Best(currentWinner, candidate);
		}

		public static IEnumerable

	}
}
