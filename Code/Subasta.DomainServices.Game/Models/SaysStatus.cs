using System.Collections.Generic;
using System.Linq;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.DomainServices.Game.Models
{
	internal class SaysStatus : ISaysStatus
	{
	    private class Say : ISay
	    {
	        public Say(int playerNum, SayKind kind)
	        {
	            Kind = kind;
	            PlayerNum = playerNum;
	        }

	        public int PlayerNum { get; private set; }
	        public SayKind Kind { get; private set; }
	    }

		private readonly IExplorationStatus _status;

        private readonly List<ISay>_says=new List<ISay>();

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
			get { return _says.Last().PlayerNum; }
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

		public void Add(int playerNumber, SayKind sayKind)
		{
		    _says.Add(new Say(playerNumber, sayKind));
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