using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WR.Modelo.Domain.Entities.Base;

namespace WR.Modelo.Repository.Base
{
    public abstract class BaseEntityMap<TEntity, TKey> : IEntityTypeConfiguration<TEntity> where TKey 
                                                       : struct where TEntity 
                                                       : EntityBase<TKey>
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Ativo)
                   .IsRequired();

            builder.Property(x => x.UsuarioCriacaoId)
                   .IsRequired();

            builder.Property(x => x.DataCriacao)
                   .IsRequired();

            builder.Property(x => x.UsuarioAlteracaoId);
            builder.Property(x => x.DataAlteracao);

            ConfigurarMapeamento(builder);
        }

        protected abstract void ConfigurarMapeamento(EntityTypeBuilder<TEntity> builder);
    }
}
