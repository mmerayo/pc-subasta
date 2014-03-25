using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;
using Subasta.ApplicationServices;
using Subasta.ApplicationServices.IO;
using Subasta.Infrastructure.ApplicationServices.IO;

namespace Subasta.Infrastructure.ApplicationServices
{
	public class ApplicationServicesRegistry:Registry
	{
		public ApplicationServicesRegistry()
		{
			For<IPathHelper>().Use<PathUtils>();
		}

	}
}
