using System;
using System.Threading;
using System.Threading.Tasks;
using Subasta.ApplicationServices.Extensions;
using Subasta.Domain.Deck;
using Subasta.DomainServices.DataAccess;
using Subasta.DomainServices.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.Infrastructure.DomainServices.Game
{
	internal sealed class GameGenerator : IGameGenerator
	{
		private IDeck _deck;
		private readonly IDeckSuffler _suffler;
		private readonly IGameExplorer _gameExplorer;
		private readonly IGameDataWritter _gameDataAllocator;

		public GameGenerator(
			IDeck deck,
			IDeckSuffler suffler,
			IGameExplorer gameExplorer,
			IGameDataWritter gameDataAllocator)
		{
			_deck = deck;
			_suffler = suffler;
			_gameExplorer = gameExplorer;
			_gameDataAllocator = gameDataAllocator;
		}

		public bool TryGenerateNewGame(out Guid gameId)
		{
			gameId = Guid.Empty;
			try
			{
				gameId = _gameDataAllocator.CreateNewGameStorage();
				DoGeneration(gameId);
				_gameDataAllocator.RecordGenerationOutput(gameId, true);
				return true;
			}
			catch (Exception ex)
			{
				//TODO: log result and exception
				if (gameId != Guid.Empty)
					_gameDataAllocator.RecordGenerationOutput(gameId, false);
				return false;
			}


		}

		private void DoGeneration(Guid gameId)
		{
			_deck = _suffler.Suffle(_deck);

			var p1 = _deck.Cards.Cards.GetRange(0, 10).ToArray();
			var p2 = _deck.Cards.Cards.GetRange(10, 10).ToArray();
			var p3 = _deck.Cards.Cards.GetRange(20, 10).ToArray();
			var p4 = _deck.Cards.Cards.GetRange(30, 10).ToArray();

			foreach (var suit in Suit.Suits)
			{
				//TODO: LOGGER
				Console.WriteLine("Explore team 1 Suit:{0}",suit.Name);
				Task.Factory.StartNew(() => _gameExplorer.Execute(gameId, 1, 1, p1, p2, p3, p4, suit)).LogTaskException();

				Console.WriteLine("Explore team 2 Suit:{0}", suit.Name);
				Task.Factory.StartNew(() =>_gameExplorer.Execute(gameId, 1, 2, p1, p2, p3, p4, suit)).LogTaskException();
			}
		}

	}
}