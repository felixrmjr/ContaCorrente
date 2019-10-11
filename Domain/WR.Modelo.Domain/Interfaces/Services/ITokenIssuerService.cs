using System.Threading.Tasks;
using WR.Modelo.Domain.Entities;

namespace WR.Modelo.Domain.Interfaces.Services
{
    public interface ITokenIssuerService
    {
        Task<string> GenerateToken(Usuario usuario);
    }
}
