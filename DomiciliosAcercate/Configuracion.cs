using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomiciliosEntrecaminos
{
	public class Configuracion
	{
		private static byte terminal = 0;

		public static bool ultimo = false;

		private static string id_suc_cli = "";
		private static string id_lista_prec = "";
		private static string id_instalacion = "";
		private static string rowid_tercerocli = "";
		private static string id_co_docto = "";

		private static string rowiditem = "";
		private static string rowiditem_ext = "";
		private static string rowid_item_lista_precio = "";
		private static string und_med = "";
		private static decimal precio;
		private static decimal existencia;

		public static string RowidItemExt
		{
			get { return rowiditem_ext; }
			set { rowiditem_ext = value; }
		}

		public static string RowidItem
		{
			get { return rowiditem; }
			set { rowiditem = value; }
		}

		public static string RowidItemListaPrecio
		{
			get { return rowid_item_lista_precio; }
			set { rowid_item_lista_precio = value; }
		}

		public static string UnidadMedida
		{
			get { return und_med; }
			set { und_med = value; }
		}

		public static decimal Precio
		{
			get { return precio; }
			set { precio = value; }
		}

		public static decimal Existencia
		{
			get { return existencia; }
			set { existencia = value; }
		}

		public static string IdSucursalCliente
		{
			get { return id_suc_cli; }
			set { id_suc_cli = value; }
		}

		public static string IdListaPrecio
		{
			get { return id_lista_prec; }
			set { id_lista_prec = value; }
		}

		public static string IdInstalacion
		{
			get { return id_instalacion; }
			set { id_instalacion = value; }
		}

		public static string RowidTercero
		{
			get { return rowid_tercerocli; }
			set { rowid_tercerocli = value; }
		}

		public static byte Terminal
		{
			get { return terminal; }
			set { terminal = value; }
		}

		public static string IdCoDcoto
		{
			get { return id_co_docto; }
			set { id_co_docto = value; }
		}

		public static string NroTerminal { get; set; }
		public static string DescripcionUsuario { get; set; }
		public static int TipoUsuario { get; set; }

		public static char separador = Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

		public object[] ObtenerConfiguracion()
		{
			string SQL = "select " +
							"cf_usuario, " +
							"cf_clave, " +
							"cf_cos, " +
							"cf_unds_decimal, " +
							"cf_val_pedido_min, " +
							"cf_cant_max_express, " +
							"isnull(cf_lp,'') cf_lp," +
							"cf_url_pedido," +
							"cf_url_detalle," +
							"cf_url_cierre," +
							"cf_url_estado " +
						"from " +
							"Configuracion";
			object[] res = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new object[11];
					dr.Read();
					res[0] = dr[0];
					res[1] = dr[1];
					res[2] = dr[2];
					res[3] = dr[3];
					res[4] = dr[4];
					res[5] = dr[5];
					res[6] = dr[6];
					res[7] = dr[7];
					res[8] = dr[8];
					res[9] = dr[9];
					res[10] = dr[10];
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener la configuración: " + ex.Message);
			}
			return res;
		}

		public void ActualizarConfiguracion(decimal val_pedido_min, string cantidad_maxima_express, string cos)
		{
			string SQL = "update " +
							"Configuracion " +
						"set " +
							"cf_cant_max_express = @cant_max_express, " +
							"cf_val_pedido_min = @val_pedido, " +
							"cf_cos = @cos";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@cant_max_express", cantidad_maxima_express);
				cmd.Parameters.AddWithValue("@val_pedido", val_pedido_min);
				cmd.Parameters.AddWithValue("@cos", cos);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al actualizar la confuguración: " + ex.Message);
			}
		}
	}
}
