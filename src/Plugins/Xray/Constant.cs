namespace Xray;

public sealed class Constant
{
    public const string PluginName = "Xray";
    public static string RootPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", PluginName);
}
