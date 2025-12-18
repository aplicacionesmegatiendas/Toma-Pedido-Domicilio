using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DomiciliosEntrecaminos
{
    public partial class FrmUsuarioEntrecaminos : Form
    {
        public FrmUsuarioEntrecaminos()
        {
            InitializeComponent();
        }

        private void Limpiar()
        {
            txt_val_min.Text = "";
            txt_cant_max.Text = "";
            txt_cos.Text = "";
        }

        private void ObtenerConfiguracionActual()
        {
            Configuracion configuracion = new Configuracion();
            object[] conf = configuracion.ObtenerConfiguracion();

            string cos = conf[2].ToString();

            string valpedidomin = conf[4].ToString();
            int cant_max = Convert.ToInt32(conf[5]);

            txt_cos.Text = cos;

            txt_val_min.Text = valpedidomin;
            txt_cant_max.Text = cant_max.ToString();
        }

        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || char.IsPunctuation(e.KeyChar)))
            {
                e.Handled = true;
            }
            else if (!(e.KeyChar == '1' || e.KeyChar == '2' || e.KeyChar == '3' || e.KeyChar == '4' || e.KeyChar == '5' || e.KeyChar == '6' || e.KeyChar == '7' || e.KeyChar == '8' || e.KeyChar == '9' || e.KeyChar == '0' || e.KeyChar == (char)8 || e.KeyChar == '-'))
            {
                e.Handled = true;
            }
        }

        private bool ValidaCos(String cadena)
        {
            //Regex patronAlfabetico = new Regex("'[0-9]{3}'(,'[0-9]{3}')*$");
            Regex patronAlfabetico = new Regex("[0-9]{3}(,[0-9]{3})*$");
            return patronAlfabetico.IsMatch(cadena);
        }

        private void FrmUsuarioEntrecaminos_Load(object sender, EventArgs e)
        {
            try
            {
                ObtenerConfiguracionActual();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            if (ValidaCos(txt_cos.Text.Trim().Replace(Environment.NewLine,"")).Equals(false))
            {
                MessageBox.Show("Listado de centros de operación no valido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txt_cos.Focus();
                return;
            }

            try
            {
                Configuracion configuracion = new Configuracion();
				configuracion.ActualizarConfiguracion(Convert.ToDecimal(txt_val_min.Text.Trim()), txt_cant_max.Text.Trim(), txt_cos.Text.Trim().Replace(Environment.NewLine,""));
                MessageBox.Show("Configuración actualizada correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Limpiar();
                ObtenerConfiguracionActual();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_barrios_Click(object sender, EventArgs e)
        {
            new FrmBarrios().ShowDialog(this);
        }

        private void btn_cobertura_Click(object sender, EventArgs e)
        {
            new FrmCobertura().ShowDialog(this);
        }

        private void btn_usuarios_term_Click(object sender, EventArgs e)
        {
            new FrmUsuarioTerminal().ShowDialog(this);
        }
    }
}
