using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subasta.ApplicationServices
{
	public interface IGameGenerator
	{
		Guid GenerateNewGame();
	}
}
