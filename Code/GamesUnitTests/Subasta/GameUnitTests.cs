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
			var sut = _context.Sut;

			_context.AssertGameIsWellFormed();
			_context.AssertHandsToPlay();
		}
		
		private class TestContext
		{
			public Game Sut
			{
				get { throw new NotImplementedException(); }
			}

			public void AssertGameIsWellFormed()
			{
				throw new NotImplementedException();
			}

			public void AssertHandsToPlay()
			{
				throw new NotImplementedException();
			}
		}
	}
}
