using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StructureMap;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Client.Common.Media;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.Lib.UserControls
{
	public partial class UcTrumpSelection : UserControl,ICustomUserControl
	{
		private IUserInteractionManager _interactionManager;
		private IGameSetHandler _gameSetHandler;
		private IMediaProvider _mediaProvider;
		private ISoundPlayer _soundPlayer	;

		public UcTrumpSelection()
		{
			InitializeComponent();
			
		}

		public void Initialize()
		{
			EnableTrumpInteraction(false);
			_interactionManager = ObjectFactory.GetInstance<IUserInteractionManager>();
			_mediaProvider = ObjectFactory.GetInstance<IMediaProvider>();
			_gameSetHandler = ObjectFactory.GetInstance<IGameSetHandler>();
			_soundPlayer=ObjectFactory.GetInstance<ISoundPlayer>();

			LoadSuits();

			_gameSetHandler.GameHandler.HumanPlayerTrumpNeeded += GameHandler_HumanPlayerTrumpNeeded;

		}

		private void LoadSuits()
		{
			LoadSuit(pbOros, 'O');
			LoadSuit(pbCopas, 'C');
			LoadSuit(pbEspadas, 'E');
			LoadSuit(pbBastos, 'B');	
		}

		private void LoadSuit(PictureBox pictureBox, char suitId)
		{
			ISuit suit = Suit.FromId(suitId);
			pictureBox.Image = _mediaProvider.GetCard(suit, 1);
			pictureBox.Tag = suit;
		}


		private void EnableTrumpInteraction(bool enable)
		{
			this.PerformSafely(x=>x.Visible=enable);
		}

		private ISuit GameHandler_HumanPlayerTrumpNeeded(IHumanPlayer source)
		{
			EnableTrumpInteraction(true);
			var result = _interactionManager.WaitUserInput<ISuit>();

			return result;
		}

		private void pb_Click(object sender, EventArgs e)
		{
			_soundPlayer.PlayAsync(GameSoundType.Selection);
			_interactionManager.InputProvided(() =>
			                                  {
												
			                                  	var result = ((PictureBox)sender).Tag;
			                                  	EnableTrumpInteraction(false);
			                                  	return result;
			                                  });
		}
	}
}
