using System;
using System.Collections.Generic;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.DomainServices.Game.Models
{
	internal class SaysStatus : ISaysStatus
	{
		private class Say : ISay
		{
			public Say(int playerNum, IFigure kind,int sequence)
			{
				Figure = kind;
				Sequence = sequence;
				PlayerNum = playerNum;
			}

			public int PlayerNum { get; private set; }
			public int PlayerTeamNum { get { return PlayerNum%2 == 1 ? 1 : 2; }}
			public IFigure Figure { get; private set; }
			public int Sequence { get; private set; }

		}

		private readonly IExplorationStatus _status;
		private readonly int _firstPlayer;
		private readonly ISayCard[] _cardsP1;
		private readonly ISayCard[] _cardsP2;
		private readonly ISayCard[] _cardsP3;
		private readonly ISayCard[] _cardsP4;

		private readonly List<ISay>_says=new List<ISay>();

		public SaysStatus(IExplorationStatus status, int firstPlayer)
		{
			_status = status.Clone();
			_status.LogicalComplete = false;
			_firstPlayer = firstPlayer;
			_cardsP1 = SayCard.FromCards(status.PlayerCards(1));
			_cardsP2 = SayCard.FromCards(status.PlayerCards(2));
			_cardsP3 = SayCard.FromCards(status.PlayerCards(3));
			_cardsP4 = SayCard.FromCards(status.PlayerCards(4));
		}

		public bool IsCompleted
		{
			get
			{
				return _says.Count(x => x.Figure.Say.Contains(SayKind.Paso)) == 3 && _says.Count>3;
			}
		}

		public int Turn
		{
			get
			{
				//ThrowIfCompleted();

				int result;
				do
				{
					if (_says.Count > 0)
					{
						result = NextPlayer(_says.Last().PlayerNum);

					}
					else
					{
						result = _firstPlayer;
					}
				} while (PlayerHasPassed(result));
				return result;
			}
		}

		private bool PlayerHasPassed(int playerNum)
		{
			return _says.Any(x => x.Figure.Say.Contains(SayKind.Paso) && x.PlayerNum == playerNum);
		}

		private int NextPlayer(int playerNum)
		{
			playerNum += 1;
			if (playerNum > 4)
				playerNum = 1;
			return playerNum;
		}

		private void ThrowIfCompleted()
		{
			if(IsCompleted)
				throw new InvalidOperationException("The say is completed");
		}

		public int PlayerBets
		{
			get { return _says.Last().PlayerNum; }
		}

		public int PointsBet
		{
			get { return _says.Where(x => x.PlayerTeamNum == TeamBets).Sum(x => x.Figure.PointsBet); }
		}

		public int TurnTeam
		{
			get { return Turn%2 == 1 ? 1 : 2; }
		}

		public int TeamBets
		{
			get { return PlayerBets%2 == 1 ? 1 : 2; }
		}

		public int Sequences
		{
			get
			{
				var lastOrDefault = Says.LastOrDefault();
				return lastOrDefault != null ? lastOrDefault.Sequence : 1;
			}
		}


		public List<ISay> Says{get { return new List<ISay>(_says); }} 

		public ISaysStatus Clone()
		{
//TODO:
			return this;
		}

		public void Add(int playerNumber, IFigure figure)
		{
			_says.Add(new Say(playerNumber, figure, _says.Count > 0 ? _says.Max(x => x.Sequence) + 1 : 1));
		}

	    public ISayCard[] GetPlayerCards(int playerNum)
	    {
	        switch (playerNum)
	        {
	            case 1:
	                return _cardsP1;
	            case 2:
	                return _cardsP2;
	            case 3:
	                return _cardsP3;
	            case 4:
	                return _cardsP4;

	        }
	        throw new ArgumentOutOfRangeException();
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