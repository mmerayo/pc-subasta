using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Subasta.Client.Common.Game;

namespace Subasta
{
	public partial class FrmGame : Form
	{
		private readonly IGameSetHandler _gameSetHandler;

		public FrmGame(IGameSetHandler gameSetHandler)
		{
			_gameSetHandler = gameSetHandler;
			InitializeComponent();
		}
	}
}
