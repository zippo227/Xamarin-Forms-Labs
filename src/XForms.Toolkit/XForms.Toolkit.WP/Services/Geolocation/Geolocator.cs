// ***********************************************************************
// Assembly         : XForms.Toolkit.WP
// Author           : Sami M. Kallio
// Created          : 06-16-2014
//
// Last Modified By : Sami M. Kallio
// Last Modified On : 06-16-2014
// ***********************************************************************
// <copyright file="Geolocator.cs" company="">
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
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(XForms.Toolkit.Services.Geolocation.Geolocator))]

namespace XForms.Toolkit.Services.Geolocation
{
    using Windows.Devices.Geolocation;
    using Locator = Windows.Devices.Geolocation.Geolocator;

    /// <summary>
    /// The geolocator implements <see cref="IGeolocator"/> interface for Windows Phone 8.
    /// </summary>
    public class Geolocator : IGeolocator
    {
        private Locator locator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Geolocator"/> class.
        /// </summary>
        public Geolocator()
        {
            locator = new Locator();
        }

        #region IGeolocator Members

        public event EventHandler<PositionErrorEventArgs> PositionError;

        public event EventHandler<PositionEventArgs> PositionChanged;

        public double DesiredAccuracy
        {
            get
            {
                return (locator.DesiredAccuracy == PositionAccuracy.Default)
                           ? 100
                           : 10;
            }

            set
            {
                locator.DesiredAccuracy = (value > 10) ?
                        PositionAccuracy.Default :
                        PositionAccuracy.High;
            }
        }

        public bool IsListening
        {
            get;
            private set;
        }

        public bool SupportsHeading
        {
            get { return true; }
        }

        public bool IsGeolocationAvailable
        {
            get { return locator.LocationStatus != PositionStatus.NotAvailable; }
        }

        public bool IsGeolocationEnabled
        {
            get { return locator.LocationStatus != PositionStatus.Disabled; }
        }

        public async Task<Position> GetPositionAsync(int timeout)
        {
            var position = await locator.GetGeopositionAsync(TimeSpan.MaxValue, TimeSpan.FromMilliseconds(timeout));
            return position.Coordinate.GetPosition();
        }

        public async Task<Position> GetPositionAsync(int timeout, bool includeHeading)
        {
            return await GetPositionAsync(timeout);
        }

        public async Task<Position> GetPositionAsync(System.Threading.CancellationToken cancelToken)
        {
            var t = locator.GetGeopositionAsync().AsTask();

            while (t.Status == TaskStatus.Running)
            {
                cancelToken.ThrowIfCancellationRequested();
            }

            var position = await t;
            
            return position.Coordinate.GetPosition();
        }

        public Task<Position> GetPositionAsync(System.Threading.CancellationToken cancelToken, bool includeHeading)
        {
            return this.GetPositionAsync(cancelToken);
        }

        public Task<Position> GetPositionAsync(int timeout, System.Threading.CancellationToken cancelToken)
        {
            var t = GetPositionAsync(timeout);

            while (t.Status == TaskStatus.Running)
            {
                cancelToken.ThrowIfCancellationRequested();
            }

            return t;
        }

        public Task<Position> GetPositionAsync(int timeout, System.Threading.CancellationToken cancelToken, bool includeHeading)
        {
            var t = GetPositionAsync(timeout, includeHeading);

            while (t.Status == TaskStatus.Running)
            {
                cancelToken.ThrowIfCancellationRequested();
            }

            return t;
        }

        public void StartListening(uint minTime, double minDistance)
        {
            locator.MovementThreshold = minDistance;
            locator.ReportInterval = minTime;
            locator.PositionChanged += locator_PositionChanged;
            locator.StatusChanged += locator_StatusChanged;
        }

        public void StartListening(uint minTime, double minDistance, bool includeHeading)
        {
            this.StartListening(minTime, minDistance);
        }

        public void StopListening()
        {
            locator.PositionChanged -= locator_PositionChanged;
            locator.StatusChanged -= locator_StatusChanged;
        }

        #endregion

        void locator_PositionChanged(Locator sender, PositionChangedEventArgs args)
        {
            this.PositionChanged.TryInvoke(
                sender, 
                new PositionEventArgs(args.Position.Coordinate.GetPosition()));
        }

        void locator_StatusChanged(Locator sender, StatusChangedEventArgs args)
        {
            switch (args.Status)
            {
                case PositionStatus.Disabled:
                    this.PositionError.TryInvoke(
                        sender, 
                        new PositionErrorEventArgs(GeolocationError.Unauthorized));
                    break;
                case PositionStatus.Initializing:
                    break;
                case PositionStatus.NoData:
                    this.PositionError.TryInvoke(
                        sender, 
                        new PositionErrorEventArgs(GeolocationError.PositionUnavailable));
                    break;
                case PositionStatus.NotInitialized:
                    this.IsListening = false;
                    break;
                case PositionStatus.Ready:
                    this.IsListening = true;
                    break;
            }
        }
    }
}
