using System;
using System.Threading;
using System.Windows.Forms;

namespace Subasta
{
	static class Program
	{
		//The named mutex is destroyed when all the Mutex objects that represent it have been released.
		static readonly Mutex SingleInstanceMutex = new Mutex(false, "197D1DAB-F44C-42D0-824D-08863299F2F9");
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			

			if (!SingleInstanceMutex.WaitOne(TimeSpan.FromSeconds(2), false))
			{
				MessageBox.Show("Existe otra instancia del juego de la subasta funcionando!", "", MessageBoxButtons.OK);
				return;
			}
			try
			{
				if (!ModuleDownloader.Instance.Update())
					ShowCouldNotupdateMessage(String.Empty);
			}
			catch (Exception ex)
			{
				ShowCouldNotupdateMessage(ex.ToString());
			}
			
			try
			{
				LibInvoker.Initialize();
			}
			catch(Exception ex)
			{
				MessageBox.Show("No se ha podido iniciar la aplicacion. Error original:"+ ex, "Subasta:Error", MessageBoxButtons.OK,
					MessageBoxIcon.Stop);
			}
			finally { SingleInstanceMutex.ReleaseMutex(); }
		}

		
		

		private static void ShowCouldNotupdateMessage(string exception)
		{
			MessageBox.Show("No se ha podido comprobar actualizaciones del juego, Verifique su conexion a internet. " + exception,
				"Subasta:Error", MessageBoxButtons.OK,
				MessageBoxIcon.Information);
		}
	}

}
