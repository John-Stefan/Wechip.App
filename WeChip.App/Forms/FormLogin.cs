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
    public partial class FormLogin : Form
    {
        ILoginApiRepository _repositorio = new LoginRepository();

        public FormLogin()
        {
            InitializeComponent();

            textBoxSenha.PasswordChar = '*';
        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private async void buttonCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBoxUsuario.Text))
                    throw new Exception("O campo usuario é obrigatorio");


                if (string.IsNullOrEmpty(textBoxSenha.Text))
                    throw new Exception("O campo senha é obrigatorio");

                await _repositorio.LoginCadastroAsync(new Login
                {
                    Username = textBoxUsuario.Text,
                    Password = textBoxSenha.Text
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            var response = await _repositorio.LoginTokenAsync(new Login
            {
                Username = textBoxUsuario.Text,
                Password = textBoxSenha.Text
            });

            if (response.IsSuccessStatusCode && Login.Token != null)
            {
                FormHome formHome = new FormHome();
                formHome.Show();

                Hide();
            }
        }
    }
}
