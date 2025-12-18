using System;
using System.Windows.Forms;
using System.Data;
using System.Drawing;

namespace DomiciliosEntrecaminos
{
    public partial class FrmDetalleCobertura : Form
    {
        DataTable dt_detalle = null;
        public FrmDetalleCobertura()
        {
            InitializeComponent();
        }

        private void ListarCoberturas()
        {
			CiudadCentroOperacion detalle = new CiudadCentroOperacion();
            dgv_detalle.AutoGenerateColumns = false;
            dgv_detalle.Columns[0].DataPropertyName = "zc_id";
            dgv_detalle.Columns[1].DataPropertyName = "zc_co";
            dgv_detalle.Columns[2].DataPropertyName = "ci_nombre";
            dgv_detalle.Columns[3].DataPropertyName = "br_nombre";
            dt_detalle=detalle.ObtenerCoberturaGeneral();
            dgv_detalle.DataSource = dt_detalle;
        }

        private void FrmDetalleCobertura_Load(object sender, EventArgs e)
        {
            
            try
            {
                ListarCoberturas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgv_detalle_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgv_detalle.SelectedRows.Count>0)
            {
                if (e.KeyCode==Keys.Delete)
                {
                    if (MessageBox.Show("¿Confirma eliminar esta cobertura?","Confirmación",MessageBoxButtons.YesNo,MessageBoxIcon.Question).Equals(DialogResult.Yes))
                    {
						CiudadCentroOperacion eliminar = new CiudadCentroOperacion();
                        eliminar.EliminarCobertura(Convert.ToInt32(dgv_detalle[0, dgv_detalle.CurrentRow.Index].Value));
                        ListarCoberturas();
                    }
                }
            }
        }

        private void txt_buscar_TextChanged(object sender, EventArgs e)
        {
            dt_detalle.DefaultView.RowFilter = string.Format("zc_co like '%{0}%' OR ci_nombre like '%{0}%' OR br_nombre like '%{0}%'", txt_buscar.Text);
        }

        private void dgv_detalle_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
