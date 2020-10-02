// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pixeval.Data.Web.Response
{
    public class SpotlightArticleResponse
    {
        [JsonProperty("body")]
        public List<Body> BodyList { get; set; }

        public class Body
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("illusts")]
            public List<Illust> Illusts { get; set; }
        }

        public class Illust
        {
            [JsonProperty("illust_id")]
            public long IllustId { get; set; }
        }
    }
}