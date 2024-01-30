namespace Away.Service.DB.Repositories.Impl;

public abstract class RepositoryBase<T>(ISugerDbContext db) : SimpleClient<T>(db), ISimpleClient<T> where T : class, new()
{
}