using Microsoft.OpenApi.Models;

namespace ApiFuncionarios.Services
{
    /// <summary>
    /// Classe para configuração do Swagger
    /// </summary>
    public class SwaggerConfiguration
    {
        /// <summary>
        /// Método para registrar a configuração
        /// </summary>
        public static void Register(WebApplicationBuilder builder)
        {
            if (builder.Services == null) throw new ArgumentNullException(nameof(builder.Services));

            builder.Services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Contatos - Treinamento Angular.",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "COTI Informática - Escola de NERDS",
                        Url = new Uri("http://www.cotiinformatica.com.br"),
                        Email = "contato@cotiinformatica.com.br"
                    }
                });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }

        /// <summary>
        /// Método para registrar a configuração
        /// </summary>
        public static void Use(WebApplication app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiContato");
            });
        }
    }
}
