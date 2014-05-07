using System.Drawing;
using System.Windows.Forms;

namespace Subasta.Client.Common.Images
{
	public interface IResourceReadingUtils
	{
		void LoadCardImages(ImageList imageListCardsTarget, Size size);
		Image GetImage(string fileName);
		string GetResourceName(string fileName);
		string GetText(string fileName);
		void LoadSingleImage(ImageList imageListCards, string imageId, string resourceName);
	}
}