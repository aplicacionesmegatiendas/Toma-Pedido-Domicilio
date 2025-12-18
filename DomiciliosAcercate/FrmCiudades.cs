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
    public partial class FrmCiudades : Form
    {
        public FrmCiudades()
        {
            InitializeComponent();
        }

        private void ListarCiudades()
        {
			CiudadCentroOperacion ciudades = new CiudadCentroOperacion();
            dgv_ciudades.AutoGenerateColumns = false;
            dgv_ciudades.Columns[0].DataPropertyName = "ci_id";
            dgv_ciudades.Columns[1].DataPropertyName = "ci_nombre";
            dgv_ciudades.DataSource = ciudades.ListarCiudades();
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmCiudades_Load(object sender, EventArgs e)
        {
            try
            {
                ListarCiudades();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            if (txt_nombre.Text.Trim().Equals(""))
            {
                MessageBox.Show("Escriba el nombre de la ciudad","Aviso", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                txt_nombre.Focus();
                return;
            }
            try
            {
                CiudadCentroOperacion guardar = new CiudadCentroOperacion();
                guardar.GuardarCiudad(txt_nombre.Text);
                ListarCiudades();
                txt_nombre.Text = "";
                MessageBox.Show("Ciudad guardada correctamente","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
