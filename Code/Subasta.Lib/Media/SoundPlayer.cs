using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using Subasta.ApplicationServices.Extensions;
using Subasta.Client.Common.Media;
using log4net;

namespace Subasta.Lib.Media
{
	internal class SoundPlayer : ISoundPlayer
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof (SoundPlayer));

		private readonly IMediaProvider _mediaProvider;

		public SoundPlayer(IMediaProvider mediaProvider)
		{
			_mediaProvider = mediaProvider;
		}

		public void Play(GameSoundType soundType)
		{
			try
			{
				using (var ms = new MemoryStream())
				{
					using (var stream = _mediaProvider.GetSoundStream(soundType))
					{
						var buffer = new byte[32768];
						int read;
						while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
						{
							ms.Write(buffer, 0, read);
						}
					}

					ms.Position = 0;
					using (
						var blockAlignedStream =
							new BlockAlignReductionStream(WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(ms))))
					{
						using (var waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
						{
							waveOut.Init(blockAlignedStream);
							waveOut.Play();
							while (waveOut.PlaybackState == PlaybackState.Playing)
							{
								System.Threading.Thread.Sleep(100);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Play", ex);
			}
		}

		public void PlayAsync(GameSoundType soundType)
		{
			Task.Factory.StartNew(() => Play(soundType)).LogTaskException(Logger);
		}
	}


}
