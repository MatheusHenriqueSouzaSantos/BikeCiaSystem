using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Services.Interfaces;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;

namespace ApiEstagioBicicletaria.Services
{
    public class SenhaService : ISenhaService
    {
        public string GerarHashDaSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public bool ValidarSenha(string hashSenhaSalva, string senhaInformada)
        {
            var resultado = BCrypt.Net.BCrypt.Verify(senhaInformada, hashSenhaSalva);

            return resultado;
        }
    }
}