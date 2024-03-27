using SqlSugar;

namespace Away.App.Core.Database.Impl;

public abstract class RepositoryBase<T> : SimpleClient<T> where T : class, new()
{
    public RepositoryBase(ISugarDbContext db)
    {
        Context = db;
        db.CodeFirst.InitTables<T>();
    }
}
