using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;
using Subasta.Lib.Interaction;

namespace Subasta.Lib
{
	public partial class FrmGameSetInfo : Form
	{
		private readonly IGameSetHandler _gameSetHandler;
		private readonly IFiguresCatalog _figuresCatalog;
		private readonly IUserInteractionManager _interactionManager;

		public FrmGameSetInfo(IGameSetHandler gameSetHandler, IFiguresCatalog figuresCatalog, IUserInteractionManager interactionManager)
		{
			_gameSetHandler = gameSetHandler;
			_figuresCatalog = figuresCatalog;
			_interactionManager = interactionManager;

			InitializeComponent();

			grpDeclarations.Location = grpTrumpOptions.Location = grpSayOptions.Location;
			grpSayOptions.BringToFront();

			gameSetHandler.GameSaysCompleted += gameSetHandler_GameSaysCompleted;
			gameSetHandler.GameCompleted += gameSetHandler_GameCompleted;
			gameSetHandler.GameSetStarted += gameSetHandler_GameSetStarted;

			//as copied from frmSays

			_gameSetHandler.GameHandler.GameSaysCompleted += GameHandler_GameSaysCompleted;
			_gameSetHandler.GameHandler.HumanPlayerSayNeeded += GameHandler_HumanPlayerSayNeeded;
			_gameSetHandler.GameHandler.HumanPlayerTrumpNeeded += GameHandler_HumanPlayerTrumpNeeded;
			_gameSetHandler.GameHandler.HumanPlayerDeclarationSelectionNeeded += GameHandler_HumanPlayerDeclarationSelectionNeeded;

			EnableSayInteraction(false);
			EnableTrumpInteraction(false);
			EnableDeclarationsInteraction(false);
			LoadSuits();

		}

		#region Brought from frmSays

		private void GameHandler_GameSaysCompleted(ISaysStatus status)
		{
			_interactionManager.Reset();
		}


		Declaration? GameHandler_HumanPlayerDeclarationSelectionNeeded(IHumanPlayer source, Domain.Declaration[] availableDeclarations, IExplorationStatus status)
		{
			var declarables = source.GetUserDeclarables(status).ToArray();
			if (!declarables.Any())
				return null;

			LoadDeclarables(declarables.Cast<Declaration?>(), availableDeclarations.Length > declarables.Count());
			EnableDeclarationsInteraction(true);
			return _interactionManager.WaitUserInput<Declaration?>();

		}

		private void LoadDeclarables(IEnumerable<Declaration?> declarables, bool loadMateChooseOption)
		{
			var source = declarables.ToDictionary(value => value.ToString(), value => value.ToString().SeparateCamelCase());

			if (loadMateChooseOption)
				source.Add("Mate", "(Que cante el compañero)");

			cmbDeclarations.PerformSafely(x => x.DataSource = new BindingSource(source, null));
			cmbDeclarations.PerformSafely(x => x.ValueMember = "Key");
			cmbDeclarations.PerformSafely(x => x.DisplayMember = "Value");
		}


		private IFigure GameHandler_HumanPlayerSayNeeded(IHumanPlayer source, ISaysStatus saysStatus)
		{
			return OnSayNeeded(source, saysStatus);
		}

		private IFigure OnSayNeeded(IHumanPlayer source, ISaysStatus saysStatus)
		{
			LoadSayKinds(saysStatus);
			EnableSayInteraction(true);

			var result = _interactionManager.WaitUserInput<IFigure>();

			return result;
		}

		private ISuit GameHandler_HumanPlayerTrumpNeeded(IHumanPlayer source)
		{
			EnableTrumpInteraction(true);
			var result = _interactionManager.WaitUserInput<ISuit>();

			return result;
		}

		private void LoadSayKinds(ISaysStatus saysStatus)
		{
			IEnumerable<SayKind> sayKinds =
				Enum.GetValues(typeof(SayKind))
					.Cast<SayKind>()
					.Where(
						x =>
						(int)x > saysStatus.PointsBet || x == SayKind.Paso ||
						(saysStatus.PointsBet > 0 && saysStatus.PointsBet < 25 && x == SayKind.UnaMas));
			var source = sayKinds.ToDictionary(value => value, value => value.ToString().SeparateCamelCase());

			cmbSays.PerformSafely(x => cmbSays.DataSource = new BindingSource(source, null));
			cmbSays.PerformSafely(x => cmbSays.ValueMember = "Key");
			cmbSays.PerformSafely(x => cmbSays.DisplayMember = "Value");

		}

		private void LoadSuits()
		{
			var source = Suit.Suits.ToDictionary(value => value, value => value.Name);

			cmbSuits.DataSource = new BindingSource(source, null);
			cmbSuits.PerformSafely(x => x.ValueMember = "Key");
			cmbSuits.PerformSafely(x => x.DisplayMember = "Value");
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			_interactionManager.InputProvided(() =>
			{
				var selectedValue = (SayKind)cmbSays.SelectedValue;

				var result = _figuresCatalog.GetFigureJustPoints(selectedValue != SayKind.UnaMas ? (int)selectedValue : 1);
				EnableSayInteraction(false);
				return result;
			});
		}

		private void btnSelectTrump_Click(object sender, EventArgs e)
		{
			_interactionManager.InputProvided(() =>
			{
				var result = (ISuit)cmbSuits.SelectedValue;
				EnableTrumpInteraction(false);
				return result;
			});
		}

		private void btnDeclarations_Click(object sender, EventArgs e)
		{
			_interactionManager.InputProvided(() =>
			{
				var selectedValue = (string)cmbDeclarations.SelectedValue;
				var result = selectedValue == "Mate" ? null : (Declaration?)Enum.Parse(typeof(Declaration), selectedValue);
				EnableDeclarationsInteraction(false);
				return result;
			});
		}

		private void EnableSayInteraction(bool enable)
		{
			grpSayOptions.PerformSafely(x => x.Visible = enable);

			grpSayOptions.PerformSafely(x => x.BringToFront());
			btnSelect.PerformSafely(x => x.Enabled = enable);
			cmbSays.PerformSafely(x => x.Enabled = enable);
			Application.DoEvents();
		}

		private void EnableTrumpInteraction(bool enable)
		{
			grpTrumpOptions.PerformSafely(x => x.Visible = enable);
			grpTrumpOptions.PerformSafely(x => x.BringToFront());
			btnSelectTrump.PerformSafely(x => x.Enabled = enable);
			cmbSuits.PerformSafely(x => x.Enabled = enable);
			Application.DoEvents();
		}


		private void EnableDeclarationsInteraction(bool enable)
		{
			btnDeclarations.PerformSafely(x => x.Enabled = enable);
			cmbDeclarations.PerformSafely(x => x.Enabled = enable);
			grpDeclarations.PerformSafely(x => x.Visible = enable);
			grpDeclarations.PerformSafely(x => x.BringToFront());
			
			Application.DoEvents();
		}

		#endregion

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
			             		                        System.Drawing.GraphicsUnit.Point, ((byte) (0))),
			             	Location = new System.Drawing.Point(7, 3),
			             	Multiline = true,
			             	Name = "txtGameSetStatus" + newTab.TabIndex,
			             	ReadOnly = true,
			             	ScrollBars = System.Windows.Forms.ScrollBars.Vertical,
			             	Size = new System.Drawing.Size(191, 360),

			             };
			newTab.PerformSafely(x=>x.Controls.Add(newTxt));
		}

		private TextBox GetCurrentTextBoxTarget()
		{
			return this.FindControls<TextBox>(x => x.Name == string.Format("txtGameSetStatus{0}", (_gameSetHandler.Sets.Count - 1))).
				Single();
		}

		private void gameSetHandler_GameCompleted(IExplorationStatus status)
		{
			//remove previous line
			TextBox txtBox = GetCurrentTextBoxTarget();
			txtBox.PerformSafely(
				x => x.Text = txtBox.Text.Remove(txtBox.Text.LastIndexOf(Environment.NewLine)));

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
				infoCenter = "*" + infoCenter+ " ";
			else
				infoCenter = " "+infoCenter + "*";
			WriteGameLine("?", infoCenter, "?");
		}

		private void WriteGameLine(string infoT1, string infoCenter, string infoT2)
		{
			TextBox txtBox = GetCurrentTextBoxTarget();
			txtBox.PerformSafely(x =>
			                               {
										   x.Text += Environment.NewLine;
			                               	x.Text += string.Format("{0}|{1}|{2}", infoT1.PadLeft(8, ' '),
			                               	                        infoCenter.PadLeft(5, ' '), infoT2.PadRight(8, ' '));
			                               });
		}
	}
}