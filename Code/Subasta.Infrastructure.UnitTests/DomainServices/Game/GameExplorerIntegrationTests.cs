using System;
using System.Collections;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Subasta.Domain.Deck;
using Subasta.DomainServices.Game;
using Subasta.Infrastructure.Domain;
using Subasta.Infrastructure.DomainServices.Game;
using Subasta.Infrastructure.UnitTests.Tools.Autofixture;

namespace Subasta.Infrastructure.UnitTests.DomainServices.Game
{
	[TestFixture]
	public class GameExplorerIntegrationTests
	{
		private const string Oros = "Oros";
		private const string Copas = "Copas";
		private const string Espadas = "Espadas";
		private const string Bastos = "Bastos";

		private TestContext _context;

		[SetUp]
		public void OnSetUp()
		{
			_context=new TestContext();
		}

		[Test,TestCaseSource("CanResolveForFinalHand_TestCases")]
		public ICard CanResolveForFinalHand(string trump, 
			ICard[] cardsP1, ICard[] cardsP2, ICard[] cardsP3, ICard[] cardsP4,
		                                   int firstPlayer,int playerEvaluated, int moveNumber)
		{
			_context
				.WithTrump(trump)
				.WithCards(cardsP1, cardsP2, cardsP3, cardsP4)
				.WithFirstPlayer(firstPlayer)
				.WithPlayerBet(firstPlayer);
				

			var nodeResult = _context.Sut.Execute(_context.Status, firstPlayer);

			return nodeResult.CardAtMove(playerEvaluated, moveNumber);
		}

		private static IEnumerable CanResolveForFinalHand_TestCases()
		{
			//ultimo
			yield return
				new TestCaseData(Oros,
								 new[] { new Card(Oros, 1) },
								 new[] { new Card(Copas, 1) },
								 new[] { new Card(Espadas, 1) },
								 new[] { new Card(Bastos, 1) },
								 1, 1, 1).Returns(new Card(Oros, 1));
			//necesita arrastrar
			yield return
				new TestCaseData(Oros,
								 new[] { new Card(Oros, 7), new Card(Oros, 1) },
								 new[] { new Card(Copas, 1), new Card(Oros, 10) },
								 new[] { new Card(Espadas, 1), new Card(Oros, 11) },
								 new[] { new Card(Bastos, 1), new Card(Oros, 3) },
								 1, 1, 1).Returns(new Card(Oros, 1));

		}

		[Test, TestCaseSource("CanResolvePoints_TestCases")]
		public int[] CanResolvePoints(int id,string trump,
			ICard[] cardsP1, ICard[] cardsP2, ICard[] cardsP3, ICard[] cardsP4,
										   int firstPlayer, int moveNumber)
		{
			Console.WriteLine("Id#:{0}",id);
			_context
				.WithTrump(trump)
				.WithCards(cardsP1, cardsP2, cardsP3, cardsP4)
				.WithFirstPlayer(firstPlayer)
				.WithPlayerBet(firstPlayer);


			var nodeResult = _context.Sut.Execute(_context.Status, firstPlayer);
			
			return new[] {nodeResult.Points1And3,nodeResult.Points2And4};
		}

		private static IEnumerable CanResolvePoints_TestCases()
		{
			//ultimo
			yield return
				new TestCaseData(100,Oros,
								 new[] { new Card(Oros, 1) },
								 new[] { new Card(Copas, 1) },
								 new[] { new Card(Espadas, 1) },
								 new[] { new Card(Bastos, 1) },
								 1, 1).Returns(new[] { 54, 0 });
			//necesita arrastrar
			yield return
				new TestCaseData(200,Oros,
								 new[] { new Card(Oros, 7), new Card(Oros, 1) },
								 new[] { new Card(Copas, 1), new Card(Oros, 10) },
								 new[] { new Card(Espadas, 1), new Card(Oros, 11) },
								 new[] { new Card(Bastos, 1), new Card(Oros, 3) },
								 1, 1).Returns(new[] { 69, 0 });

			//Asegura monte
			yield return
				new TestCaseData(300,Oros,
								 new[] { new Card(Bastos, 3), new Card(Oros, 2) },
								 new[] { new Card(Espadas, 12), new Card(Espadas, 10) },
								 new[] { new Card(Copas, 5), new Card(Copas, 11) },
								 new[] { new Card(Copas, 4), new Card(Oros, 12) },
								 1, 1).Returns(new[] { 25, 8 });
		}
		


		private class TestContext
		{
			private GameExplorer _sut;
			private IFixture _fixture;
			private Status _status;
			
			public TestContext()
			{
				_fixture = new Fixture().Customize(new SubastaAutoFixtureCustomizations());
				_fixture.Register<IValidCardsRule>(()=>new ValidCardsRule());
				
			}

			public TestContext WithTrump(string trump)
			{
				 _fixture.Register<ISuit>(()=>Suit.FromName(trump));
				
				_fixture.Register<ICardComparer>(()=>_fixture.CreateAnonymous<CardComparer>());
				_status = _fixture.Freeze<Status>();
				
				return this;
			}

			public TestContext WithFirstPlayer(int firstPlayer)
			{
				_status.Turn=firstPlayer;

				return this;
			}


			public GameExplorer Sut
			{
				get { return _sut ?? (_sut = _fixture.CreateAnonymous<GameExplorer>()); }
			}

			public Status Status
			{
				get { return _status; }
			}

			public TestContext WithCards(ICard[] cardsP1, ICard[] cardsP2, ICard[] cardsP3, ICard[] cardsP4)
			{
				Status.SetCards(1,cardsP1);
				Status.SetCards(2, cardsP2);
				Status.SetCards(3, cardsP3);
				Status.SetCards(4, cardsP4);
				return this;
			}

			public TestContext WithPlayerBet(int player)
			{
				throw new NotImplementedException();
				//Status.SetPlayerBet(player, TODO);

				return this;
			}
		}
	}
}
