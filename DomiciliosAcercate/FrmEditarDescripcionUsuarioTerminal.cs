using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DomiciliosEntrecaminos
{
    public partial class FrmEditarDescripcionUsuarioTerminal : Form
    {
        int numero;
        public FrmEditarDescripcionUsuarioTerminal(int numero, string usuario, string descripcion)
        {
            InitializeComponent();
            this.numero = numero;
            txt_terminal.Text = numero.ToString();
            txt_usuario.Text = usuario;
            txt_descripcion.Text = descripcion;
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_descripcion.Text.Trim()))
            {
                MessageBox.Show("Escriba la descripción","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                txt_descripcion.Focus();
                return;
            }
            try
            {
                Terminal terminal = new Terminal();
				terminal.ActualizarDescripcionTerminal(numero, txt_descripcion.Text.Trim());
                MessageBox.Show("Descripción editada correctamente", "Aviso",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmEditarDescripcionUsuarioTerminal_Load(object sender, EventArgs e)
        {
            txt_descripcion.Focus();
            txt_descripcion.SelectAll();
        }
    }
}
