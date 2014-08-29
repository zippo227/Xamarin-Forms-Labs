using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MonoTouch.AudioToolbox;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms.Labs.Mvvm;
using Xamarin.Forms.Labs.Services.Media;

namespace Xamarin.Forms.Labs.iOS.Services.Media
{
    public class Microphone : IAudioStream
    {
        private InputAudioQueue audioQueue;

        private readonly int bufferSize;

        public Microphone(int bufferSize = 4098)
        {
            this.bufferSize = bufferSize;

            this.Start = new RelayCommand<int>(
                rate => this.StartRecording(rate),
                rate => this.audioQueue == null && this.SupportedSampleRates.Contains(rate)
            );

            this.Stop = new Command(
                () => this.Clear(),
                () => this.Active
                );
        }

        #region IAudioStream implementation

        public event EventHandler<EventArgs<byte[]>> OnBroadcast;

        public int SampleRate
        {
            get;
            private set;
        }

        public int ChannelCount
        {
            get
            {
                return 1;
            }
        }

        public int BitsPerSample
        {
            get
            {
                return 16;
            }
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

        public bool Active
        {
            get
            {
                return this.audioQueue != null && this.audioQueue.IsRunning;
            }
        }

        public IEnumerable<int> SupportedSampleRates
        {
            get 
            {
                return new[]
                {
                    8000,
                    16000,
                    22050,
                    41000,
                    44100
                };
            }
        }

        #endregion

        private void StartRecording(int rate)
        {
            if (this.Active)
            {
                this.Clear();
            }

            this.SampleRate = rate;

            var audioFormat = new AudioStreamBasicDescription()
            {
                SampleRate = this.SampleRate,
                Format = AudioFormatType.LinearPCM,
                FormatFlags = AudioFormatFlags.LinearPCMIsSignedInteger | AudioFormatFlags.LinearPCMIsPacked,
                FramesPerPacket = 1,
                ChannelsPerFrame = 1,
                BitsPerChannel = this.BitsPerSample,
                BytesPerPacket = 2,
                BytesPerFrame = 2,
                Reserved = 0
            };

            audioQueue = new InputAudioQueue(audioFormat);
            audioQueue.InputCompleted += QueueInputCompleted;

            var bufferByteSize = this.bufferSize * audioFormat.BytesPerPacket;

            IntPtr bufferPtr;
            for (var index = 0; index < 3; index++)
            {
                audioQueue.AllocateBufferWithPacketDescriptors(bufferByteSize, this.bufferSize, out bufferPtr);
                audioQueue.EnqueueBuffer(bufferPtr, bufferByteSize, null);
            }

            this.audioQueue.Start();
        }

        private void Clear()
        {
            if (this.audioQueue != null)
            {
                this.audioQueue.Stop(true);
                this.audioQueue.InputCompleted -= QueueInputCompleted;
                this.audioQueue.Dispose();
                this.audioQueue = null;
            }
        }

        /// <summary>
        /// Handles iOS audio buffer queue completed message.
        /// </summary>
        /// <param name='sender'>Sender object</param>
        /// <param name='e'> Input completed parameters.</param>
        private void QueueInputCompleted(object sender, InputCompletedEventArgs e)
        {
            // return if we aren't actively monitoring audio packets
            if (!this.Active)
            {
                return;
            }

            var buffer = (AudioQueueBuffer)System.Runtime.InteropServices.Marshal.PtrToStructure(e.IntPtrBuffer, typeof(AudioQueueBuffer));
            if (this.OnBroadcast != null)
            {
                var send = new byte[buffer.AudioDataByteSize];
                System.Runtime.InteropServices.Marshal.Copy(buffer.AudioData, send, 0, (int)buffer.AudioDataByteSize);

                this.OnBroadcast(this, new EventArgs<byte[]>(send));
            }
                               
            var status = audioQueue.EnqueueBuffer(e.IntPtrBuffer, this.bufferSize, e.PacketDescriptions);  

            if (status != AudioQueueStatus.Ok)
            {
                // todo: 
            }
        }

        #region IAudioStream Members




        #endregion
    }
}