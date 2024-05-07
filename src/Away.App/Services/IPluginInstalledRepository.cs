using Away.App.Services.Entities;
using System.Collections.Generic;

namespace Away.App.Services;

/// <summary>
/// 已安装的插件仓储
/// </summary>
public interface IPluginInstalledRepository
{
    List<PluginInstalledEntity> GetList();
    List<PluginInstalledEntity> GetListByNotDisabled();
    bool Save(PluginInstalledEntity entity);
    bool Delete(string module);
}
