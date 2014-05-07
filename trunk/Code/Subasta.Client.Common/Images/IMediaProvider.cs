using System.Drawing;

namespace Subasta.Client.Common.Images
{
	public enum GameMediaType
	{
		Player,
		Petar,
		Reverso,
		Card
	}
	
	public interface IMediaProvider
	{
		Image GetImage(GameMediaType image);
		Image GetCard(string cardShortId);
	}
}