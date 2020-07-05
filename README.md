# Child browser

An internet browser for a child with restriction of sites she can visit.

To specify sites you want to allow the user to see, please add them to the `AllowedHosts.txt` file. User must have only read permissions for this file in order to disallow her to modify it. There are the following rules:

* If you don't specify the scheme, both `http` and `https` are allowed. So if you write `google.com` then both `https://google.com` and `http://google.com` are allowed.
* If you specify `http` scheme, `https` scheme is still allowed. For example, if you write `http://google.com`, `https://google.com` is still allowed.
* If you specify `https` scheme, `http` scheme is not allowed. So if you write `https://google.com` the `http://google.com` is not allowed.
* The program automatically adds and removes `www` prefix. So if you write `http://google.com`, `http://www.google.com` is allowed. And if you write `http://www.google.com`, `http://google.com` is allowed.
* You can specify the start of paths for your URLs. If you write `https://example.com/data` then `https://example.com/data/1` and `https://example.com/data/report` are allowed. But `https://example.com/views` and `https://example.com/datagram` ae not allowed.
* Any query parts and hashes are allowed. E.g. if you write `https://example.com` user can contact `https://example.com/data?a=1&b=3#start`.

## Icons

Icons made by [Freepik](https://www.flaticon.com/authors/freepik) from [www.flaticon.com](https://www.flaticon.com).