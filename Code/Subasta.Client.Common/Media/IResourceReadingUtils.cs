using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Subasta.Client.Common.Media
{
	public interface IResourceReadingUtils
	{
		void LoadCardImages(ImageList imageListCardsTarget, Size size);
		Image GetImage(string fileName);
		string GetResourceName(string fileName);
		string GetText(string fileName);
		void LoadSingleImage(ImageList imageListCards, string imageId, string resourceName);
		Stream GetResourceStream(string resourceName);
	}
}