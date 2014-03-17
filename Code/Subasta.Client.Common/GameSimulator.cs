using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
		private readonly IGameExplorer _explorer;
		private readonly IDeck _deck;
		private readonly IPlayer[] _players = new IPlayer[4];
		private IExplorationStatus _status;
		private int _depth;
		private int _firstPlayer = 1;

		public event StatusChangedHandler GameStatusChanged;
		public event StatusChangedHandler GameStarted;
		public event InputRequestedHandler InputRequested;
		//pending declarations per hand/team
		private readonly Dictionary<int, Declaration?> _pendingDeclarations = new Dictionary<int, Declaration?>();
		private Stopwatch _perMoveWatcher;
		public GameSimulator(IGameExplorer explorer,IDeck deck)
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

		public void Start(int depth=int.MinValue)
		{
			if(depth>0)
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
				OnStatusChanged(_perMoveWatcher.Elapsed);
				_perMoveWatcher.Stop();
				if (_status.CurrentHand.IsCompleted)
				{
					Debug.Assert(!_status.CurrentHand.Declaration.HasValue);
					int value = _status.CurrentHand.PlayerWinner.Value;
					if (_pendingDeclarations[value].HasValue)
					{
						_status.CurrentHand.Add(_pendingDeclarations[value].Value);
						OnStatusChanged(TimeSpan.Zero);
					}
					_explorer.MaxDepth++;
					//OnInputRequested();
					_status.Turn = value;
					_status.AddNewHand();
					_pendingDeclarations.Clear();

				}

			}

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
			var nodeResult = _explorer.Execute(_status); //TODO: TURN NEEDED??

			int playerPlays = _status.Turn;
			ICard cardAtMove = nodeResult.CardAtMove(playerPlays, _status.Hands.Count);
			_pendingDeclarations.Add(playerPlays, nodeResult.PotentialDeclaration(_status.Hands.Count));
			_status.CurrentHand.Add(playerPlays, cardAtMove);
			_status.RemovePlayerCard(playerPlays, cardAtMove);
			if (++playerPlays > 4) playerPlays = 1;
			_status.Turn = playerPlays;

		}

		private void OnStatusChanged(TimeSpan timeTaken)
		{
			if (GameStatusChanged != null)
				GameStatusChanged(_status,timeTaken);
		}


		private void OnInputRequested()
		{
			if (InputRequested != null)
				InputRequested();
		}

		private void OnStart()
		{
			if (GameStarted != null)
				GameStarted(_status,TimeSpan.Zero);
		}


	}
}