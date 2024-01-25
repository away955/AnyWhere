using Away.Service.DB.Impl;

namespace Away.Service.DB.Repositories.Impl;

public sealed class MenuRepository(ISugerDbContext db) : RepositoryBase<MenuEntity>(db), IMenuRepository
{
}
