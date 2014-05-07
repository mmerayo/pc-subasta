using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Domain.Game;
using Subasta.Lib.Interaction;
using Subasta.Lib.UserControls;

namespace Subasta.Lib
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
			
			
			
			InitializeCustomUserControls();

		}

		private void InitializeCustomUserControls()
		{
			var userControls = this.FindControls<ICustomUserControl>();
			foreach (var userControl in userControls)
			{
				userControl.Initialize();
			}
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
			
		}

		

		private void UpdateTurn(IPlayer player)
		{
			//if (!status.IsCompleted)
			lblTurn.PerformSafely(x => x.Text = player.Name);
		}

		private void _gameSetHandler_GameSaysStarted(ISaysStatus status)
		{
			this.PerformSafely(x =>
			                   {
							   grpPuntos24.Visible = grpPtos13.Visible = grpTrump.Visible = txtBazas.Visible = grpPlayerBets.Visible = grpPuntos.Visible = false;
							   lblFirstPlayer.Text = _gameSetHandler.GameHandler.GetPlayer(status.FirstPlayer).Name;
			                   });
		
			UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
		}

		private void _gameSetHandler_GameSaysCompleted(ISaysStatus status)
		{
			
			this.PerformSafely(x =>
			                   {
			                   	grpPtos13.Visible = grpPuntos24.Visible = grpTrump.Visible = txtBazas.Visible =  grpPlayerBets.Visible=grpPuntos.Visible= true;
								txtBazas.Text = WriteBaza("J1&3","J2&4");
			                   	lblPuntos13.Text = lblPuntos24.Text = "0";
			                   	lblTrump.Text = _gameSetHandler.GameHandler.Trump.Name;
			                   	lblPlayerBets.Text = _gameSetHandler.GameHandler.GetPlayer(
			                   		_gameSetHandler.GameHandler.Status.PlayerBets).
			                   		Name;
			                   	lblPuntos.Text = _gameSetHandler.GameHandler.Status.NormalizedPointsBet.ToString();

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