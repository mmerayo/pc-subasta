using System;

namespace Subasta.Client.Common.Game
{
	public interface IUserInteractionManager
	{
		void Reset();
		TInput WaitUserInput<TInput>();
		void InputProvided<TInput>(Func<TInput> action);
	}
}