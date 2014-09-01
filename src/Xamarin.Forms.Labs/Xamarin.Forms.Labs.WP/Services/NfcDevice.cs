// ***********************************************************************
// Assembly         : Xamarin.Forms.Labs.WP8
// Author           : Sami Kallio
// Created          : 08-30-2014
//
// Last Modified By : Sami Kallio
// Last Modified On : 08-30-2014
// ***********************************************************************
// <copyright file="NfcDevice.cs" company="">
//     Copyright (c) 2014 . All rights reserved.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.WP8.Services;

[assembly: Dependency(typeof(NfcDevice))]

namespace Xamarin.Forms.Labs.WP8.Services
{
    public class NfcDevice : INfcDevice
    {
        private readonly ProximityDevice device;
        private event EventHandler<EventArgs<INfcDevice>> inRange;
        private event EventHandler<EventArgs<INfcDevice>> outOfRange;
        private long? publishId;

        private Dictionary<Guid, long> published = new Dictionary<Guid,long>();

        public NfcDevice() : this(ProximityDevice.GetDefault())
        {
        }

        public NfcDevice(ProximityDevice proximityDevice)
        {
            this.device = proximityDevice;
        }

        #region INfcDevice Members
        public string DeviceId
        {
            get
            {
                return this.device == null ? "Unknown" : this.device.DeviceId;
            }
        }

        public bool IsEnabled
        {
            get { return this.device != null; }
        }

        public event EventHandler<EventArgs<INfcDevice>> DeviceInRange
        {
            add
            {
                if (this.inRange == null)
                {
                    this.device.DeviceArrived += DeviceArrived;
                }

                this.inRange += value;
            }
            remove
            {
                this.inRange -= value;

                if (this.inRange == null)
                {
                    this.device.DeviceArrived -= DeviceArrived;
                }
            }
        }

        public event EventHandler<EventArgs<INfcDevice>> DeviceOutOfRange
        {
            add
            {
                if (this.outOfRange == null)
                {
                    this.device.DeviceDeparted += DeviceDeparted;
                }

                this.outOfRange += value;
            }

            remove
            {
                this.outOfRange -= value;

                if (this.outOfRange == null)
                {
                    this.device.DeviceDeparted -= DeviceDeparted;
                }
            }
        }

        public Guid PublishUri(Uri uri)
        {
            var id = this.device.PublishUriMessage(uri);
            var key = Guid.NewGuid();

            this.published.Add(key, id);

            return key;
        }

        public void Unpublish(Guid id)
        {
            if (this.published.ContainsKey(id))
            {
                this.device.StopPublishingMessage(this.published[id]);
                this.published.Remove(id);
            }
        }
        #endregion

        private void DeviceArrived(ProximityDevice sender)
        {
            //if (sender == this.device)
            //{
            //    System.Diagnostics.Debug.WriteLine("Sender is the same NFC device.");
            //}

            //this.inRange.Invoke<INfcDevice>(this, new NfcDevice(sender));
            this.inRange.Invoke<INfcDevice>(this, this);
        }

        void DeviceDeparted(ProximityDevice sender)
        {
            this.outOfRange.Invoke<INfcDevice>(this, this);
        }
    }
}
