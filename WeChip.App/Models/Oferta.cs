using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChip.App.Models
{
    public class Oferta
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public Endereco Endereco { get; set; }
        public List<int> ProdutosOfertasId { get; set; }
    }
}
