using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeChip.App.Models;

namespace WeChip.App.Repository
{
    public interface ILoginApiRepository
    {
        Task<HttpResponseMessage> LoginCadastroAsync(Login login);
        Task<HttpResponseMessage> LoginTokenAsync(Login login);
    }
}
