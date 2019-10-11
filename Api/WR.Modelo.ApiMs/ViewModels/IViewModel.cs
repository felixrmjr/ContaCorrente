namespace WR.Modelo.Api.Helpers
{
    public interface IViewModel<out T> where T : class
    {
        T Model();
    }
}
