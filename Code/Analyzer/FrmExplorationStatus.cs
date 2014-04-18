using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Subasta.Client.Common;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;

namespace Analyzer
{
	internal partial class FrmExplorationStatus : Form
	{
		private PictureBox[][] _pbCards = new PictureBox[4][];

		private readonly IGameSimulator _gameSimulator;
		private DataTable _tableStatus;
		private DataTable _tableSaysStatus;

		public FrmExplorationStatus(IGameSimulator gameSimulator, IDeck deck)
		{
			InitializeComponent();
			LoadImages(deck);
			LoadPictureBoxControls();
			_gameSimulator = gameSimulator;
			
			SubscribeToGameEvents();


			InitializeMovesDataStructure();
			InitializeSaysDataStructure();

			dgvStatus.DataSource = _tableStatus;
			dgvSaysStatus.DataSource = _tableSaysStatus;
		}


		private void SubscribeToGameEvents()
		{
			_gameSimulator.GameStatusChanged += _gameSimulator_GameStatusChanged;
			_gameSimulator.GameStarted += _gameSimulator_GameStarted;
			_gameSimulator.GameCompleted += _gameSimulator_GameCompleted;

			_gameSimulator.HumanPlayerMoveSelectionNeeded += _gameSimulator_HumanPlayerMoveSelectionNeeded;
			_gameSimulator.HumanPlayerDeclarationSelectionNeeded += _gameSimulator_HumanPlayerDeclarationSelectionNeeded;

			_gameSimulator.GameSaysStarted += _gameSimulator_GameSaysStarted;
			_gameSimulator.GameSaysCompleted += _gameSimulator_GameSaysCompleted;
			_gameSimulator.GameSaysStatusChanged += _gameSimulator_GameSaysStatusChanged;
		}

		
		Declaration? _gameSimulator_HumanPlayerDeclarationSelectionNeeded(IHumanPlayer source, Declaration[] availableDeclarations)
		{
			string declarations = string.Join("-", availableDeclarations.Select(x => x.ToString()));
			string declarationSelected;
			Declaration parsedDeclaration;
			while (InputBox.Show(string.Format("Select declaration {0}", source.Name), declarations, out declarationSelected) !=
				   DialogResult.OK || !Enum.TryParse( declarationSelected,true,out parsedDeclaration))
			{
				MessageBox.Show(this, "must select a valid value");
			}

			return parsedDeclaration;
		}

		private ICard _gameSimulator_HumanPlayerMoveSelectionNeeded(IHumanPlayer source, ICard[] validMoves)
		{
			if (validMoves.Length == 1) return validMoves[0];
			string moves = string.Join("-", validMoves.Select(x => x.ToShortString()));
			string moveSelected;
			while (InputBox.Show(string.Format("Select move {0}", source.Name), moves, out moveSelected) != DialogResult.OK)
			{
				MessageBox.Show(this, "must select a valid value");
			}

			return new Card(moveSelected);
		}

		private void _gameSimulator_GameCompleted(IExplorationStatus status, TimeSpan timeTaken)
		{
			_gameSimulator_GameStatusChanged(status, timeTaken);
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


		private void _gameSimulator_GameStarted(Subasta.Domain.Game.IExplorationStatus status, TimeSpan t)
		{
			_tableStatus.Rows.Clear();

			for (int i = 1; i <= 4; i++)
			{
				int indexCard = 0;
				foreach (var playerCard in status.PlayerCards(i))
				{
					var image = imageListCards.Images[playerCard.ToShortString()];

					_pbCards[i - 1][indexCard++].Image = image;
				}
			}

			lblFirstPlayer.Text = "First player #: " + _gameSimulator.FirstPlayer;
			lblTrump.Text = "Trump: " + _gameSimulator.Trump.Name;
			lblPlayerBets.Text = "Team Bets#: " + _gameSimulator.TeamBets;
			Invalidate(true);
			Update();
		}

		private void InitializeMovesDataStructure()
		{
			_tableStatus = new DataTable("Game");
			_tableStatus.Columns.Add("Sequence", typeof (int));
			_tableStatus.Columns.Add("FirstPlayer");
			_tableStatus.Columns.Add("Player1");
			_tableStatus.Columns.Add("Player2");
			_tableStatus.Columns.Add("Player3");
			_tableStatus.Columns.Add("Player4");
			_tableStatus.Columns.Add("TrickWinner");
			_tableStatus.Columns.Add("Points");
			_tableStatus.Columns.Add("Declaration");
			_tableStatus.Columns.Add("BrokeToTrump", typeof (bool));
			_tableStatus.Columns.Add("T1");
			_tableStatus.Columns.Add("T2");
			_tableStatus.Columns.Add("T3");
			_tableStatus.Columns.Add("T4");
		}


		

		private void _gameSimulator_GameStatusChanged(IExplorationStatus status, TimeSpan timeTaken)
		{

			IHand lastCompletedHand = status.LastCompletedHand;
			if (lastCompletedHand != null)
			{
				DataRow row = _tableStatus.Rows[lastCompletedHand.Sequence - 1];
				for (int i = 1; i < 4; i++)
					if (row["T" + i] == DBNull.Value)
						row["T" + i] = timeTaken.ToString();
				row["Player" + lastCompletedHand.LastPlayer] = lastCompletedHand.CardsByPlaySequence().Last().ToShortString();

				Declaration? declaration = lastCompletedHand.Declaration;
				row["Declaration"] = declaration.HasValue ? declaration.Value.ToString() : "No";
				row["TrickWinner"] = lastCompletedHand.PlayerWinner;
				row["Points"] = lastCompletedHand.Points;
			}
			if (_tableStatus.Rows.Count < status.Hands.Count)
			{
				AddNewStatusRow();
			}
			IHand currentHand = status.CurrentHand;
			DataRow dataRow = _tableStatus.Rows[_tableStatus.Rows.Count - 1];

			dataRow["Sequence"] = currentHand.Sequence;
			dataRow["FirstPlayer"] = currentHand.FirstPlayer;
			ICard playerCard;
			for (int i = 1; i <= 4; i++)
			{
				playerCard = currentHand.PlayerCard(i);
				if (playerCard != null)
					dataRow["Player" + i] = playerCard.ToShortString();
			}


			dataRow["Points"] = currentHand.Points;
			//TODO: FIX
			//dataRow["Declaration"] = currentHand.Declaration.HasValue ? currentHand.Declaration.Value.ToString() : "No";

			int turn = status.Turn - 1;
			if (turn == 0) turn = 4;
			dataRow["T" + turn] = timeTaken.ToString();

			dataRow["BrokeToTrump"] = currentHand.BrokeToTrump;


			//this.dgvStatus.Invalidate(true);
			dgvStatus.Visible = false;
			this.dgvStatus.Update();
			dgvStatus.Visible = true;
			if (status.IsCompleted)
			{
				lblPointsT1.Text = "T1 total: " + status.SumTotalTeam(1);
				lblPointsT2.Text = "T2 total: " + status.SumTotalTeam(2);
				lblDeclarations.Text = status.Hands.Select(x => x.Declaration).Where(x => x.HasValue).Aggregate(string.Empty,
				                                                                                                (current, source) =>
				                                                                                                current + source);

			}
			Application.DoEvents();
		}


		private void AddNewStatusRow()
		{
			_tableStatus.Rows.Add(-1, null, null, null, null, -1, -1, null, null);
		}

		private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			foreach (DataGridViewRow row in dgvStatus.Rows)
			{
				//if(row.Index==0) continue;
				if (row.Cells["FirstPlayer"].Value != null && row.Cells["FirstPlayer"].Value != DBNull.Value)
				{
					var playerStarts = int.Parse(row.Cells["FirstPlayer"].Value.ToString());
					if (playerStarts < 1) break;

					row.Cells["Player" + playerStarts].Style.BackColor =
						row.Cells["T" + playerStarts].Style.BackColor = Color.LightPink;
				}

				if (row.Cells["TrickWinner"].Value != null && row.Cells["TrickWinner"].Value != DBNull.Value)
				{
					var playerWins = int.Parse(row.Cells["TrickWinner"].Value.ToString());
					if (playerWins < 1) break;
					row.Cells["Player" + playerWins].Style.Font = new Font(dgvStatus.Font,
					                                                       FontStyle.Bold);
					row.Cells["Player" + playerWins].Style.ForeColor = Color.Green;
				}
			}
		}

		private void InitializeSaysDataStructure()
		{
			_tableSaysStatus = new DataTable("Says");
			_tableSaysStatus.Columns.Add("Sequence", typeof(int));
			_tableSaysStatus.Columns.Add("PlayerNum", typeof(int));
			_tableSaysStatus.Columns.Add("Figure");
			_tableSaysStatus.Columns.Add("Say");
			_tableSaysStatus.Columns.Add("PointsBet", typeof(int));
			_tableSaysStatus.Columns.Add("CurrentBet", typeof(int));

			_tableSaysStatus.Columns.Add("MarkedItems");

		}

		

		void _gameSimulator_GameSaysCompleted(ISaysStatus status)
		{
			MessageBox.Show(this, "Marque completado");
		}

		void _gameSimulator_GameSaysStarted(ISaysStatus status)
		{
			_tableSaysStatus.Rows.Clear();

			dgvSaysStatus.Update();
		}


		void _gameSimulator_GameSaysStatusChanged(ISaysStatus status)
		{
			AddNewSaysRow();

			DataRow row = _tableSaysStatus.Rows[_tableSaysStatus.Rows.Count-1];

			ISay say = status.Says.Last();
			row["Sequence"] = say.Sequence;
			row["PlayerNum"] = say.PlayerNum;
			row["Figure"] = say.Figure.GetType().Name;
			row["Say"] =  say.Figure.Say;
			row["PointsBet"] = say.Figure.PointsBet;
			row["CurrentBet"] = status.PointsBet;
			row["MarkedItems"] = string.Join("-",status.Says.Last().Figure.MarkedCards.Select(x=>x.ToShortString()));
			dgvSaysStatus.Update();
			Application.DoEvents();

		}

		private void AddNewSaysRow()
		{
			_tableSaysStatus.Rows.Add(-1, -1, null, -1,-1, null);
		}

	}
}