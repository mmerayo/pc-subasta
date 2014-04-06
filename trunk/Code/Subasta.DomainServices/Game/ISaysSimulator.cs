using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game
{
	public interface ISaysSimulator
	{
		void Start(ISaysStatus sourceStatus);
		void Reset();
		SayKind GetSay(ISaysStatus saysStatus);
	}
}