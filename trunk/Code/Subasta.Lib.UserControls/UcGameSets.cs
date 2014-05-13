using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using StructureMap;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Domain.Game;

namespace Subasta.Lib.UserControls
{
	public partial class UcGameSets : UserControl, ICustomUserControl
	{
		private IGameSetHandler _gameSetHandler;

		public UcGameSets()
		{
			InitializeComponent();

		}

		public void Initialize()
		{
			_gameSetHandler = ObjectFactory.GetInstance<IGameSetHandler>();

			if (_gameSetHandler != null)
			{
				_gameSetHandler.GameSetStarted += gameSetHandler_GameSetStarted;
				_gameSetHandler.GameSaysCompleted += gameSetHandler_GameSaysCompleted;
				_gameSetHandler.GameCompleted += gameSetHandler_GameCompleted;
			}
		}

		private void gameSetHandler_GameSetStarted(IGameSetHandler sender)
		{
			CreateSetTab();

			WriteGameLine("Nosotros", " 100 ", "Ellos");
		}

		private void CreateSetTab()
		{

			this.tabs.SuspendLayout();
			var sets = _gameSetHandler.Sets;

			//create the tab
			var newTab = new TabPage
			{
				Location = new System.Drawing.Point(4, 22),
				Name = "tabGameSet" + sets.Count,

				Padding = new System.Windows.Forms.Padding(3),
				Size = new System.Drawing.Size(198, 369),
				TabIndex = sets.Count - 1,
				Text = "Juego" + sets.Count,
				UseVisualStyleBackColor = true
			};
			tabs.PerformSafely(x =>
			{
				x.Controls.Add(newTab);
				x.SelectedIndex = x.TabCount - 1;
			});
			CreateSetTabControls(newTab);


			tabs.ResumeLayout(false);
		}



		private void CreateSetTabControls(TabPage newTab)
		{
			var newTxt = new TextBox
			{
				Font =
					new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular,
											System.Drawing.GraphicsUnit.Point, ((byte)(0))),
				Location = new System.Drawing.Point(7, 3),
				Multiline = true,
				Name = "txtGameSetStatus" + newTab.TabIndex,
				ReadOnly = true,
				ScrollBars = System.Windows.Forms.ScrollBars.Vertical,
				Size = new System.Drawing.Size(170, 215),
			};
			newTab.PerformSafely(x => x.Controls.Add(newTxt));
		}

		private void WriteGameLine(string infoT1, string infoCenter, string infoT2)
		{
			TextBox txtBox = GetCurrentTextBoxTarget();
			txtBox.PerformSafely(x =>
			{
				x.Text += string.Format("{0}|{1}|{2}", infoT1.PadLeft(8, ' '),
										infoCenter.PadLeft(5, ' '), infoT2.PadRight(5, ' '));
				x.Text += Environment.NewLine;
			});
		}

		private void gameSetHandler_GameCompleted(IExplorationStatus status)
		{
			//remove previous line
			TextBox txtBox = GetCurrentTextBoxTarget();
			txtBox.PerformSafely(
				x =>
				{
					string[] lines = txtBox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

					IEnumerable<string> enumerable = lines.Take(lines.Count()-1);
					string txt = string.Empty;
					foreach (var line in enumerable)
					{
						txt += line + Environment.NewLine;
					}


					x.Text = txt;
				});

			string center = " " + status.NormalizedPointsBet.ToString() + " ";
			string left, right;
			if (status.TeamWinner == 1)
			{
				left = _gameSetHandler.GamePoints(1).ToString();
				right = "---";
			}
			else
			{
				left = "---";
				right = _gameSetHandler.GamePoints(2).ToString();
			}
			if (status.TeamBets == 1)
				center = "*" + center;
			else
				center += "*";
			WriteGameLine(left, center, right);
		}

		private void gameSetHandler_GameSaysCompleted(ISaysStatus status)
		{
			string infoCenter = status.PointsBet.ToString();
			if (status.TeamBets == 1)
				infoCenter = "*" + infoCenter + " ";
			else
				infoCenter = " " + infoCenter + "*";
			WriteGameLine("?", infoCenter, "?");
		}



		private TextBox GetCurrentTextBoxTarget()
		{
			return this.FindControls<TextBox>(x => x.Name == string.Format("txtGameSetStatus{0}", (_gameSetHandler.Sets.Count - 1))).
				Single();
		}

	
	}
}
