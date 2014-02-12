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
        int Turn { set; get; }
        IHand CurrentHand { get; }
        ReadOnlyCollection<IHand> Hands { get; }
        Declaration[] Declarables { get; }
        IHand LastCompletedHand { get; }
        void AddNewHand();
        ICard[] PlayerCards(int playerPosition);
        void SetCards(int playerPosition, ICard[] cards);

        /// <summary>
        /// Sum by player
        /// </summary>
        /// <param name="playerPosition"></param>
        /// <returns></returns>
        int SumTotal(int playerPosition);

        void SetPlayerBet(int playerPosition);
    	void AddHand(IHand hand);
	    void SetTrump(ISuit trump);
    }
}