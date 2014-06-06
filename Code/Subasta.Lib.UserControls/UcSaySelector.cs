using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using StructureMap;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Media;
using Subasta.Domain.Game;

namespace Subasta.Lib.UserControls
{
	public partial class UcSaySelector : UserControl,ICustomUserControl
	{
		private IUserInteractionManager _interactionManager;
		private IGameSetHandler _gameSetHandler;
		private IFiguresCatalog _figuresCatalog;
		private ISoundPlayer _soundPlayer;

		public UcSaySelector()
		{
			InitializeComponent();
		}

		public void Initialize()
		{
			

			_interactionManager = ObjectFactory.GetInstance<IUserInteractionManager>();
			_gameSetHandler = ObjectFactory.GetInstance<IGameSetHandler>();
			_figuresCatalog= ObjectFactory.GetInstance<IFiguresCatalog>();
			_soundPlayer=ObjectFactory.GetInstance<ISoundPlayer>();

			EnableSayInteraction(false);
			PaintFigures(_figuresCatalog);

			_gameSetHandler.GameHandler.HumanPlayerSayNeeded += GameHandler_HumanPlayerSayNeeded;
			_gameSetHandler.GameHandler.GameSaysStatusChanged += GameHandler_GameSaysStatusChanged;
			_gameSetHandler.GameSaysStarted += _gameSetHandler_GameSaysStarted;
			_gameSetHandler.GameSaysCompleted += _gameSetHandler_GameSaysCompleted;
		}


		private void _gameSetHandler_GameSaysCompleted(ISaysStatus status)
		{
			this.PerformSafely(x =>
			{
				x.Visible = false;
			});
		}

		private void _gameSetHandler_GameSaysStarted(ISaysStatus status)
		{
			this.PerformSafely(x =>
			{
				txtSays.Clear();
				x.Visible = true;
			});
			
		}

		private void PaintFigures(IFiguresCatalog figuresCatalog)
		{
			figuresCatalog.Init();
			var figures = figuresCatalog.Figures.Take(figuresCatalog.Figures.Count()-3);
			string text = string.Empty;
			int i = 1;
			foreach (var figure in figures)
			{
				text += string.Format("{3}.{0}-{1}{2}", figure.Say.ToString().SeparateCamelCase(),
				                      figure.GetType().Name.SeparateCamelCase().Replace("Figure ", string.Empty),
				                      Environment.NewLine, i++);
			}

			txtMarques.Text = text;
		}

		private void UpdateMarques(ISaysStatus status)
		{
			ISay last = status.Says.Last();
			txtSays.PerformSafely(
				x =>
				x.Text +=
				string.Format("Jugador {0}:{1}, Van:{3}{2}", last.PlayerNum, last.Figure.ToString().SeparateCamelCase(), Environment.NewLine, status.PointsBet));
		}

		private void GameHandler_GameSaysStatusChanged(ISaysStatus status)
		{
			UpdateMarques(status);

			Thread.Sleep(TimeSpan.FromSeconds(0.5));//TODO: TO CONFIGURATION source
		}

		private void EnableSayInteraction(bool enable)
		{
			this.PerformSafely(x =>
			{	grpSayOptions.BringToFront();
				btnSelect.Enabled= cmbSays.Enabled= enable;
			});
		}
		private void btnSelect_Click(object sender, EventArgs e)
		{
		_soundPlayer.PlayAsync(GameSoundType.Selection);
			
			_interactionManager.InputProvided(() =>
			{
				var selectedValue = (SayKind)cmbSays.SelectedValue;

				var result = selectedValue != SayKind.UnaMas
					? _figuresCatalog.GetFigureJustPoints((int)selectedValue)
					: _figuresCatalog.Figures.First(x => x.Say == SayKind.Una || x.Say==SayKind.UnaMas);
				EnableSayInteraction(false);

				return result;
			});
		}
		private IFigure GameHandler_HumanPlayerSayNeeded(IHumanPlayer source, ISaysStatus saysStatus)
		{
			return OnSayNeeded(source, saysStatus);
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

		private IFigure OnSayNeeded(IHumanPlayer source, ISaysStatus saysStatus)
		{
			LoadSayKinds(saysStatus);
			EnableSayInteraction(true);

			var result = _interactionManager.WaitUserInput<IFigure>();
			if (result.PointsBet == 1 && saysStatus.PointsBet >= 1)
			{
				//force using alternative, defect in algorithm
				result.IsAvailable(saysStatus, saysStatus.PointsBet);
			}

			return result;
		}

	}
}
