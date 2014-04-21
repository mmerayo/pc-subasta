﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Subasta.Client.Common.Game;
using Subasta.Domain.Game;

namespace Subasta
{
	public partial class FrmSay : Form
	{
		private readonly IGameSetHandler _gameSetHandler;
		private readonly FrmGame _frmGame;

		public FrmSay(IGameSetHandler gameSetHandler,FrmGame frmGame)
		{
			_gameSetHandler = gameSetHandler;
			_frmGame = frmGame;
			InitializeComponent();

			_gameSetHandler.GameSaysStarted += new SaysStatusChangedHandler(_gameSetHandler_GameSaysStarted);
			_gameSetHandler.GameHandler.GameSaysStatusChanged += new SaysStatusChangedHandler(GameHandler_GameSaysStatusChanged);
		}

	

		private void GameHandler_GameSaysStatusChanged(Domain.Game.ISaysStatus status)
		{
			UpdateTurn(status);
		}

		private void UpdateTurn(ISaysStatus status)
		{
			this.lblTurn.Text = _gameSetHandler.GameHandler.GetPlayer(status.Turn).Name;
		}

		private void _gameSetHandler_GameSaysStarted(Domain.Game.ISaysStatus status)
		{
			UpdateTurn(status);
		}

		private void FrmSay_VisibleChanged(object sender, EventArgs e)
		{
			Left = _frmGame.Left + _frmGame.Width;
			Top = _frmGame.Top;
		}
	}
}