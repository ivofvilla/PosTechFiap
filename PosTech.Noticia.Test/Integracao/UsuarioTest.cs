using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PosTech.Noticia.API.Data;
using PosTech.Noticia.API.Repositories;
using PosTech_Fase.Queries.Login;
using PosTech_Fase.Queries.Usuario;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using PosTech.Usuario.API.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;

namespace PosTech.Noticia.Test.Integracao
{
    public class UsuarioTest
    {
        private DbContextClass _context;
        private IUsuarioRepository _repository;
        private Mock<IConfiguration> _configuration;

        public UsuarioTest()
        {
            //Arrange
            _configuration = new Mock<IConfiguration>();
            var builder = new DbContextOptionsBuilder<DbContextClass>();
            builder.UseInMemoryDatabase("Usuario");
            var options = builder.Options;

            _context = new DbContextClass(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _configuration.Setup(c => c["AppSettings:Secret"]).Returns("JUSTTOJbaOzaUAK7pAKyteTiR#5pd&WRF7%1I*M*M22IIEee%qE*OYyOmhDXdk^BcO0%pCoKCKq4v@iq7!*MmV*QVc!gi!N8z8StN9%XKPERFEITO");
            _configuration.Setup(c => c["AppSettings:Emissor"]).Returns("2");
            _configuration.Setup(c => c["AppSettings:ValidoEm"]).Returns("32");
            _configuration.Setup(c => c["AppSettings:ExpiracaoHoras"]).Returns("1");

            _repository = new UsuarioRepository(_context);
        }

        [Fact]
        public async Task CreateUsurarioHandler_Should_CreateUser()
        {// Arrange

            var validator = new CreateUsurarioValidator();

            var handler = new CreateUsurarioHandler(validator, _repository);

            var create = new CreateUsurarioCommand
            {
                Email = "teste@teste.com",
                ConfirmacaoSenha = "Zp*4",
                Senha = "Zp*4",
                Nome = "PosTech Fiap"
            };

            // Act
            var result = await handler.Handle(create, CancellationToken.None);

            // Assert
            Assert.True(result);

            var userDatabase = await _context.Usuario.FirstOrDefaultAsync();

            Assert.NotNull(userDatabase);
            Assert.Equal(create.Email, userDatabase.Login);
        }

        [Fact]
        public async Task GetUsuarioHandle_Should_ReturnUsuario()
        {
            // Arrange
            var usuario = new API.Models.Usuarios("teste@mail.com", "Zp6#", "Usuario teste");


            _context.Usuario.Add(usuario);
            _context.SaveChanges();

            var validator = new GetUsuarioValidator();

            var handler = new ObterAlbumPorIdQueryHandler(_repository, validator, _configuration.Object);

            var usersDatabase = _context.Usuario.First();

            var query = new GetUsuarioQuery
            {
                Email = usersDatabase.Login,
                Password = usersDatabase.Senha
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Token);
        }

    }
}
