// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IconLabelRenderer.cs" company="XLabs Team">
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

using System;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Enums;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(IconLabel), typeof(IconLabelRenderer))]

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// IconLabelRender implementation.
	/// </summary>
	public class IconLabelRenderer : LabelRenderer
	{
		Typeface _iconFont;
		Typeface _textFont;
		IconLabel _iconLabel;
		//Final span including font and icon size and color
		SpannableString _iconSpan;
		int _textStartIndex = -1;
		int _textStopIndex = -1;
		Android.Widget.TextView _nativeLabel;

		/// <summary>
		/// Initializes a new instance of the <see cref="IconLabelRenderer"/> class.
		/// </summary>
		public IconLabelRenderer() : base()
		{

		}

		/// <summary>
		/// Handles the On Element Changed messages
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);
			if (this.Control != null && e.NewElement != null)
			{
				if (_iconSpan == null)
				{
					_nativeLabel = (Android.Widget.TextView) this.Control;
					_iconLabel = (IconLabel) e.NewElement;
					//Set default value
					if (_iconLabel.IconSize == 0)
						_iconLabel.IconSize = _iconLabel.FontSize;



					_iconFont = TrySetFont("fontawesome-webfont.ttf");
					_textFont = _iconLabel.Font.ToTypeface();
					SetText();


				}
			}
		}

		/// <summary>
		/// Rebuild the all span in and set it into the label
		/// </summary>
		private void SetText()
		{
			var computedString = BuildRawTextString();

			_iconSpan = BuildSpannableString(computedString);
			if (_iconLabel.TextAlignement == Xamarin.Forms.TextAlignment.Center)
			{
				_nativeLabel.Gravity = Android.Views.GravityFlags.Center;

			}
			else if (_iconLabel.TextAlignement == Xamarin.Forms.TextAlignment.End)
			{
				_nativeLabel.Gravity = Android.Views.GravityFlags.Right;
			}
			else if (_iconLabel.TextAlignement == Xamarin.Forms.TextAlignment.Start)
			{
				_nativeLabel.Gravity = Android.Views.GravityFlags.Left;
			}
			_nativeLabel.SetText(_iconSpan, TextView.BufferType.Spannable);
		}


		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (_iconSpan != null)
			{
				if (e.PropertyName == IconLabel.IconColorProperty.PropertyName ||
				    e.PropertyName == IconLabel.IconProperty.PropertyName ||
				    e.PropertyName == IconLabel.TextProperty.PropertyName ||
				    e.PropertyName == IconLabel.TextColorProperty.PropertyName ||
				    e.PropertyName == IconLabel.IsVisibleProperty.PropertyName ||
				    e.PropertyName == IconLabel.IconSizeProperty.PropertyName ||
				    e.PropertyName == IconLabel.FontSizeProperty.PropertyName ||
				    e.PropertyName == IconLabel.OrientationProperty.PropertyName)
				{
					SetText();
				}
			}


		}

		/// <summary>
		/// Build the content string by concating icon and text according to control options
		/// </summary>
		/// <returns></returns>
		private string BuildRawTextString()
		{
			string computedText = string.Empty;
			if (!string.IsNullOrEmpty(_iconLabel.Icon) && !string.IsNullOrEmpty(_iconLabel.Text))
			{
				string iconSeparator = _iconLabel.ShowIconSeparator ? " | " : " ";

				switch (_iconLabel.Orientation)
				{
					case ImageOrientation.ImageToLeft:

						computedText = _iconLabel.Icon + iconSeparator + _iconLabel.Text;
						_textStartIndex = computedText.IndexOf(iconSeparator);
						_textStopIndex = computedText.Length;

						break;
					case ImageOrientation.ImageToRight:
						computedText = _iconLabel.Text + iconSeparator + _iconLabel.Icon;
						_textStartIndex = 0;
						_textStopIndex = computedText.IndexOf(iconSeparator) + iconSeparator.Length;
						break;
					case ImageOrientation.ImageOnTop:
						computedText = _iconLabel.Icon + System.Environment.NewLine + _iconLabel.Text;
						_textStartIndex = computedText.IndexOf(_iconLabel.Text);
						_textStopIndex = computedText.Length - 1;
						break;
					case ImageOrientation.ImageOnBottom:
						computedText = _iconLabel.Text + System.Environment.NewLine + _iconLabel.Icon;
						_textStartIndex = 0;
						_textStopIndex = computedText.IndexOf(System.Environment.NewLine) - 1;
						break;
				}
			}
			else if (!string.IsNullOrEmpty(_iconLabel.Text) && string.IsNullOrEmpty(_iconLabel.Icon))
			{
				computedText = _iconLabel.Text;
			}
			else if (string.IsNullOrEmpty(_iconLabel.Text) && !string.IsNullOrEmpty(_iconLabel.Icon))
			{
				computedText = _iconLabel.Icon;
			}
			return computedText;
		}

		/// <summary>
		/// Build the spannable according to the computed text, meaning set the right font, color and size to the text and icon char index
		/// </summary>
		/// <param name="computedString"></param>
		private SpannableString BuildSpannableString(string computedString)
		{
			SpannableString span = new SpannableString(computedString);
			if (!string.IsNullOrEmpty(_iconLabel.Icon))
			{
				//set icon
				span.SetSpan(new CustomTypefaceSpan("fontawesome", _iconFont, _iconLabel.IconColor.ToAndroid()),
					computedString.IndexOf(_iconLabel.Icon),
					computedString.IndexOf(_iconLabel.Icon) + _iconLabel.Icon.Length,
					SpanTypes.ExclusiveExclusive);
				//set icon size
				span.SetSpan(new AbsoluteSizeSpan((int) _iconLabel.IconSize, true),
					computedString.IndexOf(_iconLabel.Icon),
					computedString.IndexOf(_iconLabel.Icon) + _iconLabel.Icon.Length,
					SpanTypes.ExclusiveExclusive);


			}
			if (!string.IsNullOrEmpty(_iconLabel.Text))
			{
				span.SetSpan(new CustomTypefaceSpan("", _textFont, _iconLabel.TextColor.ToAndroid()),
					_textStartIndex,
					_textStopIndex,
					SpanTypes.ExclusiveExclusive);
				span.SetSpan(new AbsoluteSizeSpan((int) _iconLabel.FontSize, true),
					_textStartIndex,
					_textStopIndex,
					SpanTypes.ExclusiveExclusive);


			}

			return span;

		}

		/// <summary>
		/// Load the FA font from assets
		/// </summary>
		/// <param name="fontName"></param>
		/// <returns></returns>
		private Typeface TrySetFont(string fontName)
		{
			try
			{
				var tp = Typeface.CreateFromAsset(Context.Assets, "fonts/" + fontName);

				return tp;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("not found in assets. Exception: {0}", ex));
				try
				{
					return Typeface.CreateFromFile("fonts/" + fontName);
				}
				catch (Exception ex1)
				{
					System.Diagnostics.Debug.WriteLine(string.Format("not found by file. Exception: {0}", ex1));

					return Typeface.Default;
				}
			}
		}


	}
}
