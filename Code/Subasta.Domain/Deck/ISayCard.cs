namespace Subasta.Domain.Deck
{
    public interface ISayCard : ICard
    {
        bool Marked { get; set; }
    }
}