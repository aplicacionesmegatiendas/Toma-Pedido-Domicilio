using System;
using System.Data;
using System.Windows.Forms;

namespace DomiciliosEntrecaminos
{
    public partial class FrmUltimoPedido : Form
    {
        DataTable dt = null;

        string identificacion = "";
        string rowidtpv = "";
        string idsucperfil = "";
        string idco;

        string rowiditemext = "";
        string idlistaprecio = "";
        string idsucursalcli = "";

        FrmPpal frm;

        public FrmUltimoPedido(string identificacion, string idsucperfil, string rowidtpv, string idco, FrmPpal frm)
        {
            InitializeComponent();
            this.identificacion = identificacion;
            this.idsucperfil = idsucperfil;
            this.rowidtpv = rowidtpv;
            this.idco = idco;

            this.frm = frm;
        }
        private void ObtenerUltimoPedido(string identificacion, DateTime fecha)
        {
            Pedido pedido = new Pedido();
            dt = pedido.ObtenerUltimoPedido(identificacion,fecha);
            if (dt.Rows.Count > 0)
            {
                lbl_fecha.Text = Convert.ToDateTime(dt.Rows[0][0]).ToString("yyyy-MM-dd");
                lbl_consecutivo.Text = dt.Rows[0][2].ToString();
                lbl_numero.Text = dt.Rows[0][4].ToString();
                lbl_terminal.Text = dt.Rows[0][3].ToString();
            }
            dgv_datos.AutoGenerateColumns = false;
            dgv_datos.Columns[0].DataPropertyName = "dp_item";
            dgv_datos.Columns[1].DataPropertyName = "dp_referencia";
            dgv_datos.Columns[2].DataPropertyName = "dp_descripcion";
            dgv_datos.Columns[3].DataPropertyName = "dp_um";
            dgv_datos.Columns[4].DataPropertyName = "dp_pvp";
            dgv_datos.Columns[5].DataPropertyName = "dp_dscto_porc";
            dgv_datos.Columns[6].DataPropertyName = "dp_dscto_val";
            dgv_datos.Columns[9].DataPropertyName = "dp_cantidad";
            dgv_datos.Columns[13].DataPropertyName = "dp_total";
            dgv_datos.Columns[14].DataPropertyName = "dp_val_unit";

            foreach (DataGridViewColumn col in dgv_datos.Columns)
            {
                if (col.Index >= 0 && col.Index <= 3)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            dgv_datos.DataSource = dt;

            if (dgv_datos.Rows.Count > 0)
            {
                dgv_datos.Rows[0].Selected = true;
            }
        }

        private void ObtenerFechasUltimoPedido(string desde, string hasta, string identificacion)
        {
            Pedido pedido = new Pedido();
            lbx_fechas.DataSource = null;
            lbx_fechas.Items.Clear();
            System.Collections.Generic.List <string> lista = pedido.ObtenerFechasUltimoPedido(desde, hasta, identificacion);
            lbx_fechas.ClearSelected();
            lbx_fechas.SelectedIndexChanged += Lbx_fechas_SelectedIndexChanged;
            if (lista!=null)
            {
                lbx_fechas.DataSource = lista;
            }
        }

        private void Lbx_fechas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ObtenerUltimoPedido(identificacion, Convert.ToDateTime(lbx_fechas.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Limpiar()
        {
            lbl_item.Text = "";
            lbl_item.Tag = null;
            lbl_referencia.Text = "";
            lbl_descripcion.Text = "";
            cmb_unidades.DataSource = null;
            cmb_unidades.Items.Clear();
            lbl_existencia.Text = "";
            lbl_pvp.Text = "";
            lbl_dscto.Text = "";
            lbl_val_unit.Text = "";
            rowiditemext = "";
            idlistaprecio = "";
            idsucursalcli = "";
        }

        private void BuscarUnidades(string rowid, string rowid_tercero)
        {
            Item item = new Item();
            DataTable dt = item.ObtenerUnidadesRowiditemExt(rowid, rowid_tercero);
            rowiditemext = dt.Rows[0][0].ToString();
            cmb_unidades.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                cmb_unidades.Items.Add(row[1]);
            }
        }

        private void ObtenerExistencia(string rowid_tpv, string rowid_item_ext, string id_sucursal_perfil, string rowid_tercero)
        {
			Item item = new Item();
			string[] res = item.ObtenerExistencia(rowid_tpv, rowid_item_ext, id_sucursal_perfil, rowid_tercero);
            if (res != null)
            {
                lbl_existencia.Text = res[3];
                idsucursalcli = res[0];
                idlistaprecio = res[1];
            }
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmUltimoPedido_Load(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmb_unidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                ObtenerExistencia(rowidtpv, rowiditemext, idsucperfil, Configuracion.RowidTercero);
                Item item = new Item();
                decimal pvp = 0;
                decimal dscto = 0;
                decimal val_dscto = 0;
                decimal val_unit = 0;
                string[] res1 = item.ObtenerPrecio(idlistaprecio, rowiditemext, cmb_unidades.Text.Trim());
                if (res1 != null)
                {
                    lbl_pvp.Text = Convert.ToDecimal(res1[1]).ToString("N");
                    pvp = Convert.ToDecimal(res1[1]);
                }
                string[] res2 = item.ObtenerDescuento(rowiditemext, idsucursalcli, idlistaprecio, "1", identificacion, Configuracion.RowidTercero, idco);
                if (res2 != null)
                {
                    if (Convert.ToDecimal(res2[0]) > 0)
                    {
                        lbl_dscto.Text = res2[0] + "%";
                        dscto = Convert.ToDecimal(res2[0]);
                    }
                    if (Convert.ToDecimal(res2[1]) > 0)
                    {
                        lbl_dscto.Text = res2[1];
                        dscto = Convert.ToDecimal(res2[1]);
                    }
                }

                if (dscto>0)
                {
                    if (lbl_dscto.Text.Contains("%"))
                    {
                        val_dscto = (pvp * dscto) / 100;
                        val_unit = pvp - val_dscto;
                    }
                }
                else
                {
                    val_unit = pvp;
                }
                lbl_val_unit.Text = val_unit.ToString("N");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor = Cursors.Default;
        }

        private void btn_seleccionar_Click(object sender, EventArgs e)
        {
            if (dgv_datos.Rows.Count > 0)
            {
                frm.txt_buscar.Text = dgv_datos[0, dgv_datos.CurrentRow.Index].Value.ToString();
				Configuracion.ultimo = true;
                this.Hide();

                frm.btn_busar_item.PerformClick();
                this.Show();
            }
        }

        private void txt_buscar_TextChanged(object sender, EventArgs e)
        {
            dt.DefaultView.RowFilter = "dp_descripcion Like'%" + txt_buscar.Text + "%'";
        }

        private void dgv_datos_SelectionChanged(object sender, EventArgs e)
        {
            Limpiar();
            try
            {
                Item item = new Item();
                string[] res = null;
                if (dgv_datos.Rows.Count > 0)
                {
                    res = item.BuscarItemsId(dgv_datos[0, dgv_datos.CurrentRow.Index].Value.ToString());
                    Limpiar();
                    if (res != null)
                    {
                        lbl_item.Text = res[1].Trim();
                        lbl_item.Tag = res[0].Trim();
                        lbl_referencia.Text = res[2].Trim();
                        lbl_descripcion.Text = res[3].ToUpper().Trim();
                        BuscarUnidades(res[0].Trim(), Configuracion.RowidTercero);
                        cmb_unidades.Text = Convert.ToString(dgv_datos[3, dgv_datos.CurrentRow.Index].Value).Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            try
            {
                ObtenerFechasUltimoPedido(dtp_desde.Value.Date.ToString("yyyyddMM"), dtp_hasta.Value.Date.ToString("yyyyddMM"), identificacion);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
