using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Subasta.Client.Common;
using Subasta.Domain.Deck;

namespace Analyzer
{
	partial class FrmExplorationStatus : Form
	{
		private readonly IGameSimulator _gameSimulator;

		public FrmExplorationStatus(IGameSimulator gameSimulator,IDeck deck)
		{
		    InitializeComponent();
			LoadImages(deck);
			_gameSimulator = gameSimulator;
			_gameSimulator.GameStatusChanged += _gameSimulator_GameStatusChanged;
			_gameSimulator.GameStarted += _gameSimulator_GameStarted;
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
			imageListCards.ImageSize = new Size(32, 32);
			
		}

		private string GetCardResourceName(ICard card)
		{
			return string.Format("Analyzer.Content.{0}_{1}s.jpg", card.Suit.Name.ToLower(), card.Number);
		}

		private PictureBox [][] pbCards=new PictureBox[4][];

		void _gameSimulator_GameStarted(Subasta.Domain.Game.IExplorationStatus status)
		{
			for (int i = 1; i <= 4; i++)
			{
				int indexCard = 0;
				foreach (var playerCard in status.PlayerCards(i))
				{
					var pb = new PictureBox();
					pb.SizeMode = PictureBoxSizeMode.StretchImage;

					Controls.Add(pb);

					var image = imageListCards.Images[playerCard.ToShortString()];

					this.ClientSize = new Size(image.Width, image.Height); // this allows you to
					
					pb.Image = image; 
				}
			}
		}

		private void _gameSimulator_GameStatusChanged(Subasta.Domain.Game.IExplorationStatus status)
		{
			throw new NotImplementedException();
		}

		
	}
}
