using System.Collections.Generic;
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
        List<IHand> Hands { get; }
        Declaration[] Declarables { get; }
        IHand LastCompletedHand { get; }
        void AddHand();
        ICard[] PlayerCards(int playerPosition);
        void SetCards(int playerPosition, ICard[] cards);

        /// <summary>
        /// Sum by player
        /// </summary>
        /// <param name="playerPosition"></param>
        /// <returns></returns>
        int SumTotal(int playerPosition);

        void SetPlayerBet(int playerPosition);
    }
}