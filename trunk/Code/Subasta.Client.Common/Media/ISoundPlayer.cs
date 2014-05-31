using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subasta.Client.Common.Media
{
	public enum GameSoundType
	{
		PetarMesa,
		CardPlayed,
		Shuffle,
		TurnChanged,
		DeclarationEmit,
		Selection,
		Voice1,
		Voice2,
		Voice3,
		Voice4,
		Voice5,
		Voice6
	}
	
	public interface ISoundPlayer
	{
		void Play(GameSoundType soundType);
		void PlayAsync(GameSoundType soundType);
		void PlayRandomVoice();
	}


}
