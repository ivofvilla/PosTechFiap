using Moq;
using PosTech.Noticia.API.Noticias.Delete;
using PosTech.Noticia.API.Repositories;
using PosTech_Fase.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PosTech.Noticia.Test.Unitario.Noticias
{
    public class GetAllTest
    {
        private GetAllHandle _getAllHandle;
        private Mock<INoticiaRepository> _noticiaRepositoryMock;
        private CancellationToken _cancellationToken = new CancellationToken();

        public GetAllTest()
        {
            _noticiaRepositoryMock = new Mock<INoticiaRepository>();
            _getAllHandle = new GetAllHandle(_noticiaRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsAllNoticias()
        {
            // Arrange
            var noticias = new List<API.Models.Noticias>()
            {
                new API.Models.Noticias { Id = Guid.NewGuid(), Titulo = "Notícia 1", Descricao = "Descrição 1" },
                new API.Models.Noticias { Id = Guid.NewGuid(), Titulo = "Notícia 2", Descricao = "Descrição 2" },
            };

            _noticiaRepositoryMock.Setup(r => r.ObterTodosAsync(_cancellationToken)).ReturnsAsync(noticias);

            // Act
            var result = await _getAllHandle.Handle(new GetAllQuery(), _cancellationToken);

            // Assert
            Assert.NotNull(result.Noticias);
            Assert.Equal(noticias.Count, result.Noticias.Count());
        }

        [Fact]
        public async Task Handle_NoNoticias_ReturnsEmptyList()
        {
            // Arrange
            _noticiaRepositoryMock.Setup(r => r.ObterTodosAsync(_cancellationToken)).ReturnsAsync(new List<API.Models.Noticias>());

            // Act
            var result = await _getAllHandle.Handle(new GetAllQuery(), _cancellationToken);

            // Assert
            Assert.NotNull(result.Noticias);
            Assert.Empty(result.Noticias);
        }

        [Fact]
        public async Task Handle_RepositoryError_ReturnsEmptyList()
        {
            // Arrange
            _noticiaRepositoryMock.Setup(r => r.ObterTodosAsync(_cancellationToken)).Throws(new Exception("Repository error"));

            Exception caughtException = null;

            // Act
            try
            {
                var result = await _getAllHandle.Handle(new GetAllQuery(), _cancellationToken);
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
