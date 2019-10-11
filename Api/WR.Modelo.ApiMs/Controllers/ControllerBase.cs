using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WR.Modelo.Api.Helpers.Response;
using WR.Modelo.IoC;
using WR.Modelo.Util;
using WR.Modelo.Api.Helpers;
using WR.Modelo.Domain.Entities.Base;
using WR.Modelo.Domain.Interfaces.Repositories;
using WR.Modelo.Domain.Interfaces.Base;

namespace WR.Modelo.ApiMs.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Route("{language:regex(^[[a-z]]{{2}}(?:-[[A-Z]]{{2}})?$)}/api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ControllerBase<TContext, TTEntity, TViewModel, TIdentity> : Controller
                                                              where TContext : IUnitOfWork<TContext>
                                                              where TTEntity : EntityBase<TIdentity>
                                                              where TViewModel : class, IViewModel<TTEntity>
    {
        protected readonly IApplicationBase<TContext, TTEntity, TIdentity> _application;
        protected readonly IStringLocalizer _localizer = ServiceLocator.Resolve<IStringLocalizer>();

        private readonly IUsuarioBase _usuarioLogado;

        public ControllerBase(IApplicationBase<TContext, TTEntity, TIdentity> application, IUsuarioBase usuarioLogado)
        {
            this._usuarioLogado = usuarioLogado;
            this._application = application;
        }

        #region Web API 

        [HttpGet]
        public virtual Task<IActionResult> Get([ModelBinder]QueryFilter filter, bool paginado = true)
        {
            if (!paginado)
                return Task.FromResult<IActionResult>(Ok(Mapper.Map<IList<TViewModel>>(_application.GetAll(filter))));

            var dados = _application.GetPaginated(filter, filter.Start, filter.Limit);
            var viewModel = Mapper.Map<IList<TViewModel>>(dados.Data);
            return Task.FromResult<IActionResult>(Ok(new Result<IList<TViewModel>>(viewModel, dados.Total)));
        }

        [HttpGet("{id}")]
        public virtual Task<IActionResult> Get(TIdentity id)
        {
            Throw.IfIsNull(id, this._localizer["campoNulo", nameof(id)]);
            var result = Mapper.Map<TViewModel>(_application.Get(id));
            return Task.FromResult<IActionResult>(Ok(result));
        }

        [HttpPost]
        public virtual Task<IActionResult> Post([FromBody] TViewModel model)
        {
            Throw.IfIsNull(model, this._localizer["campoNulo", nameof(model)]);

            var result = Mapper.Map<TViewModel>(_application.Save(model.Model()));
            return Task.FromResult<IActionResult>(Ok(result));
        }

        [HttpPut("{id}")]
        public virtual Task<IActionResult> Put(long id, [FromBody] TViewModel model)
        {
            Throw.IfIsNull(id, this._localizer["campoNulo", nameof(id)]);
            Throw.IfLessThanOrEqZero(id, this._localizer["menorOuIgual", nameof(id), "zero"]);
            Throw.IfIsNull(model, this._localizer["campoNulo", nameof(model)]);

            var result = Mapper.Map<TViewModel>(_application.Update(model.Model()));
            return Task.FromResult<IActionResult>(Ok(result));
        }

        [HttpDelete("{id}")]
        public virtual Task<IActionResult> Delete(TIdentity id)
        {
            Throw.IfIsNull(id, this._localizer["campoNulo", nameof(id)]);
            _application.Delete(id);
            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpPut("Ativar/{id}")]
        public virtual Task<IActionResult> Ativar(TIdentity id)
        {
            Throw.IfIsNull(id, this._localizer["campoNulo", nameof(id)]);
            _application.Ativar(id);
            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpPut("Inativar/{id}")]
        public virtual Task<IActionResult> Inativar(TIdentity id)
        {
            Throw.IfIsNull(id, this._localizer["campoNulo", nameof(id)]);
            _application.Inativar(id);
            return Task.FromResult<IActionResult>(Ok());
        }


        #endregion

        #region Lookups

        protected readonly List<Expression<Func<TTEntity, object>>> ListaLookup = new List<Expression<Func<TTEntity, object>>>();

        protected Expression<Func<TTEntity, ResultLookup<TIdentity>>> SelectJoin;
        protected Expression<Func<TTEntity, ResultLookup<TIdentity>>> _selecaoLookup => ObterExpressaoLambidaParaExibicao();

        [HttpGet]
        [Route("Lookup")]
        public virtual Task<IActionResult> Lookup([ModelBinder] QueryFilter filtro)
        {
            var pesquisa = filtro.Filters.SingleOrDefault(x => x.Property == "pesquisa");
            filtro.Filters.Remove(pesquisa);

            if (pesquisa != null)
                foreach (var item in ListaLookup)
                    filtro.AddFilter(item.GetMemberName(), pesquisa.Value.ToString().ToUpper(), false);

            filtro.AddFilter("ativo", true);
            var dados = _application.GetPaginated(filtro, filtro.Start, filtro.Limit == 0 ? 20 : filtro.Limit);
            var result = dados.Data.AsQueryable().Select(SelectJoin ?? _selecaoLookup).ToList();
            return Task.FromResult<IActionResult>(Ok(new Result<IList<ResultLookup<TIdentity>>>(result, dados.Total)));
        }
        protected virtual Expression<Func<TTEntity, ResultLookup<TIdentity>>> ObterExpressaoLambidaParaExibicao()
        {
            var source = typeof(TTEntity);
            var target = typeof(ResultLookup<TIdentity>);

            var t = Expression.Parameter(source, "t");
            MemberExpression sourceName;
            MemberExpression sourceId;

            if (ListaLookup.Count > 1)
            {
                sourceId = Expression.MakeMemberAccess(t, source.GetProperty(ListaLookup[0].GetMemberName()));
                sourceName = Expression.MakeMemberAccess(t, source.GetProperty((ListaLookup[1].GetMemberName())));
            }
            else
            {
                sourceId = Expression.MakeMemberAccess(t, source.GetProperty(ListaLookup[0].GetMemberName()));
                sourceName = sourceId;
            }


            var assignValue = Expression.Bind(target.GetProperty("id"), sourceId);
            var assignName = Expression.Bind(target.GetProperty("label"), sourceName);

            var targetNew = Expression.New(target);
            var init = Expression.MemberInit(targetNew, assignName, assignValue);

            var lambda = (Expression<Func<TTEntity, ResultLookup<TIdentity>>>)Expression.Lambda(init, t);
            return lambda;
        }
        protected void AdicionarItemLookup(params Expression<Func<TTEntity, object>>[] itens)
        {
            foreach (var item in itens)
                ListaLookup.Add(item);
        }


        #endregion
    }
}
