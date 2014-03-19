using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	internal interface IMctsTreeCommands
	{
		/// <summary>
		/// gets the best option found and removes previous uneeded options
		/// </summary>
		/// <param name="currentStatus"></param>
		/// <returns></returns>
		NodeResult GetBestFoundAndShallow(IExplorationStatus currentStatus); 
		
		//TODO:create option clone subtree and make it actual, Ensure the status to a node isheld
	}
}