using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PosTech.Noticia.API.Models;
using PosTech.Noticia.API.Repositories;
using PosTech.Usuario.API.Commands;
using Xunit;

namespace PosTech.Noticia.Test.Unitario.Usuario
{
    public class CreateTest
    {
        private CreateUsurarioHandler _createUsurarioHandler;
        private Mock<IValidator<CreateUsurarioCommand>> _validatorMock;
        private Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private CancellationToken _cancellationToken = new CancellationToken();

        public CreateTest()
        {
            _validatorMock = new Mock<IValidator<CreateUsurarioCommand>>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _createUsurarioHandler = new CreateUsurarioHandler(_validatorMock.Object, _usuarioRepositoryMock.Object);
            _usuarioRepositoryMock.Setup(s => s.Inserir(It.IsAny<Usuarios>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Usuarios("", "", ""));
        }

        [Fact]
        public async Task Handle_ValidCommandAndUserCreation_Success_ReturnsTrue()
        {
            // Arrange
            var command = new CreateUsurarioCommand
            {
                Email = "test@example.com",
                Senha = "TestPassword",
                ConfirmacaoSenha = "TestPassword",
                Nome = "teste nome"
            };

            _validatorMock.Setup(v => v.ValidateAsync(command, _cancellationToken)).ReturnsAsync(new ValidationResult());
            
            // Act
            var result = await _createUsurarioHandler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ReturnsFalse()
        {
            // Arrange
            var command = new CreateUsurarioCommand
            {
            };

            _validatorMock.Setup(v => v.ValidateAsync(command, _cancellationToken)).ReturnsAsync(new ValidationResult { Errors = { new ValidationFailure("Email", "O Email é um campo obrigatório.") } });

            // Act
            var result = await _createUsurarioHandler.Handle(command, _cancellationToken);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_UserCreationFailed_ReturnsFalse()
        {
            // Arrange
            var command = new CreateUsurarioCommand
            {
                Email = "test@example.com",
                Senha = "TestPassword",
                ConfirmacaoSenha = "TestPassword"
            };

            Usuarios? usuario = null;

            _validatorMock.Setup(v => v.ValidateAsync(command, _cancellationToken)).ReturnsAsync(new ValidationResult());
            _usuarioRepositoryMock.Setup(s => s.Inserir(It.IsAny<Usuarios>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);

            // Act
            var result = await _createUsurarioHandler.Handle(command, _cancellationToken);

            // Assert
            Assert.False(result);
        }
    }
}
