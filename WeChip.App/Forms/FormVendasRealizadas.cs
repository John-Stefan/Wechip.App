using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeChip.App.Repository;
using WeChip.App.View;

namespace WeChip.App.Forms
{
    public partial class FormVendasRealizadas : Form
    {
        IOfertaProdutoRepository _repositorio = new OfertaProdutoRepository();
        IProdutoRepository _repositorioProduto = new ProdutoRepository();
        IOfertaApiRepository _repositorioOferta = new OfertaApiRepository();
        IClienteApiRepository _repositorioCliente = new ClienteApiRepository();
        List<VendasModelGridView> vendasGridAttribute = new List<VendasModelGridView>();

        public FormVendasRealizadas()
        {
            InitializeComponent();

            LoadingGrid();
        }

        private async void LoadingGrid()
        {
            var listaOfertaProdutosResult = await _repositorio.GetOfertaProdutosAsync();
            var listaProdutosResult = await _repositorioProduto.GetProdutosAsync();
            var listaOfertasResult = await _repositorioOferta.GetOfertasAsync();
            var listaClientesResult = await _repositorioCliente.GetClientesAsync();

            foreach (var oferta in listaOfertaProdutosResult)
            {
                var produtoResult = listaProdutosResult.Where(c => c.Id == oferta.ProdutoId).FirstOrDefault();
                var ofertaResult = listaOfertasResult.Where(c => c.Id == oferta.OfertaId).FirstOrDefault();
                var clienteResult = listaClientesResult.Where(c => c.Id == ofertaResult.ClienteId).FirstOrDefault();

                vendasGridAttribute.Add(new VendasModelGridView
                {
                    CodigoOferta = oferta.OfertaId,
                    Cliente = clienteResult.Nome,
                    CPF = clienteResult.CPF,
                    DescricaoProduto = produtoResult.Descricao,
                    ValorProduto = produtoResult.Preco,
                    ValorTotal = ofertaResult.ValorTotal
                }); 
            }

            dataGridViewProdutos.DataSource = vendasGridAttribute;
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            List<VendasModelGridView> listaTempGridVendas = new List<VendasModelGridView>();

            foreach (var venda in vendasGridAttribute)
            {

                if (venda.CPF.Replace(".", "").Replace("-", "").Contains(textBoxBuscarVendas.Text.ToString()) 
                    || venda.DescricaoProduto.ToUpper().Contains(textBoxBuscarVendas.Text.ToUpper()) 
                    || venda.Cliente.ToUpper().Contains(textBoxBuscarVendas.Text.ToUpper()))
                {
                    listaTempGridVendas.Add(venda);
                }
            }

            if (listaTempGridVendas != null)
                dataGridViewProdutos.DataSource = listaTempGridVendas;
        }
    }
}
