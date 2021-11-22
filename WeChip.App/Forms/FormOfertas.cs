using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using WeChip.App.Models;
using WeChip.App.Repository;
using WeChip.App.View;

namespace WeChip.App.Forms
{
    public partial class FormOfertas : Form
    {
        IClienteApiRepository _repositorioCliente = new ClienteApiRepository();
        IStatusApiRepository _repositorioStatus = new StatusApiRepository();
        IProdutoRepository _repositorioProduto = new ProdutoRepository();
        IProdutoTipoRepository _repositorioTipoProduto = new ProdutoTipoRepository();
        IOfertaApiRepository _repositorio = new OfertaApiRepository();
        Cliente cliente = new Cliente();

        public FormOfertas()
        {
            InitializeComponent();

            HabilitarComponentes(false);
        }

        private async void buttonBuscar_Click(object sender, EventArgs e)
        {
            var clienteResult = await _repositorioCliente.GetClientesByNameOrCPFAsync(textBoxBuscarClientes.Text);
            List<object> clientesRemoveAttribute = new List<object>();

            if (clienteResult != null)
            {
                foreach (var cliente in clienteResult.Where(c => c.StatusId != "2" && c.StatusId != "3" && c.StatusId != "6"))
                {
                    clientesRemoveAttribute.Add(new ClienteModelGridView
                    {
                        Id = cliente.Id,
                        CPF = cliente.CPF,
                        Nome = cliente.Nome
                    });
                }
                
                dataGridViewClientes.DataSource = clientesRemoveAttribute;
            }
        }

        private async void dataGridViewClientes_SelectionChanged(object sender, EventArgs e)
        {
            Cliente clienteResult;
            Status statusResult;

            DataGridView dgv = sender as DataGridView;
            if (dgv != null && dgv.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgv.SelectedRows[0];
                if (row != null)
                {
                    var listaProdutosResult = await _repositorioProduto.GetProdutosAsync();
                    var listaTipoProdutosResult = await _repositorioTipoProduto.GetProdutoTiposAsync();
                    var listaStatusResult = await _repositorioStatus.GetStatusAsync();
                    var nomeLivre = 1;

                    clienteResult = await _repositorioCliente.GetClienteByIdAsync(Convert.ToInt32(row.Cells[0].Value.ToString()));
                    statusResult = await _repositorioStatus.GetStatusByIdAsync(Convert.ToInt32(clienteResult.StatusId));

                    foreach (var produto in listaProdutosResult)
                    {
                        produto.TipoDescricao = listaTipoProdutosResult.FirstOrDefault(c => c.Id == produto.TipoId).Descricao;
                    }

                    textBoxNome.Text = clienteResult.Nome;
                    maskedTextBoxCPF.Text = clienteResult.CPF;
                    maskedTextBoxTelefone.Text = clienteResult.Telefone;
                    textBoxStatusAtual.Text = statusResult.Descricao;
                    textBoxCredito.Text = clienteResult.Credito.ToString();

                    dataGridViewProdutos.DataSource = listaProdutosResult;
                    dataGridViewProdutos.Columns.Remove("Tipo");
                    dataGridViewProdutos.Columns.Remove("TipoId");
                    dataGridViewProdutos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridViewProdutos.MultiSelect = true;

                    if (comboBoxStatus.Items.Count == 0)
                    {
                        foreach (var status in listaStatusResult.Where(c => c.Id != nomeLivre))
                        {
                            comboBoxStatus.Items.Add(status.Descricao);
                        }
                    }

                    cliente = clienteResult;
                    HabilitarComponentes(true);
                }
            }
        }

        private void dataGridViewProdutos_SelectionChanged(object sender, EventArgs e)
        {
            decimal ValorTotal = 0;

            DataGridView dgv = sender as DataGridView;
            if (dgv != null && dgv.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dgv.SelectedRows.Count; i++)
                {
                    ValorTotal += Convert.ToDecimal(dgv.SelectedRows[i].Cells[3].Value.ToString());
                    textBoxValorTotal.Text = ValorTotal.ToString();
                }
            }
            else
                textBoxValorTotal.Text = "0,00";
        }

        private async void buttonConfirmar_Click(object sender, EventArgs e)
        {
            var notSelection = -1;
            List<int> listaProdutos = new List<int>();

            try
            {
                ValidarCampos(textBoxNome.Text, "Nome");
                ValidarCampos(maskedTextBoxTelefone.Text, "Telefone");
                ValidarCampos(maskedTextBoxCPF.Text, "CPF");

                if (comboBoxStatus.SelectedIndex == notSelection)
                    throw new Exception("Selecione o Status do cliente");

                for (int i = 0; i < dataGridViewProdutos.SelectedRows.Count; i++)
                {
                    if (dataGridViewProdutos.SelectedRows[i].Cells[4].Value.ToString() == "HARDWARE")
                    {
                        ValidarEndereco();
                    }

                    listaProdutos.Add(Convert.ToInt32(dataGridViewProdutos.SelectedRows[i].Cells[0].Value.ToString()));
                }

                if (comboBoxStatus.Text == "Cliente Aceitou Oferta")
                {
                    if (dataGridViewProdutos.SelectedRows.Count == 0)
                        throw new Exception("Para finalizar a venda é necessario selecionar algum produto");
                }
                else
                {
                    if (dataGridViewProdutos.SelectedRows.Count != 0)
                        throw new Exception("Ao selecionar algum produto é necessario finalizar a venda");
                }

                if (Convert.ToDecimal(textBoxValorTotal.Text) > Convert.ToDecimal(textBoxCredito.Text))
                    throw new Exception("Credito do cliente insuficiente para realizar a venda");

                var responseOferta = await _repositorio.CadastroOfertaAsync(new Oferta
                {
                    ProdutosOfertasId = listaProdutos,
                    Endereco = new Endereco
                    {
                        CEP = maskedTextBoxCEP.Text,
                        Rua = textBoxRua.Text,
                        Numero = Convert.ToInt32(textBoxNumero.Text),
                        Bairro = textBoxBairro.Text,
                        Cidade = textBoxCidade.Text,
                        Complemento = textBoxComplemento.Text,
                        Estado = textBoxEstado.Text
                    },
                    ClienteId = Convert.ToInt32(dataGridViewClientes.SelectedRows[0].Cells[0].Value.ToString())
                });

                var statusSelecionado = await _repositorioStatus.GetStatusByIdAsync(Convert.ToInt32(comboBoxStatus.SelectedIndex + 2));

                var responseCliente = await _repositorioCliente.AlterarClienteAsync(new Cliente
                {
                    Id = cliente.Id,
                    Nome = textBoxNome.Text,
                    CPF = maskedTextBoxCPF.Text,
                    Telefone = maskedTextBoxTelefone.Text,
                    StatusId = statusSelecionado.Codigo
                });

                if (responseCliente.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Oferta realizada com sucesso realizado com sucesso", "Oferta", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    HabilitarComponentes(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ValidarCampos(string campo, string campoName)
        {
            if (string.IsNullOrEmpty(campo))
                throw new Exception($"O campo {campoName} é obrigatorio");
        }

        private void MensagemValidacaoEndereco(string campo, string campoName)
        {
            if (string.IsNullOrEmpty(campo))
                throw new Exception($"Para realizar uma oferta de um produto do tipo HARDWARE o campo {campoName} é obrigatorio");
        }

        private void ValidarEndereco()
        {
            MensagemValidacaoEndereco(maskedTextBoxCEP.Text, "CEP");
            MensagemValidacaoEndereco(textBoxRua.Text, "RUA");
            MensagemValidacaoEndereco(textBoxNumero.Text, "Numero");
            MensagemValidacaoEndereco(textBoxComplemento.Text, "Complemento");
            MensagemValidacaoEndereco(textBoxBairro.Text, "Bairro");
            MensagemValidacaoEndereco(textBoxEstado.Text, "Estado");
            MensagemValidacaoEndereco(textBoxCidade.Text, "Cidade");
        }

        private void HabilitarComponentes(bool active)
        {
            textBoxNome.Enabled = active;
            maskedTextBoxCPF.Enabled = active;
            maskedTextBoxTelefone.Enabled = active;
            maskedTextBoxCEP.Enabled = active;
            textBoxRua.Enabled = active;
            textBoxNumero.Enabled = active;
            textBoxComplemento.Enabled = active;
            textBoxBairro.Enabled = active;
            textBoxEstado.Enabled = active;
            textBoxCidade.Enabled = active;
            comboBoxStatus.Enabled = active;
            buttonConfirmar.Enabled = active;

            if (!active)
            {
                textBoxNome.Text = "";
                maskedTextBoxCPF.Text = "";
                maskedTextBoxTelefone.Text = "";
                maskedTextBoxCEP.Text = "";
                textBoxRua.Text = "";
                textBoxNumero.Text = "";
                textBoxComplemento.Text = "";
                textBoxBairro.Text = "";
                textBoxEstado.Text = "";
                textBoxCidade.Text = "";
                textBoxCredito.Text = "";
                textBoxStatusAtual.Text = "";
                textBoxBuscarClientes.Text = "";
                dataGridViewClientes.DataSource = null;
                dataGridViewProdutos.DataSource = null;
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Tem certeza que deseja cancelar?", "Cancelar", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                HabilitarComponentes(false);
        }
    }
}
