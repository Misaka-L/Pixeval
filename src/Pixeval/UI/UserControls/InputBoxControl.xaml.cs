// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System.Windows.Input;

namespace Pixeval.UI.UserControls
{
    /// <summary>
    ///     Interaction logic for InputBoxWindow.xaml
    /// </summary>
    public partial class InputBoxControl
    {
        public InputBoxControl()
        {
            InitializeComponent();
        }

        private void ConditionTextBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Oem5) e.Handled = true;
        }
    }
}