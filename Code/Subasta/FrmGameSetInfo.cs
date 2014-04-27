using System;
using System.Windows.Forms;
using Subasta.Client.Common.Game;
using Subasta.Domain.Game;
using Subasta.Extensions;

namespace Subasta
{
	public partial class FrmGameSetInfo : Form
	{
		private readonly IGameSetHandler _gameSetHandler;

		public FrmGameSetInfo(IGameSetHandler gameSetHandler)
		{
			_gameSetHandler = gameSetHandler;
			InitializeComponent();

			gameSetHandler.GameSaysCompleted += gameSetHandler_GameSaysCompleted;
			gameSetHandler.GameCompleted += gameSetHandler_GameCompleted;
			gameSetHandler.GameSetStarted += gameSetHandler_GameSetStarted;
		}

		private void gameSetHandler_GameSetStarted(IGameSetHandler sender)
		{
			txtGameSetStatus.Text = string.Empty;
			WriteGameLine("Nosotros", " 100 ", "Ellos");
		}

		private void gameSetHandler_GameCompleted(IExplorationStatus status)
		{
			//remove previous line
			txtGameSetStatus.PerformSafely(
				x => x.Text = txtGameSetStatus.Text.Remove(txtGameSetStatus.Text.LastIndexOf(Environment.NewLine)));

			string center = " " + status.NormalizedPointsBet.ToString() + " ";
			string left, right;
			if (status.TeamWinner == 1)
			{
				left = _gameSetHandler.GamePoints(1).ToString();
				right = "---";
			}
			else
			{
				left = "---";
				right = _gameSetHandler.GamePoints(2).ToString();
			}
			if (status.TeamBets == 1)
				left += "*";
			else 
				right = "*" + right;
			WriteGameLine(left, center, right);
		}

		private void gameSetHandler_GameSaysCompleted(ISaysStatus status)
		{
			string infoCenter = status.PointsBet.ToString();
			if (status.TeamBets == 1)
				infoCenter = "*" + infoCenter+ " ";
			else
				infoCenter = " "+infoCenter + "*";
			WriteGameLine("?", infoCenter, "?");
		}

		private void WriteGameLine(string infoT1, string infoCenter, string infoT2)
		{
			txtGameSetStatus.PerformSafely(x=>
			                               {
										   x.Text += Environment.NewLine;
			                               	x.Text += string.Format("{0}|{1}|{2}", infoT1.PadLeft(8, ' '),
			                               	                        infoCenter.PadLeft(5, ' '), infoT2.PadRight(8, ' '));
			                               });
		}
	}
}