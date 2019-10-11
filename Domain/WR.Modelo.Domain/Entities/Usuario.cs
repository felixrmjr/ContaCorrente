using WR.Modelo.Domain.Entities.Base;

namespace WR.Modelo.Domain.Entities
{
    public sealed class Usuario : EntityBase<int>
    {
        #region [ Propriedades ]

        public string Nome { get; private set; }
        public string Login { get; private set; }
        public string Senha { get; private set; }

        public ContaCorrente ContaCorrente { get; private set; }

        #endregion
    }
}
