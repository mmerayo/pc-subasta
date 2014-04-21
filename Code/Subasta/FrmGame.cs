using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Images;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta
{
	public partial class FrmGame : Form
	{
		private readonly IGameSetHandler _gameSetHandler;
		private IImagesLoader _imagesLoader;
		DataTable tblMarques = new DataTable();

		List<PictureBox> _pbs=new List<PictureBox>(40); 

		public FrmGame(IGameSetHandler gameSetHandler, IImagesLoader imagesLoader)
		{
			_gameSetHandler = gameSetHandler;
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
			_gameSetHandler.GameHandler.GameSaysStatusChanged += GameHandler_GameSaysStatusChanged;
			_gameSetHandler.GameHandler.HumanPlayerSayNeeded += GameHandler_HumanPlayerSayNeeded;
			_gameSetHandler.GameHandler.HumanPlayerTrumpNeeded += SelectTrump;
			_gameSetHandler.GameHandler.GameSaysStarted += GameHandler_GameSaysStarted;
			_gameSetHandler.GameHandler.GameSaysCompleted += GameHandler_GameSaysCompleted;
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
					startPoint = new Point(sizeV.Width,Size.Height - 30 - sizeH.Height);
					break;

				case 2:
					startPoint = new Point(Width - sizeV.Width - 12, sizeH.Height + 20);
					break;

				case 3:
					startPoint = new Point(sizeV.Width, 12);
					break;

				case 4:
					startPoint = new Point(20, sizeH.Height+20);
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
					location.X += (sizeH.Width) * index;
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
				                     	Image = player.PlayerNumber == 1 ? (Image)imageList.Images[card.ToShortString()].Clone() : imgReverso,
				                     	SizeMode = PictureBoxSizeMode.StretchImage,

				                     };

				_pbs.Add(current);
				Controls.Add(current);
			}


			//

			Invalidate();
		}

		private void GameHandler_GameSaysCompleted(ISaysStatus status)
		{
			throw new NotImplementedException();
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

		private ISuit SelectTrump(IHumanPlayer source)
		{
			throw new NotImplementedException();
		}

		private IFigure GameHandler_HumanPlayerSayNeeded(IHumanPlayer source)
		{
			throw new NotImplementedException();
		}

		private void GameHandler_GameSaysStatusChanged(ISaysStatus status)
		{
			ISay say = status.Says.Last();
			tblMarques.Rows.Add(say.PlayerNum, NormalizeSay(say.Figure.Say), status.PointsBet);

			PaintStatusMarque();

			Application.DoEvents();

		}

		private void PaintStatusMarque()
		{
			throw new NotImplementedException();
		}

		private string NormalizeSay(SayKind say)
		{
			return string.Join(" ", SplitCamelCase(say.ToString()));
		}

		private string[] SplitCamelCase(string source)
		{
			return Regex.Split(source, @"(?<!^)(?=[A-Z])");
		}
	}
}
