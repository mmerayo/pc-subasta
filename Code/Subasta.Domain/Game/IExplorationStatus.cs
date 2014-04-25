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
		bool IsCompleted { get; }
		IHand FirstDeclarableHand { get; }
		int TurnTeam { get; }
		int TeamWinner { get; }
		int TeamBets { get; }
	    int TotalMoves { get; }
		bool LogicalComplete { get; set; }
		int NormalizedPointsBet { get; }
		int LastPlayerMoved { get; }
		ICard LastCardPlayed { get; }
		bool IsEmpty { get; }
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

		/// <summary>
		/// Removes the player card from the status, it was played //TODO:Encapsulate
		/// </summary>
		/// <param name="playerPosition"></param>
		/// <param name="card"></param>
		void RemovePlayerCard(int playerPosition, ICard card);
		bool IsInTeamBets(int playerPosition);
		int PlayerMateOf(int playerWinner);
		ICard[] GetCardsNotYetPlayed();
	}
}