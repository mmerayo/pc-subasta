using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Subasta.Domain.Deck;
using log4net;

namespace Subasta.Client.Common.Media
{
	internal class MediaProvider : IMediaProvider
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof (MediaProvider));
		
		private readonly IResourceReadingUtils _resourceReader;
		private readonly ImageList _imageList = new ImageList();
		private readonly Dictionary<GameMediaType, string> _imageFileMap = new Dictionary<GameMediaType, string>();
		private readonly Dictionary<GameSoundType, string> _soundFileMap = new Dictionary<GameSoundType, string>();

		public MediaProvider(IResourceReadingUtils resourceReader)
		{
			_resourceReader = resourceReader;

			InitializeMap();

			resourceReader.LoadCardImages(_imageList, new Size(50, 70));

			foreach (var value in Enum.GetValues(typeof (GameMediaType)).Cast<GameMediaType>())
			{
				resourceReader.LoadSingleImage(_imageList, value.ToString(), resourceReader.GetResourceName(_imageFileMap[value]));
			}

		}

		private void InitializeMap()
		{
			_imageFileMap.Add(GameMediaType.Petar, "petar.png");
			_imageFileMap.Add(GameMediaType.Reverso, "reverso.png");
			_imageFileMap.Add(GameMediaType.Jugador1, "jugador1.png");
			_imageFileMap.Add(GameMediaType.Jugador2, "jugador2.png");
			_imageFileMap.Add(GameMediaType.Jugador3, "jugador3.png");
			_imageFileMap.Add(GameMediaType.Jugador4, "jugador4.png");
			_imageFileMap.Add(GameMediaType.Turno, "turno.png");
			_imageFileMap.Add(GameMediaType.FirstPlayer, "FirstPlayer.png");
			_imageFileMap.Add(GameMediaType.Winner, "Winner.png");
			_imageFileMap.Add(GameMediaType.CanteRealizado, "CanteRealizado.png");

			_soundFileMap.Add(GameSoundType.PetarMesa,"petar.mp3");
			_soundFileMap.Add(GameSoundType.CardPlayed,"placeCard.mp3");
			_soundFileMap.Add(GameSoundType.Shuffle, "Shuffle.mp3");
			_soundFileMap.Add(GameSoundType.TurnChanged,"beep.mp3");
			_soundFileMap.Add(GameSoundType.DeclarationEmit, "prize.mp3");
			_soundFileMap.Add(GameSoundType.Selection, "Selection.mp3");
			
			_soundFileMap.Add(GameSoundType.Voice1, "Voice1.mp3");
			_soundFileMap.Add(GameSoundType.Voice2, "Voice2.mp3");
			_soundFileMap.Add(GameSoundType.Voice3, "Voice3.mp3");
			_soundFileMap.Add(GameSoundType.Voice4, "Voice4.mp3");
			_soundFileMap.Add(GameSoundType.Voice5, "Voice5.mp3");
			_soundFileMap.Add(GameSoundType.Voice6, "Voice6.mp3");
		}

		public Image GetImage(GameMediaType gameMediaType)
		{
			try
			{
				return (Image) _imageList.Images[gameMediaType.ToString()].Clone();
			}
			catch (Exception ex)
			{
				Logger.Warn("GetImage", ex);
				return null;
			}
		}

		public Image GetCard(string cardShortId)
		{
			try
			{
				return (Image) _imageList.Images[cardShortId].Clone();
			}
			catch (Exception ex)
			{
				Logger.Warn("GetCard", ex);
				return null;
			}
		}

		public Image GetCard(ISuit suit, int number)
		{
			return GetCard(string.Format("{0}{1}", suit.Name[0], number));
		}

		public Stream GetSoundStream(GameSoundType soundType)
		{
			string resourceName = _resourceReader.GetResourceName(_soundFileMap[soundType]);
			return _resourceReader.GetResourceStream(resourceName);
		}
	}
}