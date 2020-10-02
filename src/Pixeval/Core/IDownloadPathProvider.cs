// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using Pixeval.Data.ViewModel;

namespace Pixeval.Core
{
    public interface IIllustrationFileNameFormatter
    {
        string Format(Illustration illustration);

        string FormatManga(Illustration illustration, int idx);

        string FormatGif(Illustration illustration);
    }

    public interface IDownloadPathProvider
    {
        string GetSpotlightPath(string title, DownloadOption option = null);

        string GetIllustrationPath(DownloadOption option = null);

        string GetMangaPath(string id, DownloadOption option = null);
    }

    public class DownloadOption
    {
        public string RootDirectory { get; set; }

        public bool CreateNewWhenFromUser { get; set; }
    }
}