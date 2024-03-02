namespace Away.Service.DB;

public interface IRepositoryBase<T> : ISimpleClient<T> where T : class, new()
{
}
