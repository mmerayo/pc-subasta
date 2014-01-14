using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Games.Subasta;
using NUnit.Framework;

namespace GamesUnitTests.Subasta
{
	[TestFixture]
	class GameUnitTests
	{
		private TestContext _context;

		[SetUp]
		public void SetUp()
		{
			_context=new TestContext();
		}

		[Test]
		public void Can_CreateGame()
		{
			_context.WithPlayers();

			var sut = _context.Sut;

			_context.AssertHasPlayers();
			_context.AssertCreatesFirstSet();
			
		}
		
		private class TestContext
		{
			public Game Sut
			{
				get { throw new NotImplementedException(); }
			}
			
			public void WithPlayers()
			{
				throw new NotImplementedException();
			}

			public void AssertHasPlayers()
			{
				throw new NotImplementedException();
			}

			public void AssertCreatesFirstSet()
			{
				throw new NotImplementedException();
			}

		}
	}
}
