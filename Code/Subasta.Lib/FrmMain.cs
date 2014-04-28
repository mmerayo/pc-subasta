using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Subasta.Client.Common;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;

namespace Subasta
{
	public partial class FrmMain : Form
	{
		private readonly IGameSetHandler _gameSetHandler;
		private readonly FrmGame _frmGame;
		private readonly FrmGameInfo _frmGameInfo;
		private readonly FrmGameSetInfo _frmGameSetInfo;
		private readonly FrmSays _frmSays;

		public FrmMain(IGameSetHandler gameSetHandler, FrmGame frmGame, FrmGameInfo frmGameInfo,FrmGameSetInfo frmGameSetInfo,FrmSays frmSays)
		{
			InitializeComponent();

			_gameSetHandler = gameSetHandler;
			_frmGame = frmGame;
			_frmGameInfo = frmGameInfo;
			_frmGameSetInfo = frmGameSetInfo;
			_frmSays = frmSays;

			_frmGame.MdiParent =_frmGameInfo.MdiParent=_frmGameSetInfo.MdiParent=frmSays.MdiParent= this;
			
			InitializePositionManagement();

			SubscribeToGameSetEvents();



		}

		private void UpdateFormsLocationsAndSizes()
		{
			_frmGameSetInfo.Left =_frmGameSetInfo.Top= 0;

			_frmGame.PerformSafely((x)=>x.Location=new Point(_frmGameSetInfo.Location.X + _frmGameSetInfo.Size.Width, _frmGameSetInfo.Location.Y));

			//_frmSays.Left = _frmGame.Left;
			_frmSays.PerformSafely((x)=>x.Location=new Point(_frmGame.Location.X, _frmGame.Top + _frmGame.Height));

			_frmGameInfo.PerformSafely((x)=>x.Location=new Point( _frmGame.Left + _frmGame.Width, _frmGame.Top));
			_frmGameInfo.PerformSafely(x=>x.Height = _frmGame.Height);

			_frmSays.PerformSafely(x=>x.Width = _frmGame.Width + _frmGameInfo.Width);

			_frmGameSetInfo.PerformSafely(x=>x.Height = _frmGame.Height+_frmSays.Height);


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
			

		}

		private void _gameSet_GameSetStarted(IGameSetHandler sender)
		{
			_frmGameSetInfo.PerformSafely((x) => x.Show());
			_frmGame.PerformSafely((x) => x.Show());
			_frmGameInfo.PerformSafely((x) => x.Show());
			_frmSays.PerformSafely((x) => x.Show());
			UpdateFormsLocationsAndSizes();
			
			_frmGameSetInfo.PerformSafely(x=>x.BringToFront());
			_frmGame.PerformSafely(x=>x.BringToFront());
			_frmGameInfo.PerformSafely(x=>x.BringToFront());
			_frmSays.PerformSafely(x=>x.BringToFront());
		}

		private void _gameSet_GameSetCompleted(IGameSetHandler sender)
		{
			this.PerformSafely(x=>nuevoJuegoToolStripMenuItem.Enabled = true);
		}

		private void _gameSet_GameSaysStarted(Domain.Game.ISaysStatus status)
		{
			
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
				nuevoJuegoToolStripMenuItem.Enabled = false;
				_gameSetHandler.Start();
			}
			catch (Exception ex)
			{
				//TODO: SEND ERROR
			}
		}


	}
}