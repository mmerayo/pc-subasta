using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;
using Subasta.Domain.Game.Analysis;
using Subasta.DomainServices;

namespace Subasta
{
	internal class RegisterIoc : Registry
	{
		public RegisterIoc()
		{
			For<FrmMain>().Singleton().Use<FrmMain>();

			For<ISaysExplorationListener>().Singleton().Use<NullExplorationListenerHandler>();
			For<IApplicationEventsExecutor>().Use<ApplicationEventsExecutorExecutor>();
		}
	}
}
