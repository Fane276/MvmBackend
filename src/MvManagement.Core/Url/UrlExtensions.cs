using System;

namespace MvManagement.Url
{
    public static class UrlExtensions
    {
        public static bool IsHttps(this string url)
        {
            return new Uri(url).IsHttps();
        }

        public static bool IsHttps(this Uri url)
        {
            return url.Scheme  == Uri.UriSchemeHttps;
        }
    }
}