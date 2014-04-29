using System;
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
			while (!ModuleDownloader.Instance.Update()&&tries-->0)
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
}
