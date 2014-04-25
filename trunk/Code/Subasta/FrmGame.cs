using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
			var sizeH = new Size(50, 70);
			var sizeV = new Size(70, 50);

			Point startPoint = Point.Empty;

			switch (player.PlayerNumber)
			{
				case 1:
					startPoint = new Point(sizeV.Width, Size.Height - 30 - sizeH.Height);
					break;

				case 2:
					startPoint = new Point(Width - sizeV.Width - 12, sizeH.Height + 20);
					break;

				case 3:
					startPoint = new Point(sizeV.Width, 12);
					break;

				case 4:
					startPoint = new Point(20, sizeH.Height + 20);
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
						location.X += (sizeH.Width)*index;
						size = sizeH;
						break;

					case 2:
					case 4:
						location.Y += (sizeV.Height/2)*index;
						size = sizeV;
						imgReverso = imgReversoV;
						break;
				}

				var card = player.Cards[index];
				PictureBox current = new PictureBox
				{
					Location = location,
					Size = size,
					Name = card.ToShortString(),
					Image = player.PlayerNumber == 1 ? (Image) imageList.Images[card.ToShortString()].Clone() : imgReverso,
					SizeMode = PictureBoxSizeMode.StretchImage,

				};

				_pbs.Add(current);
				this.PerformSafely(x=>x.Controls.Add(current));
			}


			//

			Invalidate();
		}

		

		private void GameHandler_GameSaysStarted(ISaysStatus status)
		{
			InitializePictureBoxes(_gameSetHandler.GameHandler.Player1);
			InitializePictureBoxes(_gameSetHandler.GameHandler.Player2);
			InitializePictureBoxes(_gameSetHandler.GameHandler.Player3);
			InitializePictureBoxes(_gameSetHandler.GameHandler.Player4);

			PaintFirstPlayer();

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
			throw new NotImplementedException();
			//TODO: MOVE
			//TODO:PAINT REMAINING CARDS
			//TODO: WAIT TIME SO THE USER CAN SEE THEM
			
		}

		private void EnableMoves(IPlayer player, bool enable, ICard[] moves = null)
		{
			//GET PBS PLAYER
			//if moves !=null disable/enable all otherwise only moves
		}


		private string NormalizeSay(SayKind say)
		{
			return say.ToString().SeparateCamelCase(" ");
		}


	}
}
