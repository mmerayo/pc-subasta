using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using StructureMap.Configuration.DSL;
using Subasta.Infrastructure.IoC;


namespace ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			IoCRegistrator.Register(new List<Registry>(){new RegisterIoc()});

			var game = ObjectFactory.GetInstance<IGameSimulator>();
			game.Start();
			while (!game.IsFinished)
			{
				game.NextMove();
				Console.WriteLine("Press key to continue...");
				Console.ReadKey();
			}
		}

		private class RegisterIoc : Registry
		{
			public RegisterIoc()
			{
				For<IGameSimulator>().Use<TestGameSimulator>();
				For<IPlayer>().Use<Player>();
			}
		}
	}
}
