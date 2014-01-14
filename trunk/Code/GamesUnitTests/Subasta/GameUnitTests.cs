using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Games.Subasta;
using NUnit.Framework;

//TODO: REMOVE
//1-Create Game
//2- Create set -> shuffle -> starts bet
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
		public void Can_StartGame()
		{
			_context.WithPlayers();

			var sut = _context.Sut;
			
			sut.StartNew(_context.GameConfiguration);

			_context.AssertHasPlayers();
			_context.AssertCreatesFirstSet();
		}
		
		private class TestContext
		{

			public Game Sut
			{
				get { return new Game(); }
			}

			public GameConfiguration GameConfiguration
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
