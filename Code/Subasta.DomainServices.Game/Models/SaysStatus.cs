using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Models
{
	internal class SaysStatus:ISaysStatus
	{
		public SaysStatus(IExplorationStatus status)
		{
			throw new System.NotImplementedException();
		}

		public bool IsCompleted
		{
			get { throw new System.NotImplementedException(); }
		}

		public int Turn
		{
			get { throw new System.NotImplementedException(); }
		}

		public ISaysStatus Clone()
		{
			throw new System.NotImplementedException();
		}

		public void Add(SayKind result)
		{
			throw new System.NotImplementedException();
		}
	}
}