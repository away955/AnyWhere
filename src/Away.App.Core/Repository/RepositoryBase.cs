namespace Away.App.Core.Repository;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private IFileContext _context;
    public RepositoryBase(IFileContext context)
    {
        _context = context;
        Items = GetList();
    }

    public List<T> Items { get; set; }

    public List<T> GetList() => _context.AsQueryable<T>();

    public void Add(T entity)
    {
        Items.Add(entity);
    }

    public void Delete(T entity)
    {
        Items.Remove(entity);
    }

    public void Save()
    {
        _context.Save(Items);
    }
}
