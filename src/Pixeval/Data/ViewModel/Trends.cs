// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System;
using PropertyChanged;

namespace Pixeval.Data.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class Trends
    {
        public string PostUserId { get; set; }

        public string PostUserName { get; set; }

        public string TrendObjectId { get; set; }

        public DateTime PostDate { get; set; }

        public TrendType Type { get; set; }

        public string ByName { get; set; }

        public bool IsReferToUser { get; set; }

        public string TrendObjName { get; set; }

        public string TrendObjectThumbnail { get; set; }

        public string PostUserThumbnail { get; set; }
    }

    public enum TrendType
    {
        /// <summary>
        ///     Bookmark
        /// </summary>
        AddBookmark,

        /// <summary>
        ///     New illust
        /// </summary>
        AddIllust,

        /// <summary>
        ///     New follow
        /// </summary>
        AddFavorite
    }
}