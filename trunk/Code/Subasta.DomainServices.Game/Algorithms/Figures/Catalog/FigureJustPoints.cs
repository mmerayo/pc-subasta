using System;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	class FigureJustPoints:IFigure
	{
		private readonly byte _points;
		public FigureJustPoints(int points)
		{
			if(points<0 || points>250)
				throw new ArgumentOutOfRangeException("points","[0-250]");

			_points = (byte) Math.Truncate((double) (points/10));
			
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

		public byte PointsBet { get { return _points; } }
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

		public bool IsAvailable(ISaysStatus saysStatus, byte normalizedTopPoints)
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
