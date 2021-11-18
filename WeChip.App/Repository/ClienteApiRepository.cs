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
    public class ClienteApiRepository : IClienteApiRepository
    {
        public async Task<HttpResponseMessage> CadastroClienteAsync(Cliente cliente)
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

                var jsonContent = JsonConvert.SerializeObject(cliente);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                contentString.Headers.ContentType = new
                MediaTypeHeaderValue("application/json");

                response = await client.PostAsync("v1/clientes/", contentString);

                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return response;
            }
        }

        public async Task<Cliente> PesquisarClienteAsync(string nameOrCpf)
        {
            var response = new HttpResponseMessage();
            var cliente = new Cliente();

            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("http://localhost:5000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Login.Token);

                response = await client.GetAsync(string.Format("v1/clientes/{0}/", nameOrCpf));

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    cliente = JsonConvert.DeserializeObject<Cliente>(result);
                }
                else
                {
                    cliente = null;
                    throw new Exception("Cliente informado não encontrado");
                }                    

                return cliente;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Cliente", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return cliente;
            }
        }

        public async Task<HttpResponseMessage> AlterarClienteAsync(Cliente cliente)
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

                var jsonContent = JsonConvert.SerializeObject(new Cliente
                {
                    CPF = cliente.CPF,
                    Nome = cliente.Nome,
                    Telefone = cliente.Telefone                    
                });
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                contentString.Headers.ContentType = new
                MediaTypeHeaderValue("application/json");

                response = await client.PutAsync($"v1/clientes/{cliente.Id}", contentString);

                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Cliente", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return response;
            }
        }
    }
}
