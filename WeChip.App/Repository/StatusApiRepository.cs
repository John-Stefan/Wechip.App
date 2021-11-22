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
    public class StatusApiRepository : IStatusApiRepository
    {
        public async Task<List<Status>> GetStatusAsync()
        {
            var response = new HttpResponseMessage();
            var status = new List<Status>();

            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("http://localhost:5000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Login.Token);

                response = await client.GetAsync($"v1/status/");

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    status = JsonConvert.DeserializeObject<List<Status>>(result);
                }

                return status;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return status;
            }
        }

        public async Task<Status> GetStatusByIdAsync(int id)
        {
            var response = new HttpResponseMessage();
            var status = new Status();

            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("http://localhost:5000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Login.Token);

                response = await client.GetAsync($"v1/status/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    status = JsonConvert.DeserializeObject<Status>(result);
                }

                return status;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return status;
            }
        }
    }
}
