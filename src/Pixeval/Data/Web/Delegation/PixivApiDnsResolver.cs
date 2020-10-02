// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System.Collections.Generic;
using System.Net;

namespace Pixeval.Data.Web.Delegation
{
    public class PixivApiDnsResolver : DnsResolver
    {
        public static readonly DnsResolver Instance = new PixivApiDnsResolver();

        protected override IEnumerable<IPAddress> UseDefaultDns()
        {
            yield return IPAddress.Parse("210.140.131.219");
            yield return IPAddress.Parse("210.140.131.223");
            yield return IPAddress.Parse("210.140.131.226");
        }
    }
}