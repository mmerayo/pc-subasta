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
		private byte? _turn = null;
		private readonly ICard[][] _playerCards = new ICard[4][];
		private List<IHand> _hands;
		private bool _gameCompleted;
		
		public Status(Guid gameId, ICardComparer cardsComparer, ISuit trump, IPlayerDeclarationsChecker declarationsChecker,bool logicalComplete=false)
		{
			Trump = trump;
			_gameId = gameId;
			_cardsComparer = cardsComparer;
			_declarationsChecker = declarationsChecker;
			LogicalComplete = logicalComplete;
			PlayerBets = byte.MinValue;
		}

		private Status(ICardComparer cardsComparer, ISuit trump, IPlayerDeclarationsChecker declarationsChecker,bool logicalComplete=false)
			:this(Guid.Empty,cardsComparer,trump,declarationsChecker,logicalComplete)
		{
			
		}

		public IExplorationStatus Clone()
		{
			Debug.Assert(GameId != Guid.Empty);
			var target = new Status( _cardsComparer, Trump, _declarationsChecker,LogicalComplete)
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
				_hands.ForEach(x => target._hands.Add(x.Clone(target)));
			}
			target._gameId = GameId;
			target._gameCompleted = false;
			//CALCULATE DECLARABLES
			return target;
		}



		public ISuit Trump { get; private set; }

		//el jugador que la pone
		public byte PlayerBets { get; private set; }

		public byte TeamBets
		{
			get
			{
				if (PlayerBets == 1 || PlayerBets == 3)
					return 1;
				return 2;
			}
		}

		public byte TotalMoves
		{
			get { return (byte) (((Hands.Count - 1)*4) + CurrentHand.CardsByPlaySequence().Count(x=>x!=null)); }
		}
		//completes the game by logic even if not all the cards were played
		public bool LogicalComplete { get; set; }

		public byte Turn
		{
			set
			{
				ThrowIfNotValidPlayerPosition(value, "value");
				_turn = value;
			}
			get
			{
				if (!_turn.HasValue )
					throw new InvalidOperationException("Turn has not been set");
				return _turn.Value;
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
				if (hand == null
					|| hand.Declaration.HasValue
					|| Hands.Any(x => x.Declaration == Declaration.Reyes || x.Declaration == Declaration.Caballos))
					return new Declaration[0];
				//si no canto en la ultima baza que tuvieron
				if (Hands.Count(x=>x.TeamWinner==TeamBets &&!x.Declaration.HasValue) >= 2)
					return new Declaration[0];
			  
				var candidates = GetDeclarationCandidates();
				//havent been applied yet 
				candidates.RemoveAll(
					y => Hands.Where(x => x.Declaration.HasValue).Select(x => x.Declaration.Value).Contains(y));

				return candidates.ToArray();
			}
		}

		public IEnumerable<Declaration> GetPlayerDeclarables(byte playerNumber)
		{
			//	TODO: USE IPLAYER INSTEAD OF ARRAYS AND THE PLAYER TO KEEP DE INFO
			var declarables = Enum.GetValues(typeof (Declaration)).Cast<Declaration>();

			IEnumerable<Declaration> playerDeclarables = declarables.Where(x => _declarationsChecker.HasDeclarable(x, Trump, _playerCards[playerNumber - 1]));

			IEnumerable<Declaration?> declaredAlready = Hands.Where(x=>x.Declaration!=null).Select(x=>x.Declaration);

			return playerDeclarables.Where(x => !declaredAlready.Contains(x));

		}

		private List<Declaration> GetDeclarationCandidates()
		{
			var result = new List<Declaration>();
			var last = LastCompletedHand;
			if (TeamBets != last.TeamWinner)
				return result;

			var teamPlayers = new byte[2];
			if (last.TeamWinner== 1)
			{
				teamPlayers[0] = 1;
				teamPlayers[1] = 3;
			}
			else
			{
				teamPlayers[0] = 2;
				teamPlayers[1] = 4;
			}

			
			for (int i = 0; i < 2; i++)
			{	
				result.AddRange(GetPlayerDeclarables(teamPlayers[i]));
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

		public byte PointsBet { get; private set; }
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

		public byte TurnTeam
		{
			get
			{
				if (Turn == 1 || Turn == 3) 
					return 1;
				return 2;
			}
		}

		public byte TeamWinner
		{
			get
			{
				ThrowIfNotCompleted();

				if (SumTotalTeam(TeamBets) >= PointsBet)
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
			if (LogicalComplete)
			{
				if (!CurrentHand.IsCompleted && !CurrentHand.IsEmpty) return;
				if (SumTotalTeam(TeamBets) >= PointsBet)
					_gameCompleted = true;
				else
				{
					var other = PlayerBets + 1;
					if (other > 4) other = 1;
					// 130 + POTENTIAL DECLARATION POINTS TEAM BETS - BETPOINTS
					var maxPotential = 130;
					for (byte i = 1; i <= 4; i++)
						if (IsInTeamBets(i))
						{
							IEnumerable<Declaration> declarations = Enum.GetValues(typeof (Declaration)).Cast<Declaration>().Where(
								declarable => _declarationsChecker.HasDeclarable(declarable, Trump, _playerCards[i - 1]));
							maxPotential += declarations.Sum(source => DeclarationValues.ValueOf(source));
						}
					maxPotential += Hands.Where(x => x.Declaration.HasValue).Sum(x => DeclarationValues.ValueOf(x.Declaration.Value));

					int sumTotalTeam = SumTotalTeam(PlayerTeam(other));
					if (sumTotalTeam >= maxPotential - PointsBet + 1)
						_gameCompleted = true;
				}
			}
		}

		private byte PlayerTeam(int playerNumber)
		{
			return (byte)(playerNumber == 1 || playerNumber == 3 ? 1 : 2);
		}

		public bool IsInTeamBets(byte playerPosition)
		{
			if (PlayerBets == 1 || PlayerBets == 3)
				return playerPosition == 1 || playerPosition == 3;
			return playerPosition == 2 || playerPosition == 4;
		}

		//TODO: move to its own resolver
		public byte PlayerMateOf(byte playerWinner)
		{
			byte matePlayer;
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

		public ICard[] GetCardsNotYetPlayed()
		{
			return PlayerCards(1).Union(PlayerCards(2)).Union(PlayerCards(3)).Union(PlayerCards(4)).ToArray();
		}

	

		public byte NormalizedPointsBet
		{
			get { return (byte)Math.Truncate((double)(PointsBet/10)); }
		}

		public byte LastPlayerMoved
		{
			get
			{
				ThrowIfEmpty();
				var source = CurrentHand.IsEmpty ? LastCompletedHand : CurrentHand;
				return source.LastPlayerPlayed;
			}
		}

		private void ThrowIfEmpty()
		{
		
			if (IsEmpty)
				throw new InvalidOperationException();
		}

		public bool IsEmpty
		{
			get { return Hands.Count == 0 || (Hands.Count == 1 && Hands[0].IsEmpty); }
		}

		public ICard LastCardPlayed
		{
			get
			{
				ThrowIfEmpty();
				var source = CurrentHand.IsEmpty ? LastCompletedHand : CurrentHand;
				return source.CardsByPlaySequence().LastOrDefault(x => x != null);
			}
		}

		public void AddNewHand()
		{
			ThrowIfNotPlayerBetSet();

			if (_hands.Count == 10 || _playerCards.All(x => x.Length == 0))
				return;
			var item = new Hand(_cardsComparer, Trump,(byte)( _hands.Count+1),this);
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

		public ICard[] PlayerCards(byte playerPosition)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);
			return _playerCards[playerPosition - 1];
		}

		public void SetCards(byte playerPosition, ICard[] cards)
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
		public byte SumTotal(byte playerPosition)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);

			var result= (byte)Hands.Where(x => x.IsCompleted && x.PlayerWinner == playerPosition).Sum(x => x.Points);
			if(Hands.Count>=10 && Hands[9].IsCompleted && Hands[9].PlayerWinner==playerPosition)
				result += 10;
			return result;
		}

		public byte SumTotalTeam(byte teamNumber)
		{
			ThrowIfNotValidTeamNumber(teamNumber);

			if (teamNumber == 1 || teamNumber == 3)
				return (byte)(SumTotal(1) + SumTotal(3));
			return (byte)(SumTotal(2) + SumTotal(4));
		}


		public void RemovePlayerCard(byte playerPosition, ICard card)
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

		private void ThrowIfNotValidTeamNumber(int teamNumber)
		{
			if (teamNumber < 1 || teamNumber > 2)
				throw new ArgumentOutOfRangeException("teamNumber");
		}

		public void SetPlayerBet(byte playerPosition, byte pointsBet)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);
			if(pointsBet<0) throw new ArgumentOutOfRangeException("pointsBet","Must be 0 or higher");

			PlayerBets = playerPosition;
			PointsBet = pointsBet;
		}

		public void AddHand(IHand hand)
		{
			EnsureHands();
			if(_hands.Any(x=>x.Sequence==hand.Sequence)) 
				throw new InvalidOperationException();
			_hands.Add(hand);
		}

		public void SetTrump(ISuit trump)
		{
			Trump = trump;
		}
	}
}
