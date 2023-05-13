using System.Net;
using System.Net.Mail;

namespace ApiFuncionarios.Services.Messages
{
    /// <summary>
    /// Classe para serviço de envio de email
    /// </summary>
    public class MailService
    {
        //atributo
        private readonly MailSettings _mailSettings;

        //construtor para injeção de dependência
        public MailService(MailSettings mailSettings)
        {
            _mailSettings = mailSettings;
        }

        //método para realizar o envio do email
        public void SendMail(string to, string subject, string body)
        {
            #region Criando o conteúdo do email

            var mailMessage = new MailMessage(_mailSettings.Conta, to);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            #endregion

            #region Enviando o email

            var smtpClient = new SmtpClient(_mailSettings.Smtp, _mailSettings.Porta);
            smtpClient.Credentials = new NetworkCredential(_mailSettings.Conta, _mailSettings.Senha);
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);

            #endregion
        }
    }
}
