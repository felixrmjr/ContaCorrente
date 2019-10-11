using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Repository.Base;

namespace WR.Modelo.Repository.Maps
{
    public class LancamentosMap : BaseEntityMap<Lancamentos, int>
    {
        protected override void ConfigurarMapeamento(EntityTypeBuilder<Lancamentos> builder)
        {
            builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.ContaOrigem).IsRequired();
            builder.Property(e => e.ContaDestino);
            builder.Property(e => e.Tipo).HasColumnName("TipoLancamento").IsRequired();
            builder.Property(e => e.Valor).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(e => e.Data).IsRequired().HasDefaultValue(DateTime.Now);

            builder.Ignore(x => x.Ativo);
            builder.Ignore(x => x.DataCriacao);
            builder.Ignore(x => x.UsuarioCriacaoId);
            builder.Ignore(x => x.DataAlteracao);
            builder.Ignore(x => x.UsuarioAlteracaoId);

            builder.HasOne(l => l.ContaCorrente)
                   .WithMany(c => c.Lancamentos)
                   .HasForeignKey(d => d.ContaOrigem)
                   .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
