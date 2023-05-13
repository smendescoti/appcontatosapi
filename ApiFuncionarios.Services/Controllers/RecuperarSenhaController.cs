using ApiFuncionarios.Infra.Data.Entities;
using ApiFuncionarios.Infra.Data.Interfaces;
using ApiFuncionarios.Services.Authentication;
using ApiFuncionarios.Services.Messages;
using ApiFuncionarios.Services.Requests;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiFuncionarios.Services.Controllers
{
    [Route("api/recuperar-senha")]
    [ApiController]
    public class RecuperarSenhaController : ControllerBase
    {
        //atributos
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly MailService _mailService;

        public RecuperarSenhaController(IUsuarioRepository usuarioRepository, MailService mailService)
        {
            _usuarioRepository = usuarioRepository;
            _mailService = mailService;
        }

        [HttpPost]
        public IActionResult Post(PasswordPostModel model)
        {
            try
            {
                //buscar o usuário no banco de dados através do email
                var usuario = _usuarioRepository.Get(model.Email);

                //verificar se o usuário foi encontrado
                if (usuario != null)
                {
                    #region Enviando email de recuperação de senha

                    var novaSenha = new Faker().Internet.Password();
                    EnviarEmailDeRecuperacaoDeSenha(usuario, novaSenha);

                    #endregion

                    #region Atualizando a senha no banco de dados

                    usuario.Senha = novaSenha;
                    _usuarioRepository.Alterar(usuario);

                    #endregion

                    //retornar resposta de sucesso com o token
                    return StatusCode(200, new
                    {
                        idUsuario = usuario.IdUsuario,
                        nome = usuario.Nome,
                        email = usuario.Email
                    });
                }
                else
                {
                    return StatusCode(422, new { message = "O email informado não foi encontrado, por favor verifique." });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private void EnviarEmailDeRecuperacaoDeSenha(Usuario usuario, string novaSenha)
        {
            var subject = "Recuperação de senha de usuário - COTI Informática";

            var body = $@"
                     <div style='text-align: center; margin: 40px; padding: 60px; border: 2px solid #ccc; font-size: 16pt;'>
                     <img src='https://www.cotiinformatica.com.br/imagens/logo-coti-informatica.png' />
                     <br/><br/>
                     Olá <strong>{usuario.Nome}</strong>,
                     <br/><br/>    
                     O sistema gerou uma nova senha para que você possa acessar sua conta.<br/>
                     Por favor utilize a senha: <strong>{novaSenha}</strong>
                     <br/><br/>
                     Não esqueça de, ao acessar o sistema, atualizar esta senha para outra
                     de sua preferência.
                     <br/><br/>              
                     Att<br/>   
                     Equipe COTI Informatica
                     </div>
            ";

            _mailService.SendMail(usuario.Email, subject, body);
        }
    }
}
