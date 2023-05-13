namespace ApiFuncionarios.Services.Messages
{
    /// <summary>
    /// Modelo de dados para capturar os parametros de envio de email
    /// </summary>
    public class MailSettings
    {
        public string Conta { get; set; }
        public string Senha { get; set; }
        public string Smtp { get; set; }
        public int Porta { get; set; }
    }
}
