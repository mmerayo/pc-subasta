using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;
using Subasta.Domain.Deck;

namespace Subasta.Infrastructure.Domain
{
	public class DomainRegistry:Registry
	{
		public DomainRegistry()
		{
			For<IDeck>().Use(Deck.DefaultForSubasta);

		}
	}
}
