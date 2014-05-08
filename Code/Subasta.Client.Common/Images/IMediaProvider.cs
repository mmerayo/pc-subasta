using System.Drawing;
using Subasta.Domain.Deck;

namespace Subasta.Client.Common.Images
{
	public enum GameMediaType
	{
		Player,
		Petar,
		Reverso,
		Jugador1,
		Jugador2,
		Jugador3,
		Jugador4,
		Turno,
		FirstPlayer
	}
	
	public interface IMediaProvider
	{
		Image GetImage(GameMediaType image);
		Image GetCard(string cardShortId);
		Image GetCard(ISuit suit, int i);
	}
}