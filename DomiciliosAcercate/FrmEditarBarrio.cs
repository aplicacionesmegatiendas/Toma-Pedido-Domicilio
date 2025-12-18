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
    public partial class FrmEditarBarrio : Form
    {
        
        public FrmEditarBarrio(string id, string nombre, string ciudad, string cod_dom, string cod_dom_express, bool activo)
        {
            InitializeComponent();
            txt_id.Text = id;
            txt_nombre.Text = nombre;
            txt_ciudad.Text = ciudad;
            txt_cod_dom.Text = cod_dom;
            txt_cod_dom_express.Text = cod_dom_express;
            chk_activo.Checked = activo;
        }

        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || char.IsPunctuation(e.KeyChar)))
            {
                e.Handled = true;
            }
            else if (!(e.KeyChar == '1' || e.KeyChar == '2' || e.KeyChar == '3' || e.KeyChar == '4' || e.KeyChar == '5' || e.KeyChar == '6' || e.KeyChar == '7' || e.KeyChar == '8' || e.KeyChar == '9' || e.KeyChar == '0' || e.KeyChar == (char)8))
            {
                e.Handled = true;
            }
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            if (txt_cod_dom.Text.Trim().Equals(""))
            {
                MessageBox.Show("Escriba el código de domicilio", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                CiudadCentroOperacion guardar = new CiudadCentroOperacion();

                guardar.EditarBarrio(Convert.ToInt32(txt_id.Text), chk_activo.Checked, txt_cod_dom.Text.Trim(),txt_cod_dom_express.Text.Trim());
                
                MessageBox.Show("Barrio editado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
