using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StructureMap;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Images;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.Lib.UserControls
{
	public partial class UcPlayerOverview : UserControl, ICustomUserControl
	{
		private const string PbTurnPrefixName = "pbTurno";
		private const string PbFirstPlayerPrefixName = "pbFirstPlayer";
		private const string PbTrumpPrefixName = "pbTrump";
		private const string LblBetPrefixName = "lblBet";


		private IGameSetHandler _gameSetHandler;
		private IMediaProvider _mediaProvider;


		public UcPlayerOverview()
		{
			InitializeComponent();

			SetToolTips();
		}

		private void SetToolTips()
		{
			foreach (var pb in this.FindControls<PictureBox>(x => x.Name.StartsWith(PbTrumpPrefixName)))
			{
				toolTip.SetToolTip(pb,"El triunfo puesto por el jugador");
			}

			foreach (var pb in this.FindControls<PictureBox>(x => x.Name.StartsWith(PbFirstPlayerPrefixName)))
			{
				toolTip.SetToolTip(pb, "El jugador sale en esta mano");
			}

			foreach (var pb in this.FindControls<PictureBox>(x => x.Name.StartsWith(PbTurnPrefixName)))
			{
				toolTip.SetToolTip(pb, "Es el turno de este jugador");
			}

			foreach (var lbl in this.FindControls<Label>(x => x.Name.StartsWith(LblBetPrefixName)))
			{
				toolTip.SetToolTip(lbl, "Puntos a los que va la mano puestos por este jugador");
			}
		}

		public void Initialize()
		{
			_mediaProvider = ObjectFactory.GetInstance<IMediaProvider>();
			PaintPlayers();
			_gameSetHandler = ObjectFactory.GetInstance<IGameSetHandler>();
			IGameHandler gameHandler = _gameSetHandler.GameHandler;
			
			gameHandler.GameSaysStatusChanged += gameHandler_GameSaysStatusChanged;
			_gameSetHandler.GameHandler.GameStatusChanged += GameHandler_GameStatusChanged;
			_gameSetHandler.GameStarted += _gameSetHandler_GameStarted;
			_gameSetHandler.GameSaysStarted += _gameSetHandler_GameSaysStarted;
			_gameSetHandler.GameSaysCompleted += _gameSetHandler_GameSaysCompleted;
			_gameSetHandler.GameHandler.GameCompleted += new StatusChangedHandler(GameHandler_GameCompleted);
		}

		void GameHandler_GameCompleted(IExplorationStatus status)
		{
			DeactivateAllIconClass<Label>(LblBetPrefixName);
			DeactivateAllIconClass<PictureBox>(PbTrumpPrefixName);
		}

		private void _gameSetHandler_GameSaysCompleted(ISaysStatus status)
		{
			IPlayer playerBets = _gameSetHandler.GameHandler.GetPlayer(
				_gameSetHandler.GameHandler.Status.PlayerBets);
			UpdateTrump(_gameSetHandler.GameHandler.Trump, playerBets);

			var lbl = ActivateIconClass<Label>(playerBets, LblBetPrefixName);
			lbl.PerformSafely(x=>x.Text = _gameSetHandler.GameHandler.Status.NormalizedPointsBet.ToString());
		}

		
		private void _gameSetHandler_GameSaysStarted(ISaysStatus status)
		{
			UpdateFirstPlayer(_gameSetHandler.GameHandler.GetPlayer(status.FirstPlayer));
			UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
		}

		
		private void _gameSetHandler_GameStarted(IExplorationStatus status)
		{
			UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
		}

		private void GameHandler_GameStatusChanged(IExplorationStatus status)
		{
			if (!status.IsCompleted)
				UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
		}

		private void gameHandler_GameSaysStatusChanged(ISaysStatus status)
		{
			if (!status.IsCompleted)
				UpdateTurn(_gameSetHandler.GameHandler.GetPlayer(status.Turn));
		}

		private void UpdateTurn(IPlayer player)
		{

			ActivateIconClass<PictureBox>(player, PbTurnPrefixName);
		}

		private void UpdateTrump(ISuit trump, IPlayer playerBets)
		{
			var target = ActivateIconClass<PictureBox>(playerBets, PbTrumpPrefixName);
			target.Image = _mediaProvider.GetCard(trump, 1);
		}


		private void UpdateFirstPlayer(IPlayer player)
		{
			ActivateIconClass<PictureBox>(player, PbFirstPlayerPrefixName);
		}


		private TControl ActivateIconClass<TControl>(IPlayer player, string pbPrefixName) where TControl:Control
		{
			var pbs = DeactivateAllIconClass<TControl>(pbPrefixName);

			var actual = pbs.Single(x => x.Name.EndsWith(player.PlayerNumber.ToString()));
			actual.PerformSafely(x => x.Visible = true);

			return actual;
		}

		private IEnumerable<TControl> DeactivateAllIconClass<TControl>(string pbPrefixName) where TControl : Control
		{
			var pbs = this.FindControls<TControl>(x => x.Name.StartsWith(pbPrefixName));

			foreach (var pb in pbs)
			{
				pb.PerformSafely(x => x.Visible = false);
			}
			return pbs;
		}

		private void PaintPlayers()
		{
			pb1.Image = _mediaProvider.GetImage(GameMediaType.Jugador1);
			pb2.Image = _mediaProvider.GetImage(GameMediaType.Jugador2);
			pb3.Image = _mediaProvider.GetImage(GameMediaType.Jugador3);
			pb4.Image = _mediaProvider.GetImage(GameMediaType.Jugador4);

			foreach (var pb in this.FindControls<PictureBox>(x => x.Name.StartsWith(PbTurnPrefixName)))
			{
				pb.Image = _mediaProvider.GetImage(GameMediaType.Turno);
			}

			foreach (var pb in this.FindControls<PictureBox>(x => x.Name.StartsWith(PbFirstPlayerPrefixName)))
			{
				pb.Image = _mediaProvider.GetImage(GameMediaType.FirstPlayer);
			}

		}
	}
}