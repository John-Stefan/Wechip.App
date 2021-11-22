using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChip.App.View
{
    public class VendasModelGridView
    {
        [DisplayName("Codigo da Oferta")]
        public int CodigoOferta { get; set; }

        [DisplayName("Produto")]
        public string DescricaoProduto { get; set; }

        [DisplayName("Valor do Produto")]
        public decimal ValorProduto { get; set; }

        [DisplayName("ValorTotal")]
        public decimal ValorTotal { get; set; }

        public string Cliente { get; set; }

        public string CPF { get; set; }
    }
}
