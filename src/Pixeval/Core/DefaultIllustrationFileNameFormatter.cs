// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System.IO;
using Pixeval.Data.ViewModel;
using Pixeval.Objects.Primitive;

namespace Pixeval.Core
{
    public class DefaultIllustrationFileNameFormatter : IIllustrationFileNameFormatter
    {
        public string Format(Illustration illustration)
        {
            return $"[{Strings.FormatPath(illustration.UserName)}]{illustration.Id}{Path.GetExtension(illustration.Origin.IsNullOrEmpty() ? illustration.Large : illustration.Origin)}";
        }

        public string FormatManga(Illustration illustration, int idx)
        {
            return $"[{Strings.FormatPath(illustration.UserName)}]{illustration.Id}_p{idx}{Path.GetExtension(illustration.Origin.IsNullOrEmpty() ? illustration.Large : illustration.Origin)}";
        }

        public string FormatGif(Illustration illustration)
        {
            return $"[{Strings.FormatPath(illustration.UserName)}]{illustration.Id}.gif";
        }
    }
}