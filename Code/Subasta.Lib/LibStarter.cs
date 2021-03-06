﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Subasta.ApplicationServices.Events;
using Subasta.Domain.Events;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using StructureMap;
using StructureMap.Configuration.DSL;
using Subasta.Client.Common.Infrastructure;
using Subasta.Infrastructure.IoC;
using Subasta.Lib.Infrastructure;

namespace Subasta.Lib
{
	public static class LibStarter
	{

		private static IEventPublisher _eventPublisher;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static void Start()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es-ES");
			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es-ES");

			//ShowStartupMessage();

			LogConfigurator.ConfigureLogging();

			//TODO: move to module
			IoCRegistrator.Register(new List<Registry>
			{
				new RegisterClientCommonIoc(),
				new RegisterLibIoc()
			});

			_eventPublisher = ObjectFactory.GetInstance<IEventPublisher>();
			_eventPublisher.Publish(ApplicationStartedEvent.Create());

			Application.ApplicationExit += Application_ApplicationExit;

			Application.Run(ObjectFactory.GetInstance<FrmMain>());
		}

		static void Application_ApplicationExit(object sender, EventArgs e)
		{
			 _eventPublisher.PublishSync(ApplicationExitedEvent.Create());
		}



		private static void ShowStartupMessage()
		{
			string text =
				string.Format(
					"Versión en estado de desarrollo(Alpha).{0}- El juego busca actualizaciones al arrancar con conexion a internet.{0}- Errores y/o sugerencias: miguelmerayo@hotmail.com",
					Environment.NewLine);
			MessageBox.Show(text, "Subasta para PC", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}