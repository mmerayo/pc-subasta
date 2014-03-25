using System;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Players
{
	internal class HumanPlayer : Player, IHumanPlayer
	{
		private readonly ICandidatePlayer _candidatePlayer;
		public event MoveSelectionNeeded SelectMove;

		public HumanPlayer(ICandidatePlayer candidatePlayer)
		{
			_candidatePlayer = candidatePlayer;
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
			var onMoveSelectionNeeded = OnMoveSelectionNeeded();
			var status=_candidatePlayer.PlayCandidate(currentStatus, currentStatus.Turn, onMoveSelectionNeeded);//TODO: falta mecanismo para los CANTES

			return new NodeResult(status);
		}

		private ICard OnMoveSelectionNeeded()
		{
			if (SelectMove != null)
				return SelectMove(this);
			throw new InvalidOperationException("The event SelectMove on human players need to have one suscriptor");
		}

	}
}