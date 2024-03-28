using System.IO.Pipes;

namespace Away.App.Core.IPC;

public sealed class IPCServer : IDisposable
{
    public NamedPipeServerStream PipeServer { get; }

    public event Action<WindowStateCommandType>? OnReceive;

    public IPCServer(string pipeName)
    {
        PipeServer = new(pipeName);
    }

    public async Task Listen()
    {
        Log.Information("IPC Started");
        Log.Information("Wait for Client connect");
        await PipeServer.WaitForConnectionAsync();
        Log.Information("Client connectd");
        await Accept();
    }

    private async Task Accept()
    {
        var buffer = new byte[1];
        var len = await PipeServer.ReadAsync(buffer);
        if (len == 0)
        {
            return;
        }
        var cmd = (WindowStateCommandType)buffer[0];
        OnReceive?.Invoke(cmd);
        await Task.Delay(500);
        await CommandAsync(WindowStateCommandType.Close);
    }

    private async Task CommandAsync(WindowStateCommandType cmd)
    {
        var buffer = new byte[] { (byte)cmd };
        await PipeServer.WriteAsync(buffer);
    }

    public void Dispose()
    {
        PipeServer?.Dispose();
    }
}
