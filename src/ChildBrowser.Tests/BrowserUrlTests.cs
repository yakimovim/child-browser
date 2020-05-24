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
                "contoso.org/contacts"
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
        [InlineData("contoso.org/about", null)] // it is not allow to start with different segments

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
