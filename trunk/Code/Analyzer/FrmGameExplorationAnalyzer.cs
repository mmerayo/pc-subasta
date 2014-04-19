using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Subasta.Domain.Deck;
using Subasta.Domain.Game.Algorithms;

namespace Analyzer
	{
		internal partial class FrmGameExplorationAnalyzer : Form
		{
			private readonly FrmMain _frmMain;

			public FrmGameExplorationAnalyzer(FrmMain frmMain)
			{
				_frmMain = frmMain;
				Owner = _frmMain;
				MdiParent = _frmMain;
				InitializeComponent();
			}

			public void SetSuit(ISuit suit)
			{
				this.Text = "Suit: " + suit.Name;
			}

			public void Display(ITreeNodeInfo treeNodeTeam1, ITreeNodeInfo treeNodeTeam2)
			{
				lblAvgPoints1.Text = "AvgPoints: " + treeNodeTeam1.AvgPoints;
				lblCoeficient1.Text = "Coeficient: " + treeNodeTeam1.Coeficient;
				lblVisits1.Text = "Visits: " + treeNodeTeam1.NumberVisits;
				dgvChances1.DataSource=  GetChancesTable(treeNodeTeam1);

				lblAvgPoints2.Text = "AvgPoints: " + treeNodeTeam2.AvgPoints;
				lblCoeficient2.Text = "Coeficient: " + treeNodeTeam2.Coeficient;
				lblVisits2.Text = "Visits: " + treeNodeTeam2.NumberVisits;
				dgvChances2.DataSource = GetChancesTable(treeNodeTeam2);
			}

			private DataTable GetChancesTable(ITreeNodeInfo source)
			{
				var dataTable = new DataTable();
				dataTable.Columns.Add("Points", typeof (int));
				dataTable.Columns.Add("Percentage", typeof(string));

				for (int i = 0; i <= 25; i++)
				{
					double percent = source.PercentageChancesOfMaking(i);
					dataTable.Rows.Add(i, percent.ToString("P"));
				}

				return dataTable;
			}
		}
	}
