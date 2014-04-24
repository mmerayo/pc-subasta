using System;

namespace Subasta.Interaction
{
	public interface IUserInteractionManager
	{
		void Reset();
		void WaitUserInput();
		void InputProvided(Action action);
	}
}