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
			public Say(byte playerNum, IFigure kind,byte sequence)
			{
				Figure = kind;
				Sequence = sequence;
				PlayerNum = playerNum;
			}

			public byte PlayerNum { get; private set; }
			public byte PlayerTeamNum { get { return (byte) (PlayerNum%2 == 1 ? 1 : 2); }}
			public IFigure Figure { get; private set; }
			public byte Sequence { get; private set; }

			public override string ToString()
			{
				return string.Format("Player:{0} - {1}", PlayerNum, Figure.ToString());
			}
		}

		private readonly IExplorationStatus _status;
		private readonly byte? _firstPlayer;
		private readonly ISayCard[] _cardsP1;
		private readonly ISayCard[] _cardsP2;
		private readonly ISayCard[] _cardsP3;
		private readonly ISayCard[] _cardsP4;

		private readonly List<ISay>_says=new List<ISay>();

		public SaysStatus(IExplorationStatus status, byte firstPlayer)
		{
			_status = status;
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
				if(_says.Count(x => x.Figure.Say == SayKind.Paso) == 4 && _says.Count == 4 )
					return true;


				return _says.Count(x => x.Figure.Say == SayKind.Paso) == 3 && Says.Count>3;
			}
		}

		public bool IsEmpty
		{
			get { return _says.Count == 0; }
		}


		public byte Turn
		{
			get
			{
				ThrowIfCompleted();

				byte? result = null;
				do
				{
					if (_says.Count == 0)
						result = FirstPlayer;
					else if (!result.HasValue)
						result = NextPlayer(_says.Last().PlayerNum);
					else
						result = NextPlayer(result.Value);

				} while (PlayerHasPassed(result.Value));
				return result.Value;
			}
		}

		private bool PlayerHasPassed(byte playerNum)
		{
			return _says.Any(x => x.Figure.Say==SayKind.Paso && x.PlayerNum == playerNum);
		}

		private byte NextPlayer(byte playerNum)
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

		private void ThrowIfEmpty()
		{
			if (IsEmpty)
				throw new InvalidOperationException("The say is empty");
		}


		public byte PlayerBets
		{
			get { return _says.Last(x=>x.Figure.Say != SayKind.Paso).PlayerNum; }
		}

		public byte PointsBet
		{
			get
			{
				//return _says.Where(x => x.PlayerTeamNum == TeamBets).Sum(x => x.Figure.PointsBet);
				ISay last = _says.LastOrDefault(x => x.Figure.Say != SayKind.Paso);
				if (last == null)
					return 0;
				if (!last.Figure.UsingAlternative)
					return last.Figure.PointsBet;
				//IT CAN BE REPEATED SO IT DOES ADDITIONS	
				last = _says.LastOrDefault(x => !x.Figure.UsingAlternative && x.Figure.Say != SayKind.Paso);
				int result;
				int startIndex;

				if (last == null)
				{
					result = 0;
					startIndex = 0;
				}
				else
				{
					result = last.Figure.PointsBet;
					startIndex = _says.IndexOf(last) + 1;

				}
				for (int idx = startIndex; idx < _says.Count; idx++)
				{
					IFigure figure = _says[idx].Figure;
					result += figure.PointsBet;
				}
				return (byte) result;
			}
		}

		public byte TurnTeam
		{
			get { return (byte) (Turn%2 == 1 ? 1 : 2); }
		}

		public byte TeamBets
		{
			get { return (byte) (PlayerBets%2 == 1 ? 1 : 2); }
		}

		public byte Sequences
		{
			get
			{
				var lastOrDefault = Says.LastOrDefault();
				return (byte)(lastOrDefault != null ? lastOrDefault.Sequence : 1);
			}
		}

		public byte LastSayPlayer
		{
			get
			{
				ThrowIfEmpty();
				return _says.Last().PlayerNum;
			}
		}

		


		public List<ISay> Says{get { return new List<ISay>(_says); }}

		public IExplorationStatus OriginalStatus
		{
			get
			{
				IExplorationStatus result = _status.Clone();
				result.LogicalComplete = true;
				return result;
			}
		}

		public byte OtherTeam
		{
			get { return (byte)(TurnTeam == 1 ? 2 : 1); }
		}

		public byte FirstPlayer
		{
			get { return _firstPlayer.Value; }
		}

		public ISaysStatus Clone()
		{
//TODO:
			return this;
		}

		public void Add(byte playerNumber, IFigure figure)
		{
			var sequence = (byte)(_says.Count > 0 ? _says.Max(x => x.Sequence) + 1 : 1);
			var item = new Say(playerNumber, figure, sequence);
			_says.Add(item);
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
			//its for the exploration
			result.SetPlayerBet(1,250);
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