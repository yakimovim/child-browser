using ChildBrowser.Bookmarks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChildBrowser
{
    interface IUrisProvider
    {
        IEnumerable<Uri> GetUris();
    }

    class AddressCompletionProvider
    {
        private LinkedList<IUrisProvider> _urisProviders = new LinkedList<IUrisProvider>();

        public void RegisterProvider(IUrisProvider provider)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _urisProviders.AddLast(provider);
        }

        public string GetAddressCompletion(string addressStart)
        {
            if (string.IsNullOrWhiteSpace(addressStart)) return string.Empty;

            var startUri = BrowserUrl.ConvertToUri(addressStart, true);

            foreach (var uri in GetUris())
            {
                var matchingEnd = GetMatchingEnd(startUri, uri);
                if (!string.IsNullOrEmpty(matchingEnd))
                    return matchingEnd;
            }

            return string.Empty;
        }

        private IEnumerable<Uri> GetUris()
        {
            return _urisProviders
                .SelectMany(p => p.GetUris());
        }

        private string GetMatchingEnd(Uri startUri, Uri uri)
        {
            var builder = new UriBuilder(uri);
            builder.Scheme = startUri.Scheme;

            uri = builder.Uri;

            var startAddress = startUri.ToString().TrimEnd('/');
            var address = uri.ToString();

            if(address.StartsWith(startAddress, StringComparison.OrdinalIgnoreCase))
            {
                return address.Substring(startAddress.Length);
            }

            return string.Empty;
        }
    }

    class BookmarksUriProvider : IUrisProvider
    {
        private readonly Func<IEnumerable<Bookmark>> _bookmarksProvider;

        public BookmarksUriProvider(Func<IEnumerable<Bookmark>> bookmarksProvider)
        {
            _bookmarksProvider = bookmarksProvider ?? throw new ArgumentNullException(nameof(bookmarksProvider));
        }

        public IEnumerable<Uri> GetUris()
        {
            return (_bookmarksProvider() ?? new Bookmark[0])
                .Select(b =>
                {
                    try
                    {
                        return BrowserUrl.ConvertToUri(b.Address, true);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(u => u != null);
        }
    }

    class AllowedUrisProvider : IUrisProvider
    {
        private readonly BrowserUrl _browserUrl;

        public AllowedUrisProvider(BrowserUrl browserUrl)
        {
            _browserUrl = browserUrl ?? throw new ArgumentNullException(nameof(browserUrl));
        }

        public IEnumerable<Uri> GetUris()
        {
            return _browserUrl.AllowedUris;
        }
    }
}
