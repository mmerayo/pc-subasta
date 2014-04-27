using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Subasta.Client.Common.Game;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Extensions;
using Subasta.Infrastructure.Domain;
using Subasta.Interaction;

namespace Subasta
{
	public partial class FrmSays : Form
	{

		private readonly IGameSetHandler _gameSet;
		private readonly IFiguresCatalog _figuresCatalog;
		private readonly IUserInteractionManager _interactionManager;

		public FrmSays(IGameSetHandler gameSet, IFiguresCatalog figuresCatalog, IUserInteractionManager interactionManager)
		{
			_gameSet = gameSet;
			_figuresCatalog = figuresCatalog;
			_interactionManager = interactionManager;

			InitializeComponent();

			grpDeclarations.Location=grpTrumpOptions.Location = grpSayOptions.Location;
			grpSayOptions.BringToFront();

			_gameSet.GameHandler.GameSaysStarted += GameHandler_GameSaysStarted;
			_gameSet.GameHandler.GameSaysCompleted += GameHandler_GameSaysCompleted;
			_gameSet.GameHandler.HumanPlayerSayNeeded += GameHandler_HumanPlayerSayNeeded;
			_gameSet.GameHandler.HumanPlayerTrumpNeeded += GameHandler_HumanPlayerTrumpNeeded;
			_gameSet.GameHandler.HumanPlayerDeclarationSelectionNeeded += GameHandler_HumanPlayerDeclarationSelectionNeeded;

			EnableSayInteraction(false);
			EnableTrumpInteraction(false);
			EnableDeclarationsInteraction(false);
			LoadSuits();
		}

		

		private void GameHandler_GameSaysCompleted(ISaysStatus status)
		{
			_interactionManager.Reset();
		}

		private void GameHandler_GameSaysStarted(ISaysStatus status)
		{

		}

		Declaration? GameHandler_HumanPlayerDeclarationSelectionNeeded(IHumanPlayer source, Domain.Declaration[] availableDeclarations, IExplorationStatus status)
		{
			var declarables= source.GetUserDeclarables(status).ToArray();
			if(!declarables.Any())
				return null;

			LoadDeclarables(declarables.Cast<Declaration?>(),availableDeclarations.Length>declarables.Count());
			EnableDeclarationsInteraction(true);
			return _interactionManager.WaitUserInput<Declaration?>();

		}

		private void LoadDeclarables(IEnumerable<Declaration?> declarables, bool loadMateChooseOption)
		{
			var source = declarables.ToDictionary(value => value.ToString(), value => value.ToString().SeparateCamelCase());

			if(loadMateChooseOption)
				source.Add("Mate","(Que cante el compañero)");
				
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
				Enum.GetValues(typeof (SayKind))
					.Cast<SayKind>()
					.Where(
						x =>
						(int) x > saysStatus.PointsBet || x == SayKind.Paso ||
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
			                                  	var selectedValue = (SayKind) cmbSays.SelectedValue;

			                                  	var result = _figuresCatalog.GetFigureJustPoints(selectedValue!=SayKind.UnaMas? (int) selectedValue:1);
			                                  	EnableSayInteraction(false);
			                                  	return result;
			                                  });
		}

		private void btnSelectTrump_Click(object sender, EventArgs e)
		{
			_interactionManager.InputProvided(() =>
			                                  {
			                                  	var result = (ISuit) cmbSuits.SelectedValue;
			                                  	EnableTrumpInteraction(false);
			                                  	return result;
			                                  });
		}

		private void btnDeclarations_Click(object sender, EventArgs e)
		{
			_interactionManager.InputProvided(() =>
			                                  {
			                                  	var selectedValue = (string)cmbDeclarations.SelectedValue;
			                                  	var result = selectedValue == "Mate"?null:(Declaration?) Enum.Parse(typeof(Declaration), selectedValue);
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
			grpDeclarations.PerformSafely(x => x.Visible = enable);
			grpDeclarations.PerformSafely(x => x.BringToFront());
			btnDeclarations.PerformSafely(x => x.Enabled = enable);
			cmbDeclarations.PerformSafely(x => x.Enabled = enable);
			Application.DoEvents();
		}

		
	}
}