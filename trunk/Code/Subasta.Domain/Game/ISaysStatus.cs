using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{
	/// <summary>
	/// status of the say step(marque)
	/// </summary>
	public interface ISaysStatus
	{
		bool IsCompleted { get; }
		int Turn { get; }
		int PlayerBets { get; }
		int PointsBet { get; }
		ISaysStatus Clone();
		void Add(SayKind result);

		IExplorationStatus ExplorationStatusForOros();
		IExplorationStatus ExplorationStatusForCopas();
		IExplorationStatus ExplorationStatusForEspadas();
		IExplorationStatus ExplorationStatusForBastos();
	}

	public enum SayKind
	{
		As
	}
}
