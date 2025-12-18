using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace DomiciliosEntrecaminos
{
    public partial class FrmCobertura : Form
    {
        public FrmCobertura()
        {
            InitializeComponent();
        }

        public void ListarCentrosOperacion()
        {
            Configuracion configuracion = new Configuracion();
            CiudadCentroOperacion ciudad_centro_operacion=new CiudadCentroOperacion();
            object[] conf = configuracion.ObtenerConfiguracion();
            string cos = conf[2].ToString();

            DataTable dt = ciudad_centro_operacion.ListarCentrosOperacion(cos);

            cmb_co.DataSource = dt;
            cmb_co.DisplayMember = "f285_descripcion";
            cmb_co.ValueMember = "f285_id";

            cmb_co.SelectedIndex = -1;
        }

        private void ListarCiudades()
        {
			CiudadCentroOperacion ciudades = new CiudadCentroOperacion();
            cmb_ciudad.ValueMember = "ci_id";
            cmb_ciudad.DisplayMember = "ci_nombre";
            cmb_ciudad.DataSource = ciudades.ListarCiudades();
            cmb_ciudad.SelectedIndex = -1;
        }

        private void ListarBarriosDisponibles(int ciudad)
        {
			CiudadCentroOperacion barrios = new CiudadCentroOperacion();
            lbx_disponibles.DataSource = barrios.ObtenerBarriosDisponibles(ciudad);
            lbx_disponibles.DisplayMember = "br_nombre";
            lbx_disponibles.ValueMember = "br_id";
            lbx_disponibles.ClearSelected();
        }

        private void FrmCobertura_Load(object sender, EventArgs e)
        {
            ListarCentrosOperacion();
            ListarCiudades();
            cmb_ciudad.SelectedIndexChanged += Cmb_ciudad_SelectedIndexChanged;
        }

        private void Cmb_ciudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListarBarriosDisponibles(Convert.ToInt32(cmb_ciudad.SelectedValue));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_detalle_Click(object sender, EventArgs e)
        {
            try
            {
                new FrmDetalleCobertura().ShowDialog();
                ListarBarriosDisponibles(Convert.ToInt32(cmb_ciudad.SelectedValue));
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

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            if (cmb_co.SelectedIndex==-1 || cmb_ciudad.SelectedIndex==-1 || lbx_disponibles.CheckedItems.Count==0)
            {
                MessageBox.Show("Seleccione el centro de operación, la ciudad y los barrios","Aviso",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            try
            {
                List<int> barrios = new List<int>();
                foreach (object itemChecked in lbx_disponibles.CheckedItems)
                {
                    DataRowView castedItem = itemChecked as DataRowView;
                    int id = (int) castedItem["br_id"];
                    barrios.Add(id);
                }

				CiudadCentroOperacion guardar = new CiudadCentroOperacion();
                guardar.GuardarCobertura(barrios, cmb_co.Text, Convert.ToInt32(cmb_co.SelectedValue));
                
                ListarBarriosDisponibles(Convert.ToInt32(cmb_ciudad.SelectedValue));
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
