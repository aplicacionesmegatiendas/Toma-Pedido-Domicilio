using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DomiciliosEntrecaminos
{
    public partial class FrmPedidosEnviados : Form
    {
        DataSet ds = null;
        string terminal;
        public FrmPedidosEnviados(string terminal)
        {
            InitializeComponent();
            this.terminal = terminal;
        }

        private void SumarTotales()
        {
            decimal total_val_bruto = 0;
            decimal total_val_dscto = 0;
            decimal total_val_impuesto = 0;
            decimal total_neto = 0;

            txt_nro_art.Text = dgv_datos.RowCount.ToString();

            for (int i = 0; i < dgv_datos.RowCount; i++)
            {
                total_val_bruto += Convert.ToDecimal(dgv_datos[10, i].Value);
                total_val_dscto += Convert.ToDecimal(dgv_datos[11, i].Value);
                total_val_impuesto += Convert.ToDecimal(dgv_datos[12, i].Value);
                total_neto += Convert.ToDecimal(dgv_datos[13, i].Value);
            }
            txt_val_bruto.Text = total_val_bruto.ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
            txt_descuento.Text = total_val_dscto.ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
            txt_impuestos.Text = total_val_impuesto.ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
            txt_val_neto.Text = total_neto.ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
        }

        private void ConsultarPedidos(string fecha_ini, string fecha_fin)
        {
            Pedido pedido = new Pedido();

            ds = pedido.ConsultarPedidos(fecha_ini, fecha_fin, Convert.ToByte(lbl_terminal.Text));
            if (ds.Tables.Count > 0)
            {
                _bindingSource_pedidos.DataSource = ds;//Se le asigna al objeto bindingSource1 como origen de datos el dataset ds.
                _bindingSource_pedidos.DataMember = "Pedidos";//Se le indica que debe usar la tabla frv_sugeridos.

                _bindingSource_detalle.DataSource = _bindingSource_pedidos;//Se le asigna al objeto bindingSource2 como origen de datos el objeto bindingSource1
                _bindingSource_detalle.DataMember = "rel1";//Se le indica que debe usar la relación rel para que muestre los datos del detalle que corresponde al sugerido seleccionado. 

                txt_fecha.DataBindings.Clear();
                txt_fecha.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_fecha"));
                lbl_nro_pedido.DataBindings.Clear();
                lbl_nro_pedido.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_numero"));
                lbl_consecutivo.DataBindings.Clear();
                lbl_consecutivo.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_consecutivo"));
                txt_id_cliente.DataBindings.Clear();
                txt_id_cliente.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_id_cliente"));
                txt_nombre.DataBindings.Clear();
                txt_nombre.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_nombre_cliente"));
                txt_apellidos.DataBindings.Clear();
                txt_apellidos.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_apellidos_cliente"));
                txt_email.DataBindings.Clear();
                txt_email.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_email"));
                txt_telefono.DataBindings.Clear();
                txt_telefono.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_telefono"));
                txt_celular.DataBindings.Clear();
                txt_celular.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_celular"));
                txt_direccion.DataBindings.Clear();
                txt_direccion.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_direccion"));
                txt_barrio.DataBindings.Clear();
                txt_barrio.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_barrio"));
                txt_ciudad.DataBindings.Clear();
                txt_ciudad.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_ciudad"));
                txt_departamento.DataBindings.Clear();
                txt_departamento.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_departamento"));
                txt_pais.DataBindings.Clear();
                txt_pais.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_pais"));

                txt_nota_pedido.DataBindings.Clear();
                txt_nota_pedido.DataBindings.Add(new Binding("Text", _bindingSource_pedidos, "pe_nota"));

                dgv_datos.AutoGenerateColumns = false;
                dgv_datos.DataSource = _bindingSource_detalle;
                dgv_datos.Columns[0].DataPropertyName = "dp_item";
                dgv_datos.Columns[1].DataPropertyName = "dp_referencia";
                dgv_datos.Columns[2].DataPropertyName = "dp_descripcion";
                dgv_datos.Columns[3].DataPropertyName = "dp_um";
                dgv_datos.Columns[4].DataPropertyName = "dp_pvp";
                dgv_datos.Columns[5].DataPropertyName = "dp_dscto_porc";
                dgv_datos.Columns[6].DataPropertyName = "dp_dscto_val";
                dgv_datos.Columns[7].DataPropertyName = "dp_impuesto";
                dgv_datos.Columns[8].DataPropertyName = "dp_val_bruto_pvp";
                dgv_datos.Columns[9].DataPropertyName = "dp_cantidad";
                dgv_datos.Columns[10].DataPropertyName = "dp_val_bruto";
                dgv_datos.Columns[11].DataPropertyName = "dp_val_dscto";
                dgv_datos.Columns[12].DataPropertyName = "dp_val_impuesto";
                dgv_datos.Columns[13].DataPropertyName = "dp_total";
                dgv_datos.Columns[14].DataPropertyName = "dp_nota";

                SumarTotales();
            }
            else
            {
                MessageBox.Show("No hay datos disponibles", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            if (dtp_fecha_ini.Value.Date > dtp_fecha_fin.Value.Date)
            {
                MessageBox.Show("La fecha inicial debe ser menor a la fecha final.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.Cursor = Cursors.WaitCursor;

            btn_buscar.Enabled = false;
            btn_cerrar.Enabled = false;
            try
            {
                ConsultarPedidos(dtp_fecha_ini.Value.Date.ToString("yyyyMMdd"), dtp_fecha_fin.Value.Date.ToString("yyyyMMdd"));
				lbl_consecutivo_formateado.Text = "Consecutivo: T" + lbl_terminal.Text.PadLeft(2, '0') + "-" + lbl_consecutivo.Text.PadLeft(4, '0');
			}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
            this.Cursor = Cursors.Default;
            btn_buscar.Enabled = true;
            btn_cerrar.Enabled = true;
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmPedidosEnviados_Load(object sender, EventArgs e)
        {
            dgv_datos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgv_datos.ColumnHeadersHeight = 25;

            dgv_datos.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            this.dgv_datos.RowHeadersWidth = 25;

            try
            {
                lbl_terminal.Text = terminal;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _bindingSource_detalle_ListChanged(object sender, ListChangedEventArgs e)
        {
            try
            {
				//decimal total_val_bruto = 0;
				//decimal total_val_dscto = 0;
				//decimal total_val_impuesto = 0;
				//decimal total_neto = 0;

				//txt_nro_art.Text = dgv_datos.RowCount.ToString();

				//for (int i = 0; i < dgv_datos.RowCount; i++)
				//{
				//    total_val_bruto += Convert.ToDecimal(dgv_datos[10, i].Value);
				//    total_val_dscto += Convert.ToDecimal(dgv_datos[11, i].Value);
				//    total_val_impuesto += Convert.ToDecimal(dgv_datos[12, i].Value);
				//    total_neto += Convert.ToDecimal(dgv_datos[13, i].Value);
				//}
				//txt_val_bruto.Text = total_val_bruto.ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
				//txt_descuento.Text = total_val_dscto.ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
				//txt_impuestos.Text = total_val_impuesto.ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
				//txt_val_neto.Text = total_neto.ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
				lbl_consecutivo_formateado.Text = "Consecutivo: T" + lbl_terminal.Text.PadLeft(2, '0') + "-" + lbl_consecutivo.Text.PadLeft(4, '0');
				SumarTotales();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
