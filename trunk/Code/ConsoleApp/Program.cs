using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using StructureMap;
using StructureMap.Configuration.DSL;
using Subasta.Client.Common;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;
using Subasta.Infrastructure.IoC;


namespace ConsoleApp
{
	class Program
	{
		private static Stopwatch _stopwatch;
		private static List<TimeSpan> _timespans=new List<TimeSpan>();
		private static ICard[][]_playerCards=new ICard[4][]; 
		private static void Main(string[] args)
		{
			//try
			//{
			IoCRegistrator.Register(new List<Registry>() { new RegisterClientCommonIoc() });

			var game = ObjectFactory.GetInstance<IGameSimulator>();
			game.GameStatusChanged += game_GameStatusChanged;
			game.InputRequested += game_InputRequested;
			for (var i = 0; i < 4; i++)
				_playerCards[i] = GetCards(i);

			_stopwatch = Stopwatch.StartNew();
			game.Player1.Cards = _playerCards[0];
			game.Player2.Cards = _playerCards[1];
			game.Player3.Cards = _playerCards[2];
			game.Player4.Cards = _playerCards[3];
			game.Start( 2);

			//}
			//catch (Exception ex)
			//{
			//    Console.WriteLine(ex);
			//}
			Console.ReadLine();
		}

		private static ICard[] GetCards(int playerIdx)
		{

			var result = new ICard[10];
			switch (playerIdx)
			{
				case 0:

					result[0] = new Card("C1");

					result[1] = new Card("C5");
					result[2] = new Card("E5");
					result[3] = new Card("C10");
					result[4] = new Card("C2");

					result[5] = new Card("O6");
					result[6] = new Card("C6");
					result[7] = new Card("O11");
					result[8] = new Card("C11");
					result[9] = new Card("O10");

					break;
				case 1:
					result[0] = new Card("B10");
					result[1] = new Card("O3");
					result[2] = new Card("O5");
					result[3] = new Card("C12");
					result[4] = new Card("O2");
					result[5] = new Card("C3");

					result[6] = new Card("O1");
					result[7] = new Card("B6");
					result[8] = new Card("E1");
					result[9] = new Card("B11");
					break;

				case 2:
					result[0] = new Card("B3");
					result[1] = new Card("B5");
					result[2] = new Card("E6");
					result[3] = new Card("E4");
					result[4] = new Card("E2");
					result[5] = new Card("E3");
					result[6] = new Card("O12");
					result[7] = new Card("B4");
					result[8] = new Card("E7");
					result[9] = new Card("O4");
					break;

				case 3:
					result[0] = new Card("C4");
					result[1] = new Card("O7");
					result[2] = new Card("E11");
					result[3] = new Card("E10");
					result[4] = new Card("B7");
					result[5] = new Card("B1");
					result[6] = new Card("C7");
					result[7] = new Card("E12");
					result[8] = new Card("B2");
					result[9] = new Card("B12");
					break;
			}

			return result;
		}


		static string game_InputRequested()
		{
			Console.WriteLine("Press any key to continue...");
			ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
			return consoleKeyInfo.KeyChar.ToString(CultureInfo.InvariantCulture);
		}

		static void game_GameStatusChanged(IExplorationStatus status,TimeSpan t)
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
				PaintHand(hand, index + 1);
			}
			
			if(status.IsCompleted)
			{
				PaintSummary(status);
			}

			_stopwatch.Restart();
		}

		private static void PaintSummary(IExplorationStatus status)
		{
			ConsoleColor c = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("***    GameCompleted: PlayerBet={0} - PointsBet={1}", status.PlayerBets, status.PointsBet);
			Console.WriteLine("Points Team 1={0}", status.SumTotalTeam(1));
			Console.WriteLine("Points Team 2={0}", status.SumTotalTeam(2));

			var handsWithDeclaration = status.Hands.Where(x=>x.Declaration.HasValue).ToList();
			foreach (var hand in handsWithDeclaration)
			{
				Console.WriteLine("hand #{0} - {1}",hand.Sequence,hand.Declaration);
				
			}
			Console.ForegroundColor = c;
		}

		private static void PaintTimes(int sequence)
		{
			int top = sequence*4;
			int bottom = top - 4;
			for (int index = bottom; index < top; index++)
			{
				if (_timespans.Count >= index + 1)
				{
					var timeSpan = _timespans[index];
					Console.WriteLine("{0} - {1}", index + 1, timeSpan);
				}
			}
		}

		private static void PaintHand(IHand hand, int sequence)
		{
			int playerPosition = hand.FirstPlayer;
			Console.WriteLine("Hand #{0}- IsCompleted={1}: ",sequence,hand.IsCompleted);

			for (int i = 0; i < 4; i++)
			{
				ICard playerCard = hand.PlayerCard(playerPosition);
				if(playerCard==null) break;
				Console.WriteLine("\tplayer #{0} -> {1}", playerPosition, playerCard.ToShortString());
				if (++playerPosition > 4) playerPosition = 1;
			}

			PaintTimes(sequence);
		}

		private static void PaintPlayerCards(IEnumerable<ICard> playerCards, int playerNum)
		{
			string aggregate = playerCards.Count() > 0
			                   	? playerCards.Select(x => x.ToShortString()).Aggregate((current, next) => current + ", " + next)
			                   	: string.Empty;
			Console.WriteLine("Player #{0}: {1}", playerNum,
			                  aggregate);
		}


		
	}
}
