using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.DomainServices.Game.Models
{
	internal class Status : IExplorationStatus
	{
		
		private Guid _gameId;
		private readonly ICardComparer _cardsComparer;
		private readonly IPlayerDeclarationsChecker _declarationsChecker;
		private int _turn = int.MinValue;
		private readonly ICard[][] _playerCards = new ICard[4][];
		private List<IHand> _hands;
		private bool _gameCompleted;


		public Status(Guid gameId, ICardComparer cardsComparer, ISuit trump, IPlayerDeclarationsChecker declarationsChecker)
		{
			Trump = trump;
			_gameId = gameId;
			_cardsComparer = cardsComparer;
			_declarationsChecker = declarationsChecker;
			PlayerBets = int.MinValue;
		}

		private Status(ICardComparer cardsComparer, ISuit trump, IPlayerDeclarationsChecker declarationsChecker)
			:this(Guid.Empty,cardsComparer,trump,declarationsChecker)
		{
			
		}

		public IExplorationStatus Clone()
		{
			Debug.Assert(GameId != Guid.Empty);
			var target = new Status( _cardsComparer, Trump, _declarationsChecker)
			             	{
			             		_turn = _turn, 
								PlayerBets = PlayerBets,
								PointsBet = PointsBet,
								_gameCompleted = _gameCompleted
			             	};
			Array.Copy(_playerCards, target._playerCards, 4);
			if (_hands != null)
			{
				target._hands = new List<IHand>();
				_hands.ForEach(x => target._hands.Add(x.Clone()));
			}
			target._gameId = GameId;
			target._gameCompleted = false;
			//CALCULATE DECLARABLES
			return target;
		}

		public ISuit Trump { get; private set; }

		//el jugador que la pone
		public int PlayerBets { get; private set; }

		public int TeamBets
		{
			get
			{
				if (PlayerBets == 1 || PlayerBets == 3)
					return 1;
				return 2;
			}
		}

		public int Turn
		{
			set
			{
				ThrowIfNotValidPlayerPosition(value, "value");
				_turn = value;
			}
			get
			{
				if (_turn == int.MinValue)
					throw new InvalidOperationException("Turn has not been set");
				return _turn;
			}
		}

		public IHand CurrentHand
		{
			get { return Hands.Last(); }
		}

		public ReadOnlyCollection<IHand> Hands
		{
			get
			{
				EnsureHands();
				if(_hands.Count==0)
					AddNewHand();
				return _hands.AsReadOnly();
			}
		}

		private void EnsureHands()
		{
			if (_hands == null)
			{
				_hands = new List<IHand>(10);
			}
		}

		//expects the last hand to be completed before this request as 
		//its expected to be invoked on completed hand
		public Declaration[] Declarables
		{
			get
			{
				//in the latest completed hand no se ha cantado 
				IHand hand = LastCompletedHand;
				if (hand == null )
					return new Declaration[0];

				if(hand.Declaration.HasValue)
					return new Declaration[0];
				
				var candidates = GetDeclarationCandidates();
				//havent been applied yet 
				candidates.RemoveAll(y => Hands.Where(x => x.Declaration.HasValue).Select(x => x.Declaration.Value).Contains(y));

				return candidates.ToArray();
			}
		}

		private List<Declaration> GetDeclarationCandidates()
		{

			var last = LastCompletedHand;
			var teamPlayers = new int[2];
			if (last.PlayerWinner == 1 || last.PlayerWinner == 3)
			{
				teamPlayers[0] = 1;
				teamPlayers[1] = 3;
			}
			else
			{
				teamPlayers[0] = 2;
				teamPlayers[1] = 4;
			}

			var declarables = Enum.GetValues(typeof (Declaration)).Cast<Declaration>();
			var result = new List<Declaration>();
			for (int i = 0; i < 2; i++)
			{
				var declarations = declarables.Where(
					x => _declarationsChecker.HasDeclarable(x, Trump, _playerCards[teamPlayers[i] - 1]));
				result.AddRange(declarations);
			}

			return result;
		}

		public IHand LastCompletedHand
		{
			get
			{
				if (!Hands[0].IsCompleted)
					return null;
				return Hands.Last(x => x.IsCompleted);

			}
		}

	    public Guid GameId
	    {
	        get { return _gameId; }
	    }

		public int PointsBet { get; private set; }
		public bool IsCompleted
		{
			get
			{
				UpdateIsCompleted();
				return _gameCompleted;
			}
		}

		public IHand FirstDeclarableHand
		{
			get
			{
				IHand firstHand = Hands.FirstOrDefault(x => x.IsCompleted && IsInTeamBets(x.PlayerWinner.Value) && !x.Declaration.HasValue);
				if (firstHand!=null && firstHand.Sequence > 1 && !Hands[Hands.IndexOf(firstHand) - 1].Declaration.HasValue)
					return null;

				return firstHand;
			}
		}

		public int TurnTeam
		{
			get
			{
				if (Turn == 1 || Turn == 3) 
					return 1;
				return 2;
			}
		}

		public int TeamWinner
		{
			get
			{
				ThrowIfNotCompleted();

				if (SumTotalTeam(PlayerBets) >= PointsBet)
					return TeamBets;

				if (TeamBets == 1)
					return 2;
				return 1;
			}
		}



		private void UpdateIsCompleted()
		{
			if (_gameCompleted) return;
			_gameCompleted = _hands != null && _hands.Count == 10 && _hands[9].IsCompleted;
			if (_gameCompleted) return;
			if(!CurrentHand.IsCompleted && !CurrentHand.IsEmpty) return;
			if (SumTotalTeam(PlayerBets) >= PointsBet)
				_gameCompleted = true;
			else
			{
				var other = PlayerBets + 1;
				if (other > 4) other = 1;
				// 130 + POTENTIAL DECLARATION POINTS TEAM BETS - BETPOINTS
				int maxPotential = 130;
				for (int i = 1; i <= 4; i++)
					if (IsInTeamBets(i))
					{
						IEnumerable<Declaration> declarations = Enum.GetValues(typeof (Declaration)).Cast<Declaration>().Where(
							declarable => _declarationsChecker.HasDeclarable(declarable, Trump, _playerCards[i - 1]));
						maxPotential += declarations.Sum(source => DeclarationValues.ValueOf(source));
					}
				maxPotential+=Hands.Where(x => x.Declaration.HasValue).Sum(x => DeclarationValues.ValueOf(x.Declaration.Value));

				int sumTotalTeam = SumTotalTeam(other);
				if (sumTotalTeam >= maxPotential - PointsBet + 1)
					_gameCompleted = true;
			}
		}

		public bool IsInTeamBets(int playerPosition)
		{
			if (PlayerBets == 1 || PlayerBets == 3)
				return playerPosition == 1 || playerPosition == 3;
			return playerPosition == 2 || playerPosition == 4;
		}

		//TODO: move to its own resolver
		public int PlayerMateOf(int playerWinner)
		{
			int matePlayer;
			switch (playerWinner)
			{
				case 1:
					matePlayer = 3;
					break;
				case 2:
					matePlayer = 4;
					break;
				case 3:
					matePlayer = 1;
					break;
				case 4:
					matePlayer = 2;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return matePlayer;
		}

		public void AddNewHand()
		{
			ThrowIfNotPlayerBetSet();

			if (_hands.Count == 10 || _playerCards.All(x => x.Length == 0))
				return;
			var item = new Hand(_cardsComparer, Trump,_hands.Count+1);
			_hands.Add(item);
		}

		private void ThrowIfNotPlayerBetSet()
		{
			if (PlayerBets == int.MinValue)
				throw new InvalidOperationException("Player bets need to be set");
		}

		private void ThrowIfNotCompleted()
		{
			if(!IsCompleted)
				throw new InvalidOperationException("Status is not completed");
		}

		public ICard[] PlayerCards(int playerPosition)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);
			return _playerCards[playerPosition - 1];
		}

		public void SetCards(int playerPosition, ICard[] cards)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);

			_playerCards[playerPosition - 1] =
				cards.OrderBy(x => x.Suit.Name)
				.ThenByDescending(x => x.Value)
				.ThenByDescending(x => x.Number).ToArray();


			//TODO:CALCULATE DECLARABLES
		}


		/// <summary>
		/// Sum by player
		/// </summary>
		/// <param name="playerPosition"></param>
		/// <returns></returns>
		public int SumTotal(int playerPosition)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);

			var result= Hands.Where(x => x.IsCompleted && x.PlayerWinner == playerPosition).Sum(x => x.Points);
			if(Hands.Count>=10 && Hands[9].IsCompleted && Hands[9].PlayerWinner==playerPosition)
				result += 10;
			return result;
		}

		public int SumTotalTeam(int playerPosition)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);

			if (playerPosition == 1 || playerPosition == 3)
				return SumTotal(1) + SumTotal(3);
			return SumTotal(2) + SumTotal(4);
		}

		public void RemovePlayerCard(int playerPosition, ICard card)
		{
			var playerCards = PlayerCards(playerPosition).ToList();
			playerCards.RemoveAt(playerCards.IndexOf(card));

			SetCards(playerPosition, playerCards.ToArray());
		}

		private static void ThrowIfNotValidPlayerPosition(int playerPosition, string argName = "playerPosition")
		{
			if (playerPosition < 1 || playerPosition > 4)
				throw new ArgumentOutOfRangeException(argName);
		}


		public void SetPlayerBet(int playerPosition, int pointsBet)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);
			if(pointsBet<0) throw new ArgumentOutOfRangeException("pointsBet","Must be 0 or higher");

			PlayerBets = playerPosition;
			PointsBet = pointsBet;
		}

		public void AddHand(IHand hand)
		{
			EnsureHands();
			_hands.Add(hand);
		}

		public void SetTrump(ISuit trump)
		{
			Trump = trump;
		}
	}
}
