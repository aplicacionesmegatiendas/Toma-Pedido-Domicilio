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
    public partial class FrmPedidosCongelados : Form
    {
        FrmPpal frm;
        bool selecionado = false;
        public FrmPedidosCongelados(FrmPpal frm)
        {
            InitializeComponent();
            this.frm = frm;
        }

        private void ObtenerPedidosCongelados()
        {
            dgv_pedidos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgv_pedidos.ColumnHeadersHeight = 20;

            Pedido pedido = new Pedido();
            dgv_pedidos.AutoGenerateColumns = false;
            dgv_pedidos.Columns[0].DataPropertyName = "pe_consecutivo";
            dgv_pedidos.Columns[1].DataPropertyName = "pe_terminal";
            dgv_pedidos.Columns[2].DataPropertyName = "pe_fecha";
            dgv_pedidos.Columns[3].DataPropertyName = "pe_co";
            dgv_pedidos.Columns[4].DataPropertyName = "pe_sucursal";
            dgv_pedidos.Columns[5].DataPropertyName = "pe_id_cliente";
            dgv_pedidos.Columns[6].DataPropertyName = "nombre";
            dgv_pedidos.Columns[7].DataPropertyName = "pe_nota";
            dgv_pedidos.Columns[8].DataPropertyName = "pe_tipo_cliente";
            dgv_pedidos.Columns[9].DataPropertyName = "pe_fuente";
            dgv_pedidos.Columns[10].DataPropertyName = "pe_ciudad_pedido";
            dgv_pedidos.Columns[11].DataPropertyName = "pe_barrio_pedido";
            dgv_pedidos.DataSource = pedido.ObtenerPedidosCongelados();
        }

        private void FrmPedidosCongelados_Load(object sender, EventArgs e)
        {
            try
            {
                ObtenerPedidosCongelados();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            if (dgv_pedidos.SelectedRows.Count == 0)
            {
                frm.nuevoToolStripMenuItem.PerformClick();
            }
            this.Close();
        }

        private void cancelarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv_pedidos.Rows.Count > 0 && dgv_pedidos.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("¿Confirma cancelar este pedido?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        Pedido pedido = new Pedido();
						pedido.CancelarPedido(Convert.ToInt32(dgv_pedidos[0, dgv_pedidos.CurrentRow.Index].Value), Convert.ToByte(dgv_pedidos[1, dgv_pedidos.CurrentRow.Index].Value));
                        
                        frm.lbl_consecutivo.Text = "";
                        frm.lbl_terminal.Text = "";
                        frm.LimpiarCo();
                        frm.LimpiarCabecera();
                        frm.LimpiarItem();
                        frm.LimpiarTotales();
                        frm.txt_id_cliente.Text = "";
                        frm.txt_id_cliente.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            if (dgv_pedidos.Rows.Count > 0)
            {
                if (dgv_pedidos.SelectedRows.Count > 0)
                {
                    try
                    {
                        frm.lbl_terminal.Text = Convert.ToString(dgv_pedidos[1, dgv_pedidos.CurrentRow.Index].Value);
                        frm.lbl_consecutivo.Text = Convert.ToString(dgv_pedidos[0, dgv_pedidos.CurrentRow.Index].Value);
						frm.lbl_consecutivo_formateado.Text = "Consecutivo: T" + frm.lbl_terminal.Text.PadLeft(2, '0') + "-" + frm.lbl_consecutivo.Text.PadLeft(4, '0');
						frm.cmb_ciudad.SelectedIndex = -1;
                        if (!Convert.ToString(dgv_pedidos[10, dgv_pedidos.CurrentRow.Index].Value).Equals(""))
                        {
                            frm.cmb_ciudad.Text = Convert.ToString(dgv_pedidos[10, dgv_pedidos.CurrentRow.Index].Value);
                        }

                        frm.cmb_barrio.SelectedIndex = -1;
                        frm.cmb_barrio.Text = "";
                        if (!Convert.ToString(dgv_pedidos[11, dgv_pedidos.CurrentRow.Index].Value).Equals(""))
                        {
                            frm.cmb_barrio.Text = Convert.ToString(dgv_pedidos[11, dgv_pedidos.CurrentRow.Index].Value);
                        }

                        frm.cmb_co.SelectedValue = dgv_pedidos[3, dgv_pedidos.CurrentRow.Index].Value;
                        frm.cmb_sucursal.SelectedValue = dgv_pedidos[4, dgv_pedidos.CurrentRow.Index].Value;

                        if (Convert.ToString(dgv_pedidos[9, dgv_pedidos.CurrentRow.Index].Value).Equals("C"))
                        {
                            frm.rdb_call_center.Checked = true;
                        }
                        else if (Convert.ToString(dgv_pedidos[9, dgv_pedidos.CurrentRow.Index].Value).Equals("W"))
                        {
                            frm.rdb_whatsapp.Checked = true;
                        }
                        else if (Convert.ToString(dgv_pedidos[9, dgv_pedidos.CurrentRow.Index].Value).Equals("E"))
                        {
                            frm.rdb_email.Checked = true;
                        }
                        else
                        {
                            frm.rdb_call_center.Checked = true;
                        }

                        if (Convert.ToString(dgv_pedidos[8, dgv_pedidos.CurrentRow.Index].Value).Trim().Equals("n"))
                        {
                            frm.rdb_nat.Checked = true;
                        }
                        if (Convert.ToString(dgv_pedidos[8, dgv_pedidos.CurrentRow.Index].Value).Trim().Equals("j"))
                        {
                            frm.rdb_juridica.Checked = true;
                        }

                        frm.txt_id_cliente.Text = Convert.ToString(dgv_pedidos[5, dgv_pedidos.CurrentRow.Index].Value);

                        Pedido pedido = new Pedido();

                        frm.btn_buscar_cliente.PerformClick();
                        object[] res = pedido.ConsultarEncabezadoPedido(frm.lbl_consecutivo.Text, frm.lbl_terminal.Text, frm.txt_id_cliente.Text);

                        frm.txt_barrio.Text = Convert.ToString(res[1]);
                        frm.txt_direccion.Text = Convert.ToString(res[2]);
                        frm.txt_telefono.Text = Convert.ToString(res[3]);
                        frm.txt_nota_pedido.Text = Convert.ToString(dgv_pedidos[7, dgv_pedidos.CurrentRow.Index].Value);

                        DataTable dt = pedido.ConsultarDetallePedido(Convert.ToInt32(dgv_pedidos[0, dgv_pedidos.CurrentRow.Index].Value), Convert.ToByte(dgv_pedidos[1, dgv_pedidos.CurrentRow.Index].Value));
                        frm.dgv_datos.Rows.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            frm.dgv_datos.Rows.Add(row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], row[10], row[11], row[12], row[13], row[14], row[15], row[19], row[16], row[17], row[18]);
                        }
                        
                        frm.SumarTotales();
                        selecionado = true;
                        

                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void FrmPedidosCongelados_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (selecionado == false)
            {
                frm.nuevoToolStripMenuItem.PerformClick();
            }
        }
    }
}
