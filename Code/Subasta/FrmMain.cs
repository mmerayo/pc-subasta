using System;
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
		private readonly IGameSet _gameSet;

		public FrmMain(IGameSet gameSet)
		{
			InitializeComponent();
			InitializePositionManagement();

			_gameSet = gameSet;
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
				
				_gameSet.Start();
			}
			catch (Exception ex)
			{
				//TODO: SEND ERROR
			}
		}
	}
}
