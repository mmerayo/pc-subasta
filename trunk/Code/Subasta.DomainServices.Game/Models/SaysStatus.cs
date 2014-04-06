using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.DomainServices.Game.Models
{
	internal class SaysStatus : ISaysStatus
	{
		private readonly IExplorationStatus _status;

		public SaysStatus(IExplorationStatus status)
		{
			_status = status;
		}

		public bool IsCompleted
		{
			get
			{
//TODO:
				return true;
			}
		}

		public int Turn
		{
			get
			{
				//TODO:
				return 1;
			}
		}

		public int PlayerBets
		{
			get { throw new System.NotImplementedException(); }
		}

		public int PointsBet
		{
			get { throw new System.NotImplementedException(); }
		}

		public ISaysStatus Clone()
		{
//TODO:
			return this;
		}

		public void Add(SayKind result)
		{
//TODO:

		}


		private IExplorationStatus GetExplorationStatus(string suitName)
		{
			var result = _status.Clone();
			result.SetTrump(Suit.FromName(suitName));
			return result;
		}

		public IExplorationStatus ExplorationStatusForOros()
		{
			return GetExplorationStatus("Oros");
		}

		public IExplorationStatus ExplorationStatusForCopas()
		{
			return GetExplorationStatus("Copas");
		}

		public IExplorationStatus ExplorationStatusForEspadas()
		{
			return GetExplorationStatus("Espadas");
		}

		public IExplorationStatus ExplorationStatusForBastos()
		{
			return GetExplorationStatus("Bastos");
		}
	}
}