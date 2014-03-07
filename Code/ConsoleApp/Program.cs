using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using StructureMap;
using StructureMap.Configuration.DSL;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.IoC;


namespace ConsoleApp
{
	class Program
	{
		private static Stopwatch _stopwatch;
		private static List<TimeSpan> _timespans=new List<TimeSpan>();

		private static void Main(string[] args)
		{
			

			try
			{
				IoCRegistrator.Register(new List<Registry>() {new RegisterIoc()});

				var game = ObjectFactory.GetInstance<IGameSimulator>();
				game.GameStatusChanged += game_GameStatusChanged;
				game.InputRequested += game_InputRequested;
				_stopwatch = Stopwatch.StartNew();
				game.Start();

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			Console.ReadLine();
		}

		static string game_InputRequested()
		{
			Console.WriteLine("Press any key to continue...");
			ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
			return consoleKeyInfo.KeyChar.ToString(CultureInfo.InvariantCulture);
		}

		static void game_GameStatusChanged(IExplorationStatus status)
		{
			PaintGameStatus(status);
		}
		private static void PaintGameStatus(IExplorationStatus status)
		{
			_stopwatch.Stop();
			_timespans.Add(_stopwatch.Elapsed);
			Console.Clear();
			for (int i = 1; i <= 4; i++)
				PaintPlayerCards(status.PlayerCards(i), i);
			for (int index = 0; index < status.Hands.Count; index++)
			{
				var hand = status.Hands[index];
				if(hand.IsCompleted)
					PaintHand(hand, index + 1);
			}

			PaintTimes();

			_stopwatch.Restart();
		}

		private static void PaintTimes()
		{
			for (int index = 0; index < _timespans.Count; index++)
			{
				var timeSpan = _timespans[index];
				Console.WriteLine("{0} - {1}",index+1,timeSpan);
			}
		}

		private static void PaintHand(IHand hand, int sequence)
		{
			int playerPosition = hand.FirstPlayer;
			Console.WriteLine("Hand #{0}: starts{1}",sequence,playerPosition);

			for (int i = 0; i < 4; i++)
			{
				Console.WriteLine("player #{0}", hand.PlayerCard(playerPosition));
				if (++playerPosition > 4) playerPosition = 1;
			}
		}

		private static void PaintPlayerCards(IEnumerable<ICard> playerCards, int playerNum)
		{
			Console.WriteLine("Player #{0}: {1}", playerNum,
			                  playerCards.Select(x => x.ToShortString()).Aggregate((current, next) => current + ", " + next));
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
