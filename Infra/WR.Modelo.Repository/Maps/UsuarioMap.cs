using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Repository.Base;

namespace WR.Modelo.Repository.Maps
{
    public class UsuarioMap : BaseEntityMap<Usuario, int>
    {
        protected override void ConfigurarMapeamento(EntityTypeBuilder<Usuario> builder)
        {
            builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.Nome).HasColumnType("varchar(256)").IsRequired();
            builder.Property(e => e.Login).HasColumnType("varchar(64)").IsRequired();
            builder.Property(e => e.Senha).HasColumnType("varchar(256)").IsRequired();
            builder.Property(e => e.Ativo).IsRequired();

            builder.Ignore(x => x.DataCriacao);
            builder.Ignore(x => x.UsuarioCriacaoId);
            builder.Ignore(x => x.DataAlteracao);
            builder.Ignore(x => x.UsuarioAlteracaoId);

       
        }
    }
}
