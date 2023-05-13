using ApiFuncionarios.Infra.Data.Entities;
using ApiFuncionarios.Infra.Data.Interfaces;
using ApiFuncionarios.Services.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiFuncionarios.Services.Controllers
{
    [Route("api/criar-conta")]
    [ApiController]
    public class CriarContaController : ControllerBase
    {
        //atributo
        private readonly IUsuarioRepository _usuarioRepository;

        //construtor para injeção de dependência
        public CriarContaController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost]
        public IActionResult Post(RegisterPostModel request)
        {
            try
            {
                //verificar se o email informado já está cadastrado no banco de dados
                if (_usuarioRepository.Get(request.Email) != null)
                    return StatusCode(422, new { message = "O email informado já está cadastrado, por favor verifique." });

                //cadastrando o usuário
                var usuario = new Usuario()
                {
                    IdUsuario = Guid.NewGuid(),
                    Nome = request.Nome,
                    Email = request.Email,
                    Senha = request.Senha
                };

                //gravando o usuário no banco de dados
                _usuarioRepository.Inserir(usuario);

                //retornar resposta de sucesso com o token
                return StatusCode(200, new
                {
                    idUsuario = usuario.IdUsuario,
                    nome = usuario.Nome,
                    email = usuario.Email
                });
            }
            catch (Exception e)
            {
                //HTTP 500 - INTERNAL SERVER ERROR
                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}
