using SqlSugar;

namespace Away.App.Core.Database;

public interface IRepositoryBase<T> : ISimpleClient<T> where T : class, new()
{
}