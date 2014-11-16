// ***********************************************************************
// Assembly         : Xamarin.Forms.Labs.Droid
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

using XLabs.Platform.Droid.Services;

[assembly: Dependency(typeof(NfcDevice))]

namespace XLabs.Platform.Droid.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Android.App;
	using Android.Content;
	using Android.Nfc;

	using XLabs.Platform.Device;
	using XLabs.Platform.Droid.Device;
	using XLabs.Platform.Services;

	public class NfcDevice : Java.Lang.Object, INfcDevice, NfcAdapter.ICreateNdefMessageCallback
    {
        private readonly NfcAdapter device;
        private event EventHandler<EventArgs<INfcDevice>> inRange;
        private event EventHandler<EventArgs<INfcDevice>> outOfRange;
        private NfcMonitor monitor;

        private Dictionary<Guid, NdefRecord> published = new Dictionary<Guid, NdefRecord>();

        public NfcDevice() : this(Manager.DefaultAdapter)
        {
        }

        public NfcDevice(NfcAdapter adapter)
        {
            this.device = adapter;

            if (this.device != null)
            {
                var app = Resolver.Resolve<IXFormsApp>();
                var tapp = app as IXFormsApp<XFormsApplicationDroid>;

                this.device.SetNdefPushMessageCallback(this, tapp.AppContext);
            }
            else
            {
                Android.Util.Log.Info("INfcDevice", "NFC adapter is null. Either device does not support NFC or the application does not have NFC priviledges.");
            }
        }

        public static NfcManager Manager
        {
            get
            {
                return (NfcManager)Application.Context.GetSystemService(Context.NfcService);
            }
        }

        public static bool SupportsNfc
        {
            get
            {
                return Manager.DefaultAdapter != null;
            }
        }

        #region INfcDevice Members
        /// TODO: figure out if the NFC device has an ID or name. 
        /// The below method will not identify external NFC devices.
        public string DeviceId 
        { 
            get
            {
                var d = Resolver.Resolve<IDevice>();

                if (this.device == null || d == null)
                {
                    return "Unknown";
                }

                return d.Name;
            }
        }

        public bool IsEnabled
        {
            get { return this.device != null && this.device.IsEnabled; }
        }

        public event EventHandler<EventArgs<INfcDevice>> DeviceInRange
        {
            add
            {
                if (this.inRange == null)
                {
                    this.RegisterNfcCallback();
                }

                this.inRange += value;
            }
            remove
            {
                this.inRange -= value;

                if (this.inRange == null)
                {
                    this.UnregisterNfcCallback();
                }
            }
        }

        public event EventHandler<EventArgs<INfcDevice>> DeviceOutOfRange
        {
            add
            {
                if (this.outOfRange == null)
                {
                    //this.device.DeviceDeparted += DeviceDeparted;
                }

                this.outOfRange += value;
            }

            remove
            {
                this.outOfRange -= value;

                if (this.outOfRange == null)
                {
                    //this.device.DeviceDeparted -= DeviceDeparted;
                }
            }
        }

        public Guid PublishUri(Uri uri)
        {
            var key = Guid.NewGuid();

            this.published.Add(key, NdefRecord.CreateUri(uri.AbsoluteUri));

            return key;
        }

        public void Unpublish(Guid id)
        {
            if (this.published.ContainsKey(id))
            {
                this.published.Remove(id);
            }
        }
        #endregion

        private void RegisterNfcCallback()
        {
            UnregisterNfcCallback();
            this.monitor = new NfcMonitor(this);
            this.monitor.Start();
        }

        private void UnregisterNfcCallback()
        {
            if (this.monitor != null)
            {
                this.monitor.Stop();
                this.monitor = null;
            }
        }

        private class NfcMonitor : BroadcastMonitor
        {
            private readonly WeakReference<NfcDevice> deviceReference;

            public NfcMonitor(NfcDevice device)
            {
                deviceReference = new WeakReference<NfcDevice>(device);
            }

            protected override IntentFilter Filter
            {
                get 
                {
                    var filter = new IntentFilter(NfcAdapter.ActionTechDiscovered);
                    filter.AddAction(NfcAdapter.ActionTagDiscovered);
                    filter.AddAction(NfcAdapter.ActionNdefDiscovered);
                    
                    return filter;
                }
            }

            public override void OnReceive(Context context, Intent intent)
            {
                System.Diagnostics.Debug.WriteLine(intent.Action);
            }
        }

        #region ICreateNdefMessageCallback Members

        public NdefMessage CreateNdefMessage(NfcEvent e)
        {
            this.inRange.Invoke<INfcDevice>(this, this);
            return new NdefMessage(this.published.Values.ToArray());
        }

        #endregion
    }
}