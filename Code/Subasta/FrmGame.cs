﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Images;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Extensions;
using Subasta.Interaction;

namespace Subasta
{
	public partial class FrmGame : Form
	{
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

		private readonly IGameSetHandler _gameSetHandler;
		private readonly IUserInteractionManager _userInteractionManager;
		private IImagesLoader _imagesLoader;
		private DataTable tblMarques = new DataTable();
		Size _sizePbs13 = new Size(50, 70);
		Size _sizePbs24 = new Size(70, 50);

		private List<PictureBox> _pbs = new List<PictureBox>(40);

		public FrmGame(IGameSetHandler gameSetHandler, IImagesLoader imagesLoader,IUserInteractionManager userInteractionManager)
		{
			_gameSetHandler = gameSetHandler;
			_userInteractionManager = userInteractionManager;
			InitializeComponent();

			LoadImages(imagesLoader);

			InitializeTableMarques();

			SuscribeToEvents();
		}

		private void LoadImages(IImagesLoader imagesLoader)
		{
			_imagesLoader = imagesLoader;
			_imagesLoader.LoadImages(imageList, new Size(50, 70));
		}

		private void SuscribeToEvents()
		{
			_gameSetHandler.GameHandler.GameSaysStarted += GameHandler_GameSaysStarted;
			_gameSetHandler.GameHandler.HumanPlayerMoveSelectionNeeded += GameHandler_HumanPlayerMoveSelectionNeeded;
			_gameSetHandler.GameHandler.GameStatusChanged += GameHandler_GameStatusChanged;
			_gameSetHandler.GameHandler.HandCompleted += GameHandler_HandCompleted;
			
		}

		void GameHandler_HandCompleted(IExplorationStatus status)
		{
			throw new NotImplementedException();
			//TODO: CLEAN TABLE
			//CREATE BAZA

		}

		void GameHandler_GameStatusChanged(IExplorationStatus status)
		{
			MoveCard(status.LastPlayerMoved,status.LastCardPlayed);
		}
		
		private void InitializeTableMarques()
		{
			tblMarques.Columns.Add("Jugador");
			tblMarques.Columns.Add("Marque");
			tblMarques.Columns.Add("Suma");
		}

		private void InitializePictureBoxes(IPlayer player)
		{
			

			Point startPoint = Point.Empty;

			switch (player.PlayerNumber)
			{
				case 1:
					startPoint = new Point(_sizePbs24.Width, Size.Height - 30 - _sizePbs13.Height);
					break;

				case 2:
					startPoint = new Point(Width - _sizePbs24.Width - 12, _sizePbs13.Height + 20);
					break;

				case 3:
					startPoint = new Point(_sizePbs24.Width, 12);
					break;

				case 4:
					startPoint = new Point(20, _sizePbs13.Height + 20);
					break;
			}
			Image imgReverso = imageList.Images["reverso"];
			Image imgReversoV = (Image) imgReverso.Clone();
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
						location.Y += (_sizePbs24.Height/2)*index;
						size = _sizePbs24;
						imgReverso = imgReversoV;
						break;
				}

				var card = player.Cards[index];
				Image image = player.PlayerNumber == 1 ? (Image) imageList.Images[card.ToShortString()].Clone() : imgReverso;
				var control = CreatePictureBoxControl(location, size, card, image);

				_pbs.Add(control);

			}


			//

			Invalidate();
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
			};

			this.PerformSafely(x => x.Controls.Add(current));
			return current;
		}


		private void GameHandler_GameSaysStarted(ISaysStatus status)
		{
			ClearAllPictureBoxControls();
			InitializePictureBoxes(_gameSetHandler.GameHandler.Player1);
			InitializePictureBoxes(_gameSetHandler.GameHandler.Player2);
			InitializePictureBoxes(_gameSetHandler.GameHandler.Player3);
			InitializePictureBoxes(_gameSetHandler.GameHandler.Player4);

			PaintFirstPlayer();

		}

		private void ClearAllPictureBoxControls()
		{
			PictureBox[] pictureBoxs = _pbs.ToArray();
			foreach (var pictureBox in pictureBoxs)
			{
				RemovePictureBox(pictureBox);
			}

		}

		private void RemovePictureBox(PictureBox pictureBox)
		{
			this.PerformSafely(x => x.Controls.Remove(pictureBox));
			_pbs.Remove(pictureBox);
		}

		private void PaintFirstPlayer()
		{
		}

		ICard GameHandler_HumanPlayerMoveSelectionNeeded(IHumanPlayer source, ICard[] validMoves, out bool peta)
		{
			EnableMoves(source,true, validMoves);
			var selection=_userInteractionManager.WaitUserInput<UserCardSelection>();
			MoveCard(source.PlayerNumber, selection.Card);
			EnableMoves(source, false);
			peta = selection.Peta;
			return selection.Card;
		}

		private void MoveCard(int playerNumber, ICard card)
		{

			PictureBox pbCard = PaintCardPlayed(playerNumber, card);// _pbs.Single(x => x.Name == card.ToShortString());
			
			//Remove from hand
			RemovePictureBox( _pbs.Single(x => x.Name == card.ToShortString()));
			//PAINT REMAINING CARDS
			CompactPlayerCards(playerNumber);
			// WAIT TIME SO THE USER CAN SEE THEM
			Thread.Sleep(TimeSpan.FromSeconds(1));

		}

		private void CompactPlayerCards(int playerNumber)
		{
			//TODO:
		}

		private PictureBox PaintCardPlayed(int playerNumber, ICard card)
		{
			Point centerPoint=new Point(this.Width/2,this.Height/2);
			Point destination=new Point(0,0);

			//set x
			switch (playerNumber)
			{
				case 1:
				case 3:
					destination.X = centerPoint.X - (_sizePbs13.Width/2);
					break;
				case 2:
					destination.X = centerPoint.X - (_sizePbs13.Width/2) - _sizePbs24.Width;
					break;
				case 4:
					destination.X = centerPoint.X + (_sizePbs13.Width/2);
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
			var image = (Image) imageList.Images[card.ToShortString()].Clone();
			switch (playerNumber)
			{
				case 2:
					image.RotateFlip(RotateFlipType.Rotate90FlipY);
					break;
				case 3:
					image.RotateFlip(RotateFlipType.Rotate180FlipY);
					break;
				case 4:
					image.RotateFlip(RotateFlipType.Rotate270FlipY);
					break;
			}

			var result = CreatePictureBoxControl(destination, playerNumber == 2 || playerNumber == 4 ? _sizePbs24 : _sizePbs13,
				card, image);

			return result;
		}

		private void EnableMoves(IPlayer player, bool enable, ICard[] moves = null)
		{
			//GET PBS PLAYER
			var playerCards = _pbs.Where(x => player.Cards.Any(y=>y.ToShortString()==x.Name));
			foreach (var playerCard in playerCards)
			{
				playerCard.Enabled = false;
			}
			if (enable && moves != null)
			{
				IEnumerable<PictureBox> pictureBoxs = playerCards.Where(x=>moves.Any(y=>y.ToShortString()==x.Name));
				foreach (var source in pictureBoxs)
				{
					source.Enabled = true;
				}
			}
		}


		private string NormalizeSay(SayKind say)
		{
			return say.ToString().SeparateCamelCase(" ");
		}


	}
}
