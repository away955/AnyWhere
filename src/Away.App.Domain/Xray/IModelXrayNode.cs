using Away.App.Domain.Xray.Entities;

namespace Away.App.Domain.Xray;

public interface IModelXrayNode
{
    XrayNodeEntity ToEntity();

    XrayOutbound ToXrayOutbound();
}
