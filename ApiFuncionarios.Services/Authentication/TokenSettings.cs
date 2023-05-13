namespace ApiFuncionarios.Services.Authentication
{
    /// <summary>
    /// Classe para capturar os parametros de geração do TOKEN
    /// </summary>
    public class TokenSettings
    {
        /// <summary>
        /// Chave secreta antifalsificação
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Tempo de expiração do TOKEN
        /// </summary>
        public int ExpirationInHours { get; set; }
    }
}
