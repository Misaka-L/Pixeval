// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System.Net.Http;

namespace Pixeval.Data.Web.Delegation
{
    public class PixivApiHttpClientHandler : DnsResolvedHttpClientHandler
    {
        private PixivApiHttpClientHandler(bool directConnect) : base(PixivAuthenticationHttpRequestHandler.Instance, directConnect)
        {
        }

        protected override DnsResolver DnsResolver { get; set; } = PixivApiDnsResolver.Instance;

        public static HttpMessageHandler Instance(bool directConnect)
        {
            return new PixivApiHttpClientHandler(directConnect);
        }
    }
}