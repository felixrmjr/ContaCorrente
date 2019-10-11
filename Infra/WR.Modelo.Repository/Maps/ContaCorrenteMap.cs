using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Repository.Base;

namespace WR.Modelo.Repository.Maps
{
    public class ContaCorrenteMap : BaseEntityMap<ContaCorrente, int>
    {
        protected override void ConfigurarMapeamento(EntityTypeBuilder<ContaCorrente> builder)
        {
            builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.UsuarioId).IsRequired();
            builder.Property(e => e.Numero).IsRequired();
            builder.Property(e => e.Saldo).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(e => e.Ativo).IsRequired();

            builder.Ignore(x => x.DataCriacao);
            builder.Ignore(x => x.UsuarioCriacaoId);
            builder.Ignore(x => x.DataAlteracao);
            builder.Ignore(x => x.UsuarioAlteracaoId);

            builder.HasOne(c => c.Usuario)
                   .WithOne(u => u.ContaCorrente)
                   .HasForeignKey<ContaCorrente>( d=> d.UsuarioId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
