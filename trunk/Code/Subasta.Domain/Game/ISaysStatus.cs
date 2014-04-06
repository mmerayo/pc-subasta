using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subasta.Domain.Game
{
	/// <summary>
	/// status of the say step(marque)
	/// </summary>
	public interface ISaysStatus
	{
		bool IsCompleted { get; }
		int Turn { get; }
		ISaysStatus Clone();
		void Add(SayKind result);
	}

	public enum SayKind
	{
		
	}
}
