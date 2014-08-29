using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Xna.Framework.Audio;
using Xamarin.Forms.Labs.Mvvm;
using Xamarin.Forms.Labs.Services.Media;

namespace Xamarin.Forms.Labs.WP8.Services.Media
{
    public class XnaMicrophone : IAudioStream
    {
        private Microphone microphone;

        public XnaMicrophone()
        {
            this.microphone = Microphone.Default;
            this.microphone.BufferReady += microphone_BufferReady;

            this.Start = new RelayCommand<int>(
                (rate) => this.microphone.Start(),
                (rate) => this.SupportedSampleRates.Contains(rate) && this.microphone != null && this.microphone.State == MicrophoneState.Stopped);

            this.Stop = new Command(
                () => this.microphone.Stop(),
                () => this.microphone != null && this.microphone.State == MicrophoneState.Started);
        }

        public event EventHandler<EventArgs<byte[]>> OnBroadcast;

        public int SampleRate
        {
            get { return this.microphone.SampleRate; }
        }

        public int ChannelCount
        {
            get { return 1; }
        }

        public int BitsPerSample
        {
            get { return 16; }
        }

        public bool Active
        {
            get { return this.microphone.State == MicrophoneState.Started; }
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

        public IEnumerable<int> SupportedSampleRates
        {
            get
            {
                return new [] { 16000 };
            }
        }

        void microphone_BufferReady(object sender, EventArgs e)
        {
            var buffer = new byte[4096];
            int read;

            do
            {
                read = this.microphone.GetData(buffer, 0, buffer.Length);
                this.OnBroadcast.Invoke<byte[]>(this, buffer);
            }
            while (read > 0);
        }
    }
}
