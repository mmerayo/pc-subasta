using System;

namespace Subasta
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			ModuleLoader.Instance.GetLatest();
		}
	}

	internal class ModuleLoader
	{
		private ModuleLoader(){}
		public static readonly ModuleLoader _instance=new ModuleLoader();

	}
}
