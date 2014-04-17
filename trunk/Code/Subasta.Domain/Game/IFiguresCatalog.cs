using System.Collections.Generic;

namespace Subasta.Domain.Game
{
	public interface IFiguresCatalog
	{
		IEnumerable<IFigure> Figures { get; }
	}
}