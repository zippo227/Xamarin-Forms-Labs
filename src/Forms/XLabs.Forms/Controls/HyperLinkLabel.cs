// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="HyperLinkLabel.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class HyperLinkLabel.
	/// </summary>
	public class HyperLinkLabel : Label
	{
		/// <summary>
		/// The subject property
		/// </summary>
		public static readonly BindableProperty SubjectProperty;
		/// <summary>
		/// The navigate URI property
		/// </summary>
		public static readonly BindableProperty NavigateUriProperty;

		/// <summary>
		/// Initializes static members of the <see cref="HyperLinkLabel"/> class.
		/// </summary>
		static HyperLinkLabel()
		{
			SubjectProperty = BindableProperty.Create("Subject", typeof(string), typeof(HyperLinkLabel), string.Empty, BindingMode.OneWay, null, null, null, null);
			NavigateUriProperty = BindableProperty.Create("NavigateUri", typeof(string), typeof(HyperLinkLabel), string.Empty, BindingMode.OneWay, null, null, null, null);          
		}
		/// <summary>
		/// Gets or sets the subject.
		/// </summary>
		/// <value>The subject.</value>
		public string Subject
		{
			get { return (string)base.GetValue(SubjectProperty); }
			set { base.SetValue(SubjectProperty, value); }
		}

		/// <summary>
		/// Gets or sets the navigate URI.
		/// </summary>
		/// <value>The navigate URI.</value>
		public string NavigateUri
		{
			get { return (string)base.GetValue(NavigateUriProperty); }
			set { base.SetValue(NavigateUriProperty, value); }
		}
	}
}
