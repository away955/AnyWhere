namespace Away.App.Services.IPC;

public static class OnlyProcess
{
    private static string PipeName { get; set; } = "Default";

    public static void Listen(string pipeName)
    {
        PipeName = pipeName;
        CheckOnlyProcess();
        _ = IPCListen(PipeName);
    }

    private static async void CheckOnlyProcess()
    {
        try
        {
            using IPCClient ipcClient = new(".", PipeName);
            ipcClient.OnReceive += (cmd) =>
            {
                Environment.Exit(0);
            };
            ipcClient.Connect(TimeSpan.FromSeconds(1));
            await ipcClient.CommandAsync(WindowStateCommandType.ShowActivate);
        }
        catch
        {
            Log.Information("程序未运行，正在启动...");
        }
    }

    private static async Task IPCListen(string pipeName)
    {
        try
        {
            while (true)
            {
                using IPCServer ipcServer = new(pipeName);
                ipcServer.OnReceive += (cmd) => MessageWindowState.State(cmd);
                await ipcServer.Listen();
            }
        }
        catch (Exception ex)
        {
            Log.Information(ex, "IPC Server Starting Error");
        }
    }
}
