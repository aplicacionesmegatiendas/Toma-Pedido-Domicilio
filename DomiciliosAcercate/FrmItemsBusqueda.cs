using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DomiciliosEntrecaminos
{
    public partial class FrmItemsBusqueda : Form
    {
        FrmPpal frm;
        public FrmItemsBusqueda(FrmPpal frm)
        {
            InitializeComponent();
            this.frm = frm;
        }

        static string CleanInput(string strIn)
        {
            try
            {
                return Regex.Replace(strIn, @"[^\w\.@-\\*./]", " ", RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        private void ObtenerExistencia()
        {
			Configuracion configuracion = new Configuracion();
            Item item = new Item();
			try
            {
                string[] res = item.ObtenerExistencia(frm.txt_tpv.Text.Trim(), Convert.ToString(dgv_unds[0, 0].Value), Convert.ToString(frm.cmb_sucursal.SelectedValue).Trim(), Configuracion.RowidTercero);
                if (res != null)
                {
                    object[] conf = configuracion.ObtenerConfiguracion();
                    string lp = conf[6].ToString();

                    lbl_existencia.Text = res[3];
					frm.txt_existencia.Text = res[3];
					Configuracion.IdSucursalCliente = res[0];
                    if (lp.Equals(string.Empty))
                    {
						Configuracion.IdListaPrecio = res[1];
                    }
                    else
                    {
						Configuracion.IdListaPrecio = lp;
                    }
					Configuracion.IdInstalacion = res[2];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuscarItems(string criterios)
        {
			Item item = new Item();
			dgv_datos.AutoGenerateColumns = false;
            dgv_datos.Columns[0].DataPropertyName = "f120_rowid";
            dgv_datos.Columns[1].DataPropertyName = "f120_id";
            dgv_datos.Columns[2].DataPropertyName = "f120_referencia";
            dgv_datos.Columns[3].DataPropertyName = "f120_descripcion";
            dgv_datos.Columns[4].DataPropertyName = "f132_cant_existencia_1";
            dgv_datos.DataSource = item.BuscarItemsDescripcion(criterios.Trim('%'),frm.cmb_co.SelectedValue.ToString());
        }

        private void BuscarUnidades(string rowid, string rowid_tercero)
        {
			Item item = new Item();
			try
            {
                dgv_unds.AutoGenerateColumns = false;
                dgv_unds.Columns[0].DataPropertyName = "rowid_item_ext";
                dgv_unds.Columns[1].DataPropertyName = "id_und";
                dgv_unds.Columns[2].DataPropertyName = "precio_vta";
                DataTable dt = item.ObtenerUnidadesRowiditemExt(rowid, rowid_tercero);
                dgv_unds.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ObtenerPrecio(string id_lista_prec, string rowid_item_ext, string id_um)
        {
			Item item = new Item();
			try
            {
                string[] res = item.ObtenerPrecio(id_lista_prec, rowid_item_ext, id_um);
                if (res != null)
                {
                    frm.txt_precio.Text = res[1];
                    Configuracion.RowidItemListaPrecio = res[0];
                }
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

        private void FrmItemsBusqueda_Load(object sender, EventArgs e)
        {
            dgv_datos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgv_datos.ColumnHeadersHeight = 25;

            dgv_unds.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgv_unds.ColumnHeadersHeight = 25;
        }

        private void dgv_datos_SelectionChanged(object sender, EventArgs e)
        {
            dgv_unds.DataSource = null;
            if (dgv_datos.SelectedRows.Count > 0)
            {
                try
                {
                    Configuracion.RowidItem = Convert.ToString(dgv_datos[0, dgv_datos.CurrentRow.Index].Value);
                    BuscarUnidades(Configuracion.RowidItem, Configuracion.RowidTercero);
                    ObtenerExistencia();
					////////////PARA OBTENER EL PRECIO DE VENTA REAL/////////////
					Item item = new Item();
					for (int i = 0; i < dgv_unds.RowCount; i++)
                    {
                        string[] res = item.ObtenerPrecio(Configuracion.IdListaPrecio, dgv_unds[0, i].Value.ToString(), dgv_unds[1, i].Value.ToString());
                        dgv_unds[2, i].Value = res[1];
                    }
                    /////////////////////////////////////////////////////////////
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            if (dgv_datos.RowCount > 0 && dgv_datos.SelectedRows.Count > 0 && dgv_unds.SelectedRows.Count > 0)
            {
                if (Convert.ToDecimal(dgv_unds[2, dgv_unds.CurrentRow.Index].Value) > 0)
                {
                    frm.txt_id.Text = Convert.ToString(dgv_datos[1, dgv_datos.CurrentRow.Index].Value);
                    frm.txt_buscar.Text= Convert.ToString(dgv_datos[1, dgv_datos.CurrentRow.Index].Value);
                    frm.txt_referencia.Text = Convert.ToString(dgv_datos[2, dgv_datos.CurrentRow.Index].Value);
                    frm.txt_descripcion.Text = CleanInput(Convert.ToString(dgv_datos[3, dgv_datos.CurrentRow.Index].Value)).ToUpper().Replace('Á', 'A').Replace('É', 'E').Replace('Í', 'I').Replace('Ó', 'O').Replace('Ú', 'U').Replace('Ñ', 'N'); ;
                    frm.txt_unidad.Text = Convert.ToString(dgv_unds[1, dgv_unds.CurrentRow.Index].Value);

                    Configuracion.RowidItemExt = Convert.ToString(dgv_unds[0, dgv_unds.CurrentRow.Index].Value);
					Configuracion.UnidadMedida = Convert.ToString(dgv_unds[1, dgv_unds.CurrentRow.Index].Value);

                    //ObtenerExistencia(frm.txt_tpv.Text.Trim(), Metodos.RowidItemExt, Convert.ToString(frm.cmb_sucursal.SelectedValue).Trim(), Metodos.RowidTercero);//PASO 3

                    ObtenerPrecio(Configuracion.IdListaPrecio, Configuracion.RowidItemExt, Configuracion.UnidadMedida);

                    this.DialogResult = DialogResult.OK;

                    this.Close();
                }
                else
                {
                    MessageBox.Show("La unidad seleccionada no tiene un precio valido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgv_unds.Focus();
                }
            }
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                BuscarItems(txt_buscar.Text.Trim('%'));
                dgv_datos.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void txt_buscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (!txt_buscar.Text.Equals(""))
            {
                if (e.KeyCode.Equals(Keys.Enter))
                {
                    btn_buscar.PerformClick();
                    dgv_datos.Focus();
                }
            }
        }

        private void txt_buscar_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (!txt_buscar.Text.Equals(""))
            {
                if (e.KeyCode.Equals(Keys.Tab))
                {
                    btn_buscar.PerformClick();
                    dgv_datos.Focus();
                }
            }
        }

        private void dgv_datos_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                SelectNextControl(dgv_datos, true, true, true, true);
            }
        }

        private void dgv_unds_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                SelectNextControl(dgv_unds, true, true, true, true);
            }
        }

        private void dgv_unds_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                btn_aceptar.PerformClick();
            }
        }
    }
}
