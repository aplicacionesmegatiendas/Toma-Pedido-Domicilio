using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DomiciliosEntrecaminos
{
	public partial class FrmEstadoPedido : Form
	{
		public FrmEstadoPedido()
		{
			InitializeComponent();
		}

		private void Limpiar()
		{
			txt_numero_pedido.Text = "";
			txt_numero_pedido.Focus();
			lbl_estado.Text = "...";
		}

		private void btn_consultar_Click(object sender, EventArgs e)
		{
			Pedido pedido = new Pedido();
			lbl_estado.Text = "...";
			if (rdb_call_center.Checked)
			{
				if (txt_terminal.Text.Trim().Equals("") || txt_numero_pedido.Text.Trim().Equals(""))
				{
					MessageBox.Show("Escriba el número de la terminal y el consecutivo del pedido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}
				string estado = pedido.ConsultarEstadoPedido(Convert.ToInt32(txt_terminal.Text), Convert.ToInt32(txt_numero_pedido.Text));
				if (estado.Equals(""))
					MessageBox.Show("No se encontro estado para ese consecutivo", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				else
					lbl_estado.Text = estado;
			}
			if (rdb_tienda_virtual.Checked)
			{
				if (txt_numero_pedido.Text.Trim().Equals(""))
				{
					MessageBox.Show("Escriba el consecutivo del pedido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}
				string estado = pedido.ConsultarEstadoPedidoTercero(txt_numero_pedido.Text);
				if (estado.Equals(""))
					MessageBox.Show("No se encontro estado para ese consecutivo", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				else
					lbl_estado.Text = estado;
			}
		}

		private void rdb_call_center_CheckedChanged(object sender, EventArgs e)
		{
			if (rdb_call_center.Checked.Equals(true))
			{
				lbl_t.Enabled = true;
				txt_terminal.Enabled = true;
				txt_terminal.Text = "";
				txt_numero_pedido.Text = "";
				txt_terminal.Focus();
			}
		}

		private void rdb_tienda_virtual_CheckedChanged(object sender, EventArgs e)
		{
			if (rdb_tienda_virtual.Checked.Equals(true))
			{
				lbl_t.Enabled = false;
				txt_terminal.Enabled = false;
				txt_terminal.Text = "";
				txt_numero_pedido.Text = "";
				txt_numero_pedido.Focus();
			}
		}

		private void FrmEstadoPedido_Shown(object sender, EventArgs e)
		{
			rdb_call_center.Checked = true;
		}
	}
}
