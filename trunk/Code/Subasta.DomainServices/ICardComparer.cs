namespace Subasta.DomainServices
{
	interface ICardComparer
	{
		ICard Best(ICard current, ICard candidate);
	}
}
