using Subasta.Domain.DalModels;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game.Algorithms.MaxN;

namespace Subasta.DomainServices.Game.Players
{
	internal class MaxNPlayer : AIPlayer, IMaxNPlayer
	{
		public MaxNPlayer(IMaxNSimulator simulator, ISaysSimulator saysSimulator)
			: base(simulator,saysSimulator)
		{

		}

		public override PlayerType PlayerType
		{
			get { return PlayerType.MaxN; }
		}

	}
}