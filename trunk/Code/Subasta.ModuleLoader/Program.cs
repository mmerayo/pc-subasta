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
			ModuleDownloader.Instance.GetLatest();
		}
	}
}
