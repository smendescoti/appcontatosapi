using ApiFuncionarios.Infra.Data.Entities;
using ApiFuncionarios.Infra.Data.Interfaces;
using ApiFuncionarios.Services.Authentication;
using ApiFuncionarios.Services.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiFuncionarios.Services.Controllers
{
    [Route("api/autenticar")]
    [ApiController]
    public class AutenticarController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly TokenCreator _tokenCreator;

        public AutenticarController(IUsuarioRepository usuarioRepository, TokenCreator tokenCreator)
        {
            _usuarioRepository = usuarioRepository;
            _tokenCreator = tokenCreator;
        }

        [HttpPost]
        public IActionResult Post(LoginPostModel model)
        {
            try
            {
                //buscando o usuário no banco de dados através do email e senha
                var usuario = _usuarioRepository.Get(model.Email, model.Senha);

                //verificar se o usuário foi encontrado
                if (usuario != null)
                {
                    var br = TimeZoneInfo.FindSystemTimeZoneById("Brazil/East");

                    //retornar resposta de sucesso com o token
                    return StatusCode(200, new
                    {
                        idUsuario = usuario.IdUsuario,
                        nome = usuario.Nome,
                        email = usuario.Email,
                        accessToken = _tokenCreator.GenerateToken(usuario.Email),
                        createdAt = string.Format("{0}", TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, br).ToString()),
                        expiration = string.Format("{0}", TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddHours(6), br).ToString())
                    });
                }
                else
                {
                    //HTTP STATUS 401 - UNAUTHORIZED
                    return StatusCode(401, new { message = "Acesso não autorizado, email e senha inválidos." });
                }
            }
            catch (Exception e)
            {
                //HTTP STATUS 500 (INTERNAL SERVER ERROR)
                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}
