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
                Titulo = "Notícia de Teste",
                Descricao = "Descrição de Teste",
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
                Titulo = "Título Válido",
                Descricao = "Descrição Válida",
                Chapeu = "Chapéu Válido",
                Autor = ""
            };

            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Equal("A Autor da noticia é obrigatória.", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Validate_ValidCommand_NoValidationErrors()
        {
            var command = new CreateNoticiaCommand
            {
                Titulo = "Título Válido",
                Descricao = "Descrição Válida",
                Chapeu = "Chapéu Válido",
                Autor = "Autor Válido"
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
                Descricao = "Descrição Válida",
                Chapeu = "Chapéu Válido",
                Autor = "Autor Válido"
            };

            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Equal("O título da noticia é obrigatório.", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Validate_EmptyDescricao_ValidationErrors()
        {
            var command = new CreateNoticiaCommand
            {
                Titulo = "Título Válido",
                Descricao = "",
                Chapeu = "Chapéu Válido",
                Autor = "Autor Válido"
            };

            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Equal("A Descrição da noticia é obrigatória.", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Validate_EmptyChapeu_ValidationErrors()
        {
            var command = new CreateNoticiaCommand
            {
                Titulo = "Título Válido",
                Descricao = "Descrição Válida",
                Chapeu = "",
                Autor = "Autor Válido"
            };

            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Equal("A Chápeu da noticia é obrigatória.", result.Errors[0].ErrorMessage);
        }
    }
}