using System;
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
		public event MoveSelectionNeeded SelectMove;
		public event DeclarationSelectionNeeded SelectDeclaration;

		public HumanPlayer(ICandidatePlayer candidatePlayer,IValidCardsRule validCardsRule)
		{
			_candidatePlayer = candidatePlayer;
			_validCardsRule = validCardsRule;
		}

		public override PlayerType PlayerType
		{
			get
			{
				return PlayerType.Human;
			}
		}

		public override NodeResult ChooseMove(IExplorationStatus currentStatus)
		{
			var onMoveSelectionNeeded = OnMoveSelectionNeeded(currentStatus);
			var status=_candidatePlayer.PlayCandidate(currentStatus, currentStatus.Turn, onMoveSelectionNeeded);//TODO: falta mecanismo para los CANTES

			return new NodeResult(status);
		}

		public override Declaration? ChooseDeclaration(IExplorationStatus status)
		{
			return OnDeclarationSelectionNeeded(status);
		}

		private Declaration? OnDeclarationSelectionNeeded(IExplorationStatus status)
		{
			var declarables = status.Declarables;
			if (declarables == null || declarables.Length == 0) return null;

			if (SelectDeclaration != null)
			{
				return SelectDeclaration(this, declarables);
			}
			throw new InvalidOperationException("The event SelectDeclaration on human players need to have one suscriptor");
		}

		private ICard OnMoveSelectionNeeded(IExplorationStatus currentStatus)
		{
			if (SelectMove != null)
			{
				ICard[] validMoves = _validCardsRule.GetValidMoves(currentStatus.PlayerCards(PlayerNumber),
				                                                   currentStatus.CurrentHand);
				return SelectMove(this, validMoves);
			}
			throw new InvalidOperationException("The event SelectMove on human players need to have one suscriptor");
		}

	}
}