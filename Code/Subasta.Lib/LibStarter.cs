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
