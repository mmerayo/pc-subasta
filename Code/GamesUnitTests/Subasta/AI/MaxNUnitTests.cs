using System;
using System.Collections;
using System.Collections.Generic;
using Games.Deck;
using Games.Subasta.AI;
using NUnit.Framework;
using Ploeh.AutoFixture;

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

		[Test, TestCaseSource(typeof(TestCasesFactory),"BasicBestMoveTestCases")]
		public void CanConfigureBestMoves(Status status)
		{
			throw new NotImplementedException();
		}

		
		private class TestContext
		{
			private Fixture _fixture;


		}

		private static class TestCasesFactory
		{

			public static IEnumerable BasicBestMoveTestCases
			{
				get
				{
					yield return new TestCaseData(
						new Status(
							));
				}
			}
		}
	}
}
