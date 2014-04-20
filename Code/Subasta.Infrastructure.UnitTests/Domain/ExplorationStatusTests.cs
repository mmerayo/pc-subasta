using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game;
using Subasta.DomainServices.Game.Models;
using Subasta.DomainServices.Game.Strategies;
using Subasta.DomainServices.Game.Utils;
using Subasta.Infrastructure.Domain;
using Subasta.Infrastructure.UnitTests.Tools.Autofixture;

namespace Subasta.Infrastructure.UnitTests.Domain
{
	[TestFixture]
	 class ExplorationStatusTests
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
		public Declaration[] CanResolveDeclarables(int id, string trump, ICard[][] hands, Declaration?[] orderedDeclarations, ICard[] cardsP1, ICard[] cardsP2,
												   ICard[] cardsP3, ICard[] cardsP4, int playerBet)
		{
			Console.WriteLine("Id: {0}", id);
			//Assert.IsTrue(hand.Length==4);
			//CollectionAssert.AllItemsAreNotNull(hand);



			_context
				.WithTrump(trump)
				.WithCards(cardsP1, cardsP2, cardsP3, cardsP4)
				.WithHands(hands,orderedDeclarations) 
				.WithPlayerBet(playerBet);

			return _context.Sut.Declarables;
		}

		private static IEnumerable CanResolveDeclarables_TestCases()
		{
		//Error found:
			// la primera hace baza el otro equipo
			//la segunda hace baza el actual
		yield return new TestCaseData(600, Copas,
			new ICard[][]
			{
				new[] { new Card(Oros, 2), new Card(Oros, 1), new Card(Oros, 5), new Card(Oros, 6) },
				new[] { new Card(Copas, 3), new Card(Copas, 2), new Card(Copas, 5), new Card(Copas, 6) },
				new Card[] {null, null, null, null }
			},
			new Declaration?[] { null,null,null },
			new[] { new Card(Copas, 11), new Card(Copas, 12) },
			new[] { new Card(Oros, 11), new Card(Oros, 12) },
			new[] { new Card(Espadas, 11), new Card(Espadas, 1) },
			new[] { new Card(Bastos, 11), new Card(Bastos, 12) },
			1).Returns(new Declaration[] {Declaration.Cuarenta });

			//No completed hands
			yield return new TestCaseData(10, Oros,
				null,
				null,
				new[] {new Card(Oros, 11)},
				new[] {new Card(Copas, 11)},
				new[] {new Card(Espadas, 11)},
				new[] {new Card(Bastos, 11)},
				1).Returns(new Declaration[0]);


			//No possible declarations
			yield return new TestCaseData(20, Oros,
				new ICard[][]
				{
					new[] {new Card(Oros, 2), new Card(Oros, 3), new Card(Oros, 4), new Card(Oros, 5)},
					new Card[] {null, null, null, null }
				},
				new Declaration?[] {null,null},
				new[] {new Card(Oros, 11)},
				new[] {new Card(Copas, 11)},
				new[] {new Card(Espadas, 11)},
				new[] {new Card(Bastos, 11)},
				1).Returns(new Declaration[0]);

			//team player bet = declarable team 1
			yield return new TestCaseData(30, Bastos,
				new ICard[][]
				{
					new[] {new Card(Oros, 2), new Card(Oros, 5), new Card(Oros, 6), new Card(Oros, 4)}
					,new Card[] {null, null, null, null }
				},
				new Declaration?[] {null,null},
				new[] {new Card(Oros, 11), new Card(Oros, 12)},
				new[] {new Card(Copas, 11), new Card(Copas, 12)},
				new[] {new Card(Espadas, 11), new Card(Espadas, 12)},
				new[] {new Card(Bastos, 11), new Card(Bastos, 12)},
				1).Returns(new[] {Declaration.ParejaOros, Declaration.ParejaEspadas});

			//no team player bet= no declarable team 2
			yield return new TestCaseData(33, Oros,
				new ICard[][]
				{
					new[] {new Card(Oros, 2), new Card(Oros, 4), new Card(Oros, 5), new Card(Oros, 6)},
					new Card[] {null, null, null, null }
				},
				new Declaration?[] {null,null},
				new[] {new Card(Oros, 11), new Card(Oros, 12)},
				new[] {new Card(Copas, 11), new Card(Copas, 12)},
				new[] {new Card(Espadas, 11), new Card(Espadas, 12)},
				new[] {new Card(Bastos, 11), new Card(Bastos, 12)},
				1).Returns(new Declaration[] {});

			//no team player bet= no declarable --> cuarenta
			yield return new TestCaseData(36, Oros,
				new ICard[][] {new[]
				               {
				               	new Card(Oros, 2), new Card(Oros, 4), new Card(Oros, 6), new Card(Oros, 5)
				               },
							   new Card[] {null, null, null, null }},
				new Declaration?[] {null,null},
				new[] {new Card(Oros, 11), new Card(Oros, 12)},
				new[] {new Card(Copas, 11), new Card(Copas, 12)},
				new[] {new Card(Espadas, 11), new Card(Espadas, 12)},
				new[] {new Card(Bastos, 11), new Card(Bastos, 12)},
				1).Returns(new[] {Declaration.Cuarenta, Declaration.ParejaEspadas});


			//declared= no declarable
			yield return new TestCaseData(40, Oros,
				new ICard[][]
				{
					new[]
					{
						new Card(Espadas, 1), new Card(Espadas, 2), new Card(Espadas, 3),new Card(Espadas, 4)
					},
					new[]
					{
						new Card(Espadas, 10), new Card(Espadas, 5), new Card(Espadas, 6),new Card(Espadas, 7)
					},
					new Card[] {null, null, null, null }
				},
				new Declaration?[] {Declaration.Cuarenta,null,null},
				new[] {new Card(Oros, 11), new Card(Oros, 12)},
				new[] {new Card(Copas, 11), new Card(Copas, 12)},
				new[] {new Card(Espadas, 11), new Card(Espadas, 12)},
				new[] {new Card(Bastos, 11), new Card(Bastos, 12)},
				1).Returns(new[] {Declaration.ParejaEspadas});

			

			//when kings and more declarations is empty
			yield return new TestCaseData(45, Oros,
			   new ICard[][]
				{
					new[]
					{
						new Card(Espadas, 1), new Card(Espadas, 2), new Card(Espadas, 3),new Card(Espadas, 4)
					},
					new[]
					{
						new Card(Espadas, 10), new Card(Espadas, 5), new Card(Espadas, 6),new Card(Espadas, 7)
					},
					new Card[] {null, null, null, null }
				},
			   new Declaration?[] { Declaration.Reyes, null,null },
			   new[] { new Card(Copas, 12), new Card(Oros, 12), new Card(Espadas, 12), new Card(Bastos, 12) },
			   new[] { new Card(Copas, 1), new Card(Copas, 2), new Card(Copas, 3), new Card(Copas, 4) },
			   new[] { new Card(Copas, 11), new Card(Oros, 11), new Card(Espadas, 11), new Card(Bastos, 11) },
			   new[] { new Card(Bastos, 1), new Card(Bastos, 2), new Card(Bastos, 3), new Card(Bastos, 4) },
			   1).Returns(new Declaration[] { });

			//when knights and more declarations is empty
			yield return new TestCaseData(50, Oros,
			   new ICard[][]
				{
					new[]
					{
						new Card(Espadas, 1), new Card(Espadas, 2), new Card(Espadas, 3),new Card(Espadas, 4)
					},
					new[]
					{
						new Card(Espadas, 10), new Card(Espadas, 5), new Card(Espadas, 6),new Card(Espadas, 7)
					},
					new Card[] {null, null, null, null }
				},
			   new Declaration?[] { Declaration.Caballos, null,null },
			   new[] { new Card(Copas, 12), new Card(Oros, 12), new Card(Espadas, 12), new Card(Bastos, 12) },
			   new[] { new Card(Copas, 1), new Card(Copas, 2), new Card(Copas, 3), new Card(Copas, 4) },
			   new[] { new Card(Copas, 11), new Card(Oros, 11), new Card(Espadas, 11), new Card(Bastos, 11) },
			   new[] { new Card(Bastos, 1), new Card(Bastos, 2), new Card(Bastos, 3), new Card(Bastos, 4) },
			   1).Returns(new Declaration[] { });

			//when kings and knights
			yield return new TestCaseData(55, Oros,
			   new ICard[][]
				{
					new[]
					{
						new Card(Espadas, 1), new Card(Espadas, 2), new Card(Espadas, 3),new Card(Espadas, 4)
					},
					new Card[] {null, null, null, null }
				},
			   new Declaration?[] { null ,null},
			   new[] { new Card(Copas, 12), new Card(Oros, 12), new Card(Espadas, 12), new Card(Bastos, 12) },
			   new[] { new Card(Copas, 1), new Card(Copas, 2), new Card(Copas, 3), new Card(Copas, 4) },
			   new[] { new Card(Copas, 11), new Card(Oros, 11), new Card(Espadas, 11), new Card(Bastos, 11) },
			   new[] { new Card(Bastos, 1), new Card(Bastos, 2), new Card(Bastos, 3), new Card(Bastos, 4) },
			   1).Returns(new [] {Declaration.Reyes,Declaration.Caballos });

			//when 2 hand completed are empty declarations is empty
			yield return new TestCaseData(60, Oros,
			   new ICard[][]
				{
					new[]
					{
						new Card(Espadas, 1), new Card(Espadas, 2), new Card(Espadas, 3),new Card(Espadas, 4)
					},
					new[]
					{
						new Card(Espadas, 10), new Card(Espadas, 5), new Card(Espadas, 6),new Card(Espadas, 7)
					},
					new Card[] {null, null, null, null }
				},
			   new Declaration?[] { null, null ,null},
			   new[] { new Card(Oros, 11), new Card(Oros, 12) },
			   new[] { new Card(Copas, 11), new Card(Copas, 12) },
			   new[] { new Card(Espadas, 11), new Card(Espadas, 12) },
			   new[] { new Card(Bastos, 11), new Card(Bastos, 12) },
			   1).Returns(new Declaration[] {  });
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
			private  readonly IFixture _fixture = new Fixture().Customize(new SubastaAutoFixtureCustomizations());
			private Status _sut;
			private int _playerBet;
			private ICard[][] _hands;
			private Declaration?[] _orderedDeclarations;

			public TestContext()
			{

			}

			public TestContext WithTrump(string trump)
			{
				Fixture.Register<ISuit>(() => Suit.FromName(trump));

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

			public TestContext WithHands(ICard[][] hands,Declaration?[] orderedDeclarations)
			{
				if (hands!=null && orderedDeclarations.Length != hands.Length) 
					throw new InvalidOperationException("the declarations will be assigned in order being the last hand null");
				_hands = hands;

				_orderedDeclarations = orderedDeclarations;
				return this;
			}

			
			public Status Sut
			{
				get { return _sut ?? (_sut = CreateSut()); }
			}

			public IFixture Fixture
			{
				get { return _fixture; }
			}


			private Status CreateSut()
			{
				Fixture.Register<ICardComparer>(() => Fixture.CreateAnonymous<CardComparer>());
				Fixture.Register<IPlayerDeclarationsChecker>(()=>Fixture.CreateAnonymous<PlayerDeclarationsChecker>());

				_sut = Fixture.CreateAnonymous<Status>();
				_sut.SetCards(1, _p1);
				_sut.SetCards(2, _p2);
				_sut.SetCards(3, _p3);
				_sut.SetCards(4, _p4);
				_sut.SetPlayerBet(_playerBet, 70);
				if (_hands != null)
				{
					for (int index = 0; index < _hands.Length; index++)
					{
						var handArray = _hands[index];
						if (handArray == null)
							continue;

						var hand = CreateHand(handArray,_orderedDeclarations[index]);
						_sut.AddHand(hand);
					}
				}

				return _sut;
			}

			private Hand CreateHand(ICard[] cards, Declaration? declaration)
			{
				var hand = Fixture.CreateAnonymous<Hand>();
			   
				for (int i = 1; i <= 4; i++)
					hand.Add(i, cards[i - 1]);
				
				if(declaration.HasValue)
					hand.SetDeclaration (declaration);
				return hand;
			}
		}
	}
}