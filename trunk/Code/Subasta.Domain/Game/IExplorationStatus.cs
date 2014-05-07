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
		byte PlayerBets { get; }
		byte Turn { get; set; }
		IHand CurrentHand { get; }
		ReadOnlyCollection<IHand> Hands { get; }
		Declaration[] Declarables { get; }
		IHand LastCompletedHand { get; }
		Guid GameId { get; }
		byte PointsBet { get; }
		bool IsCompleted { get; }
		IHand FirstDeclarableHand { get; }
		byte TurnTeam { get; }
		byte TeamWinner { get; }
		byte TeamBets { get; }
		byte TotalMoves { get; }
		bool LogicalComplete { get; set; }
		byte NormalizedPointsBet { get; }
		byte LastPlayerMoved { get; }
		ICard LastCardPlayed { get; }
		bool IsEmpty { get; }
		void AddNewHand();
		ICard[] PlayerCards(byte playerPosition);
		void SetCards(byte playerPosition, ICard[] cards);

		/// <summary>
		/// Sum by player
		/// </summary>
		/// <param name="playerPosition"></param>
		/// <returns></returns>
		byte SumTotal(byte playerPosition);

		void SetPlayerBet(byte playerPosition, byte pointsBet);
		void AddHand(IHand hand);
		void SetTrump(ISuit trump);
		byte SumTotalTeam(byte teamNumber);

		/// <summary>
		/// Removes the player card from the status, it was played //TODO:Encapsulate
		/// </summary>
		/// <param name="playerPosition"></param>
		/// <param name="card"></param>
		void RemovePlayerCard(byte playerPosition, ICard card);
		bool IsInTeamBets(byte playerPosition);
		byte PlayerMateOf(byte playerWinner);
		ICard[] GetCardsNotYetPlayed();
		IEnumerable<Declaration> GetPlayerDeclarables(byte playerNumber);
	}
}