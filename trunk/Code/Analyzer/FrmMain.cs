using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StructureMap;
using Subasta.Client.Common;
using Subasta.Client.Common.Games;
using Subasta.Domain.Deck;
using Subasta.Infrastructure.Domain;

namespace Analyzer
{
	internal partial class FrmMain : Form
	{
		private readonly IStoredGamesCommands _storedGamesCommands;
		public IGameSimulator CurrentSimulation { get; private set; }

		public FrmMain(IGameSimulator gameSimulator,IStoredGamesCommands storedGamesCommands)
		{
			_storedGamesCommands = storedGamesCommands;
			InitializeComponent();
			CurrentSimulation = gameSimulator;
		}

		private void NewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openGameFile.ShowDialog() == DialogResult.OK)
			{
				try
				{
					_storedGamesCommands.RestoreSimulation(openGameFile.FileName);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
			}
		}

		private void startToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

	}
}
