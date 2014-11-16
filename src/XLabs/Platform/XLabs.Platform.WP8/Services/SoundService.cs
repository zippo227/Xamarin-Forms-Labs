using XLabs.Platform.WP8.Services;

[assembly: Dependency(typeof(SoundService))]
namespace XLabs.Platform.WP8.Services
{
	using System;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
    /// SoundService implementation on the Windows Phone platform
    /// Nees a GlobalMEdiaElement instance on the App resources dictionary
    /// </summary>
    public class SoundService : ISoundService
    {

        private bool _isScrubbing = false;
        private bool _isPlaying = false;
        private SoundFile _currentFile = null;

        /// <summary>
        /// Occurs when [sound file finished].
        /// </summary>
        public event EventHandler SoundFileFinished;

        /// <summary>
        /// Raises the <see cref="E:FileFinished" /> event.
        /// </summary>
        /// <param name="e">The <see cref="SoundFinishedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileFinished(SoundFinishedEventArgs e)
        {
            if (SoundFileFinished != null)
            {
                SoundFileFinished(this, e);
            }
        }

        /// <summary>
        /// Plays the asynchronous.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        public async Task<SoundFile> PlayAsync(string filename, string extension = null)
        {
            if (GlobalMediaElement != null || string.Compare(filename, _currentFile.Filename) > 0)
            {
                await SetMediaAsync(filename);

                GlobalMediaElement.Play();

                _isPlaying = true;
                return _currentFile;
            }
            return null;
        }
        /// <summary>
        /// Gets the global media element.
        /// </summary>
        /// <value>
        /// The global media element.
        /// </value>
        public static MediaElement GlobalMediaElement
        {
            get
            {
                if (Application.Current.Resources.Contains("GlobalMedia"))
                    return Application.Current.Resources["GlobalMedia"] as MediaElement;
                else
                    throw new ArgumentNullException("GlobalMedia is missing");
            }

        }

        TaskCompletionSource<SoundFile> tcsSetMedia;
        /// <summary>
        /// Sets the media asynchronous.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public Task<SoundFile> SetMediaAsync(string filename)
        {
            tcsSetMedia = new TaskCompletionSource<SoundFile>();

            _currentFile = new SoundFile();
            _currentFile.Filename = filename;
            Device.BeginInvokeOnMainThread(() =>
            {
                if (Application.GetResourceStream(new Uri(_currentFile.Filename, UriKind.Relative)) == null)
                    MessageBox.Show("File doesn't exist!");

                //TODO: need to clean this events
                GlobalMediaElement.MediaEnded += GlobalMediaElement_MediaEnded;
                GlobalMediaElement.MediaOpened += GlobalMediaElement_MediaOpened;

                GlobalMediaElement.Source = new Uri(_currentFile.Filename, UriKind.Relative);

            });
            return tcsSetMedia.Task;
        }

        void GlobalMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (tcsSetMedia != null)
            {
                _currentFile.Duration = TimeSpan.FromSeconds(GlobalMediaElement.NaturalDuration.TimeSpan.TotalSeconds);
                tcsSetMedia.SetResult(_currentFile);
            }
        }

        /// <summary>
        /// Handles the MediaEnded event of the player control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void GlobalMediaElement_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OnFileFinished(new SoundFinishedEventArgs(this._currentFile));
        }

        /// <summary>
        /// Goes to asynchronous.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public Task GoToAsync(double position)
        {
            return Task.Run(() =>
            {
                if (!_isScrubbing)
                {
                    _isScrubbing = true;
                    //    player.CurrentTime = position;
                    _isScrubbing = false;
                }
            });
        }

        /// <summary>
        /// Gets the current file.
        /// </summary>
        /// <value>
        /// The current file.
        /// </value>
        public SoundFile CurrentFile
        {
            get
            {
                return _currentFile;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is playing.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is playing; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }
        }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <value>
        /// The current time.
        /// </value>
        public double CurrentTime
        {
            get
            {
                if (GlobalMediaElement == null)
                    return 0;
                return (double)GlobalMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            }
        }

        /// <summary>
        /// Plays this instance.
        /// </summary>
        public void Play()
        {
            if (GlobalMediaElement != null && !_isPlaying)
            {
                GlobalMediaElement.Play();
                _isPlaying = true;
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (GlobalMediaElement != null)
            {
                GlobalMediaElement.Stop();
                _isPlaying = false;
                //   player.du = 0.0;
            }

        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            if (GlobalMediaElement != null && _isPlaying)
            {
                GlobalMediaElement.Pause();
                _isPlaying = false;
            }
        }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public double Volume
        {
            get
            {
                if (GlobalMediaElement == null)
                    return 0;
                return GlobalMediaElement.Volume;
            }
            set
            {
                if (GlobalMediaElement != null)
                {
                    GlobalMediaElement.Volume = value;
                }
            }
        }
    }
}
