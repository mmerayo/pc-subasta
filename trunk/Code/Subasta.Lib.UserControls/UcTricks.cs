using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using StructureMap;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Images;
using Subasta.Domain.Game;

namespace Subasta.Lib.UserControls
{
	public partial class UcTricks : UserControl, ICustomUserControl
	{
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

			pbs = this.FindControls<PictureBox>(c => c.Name.StartsWith("pbWinner"));
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
			                   	}

			                   	pbs = this.FindControls<PictureBox>(c => c.Name.StartsWith("pbWinner"));
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

			//winner

			PictureBox pbLeftRef, pbRightRef;
			pbLeftRef = pbRightRef = pbBaza;

			if (status.LastCompletedHand.TeamWinner == 1)
			{
				pbLeftRef = CreatePbOnLeft(pbLeftRef, pbBaza.Size, _mediaProvider.GetImage(GameMediaType.Winner));
				toolTip1.SetToolTip(pbLeftRef,"Pareja 1 gana la baza");
			}
			else
			{
				pbRightRef = CreatePbOnRight(pbRightRef, pbBaza.Size,
				                             _mediaProvider.GetImage(GameMediaType.Winner));
				toolTip1.SetToolTip(pbRightRef, "Pareja 2 gana la baza");
			}

		}

		private PictureBox CreatePbOnLeft(PictureBox currLeftRef, Size size, Image image)
		{
		return CreatePictureBoxControl(new Point(currLeftRef.Left - currLeftRef.Width - 3, currLeftRef.Top), size, image,
			                               "pbWinner" + Guid.NewGuid().ToString().Replace("-", "_"));
		}

		private PictureBox CreatePbOnRight(PictureBox currRightRef, Size size, Image image)
		{
			return CreatePictureBoxControl(new Point(currRightRef.Left + currRightRef.Width + 3,currRightRef.Top), size, image,
			                               "pbWinner" + Guid.NewGuid().ToString().Replace("-", "_"));
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