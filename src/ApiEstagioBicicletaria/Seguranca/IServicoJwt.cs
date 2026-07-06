using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Seguranca
{
    public interface IServicoJwt
    {
        string GerarJWT(Usuario usuario);
    }
}
