using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeChip.App.Models;
using WeChip.App.Repository;

namespace WeChip.App.Forms
{
    public partial class FormCliente : Form
    {
        IClienteApiRepository _repositorio = new ClienteApiRepository();

        public FormCliente()
        {
            InitializeComponent();

            HabilitaCampos(false);
        }

        private void HabilitaCampos(bool active)
        {
            textBoxNome.Enabled = active;
            textBoxCredito.Enabled = active;
            maskedTextBoxCPF.Enabled = active;
            maskedTextBoxTelefone.Enabled = active;
        }  
        
        private void LimpaCampos()
        {
            textBoxNome.Text = "";
            textBoxCredito.Text = "";
            maskedTextBoxCPF.Text = "";
            maskedTextBoxTelefone.Text = "";
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            HabilitaCampos(true);
            LimpaCampos();
        }

        private async void buttonSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos(textBoxNome.Text, "Nome");
                ValidarCampos(maskedTextBoxTelefone.Text, "Telefone");
                ValidarCampos(maskedTextBoxCPF.Text, "CPF");

                var response = await _repositorio.CadastroClienteAsync(new Cliente
                {
                    Nome = textBoxNome.Text,
                    Credito = decimal.Parse(textBoxCredito.Text),
                    CPF = maskedTextBoxCPF.Text,
                    Telefone = maskedTextBoxTelefone.Text,
                    CodigoStatus = "0001"
                });

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Cadastro do cliente {textBoxNome.Text} realizado com sucesso", "Cliente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HabilitaCampos(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Cliente", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ValidarCampos(string campo, string campoName)
        {
            if (string.IsNullOrEmpty(campo))
                throw new Exception($"O campo {campoName} é obrigatorio");
        }
    }
}
