using System;
using Ploeh.AutoFixture.Kernel;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game.Models;

namespace Subasta.Infrastructure.UnitTests.Tools.Autofixture
{
	public class SaysStatusSpecimenBuilder : ISpecimenBuilder
	{
		private const int PlayerBets = 1;
		private const int TeamBets = 1;
		public object Create(object request, ISpecimenContext context)
		{
			var t = request as Type;
			if (typeof(SaysStatus) == t)
			{
				return new SaysStatus((IExplorationStatus)context.Resolve(typeof (IExplorationStatus)), 1);
			}

			return new NoSpecimen(request);
		}
	}
}