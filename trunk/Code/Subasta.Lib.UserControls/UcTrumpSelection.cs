using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StructureMap;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.Lib.UserControls
{
	public partial class UcTrumpSelection : UserControl,ICustomUserControl
	{
		private IUserInteractionManager _interactionManager;
		private IGameSetHandler _gameSetHandler;
		
		public UcTrumpSelection()
		{
			InitializeComponent();
			
		}

		public void Initialize()
		{
			LoadSuits();
			EnableTrumpInteraction(false);
			_interactionManager = ObjectFactory.GetInstance<IUserInteractionManager>();
			_gameSetHandler = ObjectFactory.GetInstance<IGameSetHandler>();
			_gameSetHandler.GameHandler.HumanPlayerTrumpNeeded += GameHandler_HumanPlayerTrumpNeeded;

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

		private void EnableTrumpInteraction(bool enable)
		{
			grpTrumpOptions.PerformSafely(x => x.Visible = enable);
			grpTrumpOptions.PerformSafely(x => x.BringToFront());
			btnSelectTrump.PerformSafely(x => x.Enabled = enable);
			cmbSuits.PerformSafely(x => x.Enabled = enable);
		}

		private ISuit GameHandler_HumanPlayerTrumpNeeded(IHumanPlayer source)
		{
			EnableTrumpInteraction(true);
			var result = _interactionManager.WaitUserInput<ISuit>();

			return result;
		}
	}
}
