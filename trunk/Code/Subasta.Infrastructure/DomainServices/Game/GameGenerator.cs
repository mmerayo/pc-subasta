using System;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.DataAccess;
using Subasta.DomainServices.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.Infrastructure.DomainServices.Game
{
	sealed class GameGenerator : IGameGenerator
	{
		private IDeck _deck;
		private readonly IDeckSuffler _suffler;
		private readonly IGameExplorer _gameExplorer;
		private readonly IGameDataAllocator _gameDataAllocator;
		private readonly ICardComparer _cardComparer;
		private readonly IPlayerDeclarationsChecker _playerDeclarationsChecker;

		public GameGenerator(
			IDeck deck,
			IDeckSuffler suffler,
			IGameExplorer gameExplorer,
			IGameDataAllocator gameDataAllocator,
			ICardComparer cardComparer,
			IPlayerDeclarationsChecker playerDeclarationsChecker)
		{
			_deck = deck;
			_suffler = suffler;
			_gameExplorer = gameExplorer;
			_gameDataAllocator = gameDataAllocator;
			_cardComparer = cardComparer;
			_playerDeclarationsChecker = playerDeclarationsChecker;
		}

		public Guid GenerateNewGame()
		{
			Guid gameId = _gameDataAllocator.CreateNewGame();

			try
			{
				DoGeneration();
				_gameDataAllocator.RecordGenerationOutput(gameId,true);
			}
			catch (Exception ex)
			{
				//TODO: log result and exception
				_gameDataAllocator.RecordGenerationOutput(gameId,false);
			}

			return gameId;
		}

		private void DoGeneration()
		{
			var currentStatus = (IExplorationStatus) new Status(_cardComparer, Suit.FromName("Oros"), _playerDeclarationsChecker);
			AssignCards(ref currentStatus);
			_gameExplorer.Execute(currentStatus, 1);
			currentStatus = GetStatusFor(currentStatus, Suit.FromName("Copas"));
			_gameExplorer.Execute(currentStatus, 1);
			currentStatus = GetStatusFor(currentStatus, Suit.FromName("Espadas"));
			_gameExplorer.Execute(currentStatus, 1);
			currentStatus = GetStatusFor(currentStatus, Suit.FromName("Bastos"));
			_gameExplorer.Execute(currentStatus, 1);
		}

		private void AssignCards(ref IExplorationStatus currentStatus)
		{
			_deck=_suffler.Suffle(_deck);
			currentStatus.SetCards(1,_deck.Cards.Cards.GetRange(0,10).ToArray());
			currentStatus.SetCards(2, _deck.Cards.Cards.GetRange(10, 10).ToArray());
			currentStatus.SetCards(3, _deck.Cards.Cards.GetRange(20, 10).ToArray());
			currentStatus.SetCards(4, _deck.Cards.Cards.GetRange(30, 10).ToArray());

		}

		private static IExplorationStatus GetStatusFor(IExplorationStatus currentStatus, ISuit suit)
		{
			currentStatus = currentStatus.Clone();
			currentStatus.SetTrump(suit);
			return currentStatus;
		}
	}
}