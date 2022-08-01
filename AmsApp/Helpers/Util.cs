// <copyright file="Util.cs" company="Thinksmart, Inc.">
// Copyright (c) Thinksmart, Inc.. All rights reserved.
// </copyright>

namespace AmsApp.Helpers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class Util
    {
        public static void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            try
            {
                if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        public static void DeleteFolder(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            try
            {
                if (System.IO.Directory.Exists(filePath)) System.IO.Directory.Delete(filePath);
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        public static string GetMD5Hash(string input)
        {
            StringBuilder sb = new StringBuilder();
            using var md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            foreach (byte b in data)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        //public static string GetClaim(string claimType)
        //{
        //    claims = this.User.Claims;
        //    switch (claimType.ToUpper())
        //    {
        //        case "USERNAME":

        //            break;
        //        default:
        //            break;
        //    }
            
        //}
    }
}
