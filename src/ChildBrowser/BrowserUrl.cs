using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChildBrowser
{
    public class BrowserUrl
    {
        private readonly IReadOnlyCollection<Uri> _allowedUris;

        public BrowserUrl(IReadOnlyCollection<string> allowedAddresses)
        {
            if (allowedAddresses is null)
            {
                throw new ArgumentNullException(nameof(allowedAddresses));
            }

            var allowedUris = new LinkedList<Uri>();

            foreach (var allowedAddress in allowedAddresses)
            {
                var uri = ConvertToUri(allowedAddress?.ToLower(), false);

                if(uri == null)
                    throw new ArgumentException($"Invalid URL '{allowedAddress}'");

                allowedUris.AddLast(uri);
            }

            _allowedUris = allowedUris;
        }

        public Uri GetUri(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;

            var uri = ConvertToUri(url.ToLower(), true);

            if (uri == null) return null;

            foreach (var allowedUri in _allowedUris)
            {
                if (Match(allowedUri, uri))
                    return uri;
            }

            return null;
        }

        private Uri ConvertToUri(string address, bool useHttps)
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
            if (scheme != Uri.UriSchemeHttp && scheme != Uri.UriSchemeHttps) return false;
            if (allowedScheme == Uri.UriSchemeHttps && scheme != Uri.UriSchemeHttps) return false;
            return null;
        }

        private bool? HostMatch(string allowedHost, string host)
        {
            if (allowedHost == host) return null;
            if (allowedHost == $"www.{host}") return null;
            if ($"www.{allowedHost}" == host) return null;
            return false;
        }

        private bool? PathSegmentsMatch(string[] allowedSegments, string[] segments)
        {
            if (allowedSegments.Length > segments.Length) return false;

            for (int i = 0; i < allowedSegments.Length; i++)
            {
                if (allowedSegments[i].TrimEnd('/') != segments[i].TrimEnd('/')) return false;
            }

            return null;
        }
    }
}
