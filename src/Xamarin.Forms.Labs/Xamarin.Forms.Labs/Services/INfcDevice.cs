// ***********************************************************************
// Assembly         : Xamarin.Forms.Labs
// Author           : Sami Kallio
// Created          : 08-30-2014
//
// Last Modified By : Sami Kallio
// Last Modified On : 08-30-2014
// ***********************************************************************
// <copyright file="INfcDevice.cs" company="">
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

namespace Xamarin.Forms.Labs.Services
{
    public interface INfcDevice
    {
        //string DeviceId { get; }

        bool IsEnabled { get; }

        event EventHandler<EventArgs<INfcDevice>> DeviceInRange;

        event EventHandler<EventArgs<INfcDevice>> DeviceOutOfRange;

        //byte[] Message { get; set; }

        Guid PublishUri(Uri uri);

        void Unpublish(Guid id);
    }

    public enum NdefType
    {
        Uri = 0x01,
    }
}
