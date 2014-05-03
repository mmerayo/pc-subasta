using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Images;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Lib.Interaction;

namespace Subasta.Lib
{
	public partial class FrmGame : Form
	{
		private readonly IGameSetHandler _gameSetHandler;
		private readonly IUserInteractionManager _userInteractionManager;
		private IImagesLoader _imagesLoader;
		private ICard _lastCardPlayed;
		private Size _sizePbs13 = new Size(50, 70);
		private Size _sizePbs24 = new Size(70, 50);


		public FrmGame(IGameSetHandler gameSetHandler, IImagesLoader imagesLoader,
		               IUserInteractionManager userInteractionManager)
		{
			_gameSetHandler = gameSetHandler;
			_userInteractionManager = userInteractionManager;
			InitializeComponent();

			LoadImages(imagesLoader);

			SuscribeToEvents();

			ConfigureControlsAndLocations();
		}


		private void ConfigureControlsAndLocations()
		{
			pb1.Location = new Point((Width/2) - pb1.Width/2, Height - pb1.Height);
			pb3.Location = new Point((Width/2) - pb3.Width/2, 0);
			pb2.Location = new Point(Width - pb2.Width, (Height/2) - pb2.Height/2);
			pb4.Location = new Point(0, (Height/2) - pb4.Height/2);

			Image image = _imagesLoader.GetImage("Player.png");
			pb1.Image = pb2.Image = pb3.Image = pb4.Image = image;
			pb1.SizeMode = pb2.SizeMode = pb3.SizeMode = pb4.SizeMode = PictureBoxSizeMode.StretchImage;

			this.lblInfo.Left = 0;
			lblInfo.Top = this.Height - lblInfo.Height;
			
		}

		private void LoadImages(IImagesLoader imagesLoader)
		{
			_imagesLoader = imagesLoader;
			_imagesLoader.LoadCardImages(imageList, new Size(50, 70));
		}

		private void SuscribeToEvents()
		{
		_gameSetHandler.GameStarted += new StatusChangedHandler(_gameSetHandler_GameStarted);

			IGameHandler gameHandler = _gameSetHandler.GameHandler;
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

		void _gameSetHandler_GameStarted(IExplorationStatus status)
		{
			
		} 

		void gameHandler_GamePlayerPeta(IPlayer player, IExplorationStatus status)
		{
			
		}

		void gameHandler_DeclarationEmit(IPlayer player, Domain.Declaration declaration)
		{
			PictureBox target = this.FindControls<PictureBox>(x => x.Name == "pb" + player.PlayerNumber).First();

			string text = declaration.ToString().SeparateCamelCase() + "!!";
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
			this.PerformSafely(x=> balloonInfo.Show(text,(IWin32Window)target));
			
			Thread.Sleep(showLenght);
			this.PerformSafely(x => balloonInfo.Hide(target));
		}

		private void GameHandler_TurnChanged(int turn)
		{
			foreach (PictureBox pb in this.FindControls<PictureBox>(x => x.Name.StartsWith("pb")))
			{
				pb.PerformSafely(x => x.Visible = false);
			}
			this.FindControls<PictureBox>(x => x.Name == "pb" + turn).First().PerformSafely(x => x.Visible = true);
		}

		private void GameHandler_GameCompleted(IExplorationStatus status)
		{
			Thread.Sleep(TimeSpan.FromSeconds(5));
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
			if (lastCardPlayed != _lastCardPlayed) //due to defect  as the event is triggered twice
			{
				MoveCard(status, status.LastPlayerMoved, lastCardPlayed);
				_lastCardPlayed = lastCardPlayed;
				// WAIT TIME SO THE USER CAN SEE it
				Thread.Sleep(TimeSpan.FromSeconds(1));
			}
		}


		private void InitializePictureBoxes(IPlayer player)
		{
			Point startPoint = Point.Empty;

			startPoint = GetPlayerCardsStartPaintingPoint(player, player.Cards.Length);
			Image imgReverso = imageList.Images["reverso"];
			var imgReversoV = (Image) imgReverso.Clone();
			imgReversoV.RotateFlip(RotateFlipType.Rotate90FlipX);
			for (int index = 0; index < player.Cards.Length; index++)
			{
				Size size = Size.Empty;
				Point location = startPoint;

				switch (player.PlayerNumber)
				{
					case 1:
					case 3:
						location.X += (_sizePbs13.Width)*index;
						size = _sizePbs13;
						break;

					case 2:
					case 4:
						location.Y += (_sizePbs24.Height)*index;
						size = _sizePbs24;
						imgReverso = imgReversoV;
						break;
				}

				ICard card = player.Cards[index];
				Image image = player.PlayerNumber == 1 ? (Image) imageList.Images[card.ToShortString()].Clone() : imgReverso;
				PictureBox control = CreatePictureBoxControl(location, size, card, image);
			}


			//

			Invalidate();
		}

		private Point GetPlayerCardsStartPaintingPoint(IPlayer player, int numCards)
		{
			Point startPoint;
			var centerPoint = new Point(Width/2, Height/2);
			switch (player.PlayerNumber)
			{
				case 1:
					startPoint = new Point(centerPoint.X - (_sizePbs13.Width*numCards)/2, Size.Height - pb1.Height - _sizePbs13.Height);
					break;

				case 2:
					startPoint = new Point(Width - _sizePbs24.Width - pb2.Width, centerPoint.Y - ((_sizePbs24.Height)*numCards)/2);
					break;

				case 3:
					startPoint = new Point(centerPoint.X - (_sizePbs13.Width*numCards)/2, pb3.Height);
					break;

				case 4:
					startPoint = new Point(pb4.Width, centerPoint.Y - ((_sizePbs24.Height)*numCards)/2);
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
			var pictureBox = (PictureBox) sender;
			bool peta = e.Button == MouseButtons.Right;

			_userInteractionManager.InputProvided(() =>
			                                      {
			                                      	EnableMoves(_gameSetHandler.GameHandler.Player1, false);
			                                      	return new UserCardSelection((ICard) pictureBox.Tag, peta);
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
				this.PerformSafely(x => x.Controls.Remove(pb));
			}
		}

		private void RemovePictureBoxesByCard(ICard card)
		{
			IEnumerable<Control> pbs = this.FindControls<PictureBox>(x => x.Tag == card).ToArray();
			foreach (Control pb in pbs)
			{
				this.PerformSafely(x => x.Controls.Remove(pb));
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
		}

		private void CompactPlayerCards(IExplorationStatus status, int playerNumber)
		{
			IPlayer player = _gameSetHandler.GameHandler.GetPlayer(playerNumber);
			ICard[] playerCards = status.PlayerCards(playerNumber);
			Point startPoint = GetPlayerCardsStartPaintingPoint(player, playerCards.Count());
			int index = 0;
			foreach (ICard card in playerCards)
			{
				Point location = startPoint;

				switch (player.PlayerNumber)
				{
					case 1:
					case 3:
						location.X += (_sizePbs13.Width)*index;
						break;

					case 2:
					case 4:
						location.Y += (_sizePbs24.Height)*index;
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
			var centerPoint = new Point(Width/2, Height/2);
			var destination = new Point(0, 0);

			//set x
			switch (playerNumber)
			{
				case 1:
				case 3:
					destination.X = centerPoint.X - (_sizePbs13.Width/2);
					break;
				case 4:
					destination.X = centerPoint.X - (_sizePbs13.Width/2) - _sizePbs24.Width;
					break;
				case 2:
					destination.X = centerPoint.X + (_sizePbs13.Width/2);
					break;
			}
			//set Y
			switch (playerNumber)
			{
				case 1:
					destination.Y = centerPoint.Y + (_sizePbs24.Height/2);
					break;
				case 3:
					destination.Y = centerPoint.Y - (_sizePbs24.Height/2) - _sizePbs13.Height;
					break;
				case 2:
				case 4:
					destination.Y = centerPoint.Y - (_sizePbs24.Height/2);
					break;
			}

			//image
			var image = (Image) imageList.Images[card.ToShortString()].Clone();
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

			lblInfo.PerformSafely(x=>x.Visible=enable);
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