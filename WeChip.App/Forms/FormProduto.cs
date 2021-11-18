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

namespace WeChip.App.Forms
{
    public partial class FormProduto : Form
    {
        IProdutoTipoRepository _repositorioProdutoTipo = new ProdutoTipoRepository();

        public FormProduto()
        {
            InitializeComponent();

            InicializeComboBox();
            HabilitaCampos(false);
        }

        private async void InicializeComboBox()
        {
            var listaProdutoTipos = await _repositorioProdutoTipo.GetProdutoTiposAsync();

            foreach (var produtoTipo in listaProdutoTipos)
                comboBoxProduto.Items.Add(produtoTipo.Descricao);
        }

        private void HabilitaCampos(bool active)
        {
            textBoxCodigo.Enabled = active;
            textBoxDescricao.Enabled = active;
            textBoxPreco.Enabled = active;
            comboBoxProduto.Enabled = active;

            buttonEditar.Enabled = active;
        }

        private void LimpaCampos()
        {
            textBoxCodigo.Text = "";
            textBoxDescricao.Text = "";
            textBoxPreco.Text = "";
            comboBoxProduto.Text = "";
        }

        private void ValidarCampos(string campo, string campoName)
        {
            if (string.IsNullOrEmpty(campo))
                throw new Exception($"O campo {campoName} é obrigatorio");
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            HabilitaCampos(true);
            LimpaCampos();
        }

        private async void buttonSalvar_Click(object sender, EventArgs e)
        {
            var notSelection = -1;

            try
            {
                ValidarCampos(textBoxCodigo.Text, "Codigo");
                ValidarCampos(textBoxDescricao.Text, "Descrição");
                ValidarCampos(textBoxPreco.Text, "Preço");

                if (comboBoxProduto.SelectedIndex == notSelection)
                {
                    throw new Exception("Selecione o tipo do produto");
                }

               //Realizar o insert
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
