using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Subasta.Client.Common;
using Subasta.Domain.Deck;

namespace Analyzer
{
	internal partial class FrmExplorationStatus : Form
	{
		private PictureBox[][] _pbCards = new PictureBox[4][];

		private readonly IGameSimulator _gameSimulator;
		private DataTable _tableStatus;
		public FrmExplorationStatus(IGameSimulator gameSimulator, IDeck deck)
		{
			InitializeComponent();
			LoadImages(deck);
			LoadPictureBoxControls();
			_gameSimulator = gameSimulator;
			_gameSimulator.GameStatusChanged += _gameSimulator_GameStatusChanged;
			_gameSimulator.GameStarted += _gameSimulator_GameStarted;
			InitializeDataStructure();
			dataGridView1.DataSource = _tableStatus;
			

		}

		private void LoadPictureBoxControls()
		{

			for (int i = 0; i < 4; i++)
			{
				_pbCards[i] = new PictureBox[10];
				int left = label1.Left + label1.Width;
				for (int j = 0; j < 10; j++)
				{

					Label theLabel = null;
					switch (i)
					{
						case 0:
							theLabel = label1;
							break;
						case 1:
							theLabel = label2;
							break;
						case 2:
							theLabel = label3;
							break;
						case 3:
							theLabel = label4;
							break;

					}
					var pb = new PictureBox
					         	{
					         		SizeMode = PictureBoxSizeMode.StretchImage,
									 Top = theLabel.Top,
									 Left = left,
									 Width = 36,
									 Height = 54
					         	};

					_pbCards[i][j] = pb;
									
					grpStatus.Controls.Add(pb);
					left += pb.Width;

				}
			}
		}

		private void LoadImages(IDeck deck)
		{
			foreach (var card in deck.Cards.Cards)
			{

				string cardResourceName = GetCardResourceName(card);
				using (var manifestResourceStream = GetType().Assembly.GetManifestResourceStream(cardResourceName))
					imageListCards.Images.Add(card.ToShortString(),
					                          Image.FromStream(manifestResourceStream));
			}
			imageListCards.ImageSize = new Size(36, 54);

		}

		private string GetCardResourceName(ICard card)
		{
			return string.Format("Analyzer.Content.{0}_{1}s.jpg", card.Suit.Name.ToLower(), card.Number);
		}


		private void _gameSimulator_GameStarted(Subasta.Domain.Game.IExplorationStatus status)
		{
			_tableStatus.Rows.Clear();
			AddNewRow();

			for (int i = 1; i <= 4; i++)
			{
				int indexCard = 0;
				foreach (var playerCard in status.PlayerCards(i))
				{
					var image = imageListCards.Images[playerCard.ToShortString()];

					_pbCards[i-1][indexCard++].Image = image;
				}
			}
		}

		private void InitializeDataStructure()
		{
			_tableStatus=new DataTable("Game");
			_tableStatus.Columns.Add("Sequence", typeof (int));
			_tableStatus.Columns.Add("FirstPlayer");
			_tableStatus.Columns.Add("Player1");
			_tableStatus.Columns.Add("Player2");
			_tableStatus.Columns.Add("Player3");
			_tableStatus.Columns.Add("Player4");
			_tableStatus.Columns.Add("TrickWinner" );
			_tableStatus.Columns.Add("Points");
			_tableStatus.Columns.Add("Declaration");
		}

		private void _gameSimulator_GameStatusChanged(Subasta.Domain.Game.IExplorationStatus status)
		{
			DataRow dataRow = _tableStatus.Rows[_tableStatus.Rows.Count - 1];
			dataRow["Sequence"] = status.CurrentHand.Sequence;
			dataRow["FirstPlayer"] = status.CurrentHand.FirstPlayer;
			dataRow["Player1"] = status.CurrentHand.PlayerCard(1);
			dataRow["Player2"] = status.CurrentHand.PlayerCard(2);
			dataRow["Player3"] = status.CurrentHand.PlayerCard(3);
			dataRow["Player4"] = status.CurrentHand.PlayerCard(4);
			dataRow["TrickWinner"] = status.CurrentHand.PlayerWinner;
			dataRow["Points"] = status.CurrentHand.Points;
			dataRow["Declaration"] = status.CurrentHand.Declaration.HasValue ? status.CurrentHand.Declaration.Value.ToString() : "No";
			if (status.CurrentHand.IsCompleted && !status.IsCompleted)
				AddNewRow();
		}

		private void AddNewRow()
		{
			_tableStatus.Rows.Add(-1, null, null, null, null, -1, -1, null);
		}
	}
}