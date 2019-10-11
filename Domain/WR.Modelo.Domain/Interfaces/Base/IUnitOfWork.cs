namespace WR.Modelo.Domain.Interfaces.Base
{
    public interface IUnitOfWork<TContext>
    {
        int Commit();
    }
}