using System.Text.RegularExpressions;

namespace Subasta
{
	public static class StringExtensions
	{
		public static string[] SplitCamelCase(this string source)
		{
			return Regex.Split(source, @"(?<!^)(?=[A-Z])");
		}

		public static string SeparateCamelCase(this string source,string separator=" ")
		{
			return string.Join(separator, source.SplitCamelCase());
		}
	}
}