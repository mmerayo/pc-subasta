using System;

namespace Subasta.Domain
{
//sorted by value
	public enum Declaration
	{
		Reyes=1,
		Caballos,
		Cuarenta,
		ParejaOros,
		ParejaCopas,
		ParejaEspadas,
		ParejaBastos,
	}

	public static class DeclarationValues
	{
		public static int ValueOf(Declaration declaration)
		{
			int result = 0;

			switch (declaration)
			{
				case Declaration.Reyes:
					result = 120;
					break;
				case Declaration.Caballos:
					result = 60;
					break;
				case Declaration.ParejaOros:
				case Declaration.ParejaCopas:
				case Declaration.ParejaEspadas:
				case Declaration.ParejaBastos:
					result = 20;
					break;
				case Declaration.Cuarenta:
					result = 40;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return result;
		}

	}
}