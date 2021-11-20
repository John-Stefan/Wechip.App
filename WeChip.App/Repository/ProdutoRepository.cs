using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeChip.App.Models;

namespace WeChip.App.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        public async Task<HttpResponseMessage> CadastroProdutoAsync(Produto produto)
        {
            var response = new HttpResponseMessage();

            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("http://localhost:5000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Login.Token);

                var jsonContent = JsonConvert.SerializeObject(produto);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                contentString.Headers.ContentType = new 
                    MediaTypeHeaderValue("application/json");

                response = await client.PostAsync("v1/produtos/", contentString);

                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return response;
            }
        }

        public async Task<Produto> PesquisarProdutoAsync(string codigoOrDescricao)
        {
            var response = new HttpResponseMessage();
            var produto = new Produto();

            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("http://localhost:5000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Login.Token);

                response = await client.GetAsync(string.Format("v1/produtos/{0}/", codigoOrDescricao));

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    produto = JsonConvert.DeserializeObject<Produto>(result);
                }
                else
                {
                    produto = null;
                    throw new Exception("Produto informado não encontrado");
                }

                return produto;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return produto;
            }
        }
    }
}
