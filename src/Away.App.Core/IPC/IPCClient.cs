using Away.App.Core.MessageBus;
using System.IO.Pipes;

namespace Away.App.Core.IPC;

public sealed class IPCClient : IDisposable
{
    public NamedPipeClientStream PipeClient { get; }
    public event Action<WindowStateCommandType>? OnReceive;

    public IPCClient(string serverName, string pipeName)
    {
        PipeClient = new(serverName, pipeName);
    }

    public void Close()
    {
        PipeClient.Close();
    }
    public void Connect(TimeSpan timeout)
    {
        PipeClient.Connect(timeout);
    }

    public async Task CommandAsync(WindowStateCommandType cmd)
    {
        if (!PipeClient.IsConnected)
        {
            Log.Warning($"IPC Client Not Connect");
        }
        var buffer = new byte[] { (byte)cmd };
        await PipeClient.WriteAsync(buffer);
        await Accept();
    }

    private async Task Accept()
    {
        var buffer = new byte[1];
        var len = await PipeClient.ReadAsync(buffer);
        if (len == 0)
        {
            return;
        }
        var cmd = (WindowStateCommandType)buffer[0];
        OnReceive?.Invoke(cmd);
    }

    public void Dispose()
    {
        PipeClient.Dispose();
    }
}
