using System;
using System.Collections.Generic;

namespace ChildBrowser
{
    public class BrowserUrl
    {
        public IReadOnlyCollection<Uri> AllowedUris { get; }

        public BrowserUrl(IReadOnlyCollection<string> allowedAddresses)
        {
            if (allowedAddresses is null)
            {
                throw new ArgumentNullException(nameof(allowedAddresses));
            }

            var allowedUris = new LinkedList<Uri>();

            foreach (var allowedAddress in allowedAddresses)
            {
                var uri = ConvertToUri(allowedAddress, false);

                if(uri == null)
                    throw new ArgumentException($"Invalid URL '{allowedAddress}'");

                allowedUris.AddLast(uri);
            }

            AllowedUris = allowedUris;
        }

        public Uri GetUri(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;

            var uri = ConvertToUri(url, true);

            if (uri == null) return null;

            foreach (var allowedUri in AllowedUris)
            {
                if (Match(allowedUri, uri))
                    return uri;
            }

            return null;
        }

        public static Uri ConvertToUri(string address, bool useHttps)
        {
            var schema = useHttps ? "https" : "http";

            if (Uri.TryCreate(address, UriKind.Absolute, out var uri))
            {
                return uri;
            }
            else if (Uri.TryCreate($"{schema}://{address}", UriKind.Absolute, out uri))
            {
                return uri;
            }
            else
                return null;
        }

        private bool Match(Uri allowedUri, Uri uri)
        {
            return SchemeMatch(allowedUri.Scheme, uri.Scheme)
                ?? HostMatch(allowedUri.Host, uri.Host)
                ?? PathSegmentsMatch(allowedUri.Segments, uri.Segments)
                ?? true;
        }

        private bool? SchemeMatch(string allowedScheme, string scheme)
        {
            if (!Uri.UriSchemeHttp.Equals(scheme, StringComparison.OrdinalIgnoreCase) 
                && !Uri.UriSchemeHttps.Equals(scheme, StringComparison.OrdinalIgnoreCase)) return false;
            if (Uri.UriSchemeHttps.Equals(allowedScheme, StringComparison.OrdinalIgnoreCase) 
                && !Uri.UriSchemeHttps.Equals(scheme, StringComparison.OrdinalIgnoreCase)) return false;
            return null;
        }

        private bool? HostMatch(string allowedHost, string host)
        {
            if (string.Equals(allowedHost, host, StringComparison.OrdinalIgnoreCase)) return null;
            if (string.Equals(allowedHost, $"www.{host}", StringComparison.OrdinalIgnoreCase)) return null;
            if (string.Equals($"www.{allowedHost}", host, StringComparison.OrdinalIgnoreCase)) return null;
            return false;
        }

        private bool? PathSegmentsMatch(string[] allowedSegments, string[] segments)
        {
            if (allowedSegments.Length > segments.Length) return false;

            for (int i = 0; i < allowedSegments.Length; i++)
            {
                if (!string.Equals(allowedSegments[i].TrimEnd('/'), segments[i].TrimEnd('/'), StringComparison.OrdinalIgnoreCase)) return false;
            }

            return null;
        }
    }
}
