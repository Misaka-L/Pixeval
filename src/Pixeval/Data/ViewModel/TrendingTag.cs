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
    public class TrendingTag
    {
        public string Tag { get; set; }

        public string TranslatedName { get; set; }

        public string Thumbnail { get; set; }
    }
}