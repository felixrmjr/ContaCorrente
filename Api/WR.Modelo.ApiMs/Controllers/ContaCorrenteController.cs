using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WR.Modelo.Api.Helpers.Response;
using WR.Modelo.Domain.Entities;
using WR.Modelo.ApiMs.ViewModels;
using WR.Modelo.Domain.Entities.Base;
using WR.Modelo.Domain.Interfaces.Applications;
using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Repository.Contexts;
using WR.Modelo.Domain.Enums;

namespace WR.Modelo.ApiMs.Controllers
{
    public class ContaCorrenteController : ControllerBase<WRContext, ContaCorrente, ContaCorrenteVm, int>
    {
        private readonly IContaCorrenteApplication<WRContext> _contaCorrenteApplication;
        private readonly ILancamentosApplication<WRContext> _lancamentosApplication;
        private readonly IUsuarioBase _usuarioBase;

        public ContaCorrenteController(IContaCorrenteApplication<WRContext> contaCorrenteApplication,
                                       ILancamentosApplication<WRContext> lancamentosApplication,
                                       IUsuarioBase usuarioBase) : base(contaCorrenteApplication, usuarioBase)
        {
            _contaCorrenteApplication = contaCorrenteApplication;
            _lancamentosApplication = lancamentosApplication;
            _usuarioBase = usuarioBase;
        }

        public override Task<IActionResult> Get([ModelBinder] QueryFilter filter, bool paginado = true)
        {
            var dados = _contaCorrenteApplication.GetPaginated(filter, filter.Start, filter.Limit);
            var viewModel = Mapper.Map<IList<ContaCorrenteVm>>(dados.Data);
            return Task.FromResult<IActionResult>(Ok(new Result<IList<ContaCorrenteVm>>(viewModel, dados.Total)));
        }

        [HttpPost("criar")]
        public Task<IActionResult> CriarContaCorrente()
        {
            var entidade = _contaCorrenteApplication.CriarContaCorrente();
            return Task.FromResult<IActionResult>(Ok(Mapper.Map<ContaCorrenteVm>(entidade)));
        }

        [HttpPost("transferencia")]
        public Task<IActionResult> Transferencia([FromBody] LancamentosVm model)
        {
            model.Tipo = TipoLancamento.Transferencia;
            var entidade = _lancamentosApplication.AdicionarTransacao(model.Model());
            return Task.FromResult<IActionResult>(Ok(Mapper.Map<LancamentosVm>(entidade)));
        }

        [HttpPost("credito")]
        public Task<IActionResult> Credito([FromBody] LancamentosVm model)
        {
            model.Tipo = TipoLancamento.Credito;
            var entidade = _lancamentosApplication.AdicionarTransacao(model.Model());
            return Task.FromResult<IActionResult>(Ok(Mapper.Map<LancamentosVm>(entidade)));
        }

        [HttpPost("debito")]
        public Task<IActionResult> Debito([FromBody] LancamentosVm model)
        {
            model.Tipo = TipoLancamento.Debito;
            var entidade = _lancamentosApplication.AdicionarTransacao(model.Model());
            return Task.FromResult<IActionResult>(Ok(Mapper.Map<LancamentosVm>(entidade)));
        }
    }
}
