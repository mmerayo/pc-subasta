using System.Collections.Generic;

namespace Subasta.Domain.Deck
{
	public interface IPack
	{
		List<ICard> Cards { get; }

		
	}
}