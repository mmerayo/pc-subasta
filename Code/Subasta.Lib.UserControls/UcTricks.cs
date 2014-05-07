using System;
using System.Windows.Forms;
using StructureMap;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Domain.Game;

namespace Subasta.Lib.UserControls
{
	public partial class UcTricks : UserControl, ICustomUserControl
	{
		private IGameSetHandler _gameSetHandler;

		public UcTricks()
		{
			InitializeComponent();
		}

		public void Initialize()
		{
			_gameSetHandler = ObjectFactory.GetInstance<IGameSetHandler>();
			IGameHandler gameHandler = _gameSetHandler.GameHandler;
			gameHandler.HandCompleted += GameHandler_HandCompleted;
			gameHandler.GameSaysStarted += gameHandler_GameSaysStarted;
			gameHandler.GameStarted += gameHandler_GameStarted;
			_gameSetHandler.GameHandler.GameStatusChanged += GameHandler_GameStatusChanged;
		}
		private void GameHandler_GameStatusChanged(IExplorationStatus status)
			{
			
			lblPuntos13.PerformSafely(x => x.Text = status.SumTotalTeam(1).ToString());
			lblPuntos24.PerformSafely(x => x.Text = status.SumTotalTeam(2).ToString());
			}

		private void gameHandler_GameStarted(IExplorationStatus status)
		{
			this.PerformSafely(x =>
			{
				x.Visible = true;
				txtBazas.Text = WriteBaza("J1&3", "J2&4");
			});
		}

		private void gameHandler_GameSaysStarted(ISaysStatus status)
		{
			this.PerformSafely(x =>
			                   {
			                   	x.Visible = false;
								lblPuntos13.Text = lblPuntos24.Text = "0";
			                   });
		}

		private void GameHandler_HandCompleted(IExplorationStatus status)
		{
			PaintTrick(status);
		}

		private void PaintTrick(IExplorationStatus status)
		{
			string t1 = "---", t2 = "---";
			if (status.LastCompletedHand.Declaration.HasValue)
			{
				string target = status.LastCompletedHand.Declaration.Value.ToString().SeparateCamelCase();
				if (status.LastCompletedHand.TeamWinner == 1)
					t1 = target + "*";
				else
					t2 = "*" + target;
			}

			else
			{
				if (status.LastCompletedHand.TeamWinner == 1)
					t1 = "*gana*";
				else
					t2 = "*gana*";
			}

			txtBazas.PerformSafely(x => x.Text += WriteBaza(t1, t2));
		}


		private static string WriteBaza(string infoT1, string infoT2)
		{
			return string.Format("{0} - {1}{2}", infoT1.PadLeft(10), infoT2.PadRight(10), Environment.NewLine);
		}

	}
}