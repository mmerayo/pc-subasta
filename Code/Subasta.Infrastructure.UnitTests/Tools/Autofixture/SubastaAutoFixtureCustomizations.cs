using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Subasta.Infrastructure.Domain;

namespace Subasta.Infrastructure.UnitTests.Tools.Autofixture
{
	class SubastaAutoFixtureCustomizations:ICustomization
	{
		public void Customize(IFixture fixture)
		{
			fixture.Customize(new MultipleCustomization());

			//Enable Rhino mocking support for interfaces creation
			fixture.Customize(new AutoRhinoMockCustomization());
			fixture.Customize<Status>(c => c.OmitAutoProperties());
			
			//fixture.Customizations.Add(new StatusSpecimenBuilder());
		}
	}
}
