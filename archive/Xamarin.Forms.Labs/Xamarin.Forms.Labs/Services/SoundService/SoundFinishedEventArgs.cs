using System;

namespace Xamarin.Forms.Labs.Services.SoundService
{
	public class SoundFinishedEventArgs : EventArgs
	{
		public SoundFinishedEventArgs (SoundFile f)
		{
			File = f;
		}
		public SoundFile File {
			get;
			set;
		}
	}
}

