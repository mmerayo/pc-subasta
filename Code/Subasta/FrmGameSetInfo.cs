using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Subasta.Domain.Game;

namespace Subasta
{
	public partial class FrmGameSetInfo : Form
	{
		public FrmGameSetInfo()
		{
			InitializeComponent();
		}



		private void GameHandler_GameSaysStatusChanged(ISaysStatus status)
		{//TODO:
			//ISay say = status.Says.Last();
			//tblMarques.Rows.Add(say.PlayerNum, NormalizeSay(say.Figure.Say), status.PointsBet);

			//PaintStatusMarque();

			//Application.DoEvents();

		}

	}
}
