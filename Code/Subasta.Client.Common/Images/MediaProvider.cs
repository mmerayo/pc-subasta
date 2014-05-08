using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Subasta.Domain.Deck;
using Subasta.Infrastructure.Domain;

namespace Subasta.Client.Common.Images
{
	class MediaProvider : IMediaProvider
	{
		private readonly ImageList _imageList=new ImageList();
		private readonly Dictionary<GameMediaType, string> _fileNameMap=new Dictionary<GameMediaType, string>();
		public MediaProvider(IResourceReadingUtils resourceReader)
		{
			
				InitializeMap();

				resourceReader.LoadCardImages(_imageList, new Size(50, 70));

				foreach (var value in Enum.GetValues(typeof (GameMediaType)).Cast<GameMediaType>())
				{
					resourceReader.LoadSingleImage(_imageList, value.ToString(), resourceReader.GetResourceName(_fileNameMap[value]));
				}
			
		}

		private void InitializeMap()
		{
			_fileNameMap.Add(GameMediaType.Petar,"petar.jpg");
			_fileNameMap.Add(GameMediaType.Reverso,"reverso.jpg");
			_fileNameMap.Add(GameMediaType.Player, "player.png");
			_fileNameMap.Add(GameMediaType.Jugador1, "jugador1.png");
			_fileNameMap.Add(GameMediaType.Jugador2, "jugador2.png");
			_fileNameMap.Add(GameMediaType.Jugador3, "jugador3.png");
			_fileNameMap.Add(GameMediaType.Jugador4, "jugador4.png");
			_fileNameMap.Add(GameMediaType.Turno, "turno.jpg");
			_fileNameMap.Add(GameMediaType.FirstPlayer, "FirstPlayer.jpg");
		}

		public Image GetImage(GameMediaType gameMediaType)
		{
			return (Image)_imageList.Images[gameMediaType.ToString()].Clone();
		}

		public Image GetCard(string cardShortId)
		{
			return (Image)_imageList.Images[cardShortId].Clone();
		}

		public Image GetCard(ISuit suit, int number)
		{
			return GetCard(string.Format("{0}{1}", suit.Name[0], number));
		}
	}
}