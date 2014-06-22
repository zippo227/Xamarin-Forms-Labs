// ***********************************************************************
// Assembly         : XForms.Toolkit
// Author           : Shawn Anderson
// Created          : 06-16-2014
//
// Last Modified By : Shawn Anderson
// Last Modified On : 06-16-2014
// ***********************************************************************
// <copyright file="MediaFileNoteFoundException.cs" company="">
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
	/// <summary>
	/// Class MediaFileNotFoundException.
	/// </summary>
	public class MediaFileNotFoundException
		: Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MediaFileNotFoundException" /> class.
		/// </summary>
		/// <param name="path">The path.</param>
		public MediaFileNotFoundException (string path)
			: base ("Unable to locate media file at " + path)
		{
			Path = path;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaFileNotFoundException" /> class.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="innerException">The inner exception.</param>
		public MediaFileNotFoundException (string path, Exception innerException)
			: base ("Unable to locate media file at " + path, innerException)
		{
			Path = path;
		}

		/// <summary>
		/// Gets the path.
		/// </summary>
		/// <value>The path.</value>
		public string Path
		{
			get;
			private set;
		}
	}
}
