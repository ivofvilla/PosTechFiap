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
using Xunit;
using FluentValidation.Results;
using PosTech.Noticia.API.Models;
using System.Threading;

namespace PosTech.Noticia.Test.Unitario.Noticias
{
    public class DeleteTest
    {
        private DeleteNoticiaHandle _deleteNoticiaHandle;
        private Mock<INoticiaRepository> _noticiaRepositoryMock;
        private Mock<IValidator<DeleteNoticiaCommand>> _validatorMock;
        private CancellationToken _cancellationToken = new CancellationToken();


        public DeleteTest()
        {
            _validatorMock = new Mock<IValidator<DeleteNoticiaCommand>>();
            _noticiaRepositoryMock = new Mock<INoticiaRepository>();
            _deleteNoticiaHandle = new DeleteNoticiaHandle(_noticiaRepositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommandAndRemoveSuccess_ReturnsTrue()
        {
            // Arrange
            var command = new DeleteNoticiaCommand { Id = Guid.NewGuid() };

            _validatorMock.Setup(s => s.ValidateAsync(command, _cancellationToken)).ReturnsAsync(new ValidationResult());
            _noticiaRepositoryMock.Setup(r => r.ObterPorIdAsync(command.Id, _cancellationToken)).ReturnsAsync(new API.Models.Noticias());
            _noticiaRepositoryMock.Setup(r => r.RemoverAsync(It.IsAny<API.Models.Noticias>(), _cancellationToken)).Returns(Task.CompletedTask);

            // Act
            var result = await _deleteNoticiaHandle.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ReturnsFalse()
        {
            // Arrange
            var command = new DeleteNoticiaCommand { Id = Guid.Empty }; // ID inválido

            _validatorMock.Setup(s => s.ValidateAsync(command, _cancellationToken)).ReturnsAsync(new ValidationResult { Errors = { new ValidationFailure("Id", "O ID da noticia é obrigatório.") } });

            // Act
            var result = await _deleteNoticiaHandle.Handle(command, _cancellationToken);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_NotFoundNoticia_ReturnsFalse()
        {
            // Arrange
            var command = new DeleteNoticiaCommand { Id = Guid.NewGuid() };

            _validatorMock.Setup(s => s.ValidateAsync(command, _cancellationToken)).ReturnsAsync(new ValidationResult());
            _noticiaRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<API.Models.Noticias>(null));

            // Act
            var result = await _deleteNoticiaHandle.Handle(command, _cancellationToken);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_RemoveFailure_ReturnsFalses()
        {
            // Arrange
            var command = new DeleteNoticiaCommand { Id = Guid.NewGuid() };

            _validatorMock.Setup(s => s.ValidateAsync(command, _cancellationToken)).ReturnsAsync(new ValidationResult());
            _noticiaRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>(), _cancellationToken)).ReturnsAsync(new API.Models.Noticias());
            _noticiaRepositoryMock.Setup(r => r.RemoverAsync(It.IsAny<API.Models.Noticias>(), _cancellationToken)).Throws(new Exception("Failed to remove"));

            Exception caughtException = null;

            // Act
            try
            {
                var result = await _deleteNoticiaHandle.Handle(command, _cancellationToken);
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
