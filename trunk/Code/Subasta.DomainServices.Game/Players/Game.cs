using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.DataAccess;
using Subasta.DomainServices.Game.Models;

namespace Subasta.DomainServices.Game.Players
{
	internal sealed class Game : IGame
	{
		//TODO: CREATE GAME State machine to handle marque(in another class) & game

		private readonly ICardComparer _cardComparer;
		private readonly IPlayerDeclarationsChecker _declarationsChecker;
		private readonly IPlayer[] _players = new IPlayer[4];
		private IExplorationStatus _status;

		public event GameStatusChangedHandler GameStatusChanged;
		public event GameStatusChangedHandler GameStarted;
		public event GameStatusChangedHandler GameCompleted;

		public IPlayer Player1
		{
			get { return _players[0]; }
		}

		public IPlayer Player2
		{
			get { return _players[1]; }
		}

		public IPlayer Player3
		{
			get { return _players[2]; }
		}

		public IPlayer Player4
		{
			get { return _players[3]; }
		}

		public ISuit Trump { get; private set; }

		public int TeamBets { get; private set; }
		public int FirstPlayer { get; private set; }
		public int PointsBet { get; private set; }

		public Game(ICardComparer cardComparer,
		            IPlayerDeclarationsChecker declarationsChecker)
		{
			_cardComparer = cardComparer;
			_declarationsChecker = declarationsChecker;
		}

		public void SetGameInfo(IPlayer p1, IPlayer p2, IPlayer p3, IPlayer p4, int firstPlayer, int teamBets, ISuit trump,
		                        int pointsBet)
		{
			_players[0] = p1;
			_players[1] = p2;
			_players[2] = p3;
			_players[3] = p4;

			Trump = trump;
			TeamBets = teamBets;
			FirstPlayer = firstPlayer;
			PointsBet = pointsBet;

			_status = GetInitialStatus();

			GameCompleted = null;
			GameStarted = null;
			GameStatusChanged = null;
		}

		private IExplorationStatus GetInitialStatus()
		{
			var status = new Status(Guid.NewGuid(), _cardComparer, Trump, _declarationsChecker);
			status.SetCards(1, Player1.Cards);
			status.SetCards(2, Player2.Cards);
			status.SetCards(3, Player3.Cards);
			status.SetCards(4, Player4.Cards);
			status.SetPlayerBet(TeamBets, PointsBet);
			status.Turn = FirstPlayer;

			return status;
		}


		public void StartGame()
		{
			//TODO:state machine here Marque primero(EN OTRA CLASe) y juego despues en esta

			Player1.SetNewGame(_status);
			Player2.SetNewGame(_status);
			Player3.SetNewGame(_status);
			Player4.SetNewGame(_status);
			OnStart();
			while (!_status.IsCompleted)
			{
				NextMove();
				OnStatusChanged();
			}

			OnCompleted();

		}

		private void NextMove()
		{
			var playerMoves = _players[_status.Turn - 1];
			var result = playerMoves.ChooseMove(_status);

			_status = result.Status.Clone();
		}


		private void OnStatusChanged()
		{
			if (GameStatusChanged != null)
				GameStatusChanged(_status);
		}

		private void OnStart()
		{
			if (GameStarted != null)
				GameStarted(_status);
		}

		private void OnCompleted()
		{
			
			if (GameCompleted != null)
				GameCompleted(_status);
		}
	}
}
