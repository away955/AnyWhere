namespace Away.App;

public sealed class Constant
{
    public const string DBKey = "App";
    public const string PluginRegisterServiceKey = "PluginRegister";
    public static string RootPath => AppDomain.CurrentDomain.BaseDirectory;
    public static string DBConn => $"DataSource={Path.Combine(RootPath, "app.sqlite")}";
    public static string PluginsRootPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

    public const string Version = "v1.2.5";
    public const string Title = "哪都通";

    public const string Host = "https://proxy.v2gh.com/https://raw.githubusercontent.com/away955/anywhere/master/dist/latest";
    public const string AppInfoUrl = $"{Host}/info.md";

    public const string AppUpdateInfoUrl = $"{Host}/update.md";
    public const string AppUpdateDownloadUrl = $"{Host}/windows-away-update.zip";

}
