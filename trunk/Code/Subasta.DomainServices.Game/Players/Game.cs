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
		private IExplorationStatus _status;

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

		public void SetGameInfo(IPlayer p1, IPlayer p2, IPlayer p3, IPlayer p4, int firstPlayer)
		{
			_players[0] = p1;
			_players[1] = p2;
			_players[2] = p3;
			_players[3] = p4;

			FirstPlayer = firstPlayer;

			_status = GetInitialStatus();
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
			return new SaysStatus(_status.Clone(), FirstPlayer);
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

			_status.SetPlayerBet(playerBets, pointsBet);
			_status.SetTrump(chooseTrump);

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

			AiSimulator.Start(_status, root);
			OnGameStarted();
			while (!_status.IsCompleted)
			{
				NextMove();
				OnGameStatusChanged();
			}
			AiSimulator.Reset();
			OnGameCompleted();
		}

		private void NextMove()
		{
			IExplorationStatus previousStatus = _status.Clone();
			IPlayer playerMoves = _players[_status.Turn - 1];
			bool peta;
			NodeResult result = playerMoves.ChooseMove(_status, out peta);

			//TODO: CREATE USER STATUS AND EXPLORATION STATUS types and encapsulate the logical complete as default
			_status = result.Status.Clone();
			_status.LogicalComplete = true;
			OnGameStatusChanged();
			if (peta)
			{
				OnPlayerPeta(playerMoves);
			}

			//if its the last card of the hand AND
			//the current hand winner has a human in the team
			if (previousStatus.CurrentHand.CardsByPlaySequence().Count(x => x != null) == 3)
			{
				if (_players.Any(
					x => x.PlayerType == PlayerType.Human && x.TeamNumber == result.Status.LastCompletedHand.TeamWinner.Value))
				{
					var calculatedDeclarationByMachine = _status.LastCompletedHand.Declaration;
					_status.LastCompletedHand.SetDeclaration(null);
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
						if (_status.GetPlayerDeclarables(_status.PlayerMateOf(player.PlayerNumber)).Contains(calculatedDeclarationByMachine.Value))
						{
							declarationChosenByHuman = calculatedDeclarationByMachine;
						}
						else
						{
							//choose the first
							declarationChosenByHuman =
								_status.GetPlayerDeclarables(_status.PlayerMateOf(player.PlayerNumber)).OrderBy(x => x).FirstOrDefault();
						}
					_status.LastCompletedHand.SetDeclaration(declarationChosenByHuman);
				}
				OnHandCompleted(_status);
			}

		}

		private void OnHandCompleted(IExplorationStatus status)
		{
			if (HandCompleted != null)
				HandCompleted(status);
		}

		private void OnPlayerPeta(IPlayer playerMoves)
		{
			if (GamePlayerPeta != null)
				GamePlayerPeta(playerMoves, _status);
		}


		private void OnGameStatusChanged()
		{
			if (GameStatusChanged != null)
				GameStatusChanged(_status);
		}

		private void OnGameStarted()
		{
			if (GameStarted != null)
				GameStarted(_status);
		}

		private void OnGameCompleted()
		{
			if (GameCompleted != null)
				GameCompleted(_status);
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
	}
}