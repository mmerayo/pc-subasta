﻿using Subasta.Domain.Deck;

namespace ConsoleApp
{
	public interface IPlayer
	{
		ICard[] Cards { get; set; }
	}
}