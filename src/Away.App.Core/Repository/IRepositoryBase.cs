namespace Away.App.Core.Repository;

public interface IRepositoryBase<T>
{
    List<T> GetList();
    void Delete(T entity);
    void Add(T entity);
    void Save();
}
