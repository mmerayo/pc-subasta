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
			_gameSetHandler.GameHandler.GameSaysStatusChanged += GameHandler_GameSaysStatusChanged;
			_gameSetHandler.GameStarted += _gameSetHandler_GameStarted;
			_gameSetHandler.GameHandler.GameStatusChanged += GameHandler_GameStatusChanged;
		}

		private void GameHandler_GameStatusChanged(IExplorationStatus status)
		{
		if (!status.IsCompleted)
			UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
		}

		private void _gameSetHandler_GameStarted(IExplorationStatus status)
		{
			UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
		}


		private void GameHandler_GameSaysStatusChanged(ISaysStatus status)
		{
			if(!status.IsCompleted)	
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
			txtSays.PerformSafely(x => txtSays.Clear());
			UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
		}

		private void FrmSay_Load(object sender, EventArgs e)
		{
		}
	}
}