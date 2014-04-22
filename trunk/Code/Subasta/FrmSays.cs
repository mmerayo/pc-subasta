using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Subasta.Client.Common.Game;
using Subasta.Domain.Game;

namespace Subasta
{
	public partial class FrmSays : Form
	{
		private const int SemGame = 0;
		private const int SemPlayer = 1;
		private readonly Mutex[] _mutexes= new Mutex[2]; 
		private readonly IGameSetHandler _gameSet;

		public FrmSays(IGameSetHandler gameSet)
		{
			_gameSet = gameSet;
			_gameSet.GameHandler.GameSaysStarted +=new SaysStatusChangedHandler(GameHandler_GameSaysStarted);
			_gameSet.GameHandler.GameSaysCompleted += new SaysStatusChangedHandler(GameHandler_GameSaysCompleted);
			InitializeComponent();

			EnableInteraction(false);
			
			
			_gameSet.GameHandler.HumanPlayerSayNeeded += GameHandler_HumanPlayerSayNeeded;
		}

		void GameHandler_GameSaysCompleted(ISaysStatus status)
		{
			_mutexes[SemGame].Dispose();
			_mutexes[SemPlayer].Dispose();
		}

		void GameHandler_GameSaysStarted(ISaysStatus status)
		{
			_mutexes[SemGame] = new Mutex(true);
			_mutexes[SemPlayer] = new Mutex(true);
		}

	
		IFigure GameHandler_HumanPlayerSayNeeded(IHumanPlayer source)
		{
			return OnSayNeeded(source);
		}

		private IFigure OnSayNeeded(IHumanPlayer source)
		{
			LoadSayKinds();
			EnableInteraction(true);
			_mutexes[SemPlayer].ReleaseMutex();
			_mutexes[SemGame].WaitOne();

			throw new NotImplementedException();
			//TODO: PICK SELECTED VALUE
		}

		private void LoadSayKinds()
		{
			IEnumerable<SayKind> sayKinds = Enum.GetValues(typeof(SayKind)).Cast<SayKind>();
			var source = sayKinds.ToDictionary(value => value, value => value.ToString().SeparateCamelCase());

			cmbSays.DataSource = new BindingSource(source, null);
			cmbSays.ValueMember = "Key";
			cmbSays.DisplayMember = "Value";

			//TODO:eliminar los ya pasados que solo se usen como primary say por las figuras
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			_mutexes[SemPlayer].WaitOne();
			//TODO: store selection
			_mutexes[SemGame].ReleaseMutex();
		}

		private void EnableInteraction(bool enable)
		{
			btnSelect.Enabled = cmbSays.Enabled= enable;
		}
	}
}
