using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChip.App.Models
{
    public class Status
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("finalizacliente")]
        public bool FinalizaCliente { get; set; }

        [JsonProperty("contabilizavenda")]
        public bool ContabilizaVenda { get; set; }
    }
}
