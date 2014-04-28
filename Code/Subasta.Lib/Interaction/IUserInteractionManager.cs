using System;

namespace Subasta.Lib.Interaction
{
	public interface IUserInteractionManager
	{
		void Reset();
		TInput WaitUserInput<TInput>();
		void InputProvided<TInput>(Func<TInput> action);
	}
}