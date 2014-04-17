using System;
using System.Collections.Generic;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.DomainServices.Game.Algorithms.Figures.Catalog
{
	internal abstract class Figure : IFigure
	{
		//stands for figures that can be repeated (for different cards)like aces
		public abstract bool CanBeRepeated { get; }

		public abstract SayKind Say { get; }
		public abstract SayKind AlternativeSay { get; }
		public abstract int PointsBet { get; }
		public abstract int AlternativePointsBet { get; }

		private readonly List<ISayCard> _potentiallyMarkedCards = new List<ISayCard>();

		protected void BookCard(ISayCard card)
		{
			if (card == null) return;

			if (_potentiallyMarkedCards.Contains(card)) throw new ArgumentException();

			card.MarkedAsCandidate = true;
			_potentiallyMarkedCards.Add(card);

		}

		//TODO: TO ABSTRACT
		public void MarkFigures(ISaysStatus saysStatus)
		{
			foreach (var potentiallyMarkedCard in _potentiallyMarkedCards)
			{
				potentiallyMarkedCard.Marked = true;
			}
		}

		public void UnMarkPotentialCandidates()
		{
			foreach (var potentiallyMarkedCard in _potentiallyMarkedCards)
			{
				potentiallyMarkedCard.MarkedAsCandidate = false;
			}
			_potentiallyMarkedCards.Clear();
		}



		public ICard[] MarkedCards
		{
			get { return _potentiallyMarkedCards.Cast<ICard>().ToArray(); }
		}

		public virtual bool IsAvailable(ISaysStatus saysStatus, int normalizedTopPoints)
		{
			bool alreadyUsed = saysStatus.Says.Any(x => x.PlayerNum == saysStatus.Turn && x.Figure.Say == Say);
			if (!CanBeRepeated && alreadyUsed)
				return false;

			ISayCard[] playerCards = saysStatus.GetPlayerCards(saysStatus.Turn);
			bool result = false;
			if (GetPotentialPointsBet(saysStatus, alreadyUsed) <= normalizedTopPoints)
			{
				ISayCard[] cards;

				if (HasCandidates(playerCards, out cards))
				{
					foreach (var sayCard in cards)
					{
						BookCard(sayCard);
					}

					result = true;

				}
			}
			return result;
		}

		private int GetPotentialPointsBet(ISaysStatus saysStatus, bool alreadyUsed)
		{
			return !alreadyUsed ? saysStatus.PointsBet + PointsBet : saysStatus.PointsBet + AlternativePointsBet;
		}


		private bool ContainsCardsOfSuit(IEnumerable<ISayCard> source, ISuit suit, IEnumerable<int> cardNumbers,
			IEnumerable<int> notHavingCardNumbers)
		{
			bool containsCardsOfSuit = source.Count(x => x.Suit.Equals(suit) && cardNumbers.Contains(x.Number)) ==
			                           cardNumbers.Count();
			return containsCardsOfSuit && !source.Any(x => x.Suit.Equals(suit) && notHavingCardNumbers.Contains(x.Number));
		}

		private ISayCard[] GetCardsSubset(IEnumerable<ISayCard> source, ISuit suit, IEnumerable<int> cardNumbers)
		{
			return source.Where(x => x.Suit.Equals(suit) && cardNumbers.Contains(x.Number)).ToArray();
		}

		private bool TryGetCandidateCardsWhenMatch(IEnumerable<ISayCard> source, int[] cardNumbers, int[] notHavingCardNumbers,
			ISuit suit, out ISayCard[] cards)
		{
			var items = source.Where(x => !x.Marked && !x.MarkedAsCandidate && x.Suit.Equals(suit)).ToList();

			cards = new ISayCard[0];
			if (ContainsCardsOfSuit(items, suit, cardNumbers, notHavingCardNumbers))
			{
				cards = GetCardsSubset(items, suit, cardNumbers);
				return true;
			}
			return false;
		}

		private bool HasCandidates(ISayCard[] playerCards, out ISayCard[] cards)
		{
			if (!HavingCardNumberCombinations.Any())
			{
				cards = new ISayCard[0];
				return true;
			}
			if (CombinationPerSuit)
			{

				foreach (var suit in Suit.Suits)
				{
					foreach (var combination in HavingCardNumberCombinations)
					{
						if (TryGetCandidateCardsWhenMatch(playerCards, combination, NotHavingCardNumbers, suit, out cards))
							return true;
					}

				}

			}
			else
			{
				var havingCardNumberCombination = HavingCardNumberCombinations.First()[0];

				if (playerCards.Count(x => !x.Marked && !x.MarkedAsCandidate && x.Number == havingCardNumberCombination) >=
				    CardRepetitionMin)
				{
					cards =
						playerCards.Where(x => !x.Marked && !x.MarkedAsCandidate && x.Number == havingCardNumberCombination).ToArray();
					return true;
				}
			}
			cards = new ISayCard[0];
			return false;
		}

		protected abstract bool CombinationPerSuit { get; }

		protected abstract IEnumerable<int[]> HavingCardNumberCombinations { get; }
		protected abstract int[] NotHavingCardNumbers { get; }

		protected virtual int CardRepetitionMin
		{
			get { return 4; }
		}

		public override string ToString()
		{
			return Say.ToString();
		}
	}
}