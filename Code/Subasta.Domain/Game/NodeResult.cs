using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{
    public class NodeResult
    {
        private readonly IExplorationStatus _status;

        public int Points1And3 { get; set; }

        public int Points2And4 { get; set; }

        private int GetPoints(int playerNum1, int playerNum2)
        {
            int result = _status.SumTotal(playerNum1) + _status.SumTotal(playerNum2);
            if (_status.CurrentHand.PlayerWinner == playerNum1 || _status.CurrentHand.PlayerWinner == playerNum2)
                result += 10;
            return result;
        }


        public ICard CardAtMove(int playerPosition, int moveNumber)
        {
            return _status.Hands[moveNumber - 1].PlayerCard(playerPosition);
        }

        public NodeResult(IExplorationStatus status)
        {
            _status = status;
            Points1And3 = GetPoints(1, 3);
            Points2And4 = GetPoints(2, 4);
        }

        public int this[int playerPosition]
        {
            get { return playerPosition == 1 || playerPosition == 3 ? Points1And3 : Points2And4; }
        }
    }
}