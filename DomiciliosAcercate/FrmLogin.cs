using System;
using System.Windows.Forms;

namespace DomiciliosEntrecaminos
{
    public partial class FrmLogin : Form
    {
        Label lblterminal;
        public FrmLogin(Label lblterminal)
        {
            InitializeComponent();
            this.lblterminal = lblterminal;
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            FrmPpal.reset = true;
            Application.Exit();
        }

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            if (txt_usuario.Text.Trim() == "")
            {
                MessageBox.Show("Escriba el nombre de usuario", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                Terminal terminal = new Terminal();
                object[] t = terminal.ObtenerDatosUsuario(txt_usuario.Text.Trim());
                if (t == null)
                {
                    MessageBox.Show("El usuario no esta vinculado con ninguna terminal", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_usuario.Focus();
                    txt_usuario.SelectAll();
                    return;
                }
                Configuracion.NroTerminal= t[0].ToString();
				Configuracion.DescripcionUsuario= t[1].ToString();
                Configuracion.TipoUsuario = Convert.ToInt32(t[2]);
                lblterminal.Text = Configuracion.NroTerminal;

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
