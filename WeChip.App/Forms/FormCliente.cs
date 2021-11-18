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
        Cliente cliente = new Cliente();

        public FormCliente()
        {
            InitializeComponent();

            HabilitaCampos(false);
            textBoxCredito.Enabled = false;
        }   

        private void HabilitaCampos(bool active)
        {
            textBoxNome.Enabled = active;
            maskedTextBoxCPF.Enabled = active;
            maskedTextBoxTelefone.Enabled = active;

            buttonEditar.Enabled = active;
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
            textBoxCredito.Enabled = true;
            LimpaCampos();
        }

        private async void buttonSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos(textBoxNome.Text, "Nome");
                ValidarCampos(maskedTextBoxTelefone.Text, "Telefone");
                ValidarCampos(maskedTextBoxCPF.Text, "CPF");

                if (string.IsNullOrEmpty(textBoxCredito.Text))
                    throw new Exception("O campo Credito não pode ficar vazio");

                var response = await _repositorio.CadastroClienteAsync(new Cliente
                {
                    Nome = textBoxNome.Text,
                    Credito = Convert.ToDecimal(textBoxCredito.Text.Replace("R$ ", "")),
                    CPF = maskedTextBoxCPF.Text,
                    Telefone = maskedTextBoxTelefone.Text,
                    StatusId = "0001"
                });

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Cadastro do cliente {textBoxNome.Text} realizado com sucesso", "Cliente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpaCampos();
                    textBoxCredito.Enabled = false;
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

        private async void buttonBuscar_Click(object sender, EventArgs e)
        {
            var clienteResult = await _repositorio.PesquisarClienteAsync(textBoxBuscar.Text);

            if (clienteResult != null)
            {
                textBoxCredito.Enabled = false;
                textBoxNome.Text = clienteResult.Nome;
                textBoxCredito.Text = clienteResult.Credito.ToString();
                maskedTextBoxTelefone.Text = clienteResult.Telefone;
                maskedTextBoxCPF.Text = clienteResult.CPF;
                textBoxBuscar.Text = "Pesquisar Cliente";
                cliente = clienteResult;

                HabilitaCampos(true);
            }
        }

        private async void buttonEditar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos(textBoxNome.Text, "Nome");
                ValidarCampos(maskedTextBoxTelefone.Text, "Telefone");
                ValidarCampos(maskedTextBoxCPF.Text, "CPF");

                if (string.IsNullOrEmpty(textBoxCredito.Text))
                    throw new Exception("O campo Credito não pode ficar vazio");

                cliente.Nome = textBoxNome.Text;
                cliente.Telefone = maskedTextBoxTelefone.Text;
                cliente.CPF = maskedTextBoxCPF.Text;

                var response = await _repositorio.AlterarClienteAsync(cliente);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Cliente {textBoxNome.Text} foi atualizado com sucesso", "Cliente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HabilitaCampos(false);
                    LimpaCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Cliente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpaCampos();
            }
        }
    }
}
