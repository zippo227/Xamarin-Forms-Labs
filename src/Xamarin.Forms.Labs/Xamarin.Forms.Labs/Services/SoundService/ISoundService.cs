using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Services.SoundService
{
	/// <summary>
	/// Interface ISoundService
	/// Enables playing any type of sound
	/// </summary>
	public interface ISoundService
	{
		event EventHandler SoundFileFinished;

		Task<SoundFile> PlayAsync(string filename , string extension = null);

		Task<SoundFile> SetMediaAsync(string filename);

		Task GoToAsync(double position);

		void Play();

		void Stop();

		void Pause();

		double Volume {get;set;}

		double CurrentTime {get;}

		bool IsPlaying { get;}

		SoundFile CurrentFile {get;}

	}
}

