using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{
	/// <summary>
	/// status of the say step(marque)
	/// </summary>
	public interface ISaysStatus
	{
		bool IsCompleted { get; }
		byte Turn { get; }
		byte PlayerBets { get; }
		byte PointsBet { get; }
		byte TurnTeam { get; }
		List<ISay> Says { get; }
		byte TeamBets { get; }
		byte Sequences { get; }
		byte LastSayPlayer { get; }
		bool IsEmpty { get; }
		IExplorationStatus OriginalStatus { get; }
		byte OtherTeam { get; }
		byte FirstPlayer { get; }
		ISaysStatus Clone();
		void Add(byte playerNumber, IFigure figure);
	    ISayCard[] GetPlayerCards(int playerNum);

		IExplorationStatus ExplorationStatusForOros();
		IExplorationStatus ExplorationStatusForCopas();
		IExplorationStatus ExplorationStatusForEspadas();
		IExplorationStatus ExplorationStatusForBastos();
	}

	public interface ISay
	{
		byte PlayerNum { get; }
		IFigure Figure { get; }
		byte Sequence { get; }
		byte PlayerTeamNum { get; }
	}

	public enum SayKind:byte
	{
		Paso=0,
		Una=1,
		Dos=2,
		Tres=3,
		Cuatro=4,
		Cinco=5,
		Seis=6,
		Siete=7,
		Ocho=8,
		Nueve=9,
		Diez=10,
		Once=11,
		Doce=12,
		Trece=13,
		Catorce=14,
		Quince=15,
		Dieciseis=16,
		Diecisiete=17,
		Dieciocho=18,
		Diecinueve=19,
		Veinte=20,
		Veintiuno=21,
		Veintidos=22,
		Veintitres=23,
		Veinticuatro=24,
		Veinticinco=25,
		UnaMas,
	}
}
