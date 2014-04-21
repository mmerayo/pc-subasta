﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Subasta.Client.Common;
using Subasta.Client.Common.Game;

namespace Subasta
{
	public partial class FrmMain : Form
	{
		private readonly IGameSetHandler _gameSetHandler;
		private readonly FrmGame _frmGame;
		private readonly FrmSays _frmSays;

		public FrmMain(IGameSetHandler gameSetHandler,FrmGame frmGame,FrmSays frmSays)
		{
			_gameSetHandler = gameSetHandler;
			_frmGame = frmGame;
			_frmSays = frmSays;

			InitializeComponent();
			InitializePositionManagement();

			SubscribeToGameSetEvents();
		}

		private void SubscribeToGameSetEvents()
		{
			_gameSetHandler.GameStarted += new StatusChangedHandler(_gameSet_GameStarted);
			_gameSetHandler.GameCompleted += new StatusChangedHandler(_gameSet_GameCompleted);

			_gameSetHandler.GameSaysCompleted += new SaysStatusChangedHandler(_gameSet_GameSaysCompleted);
			_gameSetHandler.GameSaysStarted += new SaysStatusChangedHandler(_gameSet_GameSaysStarted);

			_gameSetHandler.GameSetCompleted += new GameSetCompletedHandler(_gameSet_GameSetCompleted);
			_gameSetHandler.GameSetStarted += new GameSetStartedHandler(_gameSet_GameSetStarted);

			
		}


		private void _gameSet_GameStarted(Domain.Game.IExplorationStatus status)
		{
			_frmSays.Hide();
			_frmGame.Show();
			_frmGame.BringToFront();
		}

		private void _gameSet_GameSetStarted(IGameSetHandler sender)
		{
			MessageBox.Show(this, "STARTED");
		}

		private void _gameSet_GameSetCompleted(IGameSetHandler sender)
		{
			MessageBox.Show(this, "COMPLETED");
		}

		private void _gameSet_GameSaysStarted(Domain.Game.ISaysStatus status)
		{
			//_frmGame.Hide();
			_frmSays.Show();
			_frmSays.BringToFront();
		}

		private void _gameSet_GameSaysCompleted(Domain.Game.ISaysStatus status)
		{
		}

		private void _gameSet_GameCompleted(Domain.Game.IExplorationStatus status)
		{
		}

		private void InitializePositionManagement()
		{
			this.FormClosing += this.FrmMain_FormClosing;
			this.LocationChanged += this.FrmMain_LocationChanged;
			this.Resize += this.FrmMain_Resize;
			this.StartPosition = FormStartPosition.Manual;
			this.Location = Properties.Settings.Default.FrmMainLocation;
			this.WindowState = Properties.Settings.Default.FrmMainState;
			if (this.WindowState == FormWindowState.Normal)
				this.Size = Properties.Settings.Default.FrmMainSize;
		}

		private void FrmMain_Resize(object sender, EventArgs e)
		{
			Properties.Settings.Default.FrmMainState = this.WindowState;
			if (this.WindowState == FormWindowState.Normal)
				Properties.Settings.Default.FrmMainSize = this.Size;
		}

		private void FrmMain_LocationChanged(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Normal)
				Properties.Settings.Default.FrmMainLocation = this.Location;
		}

		private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Properties.Settings.Default.Save();
		}

		private void nuevoJuegoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				_gameSetHandler.Start();
			}
			catch (Exception ex)
			{
				//TODO: SEND ERROR
			}
		}
	}
}