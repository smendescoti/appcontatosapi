using ApiFuncionarios.Infra.Data.Entities;
using ApiFuncionarios.Infra.Data.Interfaces;
using ApiFuncionarios.Services.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiFuncionarios.Services.Controllers
{
    [Authorize]
    [Route("api/contatos")]
    [ApiController]
    public class ContatosController : ControllerBase
    {
        //atributo
        private readonly IContatoRepository _contatoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        //construtor para injeção de dependência (inicialização)
        public ContatosController(IContatoRepository contatoRepository, IUsuarioRepository usuarioRepository)
        {
            _contatoRepository = contatoRepository;
            _usuarioRepository = usuarioRepository;
        }

        /// <summary>
        /// Método da API para cadastro de funcionário
        /// </summary>
        [HttpPost]
        public IActionResult Post(ContatoPostModel request)
        {
            try
            {
                var usuario = _usuarioRepository.Get(User.Identity.Name);

                //capturando os dados do funcionário
                var contato = new Contato()
                {
                    IdContato = Guid.NewGuid(),
                    Nome = request.Nome,
                    Email = request.Email,
                    Telefone = request.Telefone,
                    DataCriacao = DateTime.Now,
                    IdUsuario = usuario.IdUsuario
                };

                //cadastrando o funcionário
                _contatoRepository.Inserir(contato);

                //HTTP 201 (CREATED)
                return StatusCode(201, contato);
            }
            catch (Exception e)
            {
                //HTTP 500 (INTERNAL SERVER ERROR)
                return StatusCode(500, new { message = e.Message });
            }
        }

        /// <summary>
        /// Método da API para atualização de funcionário
        /// </summary>
        [HttpPut]
        public IActionResult Put(ContatoPutModel request)
        {
            try
            {
                //consultar o funcionário no banco de dados através do id
                var contato = _contatoRepository.ObterPorId(request.IdContato);
                var usuario = _usuarioRepository.Get(User.Identity.Name);

                if (contato.IdUsuario != usuario.IdUsuario)
                    return StatusCode(422, new { message = "Contato inválido." });

                //capturando os dados do funcionário
                contato.Nome = request.Nome;
                contato.Email = request.Email;
                contato.Telefone = request.Telefone;

                //atualizar os dados do funcionário
                _contatoRepository.Alterar(contato);

                //HTTP 200 (OK)
                return StatusCode(200, contato);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        /// <summary>
        /// Método da API para exclusão de funcionário
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                //consultar o funcionário no banco de dados através do id
                var contato = _contatoRepository.ObterPorId(id);
                var usuario = _usuarioRepository.Get(User.Identity.Name);

                if (contato.IdUsuario != usuario.IdUsuario)
                    return StatusCode(422, new { message = "Contato inválido." });

                #region Verificar se o funcionário não existe no banco de dados

                if (contato == null)
                    //HTTP 422 (UNPROCESSABLE ENTITY)
                    return StatusCode(422, new { message = "Contato não encontrado." });

                #endregion

                //excluindoo funcionário
                _contatoRepository.Excluir(contato);

                //HTTP 200 (OK)
                return StatusCode(200, contato);
            }
            catch (Exception e)
            {
                //HTTP 500 (INTERNAL SERVER ERROR)
                return StatusCode(500, new { message = e.Message });
            }
        }

        /// <summary>
        /// Método da API para consulta de funcionários 
        /// </summary>
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var usuario = _usuarioRepository.Get(User.Identity.Name);

                //consultar todos os funcionários cadastrados
                var contatos = _contatoRepository.Consultar()
                    .Where(c => c.IdUsuario == usuario.IdUsuario)
                    .ToList();

                //HTTP 200 (OK)
                return StatusCode(200, contatos);
            }
            catch(Exception e)
            {
                //HTTP 500 (INTERNAL SERVER ERROR)
                return StatusCode(500, new { message = e.Message });
            }
        }

        /// <summary>
        /// Método para API para consultar de 1 funcionário baseado no ID
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                //consultar 1 funcionário baseado no ID..
                var contato = _contatoRepository.ObterPorId(id);
                var usuario = _usuarioRepository.Get(User.Identity.Name);

                if(contato.IdUsuario == usuario.IdUsuario)
                {
                    //HTTP 200 (OK)
                    return StatusCode(200, contato);
                }
                else
                {
                    return StatusCode(204);
                }                
            }
            catch(Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}
