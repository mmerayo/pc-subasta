using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;
using Subasta.Client.Common.Infrastructure;
using Subasta.Domain.Game.Analysis;
using Subasta.DomainServices;
using Subasta.Infrastructure;

namespace Subasta
{
	internal class RegisterIoc : Registry
	{
		public RegisterIoc()
		{
			For<FrmMain>().Singleton().Use<FrmMain>();
			For<FrmGame>().Singleton().Use<FrmGame>();
			For<FrmSay>().Singleton().Use<FrmSay>();

			For<ISaysExplorationListener>().Singleton().Use<NullExplorationListenerHandler>();
			For<IApplicationEventsExecutor>().Use<ApplicationEventsExecutorExecutor>();
		}
	}
}
