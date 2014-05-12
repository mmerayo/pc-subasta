using System.Drawing;
using System.IO;
using Subasta.Domain.Deck;

namespace Subasta.Client.Common.Media
{
	public enum GameMediaType
	{
		Petar,
		Reverso,
		Jugador1,
		Jugador2,
		Jugador3,
		Jugador4,
		Turno,
		FirstPlayer,
		Winner,
		CanteRealizado
	}
	
	public interface IMediaProvider
	{
		Image GetImage(GameMediaType image);
		Image GetCard(string cardShortId);
		Image GetCard(ISuit suit, int i);
		Stream GetSoundStream(GameSoundType soundType);
	}
}