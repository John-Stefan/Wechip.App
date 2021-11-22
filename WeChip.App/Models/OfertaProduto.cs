using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChip.App.Models
{
    public class OfertaProduto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("produtoid")]
        public int ProdutoId { get; set; }

        [JsonProperty("ofertaid")]
        public int OfertaId { get; set; }
    }
}
