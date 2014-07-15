using System;
using Xamarin.Forms.Labs.Services.SoundService;
using Android.Media;
using Android.Content.Res;
using System.Threading.Tasks;
using Android.App;
using System.IO;
using Xamarin.Forms;


[assembly: Dependency (typeof(Xamarin.Forms.Labs.Droid.Services.SoundService))]
namespace Xamarin.Forms.Labs.Droid.Services
{
	public class SoundService : ISoundService
	{
		bool _isPlayerPrepared = false;
		bool _isScrubbing = false;

		public SoundService ()
		{
		}

		#region ISoundService implementation

		public event EventHandler SoundFileFinished;

		public System.Threading.Tasks.Task<SoundFile> PlayAsync (string filename, string extension = null)
		{
			return Task.Run<SoundFile> (async () => {
				if (player == null || string.Compare (filename, _currentFile.Filename) > 0) {
					await SetMediaAsync (filename);
				}
				player.Start ();
				return _currentFile;
			});
		}

		public async System.Threading.Tasks.Task<SoundFile> SetMediaAsync (string filename)
		{
			_currentFile = new SoundFile ();
			_currentFile.Filename = filename;
			await StartPlayerAsyncFromAssetsFolder (Application.Context.Assets.OpenFd (filename));
			_currentFile.Duration =  TimeSpan.FromSeconds( player.Duration);
			return _currentFile;
		}

		public System.Threading.Tasks.Task GoToAsync (double position)
		{
			return Task.Run (() => {
				if (!_isScrubbing) {
					_isScrubbing = true;
					player.SeekTo (TimeSpan.FromSeconds (position).Milliseconds);
					_isScrubbing = false;
				}
			});
		}

		public void Play ()
		{
			if ((player != null)) {
				if (!player.IsPlaying) {
					player.Start ();
				}
			}
		}

		public void Stop ()
		{
			if ((player != null)) {
				if (player.IsPlaying) {
					player.Stop ();
				}
				player.Release ();
				player = null;
			}
		}

		public void Pause ()
		{
			if ((player != null)) {
				if (player.IsPlaying) {
					player.Pause ();
				}
			}
		}

		public double Volume {
			get {
				return 0.5;
			}
			set {
				if(player != null && _isPlayerPrepared)
					player.SetVolume((float)value,(float)value);
			}
		}

		private double _currentTime;
		public double CurrentTime {
			get {
				if (player == null)
					return 0;
				return TimeSpan.FromMilliseconds( player.CurrentPosition).TotalSeconds;
			}
			set {
				_currentTime = value;
			}
		}


		public bool IsPlaying {
			get{ 
				return player.IsPlaying;
			}
		}
		private SoundFile _currentFile;
		public SoundFile CurrentFile {
			get {
				return _currentFile;
			}
		}

		#endregion

		MediaPlayer player = null;

		private async Task StartPlayerAsyncFromAssetsFolder (AssetFileDescriptor fp)
		{
			try {
				if (player == null) {
					player = new MediaPlayer ();
				} else {
					player.Reset ();
				}

				if(fp == null)
					throw new FileNotFoundException("Make sure you set your file in the Assets folder");

				await player.SetDataSourceAsync (fp.FileDescriptor);
				player.Prepared+=(s,e)=>{

					player.SetVolume(0,0);
					_isPlayerPrepared = true;

				};
				player.Prepare ();
			} catch (Exception ex) {
				Console.Out.WriteLine (ex.StackTrace);
			}
		}

	}
}

