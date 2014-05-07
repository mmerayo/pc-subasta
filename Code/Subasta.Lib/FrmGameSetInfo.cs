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
			
			//as copied from frmSays
			_gameSetHandler.GameHandler.GameSaysCompleted += GameHandler_GameSaysCompleted;
			_gameSetHandler.GameHandler.HumanPlayerDeclarationSelectionNeeded += GameHandler_HumanPlayerDeclarationSelectionNeeded;

			EnableDeclarationsInteraction(false);


			InitializeCustomUserControls();

		}

		private void InitializeCustomUserControls()
		{
			var userControls = this.FindControls<ICustomUserControl>();
			foreach (var userControl in userControls)
			{
				userControl.Initialize();
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