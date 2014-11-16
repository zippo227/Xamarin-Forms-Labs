namespace XLabs.Platform.WP8.Services.Media
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using System.Windows.Threading;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Audio;

	public class XnaMicrophone : IAudioStream
    {
        private Microphone microphone;
        private DispatcherTimer timer;

        public XnaMicrophone()
        {
            this.microphone = Microphone.Default;

            this.timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };

            this.timer.Tick += (s, e) => FrameworkDispatcher.Update();
        }

        public static bool IsAvailable
        {
            get
            {
                return Microphone.Default != null;
            }
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
            get { return this.microphone != null && this.microphone.State == MicrophoneState.Started; }
        }

        public IEnumerable<int> SupportedSampleRates
        {
            get
            {
                return new [] { 16000 };
            }
        }

        #region IAudioStream Members


        public Task<bool> Start(int sampleRate)
        {
            return Task.Run<bool>(
                () =>
                {
                    try
                    {
                        this.timer.Start();
                        this.microphone.BufferReady += microphone_BufferReady;
                        this.microphone.Start();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                });
        }

        public Task Stop()
        {
            return Task.Run(() =>
            {
                this.microphone.BufferReady -= microphone_BufferReady;
                this.microphone.Stop();
                this.timer.Stop();
            });
        }

        #endregion

        void microphone_BufferReady(object sender, EventArgs e)
        {
            var buffer = new byte[this.microphone.GetSampleSizeInBytes(this.microphone.BufferDuration)];
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
