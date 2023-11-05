using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PosTech.Noticia.API.Models;
using PosTech.Noticia.API.Repositories;
using PosTech_Fase.Queries.Login;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PosTech_Fase.Queries.Usuario
{
    public class ObterAlbumPorIdQueryHandler : IRequestHandler<GetUsuarioQuery, GetUsuarioResult>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IValidator<GetUsuarioQuery> _validator;
        private readonly IConfiguration _configuration;

        public ObterAlbumPorIdQueryHandler(IUsuarioRepository usuarioRepository, IValidator<GetUsuarioQuery> validator, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _validator = validator;
            _configuration = configuration;
        }

        public async Task<GetUsuarioResult?> Handle(GetUsuarioQuery query, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);
            if (!validationResult.IsValid)
            {
                return null;
            }

            var result = await _usuarioRepository.ObterLogin(query.Email, query.Password);
            if (result != null)
            {
                string token = await GerarJwt(result);
                var userToken = new GetUsuarioResult();
                userToken.SetToken(token);
                return userToken;
            }

            return null;

        }

        private async Task<string> GerarJwt(Usuarios usuario)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, usuario.Login));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));


            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _configuration["AppSettings:Emissor"],
                Subject = identityClaims,
                Audience = _configuration["AppSettings:ValidoEm"],
                Expires = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["AppSettings:ExpiracaoHoras"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);
            return encodedToken;
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
