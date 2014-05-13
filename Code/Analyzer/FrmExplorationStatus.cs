using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Subasta.Client.Common;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Media;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;

namespace Analyzer
{
	internal partial class FrmExplorationStatus : Form
	{

		private PictureBox[][] _pbCards = new PictureBox[4][];

		private readonly IGameHandler _GameHandler;
		private readonly IFiguresCatalog _figuresCatalog;
		private DataTable _tableStatus;
		private DataTable _tableSaysStatus;

		public FrmExplorationStatus(IGameHandler GameHandler, IDeck deck,IFiguresCatalog figuresCatalog,IResourceReadingUtils imagesLoader)
		{

			InitializeComponent();
			imagesLoader.LoadCardImages(imageListCards, new Size(36, 54));
			LoadPictureBoxControls();
			_GameHandler = GameHandler;
			_figuresCatalog = figuresCatalog;

			SubscribeToGameEvents();


			InitializeMovesDataStructure();
			InitializeSaysDataStructure();

			dgvStatus.DataSource = _tableStatus;
			dgvSaysStatus.DataSource = _tableSaysStatus;
		}


		private void SubscribeToGameEvents()
		{
			_GameHandler.GameStatusChanged += _GameHandler_GameStatusChanged;
			_GameHandler.GameStarted += _GameHandler_GameStarted;
			_GameHandler.GameCompleted += _GameHandler_GameCompleted;

			_GameHandler.HumanPlayerMoveSelectionNeeded += _GameHandler_HumanPlayerMoveSelectionNeeded;
			_GameHandler.HumanPlayerDeclarationSelectionNeeded += _GameHandler_HumanPlayerDeclarationSelectionNeeded;
			_GameHandler.HumanPlayerSayNeeded += _GameHandler_HumanPlayerSayNeeded;
			_GameHandler.HumanPlayerTrumpNeeded += _GameHandler_HumanPlayerTrumpNeeded;
			_GameHandler.GameSaysStarted += _GameHandler_GameSaysStarted;
			_GameHandler.GameSaysCompleted += _GameHandler_GameSaysCompleted;
			_GameHandler.GameSaysStatusChanged += _GameHandler_GameSaysStatusChanged;
		}

		ISuit _GameHandler_HumanPlayerTrumpNeeded(IHumanPlayer source)
		{
			ISuit result;
			DialogResult dialogResult;
			string stringResult;
			do
			{
				dialogResult = InputBox.Show(string.Format("Select trump {0}", source.Name), "Values:[O-C-E-B]", out stringResult);
				try
				{
					result = Suit.FromId(stringResult[0]);
				}
				catch(Exception ex)
				{
					result = null;
				}
			} while (dialogResult != DialogResult.OK || result == null);

			return result;

		}

		IFigure _GameHandler_HumanPlayerSayNeeded(IHumanPlayer source,ISaysStatus saysStatus)
		{
			IFigure result;
			DialogResult dialogResult;
			string stringResult;
			do
			{
				dialogResult = InputBox.Show(string.Format("Select points {0}", source.Name), "Values:[0-25]", out stringResult);
				try
				{
					result = _figuresCatalog.GetFigureJustPoints(int.Parse(stringResult));
				}
				catch
				{
					result = null;
				}
			} while (dialogResult != DialogResult.OK || result == null);

			return result;
		}


		Declaration? _GameHandler_HumanPlayerDeclarationSelectionNeeded(IHumanPlayer source, Declaration[] availableDeclarations, IExplorationStatus status)
		{
			string declarations = string.Join("-", availableDeclarations.Select(x => x.ToString()));
			string declarationSelected;
			Declaration parsedDeclaration;
			while (InputBox.Show(string.Format("Select declaration {0}", source.Name), declarations, out declarationSelected) !=
				   DialogResult.OK || (!Enum.TryParse( declarationSelected,true,out parsedDeclaration)&& declarationSelected!="Mate" ) )
			{
				MessageBox.Show(this, "must select a valid value");
			}

			return declarationSelected == "Mate" ? null : (Declaration?) parsedDeclaration;

		}

		private ICard _GameHandler_HumanPlayerMoveSelectionNeeded(IHumanPlayer source, ICard[] validMoves,out bool peta)
		{
			ICard result;

			string moves = string.Join("-", validMoves.Select(x => x.ToShortString()));
			string moveSelected;
			while (InputBox.Show(string.Format("Select move {0}", source.Name), moves, out moveSelected) != DialogResult.OK)
			{
				MessageBox.Show(this, "must select a valid value");
			}

			result = new Card(moveSelected);

			DialogResult dialogResult = MessageBox.Show(this,"Peta?","Petar",MessageBoxButtons.YesNo);

			peta = dialogResult == DialogResult.Yes;

			return result;
		}

		private void _GameHandler_GameCompleted(IExplorationStatus status)
		{
			_GameHandler_GameStatusChanged(status);
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

		

		private void _GameHandler_GameStarted(Subasta.Domain.Game.IExplorationStatus status)
		{
			PaintCards(status);
			_tableStatus.Rows.Clear();

			lblFirstPlayer.Text = "First player #: " + _GameHandler.FirstPlayer;
			lblTrump.Text = "Trump: " + _GameHandler.Trump.Name;
			lblPlayerBets.Text = "Team Bets#: " + _GameHandler.TeamBets;
			Invalidate(true);
			Update();
		}

		private void PaintCards(IExplorationStatus status)
		{
			for (byte i = 1; i <= 4; i++)
			{
				int indexCard = 0;
				foreach (var playerCard in status.PlayerCards(i))
				{
					var image = imageListCards.Images[playerCard.ToShortString()];

					_pbCards[i - 1][indexCard++].Image = image;
				}
			}
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
			
		}


		

		private void _GameHandler_GameStatusChanged(IExplorationStatus status)
		{

			IHand lastCompletedHand = status.LastCompletedHand;
			if (lastCompletedHand != null)
			{
				DataRow row = _tableStatus.Rows[lastCompletedHand.Sequence - 1];
				
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
				playerCard = currentHand.PlayerCardResolve(i);
				if (playerCard != null)
					dataRow["Player" + i] = playerCard.ToShortString();
			}


			dataRow["Points"] = currentHand.Points;
			//TODO: FIX
			//dataRow["Declaration"] = currentHand.Declaration.HasValue ? currentHand.Declaration.Value.ToString() : "No";

			int turn = status.Turn - 1;
			if (turn == 0) turn = 4;

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
				lblTeamWins.Text = string.Format("Trump: {3} - Team Bets: {2} - Points Bet:{1} - Team Winner: {0}", status.TeamWinner.ToString(), status.PointsBet,status.TeamBets,status.Trump);

				
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
					try
						{
						row.Cells["Player" + playerStarts].Style.BackColor =
							row.Cells["T" + playerStarts].Style.BackColor = Color.LightPink;
						}
					catch { }
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

		

		void _GameHandler_GameSaysCompleted(ISaysStatus status)
		{
			MessageBox.Show(this, "Marque completado");
		}

		void _GameHandler_GameSaysStarted(ISaysStatus status)
		{
			PaintCards(status.OriginalStatus);


			_tableSaysStatus.Rows.Clear();

			dgvSaysStatus.Update();
		}


		void _GameHandler_GameSaysStatusChanged(ISaysStatus status)
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