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
    public class ProdutoTipoRepository : IProdutoTipoRepository
    {
        public async Task<List<ProdutoTipo>> GetProdutoTiposAsync()
        {
            var response = new HttpResponseMessage();
            var produtoTipo = new List<ProdutoTipo>();

            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("http://localhost:5000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Login.Token);

                response = await client.GetAsync("v1/produtotipos/");

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    produtoTipo = JsonConvert.DeserializeObject<List<ProdutoTipo>>(result);
                }

                return produtoTipo;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return produtoTipo;
            }
        }
    }
}
