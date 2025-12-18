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
    public partial class FrmDirecciones : Form
    {
        string id;
        TextBox barrio;
        TextBox direccion;
        public FrmDirecciones(string id, TextBox barrio, TextBox direccion)
        {
            InitializeComponent();
            this.id = id;
            this.barrio = barrio;
            this.direccion = direccion;
        }

        private void ObtenerDirecciones(string id)
        {
            Pedido direcciones = new Pedido();
            dgv_direcciones.AutoGenerateColumns = false;
            dgv_direcciones.Columns["Col1"].DataPropertyName = "pe_barrio";
            dgv_direcciones.Columns["Col2"].DataPropertyName = "pe_direccion";
            dgv_direcciones.Columns["Col3"].DataPropertyName = "pe_ciudad";
            dgv_direcciones.DataSource = direcciones.ObtenerDireccionesCliente(id);
        }

        private void FrmDirecciones_Load(object sender, EventArgs e)
        {
            try
            {
                ObtenerDirecciones(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            if (dgv_direcciones.SelectedRows.Count > 0)
            {
                this.barrio.Text= dgv_direcciones[0, dgv_direcciones.CurrentRow.Index].Value.ToString().Trim();
                this.direccion.Text = dgv_direcciones[1, dgv_direcciones.CurrentRow.Index].Value.ToString().Trim();
                this.Close();
            }
        }
    }
}
