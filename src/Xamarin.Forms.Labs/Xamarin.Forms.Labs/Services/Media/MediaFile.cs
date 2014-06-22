// ***********************************************************************
// Assembly         : XForms.Toolkit
// Author           : Shawn Anderson
// Created          : 06-16-2014
//
// Last Modified By : Shawn Anderson
// Last Modified On : 06-16-2014
// ***********************************************************************
// <copyright file="MediaFile.cs" company="">
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
using System.IO;
using ExifLib;

namespace Xamarin.Forms.Labs.Services.Media
{
	/// <summary>
	/// Class MediaFile. This class cannot be inherited.
	/// </summary>
	public sealed class MediaFile : IDisposable
	{
		#region Private Member Variables

		/// <summary>
		/// The _dispose
		/// </summary>
		private readonly Action<bool> _dispose;

		/// <summary>
		/// The _path
		/// </summary>
		private readonly string _path;

		/// <summary>
		/// The _stream getter
		/// </summary>
		private readonly Func<Stream> _streamGetter;

		/// <summary>
		/// The _is disposed
		/// </summary>
		private bool _isDisposed;

		#endregion Private Member Variables

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaFile" /> class.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="streamGetter">The stream getter.</param>
		/// <param name="dispose">The dispose.</param>
		public MediaFile(string path, Func<Stream> streamGetter, Action<bool> dispose = null)
		{
			_dispose = dispose;
			_streamGetter = streamGetter;
			_path = path;
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="MediaFile" /> class.
		/// </summary>
		~MediaFile()
		{
			Dispose(false);
		}

		#endregion Constructors

		#region Public Properties

		/// <summary>
		/// Gets the path.
		/// </summary>
		/// <value>The path.</value>
		/// <exception cref="System.ObjectDisposedException">null</exception>
		public string Path
		{
			get
			{
				if (_isDisposed)
				{
					throw new ObjectDisposedException(null);
				}

				return _path;
			}
		}

		/// <summary>
		/// Gets the stream.
		/// </summary>
		/// <value>The source.</value>
		/// <exception cref="System.ObjectDisposedException">null</exception>
		public Stream Source
		{
			get
			{
				if (_isDisposed)
				{
				
					throw new ObjectDisposedException(null);
				}

				return _streamGetter();
			}
		}

		/// <summary>
		/// Gets the exif.
		/// </summary>
		/// <value>The exif.</value>
		/// <exception cref="System.ObjectDisposedException">null</exception>
		public JpegInfo Exif
		{
			get
			{
				if (_isDisposed)
				{
					throw new ObjectDisposedException(null);
				}

				var result = ExifReader.ReadJpeg(Source);

				Source.Seek(0, SeekOrigin.Begin);

				return result;
			}
		}
		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
		/// unmanaged resources.</param>
		private void Dispose(bool disposing)
		{
			if (_isDisposed)
			{
				return;
			}

			_isDisposed = true;
			if (_dispose != null)
			{
				_dispose(disposing);
			}
		}

		#endregion Private Methods
	}
}