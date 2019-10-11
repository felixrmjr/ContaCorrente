using WR.Modelo.Util;

namespace WR.Modelo.Domain.Entities.Base
{
    public class UsuarioIdentity
    {
        public int Id { get; }
        public string Email { get; }
        public long ClientId { get; }
        public int ClientCodigo { get; }
        public int UsuarioId { get; set; }
        public string Login { get; set; }
        public int? ClienteId { get; set; }

        public UsuarioIdentity(int id, string email, string login)
        {
            Throw.IfLessThanOrEqZero(id);
            Throw.IfIsNullOrWhiteSpace(email);

            this.Id = id;
            this.Email = email;
            this.ClientId = id;
            this.ClientCodigo = 1;
            this.UsuarioId = id;
            this.Login = login;
        }
    }
}
