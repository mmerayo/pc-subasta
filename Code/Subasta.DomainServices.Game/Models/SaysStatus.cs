using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Models
{
	internal class SaysStatus:ISaysStatus
	{
		public SaysStatus(IExplorationStatus status)
			{//TODO:
			//throw new System.NotImplementedException();
		}

		public bool IsCompleted
		{
			get {//TODO:
			 return true; }
		}

		public int Turn
		{
			get
			{
			//TODO:
				return 1;
			}
		}

		public ISaysStatus Clone()
			{//TODO:
			return this;
		}

		public void Add(SayKind result)
			{//TODO:
			
		}
	}
}