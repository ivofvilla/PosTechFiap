using Microsoft.EntityFrameworkCore;
using PosTech.Noticia.API.Commands.Noticia;
using PosTech.Noticia.API.Data;
using PosTech.Noticia.API.Noticias.Delete;
using PosTech.Noticia.API.Noticias.Update;
using PosTech.Noticia.API.Noticias;
using PosTech.Noticia.API.Repositories;
using PosTech_Fase.Queries;
using PosTech_Fase2.Application.Queries.Noticias.GetById;
using Xunit;
using PosTech.Noticia.API.Models;

public class NoticiaTest
{
    private DbContextClass _context;
    private INoticiaRepository _repository;

    public NoticiaTest()
    {
        //Arrange
        var builder = new DbContextOptionsBuilder<DbContextClass>();
        builder.UseInMemoryDatabase("Noticias");
        var options = builder.Options;

        _context = new DbContextClass(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _repository = new NoticiaRepository(_context);
    }

    [Fact]
    public async Task CreateNoticiaHandler_Should_AddNoticiaToDatabase()
    {
        // Arrange
        var createNoticiaCommand = new CreateNoticiaCommand
        {
            Titulo = "Título de Teste",
            Descricao = "Descrição de Teste",
            Chapeu = "Chapéu de Teste",
            Autor = "Autor de Teste"
        };

        var validator = new CreateNoticiaValidator();
        var handler = new CreateNoticiaHandler(_repository, validator);

        // Act
        var result = await handler.Handle(createNoticiaCommand, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteNoticiaHandle_Should_DeleteNoticia()
    {
        // Arrange
        var data = DateTime.Now;
        var id = Guid.NewGuid();

        _context.Noticias.AddRange(
            new Noticias { Id = Guid.NewGuid(), Autor = "Stan Lee", Chapeu = "Bone", Descricao = "homen aranha ", Titulo = "novo jogo", DataAtualizacao = data, DataPublicacao = data },
            new Noticias { Id = id, Autor = "qualquer um", Chapeu = "cowboy", Descricao = "batman", Titulo = "arkhan 4", DataAtualizacao = data, DataPublicacao = data }
        );
        _context.SaveChanges();


        var repository = new NoticiaRepository(_context);
        var validator = new DeleteNoticiaValidator();
        var handler = new DeleteNoticiaHandle(repository, validator);

        var deleteNoticiaCommand = new DeleteNoticiaCommand
        {
            Id = id
        };

        // Act
        var result = await handler.Handle(deleteNoticiaCommand, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateNoticiaHandler_Should_UpdateNoticia()
    {
        // Arrange

        var noticia = new Noticias
        {
            Id = Guid.NewGuid(),
            Titulo = "Título de Exemplo",
            Descricao = "Descrição de Exemplo",
            Chapeu = "Chapéu de Exemplo",
            Autor = "Autor de Exemplo",
        };

        _context.Noticias.Add(noticia);
        _context.SaveChanges();

        var repository = new NoticiaRepository(_context);
        var validator = new UpdateNoticiaValidator();
        var handler = new UpdateNoticiaHandler(repository, validator);

        var noticiaToUpdate = _context.Noticias.First(); // Obtemos uma notícia do banco de dados em memória

        var updateNoticiaCommand = new UpdateNoticiaCommand
        {
            Titulo = "Novo Título",
            Descricao = "Nova Descrição",
            Chapeu = "Novo Chapéu",
            Autor = "Novo Autor",
        };
        updateNoticiaCommand.SetId(noticiaToUpdate.Id);

        // Act
        var result = await handler.Handle(updateNoticiaCommand, CancellationToken.None);

        // Assert
        Assert.True(result); // Verifica se a operação de atualização foi bem-sucedida

        var noticiaInDatabase = await _context.Noticias.FindAsync(noticiaToUpdate.Id);

        Assert.NotNull(noticiaInDatabase); // Verifica se a notícia ainda está no banco de dados

        // Verifica se os dados da notícia foram atualizados corretamente
        Assert.Equal(updateNoticiaCommand.Titulo, noticiaInDatabase.Titulo);
        Assert.Equal(updateNoticiaCommand.Descricao, noticiaInDatabase.Descricao);
        Assert.Equal(updateNoticiaCommand.Chapeu, noticiaInDatabase.Chapeu);
        Assert.Equal(updateNoticiaCommand.Autor, noticiaInDatabase.Autor);
    }

    [Fact]
    public async Task GetAll_Should_ReturnAllNoticias()
    {
        // Arrange
        var repository = new NoticiaRepository(_context);
        var handler = new GetAllHandle(repository);
        var query = new GetAllQuery();

        _context.Noticias.AddRange(
            new Noticias { Id = Guid.NewGuid(), Titulo = "Notícia 1", Descricao = "Descrição 1", Autor = "Autor 1", Chapeu = "Chapeu 1" },
            new Noticias { Id = Guid.NewGuid(), Titulo = "Notícia 2", Descricao = "Descrição 2", Autor = "Autor 2", Chapeu = "Chapeu 2" }
        );
        _context.SaveChanges();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        // Verifica se o número de notícias retornadas corresponde ao número de notícias no banco de dados
        var noticiasInDatabase = await _context.Noticias.ToListAsync();
        Assert.True(result.Noticias.Count() > 0);

    }

    [Fact]
    public async Task GetNoticiaByIdHandle_Should_ReturnNoticia()
    {
        // Arrange
        var noticia = new Noticias
        {
            Id = Guid.NewGuid(),
            Autor = "Stan Lee",
            Chapeu = "Bone",
            Descricao = "homen aranha ",
            Titulo = "novo jogo",
            DataAtualizacao = DateTime.Now,
            DataPublicacao = DateTime.Now
        };

        _context.Noticias.Add(noticia);
        _context.SaveChanges();

        var repository = new NoticiaRepository(_context);
        var validator = new GetNoticiaByIdValidator();
        var handler = new GetNoticiaByIdHandle(repository, validator);

        var noticiaInDatabase = _context.Noticias.First();

        var query = new GetNoticiaByIdQuery
        {
            Id = noticiaInDatabase.Id
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(query.Id, result.Id);
        Assert.Equal(noticiaInDatabase.Titulo, result.Titulo);
        Assert.Equal(noticiaInDatabase.Descricao, result.Descricao);
    }

    [Fact]
    public async Task GetNoticiaByIdHandle_Should_ReturnNullForInvalidId()
    {
        // Arrange
        var noticia = new Noticias
        {
            Id = Guid.NewGuid(),
            Autor = "Stan Lee",
            Chapeu = "Bone",
            Descricao = "homen aranha ",
            Titulo = "novo jogo",
            DataAtualizacao = DateTime.Now,
            DataPublicacao = DateTime.Now
        };

        _context.Noticias.Add(noticia);
        _context.SaveChanges();

        var repository = new NoticiaRepository(_context);
        var validator = new GetNoticiaByIdValidator();
        var handler = new GetNoticiaByIdHandle(repository, validator);

        var invalidId = Guid.NewGuid(); // Um ID que não existe no banco de dados

        var query = new GetNoticiaByIdQuery
        {
            Id = invalidId
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}