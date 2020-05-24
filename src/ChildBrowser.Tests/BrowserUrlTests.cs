using System;
using Xunit;

namespace ChildBrowser.Tests
{
    public class BrowserUrlTests
    {
        private readonly BrowserUrl _sut;

        public BrowserUrlTests()
        {
            _sut = new BrowserUrl(new[] {
                "http://google.com",
                "youtube.com",
                "https://www.example.com",
                "contoso.org/contacts",
                "wiki.com?a=1&b=2",
                "abc.ru#data"
            });
        }

        [Theory]
        /* Schema tests */
        [InlineData("http://www.example.com", null)] // don't allow to change `https` to `http`
        [InlineData("ftp://www.example.com", null)] // only `https` and `http` are allowed
        [InlineData("http://youtube.com", "http://youtube.com")] // allow `http` schema by default
        [InlineData("https://youtube.com", "https://youtube.com")] // allow `https` schema by default
        /* Host tests */
        [InlineData("http://google.com", "http://google.com")] // same host
        [InlineData("https://google.comb", null)] // different host
        [InlineData("http://www.google.com", "http://www.google.com")] // it is allowed to add `www`
        [InlineData("example.com", "https://example.com")] // it is allowed to remove `www`
        /* Path segments */
        [InlineData("http://google.com/some", "http://google.com/some")] // additional path segments are allowed
        [InlineData("contoso.org/contacts/about", "https://contoso.org/contacts/about")] // additional path segments are allowed
        [InlineData("contoso.org/about", null)] // it is not allowed to start with different segments
        /* Query */
        [InlineData("wiki.com", "https://wiki.com")] // query in allowed addresses is ignored
        [InlineData("wiki.com?c=4", "https://wiki.com?c=4")] // query in allowed addresses is ignored
        /* Hash */
        [InlineData("abc.ru", "https://abc.ru")] // hash in allowed addresses is ignored
        [InlineData("abc.ru/1/2?f=3#ddd", "https://abc.ru/1/2?f=3#ddd")] // hash in allowed addresses is ignored
        public void Test_GetUri(string input, string output)
        {
            var result = _sut.GetUri(input);

            if (output == null && result == null) return;

            Assert.True(output != null && result != null);

            var expected = new Uri(output);

            Assert.Equal(expected, result);
        }
    }
}
