﻿using Away.App.Domain.Xray;
using Away.Domain.XrayNode.Model;
using Away.Domain.XrayNode;

namespace Away.App.Tests.Xray;

public sealed class XrayNodeTest : TestBase
{
    [Theory]
    [InlineData("vmess://ew0KICAidiI6ICIyIiwNCiAgInBzIjogIue+juWbvSBDbG91ZEZsYXJl6IqC54K5IiwNCiAgImFkZCI6ICJ3d3cuY3Jpc3B5cmFpbmJvdy5iaXoiLA0KICAicG9ydCI6ICI4MDgwIiwNCiAgImlkIjogIjIyODI2YjQ0LTVjMWEtNGI0Yi1kYmFhLTgzYTJlOGJkOTVmMCIsDQogICJhaWQiOiAiMCIsDQogICJzY3kiOiAiYXV0byIsDQogICJuZXQiOiAid3MiLA0KICAidHlwZSI6ICJub25lIiwNCiAgImhvc3QiOiAiIiwNCiAgInBhdGgiOiAiLyIsDQogICJ0bHMiOiAiIiwNCiAgInNuaSI6ICIiLA0KICAiYWxwbiI6ICIiLA0KICAiZnAiOiAiIg0KfQ==")]
    public void TestVmess(string content)
    {
        var model = Vmess.Parse(content);
        Assert.NotNull(model);
    }

    [Theory]
    [InlineData("ss://Y2hhY2hhMjAtaWV0Zi1wb2x5MTMwNTo2YjJlMDYwMi1iYzFhLTRlYzMtODhkYS00MjM4YTY5NTVkZGQ@c11.twtc.dynu.net:3234#github.com/freefq%20-%20%E5%8F%B0%E6%B9%BE%E7%9C%81%E4%B8%AD%E5%8D%8E%E7%94%B5%E4%BF%A1%28HiNet%29%E6%95%B0%E6%8D%AE%E4%B8%AD%E5%BF%83%2011")]
    public void TestShadowsocks(string content)
    {
        var model = Shadowsocks.Parse(content);
        Assert.NotNull(model);
    }

    [Theory]
    [InlineData("trojan://5bae27f5-3b8e-48f3-b91f-30fc680ea78f@103.154.63.16:443#%E4%BA%9A%E5%A4%AA%E5%9C%B0%E5%8C%BA+V2CROSS.COM")]
    [InlineData("trojan://3e5d1046-5807-4b79-8397-31a006f546d1@xibaozi.19890604.day:10848?security=tls&sni=cloudflare.node-ssl.cdn-alibaba.com&type=tcp&headerType=none#%E5%B9%BF%E4%B8%9C%E7%9C%81%E5%B9%BF%E5%B7%9E%E5%B8%82%2B%E7%A7%BB%E5%8A%A8")]
    public void TestTrojan(string content)
    {
        var model = Trojan.Parse(content);
        Assert.NotNull(model);
    }

    [Theory]
    [InlineData("https://bulinkbulink.com/freefq/free/master/v2")]
    public async void TestSetXrayNodeByUrl(string url)
    {
        var xrayservice = GetService<IXrayNodeService>();
        await xrayservice.SetXrayNodeByUrl(url);
    }

    [Theory]
    [InlineData("c3M6Ly9ZMmhoWTJoaE1qQXRhV1YwWmkxd2IyeDVNVE13TlRvMllqSmxNRFl3TWkxaVl6RmhMVFJsWXpNdE9EaGtZUzAwTWpNNFlUWTVOVFZrWkdRQGMxMS50d3RjLmR5bnUubmV0OjMyMzQjZ2l0aHViLmNvbS9mcmVlZnElMjAtJTIwJUU1JThGJUIwJUU2JUI5JUJFJUU3JTlDJTgxJUU0JUI4JUFEJUU1JThEJThFJUU3JTk0JUI1JUU0JUJGJUExJTI4SGlOZXQlMjklRTYlOTUlQjAlRTYlOEQlQUUlRTQlQjglQUQlRTUlQkYlODMlMjAxMQp0cm9qYW46Ly8zZTVkMTA0Ni01ODA3LTRiNzktODM5Ny0zMWEwMDZmNTQ2ZDFAeGliYW96aS4xOTg5MDYwNC5kYXk6MTA4NDg/c2VjdXJpdHk9dGxzJnNuaT1jbG91ZGZsYXJlLm5vZGUtc3NsLmNkbi1hbGliYWJhLmNvbSZ0eXBlPXRjcCZoZWFkZXJUeXBlPW5vbmUjJUU1JUI5JUJGJUU0JUI4JTlDJUU3JTlDJTgxJUU1JUI5JUJGJUU1JUI3JTlFJUU1JUI4JTgyJTJCJUU3JUE3JUJCJUU1JThBJUE4")]
    public void TestSetXrayNodeByString(string text)
    {
        var xrayservice = GetService<IXrayNodeService>();
        xrayservice.SetXrayNodeByBase64String(text);
    }

    [Fact]
    public void TestSaveXrayNodeByList()
    {
        var list = new List<string>
        {
            "ss://Y2hhY2hhMjAtaWV0Zi1wb2x5MTMwNTo2YjJlMDYwMi1iYzFhLTRlYzMtODhkYS00MjM4YTY5NTVkZGQ@c11.twtc.dynu.net:3234#github.com/freefq%20-%20%E5%8F%B0%E6%B9%BE%E7%9C%81%E4%B8%AD%E5%8D%8E%E7%94%B5%E4%BF%A1%28HiNet%29%E6%95%B0%E6%8D%AE%E4%B8%AD%E5%BF%83%2011",
            "trojan://3e5d1046-5807-4b79-8397-31a006f546d1@xibaozi.19890604.day:10848?security=tls&sni=cloudflare.node-ssl.cdn-alibaba.com&type=tcp&headerType=none#%E5%B9%BF%E4%B8%9C%E7%9C%81%E5%B9%BF%E5%B7%9E%E5%B8%82%2B%E7%A7%BB%E5%8A%A8",
            "trojan://telegram-id-directvpn@3.73.238.119:22222?security=tls&sni=trojan.miwan.co.uk&alpn=http%2F1.1&type=tcp&headerType=none#%E7%BE%8E%E5%9B%BD%2BAmazon%2BEC2%E6%9C%8D%E5%8A%A1%E5%99%A8"
        };
        var xrayservice = GetService<IXrayNodeService>();
        xrayservice.SaveXrayNodeByList(list);
    }

    [Theory]
    [InlineData("Y2hhY2hhMjAtaWV0Zi1wb2x5MTMwNTo2YjJlMDYwMi1iYzFhLTRlYzMtODhkYS00MjM4YTY5NTVkZGQ")]
    public void TestBase64Decode(string text)
    {
        var str = XrayUtils.Base64Decode(text);
        Assert.NotNull(str);
    }
}