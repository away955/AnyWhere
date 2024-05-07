using CliWrap;
using CliWrap.Buffered;

namespace Xray.Services.Impl;

public sealed class LinuxProxySetting : IProxySetting
{
    private const string Profile = "away_anywhere.sh";
    public string ProxyServer { get; set; } = string.Empty;
    public string ProxyOverride { get; set; } = string.Empty;
    public bool ProxyEnable { get; set; }

    public async void GetProxy()
    {
        var text = await V2rayProfile($"cat {Profile}");
        var items = text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        if (items.Length != 2)
        {
            return;
        }
        var serverResult = Regex.Match(items[0], "export all_proxy=\"http://(?<server>.*)\"");
        if (serverResult.Success)
        {
            ProxyServer = serverResult.Result("${server}");
        }
        var overrideResult = Regex.Match(items[0], "export no_proxy=\"(?<override>.*)\"");
        if (overrideResult.Success)
        {
            ProxyServer = overrideResult.Result("${override}");
        }
    }

    public bool Save()
    {
        var cmd = $"rm -f ./{Profile}";
        if (ProxyEnable)
        {
            var allProxy = $"export all_proxy=\"http://{ProxyServer}\"";
            var notProxy = $"export no_proxy=\"{ProxyOverride.Replace(";", ",")}\"";
            cmd = $"echo '{allProxy}\r\n{notProxy}'>{Profile}";
        }
        V2rayProfile(cmd).Wait();
        return true;
    }

    private static async Task<string> V2rayProfile(string args)
    {
        var cli = Cli.Wrap(string.Empty)
            .WithWorkingDirectory("/etc/profile.d")
            .WithArguments(args);
        var result = await cli.ExecuteBufferedAsync();
        return result.StandardOutput;
    }
}
