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
    public class AutoCompletionResponse
    {
        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }

        public class Tag
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("translated_name")]
            public string TranslatedName { get; set; }
        }
    }
}