﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Subasta.Domain.Deck;

namespace Subasta.Client.Common.Media
{
	internal class ResourceReadingUtils : IResourceReadingUtils
	{
		private readonly IDeck _deck;
		private static readonly string[] _manifestResourceNames=Assembly.GetExecutingAssembly()
				.GetManifestResourceNames();

		

		public ResourceReadingUtils(IDeck deck)
		{
			_deck = deck;
		}

		public void LoadCardImages(ImageList imageListCards, Size size)
		{
			
			foreach (var card in _deck.Cards.Cards)
			{
				string cardResourceName = GetResourceName(GetCardFileName(card));
				LoadSingleImage(imageListCards, card.ToShortString(), cardResourceName);
			}
			

			imageListCards.ImageSize = size;

		}

		public void LoadSingleImage(ImageList imageListCards, string imageId, string resourceName)
		{
			Image image = GetImageFromResourceFullName(resourceName);
			imageListCards.Images.Add(imageId,
										  image);
		}

		private Image GetImageFromResourceFullName(string resourceName)
		{
			Image image;
			using (var manifestResourceStream = GetResourceStream(resourceName))
			{
				image = Image.FromStream(manifestResourceStream);
			}
			return image;
		}

		public Stream GetResourceStream(string resourceName)
		{
			return GetType().Assembly.GetManifestResourceStream(resourceName);
		}


		public Image GetImage(string fileName)
		{
			{
				Image image;
				using (var manifestResourceStream = GetType().Assembly.GetManifestResourceStream(GetResourceName(fileName)))
				{
					image = Image.FromStream(manifestResourceStream);
				}
				return image;
			}
		}

		public string GetText(string fileName)
		{
			{
				string result;
				using (var manifestResourceStream = GetType().Assembly.GetManifestResourceStream(GetResourceName(fileName)))
					using ( var sr=new StreamReader(manifestResourceStream))
						result=sr.ReadToEnd();
				return result;
			}
		}

		public string GetResourceName(string fileName)
		{
			
			return
				_manifestResourceNames
					.Single(x => x.EndsWith(fileName, StringComparison.InvariantCultureIgnoreCase));
		}

		private string GetCardFileName(ICard card)
		{
			return string.Format("{0}_{1}s.png",  card.Suit.Name.ToLower(), card.Number);
		}


	}
}
