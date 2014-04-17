using System.Collections;
using NUnit.Framework;
using Subasta.Domain.Deck;
using Subasta.DomainServices.Game.Algorithms.Figures.Catalog;
using Subasta.Infrastructure.Domain;

namespace Subasta.Infrastructure.UnitTests.DomainServices.Game.Algorithms.Figures
{
	class FigureAsTests : FiguresTests<FigureAs>
	{
		public new static IEnumerable CanPerform_IsAvailable_TestCases()
		{
			return FiguresTests<FigureAs>.CanPerform_IsAvailable_TestCases();
		}

		protected override ISayCard[] GetCards(bool containingTheFigure)
		{
			return new ISayCard[]
			{
				containingTheFigure? new SayCard("O1"):new SayCard("O2"),
				
			};
		}
	}
}