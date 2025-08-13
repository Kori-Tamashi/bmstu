namespace Eventor.Database.Repositories;

public class BaseRepository
{
    protected readonly string RepositoryName;

    public BaseRepository()
    {
        RepositoryName = GetType().Name;
    }
}