using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Domain.Game;
using Subasta.Lib.Interaction;

namespace Subasta.Lib
{
	public partial class FrmGameInfo : Form
	{
		private readonly IGameSetHandler _gameSetHandler;
		private readonly IFiguresCatalog _figuresCatalog;
		private readonly IUserInteractionManager _interactionManager;

		public FrmGameInfo(IGameSetHandler gameSetHandler,
		IFiguresCatalog figuresCatalog
		, IUserInteractionManager interactionManager)
		{
			_gameSetHandler = gameSetHandler;
			_figuresCatalog = figuresCatalog;
			_interactionManager = interactionManager;
			InitializeComponent();

			_gameSetHandler.GameSaysStarted += _gameSetHandler_GameSaysStarted;
			_gameSetHandler.GameSaysCompleted += _gameSetHandler_GameSaysCompleted;
			_gameSetHandler.GameHandler.GameSaysStatusChanged += GameHandler_GameSaysStatusChanged;
			_gameSetHandler.GameStarted += _gameSetHandler_GameStarted;
			_gameSetHandler.GameHandler.GameStatusChanged += GameHandler_GameStatusChanged;
			_gameSetHandler.GameHandler.HandCompleted += GameHandler_HandCompleted;
			_gameSetHandler.GameHandler.HumanPlayerSayNeeded += GameHandler_HumanPlayerSayNeeded;
			
			grpSayOptions.BringToFront();
			PaintFigures(figuresCatalog);
			EnableSayInteraction(false);
		}

		#region Says
		private void EnableSayInteraction(bool enable)
			{
			grpSayOptions.PerformSafely(x => x.Visible = enable);

			grpSayOptions.PerformSafely(x => x.BringToFront());
			btnSelect.PerformSafely(x => x.Enabled = enable);
			cmbSays.PerformSafely(x => x.Enabled = enable);
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

			return result;
			}

		#endregion

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

		void GameHandler_HandCompleted(IExplorationStatus status)
		{
			
			string t1="---", t2="---";
			if(status.LastCompletedHand.Declaration.HasValue)
			{
				var target = status.LastCompletedHand.Declaration.Value.ToString().SeparateCamelCase();
				if(status.LastCompletedHand.TeamWinner==1)
					t1 = target+"*";
				else
					t2 = "*"+target;
			}

			else
			{
			if (status.LastCompletedHand.TeamWinner == 1)
				t1 = "*gana*";
			else
				t2 = "*gana*";
			}

			txtBazas.PerformSafely(x => x.Text += WriteBaza(t1,t2) );
		}



		private void GameHandler_GameStatusChanged(IExplorationStatus status)
		{
			if (!status.IsCompleted)
				UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
			lblPuntos13.PerformSafely(x => x.Text = status.SumTotalTeam(1).ToString());
			lblPuntos24.PerformSafely(x => x.Text = status.SumTotalTeam(2).ToString());
		}

		private void _gameSetHandler_GameStarted(IExplorationStatus status)
		{
			UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
		}


		private void GameHandler_GameSaysStatusChanged(ISaysStatus status)
		{
			if (!status.IsCompleted)
				UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
			UpdateMarques(status);

			Thread.Sleep(TimeSpan.FromSeconds(0.5));//TODO: TO CONFIGURATION source
		}

		private void UpdateMarques(ISaysStatus status)
		{
			ISay last = status.Says.Last();
			txtSays.PerformSafely(
				x =>
				x.Text +=
				string.Format("Jugador {0}:{1}, Van:{3}{2}", last.PlayerNum, last.Figure.ToString().SeparateCamelCase(), Environment.NewLine,status.PointsBet));
		}

		private void UpdateTurn(IPlayer player)
		{
			//if (!status.IsCompleted)
			lblTurn.PerformSafely(x => x.Text = player.Name);
		}

		private void _gameSetHandler_GameSaysStarted(ISaysStatus status)
		{
			this.PerformSafely(x =>
			                   {
							   grpPuntos24.Visible = grpPtos13.Visible = grpTrump.Visible = txtBazas.Visible = grpPlayerBets.Visible = grpPuntos.Visible = false;
			                   	txtMarques.Visible = true;
			                   });
			txtSays.PerformSafely(x =>
			                      {
			                      	x.Visible = true;
			                      	x.Clear();
			                      	lblFirstPlayer.Text = _gameSetHandler.GameHandler.GetPlayer(status.FirstPlayer).Name;

			                      });
			UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
		}

		private void _gameSetHandler_GameSaysCompleted(ISaysStatus status)
		{
			txtSays.PerformSafely(x =>
			                      {
			                      	x.Visible = false;
			                      	x.Clear();
			                      });
			this.PerformSafely(x =>
			                   {
			                   	grpPtos13.Visible = grpPuntos24.Visible = grpTrump.Visible = txtBazas.Visible =  grpPlayerBets.Visible=grpPuntos.Visible= true;
								txtBazas.Text = WriteBaza("J1&3","J2&4");
			                   	lblPuntos13.Text = lblPuntos24.Text = "0";
			                   	lblTrump.Text = _gameSetHandler.GameHandler.Trump.Name;
			                   	lblPlayerBets.Text = _gameSetHandler.GameHandler.GetPlayer(
			                   		_gameSetHandler.GameHandler.Status.PlayerBets).
			                   		Name;
			                   	lblPuntos.Text = _gameSetHandler.GameHandler.Status.NormalizedPointsBet.ToString();

			                   	lblTrump.Visible = true;
			                   	txtMarques.Visible = false;
			                   });

		}

		private static string WriteBaza(string infoT1, string infoT2)
		{
			return string.Format("{0} - {1}{2}", infoT1.PadLeft(10), infoT2.PadRight(10),Environment.NewLine);
		}

		private void FrmSay_Load(object sender, EventArgs e)
		{
		}
	}
}