// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedTextCellRenderer.cs" company="XLabs Team">
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
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Enums;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedTextCell), typeof(ExtendedTextCellRenderer))]
namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class ExtendedTextCellRenderer.
    /// </summary>
    public class ExtendedTextCellRenderer : TextCellRenderer
    {
        /// <summary>
        /// The default detail color
        /// </summary>
        private static readonly Color DefaultDetailColor = new Color(0.32, 0.4, 0.57);

        /// <summary>
        /// The default text color
        /// </summary>
        private static readonly Color DefaultTextColor = Color.Black;

        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="reusableCell">The reusable TableView cell.</param>
        /// <param name="tv">The TableView.</param>
        /// <returns>UITableViewCell.</returns>
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var extendedCell = (ExtendedTextCell)item;

            var textCell = (TextCell)item;
            var style = extendedCell.DetailLocation == TextCellDetailLocation.Right ?
                UITableViewCellStyle.Value1 :
                UITableViewCellStyle.Subtitle;

            var fullName = item.GetType().FullName;
            var cell = tv.DequeueReusableCell (fullName) as CellTableViewCell;
            if (cell == null) 
            {
                cell = new CellTableViewCell (style, fullName);
            }
            else 
            {
                cell.Cell.PropertyChanged -= cell.HandlePropertyChanged;
            }

            cell.Cell = textCell;
            textCell.PropertyChanged += cell.HandlePropertyChanged;
            cell.PropertyChanged = this.HandlePropertyChanged;
            cell.TextLabel.Text = textCell.Text;
            cell.DetailTextLabel.Text = textCell.Detail;
            cell.TextLabel.TextColor = textCell.TextColor.ToUIColor (DefaultTextColor);
            cell.DetailTextLabel.TextColor = textCell.DetailColor.ToUIColor (DefaultDetailColor);

            UpdateBackground (cell, item);

            cell.BackgroundColor = extendedCell.BackgroundColor.ToUIColor ();
            cell.SeparatorInset = new UIEdgeInsets (
                (nfloat)extendedCell.SeparatorPadding.Top, 
                (nfloat)extendedCell.SeparatorPadding.Left,
                (nfloat)extendedCell.SeparatorPadding.Bottom, 
                (nfloat)extendedCell.SeparatorPadding.Right);

            if (extendedCell.ShowDisclousure) 
            {
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                if (!string.IsNullOrEmpty (extendedCell.DisclousureImage)) 
                {
                    var detailDisclosureButton = UIButton.FromType (UIButtonType.Custom);
                    detailDisclosureButton.SetImage (UIImage.FromBundle (extendedCell.DisclousureImage), UIControlState.Normal);
                    detailDisclosureButton.SetImage (UIImage.FromBundle (extendedCell.DisclousureImage), UIControlState.Selected);

                    detailDisclosureButton.Frame = new CGRect (0f, 0f, 30f, 30f);
                    detailDisclosureButton.TouchUpInside += (sender, e) => 
                    {
                        var index = tv.IndexPathForCell (cell);
                        tv.SelectRow (index, true, UITableViewScrollPosition.None);
                        tv.Source.AccessoryButtonTapped (tv, index);
                    };
                    cell.AccessoryView = detailDisclosureButton;
                }
            }

            if (!extendedCell.ShowSeparator)
            {
                tv.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            }

            tv.SeparatorColor = extendedCell.SeparatorColor.ToUIColor();

            return cell;
        }
    }
}

