using System;
using System.Linq;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	class MctsTreeCommands : IMctsTreeCommands
	{
		/// <summary>
		/// gets the best found and prunes the passed non needed children
		/// </summary>
		/// <param name="currentStatus"></param>
		/// <returns></returns>
		public NodeResult GetBestFoundAndShallow(IExplorationStatus currentStatus)
		{
			var current = TreeNode.Root(currentStatus.TurnTeam);

			foreach (var hand in currentStatus.Hands)
			{
				foreach (var card in hand.CardsByPlaySequence())
				{
					//prunes those paths that have been passed so they are not used in future navigations
					
					//current.Children.RemoveAll(x =>!Equals(x.CardPlayed, card) || x.DeclarationPlayed != hand.Declaration);
					//current = current.Children.Single();
					current = current.Children.Single(x => Equals(x.CardPlayed, card) && x.DeclarationPlayed == hand.Declaration);
				}
			}
			TreeNode selectBestChild = current.SelectBestChild();
			return new NodeResult(selectBestChild.ExplorationStatus);
		}
	}
}