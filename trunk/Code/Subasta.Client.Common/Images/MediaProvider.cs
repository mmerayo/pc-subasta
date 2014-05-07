using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Subasta.Client.Common.Images
{
	class MediaProvider : IMediaProvider
	{
		private readonly ImageList _imageList=new ImageList();
		private readonly Dictionary<GameMediaType, string> _fileNameMap=new Dictionary<GameMediaType, string>();
		public MediaProvider(IResourceReadingUtils resourceReader)
		{
			InitializeMap();

			resourceReader.LoadCardImages(_imageList,new Size(50, 70));

			foreach (var value in Enum.GetValues(typeof(GameMediaType)).Cast<GameMediaType>())
			{
				if(value==GameMediaType.Card)
					continue;

				resourceReader.LoadSingleImage(_imageList, value.ToString(), resourceReader.GetResourceName(_fileNameMap[value]));
			}
		}

		private void InitializeMap()
		{
			_fileNameMap.Add(GameMediaType.Petar,"petar.jpg");
			_fileNameMap.Add(GameMediaType.Reverso,"reverso.jpg");
			_fileNameMap.Add(GameMediaType.Player, "player.png");
		}

		public Image GetImage(GameMediaType gameMediaType)
		{
			return (Image)_imageList.Images[gameMediaType.ToString()].Clone();
		}

		public Image GetCard(string cardShortId)
		{
			return (Image)_imageList.Images[cardShortId].Clone();
		}
	}
}