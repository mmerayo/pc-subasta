using System.Drawing;
using System.Windows.Forms;
using Subasta.Domain.Deck;

namespace Subasta.Client.Common.Images
{
	public interface IImagesLoader
	{
		void LoadImages( ImageList imageListCardsTarget, Size size,string resourceNamespace=null);
	}
}