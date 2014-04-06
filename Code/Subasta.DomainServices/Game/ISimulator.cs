using System;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game
{
	
	public interface ISimulator 
	{
		NodeResult GetBest(IExplorationStatus currentStatus);

		int MaxDepth { get; set; }

		void Start(IExplorationStatus initialStatus, object root=null); //TODO: TYPE

	    void Reset();
	}
}