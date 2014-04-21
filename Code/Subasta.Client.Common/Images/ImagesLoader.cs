using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Subasta.Domain.Deck;

namespace Subasta.Client.Common.Images
{
	internal class ImagesLoader : IImagesLoader
	{
		private readonly IDeck _deck;

		public ImagesLoader(IDeck deck)
		{
			_deck = deck;
		}

		public void LoadImages(ImageList imageListCards, Size size, string resourceNamespace = null)
		{
			if (resourceNamespace == null)
				resourceNamespace = string.Format("{0}.Content.Images", Assembly.GetAssembly(GetType()).GetName().Name);
			foreach (var card in _deck.Cards.Cards)
			{
				string cardResourceName = GetCardResourceName(resourceNamespace, card);
				LoadSingleImage(imageListCards, card.ToShortString(), cardResourceName);
			}
			LoadSingleImage(imageListCards, "reverso",string.Format("{0}.reverso.jpg", resourceNamespace));

			imageListCards.ImageSize = size;

		}

		private void LoadSingleImage(ImageList imageListCards, string imageId, string resourceName)
		{
			using (var manifestResourceStream = GetType().Assembly.GetManifestResourceStream(resourceName))
				imageListCards.Images.Add(imageId,
				                          Image.FromStream(manifestResourceStream));
		}

		private string GetCardResourceName(string resourceNamespace, ICard card)
		{
			return string.Format("{0}.{1}_{2}s.jpg", resourceNamespace, card.Suit.Name.ToLower(), card.Number);
		}


	}
}
