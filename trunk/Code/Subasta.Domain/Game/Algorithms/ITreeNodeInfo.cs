namespace Subasta.Domain.Game.Algorithms
{
	public interface ITreeNodeInfo
	{
		float Coeficient { get; }

		float TotalValue { get; }
		float AvgPoints { get; }
		int NumberVisits { get; }

		float PercentageChancesOfMaking(int points);
		int GetMaxPointsWithMinimumChances(double percentaje);
	}
}