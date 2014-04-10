using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.Deck;
using Subasta.DomainServices.Game;

namespace Subasta.Domain.Game
{
	/// <summary>
	/// status of the say step(marque)
	/// </summary>
	public interface ISaysStatus
	{
		bool IsCompleted { get; }
		int Turn { get; }
		int PlayerBets { get; }
		int PointsBet { get; }
	    int TurnTeam { get; } 
	    ISaysStatus Clone();
		void Add(int playerNumber, IFigure figure);
	    ISayCard[] GetPlayerCards(int playerNum);

		IExplorationStatus ExplorationStatusForOros();
		IExplorationStatus ExplorationStatusForCopas();
		IExplorationStatus ExplorationStatusForEspadas();
		IExplorationStatus ExplorationStatusForBastos();
	}

	public interface ISay
	{
		int PlayerNum { get; }
		IFigure Figure { get; }
	}

	public enum SayKind
	{
		Paso,
		Una,
		Dos,
		Tres,
		Cuatro,
		Cinco,
		Seis,
		Siete,
		Ocho,
		Nueve,
		Diez,
		Once,
		Doce,
		Trece,
		Catorce,
		Quince,
		Dieciseis,
		Diecisiete,
		Dieciocho,
		Diecinueve,
		Veinte,
		Veintiuno,
		Veintidos,
		Veintitres,
		Veinticuatro,
		Veinticinco,
		UnaMas,
	}
}
