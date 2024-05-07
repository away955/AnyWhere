namespace Youtube;

public sealed class Constant
{
    public const string PluginName = "Youtube";
    public static string RootPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", PluginName);

    public const string DBKey = PluginName;
    public static string DBConn => $"DataSource={Path.Combine(RootPath, "Data", $"{PluginName}.sqlite")}";
}
