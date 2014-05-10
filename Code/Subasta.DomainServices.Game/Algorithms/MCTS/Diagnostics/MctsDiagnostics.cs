using System.IO;
using System.Linq;
using System.Text;
using Subasta.ApplicationServices.IO;
using Subasta.Domain.Game;
using Subasta.Domain.Game.Algorithms;

namespace Subasta.DomainServices.Game.Algorithms.MCTS.Diagnostics
{
	class MctsDiagnostics : IMctsDiagnostics
	{
		

		private readonly IPathHelper _pathHelper;

		public MctsDiagnostics(IPathHelper pathHelper)
		{
			_pathHelper = pathHelper;
		}

		public void NodeStatus(TreeNode current, TreeNode selectedChild, byte playerNum)
		{
#if DEBUG
			string text = GetLogText(current,selectedChild,playerNum);

			string folderName = "DebugReports\\"+selectedChild.ExplorationStatus.GameId.ToString().Replace("-", "_");
			string filePath = _pathHelper.GetApplicationFolderPathForFile(folderName,
			                                                                                  selectedChild.CardPlayed.ToString() + ".data");

			using(var sw = new StreamWriter(filePath))
				sw.Write(text);

#endif
		}

		private string GetLogText(TreeNode parentNode, TreeNode selectedChild, byte playerNum)
		{
			var sb=new StringBuilder();
			sb.AppendLine("Player: " + playerNum);
			sb.AppendLine("".PadRight(50, '-'));

			sb.AppendLine(string.Format("Card played: {0} Declaration played: {1}", selectedChild.CardPlayed, selectedChild.DeclarationPlayed));
			sb.AppendLine("".PadRight(50, '-'));
			
			sb.AppendLine(string.Format("Parent Node Card played: {0} Declaration played: {1}", parentNode.CardPlayed, parentNode.DeclarationPlayed));
			sb.AppendLine("".PadRight(50,'-'));

			WriteNodeLine(parentNode, sb, 1);
			sb.AppendLine("".PadRight(50, '-'));

			WriteNodeLine(parentNode, sb, 2);
			sb.AppendLine("".PadRight(50, '-'));

			return sb.ToString();
		}

		private static void WriteNodeLine(TreeNode parentNode, StringBuilder sb, int teamNumber)
		{
			sb.AppendLine(string.Format("Team {0}:", teamNumber));
			sb.AppendLine(string.Format("{0}{1}{2}{3}{4}{5}", "Card".PadRight(15),"Declaration".PadRight(15), "Coeficient".PadRight(15), "TotalValue".PadRight(15),
			                            "AvgPoints".PadRight(15), "NumberVisits".PadRight(15)));
			//same order applies in selection
			var childInOrder = parentNode.Children.OrderByDescending(x => x.GetNodeInfo(teamNumber).Coeficient)
				//.ThenByDescending(x => x.GetNodeInfo(teamNumber).AvgPoints)
				.ThenBy(x => x.CardPlayed.Value)
				.ThenBy(x => x.CardPlayed.Number);

			foreach (var treeNode in childInOrder)
			{
				ITreeNodeInfo ti = treeNode.GetNodeInfo(teamNumber);
				sb.AppendLine(string.Format("{0}{1}{2}{3}{4}{5}", treeNode.CardPlayed.ToString().PadRight(15),
				                            (treeNode.DeclarationPlayed.HasValue
				                             	? treeNode.DeclarationPlayed.ToString()
				                             	: string.Empty).PadRight(15),
											ti.Coeficient.ToString("0.0000").PadRight(15),
				                            ti.TotalValue.ToString().PadRight(15), ti.AvgPoints.ToString().PadRight(15),
				                            ti.NumberVisits.ToString().PadRight(15)));
			}
		}
	}
}