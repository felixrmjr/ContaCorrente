using Microsoft.EntityFrameworkCore;
using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Repository.Maps;

namespace WR.Modelo.Repository.Contexts
{
    public class WRContext : DbContext, IUnitOfWork<WRContext>
    {
        public WRContext(DbContextOptions<WRContext> options) : base(options) { }

        public int Commit() => SaveChanges();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ContaCorrenteMap());
            modelBuilder.ApplyConfiguration(new LancamentosMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
        }
    }
}