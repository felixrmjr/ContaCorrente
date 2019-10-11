using System;
using System.Collections.Generic;
using WR.Modelo.Domain.Entities.Base;

namespace WR.Modelo.Domain.Entities
{
    public class ContaCorrente : EntityBase<int>, ICloneable
    {
        #region [ Propriedades ]

        public int Numero { get; private set; }
        public int UsuarioId { get; private set; }
        public decimal Saldo { get; private set; }

        public Usuario Usuario { get; private set; }
        public IReadOnlyCollection<Lancamentos> Lancamentos { get; private set; }

        #endregion

        #region [ Contrutores ]

        public ContaCorrente()
        {
            AlterarSaldo(0);
            Ativar();
        }

        public ContaCorrente(int usuarioId)
        {
            AlterarUsuario(usuarioId);
        }

        public ContaCorrente(int numero, int usuarioId) : base()
        {
            AlterarNumero(numero);
            AlterarUsuario(usuarioId);
        }

        public ContaCorrente(int id, int numero, int usuarioId, decimal saldo, bool ativo)
        {
            AtualizarId(id);
            AlterarNumero(numero);
            AlterarUsuario(usuarioId);
            AlterarSaldo(saldo);
            AtualizarAtivo(ativo);
        }

        #endregion

        #region [ Métodos ]

        public void AlterarUsuario(int usuarioId)
        {
            if (usuarioId <= 0)
                AddException(nameof(ContaCorrente), nameof(AlterarUsuario), "campoObrigatorio", nameof(usuarioId));

            Numero = usuarioId;
        }

        public void AlterarNumero(int numero)
        {
            if (numero <= 0)
                AddException(nameof(ContaCorrente), nameof(AlterarNumero), "campoObrigatorio", nameof(numero));

            Numero = numero;
        }

        public void AlterarSaldo(decimal saldo) => this.Saldo = saldo;

        public void GerarNumeroConta()
        {
            var soma = 0;
            var resto = 0;
            var multiplicador1 = new int[5] { 10, 9, 8, 7, 6 };
            var multiplicador2 = new int[6] { 11, 10, 9, 8, 7, 6 };

            var random = new Random();
            string semente = random.Next(10000, 99999).ToString();

            for (int i = 0; i < 5; i++)
                soma += int.Parse(semente[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            resto = (resto < 2) ? 0 : (11 - resto);

            semente = semente + resto;
            soma = 0;

            for (int i = 0; i < 6; i++)
                soma += int.Parse(semente[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = (resto < 2) ? 0 : (11 - resto);

            semente = semente + resto;
            this.Numero = Convert.ToInt32(semente);
        }

        public void AdicionarDebito(decimal valor) => this.Saldo -= valor;

        public void AdicionarCredito(decimal valor) => this.Saldo += valor;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
