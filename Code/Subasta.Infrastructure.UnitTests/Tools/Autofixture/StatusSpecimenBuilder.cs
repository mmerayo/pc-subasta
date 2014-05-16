using System;
using Ploeh.AutoFixture.Kernel;
using Rhino.Mocks;
using Subasta.Domain;
using Subasta.Domain.Game;

namespace Subasta.Infrastructure.UnitTests.Tools.Autofixture
{
	public class StatusSpecimenBuilder : ISpecimenBuilder
	{
		private const int PlayerBets = 1;
		private const int TeamBets = 1;
		public object Create(object request, ISpecimenContext context)
		{
			var t = request as Type;
			if (typeof (IExplorationStatus) == t)
			{
				var result=MockRepository.GenerateMock<IExplorationStatus>();
				result.Stub(x => x.PlayerBets).Return(PlayerBets); 
				result.Stub(x => x.TeamBets).Return(TeamBets);
				result.Stub(x => x.PlayerMateOf(PlayerBets)).Return(PlayerBets + 2);
				result.Stub(x => x.GetPlayerDeclarables(PlayerBets)).Return(new Declaration[0]);
				result.Stub(x => x.GetPlayerDeclarables(PlayerBets+2)).Return(new Declaration[0]);
				return result;
			}

			return new NoSpecimen(request);
		}
	}
}