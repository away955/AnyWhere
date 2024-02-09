namespace Away.Service.DB.Repositories.Impl;

[ServiceInject]
public class XrayNodeSubRepository(ISugerDbContext db) : RepositoryBase<XrayNodeSubEntity>(db), IXrayNodeSubRepository
{
}
