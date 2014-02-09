using System;
using System.Collections;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.DomainServices;
using Subasta.Infrastructure.Domain;
using Subasta.Infrastructure.DomainServices;

namespace GamesUnitTests.Subasta.GameGeneration.AI
{
	[TestFixture]
	 class StatusTests
	{
		private const string Oros = "Oros";
		private const string Copas = "Copas";
		private const string Espadas = "Espadas";
		private const string Bastos = "Bastos";

		private TestContext _context;

		[SetUp]
		public void OnSetUp()
		{
			_context = new TestContext();
		}

		[Test, TestCaseSource("CanResolveDeclarables_TestCases")]
		public Declaration[] CanResolveDeclarables(int id, string trump, ICard[] hand, ICard[] cardsP1, ICard[] cardsP2,
		                                           ICard[] cardsP3, ICard[] cardsP4, int playerBet)
		{
			Console.WriteLine("Id: {0}", id);
			//Assert.IsTrue(hand.Length==4);
			//CollectionAssert.AllItemsAreNotNull(hand);

			_context
				.WithTrump(trump)
				.WithCards(cardsP1, cardsP2, cardsP3, cardsP4)
				.WithHands(new[]{hand}) 
				.WithPlayerBet(playerBet);

			return _context.Sut.Declarables;
		}

		private static IEnumerable CanResolveDeclarables_TestCases()
		{
			//No completed hands
			yield return new TestCaseData(10, Oros,
			                              null,
			                              new[] {new Card(Oros, 11)},
			                              new[] {new Card(Copas, 11)},
			                              new[] {new Card(Espadas, 11)},
			                              new[] {new Card(Bastos, 11)},
			                              1).Returns(new Declaration[0]);


			//No possible declarations
			yield return new TestCaseData(20, Oros,
			                              new[] {new Card(Oros, 2), new Card(Oros, 3), new Card(Oros, 4), new Card(Oros, 5)},
			                              new[] {new Card(Oros, 11)},
			                              new[] {new Card(Copas, 11)},
			                              new[] {new Card(Espadas, 11)},
			                              new[] {new Card(Bastos, 11)},
			                              1).Returns(new Declaration[0]);

			//no team player bet= no declarable team 1
			yield return new TestCaseData(30, Bastos,
			                              new[] {new Card(Oros, 2), new Card(Oros, 5), new Card(Oros, 6), new Card(Oros, 4)},
			                              new[] {new Card(Oros, 11), new Card(Oros, 12)},
			                              new[] {new Card(Copas, 11), new Card(Copas, 12)},
			                              new[] {new Card(Espadas, 11), new Card(Espadas, 12)},
			                              new[] {new Card(Bastos, 11), new Card(Bastos, 12)},
			                              1).Returns(new[] {Declaration.ParejaOros, Declaration.ParejaEspadas});
			
			//no team player bet= no declarable team 2
			yield return new TestCaseData(33, Oros,
										  new[] { new Card(Oros, 2), new Card(Oros, 4), new Card(Oros, 5), new Card(Oros, 6) },
										  new[] { new Card(Oros, 11), new Card(Oros, 12) },
										  new[] { new Card(Copas, 11), new Card(Copas, 12) },
										  new[] { new Card(Espadas, 11), new Card(Espadas, 12) },
										  new[] { new Card(Bastos, 11), new Card(Bastos, 12) },
										  1).Returns(new[] { Declaration.ParejaCopas, Declaration.ParejaBastos });

			//no team player bet= no declarable --> cuarenta
			yield return new TestCaseData(36, Oros,
										  new[] { new Card(Oros, 2), new Card(Oros, 4), new Card(Oros, 6), new Card(Oros, 5) },
										  new[] { new Card(Oros, 11), new Card(Oros, 12) },
										  new[] { new Card(Copas, 11), new Card(Copas, 12) },
										  new[] { new Card(Espadas, 11), new Card(Espadas, 12) },
										  new[] { new Card(Bastos, 11), new Card(Bastos, 12) },
										  1).Returns(new[] { Declaration.Cuarenta, Declaration.ParejaEspadas });


			//declared= no declarable--> TODO: ADD CANTES A LA MANO EN EL TEST PARA PODER TESTEAR ESTO
			//yield return new TestCaseData(40, Oros,
			//                              new[]
			//                                  {
			//                                      new Card(Espadas, 1), new Card(Espadas, 2), new Card(Espadas, 3),
			//                                      new Card(Espadas, 4)
			//                                  },
			//                              new[] {new Card(Oros, 11), new Card(Oros, 12)},
			//                              new[] {new Card(Copas, 11), new Card(Copas, 12)},
			//                              new[] {new Card(Espadas, 11), new Card(Espadas, 12)},
			//                              new[] {new Card(Bastos, 11), new Card(Bastos, 12)},
			//                              1).Returns(new[] {Declaration.ParejaOros, Declaration.ParejaEspadas});

			//when kings and more declarations is empty

			//when knights and more declarations is empty

			//when 2 hand completed are empty declarations is empty

			//when current hand is not empty throws exception

			//CurrentHand is empty and is the first throws exception
			//If last completed hand wasnt done but the team bet returns empty 
		}

		private class TestContext
		{
			private ICard[] _p1;
			private ICard[] _p2;
			private ICard[] _p3;
			private ICard[] _p4;
			private readonly IFixture _fixture;
			private Status _sut;
			private int _playerBet;
			private ICard[][] _hands;

			public TestContext()
			{

				_fixture = new Fixture().Customize(new AutoMoqCustomization());
			}

			public TestContext WithTrump(string trump)
			{
				_fixture.Register<ISuit>(() => Suit.FromName(trump));

				return this;
			}

			public TestContext WithCards(ICard[] p1, ICard[] p2, ICard[] p3, ICard[] p4)
			{
				_p1 = p1;
				_p2 = p2;
				_p3 = p3;
				_p4 = p4;

				return this;
			}

			public TestContext WithPlayerBet(int playerBet)
			{
				_playerBet = playerBet;
				return this;
			}

			public TestContext WithHands(ICard[][] hands)
			{
				_hands = hands;
				return this;
			}

			public Status Sut
			{
				get { return _sut ?? (_sut = CreateSut()); }
			}

			private Status CreateSut()
			{
				_fixture.Register<ICardComparer>(() => _fixture.CreateAnonymous<CardComparer>());
                _fixture.Register<IPlayerDeclarationsChecker>(()=>_fixture.CreateAnonymous<PlayerDeclarationsChecker>());

				_sut = _fixture.CreateAnonymous<Status>();
				_sut.SetCards(1, _p1);
				_sut.SetCards(2, _p2);
				_sut.SetCards(3, _p3);
				_sut.SetCards(4, _p4);
				_sut.SetPlayerBet(_playerBet);
				if (_hands != null)
				{
					foreach (var handArray in _hands)
					{
						if (handArray == null)
							continue;

						var hand = _fixture.CreateAnonymous<Hand>();
						for (int i = 1; i <= 4; i++)
							hand.Add(i, handArray[i - 1]);
						_sut.AddHand(hand);
					}
					
				}

				return _sut;
			}
		}
	}
}