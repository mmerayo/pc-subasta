using System;
using System.Collections.Generic;
using System.Linq;
using Subasta.Domain;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Players
{
	internal class HumanPlayer : Player, IHumanPlayer
	{
		private readonly ICandidatePlayer _candidatePlayer;
		private readonly IValidCardsRule _validCardsRule;
		private readonly IPlayerDeclarationsChecker _playerDeclarationsChecker;
		public event MoveSelectionNeeded SelectMove;
		public event DeclarationSelectionNeeded SelectDeclaration;
		public event SayNeededEvent SelectSay;
		public event TrumpNeededEvent ChooseTrumpRequest;
		


		public HumanPlayer(ICandidatePlayer candidatePlayer,IValidCardsRule validCardsRule,IPlayerDeclarationsChecker playerDeclarationsChecker)
		{
			_candidatePlayer = candidatePlayer;
			_validCardsRule = validCardsRule;
			_playerDeclarationsChecker = playerDeclarationsChecker;
		}

		public override PlayerType PlayerType
		{
			get
			{
				return PlayerType.Human;
			}
		}

		public override NodeResult ChooseMove(IExplorationStatus currentStatus, out bool peta)
		{
			var onMoveSelectionNeeded = OnMoveSelectionNeeded(currentStatus,out peta);
			var status=_candidatePlayer.PlayCandidate(currentStatus, currentStatus.Turn, onMoveSelectionNeeded);
			

			return new NodeResult(status);
		}

		public override Declaration? ChooseDeclaration(IExplorationStatus status)
		{
			return OnDeclarationSelectionNeeded(status);
		}

		public override IFigure ChooseSay(ISaysStatus saysStatus)
		{
			return OnSayRequired(saysStatus);
		}

		public override ISuit ChooseTrump(ISaysStatus saysStatus)
		{
			return OnTrumpChoiceRequired(saysStatus);
		}

		public IEnumerable<Declaration> GetUserDeclarables(IExplorationStatus status)
		{
			return status.GetPlayerDeclarables(PlayerNumber);
		}

		private Declaration? OnDeclarationSelectionNeeded(IExplorationStatus status)
		{
			var declarables = status.Declarables;
			if (declarables == null || declarables.Length == 0) return null;

			if (SelectDeclaration != null)
			{
				return SelectDeclaration(this, declarables,status);
			}
			throw new InvalidOperationException("The event SelectDeclaration on human players need to have one suscriptor");
		}

		private ICard OnMoveSelectionNeeded(IExplorationStatus currentStatus, out bool peta)
		{
			if (SelectMove != null)
			{
				ICard[] validMoves = _validCardsRule.GetValidMoves(currentStatus.PlayerCards(PlayerNumber),
				                                                   currentStatus.CurrentHand);
				return SelectMove(this, validMoves,out peta);
			}
			throw new InvalidOperationException("The event SelectMove on human players need to have one suscriptor");
		}


		private IFigure OnSayRequired(ISaysStatus saysStatus)
		{
			if (SelectSay != null)
			{
				return SelectSay(this,saysStatus);
			}
			throw new InvalidOperationException("The event SelectSay on human players need to have one suscriptor");
		}

		private ISuit OnTrumpChoiceRequired(ISaysStatus saysStatus)
		{
			if (ChooseTrumpRequest != null)
			{
				return ChooseTrumpRequest(this);
			}
			throw new InvalidOperationException("The event ChooseTrumpRequest on human players need to have one suscriptor");
		}

	}
}