using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeChip.App.Models;

namespace WeChip.App.Repository
{
    public interface IClienteApiRepository
    {
        Task<HttpResponseMessage> CadastroClienteAsync(Cliente cliente);
        Task<Cliente> PesquisarClienteAsync(string nameOrCpf);
        Task<HttpResponseMessage> AlterarClienteAsync(Cliente cliente);
    }
}
