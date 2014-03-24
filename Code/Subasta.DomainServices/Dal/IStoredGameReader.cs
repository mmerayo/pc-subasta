namespace Subasta.Client.Common.Games
{
	public interface IStoredGameReader
	{
		StoredGameData Load(string file);
	}
}