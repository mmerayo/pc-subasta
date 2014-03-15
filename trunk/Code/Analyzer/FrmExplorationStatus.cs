using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Subasta.Client.Common;

namespace Analyzer
{
	partial class FrmExplorationStatus : Form
	{
		private readonly IGameSimulator _gameSimulator;

		public FrmExplorationStatus(IGameSimulator gameSimulator)
		{

			InitializeComponent();

			_gameSimulator = gameSimulator;
			_gameSimulator.GameStatusChanged += new StatusChangedHandler(_gameSimulator_GameStatusChanged);
		}

		private void _gameSimulator_GameStatusChanged(Subasta.Domain.Game.IExplorationStatus status)
		{
			throw new NotImplementedException();
		}
	}
}
