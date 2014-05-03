using System;
using System.Linq;
using Subasta.Domain;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game.Algorithms.MCTS;
using Subasta.DomainServices.Game.Models;

namespace Subasta.DomainServices.Game.Players
{
	internal sealed class Game : IGame
	{
		//TODO: CREATE GAME State machine to handle marque(in another class) & game

		private readonly ICardComparer _cardComparer;
		private readonly IPlayerDeclarationsChecker _declarationsChecker;
		private readonly IPlayer[] _players = new IPlayer[4];
		private readonly ISaysSimulator _saysRunner;
		private ISaysStatus _saysStatus;

		public Game(ICardComparer cardComparer,
		            IPlayerDeclarationsChecker declarationsChecker,
		            ISimulator aiSimulator, ISaysSimulator saysSimulator)
		{
			AiSimulator = aiSimulator;
			_cardComparer = cardComparer;
			_declarationsChecker = declarationsChecker;
			_saysRunner = saysSimulator;
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
		public event GameStatusChangedHandler HandCompleted;
		public event GamePlayerPetaHandler GamePlayerPeta;
		public event GamePlayerDeclaration PlayerDeclarationEmitted;
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

		public void StartGame()
		{
			TreeNode root = RunSays();
			if (_saysStatus.PointsBet == 0)
				return; //TODO: DONE

			RunGame(root);
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
			status.Turn = FirstPlayer;

			return status;
		}

		private ISaysStatus GetInitialSaysStatus()
		{
			return new SaysStatus(Status.Clone(), FirstPlayer);
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

			int playerBets = _saysStatus.PlayerBets;
			ISuit chooseTrump = _players[playerBets - 1].ChooseTrump(_saysStatus);
			int pointsBet = _saysStatus.PointsBet*10;

			Status.SetPlayerBet(playerBets, pointsBet);
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
			OnGameCompleted();
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
					x => x.PlayerType == PlayerType.Human && x.TeamNumber == result.Status.LastCompletedHand.TeamWinner.Value))
				{
					var calculatedDeclarationByMachine = Status.LastCompletedHand.Declaration;
					Status.LastCompletedHand.SetDeclaration(null);
					OnGameStatusChanged();
					//always the first
					IPlayer player =
						_players.First(
							x => x.PlayerType == PlayerType.Human && x.TeamNumber == result.Status.LastCompletedHand.TeamWinner.Value);

					//Add the hand to chose a declaration
					previousStatus.CurrentHand.Add(previousStatus.Turn, result.Status.LastCompletedHand.CardsByPlaySequence().Last());
					Declaration? declarationChosenByHuman = player.ChooseDeclaration(previousStatus);


					//AS-IS NOW: The human player must select a declaration
					//if the user didnt select any then the machine selects while is on its hand
					if (!declarationChosenByHuman.HasValue &&
						calculatedDeclarationByMachine.HasValue)
					{
						int playerMateOf = Status.PlayerMateOf(player.PlayerNumber);
						if (Status.GetPlayerDeclarables(playerMateOf).Contains(calculatedDeclarationByMachine.Value))
						{
							declarationChosenByHuman = calculatedDeclarationByMachine;
						}
						else
						{
							//choose the first
							declarationChosenByHuman =
								Status.GetPlayerDeclarables(playerMateOf).OrderBy(x => x).FirstOrDefault();
						}
					}
					Status.LastCompletedHand.SetDeclaration(declarationChosenByHuman);
				}


				OnHandCompleted(Status);
			}

		}

		private void OnHandCompleted(IExplorationStatus status)
		{
			Declaration? declaration = status.LastCompletedHand.Declaration;
			if(declaration.HasValue)
			foreach (var player in _players)
			{
				if(_declarationsChecker.HasDeclarable(declaration.Value, status.Trump, player.Cards))
				{
					OnPlayerDeclarationEmitted(player,declaration.Value);
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
				PlayerDeclarationEmitted(player,declaration);
			}

	}
}