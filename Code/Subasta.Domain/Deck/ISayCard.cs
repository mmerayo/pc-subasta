namespace Subasta.Domain.Deck
{
    public interface ISayCard : ICard
    {
        bool Marked { get; set; }
	    bool MarkCandidate { get; set; }
    }
}