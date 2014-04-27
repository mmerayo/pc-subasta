using System;
using System.Linq;
using System.Windows.Forms;
using Subasta.Client.Common.Game;
using Subasta.Domain.Game;
using Subasta.Extensions;

namespace Subasta
{
	public partial class FrmGameInfo : Form
	{
		private readonly IGameSetHandler _gameSetHandler;

		public FrmGameInfo(IGameSetHandler gameSetHandler)
		{
			_gameSetHandler = gameSetHandler;
			InitializeComponent();

			_gameSetHandler.GameSaysStarted += _gameSetHandler_GameSaysStarted;
			_gameSetHandler.GameSaysCompleted += _gameSetHandler_GameSaysCompleted;
			_gameSetHandler.GameHandler.GameSaysStatusChanged += GameHandler_GameSaysStatusChanged;
			_gameSetHandler.GameStarted += _gameSetHandler_GameStarted;
			_gameSetHandler.GameHandler.GameStatusChanged += GameHandler_GameStatusChanged;
			_gameSetHandler.GameHandler.HandCompleted += GameHandler_HandCompleted;
		}

		void GameHandler_HandCompleted(IExplorationStatus status)
		{
			string t1="---", t2="---";
			if(status.LastCompletedHand.Declaration.HasValue)
			{
				var target = status.LastCompletedHand.Declaration.Value.ToString().SeparateCamelCase();
				if(status.LastCompletedHand.TeamWinner==1)
					t1 = target+"*";
				else
					t2 = "*"+target;
			}

			else
			{
			if (status.LastCompletedHand.TeamWinner == 1)
				t1 = "*gana*";
			else
				t2 = "*gana*";
			}

			txtBazas.PerformSafely(x => x.Text += WriteBaza(t1,t2) );
		}



		private void GameHandler_GameStatusChanged(IExplorationStatus status)
		{
			if (!status.IsCompleted)
				UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
			lblPuntos13.PerformSafely(x => x.Text = status.SumTotalTeam(1).ToString());
			lblPuntos24.PerformSafely(x => x.Text = status.SumTotalTeam(2).ToString());
		}

		private void _gameSetHandler_GameStarted(IExplorationStatus status)
		{
			UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
		}


		private void GameHandler_GameSaysStatusChanged(ISaysStatus status)
		{
			if (!status.IsCompleted)
				UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
			UpdateMarques(status);
		}

		private void UpdateMarques(ISaysStatus status)
		{
			ISay last = status.Says.Last();
			txtSays.PerformSafely(
				x =>
				x.Text +=
				string.Format("Jugador {0}:{1}{2}", last.PlayerNum, last.Figure.ToString().SeparateCamelCase(), Environment.NewLine));
		}

		private void UpdateTurn(IPlayer player)
		{
			//if (!status.IsCompleted)
			lblTurn.PerformSafely(x => x.Text = player.Name);
		}

		private void _gameSetHandler_GameSaysStarted(ISaysStatus status)
		{
			this.PerformSafely(x => grpPuntos24.Visible = grpPtos13.Visible = grpTrump.Visible = txtBazas.Visible = false);
			txtSays.PerformSafely(x =>
			                      {
			                      	x.Visible = true;
			                      	x.Clear();
			                      });
			UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
		}

		private void _gameSetHandler_GameSaysCompleted(ISaysStatus status)
		{
			txtSays.PerformSafely(x =>
			                      {
			                      	x.Visible = false;
			                      	x.Clear();
			                      });
			this.PerformSafely(x =>
			                   {
			                   	grpPtos13.Visible = grpPuntos24.Visible = grpTrump.Visible = txtBazas.Visible = true;
								txtBazas.Text = WriteBaza("J1&3","J2&4");
			                   	lblPuntos13.Text = lblPuntos24.Text = "0";
			                   	lblTrump.Text = string.Format("{1} pone {2} a {0}", _gameSetHandler.GameHandler.Trump.Name,
			                   	                              _gameSetHandler.GameHandler.GetPlayer(
			                   	                              	_gameSetHandler.GameHandler.Status.PlayerBets).
			                   	                              	Name, _gameSetHandler.GameHandler.Status.NormalizedPointsBet);
			                   	lblTrump.Visible = true;

			                   });
		}

		private static string WriteBaza(string infoT1, string infoT2)
		{
			return string.Format("{0} - {1}{2}", infoT1.PadLeft(10), infoT2.PadRight(10),Environment.NewLine);
		}

		private void FrmSay_Load(object sender, EventArgs e)
		{
		}
	}
}