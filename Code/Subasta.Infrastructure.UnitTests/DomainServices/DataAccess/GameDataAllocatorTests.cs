﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Subasta.Infrastructure.DomainServices.DataAccess;
using Subasta.Infrastructure.UnitTests.Tools.Autofixture;

namespace Subasta.Infrastructure.UnitTests.DomainServices.DataAccess
{
	[TestFixture]
	class GameDataAllocatorTests
	{
		private TestContext _context;

		[SetUp]
		public void OnSetUp()
		{
			_context=new TestContext();
		}

		[Test]
		public void Can_CreateNewGameStorage()
		{
			Guid gameId = _context.Sut.CreateNewGameStorage();
			_context.VerifyGameWasCreated(gameId);
		}

		[Test,Theory]
		public void Can_RecordGameResult(bool success)
		{
			throw new NotImplementedException();
		}

		private class TestContext
		{
			private readonly IFixture _fixture;

			public TestContext()
			{
				_fixture=new Fixture().Customize(new SubastaAutoFixtureCustomizations());
			}

			public GameDataAllocator Sut { get { return _fixture.CreateAnonymous<GameDataAllocator>(); } }

			public void VerifyGameWasCreated(Guid gameId)
			{
				throw new NotImplementedException();
			}
		}
	}
}
