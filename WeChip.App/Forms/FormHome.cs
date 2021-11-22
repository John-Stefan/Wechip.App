using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeChip.App.Forms
{
    public partial class FormHome : Form
    {
        public FormHome()
        {
            InitializeComponent();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCliente formCliente = new FormCliente();
            formCliente.ShowDialog();
        }

        private void sAIRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Tem certeza que deseja fechar?", "Sair", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    e.Cancel = true;

                if (e.Cancel == false)
                    Application.Exit();
            }
        }

        private void produtosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProduto formProduto = new FormProduto();
            formProduto.ShowDialog();
        }

        private void ofertasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOfertas formOfertas = new FormOfertas();
            formOfertas.ShowDialog();
        }
    }
}
