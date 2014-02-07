namespace Games.Subasta.GameGeneration.AI
{
	internal interface IGameExplorationAlgorithm
	{
		MaxN.NodeResult Execute(Status currentStatus, int playerPosition);
	}
}