namespace Away.App.Core.Messages;

/// <summary>
/// 菜单路由
/// </summary>
public static class MessageRouter
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

    public static void Go(string url, string? contract = null)
    {
        if (string.IsNullOrWhiteSpace(url) || url == CurrentRoute)
        {
            return;
        }
        Routes = Routes[CurrentRouteIndex..];
        Routes.Insert(0, url);
        CurrentRouteIndex = 0;
        Nav(url, contract);
    }

    public static void Listen(Action<object> action, string? contract = null)
    {
        MessageBus.Current.Subscribe(MessageBusType.Routes, action, contract);
    }
    private static void Nav(string url, string? contract = null)
    {
        MessageBus.Current.Publish(MessageBusType.Routes, url, contract);
    }
}
