// ***********************************************************************
// Assembly         : XForms.Toolkit.WP
// Author           : Sami M. Kallio
// Created          : 06-16-2014
//
// Last Modified By : Sami M. Kallio
// Last Modified On : 06-16-2014
// ***********************************************************************
// <copyright file="PhoneService.cs" company="">
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
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Tasks;

namespace XForms.Toolkit.Services
{
    /// <summary>
    /// Phone service for Windows Phone devices.
    /// </summary>
    public class PhoneService : IPhoneService
    {
        /// <summary>
        /// Gets the cellular provider.
        /// </summary>
        public string CellularProvider
        {
            get
            {
                return DeviceNetworkInformation.CellularMobileOperator;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has cellular data enabled.
        /// </summary>
        public bool? IsCellularDataEnabled
        {
            get
            {
                return DeviceNetworkInformation.IsCellularDataEnabled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has cellular data roaming enabled.
        /// </summary>
        public bool? IsCellularDataRoamingEnabled
        {
            get
            {
                return DeviceNetworkInformation.IsCellularDataRoamingEnabled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is network available.
        /// </summary>
        public bool? IsNetworkAvailable
        {
            get
            {
                return DeviceNetworkInformation.IsNetworkAvailable;
            }
        }

        /// <summary>
        /// Gets the ISO Country Code
        /// </summary>
        public string ICC
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets the Mobile Country Code
        /// </summary>
        public string MCC
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets the Mobile Network Code
        /// </summary>
        public string MNC
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Opens native dialog to dial the specified number
        /// </summary>
        /// <param name="number">Number to dial.</param>
        public void DialNumber(string number)
        {
            new PhoneCallTask() { PhoneNumber = number }.Show();
        }
    }
}
