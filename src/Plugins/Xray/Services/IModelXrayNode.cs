namespace Xray.Services;

public interface IModelXrayNode
{
    XrayNodeEntity ToEntity();

    XrayOutbound ToXrayOutbound();
}
