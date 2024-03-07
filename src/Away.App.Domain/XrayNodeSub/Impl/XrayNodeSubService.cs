﻿using System.Text.RegularExpressions;

namespace Away.App.Domain.XrayNodeSub.Impl;

[DI]
public sealed class XrayNodeSubService(IHttpClientFactory httpClientFactory) : IXrayNodeSubService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("xray");
    private CancellationTokenSource _cts = new();

    public void Cancel()
    {
        _cts.Cancel();
    }

    public async Task<List<string>> GetXrayNode(string url)
    {
        _cts = new();
        List<string> nodes = [];
        try
        {
            if (url.EndsWith("README.md"))
            {
                nodes = await GetREADME(url);
                return nodes;
            }

            var text = await Request(url);
            return [.. XrayUtils.Base64Decode(text).Split('\n', StringSplitOptions.RemoveEmptyEntries)];
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "获取订阅节点失败");
        }
        return nodes;
    }

    private async Task<List<string>> GetREADME(string url)
    {
        List<string> nodes = [];
        var content = await Request(url);
        if (string.IsNullOrWhiteSpace(content))
        {
            return nodes;
        }
        var reg = Regex.Match(content, "```(?<nodes>[^`]+)```");
        if (!reg.Success)
        {
            return nodes;
        }
        var text = reg.Result("${nodes}");
        return [.. text.Split('\n', StringSplitOptions.RemoveEmptyEntries)];
    }

    private Task<string> Request(string url)
    {
        return _httpClient.GetStringAsync(url, _cts.Token);
    }
}