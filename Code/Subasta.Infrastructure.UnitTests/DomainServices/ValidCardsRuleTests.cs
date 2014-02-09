using System.Collections;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices;
using Subasta.Infrastructure.Domain;
using Subasta.Infrastructure.DomainServices;

namespace Subasta.Infrastructure.UnitTests.DomainServices
{
	[TestFixture]
	class ValidCardsRuleTests
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

		[Test,TestCaseSource("CanGetValidMoves_TestCases")]
		public ICard[] CanGetValidMoves(string trumpName, ICard[] currentHandCards, ICard[] playerCards)
		{
			_context.WithTrump(trumpName).WithCurrentHandCards(currentHandCards).WithPlayerCards(playerCards);

			return _context.Result();

		}

		public static IEnumerable CanGetValidMoves_TestCases()
		{
			//primero
			yield return
			    new TestCaseData(Oros, new Card[0], new[] { new Card(Copas, 1), new Card(Oros, 2) }).Returns(new[] { new Card(Copas, 1), new Card(Oros, 2) });

			//tiene para asistir y fallo
			yield return
			    new TestCaseData(Oros, new[] { new Card(Copas, 7) }, new[] { new Card(Copas, 6), new Card(Oros, 2) }).Returns(new[] { new Card(Copas, 6) });

			//tiene para asistir y fallo y subir
			yield return
			    new TestCaseData(Oros, new[] { new Card(Copas, 6) }, new[] { new Card(Copas, 4), new Card(Copas, 7), new Card(Oros, 2) })
			        .Returns(new[] { new Card(Copas, 7) });

			//Debe fallar
			yield return
				new TestCaseData(Oros, new[] { new Card(Copas, 6) },
								 new[] { new Card(Espadas, 4), new Card(Espadas, 7), new Card(Oros, 2) }).Returns(new[] { new Card(Oros, 2) });

			//DeArrastre
			yield return
			    new TestCaseData(Oros, new[] { new Card(Oros, 6) },
			                     new[] { new Card(Espadas, 4), new Card(Espadas, 7), new Card(Oros, 2) }).Returns(new[] { new Card(Oros, 2) });

			//DeArrastre y no tiene
			yield return
			    new TestCaseData(Oros, new[] { new Card(Oros, 6) }, new[] { new Card(Espadas, 4), new Card(Bastos, 2) }).Returns(new[] { new Card(Espadas, 4), new Card(Bastos, 2) });

			//Fallada y puede subir
			yield return
			    new TestCaseData(Oros, new[] { new Card(Bastos, 6), new Card(Oros, 2) },
			                     new[] { new Card(Espadas, 4), new Card(Oros, 4) }).Returns(new[] { new Card(Oros, 4) });

			//achique
			yield return
			    new TestCaseData(Oros, new[] { new Card(Bastos, 6), new Card(Oros, 4) },
			                     new[] { new Card(Espadas, 4), new Card(Oros, 2) }).Returns(new[] { new Card(Espadas, 4), new Card(Oros, 2) });
		}

		private class TestContext
		{
			private readonly Fixture _fixture;
			private ValidCardsRule _sut;
			private ISuit _trump;
			private ICard[] _playerCards;
			private IHand _hand;

			public TestContext()
			{
				_fixture=new Fixture();
				
			}

			private ValidCardsRule Sut
			{
				get { return _sut??(_sut=_fixture.CreateAnonymous<ValidCardsRule>()); }
			}

			public TestContext WithTrump(string trumpName)
			{
				_trump = Suit.FromName(trumpName);
				_fixture.Register<ISuit>(() => _trump);
				_fixture.Register<ICardComparer>(() => _fixture.Freeze<CardComparer>());
				_hand = _fixture.Freeze<Hand>();
				return this;
			}

			public TestContext WithPlayerCards(ICard[] playerCards)
			{
				_playerCards = playerCards;
				return this;
			}

			public TestContext WithCurrentHandCards(ICard[] currentHandCards)
			{
				var playerPlays = 1;
				foreach (var existingCard in currentHandCards)
				{
					_hand.Add(playerPlays, existingCard);
					SetNextPlayer(ref playerPlays);
				}

				return this;
			}

			private void SetNextPlayer(ref int playerPlays)
			{
				if (++playerPlays > 4)
					playerPlays = 1;
			}

			public ICard[] Result()
			{
				return Sut.GetValidMoves(_playerCards, _hand);
			}
		}
	}
}
