using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StructureMap;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;
using Subasta.Lib.Interaction;
using Subasta.Lib.UserControls;

namespace Subasta.Lib
{
	public partial class FrmGameSetInfo : Form
	{
		private readonly IGameSetHandler _gameSetHandler;
		private readonly IUserInteractionManager _interactionManager;

		public FrmGameSetInfo(IGameSetHandler gameSetHandler, IUserInteractionManager interactionManager)
		{
			_gameSetHandler = gameSetHandler;
			_interactionManager = interactionManager;

			InitializeComponent();

			grpTrumpOptions.Location = grpDeclarations.Location;
			//as copied from frmSays
			_gameSetHandler.GameHandler.GameSaysCompleted += GameHandler_GameSaysCompleted;
			_gameSetHandler.GameHandler.HumanPlayerTrumpNeeded += GameHandler_HumanPlayerTrumpNeeded;
			_gameSetHandler.GameHandler.HumanPlayerDeclarationSelectionNeeded += GameHandler_HumanPlayerDeclarationSelectionNeeded;

			
			EnableTrumpInteraction(false);
			EnableDeclarationsInteraction(false);
			LoadSuits();

			InjectUserControls();

		}

		private void InjectUserControls()
		{
			var userControls = this.FindControls<ICustomUserControl>();
			foreach (var userControl in userControls)
			{
				userControl.Inject();
			}
		}

		#region Brought from frmSays

		private void GameHandler_GameSaysCompleted(ISaysStatus status)
		{
			_interactionManager.Reset();
		}


		Declaration? GameHandler_HumanPlayerDeclarationSelectionNeeded(IHumanPlayer source, Domain.Declaration[] availableDeclarations, IExplorationStatus status)
		{
			var declarables = source.GetUserDeclarables(status).Where(availableDeclarations.Contains);
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



		

		private ISuit GameHandler_HumanPlayerTrumpNeeded(IHumanPlayer source)
		{
			EnableTrumpInteraction(true);
			var result = _interactionManager.WaitUserInput<ISuit>();

			return result;
		}

		

		private void LoadSuits()
		{
			var source = Suit.Suits.ToDictionary(value => value, value => value.Name);

			cmbSuits.DataSource = new BindingSource(source, null);
			cmbSuits.PerformSafely(x => x.ValueMember = "Key");
			cmbSuits.PerformSafely(x => x.DisplayMember = "Value");
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

		private void EnableTrumpInteraction(bool enable)
		{
			grpTrumpOptions.PerformSafely(x => x.Visible = enable);
			grpTrumpOptions.PerformSafely(x => x.BringToFront());
			btnSelectTrump.PerformSafely(x => x.Enabled = enable);
			cmbSuits.PerformSafely(x => x.Enabled = enable);
		}


		private void EnableDeclarationsInteraction(bool enable)
		{
			btnDeclarations.PerformSafely(x => x.Enabled = enable);
			cmbDeclarations.PerformSafely(x => x.Enabled = enable);
			grpDeclarations.PerformSafely(x => x.Visible = enable);
			grpDeclarations.PerformSafely(x => x.BringToFront());
			
		}

		#endregion

		

		

		
	}
}