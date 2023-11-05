using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PosTech.Noticia.API.Models;
using PosTech.Noticia.API.Repositories;
using PosTech_Fase.Queries.Login;
using PosTech_Fase.Queries.Usuario;
using System.Security.Claims;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PosTech.Noticia.Test.Unitario.Usuario
{
    public class GetUsuarioTest
    {
        private ObterAlbumPorIdQueryHandler _queryHandler;
        private Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private Mock<IValidator<GetUsuarioQuery>> _validatorMock;
        private Mock<IConfiguration> _configurationMock;
        private CancellationToken _cancellationToken = new CancellationToken();

        public GetUsuarioTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            _validatorMock = new Mock<IValidator<GetUsuarioQuery>>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();

            _configurationMock.Setup(c => c["AppSettings:Secret"]).Returns("JUSTTOJbaOzaUAK7pAKyteTiR#5pd&WRF7%1I*M*M22IIEee%qE*OYyOmhDXdk^BcO0%pCoKCKq4v@iq7!*MmV*QVc!gi!N8z8StN9%XKPERFEITO");
            _configurationMock.Setup(c => c["AppSettings:Emissor"]).Returns("2");
            _configurationMock.Setup(c => c["AppSettings:ValidoEm"]).Returns("32");
            _configurationMock.Setup(c => c["AppSettings:ExpiracaoHoras"]).Returns("1");

            _queryHandler = new ObterAlbumPorIdQueryHandler(_usuarioRepositoryMock.Object, _validatorMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task Handle_ValidQueryAndAuthentication_Success_ReturnsToken()
        {

            // Arrange
            var query = new GetUsuarioQuery
            {
                Email = "test@example.com",
                Password = "TestPassword",
            };

            _validatorMock.Setup(v => v.ValidateAsync(query, _cancellationToken)).ReturnsAsync(new ValidationResult());
            _usuarioRepositoryMock.Setup(s => s.ObterLogin(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Usuarios("Teste@mail.com", "Zq1(", "Usuario teste"));

            // Act
            var result = await _queryHandler.Handle(query, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Token);
        }

        [Fact]
        public async Task Handle_InvalidQuery_ReturnsNull()
        {
            // Arrange
            var query = new GetUsuarioQuery
            {
                // Missing required fields
            };

            _validatorMock.Setup(v => v.ValidateAsync(query, _cancellationToken)).ReturnsAsync(new ValidationResult { Errors = { new ValidationFailure("Email", "O Email é um campo obrigatório.") } });
            

            // Act
            var result = await _queryHandler.Handle(query, _cancellationToken);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_AuthenticationFailed_ReturnsNull()
        {
            // Arrange
            var query = new GetUsuarioQuery
            {
                Email = "test@example.com",
                Password = "TestPassword"
            };

            _validatorMock.Setup(v => v.ValidateAsync(query, _cancellationToken)).ReturnsAsync(new ValidationResult());

            // Act
            var result = await _queryHandler.Handle(query, _cancellationToken);

            // Assert
            Assert.Null(result);
        }
    }
}
