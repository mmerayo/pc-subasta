using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using StructureMap;
using StructureMap.Configuration.DSL;
using Subasta.Client.Common.Infrastructure;
using Subasta.Infrastructure.IoC;
using Subasta.Lib.Infrastructure;

namespace Subasta.Lib
{
	public static class LibStarter
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static void Start()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es-ES");
			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es-ES");

			string text = string.Format("Gracias por testear esta versión en estado de desarrollo(Alpha).{0} Esta versión no es estable por lo que se espera que pueda haber errores, estos se notifican al desarrollador automáticamente.{0} Las actualizaciones automaticas incluirán bug fixtures progresivamente.{0} La configuración mínima probada es 2GB RAM con CPU de 4 hilos, aún así funciona en máquinas inferiores, esto es debido a que la inteligencia artificial implementada requiere optimizaciones.",Environment.NewLine);
			MessageBox.Show(text,"Subasta para PC",MessageBoxButtons.OK,MessageBoxIcon.Information);

			//TODO: move to module
			IoCRegistrator.Register(new List<Registry>
			                        {
			                        	new RegisterClientCommonIoc(),
			                        	new RegisterLibIoc()
			                        });
			Application.Run(ObjectFactory.GetInstance<FrmMain>());
		}
	}
}
