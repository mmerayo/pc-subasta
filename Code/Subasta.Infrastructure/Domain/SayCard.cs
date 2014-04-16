using Subasta.Domain.Deck;

namespace Subasta.Infrastructure.Domain
{
    class SayCard : Card,ISayCard
    {
        internal SayCard(ISuit suit, int number) : base(suit, number)
        {
        }

        public SayCard(string shortId) : base(shortId)
        {
        }

        public SayCard(string suitName, int number) : base(suitName, number)
        {
        }

        private SayCard(ICard playerCard) : this(playerCard.Suit.Name,playerCard.Number)
        {
        }

        public bool Marked { get; set; }
	    public bool MarkedAsCandidate { get; set; }

	    public static ISayCard[] FromCards(ICard[] playerCards)
        {
            var result=new ISayCard[playerCards.Length];
            for (int index = 0; index < playerCards.Length; index++)
            {
                result[index]=new SayCard(playerCards[index]);
            }
            return result;
        }
    }
}