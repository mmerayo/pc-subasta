using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

			_gameSetHandler.GameSaysStarted += new SaysStatusChangedHandler(_gameSetHandler_GameSaysStarted);
			_gameSetHandler.GameHandler.GameSaysStatusChanged += new SaysStatusChangedHandler(GameHandler_GameSaysStatusChanged);
		}

	


		private void GameHandler_GameSaysStatusChanged(Domain.Game.ISaysStatus status)
		{
			UpdateTurn(status);

			UpdateMarques(status);

		}

		private void UpdateMarques(ISaysStatus status)
		{
			ISay last = status.Says.Last();
			txtSays.PerformSafely(x=>x.Text += string.Format("Jugador {0}:{1}{2}", last.PlayerNum, last.Figure.ToString().SeparateCamelCase(),Environment.NewLine));
		}

		private void UpdateTurn(ISaysStatus status)
		{
			if (!status.IsCompleted)
				lblTurn.PerformSafely(x => x.Text = _gameSetHandler.GameHandler.GetPlayer(status.Turn).Name);
		}

		private void _gameSetHandler_GameSaysStarted(Domain.Game.ISaysStatus status)
		{
			txtSays.PerformSafely(x=>txtSays.Clear());
			UpdateTurn(status);
		}

		private void FrmSay_Load(object sender, EventArgs e)
		{

		}
	}
}
