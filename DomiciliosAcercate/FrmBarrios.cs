using System;
using System.Data;
using System.Windows.Forms;

namespace DomiciliosEntrecaminos
{
    public partial class FrmBarrios : Form
    {
        DataTable dt_barrios = null;
        public FrmBarrios()
        {
            InitializeComponent();
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

        private void ListarCiudades()
        {
            CiudadCentroOperacion ciudades = new CiudadCentroOperacion();
            cmb_ciudad.ValueMember = "ci_id";
            cmb_ciudad.DisplayMember = "ci_nombre";
            cmb_ciudad.DataSource = ciudades.ListarCiudades();
            cmb_ciudad.SelectedIndex = -1;
        }

        private void ListarBarrios()
        {
			CiudadCentroOperacion barrios = new CiudadCentroOperacion();
            dgv_barrios.AutoGenerateColumns = false;
            dgv_barrios.Columns[0].DataPropertyName = "br_id";
            dgv_barrios.Columns[1].DataPropertyName = "br_nombre";
            dgv_barrios.Columns[2].DataPropertyName = "ci_nombre";
            dgv_barrios.Columns[3].DataPropertyName = "ci_id";
            dgv_barrios.Columns[4].DataPropertyName = "br_activo";
            dgv_barrios.Columns[5].DataPropertyName = "br_cod_domicilio";
            dgv_barrios.Columns[6].DataPropertyName = "br_cod_express";
            dt_barrios = barrios.ListarBarriosGeneral();
            dgv_barrios.DataSource = dt_barrios;
        }

        private void FrmBarrios_Load(object sender, EventArgs e)
        {
            try
            {
                ListarCiudades();
                ListarBarrios();
                dgv_barrios.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            if (txt_nombre.Text.Trim().Equals("") || cmb_ciudad.SelectedIndex == -1 || txt_cod_dom.Text.Trim().Equals(""))
            {
                MessageBox.Show("Escriba el nombre del barrio y seleccione la ciudad", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
				CiudadCentroOperacion guardar = new CiudadCentroOperacion();

                guardar.GuardarBarrio(txt_nombre.Text.Trim(), Convert.ToInt32(cmb_ciudad.SelectedValue), chk_activo.Checked, txt_cod_dom.Text.Trim(), txt_cod_dom_express.Text.Trim());
                txt_buscar.Text = "";
                ListarBarrios();
                MessageBox.Show("Barrio guardado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                txt_nombre.Text = "";
                txt_nombre.Focus();
                chk_activo.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_ciudades_Click(object sender, EventArgs e)
        {
            new FrmCiudades().ShowDialog(this);
            ListarCiudades();
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_buscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dt_barrios.DefaultView.RowFilter = string.Format("br_nombre like '%{0}%' OR ci_nombre like '%{0}%'", txt_buscar.Text);
            }
            catch (Exception)
            {

            }
        }

        private void dgv_barrios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                new FrmEditarBarrio(dgv_barrios[0, dgv_barrios.CurrentRow.Index].Value.ToString(), dgv_barrios[1, dgv_barrios.CurrentRow.Index].Value.ToString(),
                                    dgv_barrios[2, dgv_barrios.CurrentRow.Index].Value.ToString(), Convert.ToString(dgv_barrios[5, dgv_barrios.CurrentRow.Index].Value),
                                     Convert.ToString(dgv_barrios[6, dgv_barrios.CurrentRow.Index].Value),Convert.ToBoolean(dgv_barrios[4, dgv_barrios.CurrentRow.Index].Value)).ShowDialog(this);
                txt_buscar.Text = "";
                ListarBarrios();
            }
            if (e.ColumnIndex==8)
            {
                if (MessageBox.Show("¿Confirma eliminar este barrio?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
                {
					CiudadCentroOperacion eliminar = new CiudadCentroOperacion();
                    eliminar.EliminarBarrio(Convert.ToInt32(dgv_barrios[0, dgv_barrios.CurrentRow.Index].Value));

                    txt_buscar.Text = "";
                    ListarBarrios();
                }
            }
        }
    }
}
