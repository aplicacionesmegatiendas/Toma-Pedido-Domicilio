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
    public partial class FrmCantidad : Form
    {
        TextBox cant;
        DataGridViewCell cell;
        int cons;
        public FrmCantidad(TextBox cant)
        {
            InitializeComponent();
            this.cant = cant;
            this.cons = 1;
        }
        public FrmCantidad(DataGridViewCell cell)
        {
            InitializeComponent();
            this.cell = cell;
            this.cons = 2;
        }

        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || char.IsPunctuation(e.KeyChar)))
            {
                e.Handled = true;
            }
            else if (e.KeyChar == ',' || e.KeyChar == '.')//Si lo que se digito fue una coma o un punto el programa lo reemplaza por el caracter que usa el sistema para separar decimales.
            {
                e.KeyChar = Configuracion.separador;
            }
            else if (!(e.KeyChar == '1' || e.KeyChar == '2' || e.KeyChar == '3' || e.KeyChar == '4' || e.KeyChar == '5' || e.KeyChar == '6' || e.KeyChar == '7' || e.KeyChar == '8' || e.KeyChar == '9' || e.KeyChar == '0' || e.KeyChar == (char)8))
            {
                e.Handled = true;
            }
        }

        private void txt_cantidad_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode.Equals(Keys.Enter))
                {
                    if (!txt_cantidad.Text.Trim().Equals("") && Convert.ToDecimal(txt_cantidad.Text.Trim().Replace('.', Configuracion.separador).Replace(',', Configuracion.separador)) > 0)
                    {
                        if (cons == 1)
                        {
                            this.cant.Text = txt_cantidad.Text;
                            this.Close();
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            this.cell.Value = txt_cantidad.Text;
                            this.Close();
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                    else
                    {
                        txt_cantidad.Focus(); txt_cantidad.SelectAll();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void FrmCantidad_Load(object sender, EventArgs e)
        {
            this.Top = this.Owner.Height / 2;
        }
    }
}
