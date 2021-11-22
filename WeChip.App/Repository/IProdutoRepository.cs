using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeChip.App.Models;

namespace WeChip.App.Repository
{
    public interface IProdutoRepository
    {
        Task<HttpResponseMessage> CadastroProdutoAsync(Produto produto);
        Task<Produto> PesquisarProdutoAsync(string codigoOrDescricao);
        Task<HttpResponseMessage> AlterarProdutoAsync(Produto produto);
        Task<List<Produto>> GetProdutosAsync();
    }
}
