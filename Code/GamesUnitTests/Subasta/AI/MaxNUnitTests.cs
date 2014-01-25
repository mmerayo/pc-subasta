using System;
using System.Collections;
using System.Collections.Generic;
using Games.Deck;
using Games.Subasta;
using Games.Subasta.AI;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Suit = Games.Subasta.Suit;
using Card = Games.Subasta.Card;
namespace GamesUnitTests.Subasta.AI
{
	[TestFixture]
	public class MaxNUnitTests
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
				.WithFirstPlayer(firstPlayer);
				

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
		public int[] CanResolvePoints(string trump,
			ICard[] cardsP1, ICard[] cardsP2, ICard[] cardsP3, ICard[] cardsP4,
										   int firstPlayer, int playerEvaluated, int moveNumber)
		{
			Console.WriteLine("Hands#:{0}",cardsP1.Length);
			_context
				.WithTrump(trump)
				.WithCards(cardsP1, cardsP2, cardsP3, cardsP4)
				.WithFirstPlayer(firstPlayer);


			var nodeResult = _context.Sut.Execute(_context.Status, firstPlayer);
			
			return new[] {nodeResult.Points1And3,nodeResult.Points2And4};
		}

		private static IEnumerable CanResolvePoints_TestCases()
		{
			//ultimo
			yield return
				new TestCaseData(Oros,
								 new[] { new Card(Oros, 1) },
								 new[] { new Card(Copas, 1) },
								 new[] { new Card(Espadas, 1) },
								 new[] { new Card(Bastos, 1) },
								 1, 1, 1).Returns(new []{54,0});
			//necesita arrastrar
			yield return
				new TestCaseData(Oros,
								 new[] { new Card(Oros, 7), new Card(Oros, 1) },
								 new[] { new Card(Copas, 1), new Card(Oros, 10) },
								 new[] { new Card(Espadas, 1), new Card(Oros, 11) },
								 new[] { new Card(Bastos, 1), new Card(Oros, 3) },
								 1, 1, 1).Returns(new[]{69,0});

		}
		


		private class TestContext
		{
			private MaxN _sut;
			private IFixture _fixture;
			private Status _status;
			
			public TestContext()
			{
				_fixture = new Fixture().Customize(new AutoMoqCustomization());
				_fixture.Register<IValidCardsRule>(()=>new ValidCardsRule());
				
			}

			public TestContext WithTrump(string trump)
			{
				_fixture.Register<ICardComparer>(() => new CardComparer(Suit.FromName(trump)));
				_status = _fixture.Freeze<Status>();
				return this;
			}

			public TestContext WithFirstPlayer(int firstPlayer)
			{
				_status.Turn=firstPlayer;

				return this;
			}


			public MaxN Sut
			{
				get { return _sut ?? (_sut = _fixture.CreateAnonymous<MaxN>()); }
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
		}
	}
}
