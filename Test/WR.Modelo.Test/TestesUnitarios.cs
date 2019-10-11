using Moq;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Enums;
using WR.Modelo.Domain.Interfaces.Applications;
using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Repository.Contexts;
using Xunit;

namespace WR.Modelo.Test
{
    public class TestesUnitarios
    {
        private readonly Mock<IUnitOfWork<WRContext>> _mockUnitOfWork = new Mock<IUnitOfWork<WRContext>>();
        private readonly Mock<IContaCorrenteApplication<WRContext>> _mockContaCorrenteApplication = new Mock<IContaCorrenteApplication<WRContext>>();
        private readonly Mock<ILancamentosApplication<WRContext>> _mockLancamentosApplication = new Mock<ILancamentosApplication<WRContext>>();

        [Fact]
        public void TestarCriarContaCorrente()
        {
            var contaCorrente = new ContaCorrente(1);
            _mockContaCorrenteApplication.Setup(s => s.CriarContaCorrente()).Returns(contaCorrente);
            Assert.True(contaCorrente.Saldo.Equals(0));
        }

        [Fact]
        public void TestarTransferencia()
        {
            var lancamento = new Lancamentos(4966519, TipoLancamento.Transferencia, 300, 1234567);
            _mockLancamentosApplication.Setup(s => s.AdicionarTransacao(lancamento)).Returns(lancamento);
            Assert.True(lancamento.Valor.Equals(300));
        }

        [Fact]
        public void TestarDebito()
        {
            var lancamento = new Lancamentos(4966519, TipoLancamento.Debito, 500);
            _mockLancamentosApplication.Setup(s => s.AdicionarTransacao(lancamento)).Returns(lancamento);
            Assert.True(lancamento.Valor.Equals(500));
        }

        [Fact]
        public void TestarCredito()
        {
            var lancamento = new Lancamentos(4966519, TipoLancamento.Credito, 1000, 1234567);
            _mockLancamentosApplication.Setup(s => s.AdicionarTransacao(lancamento)).Returns(lancamento);
            Assert.True(lancamento.Valor.Equals(1000));
        }
    }
}
