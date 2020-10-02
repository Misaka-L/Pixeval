// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using PropertyChanged;

namespace Pixeval.Data.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class I18NOption
    {
        public static readonly I18NOption USEnglish = new I18NOption("English(US)", "en-us");

        public static readonly I18NOption ChineseSimplified = new I18NOption("简体中文(中国)", "zh-cn");

        public I18NOption(string localizedName, string name)
        {
            LocalizedName = localizedName;
            Name = name;
        }

        public I18NOption()
        {
        }

        public string LocalizedName { get; set; }

        public string Name { get; set; }
    }
}