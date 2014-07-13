using System;

namespace Xamarin.Forms.Labs.Services.SoundService
{
	public class SoundFile
	{
		public SoundFile ()
		{
		}

		public string Filename {
			get;
			set;
		}
		public TimeSpan Duration {
			get;
			set;
		}
	}
}

