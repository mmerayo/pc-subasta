namespace Subasta.Domain.Game.Algorithms
{
	public interface ITreeNodeInfo
	{
		double Coeficient { get; }

		double TotalValue { get; }
		double AvgPoints { get; }
		int NumberVisits { get; }

		double PercentageChancesOfMaking(int points);
	}
}