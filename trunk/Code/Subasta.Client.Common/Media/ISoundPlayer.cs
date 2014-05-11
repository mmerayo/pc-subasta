﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subasta.Client.Common.Media
{
	public enum GameSoundType
	{
		PetarMesa
	}
	
	public interface ISoundPlayer
	{
		void Play(GameSoundType petarMesa);
		void PlayAsync(GameSoundType soundType);
	}


}
