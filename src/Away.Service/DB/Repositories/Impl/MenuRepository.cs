namespace Away.Service.DB.Repositories.Impl;

[ServiceInject]
public sealed class MenuRepository(ISugerDbContext db) : RepositoryBase<MenuEntity>(db), IMenuRepository
{
}
