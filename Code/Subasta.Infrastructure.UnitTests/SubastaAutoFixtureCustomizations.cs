using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Ploeh.AutoFixture.Kernel;

namespace Subasta.Infrastructure.UnitTests
{
	class SubastaAutoFixtureCustomizations:ICustomization
	{
		public void Customize(IFixture fixture)
		{
			fixture.Customize(new MultipleCustomization());

			//Enable Rhino mocking support for interfaces creation
			fixture.Customize(new AutoRhinoMockCustomization());

			fixture.Customizations.Add(new StatusSpecimenBuilder());
		}
	}

	public class StatusSpecimenBuilder : ISpecimenBuilder
	{
		public object Create(object request, ISpecimenContext context)
		{
			throw new NotImplementedException();
		}
	}
}
