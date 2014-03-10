using Subasta.Domain.Deck;

namespace Subasta.Client.Common
{
	public interface IPlayer
	{
		ICard[] Cards { get; set; }
	}
}