using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeChip.App.Forms;
using WeChip.App.Models;

namespace WeChip.App.Repository
{
    public class LoginRepository : ILoginApiRepository
    {
        public async Task<HttpResponseMessage> LoginCadastroAsync(Login login)
        {
            var response = new HttpResponseMessage();

            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("http://localhost:5000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = JsonConvert.SerializeObject(login);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                contentString.Headers.ContentType = new
                MediaTypeHeaderValue("application/json");

                response = await client.PostAsync("v1/login/", contentString);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    MessageBox.Show("Usuario cadastrado com sucesso!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    MessageBox.Show($"{result}", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return response;
            }
        }

        public async Task<HttpResponseMessage> LoginTokenAsync(Login login)
        {
            var response = new HttpResponseMessage();

            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("http://localhost:5000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                response = await client.GetAsync(string.Format("v1/login/{0}/{1}", login.Username, login.Password));

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Login.Token = JsonConvert.DeserializeObject<string>(result);
                }
                else
                    throw new Exception("Login ou senha invalidos");

                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return response;
            }
        }
    }
}
