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
using Subasta.Domain.Game;
using Subasta.Extensions;
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
			_gameSet.GameHandler.GameSaysStarted +=GameHandler_GameSaysStarted;
			_gameSet.GameHandler.GameSaysCompleted += GameHandler_GameSaysCompleted;
			_gameSet.GameHandler.HumanPlayerSayNeeded += GameHandler_HumanPlayerSayNeeded;

			InitializeComponent();

			EnableInteraction(false);
		}

		void GameHandler_GameSaysCompleted(ISaysStatus status)
		{
			_interactionManager.Reset();
		}

		void GameHandler_GameSaysStarted(ISaysStatus status)
		{
			
		}

		private IFigure LastSay { get; set; }

		IFigure GameHandler_HumanPlayerSayNeeded(IHumanPlayer source,ISaysStatus saysStatus)
		{
			return OnSayNeeded(source,saysStatus);
		}

		private IFigure OnSayNeeded(IHumanPlayer source, ISaysStatus saysStatus)
		{
			LoadSayKinds(saysStatus);
			EnableInteraction(true);

			_interactionManager.WaitUserInput();

			var result = LastSay;
			LastSay = null;
			return result;
		}

		private void LoadSayKinds(ISaysStatus saysStatus)
		{
			IEnumerable<SayKind> sayKinds =
				Enum.GetValues(typeof (SayKind))
					.Cast<SayKind>()
					.Where(x => (int) x > saysStatus.PointsBet || x == SayKind.Paso || (saysStatus.PointsBet<25 && x== SayKind.UnaMas));
			var source = sayKinds.ToDictionary(value => value, value => value.ToString().SeparateCamelCase());

			cmbSays.DataSource = new BindingSource(source, null);
			cmbSays.PerformSafely(x => cmbSays.ValueMember = "Key");
			cmbSays.PerformSafely(x => cmbSays.DisplayMember = "Value");
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			_interactionManager.InputProvided(() =>
			{
				LastSay = _figuresCatalog.GetFigureJustPoints((int)cmbSays.SelectedValue);
				EnableInteraction(false);
			});
			
		
		}

		private void EnableInteraction(bool enable)
		{
			btnSelect.PerformSafely(x=>x.Enabled = enable);
			cmbSays.PerformSafely(x => x.Enabled = enable);
		}
	}
}
