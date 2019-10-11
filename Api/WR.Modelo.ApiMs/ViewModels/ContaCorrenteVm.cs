using Microsoft.Extensions.Localization;
using WR.Modelo.Api.Helpers;
using WR.Modelo.IoC;
using WR.Modelo.Domain.Entities;
using System.Collections.Generic;

namespace WR.Modelo.ApiMs.ViewModels
{
    public class ContaCorrenteVm : IViewModel<ContaCorrente>
    {
        private readonly IStringLocalizer _localizer = ServiceLocator.Resolve<IStringLocalizer>();

        public int Id { get; set; }
        public int Numero { get; set; }
        public int UsuarioId { get; set; }
        public decimal Saldo { get; set; }
        public bool Ativo { get; set; }

        public IReadOnlyCollection<Lancamentos> Lancamentos { get; private set; }

        public ContaCorrente Model()
        {
            var entity = new ContaCorrente(Id, Numero, UsuarioId, Saldo, Ativo);
            return entity;

        }
    }
}