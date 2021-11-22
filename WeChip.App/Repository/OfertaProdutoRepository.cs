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
    public class OfertaProdutoRepository : IOfertaProdutoRepository
    {
        public async Task<List<OfertaProduto>> GetOfertaProdutosAsync()
        {
            var response = new HttpResponseMessage();
            var ofertaProduto = new List<OfertaProduto>();

            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("http://localhost:5000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Login.Token);

                response = await client.GetAsync($"v1/ofertaprodutos/");

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    ofertaProduto = JsonConvert.DeserializeObject<List<OfertaProduto>>(result);
                }

                return ofertaProduto;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return ofertaProduto;
            }
        }
    }
}
