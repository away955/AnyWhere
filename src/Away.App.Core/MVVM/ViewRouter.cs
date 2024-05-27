namespace Away.App.Core.MVVM;

/// <summary>
/// 菜单路由
/// </summary>
public static class ViewRouter
{
    public static List<string> Routes { get; private set; } = [];
    public static string CurrentRoute
    {
        get
        {
            if (Routes.Count == 0)
            {
                return string.Empty;
            }
            return Routes[CurrentRouteIndex];
        }
    }
    public static int CurrentRouteIndex { get; private set; } = 0;

    public static bool HasBack()
    {
        return CurrentRouteIndex < Routes.Count - 1 && Routes.Count > 1;
    }
    public static bool HasNext()
    {
        return CurrentRouteIndex > 0;
    }

    public static void Back()
    {
        CurrentRouteIndex += 1;
        Nav(CurrentRoute);
    }

    public static void Next()
    {
        if (CurrentRouteIndex == 0)
        {
            return;
        }
        CurrentRouteIndex -= 1;
        Nav(CurrentRoute);
    }

    public static void Go(string path, object? parameter = null, string? contract = null)
    {
        if (string.IsNullOrWhiteSpace(path) || path == CurrentRoute)
        {
            return;
        }
        Routes = Routes[CurrentRouteIndex..];
        Routes.Insert(0, path);
        CurrentRouteIndex = 0;
        Nav(path, parameter, contract);
    }

    public static void Listen(Action<ViewParameter> action, string? contract = null)
    {
        MessageBus.Current.Subscribe(MessageBusType.Routes, o => action((ViewParameter)o), contract);
    }
    private static void Nav(string path, object? parameter = null, string? contract = null)
    {
        var data = new ViewParameter { Path = path, Parameter = parameter };
        MessageBus.Current.Publish(MessageBusType.Routes, data, contract);
    }
}

public class ViewParameter
{
    /// <summary>
    /// 页面地址
    /// </summary>
    public required string Path { get; set; }
    /// <summary>
    /// 页面参数
    /// </summary>
    public object? Parameter { get; set; }
}