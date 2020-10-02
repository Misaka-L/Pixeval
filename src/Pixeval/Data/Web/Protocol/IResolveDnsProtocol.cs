// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System.Threading.Tasks;
using Pixeval.Data.Web.Request;
using Pixeval.Data.Web.Response;
using Refit;

namespace Pixeval.Data.Web.Protocol
{
    public interface IResolveDnsProtocol
    {
        [Get("/dns-query")]
        Task<DnsResolveResponse> ResolveDns(DnsResolveRequest dnsResolverRequest);
    }
}