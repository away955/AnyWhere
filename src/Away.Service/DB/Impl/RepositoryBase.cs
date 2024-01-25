namespace Away.Service.DB.Impl;

public abstract class RepositoryBase<T> : SimpleClient<T> where T : class, new()
{
    public RepositoryBase(ISugerDbContext db)
    {
        Context = db;
    }
}

