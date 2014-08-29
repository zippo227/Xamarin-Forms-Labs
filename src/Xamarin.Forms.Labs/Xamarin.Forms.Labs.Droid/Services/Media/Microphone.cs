using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Android.Media;
using Xamarin.Forms.Labs.Mvvm;
using Xamarin.Forms.Labs.Services.Media;

namespace Xamarin.Forms.Labs.Droid.Services.Media
{
    public class Microphone : IAudioStream
    {
        private readonly int bufferSize;

        /// <summary>
        /// The audio source.
        /// </summary>
        private readonly AudioRecord audioSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="Xamarin.Forms.Labs.Droid.Services.Media.Microphone"/> class.
        /// </summary>
        /// <param name="sampleRate">Sample rate.</param>
        /// <param name="bufferSize">Buffer size.</param>
        public Microphone(int sampleRate, int bufferSize)
        {
            this.bufferSize = bufferSize;
            this.audioSource = new AudioRecord(
                AudioSource.Mic,
                sampleRate,
                ChannelIn.Mono,
                Encoding.Pcm16bit,
                this.bufferSize);

            this.Start = new RelayCommand<int>(
                this.StartRecording,
                rate => this.SupportedSampleRates.Contains(rate) && this.audioSource != null && !this.Active
                );

            this.Stop = new Command(
                () => this.audioSource.Stop(),
                () => this.Active
                );

            this.SupportedSampleRates = (new[] { 8000, 11025, 16000, 22050, 44100 }).Where( 
                rate => AudioRecord.GetMinBufferSize(rate, ChannelIn.Default, Encoding.Pcm16bit) > 0
                ).ToList();
        }

        /// <summary>
        /// Occurs when new audio has been streamed.
        /// </summary>
        public event EventHandler<EventArgs<byte[]>> OnBroadcast;

        /// <summary>
        /// Gets the sample rate.
        /// </summary>
        /// <value>
        /// The sample rate.
        /// </value>
        public int SampleRate
        {
            get
            {
                return this.audioSource.SampleRate;
            }
        }

        /// <summary>
        /// Gets bits per sample.
        /// </summary>
        public int BitsPerSample
        {
            get
            {
                return (this.audioSource.AudioFormat == Encoding.Pcm16bit) ? 16 : 8;
            }
        }

        /// <summary>
        /// Gets the channel count.
        /// </summary>
        /// <value>
        /// The channel count.
        /// </value>        
        public int ChannelCount
        {
            get
            {
                return this.audioSource.ChannelCount;
            }
        }

        /// <summary>
        /// Gets the average data transfer rate
        /// </summary>
        /// <value>The average data transfer rate in bytes per second.</value>
        public int AverageBytesPerSecond
        {
            get
            {
                return this.SampleRate * this.BitsPerSample / 8 * this.ChannelCount;
            }
        }

        public bool Active
        {
            get
            {
                return (this.audioSource.RecordingState == RecordState.Recording);
            }
        }

        public IEnumerable<int> SupportedSampleRates
        {
            get;
            private set;
        }

        public ICommand Start
        {
            get;
            private set;
        }

        public ICommand Stop
        {
            get;
            private set;
        }

        /// <summary>
        /// Start recording from the hardware audio source.
        /// </summary>
        private void StartRecording(int rate)
        {
            this.audioSource.StartRecording();

            Task.Run(async () => 
            {
                do
                {
                    await Record();
                } 
                while (this.Active) ;
            }
            );
        }

        /// <summary>
        /// Record from the microphone and broadcast the buffer.
        /// </summary>
        private async Task Record()
        {
            var buffer = new byte[this.bufferSize];

            var readCount = await this.audioSource.ReadAsync(buffer, 0, this.bufferSize);
            
            this.OnBroadcast.Invoke<byte[]>(this, buffer);
        }
    }
}
