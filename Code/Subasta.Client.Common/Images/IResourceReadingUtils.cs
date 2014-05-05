using System.Drawing;
using System.Windows.Forms;
using Subasta.Domain.Deck;

namespace Subasta.Client.Common.Images
{
	public interface IResourceReadingUtils
	{
		void LoadCardImages(ImageList imageListCardsTarget, Size size);
		Image GetImage(string fileName);
		string GetResourceName(string fileName);
		string GetText(string fileName);
	}
}