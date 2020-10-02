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
    public class User
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public bool IsFollowed { get; set; }

        public string Avatar { get; set; }

        public string Introduction { get; set; }

        public string Background { get; set; }

        public int Follows { get; set; }

        public bool IsPremium { get; set; }

        public string[] Thumbnails { get; set; } = new string[3];
    }
}