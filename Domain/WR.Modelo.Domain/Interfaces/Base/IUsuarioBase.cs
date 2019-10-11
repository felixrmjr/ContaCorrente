using WR.Modelo.Domain.Entities.Base;

namespace WR.Modelo.Domain.Interfaces.Base
{
    public interface IUsuarioBase
    {
        UsuarioIdentity UsuarioLogado { get; }
        string Idioma { get; }
    }
}