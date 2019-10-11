using System;

namespace WR.Modelo.Domain.Entities.Base
{
    public abstract class EntityBase<TIdentity> : ValidatableObject
    {
        public virtual TIdentity Id { get; protected set; }
        public virtual long UsuarioCriacaoId { get; protected set; }
        public virtual DateTime DataCriacao { get; protected set; }
        public virtual long? UsuarioAlteracaoId { get; protected set; }
        public virtual DateTime? DataAlteracao { get; protected set; }
        public virtual bool Ativo { get; protected set; }

        #region [ Metados ] 

        public virtual void AtualizarId(TIdentity id)
        {
            if (id == null)
                AddException(nameof(EntityBase<TIdentity>), nameof(this.Id), "campoObrigatorio", "id");

            if (string.IsNullOrEmpty(id.ToString()))
                AddException(nameof(EntityBase<TIdentity>), nameof(this.Id), "campoObrigatorio", "id");

            this.Id = id;
        }
        public virtual void AtualizarUsuarioCriacao(long usuarioid)
        {
            if (usuarioid <= 0)
                AddException(nameof(EntityBase<TIdentity>), nameof(this.UsuarioCriacaoId), "campoObrigatorioId", "usuario");

            this.UsuarioCriacaoId = usuarioid;
        }

        public virtual void AtualizarUsuarioAlteracao(long? usuarioid)
        {
            if (usuarioid <= 0)
                AddException(nameof(EntityBase<TIdentity>), nameof(this.UsuarioAlteracaoId), "campoObrigatorioId", "usuario");
            this.UsuarioAlteracaoId = usuarioid;
        }

        public virtual void AtualizarDataCriacao(DateTime? data = null) => this.DataCriacao = data.HasValue ? data.GetValueOrDefault() : DateTime.Now;

        public virtual void AtualizarDataAlteracao(DateTime? data = null) => this.DataAlteracao = data.HasValue ? data.GetValueOrDefault() : DateTime.Now;

        public virtual void AtualizarAtivo(bool ativo) => this.Ativo = ativo;

        public virtual void Ativar() => this.Ativo = true;

        public virtual void Inativar() => this.Ativo = false;

        #endregion
    }
}
