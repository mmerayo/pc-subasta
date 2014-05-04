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

			MessageBox.Show("Gracias por testear esta versión en estado de desarrollo(Alpha). Esta versión no es estable por lo que se espera que pueda haber errores, estos se notifican al desarrollador automáticamente. Las actualizaciones automaticas incluirán bug fixtures progresivamente","Subasta para PC",MessageBoxButtons.OK,MessageBoxIcon.Information);

			//TODO: move to module
			IoCRegistrator.Register(new List<Registry>
			                        {
			                        	new RegisterClientCommonIoc(),
			                        	new RegisterIoc()
			                        });
			Application.Run(ObjectFactory.GetInstance<FrmMain>());
		}
	}
}
