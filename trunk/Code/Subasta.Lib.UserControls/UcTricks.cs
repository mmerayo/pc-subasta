using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StructureMap;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Images;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.Lib.UserControls
{
	public partial class UcTricks : UserControl, ICustomUserControl
	{
		private const string Pbdeclaration = "pbDeclaration";
		private const string Pbwinner = "pbWinner";
		private IGameSetHandler _gameSetHandler;
		private IMediaProvider _mediaProvider;

		public UcTricks()
		{
			InitializeComponent();
		}

		#region ICustomUserControl Members

		public void Initialize()
		{
			_gameSetHandler = ObjectFactory.GetInstance<IGameSetHandler>();
			_mediaProvider = ObjectFactory.GetInstance<IMediaProvider>();

			LoadPbPics();

			IGameHandler gameHandler = _gameSetHandler.GameHandler;
			gameHandler.HandCompleted += GameHandler_HandCompleted;
			gameHandler.GameSaysStarted += gameHandler_GameSaysStarted;
			gameHandler.GameStarted += gameHandler_GameStarted;
			_gameSetHandler.GameHandler.GameStatusChanged += GameHandler_GameStatusChanged;
		}

		#endregion

		private void LoadPbPics()
		{
			IEnumerable<PictureBox> pbs = this.FindControls<PictureBox>(c => c.Name.StartsWith("pictureBox"));
			foreach (PictureBox pb in pbs)
			{
				pb.Image = _mediaProvider.GetImage(GameMediaType.Reverso);
			}

			pbs = this.FindControls<PictureBox>(c => c.Name.StartsWith(Pbwinner));
			foreach (PictureBox pb in pbs)
			{
				pb.Image = _mediaProvider.GetImage(GameMediaType.Winner);
			}

			pb1.Image = _mediaProvider.GetImage(GameMediaType.Jugador1);
			pb2.Image = _mediaProvider.GetImage(GameMediaType.Jugador2);
			pb3.Image = _mediaProvider.GetImage(GameMediaType.Jugador3);
			pb4.Image = _mediaProvider.GetImage(GameMediaType.Jugador4);
		}

		private void GameHandler_GameStatusChanged(IExplorationStatus status)
		{
			lblPtos1.PerformSafely(x => x.Text = status.SumTotalTeam(1).ToString());
			lblPtos2.PerformSafely(x => x.Text = status.SumTotalTeam(2).ToString());
		}

		private void gameHandler_GameStarted(IExplorationStatus status)
		{
			this.PerformSafely(x =>
			                   {
			                   	x.Visible = true;
			                   	var pbs = this.FindControls<PictureBox>(c => c.Name.StartsWith("pictureBox"));
			                   	foreach (PictureBox pb in pbs)
			                   	{
			                   		pb.Visible = false;
			                   		toolTip1.SetToolTip(pb, string.Empty);
			                   	}

			                   	pbs = this.FindControls<PictureBox>(c => c.Name.StartsWith(Pbwinner)||c.Name.StartsWith(Pbdeclaration));
			                   	foreach (PictureBox pb in pbs)
			                   	{
			                   		pb.Dispose();
			                   	}
			                   });
		}

		private void gameHandler_GameSaysStarted(ISaysStatus status)
		{
			this.PerformSafely(x =>
			                   {
			                   	x.Visible = false;
			                   	lblPtos1.Text = lblPtos2.Text = "0";
			                   });
		}

		private void GameHandler_HandCompleted(IExplorationStatus status)
		{
			this.PerformSafely(x => PaintTrick(status));
		}

		private void PaintTrick(IExplorationStatus status)
		{

			var pbBaza = this.FindControl<PictureBox>("pictureBox" + status.LastCompletedHand.Sequence);
			pbBaza.Visible = true;
			toolTip1.SetToolTip(pbBaza,GetBazaTip(status.LastCompletedHand));
			//winner

			PictureBox pbLeftRef, pbRightRef;
			pbLeftRef = pbRightRef = pbBaza;

			if (status.LastCompletedHand.TeamWinner == 1)
			{
			pbLeftRef = CreatePbOnLeft(pbLeftRef, pbBaza.Size, _mediaProvider.GetImage(GameMediaType.Winner), Pbwinner);
				toolTip1.SetToolTip(pbLeftRef,"Pareja 1 gana la baza");
				if (status.LastCompletedHand.Declaration.HasValue)
				{
					pbLeftRef = CreatePbOnLeft(pbLeftRef, pbBaza.Size, _mediaProvider.GetImage(GameMediaType.CanteRealizado),Pbdeclaration);
					toolTip1.SetToolTip(pbLeftRef, status.LastCompletedHand.Declaration.ToString().SeparateCamelCase());
				}
			}
			else
			{
				pbRightRef = CreatePbOnRight(pbRightRef, pbBaza.Size,
											 _mediaProvider.GetImage(GameMediaType.Winner), Pbwinner);
				toolTip1.SetToolTip(pbRightRef, "Pareja 2 gana la baza");
				if (status.LastCompletedHand.Declaration.HasValue)
					{
					pbRightRef = CreatePbOnRight(pbRightRef, pbBaza.Size, _mediaProvider.GetImage(GameMediaType.CanteRealizado), Pbdeclaration);
					toolTip1.SetToolTip(pbRightRef, status.LastCompletedHand.Declaration.ToString().SeparateCamelCase());
					}
			}



		}

		private string GetBazaTip(IHand lastCompletedHand)
		{
			ICard[] array = lastCompletedHand.CardsByPlaySequence().ToArray();
			string result = string.Format("{0}{4}{1}{4}{2}{4}{3}{4}{5}", array[0], array[1], array[2],
				array[3], Environment.NewLine, lastCompletedHand.Declaration.HasValue?lastCompletedHand.Declaration.ToString().SeparateCamelCase():string.Empty);
		    return result;
		}

		private PictureBox CreatePbOnLeft(PictureBox currLeftRef, Size size, Image image,string prefix)
		{
		return CreatePictureBoxControl(new Point(currLeftRef.Left - currLeftRef.Width - 3, currLeftRef.Top), size, image,
			                               prefix+ Guid.NewGuid().ToString().Replace("-", "_"));
		}

		private PictureBox CreatePbOnRight(PictureBox currRightRef, Size size, Image image, string prefix)
		{
			return CreatePictureBoxControl(new Point(currRightRef.Left + currRightRef.Width + 3,currRightRef.Top), size, image,
			                               prefix+ Guid.NewGuid().ToString().Replace("-", "_"));
		}

		private PictureBox CreatePictureBoxControl(Point location, Size size, Image image, string name = "")
		{
			var current = new PictureBox
			              {
			              	Location = location,
			              	Size = size,
			              	Name = name != string.Empty ? name : Guid.NewGuid().ToString().Replace("-", "_"),
			              	Image = image,
			              	SizeMode = PictureBoxSizeMode.StretchImage,
			              };

			this.PerformSafely(x => x.Controls.Add(current));

			return current;
		}
	}
}