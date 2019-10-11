using System;
using System.Collections.Generic;
using System.Text;
using WR.Modelo.Api.Helpers;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Enums;

namespace WR.Modelo.ApiMs.ViewModels
{
    public class LancamentosVm : IViewModel<Lancamentos>
    {
        public int ContaOrigem { get; set; }
        public int? ContaDestino { get; set; }
        public TipoLancamento Tipo { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }

        public Lancamentos Model()
        {
            var entity = new Lancamentos(ContaOrigem, Tipo, Valor, ContaDestino);
            return entity;
        }
    }
}
