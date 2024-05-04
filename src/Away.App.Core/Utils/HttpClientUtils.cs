using Away.App.Core.API;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Away.App.Core.Utils;

public static class HttpClientUtils
{
    public static HttpClient CreateHttpClientByXrayProxy()
    {
        var xrayOptions = AwayLocator.GetService<IXrayOptions>();
        return CreateHttpClient(new WebProxy(xrayOptions.Host));
    }

    public static HttpClient CreateHttpClient(IWebProxy? proxy = null)
    {
        return new HttpClient(new HttpClientHandler
        {
            Proxy = proxy,
            ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
        });
    }

    public static Task<HttpResponseMessage> SendAsync(this HttpClient http, string rawText, bool ssl = false, CancellationToken cancellationToken = default)
    {
        HttpRequestMessage request = new();
        string[] lines = rawText.Split('\n');

        int contentStartIndex = -1;
        var contentTypeVal = string.Empty;

        // Parse headers
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line))
            {
                contentStartIndex = i + 1;
                break;
            }

            int colonIndex = line.IndexOf(':');
            if (colonIndex <= 0)
            {
                throw new FormatException("Invalid header format");
            }

            string headerName = line[..colonIndex].Trim();
            string headerValue = line[(colonIndex + 1)..].Trim();

            if (headerName.Equals("Content-Type", StringComparison.InvariantCultureIgnoreCase))
            {
                contentTypeVal = headerValue;
                continue;
            }

            request.Headers.TryAddWithoutValidation(headerName, headerValue);
        }

        // Parse request line
        string requestLine = lines[0].Trim();
        string[] requestLineParts = requestLine.Split(' ');
        if (requestLineParts.Length < 3)
        {
            throw new FormatException("Invalid request line format");
        }
        request.Method = new(requestLineParts[0]);
        var host = request.Headers.Host;
        var scheme = ssl ? "https" : "http";
        request.RequestUri = new($"{scheme}://{host}{requestLineParts[1]}");

        // Parse content if any
        if (contentStartIndex > -1 && contentStartIndex < lines.Length)
        {
            string content = string.Join("\n", lines, contentStartIndex, lines.Length - contentStartIndex).Trim();

            var items = contentTypeVal.Split(';', StringSplitOptions.RemoveEmptyEntries);
            var charset = items.Length == 2 ? items.Last() : "utf-8";
            request.Content = new ByteArrayContent(Encoding.GetEncoding(charset).GetBytes(content));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(items.First(), charset);
        }

        return http.SendAsync(request, cancellationToken);
    }

}
