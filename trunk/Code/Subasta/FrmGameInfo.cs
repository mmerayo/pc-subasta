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
		private DataTable _tblSays;
		public FrmGameInfo(IGameSetHandler gameSetHandler)
		{
			_gameSetHandler = gameSetHandler;
			InitializeComponent();

			InitializeSaysData();

			_gameSetHandler.GameSaysStarted += new SaysStatusChangedHandler(_gameSetHandler_GameSaysStarted);
			_gameSetHandler.GameHandler.GameSaysStatusChanged += new SaysStatusChangedHandler(GameHandler_GameSaysStatusChanged);
		}

		private void InitializeSaysData()
		{
			_tblSays = new DataTable();
			_tblSays.Columns.Add("Jug");
			_tblSays.Columns.Add("Marque");

		}


		private void GameHandler_GameSaysStatusChanged(Domain.Game.ISaysStatus status)
		{
			UpdateTurn(status);

			UpdateMarques(status);

		}

		private void UpdateMarques(ISaysStatus status)
		{
			_tblSays.Rows.Add(status.LastSayPlayer, status.Says.Last().Figure.Say.ToString().SeparateCamelCase());
			dgvSays.PerformSafely(x=>dgvSays.DataSource = _tblSays);
			Application.DoEvents();
		}

		private void UpdateTurn(ISaysStatus status)
		{
			if (!status.IsCompleted)
				lblTurn.PerformSafely(x => x.Text = _gameSetHandler.GameHandler.GetPlayer(status.Turn).Name);
		}

		private void _gameSetHandler_GameSaysStarted(Domain.Game.ISaysStatus status)
		{
			_tblSays.Rows.Clear();
			UpdateTurn(status);
		}

		private void FrmSay_Load(object sender, EventArgs e)
		{

		}
	}
}
