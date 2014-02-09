﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices;
using Subasta.Infrastructure.DomainServices;

namespace Subasta.Infrastructure.Domain
{
	internal class Status : IExplorationStatus
	{
		private readonly ICardComparer _cardsComparer;
		private readonly IPlayerDeclarationsChecker _declarationsChecker;
		private int _turn = int.MinValue;
		private readonly ICard[][] _playerCards = new ICard[4][];
		private List<IHand> _hands;

		public Status(ICardComparer cardsComparer, ISuit trump, IPlayerDeclarationsChecker declarationsChecker)
		{
			Trump = trump;
			_cardsComparer = cardsComparer;
			_declarationsChecker = declarationsChecker;
			PlayerBets = int.MinValue;
		}

		public IExplorationStatus Clone()
		{
			var status = new Status(_cardsComparer, Trump, _declarationsChecker) {_turn = _turn, PlayerBets = PlayerBets};
			Array.Copy(_playerCards, status._playerCards, 4);

			status._hands = new List<IHand>();
			_hands.ForEach(x => status._hands.Add(x.Clone()));

			//CALCULATE DECLARABLES
			return status;
		}

		public ISuit Trump { get; private set; }

		//el jugador que la pone
		public int PlayerBets { get; private set; }

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
				IHand last = Hands.LastOrDefault();
				if (last == null || !last.IsCompleted || last.Declaration.HasValue)
					return new Declaration[0];

				var candidates = GetDeclarationCandidates();
				//havent been applied yet 
				candidates.RemoveAll(y => Hands.Where(x => x.Declaration.HasValue).Select(x => x.Declaration.Value).Contains(y));

				return candidates.ToArray();
			}
		}

		private List<Declaration> GetDeclarationCandidates()
		{

			var last = Hands.Last(x => x.IsCompleted);
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
				result.AddRange(
					declarables.Where(
						declarable => _declarationsChecker.HasDeclarable(declarable, Trump, _playerCards[teamPlayers[i] - 1])));

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


		public void AddNewHand()
		{
			ThrowIfNotPlayerBetSet();

			if (_hands.Count == 10 || _playerCards.All(x => x.Length == 0))
				return;
			var item = new Hand(_cardsComparer, Trump);
			_hands.Add(item);
		}

		private void ThrowIfNotPlayerBetSet()
		{
			if (PlayerBets == int.MinValue)
				throw new InvalidOperationException("Player bets need to be set");
		}

		public ICard[] PlayerCards(int playerPosition)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);
			return _playerCards[playerPosition - 1];
		}

		public void SetCards(int playerPosition, ICard[] cards)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);

			_playerCards[playerPosition - 1] = cards;

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

			return Hands.Where(x => x.IsCompleted && x.PlayerWinner == playerPosition).Sum(x => x.Points);
		}

		private static void ThrowIfNotValidPlayerPosition(int playerPosition, string argName = "playerPosition")
		{
			if (playerPosition < 1 || playerPosition > 4)
				throw new ArgumentOutOfRangeException(argName);
		}


		public void SetPlayerBet(int playerPosition)
		{
			ThrowIfNotValidPlayerPosition(playerPosition);
			PlayerBets = playerPosition;
		}

		public void AddHand(IHand hand)
		{
			EnsureHands();
			_hands.Add(hand);
		}
	}
}