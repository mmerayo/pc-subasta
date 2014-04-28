using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

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
			int tries = 5;
			while (!ModuleDownloader.Instance.GetLatest()&&tries-->0)
			{
				MessageBox.Show("No se ha podido obtener una version valida. Verifique su conexion a internet", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
			if(tries==0)
				MessageBox.Show("No se ha podido obtener una version valida. Numero de intentos", "Error fatal", MessageBoxButtons.OK,
					MessageBoxIcon.Stop);
			LibInvoker.Initialize();
		}
	}

	internal class LibInvoker
	{
		public static void Initialize()
		{
			Assembly.LoadFrom(GetAssemblyPath());

			Type.GetType("Subasta.Lib.LibStarter").GetMethod("Start", BindingFlags.Static | BindingFlags.Public).Invoke(null,null);

		}

		private static string GetAssemblyPath()
		{
			string codeBase = Assembly.GetExecutingAssembly().CodeBase;
			var uri = new UriBuilder(codeBase);
			string path = Uri.UnescapeDataString(uri.Path);
			return Path.Combine(Path.GetDirectoryName(path), "Subasta.Lib.dll");
		}
	}
}
