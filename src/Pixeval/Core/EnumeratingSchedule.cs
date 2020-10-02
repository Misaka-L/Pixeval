// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System;

namespace Pixeval.Core
{
    public class EnumeratingSchedule
    {
        private static object _currentItr;

        public static void StartNewInstance<T>(IPixivAsyncEnumerable<T> itr)
        {
            CancelCurrent();
            _currentItr = itr;
        }

        public static IPixivAsyncEnumerable<T> GetCurrentEnumerator<T>()
        {
            return _currentItr as IPixivAsyncEnumerable<T>;
        }

        public static void CancelCurrent()
        {
            var iterator = _currentItr as ICancellable;
            iterator?.Cancel();
            GC.Collect();
        }
    }
}