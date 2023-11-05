using FluentValidation;
using FluentValidation.Results;
using Moq;
using PosTech.Noticia.API.Repositories;
using PosTech_Fase.Queries;
using PosTech_Fase2.Application.Queries.Noticias.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PosTech.Noticia.Test.Unitario.Noticias
{
    public class GetByIdTest
    {
        private GetNoticiaByIdHandle _getNoticiaByIdHandle;
        private Mock<INoticiaRepository> _noticiaRepositoryMock;
        private Mock<IValidator<GetNoticiaByIdQuery>> _validatorMock;
        private CancellationToken _cancellationToken = new CancellationToken();

        public GetByIdTest()
        {
            _validatorMock = new Mock<IValidator<GetNoticiaByIdQuery>>();
            _noticiaRepositoryMock = new Mock<INoticiaRepository>();
            _getNoticiaByIdHandle = new GetNoticiaByIdHandle(_noticiaRepositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_ValidQueryAndNoticiaFound_ReturnsNoticia()
        {
            // Arrange
            var noticiaId = Guid.NewGuid();
            var query = new GetNoticiaByIdQuery { Id = noticiaId };
            var noticia = new API.Models.Noticias { Id = noticiaId, Titulo = "Notícia de Teste" };

            _validatorMock.Setup(v => v.ValidateAsync(query, _cancellationToken)).ReturnsAsync(new ValidationResult());
            _noticiaRepositoryMock.Setup(r => r.ObterPorIdAsync(noticiaId, _cancellationToken)).ReturnsAsync(noticia);

            // Act
            var result = await _getNoticiaByIdHandle.Handle(query, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(noticiaId, result.Id);
        }

        [Fact]
        public async Task Handle_InvalidQuery_ReturnsNull()
        {
            // Arrange
            var query = new GetNoticiaByIdQuery { Id = Guid.Empty }; // ID inválido

            _validatorMock.Setup(v => v.ValidateAsync(query, _cancellationToken)).ReturnsAsync(new ValidationResult { Errors = { new ValidationFailure("Id", "O Id da noticia é obrigatório.") } });

            // Act
            var result = await _getNoticiaByIdHandle.Handle(query, _cancellationToken);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_NoticiaNotFound_ReturnsNull()
        {
            // Arrange
            var noticiaId = Guid.NewGuid();
            var query = new GetNoticiaByIdQuery { Id = noticiaId };

            _validatorMock.Setup(v => v.ValidateAsync(query, _cancellationToken)).ReturnsAsync(new ValidationResult());
            _noticiaRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<API.Models.Noticias>(null));

            // Act
            var result = await _getNoticiaByIdHandle.Handle(query, _cancellationToken);

            // Assert
            Assert.Null(result);
        }

    }
}
