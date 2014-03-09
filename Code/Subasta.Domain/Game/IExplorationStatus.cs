using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{
	public interface IExplorationStatus
	{
		IExplorationStatus Clone();
		ISuit Trump { get; }
		int PlayerBets { get; }
		int Turn { get; set; }
		IHand CurrentHand { get; }
		ReadOnlyCollection<IHand> Hands { get; }
		Declaration[] Declarables { get; }
		IHand LastCompletedHand { get; }
		Guid GameId { get; }
		int PointsBet { get; }
		bool GameCompleted { get; }
		void AddNewHand();
		ICard[] PlayerCards(int playerPosition);
		void SetCards(int playerPosition, ICard[] cards);

		/// <summary>
		/// Sum by player
		/// </summary>
		/// <param name="playerPosition"></param>
		/// <returns></returns>
		int SumTotal(int playerPosition);

		void SetPlayerBet(int playerPosition, int pointsBet);
		void AddHand(IHand hand);
		void SetTrump(ISuit trump);
		int SumTotalTeam(int playerPosition);
		void RemovePlayerCard(int playerPosition, ICard card);
		bool IsInTeamBets(int playerPosition);
	}
}