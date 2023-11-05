using Moq;
using FluentValidation;
using PosTech.Noticia.API.Commands.Noticia;
using PosTech.Noticia.API.Models;
using PosTech.Noticia.API.Repositories;
using Xunit;
using FluentValidation.Results;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations;

namespace PosTech.Noticia.Test.Unitario.Noticias
{
    public class CreateTest
    {
        private readonly CreateNoticiaHandler _createNoticiaCommand;
        private readonly Mock<INoticiaRepository> _noticiaRepositoryMock;
        private readonly Mock<IValidator<CreateNoticiaCommand>> _validatorMock;
        private readonly CancellationToken _cancellationToken = new CancellationToken();
        private readonly CreateNoticiaValidator _validator;

        public CreateTest()
        {
            _validatorMock = new Mock<IValidator<CreateNoticiaCommand>>();
            _noticiaRepositoryMock = new Mock<INoticiaRepository>();
            _createNoticiaCommand = new CreateNoticiaHandler(_noticiaRepositoryMock.Object, _validatorMock.Object);
            _validator = new CreateNoticiaValidator();
        }

        [Fact]
        public async Task Handle_ValidCommandAndAddSuccess_ReturnsTrue()
        {
            // Arrange
            var command = new CreateNoticiaCommand
            {
                Titulo = "Not�cia de Teste",
                Descricao = "Descri��o de Teste",
                Chapeu = "Chapeu de Teste",
                Autor = "Autor de Teste"
            };

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<CreateNoticiaCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _noticiaRepositoryMock.Setup(r => r.AdicionarAsync(It.IsAny<API.Models.Noticias>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

            // Act
            var result = await _createNoticiaCommand.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Validate_EmptyAutor_ValidationErrors()
        {
            var command = new CreateNoticiaCommand
            {
                Titulo = "T�tulo V�lido",
                Descricao = "Descri��o V�lida",
                Chapeu = "Chap�u V�lido",
                Autor = ""
            };

            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Equal("A Autor da noticia � obrigat�ria.", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Validate_ValidCommand_NoValidationErrors()
        {
            var command = new CreateNoticiaCommand
            {
                Titulo = "T�tulo V�lido",
                Descricao = "Descri��o V�lida",
                Chapeu = "Chap�u V�lido",
                Autor = "Autor V�lido"
            };

            var result = _validator.Validate(command);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_EmptyTitulo_ValidationErrors()
        {
            var command = new CreateNoticiaCommand
            {
                Titulo = "",
                Descricao = "Descri��o V�lida",
                Chapeu = "Chap�u V�lido",
                Autor = "Autor V�lido"
            };

            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Equal("O t�tulo da noticia � obrigat�rio.", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Validate_EmptyDescricao_ValidationErrors()
        {
            var command = new CreateNoticiaCommand
            {
                Titulo = "T�tulo V�lido",
                Descricao = "",
                Chapeu = "Chap�u V�lido",
                Autor = "Autor V�lido"
            };

            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Equal("A Descri��o da noticia � obrigat�ria.", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Validate_EmptyChapeu_ValidationErrors()
        {
            var command = new CreateNoticiaCommand
            {
                Titulo = "T�tulo V�lido",
                Descricao = "Descri��o V�lida",
                Chapeu = "",
                Autor = "Autor V�lido"
            };

            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Equal("A Ch�peu da noticia � obrigat�ria.", result.Errors[0].ErrorMessage);
        }
    }
}