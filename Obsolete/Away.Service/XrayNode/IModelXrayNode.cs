using Away.Service.Xray.Model;

namespace Away.Service.XrayNode;

public interface IModelXrayNode
{
    XrayNodeEntity ToEntity();

    XrayOutbound ToXrayOutbound();
}
