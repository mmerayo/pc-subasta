using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Mocks.Interfaces;

namespace Subasta.Infrastructure.UnitTests.Tools
{
	public static class RhinoExtensions
	{
		public static void OverridePrevious<T>(this IMethodOptions<T> options)
		{
			options.Repeat.Any();
		}
	}
}
