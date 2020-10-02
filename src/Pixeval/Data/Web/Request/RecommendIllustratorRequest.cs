// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using Refit;

namespace Pixeval.Data.Web.Request
{
    public class RecommendIllustratorRequest
    {
        [AliasAs("filter")]
        public string Filter { get; } = "for_android";

        [AliasAs("offset")]
        public int Offset { get; set; } = 0;
    }
}