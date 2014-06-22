// ***********************************************************************
// Assembly         : XForms.Toolkit
// Author           : Shawn Anderson
// Created          : 06-16-2014
//
// Last Modified By : Shawn Anderson
// Last Modified On : 06-16-2014
// ***********************************************************************
// <copyright file="MediaStorageOptions.cs" company="">
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

namespace Xamarin.Forms.Labs.Services.Media
{
	#region Base Options
	/// <summary>
	/// Class MediaStorageOptions.
	/// </summary>
	public class MediaStorageOptions
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MediaStorageOptions" /> class.
		/// </summary>
		protected MediaStorageOptions()
		{
		}

		/// <summary>
		/// Gets or sets the directory.
		/// </summary>
		/// <value>The directory.</value>
		public string Directory
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the maximum pixel dimension.
		/// </summary>
		/// <value>The maximum pixel dimension.</value>
		public int? MaxPixelDimension
		{
			get; 
			set;
		}

		/// <summary>
		/// Gets or sets the percent quality.
		/// </summary>
		/// <value>The percent quality.</value>
		public int? PercentQuality
		{
			get; 
			set;
		}
	}
	#endregion Base Options

	#region Camera Options
	/// <summary>
	/// Enum CameraDevice
	/// </summary>
	public enum CameraDevice
	{
		/// <summary>
		/// The rear
		/// </summary>
		Rear,
		/// <summary>
		/// The front
		/// </summary>
		Front
	}

	/// <summary>
	/// Class CameraMediaStorageOptions.
	/// </summary>
	public class CameraMediaStorageOptions
		: MediaStorageOptions
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CameraMediaStorageOptions" /> class.
		/// </summary>
		public CameraMediaStorageOptions()
		{
			SaveMediaOnCapture = true;
		}

		/// <summary>
		/// Gets or sets the default camera.
		/// </summary>
		/// <value>The default camera.</value>
		public CameraDevice DefaultCamera
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether [save media on capture].
		/// </summary>
		/// <value><c>true</c> if [save media on capture]; otherwise, <c>false</c>.</value>
		public bool SaveMediaOnCapture
		{
			get; 
			set;
		}
	}
	#endregion Camera Options

	#region Video Options
	/// <summary>
	/// Enum VideoQuality
	/// </summary>
	public enum VideoQuality
	{
		/// <summary>
		/// The low
		/// </summary>
		Low = 0,
		/// <summary>
		/// The medium
		/// </summary>
		Medium = 1,
		/// <summary>
		/// The high
		/// </summary>
		High = 2,
	}

	/// <summary>
	/// Class VideoMediaStorageOptions.
	/// </summary>
	public class VideoMediaStorageOptions
		: MediaStorageOptions
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="VideoMediaStorageOptions" /> class.
		/// </summary>
		public VideoMediaStorageOptions()
		{
			Quality = VideoQuality.High;
			DesiredLength = TimeSpan.FromMinutes(10);
			SaveMediaOnCapture = true;
		}
		/// <summary>
		/// Gets or sets the default camera.
		/// </summary>
		/// <value>The default camera.</value>
		public CameraDevice DefaultCamera
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether [save media on capture].
		/// </summary>
		/// <value><c>true</c> if [save media on capture]; otherwise, <c>false</c>.</value>
		public bool SaveMediaOnCapture
		{
			get; 
			set;
		}

		/// <summary>
		/// Gets or sets the length of the desired.
		/// </summary>
		/// <value>The length of the desired.</value>
		public TimeSpan DesiredLength
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the quality.
		/// </summary>
		/// <value>The quality.</value>
		public VideoQuality Quality
		{
			get;
			set;
		}
	}
	#endregion Video Options

}
