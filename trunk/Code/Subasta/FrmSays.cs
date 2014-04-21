using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Images;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta
{
	public partial class FrmSays : Form
	{
		private readonly IGameSetHandler _gameSetHandler;
		private readonly IImagesLoader _imagesLoader;
		private List<PictureBox> _pbs = new List<PictureBox>();

		public FrmSays(IGameSetHandler gameSetHandler, IImagesLoader imagesLoader)
		{
			_gameSetHandler = gameSetHandler;
			InitializeComponent();


			_imagesLoader = imagesLoader;
			_imagesLoader.LoadImages(imageList, new Size(50, 70));

			InitializePictureBoxes();

			_gameSetHandler.GameHandler.GameSaysStatusChanged += GameHandler_GameSaysStatusChanged;
			_gameSetHandler.GameHandler.HumanPlayerSayNeeded += GameHandler_HumanPlayerSayNeeded;
			_gameSetHandler.GameHandler.HumanPlayerTrumpNeeded += SelectTrump;
			_gameSetHandler.GameHandler.GameSaysStarted += GameHandler_GameSaysStarted;
			_gameSetHandler.GameHandler.GameSaysCompleted += GameHandler_GameSaysCompleted;


		}

		private void InitializePictureBoxes()
		{
			//SuspendLayout();
			PictureBox current = pbCard;
			_pbs.Add(pbCard);
			for (int i = 1; i < 10; i++)
			{
				var newPb = new PictureBox
				            {
				            	Location = new Point(current.Location.X + current.Width, current.Location.Y),
				            	Size = new Size(50, 70),
				            	SizeMode = PictureBoxSizeMode.StretchImage,
				            	Visible = true
				            };
				current = newPb;
				_pbs.Add(newPb);
				grpCards.Controls.Add(newPb);
			}

			//ResumeLayout(false);
			//Invalidate();
		}

		private void ShowCards()
		{

			for (int index = 0; index < _gameSetHandler.GameHandler.Player1.Cards.Length; index++)
			{
				var card = _gameSetHandler.GameHandler.Player1.Cards[index];
				_pbs[index].Image = imageList.Images[card.ToShortString()];
			}
		}

		private void GameHandler_GameSaysCompleted(ISaysStatus status)
		{
			throw new NotImplementedException();
		}

		private void GameHandler_GameSaysStarted(ISaysStatus status)
		{
			lblSale.Text = "Sale el Jugador #" + _gameSetHandler.FirstPlayer;
			ShowCards();

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
			var dataTable = new DataTable();
			dataTable.Columns.Add("Jugador");
			dataTable.Columns.Add("Marque");
			dataTable.Columns.Add("Suma");

			ISay say = status.Says.Last();
			dataTable.Rows.Add(say.PlayerNum, NormalizeSay(say.Figure.Say), status.PointsBet);


			dgvMarques.ReadOnly = true;
			dgvMarques.DataSource = dataTable;

			Application.DoEvents();

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