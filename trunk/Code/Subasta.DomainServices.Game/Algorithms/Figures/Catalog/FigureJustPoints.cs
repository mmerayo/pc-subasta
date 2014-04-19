using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	class FigureJustPoints:IFigure
	{
		private readonly int _points;
		public FigureJustPoints(int points)
		{
			_points = (int) Math.Truncate((double) (points/10));
		}

		public SayKind Say
		{
			get { return (SayKind) _points; }
		}

		public SayKind AlternativeSay
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		public int PointsBet { get { return _points; } }
		public int AlternativePointsBet
		{
			get
			{
				throw new InvalidOperationException();
			}
		}
		public ICard[] MarkedCards { get{return new ICard[0];} }

		public bool UsingAlternative
		{
			get { return false; }
		}

		public bool HasAlternativeSay { get { return false; } }

		public bool IsAvailable(ISaysStatus saysStatus, int normalizedTopPoints)
		{
			return true;

		}

		public void MarkFigures(ISaysStatus saysStatus)
		{
		}

		public void UnMarkPotentialCandidates()
		{
		}

		public override string ToString()
		{
			return Say.ToString();
		}
	}
}
