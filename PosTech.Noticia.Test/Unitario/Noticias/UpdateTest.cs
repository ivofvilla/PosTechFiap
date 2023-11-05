using FluentValidation;
using Moq;
using PosTech.Noticia.API.Noticias.Delete;
using PosTech.Noticia.API.Noticias;
using PosTech.Noticia.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PosTech.Noticia.API.Noticias.Update;
using Xunit;
using FluentValidation.Results;

namespace PosTech.Noticia.Test.Unitario.Noticias
{
    public class UpdateTest
    {
        private UpdateNoticiaHandler _updateNoticiaHandler;
        private Mock<INoticiaRepository> _noticiaRepositoryMock;
        private Mock<IValidator<UpdateNoticiaCommand>> _validatorMock;
        private CancellationToken _cancellationToken = new CancellationToken();

        public UpdateTest()
        {
            _validatorMock = new Mock<IValidator<UpdateNoticiaCommand>>();
            _noticiaRepositoryMock = new Mock<INoticiaRepository>();
            _updateNoticiaHandler = new UpdateNoticiaHandler(_noticiaRepositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommandAndUpdateSuccess_ReturnsTrue()
        {
            // Arrange
            var command = new UpdateNoticiaCommand
            {
                Titulo = "Novo Título",
                Descricao = "Nova Descrição",
                Chapeu = "Novo Chapeu",
                Autor = "Novo Autor"
            };

            command.SetId(Guid.NewGuid());

            _validatorMock.Setup(s => s.ValidateAsync(command, _cancellationToken)).ReturnsAsync(new ValidationResult());
            _noticiaRepositoryMock.Setup(r => r.ObterPorIdAsync(command.Id, _cancellationToken)).ReturnsAsync(new API.Models.Noticias());
            _noticiaRepositoryMock.Setup(r => r.AtualizarAsync(It.IsAny<API.Models.Noticias>(), _cancellationToken)).Returns(Task.CompletedTask);

            // Act
            var result = await _updateNoticiaHandler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ReturnsFalse()
        {
            // Arrange
            var command = new UpdateNoticiaCommand
            {
                Titulo = "Novo Título",
                Descricao = "Nova Descrição",
                Chapeu = "Novo Chapeu",
                Autor = "Novo Autor"
            };

            _validatorMock.Setup(s => s.ValidateAsync(command, _cancellationToken)).ReturnsAsync(new ValidationResult { Errors = { new ValidationFailure("Id", "O Id da noticia é obrigatório.") } });

            // Act
            var result = await _updateNoticiaHandler.Handle(command, _cancellationToken);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_NotFoundNoticia_ReturnsFalse()
        {
            // Arrange
            var command = new UpdateNoticiaCommand
            {
                Titulo = "Novo Título",
                Descricao = "Nova Descrição",
                Chapeu = "Novo Chapeu",
                Autor = "Novo Autor"
            };

            command.SetId(Guid.NewGuid());

            _validatorMock.Setup(s => s.ValidateAsync(command, _cancellationToken)).ReturnsAsync(new ValidationResult());
            _noticiaRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<API.Models.Noticias>(null));

            // Act
            var result = await _updateNoticiaHandler.Handle(command, _cancellationToken);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_UpdateFailure_ReturnsFalse()
        {
            // Arrange
            var command = new UpdateNoticiaCommand
            {
                Titulo = "Novo Título",
                Descricao = "Nova Descrição",
                Chapeu = "Novo Chapeu",
                Autor = "Novo Autor"
            };

            command.SetId(Guid.NewGuid());

            _validatorMock.Setup(s => s.ValidateAsync(command, _cancellationToken)).ReturnsAsync(new ValidationResult());
            _noticiaRepositoryMock.Setup(r => r.ObterPorIdAsync(command.Id, _cancellationToken)).ReturnsAsync(new API.Models.Noticias());
            _noticiaRepositoryMock.Setup(r => r.AtualizarAsync(It.IsAny<API.Models.Noticias>(), _cancellationToken)).Throws(new Exception("Failed to update"));

            Exception caughtException = null;

            // Act
            try
            {
                var result = await _updateNoticiaHandler.Handle(command, _cancellationToken);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.False(caughtException is null);
        }
    }
}
