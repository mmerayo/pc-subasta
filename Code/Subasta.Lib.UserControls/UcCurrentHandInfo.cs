using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using StructureMap;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Images;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.Lib.UserControls
{
	public partial class UcCurrentHandInfo : UserControl, ICustomUserControl
	{
		private const string PbCardPrefix = "pbCard";
		private const string PbPetaPrefix = "pbPeta";
		private const string PbPlayerPrefix = "pbPlayer";
		private const string PbWinnerPrefix = "pbWinner";

		private IGameSetHandler _gameSetHandler;
		private IMediaProvider _mediaProvider;

		public UcCurrentHandInfo()
		{
			InitializeComponent();
		}

		#region ICustomUserControl Members

		public void Initialize()
		{
			_gameSetHandler = ObjectFactory.GetInstance<IGameSetHandler>();
			_mediaProvider = ObjectFactory.GetInstance<IMediaProvider>();


			_gameSetHandler.GameStarted += _gameSetHandler_GameStarted;
			_gameSetHandler.GameCompleted += _gameSetHandler_GameCompleted;
			_gameSetHandler.GameHandler.GameStatusChanged += GameHandler_GameStatusChanged;
			_gameSetHandler.GameHandler.GamePlayerPeta += GameHandler_GamePlayerPeta;
			_gameSetHandler.GameHandler.HandCompleted += GameHandler_HandCompleted;
		}

		#endregion

		private void GameHandler_HandCompleted(IExplorationStatus status)
		{
			IPlayer player = _gameSetHandler.GameHandler.GetPlayer(status.LastCompletedHand.PlayerWinner.Value);
			PictureBox target = this.FindControls<PictureBox>(x => x.Name.StartsWith(PbWinnerPrefix) && x.Tag == player).Single();
			target.PerformSafely(x => x.Image = _mediaProvider.GetImage(GameMediaType.Winner));
			Thread.Sleep(TimeSpan.FromSeconds(1.5));
			ClearAll();
		}

		private void GameHandler_GamePlayerPeta(IPlayer player, IExplorationStatus status)
		{
			string nameTarget = string.Format("{0}{1}", PbPetaPrefix,
			                                  (status.CurrentHand.CardsByPlaySequence().Count(y => y != null)));

			var target = this.FindControl<PictureBox>(nameTarget);
			target.PerformSafely(x => x.Image = _mediaProvider.GetImage(GameMediaType.Petar));
		}

		private void GameHandler_GameStatusChanged(IExplorationStatus status)
		{
			ICard[] cardsByPlaySequence = status.CurrentHand.CardsByPlaySequence().ToArray();
			for (int index = 0; index < cardsByPlaySequence.Length; index++)
			{
				ICard card = cardsByPlaySequence[index];
				if (card != null)
				{
					var target = this.FindControl<PictureBox>(PbCardPrefix + (index + 1));
					if (target.Image == null)
					{
						target.PerformSafely(x => x.Image = _mediaProvider.GetCard(card.ToShortString()));
					}
				}
			}
			if (status.CurrentHand.CardsByPlaySequence().Count(x => x != null) == 1)
			{
				this.PerformSafely(x => PaintPlayersInOrder(status));
			}
		}

		private void _gameSetHandler_GameCompleted(IExplorationStatus status)
		{
			this.PerformSafely(x => x.Visible = false);
		}

		private void _gameSetHandler_GameStarted(IExplorationStatus status)
		{
			this.PerformSafely(x => { x.Visible = true; });
		}

		private void PaintPlayersInOrder(IExplorationStatus status)
		{
			IPlayer currentPlayer = _gameSetHandler.GameHandler.GetPlayer(status.CurrentHand.FirstPlayer);

			var images = new[]
			             {
			             	_mediaProvider.GetImage(GameMediaType.Jugador1),
			             	_mediaProvider.GetImage(GameMediaType.Jugador2),
			             	_mediaProvider.GetImage(GameMediaType.Jugador3),
			             	_mediaProvider.GetImage(GameMediaType.Jugador4)
			             };

			for (int i = 1; i <= 4; i++)
			{
				var target = this.FindControl<PictureBox>(PbPlayerPrefix + i);
				target.Image = images[currentPlayer.PlayerNumber - 1];

				IEnumerable<PictureBox> targets = this.FindControls<PictureBox>(x => x.Name.EndsWith(i.ToString()));
				foreach (PictureBox pictureBox in targets)
				{
					pictureBox.Tag = currentPlayer;
				}
				currentPlayer = _gameSetHandler.GameHandler.GetPlayer(currentPlayer.NextNumber());
			}
		}

		private void ClearAll()
		{
			IEnumerable<PictureBox> pbs = this.FindControls<PictureBox>();

			foreach (PictureBox pb in pbs)
			{
				pb.PerformSafely(x => x.Image = null);
			}
		}
	}
}