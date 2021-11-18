using System;
using System.Windows.Forms;
using System.Drawing;

namespace WeChip.App.Forms
{
    public class txtCredito : TextBox
    {
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.BackColor = Color.SkyBlue;
            this.SelectAll();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.BackColor = Color.White;

            if (this.Text == "")
                return;

            try
            {
                var credito = Convert.ToDecimal(this.Text.Replace("R$ ", ""));
                this.Text = String.Format("{0:c}", credito);
            }
            catch (Exception ex)
            {
                this.Text = "";
                MessageBox.Show("Valor informado invalido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                this.Text = "";
                e.SuppressKeyPress = true;
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.TextAlign = HorizontalAlignment.Center;
        }
    }
}
