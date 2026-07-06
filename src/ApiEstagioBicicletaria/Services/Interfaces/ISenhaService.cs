namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface ISenhaService
    {
        string GerarHashDaSenha(string senha);
        bool ValidarSenha(string hashSenhaSalva, string senhaInformada);
       
    }
}
