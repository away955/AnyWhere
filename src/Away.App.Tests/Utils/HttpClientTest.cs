using Away.App.Core.Utils;

namespace Away.App.Tests;

public sealed class HttpClientTest
{
    [InlineData("""
        GET /posts/1 HTTP/1.1
        Host: jsonplaceholder.typicode.com
        Cookie: _ga=GA1.1.866155930.1714125461; _ga_E3C3GCQVBN=GS1.1.1714128464.2.0.1714128464.0.0.0
        """)]
    [InlineData("""
        POST /posts HTTP/1.1
        Host: jsonplaceholder.typicode.com
        Content-Type: application/json

        {"title":"foo","body":"bar","userId":1}
        """)]
    [InlineData("""
        PATCH /posts/1 HTTP/1.1
        Host: jsonplaceholder.typicode.com
        Content-Type: application/json
        
        {"title":"foo","body":"bar"}
        """)]
    [InlineData("""
        PUT /posts/1 HTTP/1.1
        Host: jsonplaceholder.typicode.com
        Content-Type: application/json
        Content-Length: 46

        {"title":"foo","body":"bar","userId":1,"id":1}
        """)]
    [InlineData("""
        DELETE /posts/1 HTTP/1.1
        Host: jsonplaceholder.typicode.com
        """)]
    [Theory]
    public async Task RawTextText(string rawText)
    {
        using var http = HttpClientUtils.CreateHttpClient();
        var resp = await http.SendAsync(rawText);
        Assert.True(resp.IsSuccessStatusCode);
    }
}
