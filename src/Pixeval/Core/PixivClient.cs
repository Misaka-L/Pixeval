// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

namespace Pixeval.Core
{
    public sealed class PixivClient
    {
        private static volatile PixivClient _instance;

        private static readonly object _locker = new object();

        public static PixivClient Instance
        {
            get
            {
                if (_instance == null)
                    lock (_locker)
                    {
                        _instance ??= new PixivClient();
                    }

                return _instance;
            }
        }
    }
}