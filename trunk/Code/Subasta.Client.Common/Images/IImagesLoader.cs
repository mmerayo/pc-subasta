using System.Drawing;
using System.Windows.Forms;
using Subasta.Domain.Deck;

namespace Subasta.Client.Common.Images
{
	public interface IImagesLoader
	{
		void LoadCardImages(ImageList imageListCardsTarget, Size size);
		Image GetImage(string fileName);
	}
}