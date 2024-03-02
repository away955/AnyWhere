using Away.Domain.Xray.Model;

namespace Away.Domain.XrayNode;

public interface IModelXrayNode
{
    XrayNodeEntity ToEntity();

    XrayOutbound ToXrayOutbound();
}
