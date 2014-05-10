using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS.Diagnostics
{
	internal interface IMctsDiagnostics{
		void NodeStatus(TreeNode current, TreeNode selectedChild, byte player);
	}
}