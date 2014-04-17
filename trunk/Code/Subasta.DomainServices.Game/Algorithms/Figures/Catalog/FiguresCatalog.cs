using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FiguresCatalog : IFiguresCatalog
	{
		private readonly List<IFigure> _figures;

		public FiguresCatalog(IEnumerable<IFigure> figures)
		{
			_figures = figures.ToList();
			for (int i = 0; i < 3; i++) //hay 4 ases
				_figures.Add(new FigureAs());
		}

		public IEnumerable<IFigure> Figures
		{
			get { return _figures; }
		}
	}
}
