﻿using System;
using NUnit.Framework;
using StructureMap;
using Subasta.DomainServices.Game;

namespace Subasta.IntegrationTests
{
	[TestFixture]
	public class GameGeneration
	{
		[TestFixtureSetUp]
		public void OnFixtureSetup()
		{
			IoCRegistrator.Register();
		}

		[Test]
		public void CanGenerateGame()
		{
			var gameGenerator = ObjectFactory.GetInstance<IGameGenerator>();
			Guid gameId;
			bool result=gameGenerator.TryGenerateNewGame(out gameId);

			Assert.IsTrue(result);
			Assert.AreEqual(Guid.Empty,gameId);
		}
	}
}