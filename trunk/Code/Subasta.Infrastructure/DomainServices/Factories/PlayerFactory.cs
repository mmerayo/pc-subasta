using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Factories;

namespace Subasta.Infrastructure.DomainServices.Factories
{
	class PlayerFactory:IPlayerFactory
	{
		public IPlayer CreatePlayer(int playerNumber, StoredGameData source)
		{
			IPlayer result;

			switch (playerNumber)
			{
				case 1:
					result = CreatePlayer(source.Player1Type, source.Player1Cards, "Player 1");
					result.TeamNumber = 1;
					break;
				case 2:
					result = CreatePlayer(source.Player2Type, source.Player2Cards, "Player 2");
					result.TeamNumber = 2;
					break;
				case 3:
					result = CreatePlayer(source.Player3Type, source.Player3Cards, "Player 3");
					result.TeamNumber = 1;
					break;
				case 4:
					result = CreatePlayer(source.Player4Type, source.Player4Cards, "Player 4");
					result.TeamNumber = 2;
					break;
				default:
					throw new ArgumentOutOfRangeException("playerNumber");
			}
			result.PlayerNumber = playerNumber;
			return result;
		}

		private IPlayer CreatePlayer(PlayerType playerType, ICard[] playerCards, string playerName)
		{
			IPlayer result;
			switch (playerType)
			{
				case PlayerType.Human:
					result = ObjectFactory.GetInstance<IHumanPlayer>();
					break;
				case PlayerType.Mcts:
					result = ObjectFactory.GetInstance<IMctsPlayer>();
					break;
				default:
					throw new ArgumentOutOfRangeException("playerType");
			}
			result.Name = playerName;

			result.Cards = playerCards.OrderBy(x => x.Suit.Value).ThenByDescending(x => x.Value).ThenByDescending(x => x.Number).ToArray();
			return result;
		}
	}
}
