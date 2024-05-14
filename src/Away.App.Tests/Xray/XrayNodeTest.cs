using Xray.Services;
using Xray.Services.Models;

namespace Away.App.Tests;

public sealed class XrayNodeTest : TestBase
{
    [InlineData("vless://6646bf7a-4757-3e04-b730-489ff1c61997@to5sy84s.cc:10086?encryption=none\u0026type=tcp\u0026security=\u0026path=%2F\u0026headerType=none#%E5%A4%87%E7%94%A8%E7%BD%91%E5%9D%80%EF%BC%9Ahttps%3A%2F%2Fto5sy84s.cc")]
    [Theory]
    public void TestVless(string url)
    {
        var model = Vless.Parse(url);
        Assert.NotNull(model);
    }

    [Theory]
    [InlineData("ssr://c3NyMS5zc3JzdWIuY29tOjQyNDcxOm9yaWdpbjphZXMtMjU2LWNmYjpwbGFpbjphSFIwY0hNNkx5OWtiR291ZEdZdmMzTnljM1ZpLz9vYmZzcGFyYW09NUx1WTZMUzVVMU5TNXJPbzVZYU1PbWgwZEhBNkx5OWtiR291ZEdZdmMzTnljM1ZpJnByb3RvcGFyYW09ZEM1dFpTOVRVMUpUVlVJJnJlbWFya3M9VTFQb2lvTG5ncm5vcnFMcG1JWGxuTERsbllBNmFIUjBjSE02THk5eVlYY3VaMmwwYUhWaWRYTmxjbU52Ym5SbGJuUXVZMjl0TDNOemNuTjFZaTl6YzNJdmJXRnpkR1Z5TDNOekxYTjFZZyZncm91cD1kQzV0WlM5VFUxSlRWVUk")]
    [InlineData("ssr://c3NyMi5zc3JzdWIuY29tOjM4MTQ5Om9yaWdpbjphZXMtMjU2LWNmYjpwbGFpbjpjM1Z2TG5sMEwzTnpjbk4xWWcvP29iZnNwYXJhbT01THVZNkxTNVUxTlM1ck9vNVlhTU9taDBkSEE2THk5a2JHb3VkR1l2YzNOeWMzVmkmcHJvdG9wYXJhbT1kQzV0WlM5VFUxSlRWVUkmcmVtYXJrcz1Wakp5WVhub3JxTHBtSVhsbkxEbG5ZQTZhSFIwY0hNNkx5OXlZWGN1WjJsMGFIVmlkWE5sY21OdmJuUmxiblF1WTI5dEwzTnpjbk4xWWk5emMzSXZiV0Z6ZEdWeUwxWXlVbUY1Jmdyb3VwPWRDNXRaUzlUVTFKVFZVSQ")]
    [InlineData("ssr://Y2Ruc3NyLnNzcnN1Yi5jb206NDQzOm9yaWdpbjphZXMtMjU2LWNmYjpwbGFpbjpZMlJ1YzNOeUxuTnpjbk4xWWk1amIyMC8_b2Jmc3BhcmFtPTVMdVk2TFM1VTFOUzVyT281WWFNT21oMGRIQTZMeTlrYkdvdWRHWXZjM055YzNWaSZwcm90b3BhcmFtPWRDNXRaUzlUVTFKVFZVSSZyZW1hcmtzPVUxTlM1YTY1NXBpVDZLS3I2Wmk3NXBhdElPaV92ZWF4Z3Vlb3MtV3VtdWl2dC1hYnRPYU5vbE5UTDFZeVVtRjVMMVJ5YjJwaGJ1aXVvdW1ZaFEmZ3JvdXA9ZEM1dFpTOVRVMUpUVlVJ\r")]
    public void TestSSR(string url)
    {
        var model = ShadowsocksR.Parse(url);
        Assert.NotNull(model);
    }

    [Theory]
    [InlineData("vmess://ew0KICAidiI6ICIyIiwNCiAgInBzIjogIue+juWbvSBDbG91ZEZsYXJl6IqC54K5IiwNCiAgImFkZCI6ICJ3d3cuY3Jpc3B5cmFpbmJvdy5iaXoiLA0KICAicG9ydCI6ICI4MDgwIiwNCiAgImlkIjogIjIyODI2YjQ0LTVjMWEtNGI0Yi1kYmFhLTgzYTJlOGJkOTVmMCIsDQogICJhaWQiOiAiMCIsDQogICJzY3kiOiAiYXV0byIsDQogICJuZXQiOiAid3MiLA0KICAidHlwZSI6ICJub25lIiwNCiAgImhvc3QiOiAiIiwNCiAgInBhdGgiOiAiLyIsDQogICJ0bHMiOiAiIiwNCiAgInNuaSI6ICIiLA0KICAiYWxwbiI6ICIiLA0KICAiZnAiOiAiIg0KfQ==")]
    public void TestVmess(string content)
    {
        var model = Vmess.Parse(content);
        Assert.NotNull(model);
    }

    [Theory]
    [InlineData("ss://Y2hhY2hhMjAtaWV0Zi1wb2x5MTMwNTozYlpDbnMydGE5S2dDbnV4@eagle03.t3o5wy.xyz:30032/?group=VG9saW5r#Tolink%20-%20%F0%9F%87%BA%F0%9F%87%B8%E7%BE%8E%E5%9B%BD%20%7C%20103%20%7C%20%E4%B8%93%E7%BA%BF%7C%201x")]
    [InlineData("ss://Y2hhY2hhMjAtaWV0Zi1wb2x5MTMwNTo2YjJlMDYwMi1iYzFhLTRlYzMtODhkYS00MjM4YTY5NTVkZGQ@c11.twtc.dynu.net:3234#github.com/freefq%20-%20%E5%8F%B0%E6%B9%BE%E7%9C%81%E4%B8%AD%E5%8D%8E%E7%94%B5%E4%BF%A1%28HiNet%29%E6%95%B0%E6%8D%AE%E4%B8%AD%E5%BF%83%2011")]
    [InlineData("ss://Y2hhY2hhMjAtaWV0Zi1wb2x5MTMwNTp5NW04SUNidVNQeVlDbGdI@eagle02.t3o5wy.xyz:30031/?group=VG9saW5r#Tolink%20-%20%F0%9F%87%BA%F0%9F%87%B8%E7%BE%8E%E5%9B%BD%20%7C%20102%20%7C%20%E4%B8%93%E7%BA%BF%7C%201x")]
    public void TestSS(string content)
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
    [InlineData("Y2hhY2hhMjAtaWV0Zi1wb2x5MTMwNTo2YjJlMDYwMi1iYzFhLTRlYzMtODhkYS00MjM4YTY5NTVkZGQ")]
    public void TestBase64Decode(string text)
    {
        var str = XrayUtils.Base64Decode(text);
        Assert.NotNull(str);
    }
}
