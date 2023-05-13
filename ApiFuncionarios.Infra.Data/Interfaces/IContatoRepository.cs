using ApiFuncionarios.Infra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiFuncionarios.Infra.Data.Interfaces
{
    /// <summary>
    /// Interface de repositório específica para Funcionario
    /// </summary>
    public interface IContatoRepository : IBaseRepository<Contato>
    {
        
    }
}
