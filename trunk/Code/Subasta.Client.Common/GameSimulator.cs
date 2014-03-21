using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using StructureMap;
using Subasta.Client.Common.Games;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game;

namespace Subasta.Client.Common
{
	public class GameSimulator : IGameSimulator
	{
		private readonly ISimulator _explorer;
		private readonly IDeck _deck;
		private readonly IPlayer[] _players = new IPlayer[4];
		private IExplorationStatus _status;
		private int _depth;
		private int _firstPlayer = 1;

		public event StatusChangedHandler GameStatusChanged;
		public event StatusChangedHandler GameStarted;
		public event StatusChangedHandler GameCompleted;
		public event InputRequestedHandler InputRequested;
		//pending declarations per hand/team
		private readonly Dictionary<int, NodeResult> _currentMoveNodes = new Dictionary<int, NodeResult>();
		private Stopwatch _perMoveWatcher;

		public GameSimulator(ISimulator explorer, IDeck deck)
		{
			_explorer = explorer;
			_deck = deck;
			ReloadPlayers();
			_depth = _explorer.MaxDepth;
		}

		private void ReloadPlayers()
		{
			_players[0] = ObjectFactory.GetInstance<IPlayer>();
			_players[1] = ObjectFactory.GetInstance<IPlayer>();
			_players[2] = ObjectFactory.GetInstance<IPlayer>();
			_players[3] = ObjectFactory.GetInstance<IPlayer>();

			_players[0].Name = "Player 1";
			_players[1].Name = "Player 2";
			_players[2].Name = "Player 3";
			_players[3].Name = "Player 4";

		}

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

		public int Depth
		{
			get { return _depth; }
		}


		public ISuit Trump { get; private set; }
		public int PointsBet { get; private set; }


		public int PlayerBets { get; private set; }

		public void Start(int depth = int.MinValue)
		{
			if (depth > 0)
			{
				_depth = depth;
			}
			_explorer.MaxDepth = _depth;

			ValidateConfiguration();

			_status = _explorer.GetInitialStatus(Guid.NewGuid(), FirstPlayer, PlayerBets, _players[0].Cards, _players[1].Cards,
			                                     _players[2].Cards, _players[3].Cards, Trump, PointsBet);
			OnStart();
			_perMoveWatcher = Stopwatch.StartNew();
			while (!_status.IsCompleted)
			{
				_perMoveWatcher.Restart();
				NextMove();
				TimeSpan timeTaken = _perMoveWatcher.Elapsed;
				OnStatusChanged(timeTaken);
				_perMoveWatcher.Stop();
				if (_status.CurrentHand.IsCompleted)
				{
					Debug.Assert(!_status.CurrentHand.Declaration.HasValue);
					int playerWinner = _status.CurrentHand.PlayerWinner.Value;
					if (_status.IsInTeamBets(playerWinner))
					{
						//declare winner or mate
						Declaration? declarationAtMove = _currentMoveNodes[playerWinner].FirstDeclarable(_status.Hands.Count);
						if (!declarationAtMove.HasValue)
						{
							int matePlayer = _status.PlayerMateOf(playerWinner);

							declarationAtMove = _currentMoveNodes[matePlayer].FirstDeclarable(_status.Hands.Count);
						}
						if (declarationAtMove.HasValue)
						{
							_status.CurrentHand.SetDeclaration(declarationAtMove.Value);
							OnStatusChanged(timeTaken);
						}


					}
					_explorer.MaxDepth++;
					//OnInputRequested();
					_status.Turn = playerWinner;
					_status.AddNewHand();
					_currentMoveNodes.Clear();

				}

			}
			OnCompleted();
			OnInputRequested();
		}



		public void Load(StoredGameData storedGame)
		{
			ReloadPlayers();
			Player1.Cards = storedGame.Player1Cards;
			Player2.Cards = storedGame.Player2Cards;
			Player3.Cards = storedGame.Player3Cards;
			Player4.Cards = storedGame.Player4Cards;
			_depth = storedGame.ExplorationDepth;
			FirstPlayer = storedGame.FirstPlayer;
			Trump = storedGame.Trump;
			PointsBet = storedGame.PointsBet;
			PlayerBets = storedGame.PlayerBets;
		}

		private void ValidateConfiguration()
		{
			if (_players.Any(player => player.Cards == null))
			{
				throw new InvalidOperationException("Must set player cards");
			}

			var allCards = new List<ICard>();
			foreach (var player in _players)
			{
				allCards.AddRange(player.Cards);
			}

			var expectedCards = _deck.Cards.Cards;

			foreach (var expectedCard in expectedCards)
			{
				if (allCards.Count(x => x.Equals(expectedCard)) != 1)
					throw new InvalidOperationException(string.Format("{0} is either repeated or it does not exist", expectedCard));
			}

		}


		public bool IsFinished { get; set; }

		public int FirstPlayer
		{
			get { return _firstPlayer; }
			private set { _firstPlayer = value; }
		}



		public void NextMove()
		{
			Thread.Sleep(TimeSpan.FromSeconds(1));
			var nodeResult = _explorer.GetBest(_status); //TODO: TURN NEEDED??

			int playerPlays = _status.Turn;
			ICard cardAtMove = nodeResult.CardAtMove(playerPlays, _status.Hands.Count);
			_currentMoveNodes.Add(playerPlays, nodeResult);
			_status.CurrentHand.Add(playerPlays, cardAtMove);
			_status.RemovePlayerCard(playerPlays, cardAtMove);
			if (++playerPlays > 4) playerPlays = 1;
			_status.Turn = playerPlays;

		}

		private void OnStatusChanged(TimeSpan timeTaken)
		{
			if (GameStatusChanged != null)
				GameStatusChanged(_status, timeTaken);
		}


		private void OnInputRequested()
		{
			if (InputRequested != null)
				InputRequested();
		}

		private void OnStart()
		{
			if (GameStarted != null)
				GameStarted(_status, TimeSpan.Zero);
		}

		private void OnCompleted()
		{
		if (GameCompleted!= null)
			GameCompleted(_status, TimeSpan.Zero);



		}
	}
}