using System.Collections;
using Subasta.Domain.Deck;
using Subasta.DomainServices.Game.Algorithms.Figures.Catalog;
using Subasta.Infrastructure.Domain;

namespace Subasta.Infrastructure.UnitTests.DomainServices.Game.Algorithms.Figures
{
	class FigureReyesTests : FiguresTests<FigureReyes>
	{
		public new static IEnumerable CanPerform_IsAvailable_TestCases()
		{
			return FiguresTests<FigureReyes>.CanPerform_IsAvailable_TestCases();//issue with nunit
		}

		protected override ISayCard[] GetCards(bool containingTheFigure)
		{
			return containingTheFigure
				? new ISayCard[]
				{
					new SayCard("O12"),
					new SayCard("C12"),
					new SayCard("E12"),
					new SayCard("B12"),

				}
				: new ISayCard[] {new SayCard("O1"),};
		}
	}
}