using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;

namespace DomiciliosEntrecaminos
{
	public partial class FrmPpal : Form
	{
		public static bool reset = false;
		DataTable dtDatos = new DataTable();
		DataTable dtBarrios = new DataTable();
		string fecha_ini_promo = "";
		string fecha_fin_promo = "";
		string desc_promo = "";
		string consecutivo_guardado = "";

		bool precio_dom = false;

		public FrmPpal()
		{
			InitializeComponent();
			//CheckForIllegalCrossThreadCalls = false;
		}

		public void SumarTotales()
		{
			CiudadCentroOperacion metodos = new CiudadCentroOperacion();
			string[] cod_dom = metodos.ObternerCodigoDomicilioBarrio(Convert.ToInt32(cmb_barrio.SelectedValue));
			int cant_cod_dom = 0;
			if (cod_dom != null)
			{
				for (int i = 0; i < dgv_datos.Rows.Count; i++)
				{
					for (int j = 0; j < cod_dom.Length; j++)
					{
						if (dgv_datos[0, i].Value.ToString().Trim().Equals(cod_dom[j].Trim()))
						{
							cant_cod_dom++;
						}
					}
				}
			}

			txt_nro_art.Text = (dgv_datos.Rows.Count - cant_cod_dom).ToString();

			decimal total_val_bruto = 0;
			decimal total_val_dscto = 0;
			decimal total_val_impuesto = 0;
			decimal total_neto = 0;

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
			txt_val_neto.Tag = total_neto;
		}

		public void LimpiarItem()
		{
			txt_id.Text = "";
			txt_referencia.Text = "";
			txt_descripcion.Text = "";
			txt_unidad.Text = "";
			txt_precio.Text = "";
			txt_existencia.Text = "";

			txt_cantidad.Text = "";

			txt_porc_dscto.Text = "";
			txt_val_dscto.Text = "";

			txt_impuesto.Text = "";

			txt_nota_item.Text = "";

			Configuracion.RowidItem = "";
			Configuracion.RowidItemExt = "";
			Configuracion.UnidadMedida = "";
			Configuracion.IdSucursalCliente = "";
			Configuracion.IdListaPrecio = "";
			Configuracion.IdInstalacion = "";
			Configuracion.RowidItemListaPrecio = "";

			fecha_ini_promo = "";
			fecha_fin_promo = "";
			desc_promo = "";

			lbl_tit_dscto.ForeColor = Color.Black;
			lbl_tit_dscto.Cursor = Cursors.Default;
		}

		public void LimpiarCo()
		{
			cmb_ciudad.SelectedIndex = -1;
			cmb_ciudad.Tag = null;
			cmb_barrio.SelectedIndex = -1;
			txt_buscar_barrio.Text = "";
			dtBarrios = null;
			cmb_barrio.Text = "";
			Configuracion.RowidTercero = "";
			lbl_co.Text = "...";
			cmb_co.SelectedValue = -1;
			txt_cliente.Text = "";
			txt_desc_cliente.Text = "";
			cmb_sucursal.SelectedIndex = -1;
			txt_tpv.Text = "";
		}

		public void LimpiarCabecera()
		{
			txt_fecha.Text = DateTime.Now.Date.ToShortDateString();
			txt_id_cliente.Tag = null;
			txt_nombre.Text = "";
			txt_apellidos.Text = "";
			txt_direccion.Text = "";
			txt_telefono.Text = "";
			txt_celular.Text = "";
			txt_email.Text = "";
			txt_barrio.Text = "";
			txt_ciudad.Text = "";
			txt_ciudad.Tag = null;
			txt_departamento.Text = "";
			txt_pais.Text = "";
			txt_nota_pedido.Text = "";
			btn_cargar_ultimo.Enabled = false;
		}

		public void LimpiarTotales()
		{
			txt_nro_art.Text = "0";
			txt_val_bruto.Text = "0";
			txt_descuento.Text = "0";
			txt_impuestos.Text = "0";
			txt_val_neto.Text = "0";
			txt_val_neto.Tag = null;
			dgv_datos.Rows.Clear();
			cmb_medio_pago.SelectedIndex = 0;
			cmb_recoge_tienda.SelectedIndex = 1;
			dtp_fecha_entrega.Value = DateTime.Now;
		}

		static string RemoveDiacritics(string text)
		{
			var normalizedString = text.Normalize(NormalizationForm.FormD);
			var stringBuilder = new StringBuilder();

			foreach (var c in normalizedString)
			{
				var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

				if (unicodeCategory != UnicodeCategory.NonSpacingMark)
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
		}

		public void ListarCentrosOperacion()
		{
			Configuracion configuracion = new Configuracion();
			CiudadCentroOperacion ciudad_centro_operacion = new CiudadCentroOperacion();
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

		private void ListarBarriosCiudad(int ciudad)
		{
			CiudadCentroOperacion barrios = new CiudadCentroOperacion();
			cmb_barrio.ValueMember = "br_id";
			cmb_barrio.DisplayMember = "br_nombre";
			dtBarrios = barrios.ListarBarriosCiudad(ciudad);
			cmb_barrio.DataSource = dtBarrios;
			cmb_barrio.SelectedIndex = -1;
		}

		private DialogResult ObtenerUnidades(string rowid, string rowid_tercero)
		{
			Item item = new Item();
			DialogResult r = DialogResult.OK;
			try
			{
				DataTable dt = item.ObtenerUnidadesRowiditemExt(rowid, rowid_tercero);
				if (dt.Rows.Count > 1)
				{
					FrmUnidades _FrmUnidades = new FrmUnidades(this);
					_FrmUnidades.dgv_unds.AutoGenerateColumns = false;
					_FrmUnidades.dgv_unds.Columns[0].DataPropertyName = "rowid_item_ext";
					_FrmUnidades.dgv_unds.Columns[1].DataPropertyName = "id_und";
					_FrmUnidades.dgv_unds.Columns[2].DataPropertyName = "precio_vta";
					_FrmUnidades.dgv_unds.DataSource = dt;
					_FrmUnidades.ShowDialog(this);
					r = _FrmUnidades.DialogResult;
				}
				else
				{
					Configuracion.RowidItemExt = Convert.ToString(dt.Rows[0][0]);
					Configuracion.UnidadMedida = Convert.ToString(dt.Rows[0][1]);
					txt_unidad.Text = Configuracion.UnidadMedida;

					txt_precio.Text = Convert.ToString(dt.Rows[0][2]);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return r;
		}

		private void ObtenerExistencia(string rowid_tpv, string rowid_item_ext, string id_sucursal_perfil, string rowid_tercero)
		{
			Item item = new Item();
			try
			{
				string[] res = item.ObtenerExistencia(rowid_tpv, rowid_item_ext, id_sucursal_perfil, rowid_tercero);
				if (res != null)
				{
					Configuracion configuracion = new Configuracion();
					object[] conf = configuracion.ObtenerConfiguracion();
					string lp = conf[6].ToString();

					txt_existencia.Text = res[3];
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

		private void ObtenerPrecio(string id_lista_prec, string rowid_item_ext, string id_um)
		{
			Item item = new Item();
			try
			{
				string[] res = item.ObtenerPrecio(id_lista_prec, rowid_item_ext, id_um);

				if (res != null)
				{
					txt_precio.Text = res[1];
					Configuracion.RowidItemListaPrecio = res[0];
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private string[] ObtenerDescuento(string rowid_item_ext, string id_sucursal_cli, string id_lista_precios, string cantidad, string id_cliente_pos, string rowid_tercero)
		{
			Item item = new Item();
			string[] res = null;
			try
			{
				res = item.ObtenerDescuento(rowid_item_ext, id_sucursal_cli, id_lista_precios, cantidad, id_cliente_pos, rowid_tercero, Configuracion.IdCoDcoto);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return res;
		}

		private string[] ObtenerImpuesto(string rowid_item, string id_co, string rowid_tercero, string id_sucursal, string rowid_item_lista_precio)
		{
			Item item = new Item();
			string[] res = null;
			try
			{
				res = item.ObtenerImpuesto(rowid_item, id_co, rowid_tercero, id_sucursal, rowid_item_lista_precio);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return res;
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

		private Boolean email_bien_escrito(String email)
		{
			String expresion;
			expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
			if (Regex.IsMatch(email, expresion))
			{
				if (Regex.Replace(email, expresion, String.Empty).Length == 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		private void FrmPpal_Load(object sender, EventArgs e)
		{
			dgv_datos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
			this.dgv_datos.ColumnHeadersHeight = 25;

			dgv_datos.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
			this.dgv_datos.RowHeadersWidth = 25;

			this.Text += " V " + Application.ProductVersion.ToString();

			txt_fecha.Text = DateTime.Now.Date.ToShortDateString();

			cmb_medio_pago.SelectedIndex = 0;
			try
			{
				FrmLogin _FrmLogin = new FrmLogin(lbl_terminal);
				_FrmLogin.ShowDialog(this);

				Configuracion.Terminal = Convert.ToByte(lbl_terminal.Text);
				ListarCiudades();
				cmb_ciudad.SelectedIndexChanged += Cmb_ciudad_SelectedIndexChanged;

				ListarCentrosOperacion();
				cmb_co.SelectedValueChanged += cmb_co_SelectedValueChanged;

				Terminal terminal = new Terminal();
				Pedido pedido = new Pedido();
				lbl_consecutivo.Text = terminal.ObtenerConsecutivo(Convert.ToByte(lbl_terminal.Text)).ToString();
				lbl_consecutivo_formateado.Text = "Consecutivo: T" + lbl_terminal.Text.PadLeft(2, '0') + "-" + lbl_consecutivo.Text.PadLeft(4, '0');
				lbl_agente.Text = Configuracion.DescripcionUsuario.ToUpper();
				cmb_recoge_tienda.SelectedIndex = 1;
				pedido.CancelarPedido();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				reset = true;
				Application.Exit();
			}
		}

		private void Cmb_ciudad_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				dtBarrios = null;
				txt_buscar_barrio.Text = "";
				ListarBarriosCiudad(Convert.ToInt32(cmb_ciudad.SelectedValue));
				cmb_barrio.SelectedIndexChanged += Cmb_barrio_SelectedIndexChanged;
				txt_buscar_barrio.Focus();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void Cmb_barrio_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				cmb_co.SelectedIndex = -1;
				txt_cliente.Text = "";
				txt_desc_cliente.Text = "";
				cmb_sucursal.SelectedIndex = -1;
				txt_tpv.Text = "";
				CiudadCentroOperacion co = new CiudadCentroOperacion();
				object[] res = co.ObternerCoBarrio(Convert.ToInt32(cmb_barrio.SelectedValue));
				if (res != null)
				{
					cmb_co.SelectedValue = res[0];
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		void cmb_co_SelectedValueChanged(object sender, EventArgs e)
		{
			lbl_co.Text = "...";
			if (cmb_co.SelectedIndex >= 0)
			{
				CiudadCentroOperacion datos = new CiudadCentroOperacion();
				Configuracion.RowidTercero = "";
				Configuracion.IdCoDcoto = "";

				LimpiarCabecera();
				LimpiarItem();
				LimpiarTotales();

				txt_cliente.Text = "";
				txt_desc_cliente.Text = "";
				cmb_sucursal.DataSource = null;
				txt_tpv.Text = "";

				txt_id_cliente.Enabled = false;
				grb_fuente.ForeColor = Color.Black;
				rdb_call_center.Checked = false;
				rdb_whatsapp.Checked = false;
				rdb_email.Checked = false;

				btn_buscar_cliente.Text = "BUSCAR C&LIENTE";
				txt_id_cliente.Text = "";

				try
				{
					dtDatos = datos.ObtenerDatosCentroOperacion(cmb_co.SelectedValue.ToString());
					if (dtDatos.Rows.Count > 0)
					{
						txt_cliente.Text = Convert.ToString(dtDatos.Rows[0][2]);
						Configuracion.RowidTercero = Convert.ToString(dtDatos.Rows[0][1]);
						Configuracion.IdCoDcoto = Convert.ToString(cmb_co.SelectedValue);
						txt_desc_cliente.Text = Convert.ToString(dtDatos.Rows[0][3]);
						cmb_sucursal.DataSource = dtDatos;
						cmb_sucursal.DisplayMember = "f201_descripcion_sucursal";
						cmb_sucursal.ValueMember = "f9750_id_suc_cliente";

						grb_fuente.ForeColor = Color.FromArgb(217, 83, 79);

						if (cmb_sucursal.Items.Count > 0)
						{
							cmb_sucursal.SelectedIndex = 0;
						}
					}
					lbl_co.Text = cmb_co.Text + " [" + cmb_barrio.Text + " - " + cmb_ciudad.Text + "]";
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void btn_buscar_Click(object sender, EventArgs e)
		{
			if (txt_id_cliente.Text.Trim().Equals(""))
			{
				MessageBox.Show("Escriba la cedula del cliente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txt_id_cliente.Focus();
				return;
			}

			LimpiarCabecera();

			if (rdb_call_center.Checked == false && rdb_whatsapp.Checked == false && rdb_email.Checked == false)
			{
				MessageBox.Show("Seleccione el origen del pedido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			this.Cursor = Cursors.WaitCursor;
			try
			{
				CiudadCentroOperacion ciudad_centro_operacion = new CiudadCentroOperacion();
				Cliente cliente = new Cliente();
				string tipo = "";
				if (rdb_nat.Checked == true)
				{
					tipo = "n";
				}
				else if (rdb_juridica.Checked == true)
				{
					tipo = "j";
				}

				string[] res = cliente.ObtenerDatosCliente(txt_id_cliente.Text, tipo);
				if (res != null)
				{
					txt_fecha.Text = DateTime.Now.Date.ToShortDateString();
					txt_id_cliente.Tag = res[0].Trim();

					if (tipo.Equals("n"))
					{
						txt_nombre.Text = RemoveDiacritics(res[1].ToUpper());
						txt_apellidos.Text = RemoveDiacritics(res[2].ToUpper());
						if (cmb_recoge_tienda.SelectedIndex == 0)
						{
							txt_nombre_retira.Text = txt_nombre.Text + " " + txt_apellidos.Text;
							txt_id_retira.Text = txt_id_cliente.Text;
						}
						string[] dir = res[3].Split('|');

						txt_direccion.Text = RemoveDiacritics(dir[0]).ToUpper().Replace("°", "").Trim() + ", " + cmb_barrio.Text.Trim().ToUpper() + ", " + cmb_ciudad.Text + ", COLOMBIA";
						txt_nota_pedido.Text += RemoveDiacritics(dir[1] + " " + dir[2]).ToUpper().Replace("°", "").Trim() + Environment.NewLine;
						if (res[4].Trim().Replace(" ", "").Replace("-", "").Length > 10)
						{
							txt_telefono.Text = res[4].Trim().Replace(" ", "").Replace("-", "").Substring(0, 10);
						}
						else
						{
							txt_telefono.Text = res[4].Replace(" ", "").Replace("-", "");
						}
						if (res[5].Trim().Replace(" ", "").Replace("-", "").Length > 10)
						{
							txt_celular.Text = res[5].Trim().Replace(" ", "").Replace("-", "").Substring(0, 10);
						}
						else
						{
							txt_celular.Text = res[5].Replace(" ", "").Replace("-", "");
						}

						txt_email.Text = res[6];//
						txt_barrio.Text = res[7];
						txt_ciudad.Text = res[9];
						txt_ciudad.Tag = res[8];//
						txt_departamento.Text = res[10];
						txt_pais.Text = res[11];
					}
					else
					{
						txt_nombre.Text = RemoveDiacritics(res[1].ToUpper());
						//txt_apellidos.Text = RemoveDiacritics(res[2].ToUpper());
						string[] dir = res[2].Split('|');

						txt_direccion.Text = RemoveDiacritics(dir[0]).ToUpper().Replace("°", "").Trim() + ", " + cmb_barrio.Text.Trim().ToUpper() + ", " + cmb_ciudad.Text + ", COLOMBIA";
						txt_nota_pedido.Text += RemoveDiacritics(dir[1] + " " + dir[2]).ToUpper().Replace("°", "").Trim() + Environment.NewLine;

						if (res[3].Trim().Replace(" ", "").Replace("-", "").Length > 10)
						{
							txt_telefono.Text = res[3].Trim().Replace(" ", "").Replace("-", "").Substring(0, 10);
						}
						else
						{
							txt_telefono.Text = res[3].Replace(" ", "").Replace("-", "");
						}
						if (res[4].Trim().Replace(" ", "").Replace("-", "").Length > 10)
						{
							txt_celular.Text = res[4].Trim().Replace(" ", "").Replace("-", "").Substring(0, 10);
						}
						else
						{
							txt_celular.Text = res[4].Replace(" ", "").Replace("-", "");
						}

						txt_email.Text = res[5];//
						txt_barrio.Text = res[6];
						txt_ciudad.Text = res[8];
						txt_ciudad.Tag = res[7];//
						txt_departamento.Text = res[9];
						txt_pais.Text = res[10];
					}
					txt_buscar.Focus();
					btn_cargar_ultimo.Enabled = true;
					if (cmb_recoge_tienda.SelectedIndex == 1)
					{
						//////////////
						precio_dom = true;
						string[] cod_dom = ciudad_centro_operacion.ObternerCodigoDomicilioBarrio(Convert.ToInt32(cmb_barrio.SelectedValue));
						if (cod_dom != null)
						{
							txt_buscar.Text = cod_dom[0];
						}

						if (!txt_buscar.Text.Equals(""))
						{
							btn_busar_item.PerformClick();
							chk_express.Enabled = true;
						}
						else
						{
							precio_dom = false;
						}
						/////////////
					}
				}
				else
				{
					MessageBox.Show("No hay información para este número de identificación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
					txt_id_cliente.Focus();
					txt_id_cliente.SelectAll();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			this.Cursor = Cursors.Default;
		}

		private void btn_cerrar_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void FrmPpal_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (reset == false)
			{
				if (MessageBox.Show("¿Desea cerrar el programa?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
				{
					e.Cancel = true;
				}
			}
		}

		private void estadoDePedidoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new FrmEstadoPedido().ShowDialog(this);
		}

		private void listadoDePedidosEnviadosToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new FrmPedidosEnviados(lbl_terminal.Text).ShowDialog(this);
		}

		private void txt_id_cliente_KeyDown(object sender, KeyEventArgs e)
		{
			if (!txt_id_cliente.Text.Trim().Equals(""))
			{
				if (e.KeyData.Equals(Keys.Enter))
				{
					btn_buscar_cliente.PerformClick();
				}
			}
		}

		private void cmb_sucursal_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (cmb_sucursal.SelectedIndex > -1)
				{
					txt_tpv.Text = "";
					txt_tpv.Text = dtDatos.Rows[cmb_sucursal.SelectedIndex][6].ToString();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void rdb_descripcion_CheckedChanged(object sender, EventArgs e)
		{
			if (rdb_descripcion.Checked)
			{
				if (cmb_co.SelectedIndex == -1)
				{
					MessageBox.Show("Seleccione el Centro de operación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					cmb_co.Focus();
					rdb_id.Checked = true;
					return;
				}

				if (txt_id_cliente.Tag == null)
				{
					MessageBox.Show("Primero debe buscar la información del cliente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					rdb_id.Checked = true;
					txt_id_cliente.Focus();
					return;
				}

				LimpiarItem();
				txt_buscar.Text = "";
				FrmItemsBusqueda _FrmItemsBusqueda = new FrmItemsBusqueda(this);
				_FrmItemsBusqueda.ShowDialog(this);

				if (_FrmItemsBusqueda.DialogResult.Equals(DialogResult.OK))
				{
					FrmCantidad _FrmCantidad = new FrmCantidad(txt_cantidad);
					_FrmCantidad.ShowDialog(this);
					if (_FrmCantidad.DialogResult.Equals(DialogResult.OK))
					{
						Application.DoEvents();
						_backgroundWorker_dscto.RunWorkerAsync();
						pbx_buscar.Visible = true;
					}
					else
					{
						LimpiarItem();
						txt_buscar.Focus();
					}
				}
				else
				{
					txt_buscar.Focus();
				}
			}
			rdb_id.Checked = true;
		}

		private void btn_busar_item_Click(object sender, EventArgs e)
		{
			if (cmb_co.SelectedIndex == -1)
			{
				MessageBox.Show("Seleccione el Centro de operación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cmb_co.Focus();
				return;
			}

			if (txt_id_cliente.Tag == null)
			{
				MessageBox.Show("Primero debe buscar la información del cliente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txt_id_cliente.Focus();
				return;
			}

			if (txt_buscar.Text.Trim().Equals(""))
			{
				MessageBox.Show("Escriba el id del producto", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txt_buscar.Focus();
				return;
			}

			this.Cursor = Cursors.WaitCursor;
			try
			{
				LimpiarItem();

				Item item = new Item();

				string[] res = null;
				if (rdb_id.Checked)
				{
					res = item.BuscarItemsId(txt_buscar.Text.Trim());
				}
				if (rdb_barra.Checked)
				{
					res = item.BuscarItemsBarra(txt_buscar.Text.Trim());
				}
				if (res != null)
				{
					CiudadCentroOperacion ciudad_centro_operacion = new CiudadCentroOperacion();
					Configuracion.RowidItem = res[0];

					txt_id.Text = res[1];
					txt_referencia.Text = res[2];
					txt_descripcion.Text = RemoveDiacritics(res[3]).ToUpper();//.Replace('Á', 'A').Replace('É', 'E').Replace('Í', 'I').Replace('Ó', 'O').Replace('Ú', 'U').Replace('Ñ', 'N');

					if (ObtenerUnidades(Configuracion.RowidItem, Configuracion.RowidTercero).Equals(DialogResult.OK)) //PASO 2
					{
						ObtenerExistencia(txt_tpv.Text.Trim(), Configuracion.RowidItemExt, Convert.ToString(cmb_sucursal.SelectedValue).Trim(), Configuracion.RowidTercero);//PASO 3

						ObtenerPrecio(Configuracion.IdListaPrecio, Configuracion.RowidItemExt, Configuracion.UnidadMedida);//PASO 

						string[] cod_dom = ciudad_centro_operacion.ObternerCodigoDomicilioBarrio(Convert.ToInt32(cmb_barrio.SelectedValue));

						if (precio_dom == true || txt_buscar.Text == cod_dom[0] || txt_buscar.Text == cod_dom[1])
						{
							Application.DoEvents();
							txt_cantidad.Text = "1";
							_backgroundWorker_dscto.RunWorkerAsync();
							pbx_buscar.Visible = true;
						}
						else
						{
							FrmCantidad _FrmCantidad = new FrmCantidad(txt_cantidad);
							_FrmCantidad.ShowDialog(this);
							if (_FrmCantidad.DialogResult.Equals(DialogResult.OK))
							{
								Application.DoEvents();
								_backgroundWorker_dscto.RunWorkerAsync();
								pbx_buscar.Visible = true;
							}
							else
							{
								LimpiarItem();
								txt_buscar.Focus();
								txt_buscar.SelectAll();
							}
						}
					}
					else
					{
						LimpiarItem();
						txt_buscar.Focus();
						txt_buscar.SelectAll();
					}
				}
				else
				{
					MessageBox.Show("No hay información para este item", "Avisdo", MessageBoxButtons.OK, MessageBoxIcon.Information);
					txt_buscar.Focus();
					txt_buscar.SelectAll();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			this.Cursor = Cursors.Default;
		}

		private void txt_buscar_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (!txt_buscar.Text.Trim().Equals(""))
			{
				if (e.KeyData == Keys.Tab)
				{
					btn_busar_item.PerformClick();
				}
			}
		}

		private void txt_buscar_KeyDown(object sender, KeyEventArgs e)
		{
			if (!txt_buscar.Text.Trim().Equals(""))
			{
				if (e.KeyData == Keys.Enter)
				{
					btn_busar_item.PerformClick();
				}
			}
		}

		private void btn_agregar_Click(object sender, EventArgs e)
		{
			Configuracion configuracion = new Configuracion();
			CiudadCentroOperacion ciudad_centro_operacion = new CiudadCentroOperacion();
			object[] conf = configuracion.ObtenerConfiguracion();
			string[] unds_dec = conf[3].ToString().Split(',');

			string[] cod_dom = ciudad_centro_operacion.ObternerCodigoDomicilioBarrio(Convert.ToInt32(cmb_barrio.SelectedValue));

			if (txt_cantidad.Text.Trim().Contains(',') || txt_cantidad.Text.Trim().Contains('.'))
			{
				if (!unds_dec.Contains(txt_unidad.Text.Trim()))
				{
					MessageBox.Show("La unidad de medida del producto no admite valores decimales como cantidad", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}
			}
			if (!txt_id.Text.Equals(cod_dom[0]) && !txt_id.Text.Equals(cod_dom[1]))
			{
				if (chk_express.Checked == true)
				{
					int cant_max_express = Convert.ToInt32(conf[5]);

					if (Convert.ToInt32(txt_nro_art.Text.Trim()) + 1 > cant_max_express)
					{
						MessageBox.Show("La cantidad máxima de items para el servicio express es de " + cant_max_express.ToString(), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
						return;
					}
				}
			}
			for (int i = 0; i < dgv_datos.RowCount; i++)
			{
				if (txt_id.Text.Trim().Equals(dgv_datos[0, i].Value.ToString().Trim()) && txt_referencia.Text.Trim().Equals(dgv_datos[1, i].Value.ToString().Trim()))
				{

					if (precio_dom == false || chk_express.Checked == true)
					{
						MessageBox.Show("Este item ya esta en la lista", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
						txt_buscar.Focus();
						txt_buscar.SelectAll();
						btn_agregar.Enabled = false;
					}
					else
					{
						txt_buscar.Text = "";
						txt_buscar.Focus();
					}

					LimpiarItem();
					return;
				}
			}
			try
			{
				decimal dscto_porc = 0;
				decimal dscto_val = 0;
				string tipo_dscto = "";

				if (!txt_porc_dscto.Text.Trim().Equals("0") && txt_val_dscto.Text.Trim().Equals("0"))
				{
					dscto_porc = Convert.ToDecimal(txt_porc_dscto.Text.Trim().Replace('.', Configuracion.separador).Replace(',', Configuracion.separador));
					tipo_dscto = "%";
				}
				else if (txt_porc_dscto.Text.Trim().Equals("0") && !txt_val_dscto.Text.Trim().Equals("0"))
				{
					dscto_val = Convert.ToDecimal(txt_val_dscto.Text.Trim().Replace('.', Configuracion.separador).Replace(',', Configuracion.separador));
					tipo_dscto = "V";
				}

				decimal cantidad = Convert.ToDecimal(txt_cantidad.Text.Trim());
				decimal precio_vta = Convert.ToDecimal(txt_precio.Text.Trim());

				decimal impuesto = Convert.ToDecimal(txt_impuesto.Text.Trim().Replace('.', Configuracion.separador).Replace(',', Configuracion.separador));
				decimal base_imp = Convert.ToDecimal(txt_impuesto.Tag.ToString().Replace('.', Configuracion.separador).Replace(',', Configuracion.separador));
				decimal val_bruto_pvp = 0;
				decimal val_bruto = 0;
				decimal val_dscto = 0;
				decimal val_impuesto = 0;
				decimal val_unitario = 0;

				if (impuesto > 0 && base_imp > 0)
				{
					val_bruto_pvp = (precio_vta / (base_imp + impuesto)) * base_imp;
				}
				else
				{
					val_bruto_pvp = precio_vta;
				}

				val_bruto = cantidad * val_bruto_pvp;

				if (tipo_dscto.Equals("%"))
				{
					val_dscto = (val_bruto * dscto_porc) / 100;
				}
				else if (tipo_dscto.Equals("V"))
				{
					val_dscto = (cantidad * dscto_val);
				}

				decimal total_item = 0;
				if (impuesto > 0)
				{
					val_impuesto = ((val_bruto - val_dscto) * impuesto) / 100;
				}


				if (tipo_dscto.Equals("V"))
				{
					total_item = (precio_vta * cantidad) - val_dscto;
				}
				else
				{
					total_item = val_bruto - val_dscto + val_impuesto;
				}

				val_unitario = total_item / cantidad;

				dgv_datos.Rows.Add(txt_id.Text.Trim(), txt_referencia.Text.Trim(), txt_descripcion.Text.Trim(), txt_unidad.Text.Trim(),
									precio_vta, dscto_porc, dscto_val, impuesto, val_bruto_pvp, txt_cantidad.Text.Trim(), val_bruto,
									val_dscto, val_impuesto, total_item, val_unitario, RemoveDiacritics(txt_nota_item.Text.Trim()), base_imp, Configuracion.RowidItemExt);


				//if (!txt_id.Text.Equals(cod_dom[0]) && !txt_id.Text.Equals(cod_dom[1]))
				//{
				//    DataGridViewRow rowToMove = dgv_datos.Rows[dgv_datos.Rows.Count - 1];
				//    dgv_datos.Rows.RemoveAt(rowToMove.Index);
				//    dgv_datos.Rows.Insert(0, rowToMove);
				//}

				SumarTotales();

				LimpiarItem();
				txt_buscar.Text = "";
				txt_nota_item.Enabled = false;
				txt_buscar.Focus();
				btn_agregar.Enabled = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void lbl_tit_dscto_Click(object sender, EventArgs e)
		{
			FrmDescPromo _FrmDescPromo = new FrmDescPromo();
			_FrmDescPromo.lbl_fecha_ini.Text = fecha_ini_promo;
			_FrmDescPromo.lbl_fecha_fin.Text = fecha_fin_promo;
			_FrmDescPromo.lbl_desc_promo.Text = desc_promo;
			_FrmDescPromo.ShowDialog(this);
		}

		private void btn_quitar_Click(object sender, EventArgs e)
		{
			try
			{
				if (dgv_datos.SelectedRows.Count > 0)
				{
					CiudadCentroOperacion metodos = new CiudadCentroOperacion();
					string[] cod_dom_express = metodos.ObternerCodigoDomicilioBarrio(Convert.ToInt32(cmb_barrio.SelectedValue));
					if (dgv_datos[0, dgv_datos.CurrentRow.Index].Value.Equals(cod_dom_express[1]))
					{
						chk_express.Checked = false;
					}
					else
					{
						dgv_datos.Rows.RemoveAt(dgv_datos.SelectedRows[0].Index);
						SumarTotales();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btn_guardar_Click(object sender, EventArgs e)
		{
			if (dgv_datos.RowCount == 0)
			{
				MessageBox.Show("Seleccione los productos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			Configuracion configuracion = new Configuracion();
			CiudadCentroOperacion ciudad_centro_operacion = new CiudadCentroOperacion();
			object[] conf = configuracion.ObtenerConfiguracion();
			decimal valpedido_min = Convert.ToDecimal(conf[4].ToString());
			string[] codservdom = ciudad_centro_operacion.ObternerCodigoDomicilioBarrio(Convert.ToInt32(cmb_barrio.SelectedValue));

			decimal valtotalpedido = 0;

			foreach (DataGridViewRow fila in dgv_datos.Rows)
			{
				bool igual = false;
				for (int j = 0; j < codservdom.GetLongLength(0); j++)
				{
					if (Convert.ToString(fila.Cells[0].Value).Trim().Equals(codservdom[j]))
					{
						igual = true;
						break;
					}
				}
				if (igual == false)
				{
					valtotalpedido += Convert.ToDecimal(fila.Cells[13].Value);
				}
			}

			if (!txt_celular.Text.Trim().Length.Equals(10) || !txt_telefono.Text.Trim().Length.Equals(10))
			{
				MessageBox.Show("Los números de teléfono deben ser de 10 dígitos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txt_telefono.Focus();
				txt_telefono.SelectAll();
				return;
			}

			if (valtotalpedido < valpedido_min)
			{
				MessageBox.Show("El valor mínimo del pedido debe ser " + valpedido_min.ToString(), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (cmb_recoge_tienda.SelectedIndex == 0)
			{
				if (txt_id_retira.Text.Trim().Equals("") || txt_nombre_retira.Text.Trim().Equals(""))
				{
					MessageBox.Show("Escriba la identificación y el nombre de quien recoge en tienda", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			Pedido guardar_pedido = new Pedido();
			try
			{
				if (nuevoToolStripMenuItem.CheckState.Equals(CheckState.Checked))
				{
					if (MessageBox.Show($"¿Confirma guardar este pedido?.{Environment.NewLine}Una vez guardado no se puede agregar mas productos.", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
					{
						Terminal terminal = new Terminal();
						lbl_consecutivo.Text = terminal.ObtenerConsecutivo(Convert.ToByte(lbl_terminal.Text)).ToString();
						lbl_consecutivo_formateado.Text = "Consecutivo: T" + lbl_terminal.Text.PadLeft(2, '0') + "-" + lbl_consecutivo.Text.PadLeft(4, '0');
						object[] pedido = new object[28];
						pedido[0] = lbl_consecutivo.Text;
						pedido[1] = lbl_terminal.Text;
						pedido[2] = "";
						pedido[3] = cmb_co.SelectedValue;
						pedido[4] = cmb_sucursal.SelectedValue;
						pedido[5] = Convert.ToString(txt_id_cliente.Tag);
						pedido[6] = txt_nombre.Text.Trim();
						pedido[7] = txt_apellidos.Text.Trim();
						pedido[8] = txt_telefono.Text.Trim();
						pedido[9] = txt_celular.Text.Trim();
						pedido[10] = txt_email.Text.Trim();
						pedido[11] = txt_direccion.Text.Trim();
						pedido[12] = txt_barrio.Text.Trim();
						pedido[13] = txt_ciudad.Text.Trim();
						pedido[14] = txt_departamento.Text.Trim();
						pedido[15] = txt_pais.Text.Trim();
						pedido[16] = txt_nota_pedido.Text.Trim();
						if (rdb_nat.Checked == true)
						{
							pedido[17] = "n";
						}
						else if (rdb_juridica.Checked == true)
						{
							pedido[17] = "j";
						}
						pedido[18] = 1;

						string fuente = "";
						if (rdb_call_center.Checked == true)
						{
							fuente = "C";
						}
						if (rdb_whatsapp.Checked == true)
						{
							fuente = "W";
						}
						if (rdb_email.Checked == true)
						{
							fuente = "E";
						}

						pedido[19] = fuente;
						pedido[20] = cmb_ciudad.Text;
						pedido[21] = cmb_barrio.Text;
						//---------------------------//
						pedido[22] = lbl_agente.Text;
						pedido[23] = cmb_medio_pago.Text;
						if (cmb_recoge_tienda.SelectedIndex == 0)
						{
							pedido[24] = true;
						}
						else
						{
							pedido[24] = false;
						}
						pedido[25] = txt_id_retira.Text;
						pedido[26] = txt_nombre_retira.Text;
						pedido[27] = dtp_fecha_entrega.Value.Date;

						List<object[]> detalle = new List<object[]>();
						foreach (DataGridViewRow fila in dgv_datos.Rows)
						{
							object[] item = new object[20];
							item[0] = lbl_consecutivo.Text;
							item[1] = lbl_terminal.Text;
							item[2] = fila.Cells[0].Value;
							item[3] = fila.Cells[1].Value;
							item[4] = fila.Cells[2].Value;
							item[5] = fila.Cells[3].Value;
							item[6] = fila.Cells[4].Value;
							item[7] = fila.Cells[5].Value;
							item[8] = Convert.ToDecimal(fila.Cells[6].Value);
							item[9] = Convert.ToDecimal(fila.Cells[7].Value);
							item[10] = Convert.ToDecimal(fila.Cells[8].Value);
							item[11] = Convert.ToDecimal(fila.Cells[9].Value);
							item[12] = Convert.ToDecimal(fila.Cells[10].Value);
							item[13] = Convert.ToDecimal(fila.Cells[11].Value);
							item[14] = Convert.ToDecimal(fila.Cells[12].Value);
							item[15] = Convert.ToDecimal(fila.Cells[13].Value);
							item[16] = fila.Cells[15].Value;
							item[17] = fila.Cells[16].Value;
							item[18] = fila.Cells[17].Value;
							item[19] = Convert.ToDecimal(fila.Cells[14].Value);
							detalle.Add(item);
						}

						consecutivo_guardado = lbl_consecutivo.Text;
						guardar_pedido.GuardarPedido(pedido, detalle);
						guardar_pedido.GuardarLog(Convert.ToByte(lbl_terminal.Text), Convert.ToInt32(consecutivo_guardado), false);
						btn_guardar.Enabled = false;
						
						lbl_consecutivo.Text = terminal.ObtenerConsecutivo(Convert.ToByte(lbl_terminal.Text)).ToString();
						MessageBox.Show($"Pedido guardado exitosamente con el {lbl_consecutivo_formateado.Text}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
						nuevoToolStripMenuItem.PerformClick();
					}
				}
				else if (congeladoToolStripMenuItem.CheckState.Equals(CheckState.Checked))
				{
					if (MessageBox.Show($"¿Confirma guardar este pedido?.{Environment.NewLine}Una vez guardado no se puede agregar mas productos.", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
					{
						if (!lbl_terminal.Text.Equals("") && !lbl_consecutivo.Text.Equals(""))
						{
							object[] pedido = new object[23];
							pedido[0] = "";// metodos.ObtenerFechaActual(true);
							pedido[1] = cmb_co.SelectedValue;
							pedido[2] = cmb_sucursal.SelectedValue;
							pedido[3] = Convert.ToString(txt_id_cliente.Tag);
							pedido[4] = txt_nombre.Text.Trim();
							pedido[5] = txt_apellidos.Text.Trim();
							pedido[6] = txt_telefono.Text.Trim();
							pedido[7] = txt_celular.Text.Trim();
							pedido[8] = txt_email.Text.Trim();
							pedido[9] = txt_direccion.Text.Trim();
							pedido[10] = txt_barrio.Text.Trim();
							pedido[11] = txt_ciudad.Text.Trim();
							pedido[12] = txt_departamento.Text.Trim();
							pedido[13] = txt_pais.Text.Trim();
							pedido[14] = txt_nota_pedido.Text.Trim();
							if (rdb_nat.Checked == true)
							{
								pedido[15] = "n";
							}
							else if (rdb_juridica.Checked == true)
							{
								pedido[15] = "j";
							}
							pedido[16] = 1;

							//---------------------------//
							pedido[17] = lbl_agente.Text;
							pedido[18] = cmb_medio_pago.Text;
							if (cmb_recoge_tienda.SelectedIndex == 0)
							{
								pedido[19] = true;
							}
							else
							{
								pedido[19] = false;
							}
							pedido[20] = txt_id_retira.Text;
							pedido[21] = txt_nombre_retira.Text;
							pedido[22] = dtp_fecha_entrega.Value.Date;

							List<object[]> detalle = new List<object[]>();
							foreach (DataGridViewRow fila in dgv_datos.Rows)
							{
								object[] item = new object[18];
								item[0] = fila.Cells[0].Value;
								item[1] = fila.Cells[1].Value;
								item[2] = fila.Cells[2].Value;
								item[3] = fila.Cells[3].Value;
								item[4] = Convert.ToDecimal(fila.Cells[4].Value);
								item[5] = Convert.ToDecimal(fila.Cells[5].Value);
								item[6] = Convert.ToDecimal(fila.Cells[6].Value);
								item[7] = Convert.ToDecimal(fila.Cells[7].Value);
								item[8] = Convert.ToDecimal(fila.Cells[8].Value);
								item[9] = Convert.ToDecimal(fila.Cells[9].Value);
								item[10] = Convert.ToDecimal(fila.Cells[10].Value);
								item[11] = Convert.ToDecimal(fila.Cells[11].Value);
								item[12] = Convert.ToDecimal(fila.Cells[12].Value);
								item[13] = Convert.ToDecimal(fila.Cells[13].Value);
								item[14] = fila.Cells[15].Value;
								item[15] = fila.Cells[16].Value;
								item[16] = fila.Cells[17].Value;
								item[17] = Convert.ToDecimal(fila.Cells[14].Value);
								detalle.Add(item);
							}
							guardar_pedido.GuardarPedido2(Convert.ToInt32(lbl_consecutivo.Text), Convert.ToByte(lbl_terminal.Text), pedido, detalle);
							guardar_pedido.GuardarLog(Convert.ToByte(lbl_terminal.Text), Convert.ToInt32(lbl_consecutivo.Text), Configuracion.Terminal);

							btn_guardar.Enabled = false;
							MessageBox.Show($"Pedido guardado exitosamente con el {lbl_consecutivo_formateado.Text}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
							nuevoToolStripMenuItem.PerformClick();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void txt_nota_item_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode.Equals(Keys.Enter))
			{
				btn_agregar.PerformClick();
			}
		}

		private void FrmPpal_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.F1:
					rdb_id.Checked = true;
					break;
				case Keys.F2:
					rdb_barra.Checked = true;
					break;
				case Keys.F3:
					rdb_descripcion.Checked = true;
					break;
			}
			if ((e.Control && e.Shift && e.KeyCode == Keys.N) && Configuracion.TipoUsuario == 1)
			{
				new FrmUsuarioEntrecaminos().ShowDialog(this);
			}
		}

		private void dgv_datos_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.ColumnIndex == 9)
				{
					string val_ant = dgv_datos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

					FrmCantidad _FrmCantidad = new FrmCantidad(dgv_datos.Rows[e.RowIndex].Cells[e.ColumnIndex]);
					_FrmCantidad.ShowDialog(this);
					if (_FrmCantidad.DialogResult.Equals(DialogResult.OK))
					{
						Configuracion configuracion = new Configuracion();
						object[] conf = configuracion.ObtenerConfiguracion();
						string[] unds_dec = conf[3].ToString().Split(',');

						if (dgv_datos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim().Contains(',') || dgv_datos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim().Contains('.'))
						{
							if (!unds_dec.Contains(dgv_datos.Rows[e.RowIndex].Cells[3].Value.ToString().Trim()))
							{
								MessageBox.Show("La unidad de medida del producto no admite valores decimales como cantidad", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
								dgv_datos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = val_ant;
								return;
							}
						}

						this.Cursor = Cursors.WaitCursor;
						string[] res = ObtenerDescuento(dgv_datos.Rows[e.RowIndex].Cells[17].Value.ToString(), Configuracion.IdSucursalCliente, Configuracion.IdListaPrecio, dgv_datos.Rows[e.RowIndex].Cells[9].Value.ToString(), Convert.ToString(txt_id_cliente.Tag), Configuracion.RowidTercero);

						dgv_datos.Rows[e.RowIndex].Cells[5].Value = "0";
						dgv_datos.Rows[e.RowIndex].Cells[6].Value = "0";

						if (res != null)
						{
							dgv_datos.Rows[e.RowIndex].Cells[5].Value = res[0];
							dgv_datos.Rows[e.RowIndex].Cells[6].Value = res[1];
						}

						decimal dscto_porc = 0;
						decimal dscto_val = 0;
						string tipo_dscto = "";

						if (!dgv_datos.Rows[e.RowIndex].Cells[5].Value.Equals("0") && dgv_datos.Rows[e.RowIndex].Cells[6].Value.Equals("0"))
						{
							dscto_porc = Convert.ToDecimal(dgv_datos.Rows[e.RowIndex].Cells[5].Value.ToString().Trim());
							tipo_dscto = "%";
						}
						else if (dgv_datos.Rows[e.RowIndex].Cells[5].Value.Equals("0") && !dgv_datos.Rows[e.RowIndex].Cells[6].Value.Equals("0"))
						{
							dscto_val = Convert.ToDecimal(dgv_datos.Rows[e.RowIndex].Cells[6].Value.ToString().Trim());
							tipo_dscto = "V";
						}

						decimal cantidad = Convert.ToDecimal(dgv_datos.Rows[e.RowIndex].Cells[9].Value.ToString().Trim());
						decimal precio_vta = Convert.ToDecimal(dgv_datos.Rows[e.RowIndex].Cells[4].Value.ToString().Trim());

						decimal impuesto = Convert.ToDecimal(dgv_datos.Rows[e.RowIndex].Cells[7].Value.ToString().Trim());
						decimal base_imp = Convert.ToDecimal(dgv_datos.Rows[e.RowIndex].Cells[16].Value.ToString().Trim());
						decimal val_bruto_pvp = 0;
						decimal val_bruto = 0;
						decimal val_dscto = 0;
						decimal val_impuesto = 0;
						decimal val_unitario = 0;

						if (impuesto > 0 && base_imp > 0)
						{
							val_bruto_pvp = (precio_vta / (base_imp + impuesto)) * base_imp;
						}
						else
						{
							val_bruto_pvp = precio_vta;
						}

						val_bruto = cantidad * val_bruto_pvp;

						if (tipo_dscto.Equals("%"))
						{
							val_dscto = (val_bruto * dscto_porc) / 100;
						}
						else if (tipo_dscto.Equals("V"))
						{
							val_dscto = (cantidad * dscto_val);
						}

						if (impuesto > 0)
						{
							val_impuesto = ((val_bruto - val_dscto) * impuesto) / 100;
						}

						decimal total_item = 0;

						if (tipo_dscto.Equals("V"))
						{
							total_item = (precio_vta * cantidad) - val_dscto;
						}
						else
						{
							total_item = val_bruto - val_dscto + val_impuesto;
						}

						val_unitario = total_item / cantidad;

						dgv_datos.Rows[e.RowIndex].Cells[10].Value = val_bruto.ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
						dgv_datos.Rows[e.RowIndex].Cells[11].Value = val_dscto.ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
						dgv_datos.Rows[e.RowIndex].Cells[12].Value = val_impuesto.ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
						dgv_datos.Rows[e.RowIndex].Cells[13].Value = total_item.ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
						dgv_datos.Rows[e.RowIndex].Cells[14].Value = val_unitario.ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
						SumarTotales();
					}
				}
				if (e.ColumnIndex == 15)
				{
					new FrmNota(dgv_datos.Rows[e.RowIndex].Cells[e.ColumnIndex]).ShowDialog(this);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			this.Cursor = Cursors.Default;
		}

		private void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = ObtenerDescuento(Configuracion.RowidItemExt, Configuracion.IdSucursalCliente, Configuracion.IdListaPrecio, txt_cantidad.Text.Trim(), Convert.ToString(txt_id_cliente.Tag), Configuracion.RowidTercero);//PASO 5   
		}

		private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			string[] res = (string[])e.Result;
			if (res != null)
			{
				txt_porc_dscto.Text = res[0];
				txt_val_dscto.Text = res[1];
				fecha_ini_promo = res[2];
				fecha_fin_promo = res[3];
				desc_promo = res[4];
				lbl_tit_dscto.ForeColor = Color.Blue;
				lbl_tit_dscto.Cursor = Cursors.Hand;
			}
			else
			{
				txt_porc_dscto.Text = "0";
				txt_val_dscto.Text = "0";
				lbl_tit_dscto.ForeColor = Color.Black;
				lbl_tit_dscto.Cursor = Cursors.Default;
			}

			_backgroundWorker_imp.RunWorkerAsync(cmb_co.SelectedValue);
		}

		private void _backgroundWorker_imp_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = ObtenerImpuesto(Configuracion.RowidItem, e.Argument.ToString(), Configuracion.RowidTercero, Configuracion.IdSucursalCliente, Configuracion.RowidItemListaPrecio);//PASO 6
		}

		private void _backgroundWorker_imp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			string[] res = (string[])e.Result;
			if (res != null)
			{
				txt_impuesto.Text = res[1];
				txt_impuesto.Tag = res[0];
			}
			else
			{
				txt_impuesto.Text = "0";
				txt_impuesto.Tag = "0";
			}

			txt_nota_item.Enabled = true;
			txt_nota_item.Focus();
			btn_agregar.Enabled = true;

			pbx_buscar.Visible = false;

			if (Configuracion.ultimo == true)
			{
				btn_agregar.PerformClick();
			}
			CiudadCentroOperacion metodos = new CiudadCentroOperacion();
			string[] cod_dom = metodos.ObternerCodigoDomicilioBarrio(Convert.ToInt32(cmb_barrio.SelectedValue));

			if (precio_dom == true || txt_buscar.Text == cod_dom[0] || txt_buscar.Text == cod_dom[1])
			{
				btn_agregar.PerformClick();
			}

			precio_dom = false;
			Configuracion.ultimo = false;
			this.Cursor = Cursors.Default;
		}

		private void btn_otro_Click(object sender, EventArgs e)
		{
			LimpiarCabecera();
			LimpiarItem();
			LimpiarTotales();
			txt_id_cliente.Text = "";
			txt_id_cliente.Enabled = true;
			txt_id_cliente.Focus();
			btn_buscar_cliente.Text = "BUSCAR C&LIENTE";
		}

		private void rdb_id_CheckedChanged(object sender, EventArgs e)
		{
			if (rdb_id.Checked)
			{
				txt_buscar.Focus();
			}
		}

		private void rdb_barra_CheckedChanged(object sender, EventArgs e)
		{
			if (rdb_barra.Checked)
			{
				txt_buscar.Focus();
			}
		}

		private void rdb_nat_CheckedChanged(object sender, EventArgs e)
		{
			txt_id_cliente.Text = "";
			txt_id_cliente.Focus();
			LimpiarCabecera();
		}

		private void congelarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dgv_datos.RowCount == 0)
			{
				MessageBox.Show("Seleccione los productos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			try
			{
				Pedido guardar = new Pedido();

				if (nuevoToolStripMenuItem.CheckState.Equals(CheckState.Checked))
				{
					object[] pedido = new object[28];
					pedido[0] = lbl_consecutivo.Text;
					pedido[1] = lbl_terminal.Text;
					pedido[2] = "";
					pedido[3] = cmb_co.SelectedValue;
					pedido[4] = cmb_sucursal.SelectedValue;
					pedido[5] = Convert.ToString(txt_id_cliente.Tag);
					pedido[6] = txt_nombre.Text.Trim();
					pedido[7] = txt_apellidos.Text.Trim();
					pedido[8] = txt_telefono.Text.Trim();
					pedido[9] = txt_celular.Text.Trim();
					pedido[10] = txt_email.Text.Trim();
					pedido[11] = txt_direccion.Text.Trim();
					pedido[12] = txt_barrio.Text.Trim();
					pedido[13] = txt_ciudad.Text.Trim();
					pedido[14] = txt_departamento.Text.Trim();
					pedido[15] = txt_pais.Text.Trim();
					pedido[16] = txt_nota_pedido.Text.Trim();
					if (rdb_nat.Checked == true)
					{
						pedido[17] = "n";
					}
					else if (rdb_juridica.Checked == true)
					{
						pedido[17] = "j";
					}
					pedido[18] = 0;

					string fuente = "";
					if (rdb_call_center.Checked == true)
					{
						fuente = "C";
					}
					if (rdb_whatsapp.Checked == true)
					{
						fuente = "W";
					}
					if (rdb_email.Checked == true)
					{
						fuente = "E";
					}
					pedido[19] = fuente;
					pedido[20] = cmb_ciudad.Text;
					pedido[21] = cmb_barrio.Text;

					pedido[22] = lbl_agente.Text;
					pedido[23] = cmb_medio_pago.Text;
					if (cmb_recoge_tienda.SelectedIndex == 0)
					{
						pedido[24] = true;
					}
					else
					{
						pedido[24] = false;
					}
					pedido[25] = txt_id_retira.Text;
					pedido[26] = txt_nombre_retira.Text;
					pedido[27] = dtp_fecha_entrega.Value.Date;

					List<object[]> detalle = new List<object[]>();
					foreach (DataGridViewRow fila in dgv_datos.Rows)
					{
						object[] item = new object[20];
						item[0] = lbl_consecutivo.Text;
						item[1] = lbl_terminal.Text;
						item[2] = fila.Cells[0].Value;
						item[3] = fila.Cells[1].Value;
						item[4] = fila.Cells[2].Value;
						item[5] = fila.Cells[3].Value;
						item[6] = fila.Cells[4].Value;
						item[7] = fila.Cells[5].Value;
						item[8] = fila.Cells[6].Value;
						item[9] = fila.Cells[7].Value;
						item[10] = fila.Cells[8].Value;
						item[11] = fila.Cells[9].Value;
						item[12] = fila.Cells[10].Value;
						item[13] = fila.Cells[11].Value;
						item[14] = fila.Cells[12].Value;
						item[15] = fila.Cells[13].Value;
						item[16] = fila.Cells[15].Value;
						item[17] = fila.Cells[16].Value;
						item[18] = fila.Cells[17].Value;
						item[19] = fila.Cells[14].Value;
						detalle.Add(item);
					}

					guardar.GuardarPedido(pedido, detalle);
					Terminal terminal = new Terminal();
					guardar.GuardarLog(Convert.ToByte(lbl_terminal.Text), Convert.ToInt32(lbl_consecutivo.Text), true);
					lbl_consecutivo.Text = terminal.ObtenerConsecutivo(Convert.ToByte(lbl_terminal.Text)).ToString();
					MessageBox.Show("Pedido congelado exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
					nuevoToolStripMenuItem.PerformClick();
				}
				else if (congeladoToolStripMenuItem.CheckState.Equals(CheckState.Checked))
				{
					if (!lbl_terminal.Text.Equals("") && !lbl_consecutivo.Text.Equals(""))
					{
						object[] pedido = new object[23];
						pedido[0] = "";// guardar.ObtenerFechaActual(true);
						pedido[1] = cmb_co.SelectedValue;
						pedido[2] = cmb_sucursal.SelectedValue;
						pedido[3] = Convert.ToString(txt_id_cliente.Tag);
						pedido[4] = txt_nombre.Text.Trim();
						pedido[5] = txt_apellidos.Text.Trim();
						pedido[6] = txt_telefono.Text.Trim();
						pedido[7] = txt_celular.Text.Trim();
						pedido[8] = txt_email.Text.Trim();
						pedido[9] = txt_direccion.Text.Trim();
						pedido[10] = txt_barrio.Text.Trim();
						pedido[11] = txt_ciudad.Text.Trim();
						pedido[12] = txt_departamento.Text.Trim();
						pedido[13] = txt_pais.Text.Trim();
						pedido[14] = txt_nota_pedido.Text.Trim();
						if (rdb_nat.Checked == true)
						{
							pedido[15] = "n";
						}
						else if (rdb_juridica.Checked == true)
						{
							pedido[15] = "j";
						}
						pedido[16] = 0;

						//------------------------------------//
						pedido[17] = lbl_agente.Text;
						pedido[18] = cmb_medio_pago.Text;
						if (cmb_recoge_tienda.SelectedIndex == 0)
						{
							pedido[19] = true;
						}
						else
						{
							pedido[19] = false;
						}
						pedido[20] = txt_id_retira.Text;
						pedido[21] = txt_nombre_retira.Text;
						pedido[22] = dtp_fecha_entrega.Value.Date;

						List<object[]> detalle = new List<object[]>();
						foreach (DataGridViewRow fila in dgv_datos.Rows)
						{
							object[] item = new object[18];
							item[0] = fila.Cells[0].Value;
							item[1] = fila.Cells[1].Value;
							item[2] = fila.Cells[2].Value;
							item[3] = fila.Cells[3].Value;
							item[4] = fila.Cells[4].Value;
							item[5] = fila.Cells[5].Value;
							item[6] = fila.Cells[6].Value;
							item[7] = fila.Cells[7].Value;
							item[8] = fila.Cells[8].Value;
							item[9] = fila.Cells[9].Value;
							item[10] = fila.Cells[10].Value;
							item[11] = fila.Cells[11].Value;
							item[12] = fila.Cells[12].Value;
							item[13] = fila.Cells[13].Value;
							item[14] = fila.Cells[15].Value;
							item[15] = fila.Cells[16].Value;
							item[16] = fila.Cells[17].Value;
							item[17] = fila.Cells[14].Value;
							detalle.Add(item);
						}
						guardar.GuardarPedido2(Convert.ToInt32(lbl_consecutivo.Text), Convert.ToByte(lbl_terminal.Text), pedido, detalle);
						lbl_terminal.Text = "";
						lbl_consecutivo.Text = "";
						lbl_consecutivo_formateado.Text = "";
						MessageBox.Show("Pedido congelado exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
						nuevoToolStripMenuItem.PerformClick();
					}
				}

				txt_id_cliente.Text = "";
				LimpiarCo();
				LimpiarCabecera();
				LimpiarItem();
				LimpiarTotales();

				grb_fuente.ForeColor = Color.Black;
				rdb_call_center.Checked = false;
				rdb_whatsapp.Checked = false;
				rdb_email.Checked = false;

				txt_id_cliente.Enabled = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void congeladoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LimpiarCo();
			LimpiarCabecera();
			LimpiarItem();
			LimpiarTotales();

			grb_fuente.ForeColor = Color.Black;
			rdb_call_center.Checked = false;
			rdb_whatsapp.Checked = false;
			rdb_email.Checked = false;

			txt_id_cliente.Enabled = false;
			txt_id_cliente.Text = "";

			nuevoToolStripMenuItem.Checked = false;
			congeladoToolStripMenuItem.Checked = true;
			lbl_consecutivo.ForeColor = Color.Red;
			lbl_consecutivo_formateado.ForeColor = Color.Red;
			lbl_terminal.Text = "";
			lbl_terminal.ForeColor = Color.Red;

			lbl_consecutivo.Text = "";
			lbl_consecutivo_formateado.Text = "";
			lbl_nro_pedido.Text = "";

			try
			{
				new FrmPedidosCongelados(this).ShowDialog(this);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			btn_guardar.Enabled = true;
		}

		private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LimpiarCo();
			LimpiarCabecera();
			LimpiarItem();
			LimpiarTotales();

			grb_fuente.ForeColor = Color.Black;
			rdb_call_center.Checked = false;
			rdb_whatsapp.Checked = false;
			rdb_email.Checked = false;

			txt_id_cliente.Enabled = false;
			txt_id_cliente.Text = "";

			nuevoToolStripMenuItem.Checked = true;
			congeladoToolStripMenuItem.Checked = false;
			lbl_consecutivo.ForeColor = Color.Black;
			lbl_consecutivo_formateado.ForeColor = Color.Black;
			lbl_terminal.Text = "";
			lbl_terminal.ForeColor = Color.Black;

			lbl_nro_pedido.Text = "";
			try
			{
				lbl_terminal.Text = Configuracion.NroTerminal;

				Terminal terminal = new Terminal();
				lbl_consecutivo.Text = terminal.ObtenerConsecutivo(Convert.ToByte(lbl_terminal.Text)).ToString();
				lbl_consecutivo_formateado.Text = "Consecutivo: T" + lbl_terminal.Text.PadLeft(2, '0') + "-" + lbl_consecutivo.Text.PadLeft(4, '0');

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			btn_guardar.Enabled = true;
		}

		private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new FrmAboutBox().ShowDialog(this);
		}

		private void cmb_co_DropDown(object sender, EventArgs e)
		{
			cmb_co.Font = new Font("Segeo UI", 20);
			cmb_co.ForeColor = Color.FromArgb(100, 100, 100);
		}

		private void cmb_co_DropDownClosed(object sender, EventArgs e)
		{
			cmb_co.Font = new Font("Segeo UI", 8);
			cmb_co.ForeColor = Color.Black;
		}

		private void btn_cargar_ultimo_Click(object sender, EventArgs e)
		{
			new FrmUltimoPedido(txt_id_cliente.Tag.ToString(), Convert.ToString(cmb_sucursal.SelectedValue).Trim(), txt_tpv.Text.Trim(), Convert.ToString(cmb_co.SelectedValue), this).ShowDialog(this);
		}

		private void rdb_call_center_CheckedChanged(object sender, EventArgs e)
		{
			txt_id_cliente.Enabled = true;
			txt_id_cliente.Focus();
			grb_fuente.ForeColor = Color.Black;
		}

		private void rdb_whatsapp_CheckedChanged(object sender, EventArgs e)
		{
			txt_id_cliente.Enabled = true;
			txt_id_cliente.Focus();
			grb_fuente.ForeColor = Color.Black;
		}

		private void rdb_email_CheckedChanged(object sender, EventArgs e)
		{
			txt_id_cliente.Enabled = true;
			txt_id_cliente.Focus();
			grb_fuente.ForeColor = Color.Black;
		}

		private void salirToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void btn_direcciones_Click(object sender, EventArgs e)
		{
			FrmDirecciones _frmdir = new FrmDirecciones(txt_id_cliente.Text, txt_barrio, txt_direccion);
			_frmdir.ShowDialog(this);
		}

		private void chk_express_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				Configuracion configuracion = new Configuracion();
				CiudadCentroOperacion ciudad_centro_operacion = new CiudadCentroOperacion();
				string[] cod_dom_express = ciudad_centro_operacion.ObternerCodigoDomicilioBarrio(Convert.ToInt32(cmb_barrio.SelectedValue));
				if (chk_express.Checked == true)
				{
					object[] conf = configuracion.ObtenerConfiguracion();
					int cant_max_express = Convert.ToInt32(conf[5]);

					if (Convert.ToInt32(txt_nro_art.Text.Trim()) > cant_max_express)
					{
						MessageBox.Show("La cantidad máxima de items para el servicio express es de " + cant_max_express.ToString(), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
						chk_express.Checked = false;
						return;
					}

					if (cod_dom_express != null)
					{
						txt_buscar.Text = cod_dom_express[1];
					}

					if (!txt_buscar.Text.Equals(""))
					{
						btn_busar_item.PerformClick();
					}
					else
					{
						MessageBox.Show("No hay código de servicio express asignado a este barrio", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
						chk_express.Checked = false;
					}
				}
				else
				{
					for (int i = 0; i < dgv_datos.Rows.Count; i++)
					{
						if (dgv_datos[0, i].Value.Equals(cod_dom_express[1]))
						{
							dgv_datos.Rows.RemoveAt(i);
						}
					}
					SumarTotales();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void cmb_recoge_tienda_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				dgv_datos.ClearSelection();
				CiudadCentroOperacion metodos = new CiudadCentroOperacion();
				string[] cod_dom = metodos.ObternerCodigoDomicilioBarrio(Convert.ToInt32(cmb_barrio.SelectedValue));
				if (cmb_recoge_tienda.SelectedIndex == 0)
				{
					txt_nombre_retira.Text = txt_nombre.Text + " " + txt_apellidos.Text;
					txt_nombre_retira.Enabled = true;
					txt_id_retira.Text = txt_id_cliente.Text;
					txt_id_retira.Enabled = true;

					for (int i = 0; i < dgv_datos.Rows.Count; i++)
					{
						if (Convert.ToString(dgv_datos[0, i].Value).Equals(cod_dom[0]))
						{
							dgv_datos.Rows[i].Selected = true;
							btn_quitar.PerformClick();
						}
					}
				}
				else if (cmb_recoge_tienda.SelectedIndex == 1)
				{
					txt_nombre_retira.Text = "";
					txt_id_retira.Text = "";

					precio_dom = true;

					if (cod_dom != null)
					{
						txt_buscar.Text = cod_dom[0];
					}

					if (!txt_buscar.Text.Equals(""))
					{
						btn_busar_item.PerformClick();
						chk_express.Enabled = true;
					}
					else
					{
						precio_dom = false;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void txt_buscar_barrio_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (dtBarrios != null)
				{
					if (dtBarrios.Rows.Count > 0)
					{
						dtBarrios.DefaultView.RowFilter = "br_nombre like'%" + txt_buscar_barrio.Text.Trim() + "%'";
						cmb_barrio.DroppedDown = true;
						Cursor.Current = Cursors.Default;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
