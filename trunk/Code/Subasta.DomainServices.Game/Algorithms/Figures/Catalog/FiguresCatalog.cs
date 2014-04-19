using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal class FiguresCatalog : IFiguresCatalog
	{
		private  List<IFigure> _figures;

		public IEnumerable<IFigure> Figures
		{
			get { return _figures; }
		}

		public void Init()
		{
			_figures = ObjectFactory.GetAllInstances<IFigure>().ToList();
			for (int i = 0; i < 3; i++) //hay 4 ases
				_figures.Add(new FigureAs());
		}

		public IFigure Get(int points)
		{
			if(points==0)
				return new FigurePaso();
			return new FigureJustPoints(points*10);
		}
	}
}
