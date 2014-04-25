using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Subasta.Client.Common.Game;
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

		public FrmSays(IGameSetHandler gameSet,IFiguresCatalog figuresCatalog,IUserInteractionManager interactionManager)
		{
			_gameSet = gameSet;
			_figuresCatalog = figuresCatalog;
			_interactionManager = interactionManager;
			
			InitializeComponent();

			grpTrumpOptions.Location = grpSayOptions.Location;
			grpSayOptions.BringToFront();

			_gameSet.GameHandler.GameSaysStarted += GameHandler_GameSaysStarted;
			_gameSet.GameHandler.GameSaysCompleted += GameHandler_GameSaysCompleted;
			_gameSet.GameHandler.HumanPlayerSayNeeded += GameHandler_HumanPlayerSayNeeded;
			_gameSet.GameHandler.HumanPlayerTrumpNeeded += GameHandler_HumanPlayerTrumpNeeded;

			EnableSayInteraction(false);
			EnableTrumpInteraction(false);
			LoadSuits();
		}

		


		void GameHandler_GameSaysCompleted(ISaysStatus status)
		{
			_interactionManager.Reset();
		}

		void GameHandler_GameSaysStarted(ISaysStatus status)
		{
			
		}

		IFigure GameHandler_HumanPlayerSayNeeded(IHumanPlayer source,ISaysStatus saysStatus)
		{
			return OnSayNeeded(source,saysStatus);
		}

		private IFigure OnSayNeeded(IHumanPlayer source, ISaysStatus saysStatus)
		{
			LoadSayKinds(saysStatus);
			EnableSayInteraction(true);

			var result=_interactionManager.WaitUserInput<IFigure>();

			return result;
		}

		ISuit GameHandler_HumanPlayerTrumpNeeded(IHumanPlayer source)
		{
			EnableTrumpInteraction(true);
			var result= _interactionManager.WaitUserInput<ISuit>();

			return result;
		}

		private void LoadSayKinds(ISaysStatus saysStatus)
		{
			IEnumerable<SayKind> sayKinds =
				Enum.GetValues(typeof (SayKind))
					.Cast<SayKind>()
					.Where(x => (int) x > saysStatus.PointsBet || x == SayKind.Paso || (saysStatus.PointsBet>0 && saysStatus.PointsBet<25 && x== SayKind.UnaMas));
			var source = sayKinds.ToDictionary(value => value, value => value.ToString().SeparateCamelCase());
			
			cmbSays.PerformSafely(x =>cmbSays.DataSource = new BindingSource(source, null));
			cmbSays.PerformSafely(x => cmbSays.ValueMember = "Key");
			cmbSays.PerformSafely(x => cmbSays.DisplayMember = "Value");
			
		}
		private void LoadSuits()
		{
			var source = Suit.Suits.ToDictionary(value => value, value => value.Name);

			cmbSuits.DataSource = new BindingSource(source, null);
			cmbSuits.PerformSafely(x => cmbSuits.ValueMember = "Key");
			cmbSuits.PerformSafely(x => cmbSuits.DisplayMember = "Value");
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			_interactionManager.InputProvided(() =>
			{
				var result= _figuresCatalog.GetFigureJustPoints((int)cmbSays.SelectedValue);
				EnableSayInteraction(false);
				return result;
			});
		}

		private void btnSelectTrump_Click(object sender, EventArgs e)
		{
			_interactionManager.InputProvided(() =>
			{
				var result=(ISuit) cmbSuits.SelectedValue;
				EnableTrumpInteraction(false);
				return result;
			});
		}

		private void EnableSayInteraction(bool enable)
		{
			//grpSayOptions.PerformSafely(x => x.Visible = enable);
			grpSayOptions.PerformSafely(x => x.BringToFront());
			btnSelect.PerformSafely(x=>x.Enabled= enable);
			cmbSays.PerformSafely(x => x.Enabled= enable);
		
			Application.DoEvents();
		}

		private void EnableTrumpInteraction(bool enable)
		{
			//grpTrumpOptions.PerformSafely(x => grpTrumpOptions.Visible = enable);
			grpTrumpOptions.PerformSafely(x => x.BringToFront());

			btnSelectTrump.PerformSafely(x => x.Enabled = enable);
			//btnSelectTrump.PerformSafely(x => x.Visible = enable);
			//cmbSuits.PerformSafely(x => x.Visible = enable);
			cmbSuits.PerformSafely(x => x.Enabled = enable);
			
			this.PerformSafely(x=>x.Invalidate());
			Application.DoEvents();
		}

		
	}
}
