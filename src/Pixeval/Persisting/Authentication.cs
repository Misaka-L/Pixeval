// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using EmbedIO;
using Pixeval.Data.Web;
using Pixeval.Data.Web.Delegation;
using Pixeval.Data.Web.Protocol;
using Pixeval.Data.Web.Request;
using Pixeval.Objects.Exceptions;
using Pixeval.Objects.Generic;
using Pixeval.Objects.I18n;
using Pixeval.Objects.Primitive;
using Pixeval.Objects.Web;
using Pixeval.Persisting.WebApi;
using Pixeval.UI;
using Refit;

namespace Pixeval.Persisting
{
    /// <summary>
    ///     A helper class to process the Pixiv authentication
    /// </summary>
    public class Authentication
    {
        private const string ClientHash = "28c1fdd170a5204386cb1313c7077b34f83e4aaf4aa829ce78c231e05b0bae2c";

        private static string UtcTimeNow => DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss+00:00");

        /// <summary>
        ///     App-API authentication process using account and password
        /// </summary>
        /// <param name="name">account</param>
        /// <param name="pwd">password</param>
        /// <returns></returns>
        public static async Task AppApiAuthenticate(string name, string pwd)
        {
            var time = UtcTimeNow;
            var hash = (time + ClientHash).Hash<MD5CryptoServiceProvider>();

            try
            {
                var token = await RestService.For<ITokenProtocol>(HttpClientFactory.PixivApi(ProtocolBase.OAuthBaseUrl, true).Apply(h => h.Timeout = TimeSpan.FromSeconds(10))).GetTokenByPassword(new PasswordTokenRequest {Name = name, Password = pwd}, time, hash);
                Session.Current = Session.Parse(pwd, token);
            }
            catch (TaskCanceledException)
            {
                throw new AuthenticateFailedException(AkaI18N.AppApiAuthenticateTimeout);
            }
        }

        /// <summary>
        ///     App-API authentication process using specified refresh token
        /// </summary>
        /// <param name="refreshToken">refresh token</param>
        /// <returns></returns>
        public static async Task AppApiAuthenticate(string refreshToken)
        {
            try
            {
                var token = await RestService.For<ITokenProtocol>(HttpClientFactory.PixivApi(ProtocolBase.OAuthBaseUrl, true).Apply(h => h.Timeout = TimeSpan.FromSeconds(10))).RefreshToken(new RefreshTokenRequest {RefreshToken = refreshToken});
                Session.Current = Session.Parse(Session.Current.Password, token);
            }
            catch (TaskCanceledException)
            {
                throw new AuthenticateFailedException(AkaI18N.AppApiAuthenticateTimeout);
            }
        }
    }
}