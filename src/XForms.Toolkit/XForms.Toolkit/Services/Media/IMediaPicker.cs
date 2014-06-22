// ***********************************************************************
// Assembly         : XForms.Toolkit
// Author           : Shawn Anderson
// Created          : 06-16-2014
//
// Last Modified By : Shawn Anderson
// Last Modified On : 06-16-2014
// ***********************************************************************
// <copyright file="IMediaPicker.cs" company="">
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

namespace Xamarin.Forms.Labs.Services.Media
{
	/// <summary>
	/// Interface IMediaPicker
	/// </summary>
	public interface IMediaPicker
	{
		/// <summary>
		/// Gets a value indicating whether this instance is camera available.
		/// </summary>
		/// <value><c>true</c> if this instance is camera available; otherwise, <c>false</c>.</value>
		bool IsCameraAvailable { get; }
		/// <summary>
		/// Gets a value indicating whether this instance is photos supported.
		/// </summary>
		/// <value><c>true</c> if this instance is photos supported; otherwise, <c>false</c>.</value>
		bool IsPhotosSupported { get; }
		/// <summary>
		/// Gets a value indicating whether this instance is videos supported.
		/// </summary>
		/// <value><c>true</c> if this instance is videos supported; otherwise, <c>false</c>.</value>
		bool IsVideosSupported { get; }

		/// <summary>
		/// Select a picture from library.
		/// </summary>
		/// <param name="options">The storage options.</param>
		/// <returns>Task&lt;IMediaFile&gt;.</returns>
		Task<MediaFile> SelectPhotoAsync(CameraMediaStorageOptions options);

		/// <summary>
		/// Takes the picture.
		/// </summary>
		/// <param name="options">The storage options.</param>
		/// <returns>Task&lt;IMediaFile&gt;.</returns>
		Task<MediaFile> TakePhotoAsync(CameraMediaStorageOptions options);

		/// <summary>
		/// Selects the video asynchronous.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns>Task&lt;IMediaFile&gt;.</returns>
		Task<MediaFile> SelectVideoAsync(VideoMediaStorageOptions options);

		/// <summary>
		/// Takes the video asynchronous.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns>Task&lt;IMediaFile&gt;.</returns>
		Task<MediaFile> TakeVideoAsync(VideoMediaStorageOptions options);

		/// <summary>
		/// Event the fires when media has been selected
		/// </summary>
		/// <value>The on photo selected.</value>
		EventHandler<MediaPickerArgs> OnMediaSelected { get; set; }

		/// <summary>
		/// Gets or sets the on error.
		/// </summary>
		/// <value>The on error.</value>
		EventHandler<MediaPickerErrorArgs> OnError { get; set; }
	}

	/// <summary>
	/// Class MediaPickerArgs.
	/// </summary>
	public class MediaPickerArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MediaPickerArgs" /> class.
		/// </summary>
		/// <param name="mf">The mf.</param>
		public MediaPickerArgs(MediaFile mf)
		{
			MediaFile = mf;
		}

		/// <summary>
		/// Gets the media file.
		/// </summary>
		/// <value>The media file.</value>
		public MediaFile MediaFile { get; private set; }
	}

	/// <summary>
	/// Class MediaPickerErrorArgs.
	/// </summary>
	public class MediaPickerErrorArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MediaPickerErrorArgs" /> class.
		/// </summary>
		/// <param name="ex">The ex.</param>
		public MediaPickerErrorArgs(Exception ex)
		{
			Error = ex;
		}

		/// <summary>
		/// Gets the error.
		/// </summary>
		/// <value>The error.</value>
		public Exception Error { get; private set; }
	}
}