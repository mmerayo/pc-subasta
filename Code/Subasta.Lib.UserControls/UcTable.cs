﻿using System;
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
using Subasta.Domain;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.Lib.UserControls
{
	public partial class UcTable : UserControl,ICustomUserControl
	{
		private IGameSetHandler _gameSetHandler;
		private IUserInteractionManager _userInteractionManager;
		private IMediaProvider _mediaProvider;
		private ICard _lastCardPlayed;
		private int _lastPlayerPeta = int.MinValue;
		private Size _sizePbs13 = new Size(50, 70);
		private Size _sizePbs24 = new Size(70, 50);
		private ISoundPlayer _soundPlayer;

		public UcTable()
		{
			InitializeComponent();
		}

		public void Initialize()
		{
			_gameSetHandler = ObjectFactory.GetInstance<IGameSetHandler>();
			_userInteractionManager= ObjectFactory.GetInstance<IUserInteractionManager>();
			_mediaProvider = ObjectFactory.GetInstance<IMediaProvider>();
			_soundPlayer = ObjectFactory.GetInstance<ISoundPlayer>();

			SuscribeToEvents();
			ConfigureControlsAndLocations();

		}

		private void ConfigureControlsAndLocations()
		{
			pb1.Location = new Point((Width / 2) - pb1.Width / 2, Height - pb1.Height);
			pb3.Location = new Point((Width / 2) - pb3.Width / 2, 0);
			pb2.Location = new Point(Width - pb2.Width, (Height / 2) - pb2.Height / 2);
			pb4.Location = new Point(0, (Height / 2) - pb4.Height / 2);

			pb1.Image = _mediaProvider.GetImage(GameMediaType.Jugador1);
			pb2.Image = _mediaProvider.GetImage(GameMediaType.Jugador2);
			pb3.Image = _mediaProvider.GetImage(GameMediaType.Jugador3);
			pb4.Image = _mediaProvider.GetImage(GameMediaType.Jugador4);

			pb1.SizeMode = pb2.SizeMode = pb3.SizeMode = pb4.SizeMode = PictureBoxSizeMode.StretchImage;

			pbPetar.Image = _mediaProvider.GetImage(GameMediaType.Petar);
			pbPetar.SizeMode = PictureBoxSizeMode.StretchImage;
			pbPetar.Visible = false;

			lblInfo.Left = 0;
			lblInfo.Top = Height - lblInfo.Height;
		}

		private void SuscribeToEvents()
		{
			IGameHandler gameHandler = _gameSetHandler.GameHandler;

			_gameSetHandler.GameStarted += _gameSetHandler_GameStarted;

			gameHandler.GameSaysStarted += GameHandler_GameSaysStarted;
			gameHandler.HumanPlayerMoveSelectionNeeded += GameHandler_HumanPlayerMoveSelectionNeeded;
			gameHandler.GameStatusChanged += GameHandler_GameStatusChanged;
			gameHandler.HandCompleted += GameHandler_HandCompleted;
			gameHandler.DeclarationEmit += gameHandler_DeclarationEmit;
			gameHandler.GameCompleted += GameHandler_GameCompleted;
			gameHandler.TurnChanged += GameHandler_TurnChanged;
			gameHandler.GamePlayerPeta += gameHandler_GamePlayerPeta;
			gameHandler.GameSaysStatusChanged += gameHandler_GameSaysStatusChanged;
		}



		private void _gameSetHandler_GameStarted(IExplorationStatus status)
		{
			string format = string.Format("Triunfo {0}!!", status.Trump);
			var pb = this.FindControls<PictureBox>(x => x.Name == "pb" + status.PlayerBets).Single();
			
			ShowBalloon(pb, format, TimeSpan.FromSeconds(2));
		}

		private void gameHandler_GamePlayerPeta(IPlayer player, IExplorationStatus status)
		{
			var location = new Point();
			switch (player.PlayerNumber)
			{
				case 1:
					location.X = pb1.Left + pb1.Width;
					location.Y = pb1.Top;
					break;
				case 2:
					location.X = pb2.Left;
					location.Y = pb2.Top + pb2.Height;
					break;
				case 3:
					location.X = pb3.Left + pb3.Width;
					location.Y = 0;
					break;
				case 4:
					location.X = 0;
					location.Y = pb4.Top + pb4.Height;
					break;
			}

			pbPetar.PerformSafely(x =>
			{
				x.Location = location;
				x.Visible = true;
				x.Update();
			});
		}

		private void gameHandler_DeclarationEmit(IPlayer player, Declaration declaration)
		{
			PictureBox target = this.FindControls<PictureBox>(x => x.Name == "pb" + player.PlayerNumber).First();

			string text = declaration.ToString().SeparateCamelCase() + "!!";
			_soundPlayer.PlayAsync(GameSoundType.DeclarationEmit);
			ShowBalloon(target, text, TimeSpan.FromSeconds(2));
		}

		private void gameHandler_GameSaysStatusChanged(ISaysStatus status)
		{
			ISay last = status.Says.Last();

			PictureBox target = this.FindControls<PictureBox>(x => x.Name == "pb" + last.PlayerNum).First();

			string text = last.Figure.Say.ToString().SeparateCamelCase() + "!!";
			ShowBalloon(target, text, TimeSpan.FromSeconds(2));
		}

		private void ShowBalloon(PictureBox target, string text, TimeSpan showLenght)
		{
			_soundPlayer.PlayRandomVoice();
			this.PerformSafely(x => balloonInfo.Show(text, target));

			Thread.Sleep(showLenght);
			this.PerformSafely(x => balloonInfo.Hide(target));
		}

		private void GameHandler_TurnChanged(int turn)
		{
			if(_gameSetHandler.GameHandler.GetPlayer(turn).PlayerType==PlayerType.Human)
				_soundPlayer.PlayAsync(GameSoundType.TurnChanged);
		}

		private void GameHandler_GameCompleted(IExplorationStatus status)
		{
			Thread.Sleep(TimeSpan.FromSeconds(3));
		}

		private void GameHandler_HandCompleted(IExplorationStatus status)
		{
			//CLEAN TABLE
			IEnumerable<ICard> cardsByPlaySequence = status.LastCompletedHand.CardsByPlaySequence();
			foreach (ICard card in cardsByPlaySequence)
			{
				RemovePictureBoxesByCard(card);
			}
			//TODO:CREATE BAZA
		}


		private void GameHandler_GameStatusChanged(IExplorationStatus status)
		{
			ICard lastCardPlayed = status.LastCardPlayed;
			if (lastCardPlayed != _lastCardPlayed)
			//TODO: THIS MIGHT BE ALREADY FIXED: due to defect  as the event is triggered twice
			{
				MoveCard(status, status.LastPlayerMoved, lastCardPlayed);


				_soundPlayer.PlayAsync(GameSoundType.CardPlayed);
				_lastCardPlayed = lastCardPlayed;
				// WAIT TIME SO THE USER CAN SEE it
				Thread.Sleep(TimeSpan.FromSeconds(1));
				if (_lastPlayerPeta != status.LastPlayerMoved)
					pbPetar.PerformSafely(x => x.Visible = false);
			}
		}


		private void InitializePictureBoxes(IPlayer player)
		{
			Point startPoint = Point.Empty;

			startPoint = GetPlayerCardsStartPaintingPoint(player, player.Cards.Length);
			Image imgReverso = _mediaProvider.GetImage(GameMediaType.Reverso);
			var imgReversoV = (Image)imgReverso.Clone();
			imgReversoV.RotateFlip(RotateFlipType.Rotate90FlipX);
			for (int index = 0; index < player.Cards.Length; index++)
			{
				Size size = Size.Empty;
				Point location = startPoint;

				switch (player.PlayerNumber)
				{
					case 1:
					case 3:
						location.X += (_sizePbs13.Width) * index;
						size = _sizePbs13;
						break;

					case 2:
					case 4:
						location.Y += (_sizePbs24.Height) * index;
						size = _sizePbs24;
						imgReverso = imgReversoV;
						break;
				}

				ICard card = player.Cards[index];
				Image image = player.PlayerNumber == 1 ? (Image)_mediaProvider.GetCard(card.ToShortString()) : imgReverso;
				PictureBox control = CreatePictureBoxControl(location, size, card, image);
			}


			//

			Invalidate();
		}

		private Point GetPlayerCardsStartPaintingPoint(IPlayer player, int numCards)
		{
			Point startPoint;
			var centerPoint = new Point(Width / 2, Height / 2);
			switch (player.PlayerNumber)
			{
				case 1:
					startPoint = new Point(centerPoint.X - (_sizePbs13.Width * numCards) / 2, Size.Height - pb1.Height - _sizePbs13.Height);
					break;

				case 2:
					startPoint = new Point(Width - _sizePbs24.Width - pb2.Width, centerPoint.Y - ((_sizePbs24.Height) * numCards) / 2);
					break;

				case 3:
					startPoint = new Point(centerPoint.X - (_sizePbs13.Width * numCards) / 2, pb3.Height);
					break;

				case 4:
					startPoint = new Point(pb4.Width, centerPoint.Y - ((_sizePbs24.Height) * numCards) / 2);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return startPoint;
		}

		private PictureBox CreatePictureBoxControl(Point location, Size size, ICard card, Image image)
		{
			var current = new PictureBox
			{
				Location = location,
				Size = size,
				Name = card.ToShortString(),
				Image = image,
				SizeMode = PictureBoxSizeMode.StretchImage,
				Tag = card
			};

			this.PerformSafely(x => x.Controls.Add(current));

			return current;
		}

		private void HookPlayerEventsToPictureBox(PictureBox target, bool hook)
		{
			if (hook)
				target.MouseUp += PbCard_MouseUp;
			else
			{
				target.MouseUp -= PbCard_MouseUp;
			}
		}

		private void PbCard_MouseUp(object sender, MouseEventArgs e)
		{
			var pictureBox = (PictureBox)sender;
			bool peta = e.Button == MouseButtons.Right;

			_userInteractionManager.InputProvided(() =>
			{
				EnableMoves(_gameSetHandler.GameHandler.Player1, false);
				return new UserCardSelection((ICard)pictureBox.Tag, peta);
			});
		}


		private void GameHandler_GameSaysStarted(ISaysStatus status)
		{
			ClearAllPictureBoxControls();
			InitializePictureBoxes(_gameSetHandler.GameHandler.Player1);
			InitializePictureBoxes(_gameSetHandler.GameHandler.Player2);
			InitializePictureBoxes(_gameSetHandler.GameHandler.Player3);
			InitializePictureBoxes(_gameSetHandler.GameHandler.Player4);
		}

		private void ClearAllPictureBoxControls()
		{
			PictureBox[] pbs = this.FindControls<PictureBox>(x => x.Tag != null && x.Tag is ICard).ToArray();
			foreach (PictureBox pb in pbs)
			{
				this.PerformSafely(x =>
				{
					x.Controls.Remove(pb);
					pb.Dispose();
				});
			}
		}

		private void RemovePictureBoxesByCard(ICard card)
		{
			PictureBox[] pbs = this.FindControls<PictureBox>(x => x.Tag == card).ToArray();
			foreach (PictureBox pb in pbs)
			{
				this.PerformSafely(x =>
				{
					x.Controls.Remove(pb);
					pb.Dispose();
				});
			}
		}

		private ICard GameHandler_HumanPlayerMoveSelectionNeeded(IHumanPlayer source, ICard[] validMoves, out bool peta)
		{
			EnableMoves(source, true, validMoves);
			var selection = _userInteractionManager.WaitUserInput<UserCardSelection>();
			//MoveCard(source.PlayerNumber, selection.Card);
			EnableMoves(source, false);
			peta = selection.Peta;
			return selection.Card;
		}

		private void MoveCard(IExplorationStatus status, int playerNumber, ICard card)
		{
			//Remove from hand
			RemovePictureBoxesByCard(card);
			//PAINT REMAINING CARDS
			CompactPlayerCards(status, playerNumber);

			PictureBox pbCard = PaintCardPlayed(playerNumber, card);

			PaintPlayerComment(status, playerNumber, card);
		}

		private void PaintPlayerComment(IExplorationStatus status, int playerNumber, ICard card)
		{
			
			//arrastre

			//fallo

			//marco

			//fallo y marco

			//achique

			//subo

			//asisto


			//si condicion, localizar el pb del jugador e invocar show ballon
		}

		private void CompactPlayerCards(IExplorationStatus status, int playerNumber)
		{
			IPlayer player = _gameSetHandler.GameHandler.GetPlayer(playerNumber);
			ICard[] playerCards = status.PlayerCards((byte)playerNumber);
			Point startPoint = GetPlayerCardsStartPaintingPoint(player, playerCards.Count());
			int index = 0;
			foreach (ICard card in playerCards)
			{
				Point location = startPoint;

				switch (player.PlayerNumber)
				{
					case 1:
					case 3:
						location.X += (_sizePbs13.Width) * index;
						break;

					case 2:
					case 4:
						location.Y += (_sizePbs24.Height) * index;
						break;
				}

				PictureBox pictureBox = this.FindControls<PictureBox>(x => x.Tag == card).First();
				pictureBox.PerformSafely(x =>
				{
					x.Location = location;
					x.BringToFront();
				});
				index++;
			}
		}

		private PictureBox PaintCardPlayed(int playerNumber, ICard card)
		{
			var centerPoint = new Point(Width / 2, Height / 2);
			var destination = new Point(0, 0);

			//set x
			switch (playerNumber)
			{
				case 1:
				case 3:
					destination.X = centerPoint.X - (_sizePbs13.Width / 2);
					break;
				case 4:
					destination.X = centerPoint.X - (_sizePbs13.Width / 2) - _sizePbs24.Width;
					break;
				case 2:
					destination.X = centerPoint.X + (_sizePbs13.Width / 2);
					break;
			}
			//set Y
			switch (playerNumber)
			{
				case 1:
					destination.Y = centerPoint.Y + (_sizePbs24.Height / 2);
					break;
				case 3:
					destination.Y = centerPoint.Y - (_sizePbs24.Height / 2) - _sizePbs13.Height;
					break;
				case 2:
				case 4:
					destination.Y = centerPoint.Y - (_sizePbs24.Height / 2);
					break;
			}

			//image
			var image = _mediaProvider.GetCard(card.ToShortString());
			switch (playerNumber)
			{
				case 2:
					image.RotateFlip(RotateFlipType.Rotate270FlipY);
					break;
				case 3:
					image.RotateFlip(RotateFlipType.Rotate180FlipX);
					break;
				case 4:
					image.RotateFlip(RotateFlipType.Rotate90FlipY);
					break;
			}

			PictureBox result = CreatePictureBoxControl(destination,
				playerNumber == 2 || playerNumber == 4 ? _sizePbs24 : _sizePbs13, card,
				image);

			return result;
		}

		private void EnableMoves(IPlayer player, bool enable, ICard[] moves = null)
		{
			//GET PBS PLAYER
			IEnumerable<PictureBox> playerCards = this.FindControls<PictureBox>(x => player.Cards.Any(y => y == x.Tag));
			Point playerCardsStartPaintingPoint = GetPlayerCardsStartPaintingPoint(player, playerCards.Count());

			foreach (PictureBox playerCard in playerCards)
			{
				HookPlayerEventsToPictureBox(playerCard, false);
				playerCard.PerformSafely(x =>
				{
					x.Enabled = false;
					x.Top = playerCardsStartPaintingPoint.Y;
					x.Cursor = Cursors.WaitCursor;
				});
			}
			if (enable && moves != null)
			{
				IEnumerable<PictureBox> pictureBoxs = playerCards.Where(x => moves.Any(y => y == x.Tag));
				foreach (PictureBox source in pictureBoxs)
				{
					HookPlayerEventsToPictureBox(source, true);

					source.PerformSafely(x =>
					{
						x.Enabled = true;
						x.Top = playerCardsStartPaintingPoint.Y - 20;
						x.Cursor = Cursors.Hand;
					});
				}
			}

			lblInfo.PerformSafely(x => x.Visible = enable);
		}

		#region Nested type: UserCardSelection

		private class UserCardSelection
		{
			public UserCardSelection(ICard card, bool peta)
			{
				Card = card;
				Peta = peta;
			}

			public ICard Card { get; private set; }
			public bool Peta { get; private set; }
		}

		#endregion
	}
}
