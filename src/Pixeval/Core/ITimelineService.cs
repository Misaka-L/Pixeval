// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using Pixeval.Data.ViewModel;

namespace Pixeval.Core
{
    /// <summary>
    ///     Provides a set of functions that support a Browsing Timeline
    /// </summary>
    public interface ITimelineService
    {
        /// <summary>
        ///     Check if the <see cref="BrowsingHistory" /> has the rationality to be insert to timeline
        /// </summary>
        /// <param name="browsingHistory"></param>
        /// <returns></returns>
        bool VerifyRationality(BrowsingHistory browsingHistory);

        /// <summary>
        ///     Insert a <see cref="BrowsingHistory" /> to timeline
        /// </summary>
        /// <param name="browsingHistory"></param>
        void Insert(BrowsingHistory browsingHistory);
    }
}