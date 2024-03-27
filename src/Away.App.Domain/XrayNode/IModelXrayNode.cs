using Away.App.Domain.XrayNode.Entities;

namespace Away.Domain.XrayNode;

public interface IModelXrayNode
{
    XrayNodeEntity ToEntity();

    XrayOutbound ToXrayOutbound();
}
