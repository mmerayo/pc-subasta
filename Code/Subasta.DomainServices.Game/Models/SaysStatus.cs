using System;
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
		private readonly int _firstPlayer;
		
		private readonly List<ISay>_says=new List<ISay>();

		public SaysStatus(IExplorationStatus status, int firstPlayer)
		{
			_status = status;
			_firstPlayer = firstPlayer;
		}

		public bool IsCompleted
		{
			get
			{
				return _says.Count(x => x.Kind == SayKind.Paso) == 3;
			}
		}

		public int Turn
		{
			get
			{
				ThrowIfCompleted();

				int result;
				do
				{
					int playerNum = _says.Count > 0 ? _says.Last().PlayerNum : _firstPlayer;
					result = NextPlayer(playerNum);
				} while (PlayerHasPass(result));
				return result;
			}
		}

		private bool PlayerHasPass(int playerNum)
		{
			return _says.Any(x => x.Kind == SayKind.Paso && x.PlayerNum == playerNum);
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
			get { throw new System.NotImplementedException(); }
		}

		public int TurnTeam { get; private set; }

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