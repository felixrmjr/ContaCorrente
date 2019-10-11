using System;
using WR.Modelo.Domain.Entities.Base;
using WR.Modelo.Domain.Enums;

namespace WR.Modelo.Domain.Entities
{
    public class Lancamentos : EntityBase<int>, ICloneable
    {
        #region [ Propriedades ]

        public int ContaOrigem { get; private set; }
        public int? ContaDestino { get; private set; }
        public TipoLancamento Tipo { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime Data { get; private set; }

        public ContaCorrente ContaCorrente { get; private set; }

        #endregion

        #region [ Contrutores ]

        public Lancamentos(int contaOrigem, TipoLancamento tipo, decimal valor, int? contaDestino = null)
        {
            AlterarContaOrigem(contaOrigem);
            AlterarTipoLancamento(tipo);
            AlterarValor(valor);
            AlterarContaDestino(contaDestino);
            Data = DateTime.Now;
        }

        #endregion

        #region [ Métodos ]

        public void AlterarContaOrigem(int contaOrigem)
        {
            if (contaOrigem <= 0)
                AddException(nameof(Lancamentos), nameof(AlterarContaOrigem), "campoObrigatorio", nameof(contaOrigem));

            ContaOrigem = contaOrigem;
        }

        public void AlterarContaDestino(int? contaDestino)
        {
            if (contaDestino <= 0)
                AddException(nameof(Lancamentos), nameof(AlterarContaOrigem), "campoObrigatorio", nameof(contaDestino));

            ContaDestino = contaDestino;
        }

        public void AlterarTipoLancamento(TipoLancamento tipo) => Tipo = tipo;

        public void AlterarValor(decimal valor)
        {
            if (valor <= 0)
                AddException(nameof(Lancamentos), nameof(AlterarValor), "campoObrigatorio", nameof(valor));

            Valor = valor;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
