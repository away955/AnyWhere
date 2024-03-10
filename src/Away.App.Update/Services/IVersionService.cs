using Away.App.Update.Services.Impl;
using System.Threading.Tasks;

namespace Away.App.Update.Services;

public interface IVersionService
{
    Task<VersionInfo> GetVersionInfo();
}
