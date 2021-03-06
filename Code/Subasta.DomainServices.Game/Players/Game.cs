﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;
using Subasta.Domain;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game.Algorithms.MCTS;
using Subasta.DomainServices.Game.Models;
using Subasta.Infrastructure.Domain;

namespace Subasta.DomainServices.Game.Players
{
	internal sealed class Game : IGame
	{
		//TODO: CREATE GAME State machine to handle marque(in another class) & game

		private readonly ICardComparer _cardComparer;
		private readonly IPlayerDeclarationsChecker _declarationsChecker;
		private readonly IPlayer[] _players = new IPlayer[4];
		private readonly ISaysSimulator _saysRunner;
		private readonly IDeck _deck;
		private ISaysStatus _saysStatus;

		public Game(ICardComparer cardComparer,
			IPlayerDeclarationsChecker declarationsChecker,
			ISimulator aiSimulator, ISaysSimulator saysSimulator,IDeck deck)
		{
			AiSimulator = aiSimulator;
			_cardComparer = cardComparer;
			_declarationsChecker = declarationsChecker;
			_saysRunner = saysSimulator;
			_deck = deck;
		}

		public ISimulator AiSimulator { get; set; }
		public ISuit Trump { get; private set; }

		public int TeamBets { get; private set; }
		public int FirstPlayer { get; private set; }
		public int PointsBet { get; private set; }

		#region IGame Members

		public event GameStatusChangedHandler GameStatusChanged;
		public event GameStatusChangedHandler GameStarted;
		public event GameStatusChangedHandler GameCompleted;
		public event GameEndHandler GameEnded;
		public event GameStatusChangedHandler HandCompleted;
		public event GamePlayerPetaHandler GamePlayerPeta;
		public event GamePlayerDeclarationHandler PlayerDeclarationEmitted;
		public event GameSaysStatusChangedHandler GameSaysStatusChanged;
		public event GameSaysStatusChangedHandler GameSaysStarted;
		public event GameSaysStatusChangedHandler GameSaysCompleted;

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

		public IExplorationStatus Status { get; private set; }

		public void SetGameInfo(IPlayer p1, IPlayer p2, IPlayer p3, IPlayer p4, int firstPlayer)
		{
			_players[0] = p1;
			_players[1] = p2;
			_players[2] = p3;
			_players[3] = p4;

			FirstPlayer = firstPlayer;

			Status = GetInitialStatus();
			_saysStatus = GetInitialSaysStatus();
			ResetEvents();
		}

		private static readonly ILog Logger = LogManager.GetLogger(typeof (Game));
		public void StartGame()
		{
			try
			{
				TreeNode root = RunSays();
				if (_saysStatus.PointsBet == 0)
					return; //TODO: DONE
				RunGame(root);
			}
			catch (Exception ex)
			{
				Logger.Fatal("StartGame",ex);
				throw;
			}
		}

		#endregion

		private void ResetEvents()
		{
			GameCompleted = null;
			GameStarted = null;
			GameStatusChanged = null;

			GamePlayerPeta = null;

			GameSaysStarted = null;
			GameSaysCompleted = null;
			GameSaysStatusChanged = null;
		}


		private IExplorationStatus GetInitialStatus()
		{
			var status = new Status(Guid.NewGuid(), _cardComparer, Trump, _declarationsChecker, true);
			status.SetCards(1, Player1.Cards);
			status.SetCards(2, Player2.Cards);
			status.SetCards(3, Player3.Cards);
			status.SetCards(4, Player4.Cards);
			//status.SetPlayerBet(TeamBets, PointsBet);
			status.Turn = (byte) FirstPlayer;

			return status;
		}

		private ISaysStatus GetInitialSaysStatus()
		{
			return new SaysStatus(Status.Clone(), (byte)FirstPlayer);
		}

		private TreeNode RunSays()
		{
			_saysRunner.Start(_saysStatus);
			OnSaysStart();
			while (!_saysStatus.IsCompleted)
			{
				NextSay();
				OnSaysStatusChanged();
			}

			var playerBets = _saysStatus.PlayerBets;
			var chooseTrump = _players[playerBets - 1].ChooseTrump(_saysStatus);
			var pointsBet = _saysStatus.PointsBet*10;

			Status.SetPlayerBet(playerBets, (byte)pointsBet);
			Status.SetTrump(chooseTrump);

			var result = (TreeNode) _saysRunner.GetRoot(chooseTrump);
			_saysRunner.Reset(result);

			OnSaysCompleted();

			return result;
		}


		private void NextSay()
		{
			IPlayer playerSays = _players[_saysStatus.Turn - 1];
			IFigure result = playerSays.ChooseSay(_saysStatus.Clone());

			_saysStatus.Add(playerSays.PlayerNumber, result);
		}


		private void RunGame(TreeNode root)
		{
			//TODO: not using this until the root can be reused in the game
			root.Dispose();
			root = null;
			//TODO:end TODO

			AiSimulator.Start(Status, root);
			OnGameStarted();
			while (!Status.IsCompleted)
			{
				NextMove();
				OnGameStatusChanged();
			}
			AiSimulator.Reset();
			ReComposeDeck();

			OnGameCompleted();

			OnGameEnded();

		}

		private void ReComposeDeck()
		{
			var cards = new Pack();
			foreach (var hand in Status.Hands)
			{
				cards.AddRange(hand.CardsByPlaySequence().Where(x=>x!=null));
			}

			cards.AddRange(Status.GetCardsNotYetPlayed());
			Debug.Assert(cards.Cards.Count(x => x != null) == 40);
			Debug.Assert(cards.Cards.Distinct().Count(x => x != null) == 40);
			_deck.SetCards(cards);
		}

		private void NextMove()
		{
			IExplorationStatus previousStatus = Status.Clone();
			IPlayer playerMoves = _players[Status.Turn - 1];
			bool peta;
			NodeResult result = playerMoves.ChooseMove(Status, out peta);

			//TODO: CREATE USER STATUS AND EXPLORATION STATUS types and encapsulate the logical complete as default
			Status = result.Status.Clone();
			Status.LogicalComplete = true;
			if (peta)
			{
				OnPlayerPeta(playerMoves);
			}
			OnGameStatusChanged();

			//if its the last card of the hand AND
			//the current hand winner has a human in the team
			if (previousStatus.CurrentHand.CardsByPlaySequence().Count(x => x != null) == 3)
			{
				if (_players.Any( 
					x => x.TeamNumber==result.Status.TeamBets && //el equipo la pone
					 x.PlayerType == PlayerType.Human && 
					 x.TeamNumber == result.Status.LastCompletedHand.TeamWinner.Value)
					
					)
				{
					Declaration? calculatedDeclarationByMachine = Status.LastCompletedHand.Declaration;
					Status.LastCompletedHand.SetDeclaration(null);
					OnGameStatusChanged();
					//always the first
					IPlayer player =
						_players.First(
							x => x.PlayerType == PlayerType.Human && x.TeamNumber == result.Status.LastCompletedHand.TeamWinner.Value);

					//Add the hand to chose a declaration
					ICard lastCardMoved = result.Status.LastCompletedHand.CardsByPlaySequence().Last();

					//TODO: this is redundant
					previousStatus.CurrentHand.Add(previousStatus.Turn, lastCardMoved);
					previousStatus.RemovePlayerCard(result.Status.LastPlayerMoved, lastCardMoved);

					Declaration? declarationChosenByHuman = player.ChooseDeclaration(previousStatus);

					//TODO: REFACTOR THE WHOLE FUCKING METHOD
					//AS-IS NOW: The human player must select a declaration
					//if the user didnt select any then the machine selects while is on its hand
					if (!declarationChosenByHuman.HasValue)
					{
						if (calculatedDeclarationByMachine.HasValue)
						{
							byte playerMateOf = Status.PlayerMateOf(player.PlayerNumber);
							if (Status.GetPlayerDeclarables(playerMateOf).Contains(calculatedDeclarationByMachine.Value))
							{
								declarationChosenByHuman = calculatedDeclarationByMachine;
							}
							else
							{
								//choose the first
								declarationChosenByHuman = TryGetMateDeclaration(player);
							}
						}
						else
						{
							declarationChosenByHuman = TryGetMateDeclaration(player);
						}
					}

					Status.LastCompletedHand.SetDeclaration(declarationChosenByHuman);
				}


				OnHandCompleted(Status);
			}
		}

		private Declaration? TryGetMateDeclaration(IPlayer player)
		{
			byte playerMateOf = Status.PlayerMateOf(player.PlayerNumber);
			//TODO: this should not be random
			var playerDeclarables = Status.GetPlayerDeclarables(playerMateOf).ToArray();
			if (playerDeclarables.Length > 0)
			{
				int idx = new Random((int) DateTime.Now.Ticks).Next(0, playerDeclarables.Count());
				return playerDeclarables[idx];
			}
			
			return null;
		}


		private void OnHandCompleted(IExplorationStatus status)
		{
			Declaration? declaration = status.LastCompletedHand.Declaration;
			if (declaration.HasValue)
				foreach (IPlayer player in _players)
				{
					if (status.TeamBets==player.TeamNumber 
						&& _declarationsChecker.HasDeclarable(declaration.Value, status.Trump, player.Cards))
					{
						OnPlayerDeclarationEmitted(player, declaration.Value);
						break;
					}
				}

			if (HandCompleted != null)
				HandCompleted(status);
		}

		private void OnPlayerPeta(IPlayer playerMoves)
		{
			if (GamePlayerPeta != null)
				GamePlayerPeta(playerMoves, Status);
		}


		private void OnGameStatusChanged()
		{
			if (GameStatusChanged != null)
				GameStatusChanged(Status);
		}

		private void OnGameStarted()
		{
			if (GameStarted != null)
				GameStarted(Status);
		}

		private void OnGameCompleted()
		{
			if (GameCompleted != null)
				GameCompleted(Status);
		}

		private void OnGameEnded()
		{
			if (GameEnded != null)
				GameEnded(Status);
		}

		private void OnSaysStart()
		{
			if (GameSaysStarted != null)
				GameSaysStarted(_saysStatus);
		}


		private void OnSaysCompleted()
		{
			if (GameSaysCompleted != null)
				GameSaysCompleted(_saysStatus);
		}

		private void OnSaysStatusChanged()
		{
			if (GameSaysStatusChanged != null)
				GameSaysStatusChanged(_saysStatus);
		}

		private void OnPlayerDeclarationEmitted(IPlayer player, Declaration declaration)
		{
			if (PlayerDeclarationEmitted != null)
				PlayerDeclarationEmitted(player, declaration);
		}
	}
}