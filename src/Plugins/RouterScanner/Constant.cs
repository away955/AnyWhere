namespace RouterScanner;

public sealed class Constant
{
    public const string PluginName = "RouterScanner";
    public const string VulHubKey = "VulHubKey";
    public static string RootPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", PluginName);
}
