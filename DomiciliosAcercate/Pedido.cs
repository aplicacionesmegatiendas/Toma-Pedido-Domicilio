using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Policy;

namespace DomiciliosEntrecaminos
{
	public class Pedido
	{
		public void GuardarPedido(object[] pedido, List<object[]> detalle)
		{
			string SQL1 = "INSERT INTO " +
							"Pedido " +
								"(" +
									"pe_consecutivo, " +
									"pe_terminal, " +
									"pe_fecha, " +
									"pe_co, " +
									"pe_sucursal, " +
									"pe_id_cliente, " +
									"pe_nombre_cliente, " +
									"pe_apellidos_cliente, " +
									"pe_telefono, " +
									"pe_celular, " +
									"pe_email, " +
									"pe_direccion, " +
									"pe_barrio, " +
									"pe_ciudad, " +
									"pe_departamento, " +
									"pe_pais, " +
									"pe_nota, " +
									"pe_tipo_cliente, " +
									"pe_estado, " +
									"pe_fuente, " +
									"pe_ciudad_pedido, " +
									"pe_barrio_pedido, " +

									"pe_agente, " +
									"pe_medio_pago, " +
									"pe_recoge_tienda, " +
									"pe_id_cliente_recoge, " +
									"pe_nombre_cliente_recoge, " +
									"pe_fecha_entrega " +
								 ") " +
						   "VALUES " +
								"( " +
									"@CONSECUTIVO, " +
									"@TERMINAL, " +
									"getdate(), " +
									"@CO, " +
									"@SUCURSAL, " +
									"@ID_CLIENTE, " +
									"@NOMB_CLIENTE, " +
									"@APE_CLIENTE, " +
									"@TELEFONO, " +
									"@CELULAR, " +
									"@EMAIL, " +
									"@DIRECCION, " +
									"@BARRIO, " +
									"@CIUDAD, " +
									"@DPTO, " +
									"@PAIS, " +
									"@NOTA, " +
									"@TIPO_CLI, " +
									"@ESTADO, " +
									"@FUENTE, " +
									"@CIUDAD_PED, " +
									"@BARRIO_PED, " +

									"@AGENTE, " +
									"@MEDIO_PAGO, " +
									"@RECOGE_TIENDA, " +
									"@ID_CLIENTE_RECOGE, " +
									"@NOMB_CLIENTE_RECOGE, " +
									"@FECHA_ENTREGA " +
								")";

			string SQL2 = "INSERT INTO " +
								"DetallePedido " +
								"(" +
									"dp_consecutivo, " +
									"dp_terminal, " +
									"dp_item, " +
									"dp_referencia, " +
									"dp_descripcion, " +
									"dp_um, " +
									"dp_pvp, " +
									"dp_dscto_porc, " +
									"dp_dscto_val, " +
									"dp_impuesto, " +
									"dp_val_bruto_pvp, " +
									"dp_cantidad, " +
									"dp_val_bruto, " +
									"dp_val_dscto, " +
									"dp_val_impuesto, " +
									"dp_total, " +
									"dp_nota, " +
									"dp_base_impuesto, " +
									"dp_rowidext, " +
									"dp_val_unit" +
								") " +
						  "VALUES" +
								"(" +
									"@CONSECUTIVO, " +
									"@TERMINAL, " +
									"@ITEM, " +
									"@REFERENCIA, " +
									"@DESCRIPCION, " +
									"@UM, " +
									"@PVP, " +
									"@DSCTO_PORC, " +
									"@DSCTO_VAL, " +
									"@IMP, " +
									"@VAL_BRUTO_PVP, " +
									"@CANTIDAD, " +
									"@VAL_BRUTO, " +
									"@VAL_DSCTO, " +
									"@VAL_IMP, " +
									"@TOTAL, " +
									"@NOTA, " +
									"@BASE_IMP, " +
									"@ROWIDEXT, " +
									"@VAL_UNIT" +
								 ")";

			string SQL3 = "UPDATE " +
								"Consecutivo " +
						  "SET " +
								"cn_numero=cn_numero+1 " +
						  "WHERE " +
								"cn_terminal=@TERMINAL";

			string SQL_4 = @"insert into
									LogEstadoPedido
								(
									lep_consecutivo,
									lep_terminal,
									lep_estado
								)
								values
								(
									@consecutivo,
									@terminal,
									@estado
								)";

			SqlTransaction trans = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				trans = conn.BeginTransaction();
				SqlCommand cmd1 = new SqlCommand(SQL1, conn, trans);
				cmd1.CommandType = CommandType.Text;
				cmd1.Parameters.AddWithValue("@CONSECUTIVO", pedido[0]);
				cmd1.Parameters.AddWithValue("@TERMINAL", pedido[1]);
				cmd1.Parameters.AddWithValue("@CO", pedido[3]);
				cmd1.Parameters.AddWithValue("@SUCURSAL", pedido[4]);
				cmd1.Parameters.AddWithValue("@ID_CLIENTE", pedido[5]);
				cmd1.Parameters.AddWithValue("@NOMB_CLIENTE", pedido[6]);
				cmd1.Parameters.AddWithValue("@APE_CLIENTE", pedido[7]);
				cmd1.Parameters.AddWithValue("@TELEFONO", pedido[8]);
				cmd1.Parameters.AddWithValue("@CELULAR", pedido[9]);
				cmd1.Parameters.AddWithValue("@EMAIL", pedido[10]);
				cmd1.Parameters.AddWithValue("@DIRECCION", pedido[11]);
				cmd1.Parameters.AddWithValue("@BARRIO", pedido[12]);
				cmd1.Parameters.AddWithValue("@CIUDAD", pedido[13]);
				cmd1.Parameters.AddWithValue("@DPTO", pedido[14]);
				cmd1.Parameters.AddWithValue("@PAIS", pedido[15]);
				cmd1.Parameters.AddWithValue("@NOTA", pedido[16]);
				cmd1.Parameters.AddWithValue("@TIPO_CLI", pedido[17]);
				cmd1.Parameters.AddWithValue("@ESTADO", pedido[18]);
				cmd1.Parameters.AddWithValue("@FUENTE", pedido[19]);
				cmd1.Parameters.AddWithValue("@CIUDAD_PED", pedido[20]);
				cmd1.Parameters.AddWithValue("@BARRIO_PED", pedido[21]);

				cmd1.Parameters.AddWithValue("@AGENTE", pedido[22]);
				cmd1.Parameters.AddWithValue("@MEDIO_PAGO", pedido[23]);
				cmd1.Parameters.AddWithValue("@RECOGE_TIENDA", pedido[24]);
				cmd1.Parameters.AddWithValue("@ID_CLIENTE_RECOGE", pedido[25]);
				cmd1.Parameters.AddWithValue("@NOMB_CLIENTE_RECOGE", pedido[26]);
				cmd1.Parameters.AddWithValue("@FECHA_ENTREGA", pedido[27]);
				cmd1.ExecuteNonQuery();

				SqlCommand cmd2 = new SqlCommand(SQL2, conn, trans);
				cmd2.CommandType = CommandType.Text;
				foreach (object[] item in detalle)
				{
					cmd2.Parameters.AddWithValue("@CONSECUTIVO", item[0]);
					cmd2.Parameters.AddWithValue("@TERMINAL", item[1]);
					cmd2.Parameters.AddWithValue("@ITEM", item[2]);
					cmd2.Parameters.AddWithValue("@REFERENCIA", item[3]);
					cmd2.Parameters.AddWithValue("@DESCRIPCION", item[4]);
					cmd2.Parameters.AddWithValue("@UM", item[5]);
					cmd2.Parameters.AddWithValue("@PVP", item[6]);
					cmd2.Parameters.AddWithValue("@DSCTO_PORC", item[7]);
					cmd2.Parameters.AddWithValue("@DSCTO_VAL", item[8]);
					cmd2.Parameters.AddWithValue("@IMP", item[9]);
					cmd2.Parameters.AddWithValue("@VAL_BRUTO_PVP", item[10]);
					cmd2.Parameters.AddWithValue("@CANTIDAD", item[11]);
					cmd2.Parameters.AddWithValue("@VAL_BRUTO", item[12]);
					cmd2.Parameters.AddWithValue("@VAL_DSCTO", item[13]);
					cmd2.Parameters.AddWithValue("@VAL_IMP", item[14]);
					cmd2.Parameters.AddWithValue("@TOTAL", item[15]);
					cmd2.Parameters.AddWithValue("@NOTA", item[16]);
					cmd2.Parameters.AddWithValue("@BASE_IMP", item[17]);
					cmd2.Parameters.AddWithValue("@ROWIDEXT", item[18]);
					cmd2.Parameters.AddWithValue("@VAL_UNIT", item[19]);
					cmd2.ExecuteNonQuery();
					cmd2.Parameters.Clear();
				}

				SqlCommand cmd3 = new SqlCommand(SQL3, conn, trans);
				cmd3.CommandType = CommandType.Text;
				cmd3.Parameters.AddWithValue("@TERMINAL", pedido[1]);
				cmd3.ExecuteNonQuery();

				if (Convert.ToInt32(pedido[18]) == 1)
				{
					SqlCommand cmd4 = new SqlCommand(SQL_4, conn, trans);
					cmd4.CommandType = CommandType.Text;
					cmd4.Parameters.AddWithValue("@consecutivo", pedido[0]);
					cmd4.Parameters.AddWithValue("@terminal", pedido[1]);
					cmd4.Parameters.AddWithValue("@estado", 1);
					cmd4.ExecuteNonQuery();
				}
				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				throw new Exception("Error al guardar el pedido: " + ex.Message);
			}
		}

		public void GuardarPedido2(int consecutivo, byte terminal, object[] pedido, List<object[]> detalle)
		{
			string SQL = "UPDATE Pedido " +
							"SET " +
								"pe_co=@CO, " +
								"pe_sucursal=@SUCURSAL, " +
								"pe_id_cliente=@ID_CLIENTE, " +
								"pe_nombre_cliente=@NOMB_CLIENTE, " +
								"pe_apellidos_cliente=@APE_CLIENTE, " +
								"pe_telefono=@TELEFONO, " +
								"pe_celular=@CELULAR, " +
								"pe_email=@EMAIL, " +
								"pe_direccion=@DIRECCION, " +
								"pe_barrio=@BARRIO, " +
								"pe_ciudad=@CIUDAD, " +
								"pe_departamento=@DPTO, " +
								"pe_pais=@PAIS, " +
								"pe_nota=@NOTA, " +
								"pe_tipo_cliente=@TIPO_CLI, " +
								"pe_estado=@ESTADO, " +

								"pe_agente=@AGENTE, " +
								"pe_medio_pago=@MEDIO_PAGO, " +
								"pe_recoge_tienda=@RECOGE_TIENDA, " +
								"pe_id_cliente_recoge=@ID_CLIENTE_RECOGE, " +
								"pe_nombre_cliente_recoge=@NOMB_CLIENTE_RECOGE, " +
								"pe_fecha_entrega=@FECHA_ENTREGA " +
						"WHERE " +
							"pe_consecutivo=@CONSECUTIVO AND " +
							"pe_terminal= @TERMINAL";

			string SQL1 = "DELETE " +
							   "DetallePedido " +
						  "WHERE " +
								"dp_consecutivo=@CONSECUTIVO AND " +
								"dp_terminal=@TERMINAL";

			string SQL2 = "INSERT INTO " +
								"DetallePedido " +
								"(" +
									"dp_consecutivo, " +
									"dp_terminal, " +
									"dp_item, " +
									"dp_referencia," +
									"dp_descripcion, " +
									"dp_um, " +
									"dp_pvp, " +
									"dp_dscto_porc, " +
									"dp_dscto_val, " +
									"dp_impuesto, " +
									"dp_val_bruto_pvp, " +
									"dp_cantidad, " +
									"dp_val_bruto, " +
									"dp_val_dscto, " +
									"dp_val_impuesto, " +
									"dp_total, " +
									"dp_nota, " +
									"dp_base_impuesto, " +
									"dp_rowidext, " +
									"dp_val_unit" +
								") " +
								"VALUES" +
									"(" +
										"@CONSECUTIVO, " +
										"@TERMINAL, " +
										"@ITEM, " +
										"@REFERENCIA, " +
										"@DESCRIPCION, " +
										"@UM, " +
										"@PVP, " +
										"@DSCTO_PORC, " +
										"@DSCTO_VAL, " +
										"@IMP, " +
										"@VAL_BRUTO_PVP, " +
										"@CANTIDAD, " +
										"@VAL_BRUTO, " +
										"@VAL_DSCTO, " +
										"@VAL_IMP, " +
										"@TOTAL, " +
										"@NOTA, " +
										"@BASE_IMP," +
										"@ROWIDEXT, " +
										"@VAL_UNIT" +
									")";

			string SQL_4 = @"insert into
									LogEstadoPedido
								(
									lep_consecutivo,
									lep_terminal,
									lep_estado
								)
								values
								(
									@consecutivo,
									@terminal,
									@estado
								)";

			SqlTransaction trans = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				trans = conn.BeginTransaction();

				SqlCommand cmd3 = new SqlCommand(SQL, conn, trans);
				cmd3.CommandType = CommandType.Text;
				// cmd3.Parameters.AddWithValue("@FECHA", pedido[0]);
				cmd3.Parameters.AddWithValue("@CO", pedido[1]);
				cmd3.Parameters.AddWithValue("@SUCURSAL", pedido[2]);
				cmd3.Parameters.AddWithValue("@ID_CLIENTE", pedido[3]);
				cmd3.Parameters.AddWithValue("@NOMB_CLIENTE", pedido[4]);
				cmd3.Parameters.AddWithValue("@APE_CLIENTE", pedido[5]);
				cmd3.Parameters.AddWithValue("@TELEFONO", pedido[6]);
				cmd3.Parameters.AddWithValue("@CELULAR", pedido[7]);
				cmd3.Parameters.AddWithValue("@EMAIL", pedido[8]);
				cmd3.Parameters.AddWithValue("@DIRECCION", pedido[9]);
				cmd3.Parameters.AddWithValue("@BARRIO", pedido[10]);
				cmd3.Parameters.AddWithValue("@CIUDAD", pedido[11]);
				cmd3.Parameters.AddWithValue("@DPTO", pedido[12]);
				cmd3.Parameters.AddWithValue("@PAIS", pedido[13]);
				cmd3.Parameters.AddWithValue("@NOTA", pedido[14]);
				cmd3.Parameters.AddWithValue("@TIPO_CLI", pedido[15]);
				cmd3.Parameters.AddWithValue("@ESTADO", pedido[16]);

				cmd3.Parameters.AddWithValue("@AGENTE", pedido[17]);
				cmd3.Parameters.AddWithValue("@MEDIO_PAGO", pedido[18]);
				cmd3.Parameters.AddWithValue("@RECOGE_TIENDA", pedido[19]);
				cmd3.Parameters.AddWithValue("@ID_CLIENTE_RECOGE", pedido[20]);
				cmd3.Parameters.AddWithValue("@NOMB_CLIENTE_RECOGE", pedido[21]);
				cmd3.Parameters.AddWithValue("@FECHA_ENTREGA", pedido[22]);

				cmd3.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);
				cmd3.Parameters.AddWithValue("@TERMINAL", terminal);
				cmd3.ExecuteNonQuery();

				SqlCommand cmd1 = new SqlCommand(SQL1, conn, trans);
				cmd1.CommandType = CommandType.Text;
				cmd1.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);
				cmd1.Parameters.AddWithValue("@TERMINAL", terminal);
				cmd1.ExecuteNonQuery();

				SqlCommand cmd2 = new SqlCommand(SQL2, conn, trans);
				cmd2.CommandType = CommandType.Text;
				foreach (object[] item in detalle)
				{
					cmd2.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);
					cmd2.Parameters.AddWithValue("@TERMINAL", terminal);
					cmd2.Parameters.AddWithValue("@ITEM", item[0]);
					cmd2.Parameters.AddWithValue("@REFERENCIA", item[1]);
					cmd2.Parameters.AddWithValue("@DESCRIPCION", item[2]);
					cmd2.Parameters.AddWithValue("@UM", item[3]);
					cmd2.Parameters.AddWithValue("@PVP", item[4]);
					cmd2.Parameters.AddWithValue("@DSCTO_PORC", item[5]);
					cmd2.Parameters.AddWithValue("@DSCTO_VAL", item[6]);
					cmd2.Parameters.AddWithValue("@IMP", item[7]);
					cmd2.Parameters.AddWithValue("@VAL_BRUTO_PVP", item[8]);
					cmd2.Parameters.AddWithValue("@CANTIDAD", item[9]);
					cmd2.Parameters.AddWithValue("@VAL_BRUTO", item[10]);
					cmd2.Parameters.AddWithValue("@VAL_DSCTO", item[11]);
					cmd2.Parameters.AddWithValue("@VAL_IMP", item[12]);
					cmd2.Parameters.AddWithValue("@TOTAL", item[13]);
					cmd2.Parameters.AddWithValue("@NOTA", item[14]);
					cmd2.Parameters.AddWithValue("@BASE_IMP", item[15]);
					cmd2.Parameters.AddWithValue("@ROWIDEXT", item[16]);
					cmd2.Parameters.AddWithValue("@VAL_UNIT", item[17]);
					cmd2.ExecuteNonQuery();
					cmd2.Parameters.Clear();
				}

				if (Convert.ToInt32(pedido[16]) == 1)
				{
					SqlCommand cmd4 = new SqlCommand(SQL_4, conn, trans);
					cmd4.CommandType = CommandType.Text;
					cmd4.Parameters.AddWithValue("@consecutivo", pedido[0]);
					cmd4.Parameters.AddWithValue("@terminal", pedido[1]);
					cmd4.Parameters.AddWithValue("@estado", 1);
					cmd4.ExecuteNonQuery();
				}

				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				throw new Exception("Error al guardar el pedido: " + ex.Message);
			}
		}

		public void GuardarNumeroPedido(int consecutivo, byte terminal, string numero)
		{
			string SQL1 = "UPDATE " +
							"Pedido " +
						  "SET " +
							"pe_numero=@NUMERO " +
						  "WHERE " +
							"pe_terminal=@TERMINAL AND " +
							"pe_consecutivo = @CONSECUTIVO";

			string SQL2 = "UPDATE " +
							"DetallePedido " +
						  "SET " +
							"dp_numero=@NUMERO " +
						  "WHERE " +
							"dp_terminal=@TERMINAL AND " +
							"dp_consecutivo = @CONSECUTIVO";

			SqlTransaction trans = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				trans = conn.BeginTransaction();
				SqlCommand cmd1 = new SqlCommand(SQL1, conn, trans);
				cmd1.CommandType = CommandType.Text;
				cmd1.Parameters.AddWithValue("@NUMERO", numero);
				cmd1.Parameters.AddWithValue("@TERMINAL", terminal);
				cmd1.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);
				cmd1.ExecuteNonQuery();

				SqlCommand cmd2 = new SqlCommand(SQL2, conn, trans);
				cmd2.CommandType = CommandType.Text;
				cmd2.Parameters.AddWithValue("@NUMERO", numero);
				cmd2.Parameters.AddWithValue("@TERMINAL", terminal);
				cmd2.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);
				cmd2.ExecuteNonQuery();
				cmd2.Parameters.Clear();

				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				throw new Exception("Error al guardar el pedido: " + ex.Message);
			}
		}

		public DataSet ConsultarPedidos(string fecha_ini, string fecha_fin, byte terminal)
		{
			string SQL1 = "SELECT " +
							"pe_consecutivo, " +
							"pe_terminal, " +
							"ISNULL(pe_numero,'') pe_numero, " +
							"pe_fecha, " +
							"pe_co, " +
							"pe_id_cliente, " +
							"pe_nombre_cliente, " +
							"pe_apellidos_cliente, " +
							"pe_telefono, " +
							"ISNULL(pe_celular,'') pe_celular, " +
							"pe_email, " +
							"pe_direccion, " +
							"pe_barrio, " +
							"pe_ciudad, " +
							"pe_departamento, " +
							"pe_pais, " +
							"pe_nota " +
						  "FROM " +
							"Pedido " +
						  "WHERE " +
							"CONVERT(DATE,pe_fecha) BETWEEN @FECHA_INI AND @FECHA_FIN AND " +
							"pe_terminal=@TERMINAL";

			string SQL2 = "SELECT " +
							"dp_consecutivo, " +
							"dp_terminal, " +
							"dp_item,  " +
							"dp_referencia, " +
							"dp_descripcion, " +
							"dp_um, " +
							"dp_pvp, " +
							"dp_dscto_porc, " +
							"dp_dscto_val, " +
							"dp_impuesto, " +
							"dp_val_bruto_pvp, " +
							"dp_cantidad, " +
							"dp_val_bruto, " +
							"dp_val_dscto, " +
							"dp_val_impuesto, " +
							"dp_total, " +
							"dp_nota " +
						  "FROM " +
							"DetallePedido " +
						  "WHERE " +
							"dp_terminal=@TERMINAL AND " +
							"dp_consecutivo IN(";
			try
			{
				DataSet ds = new DataSet();

				SqlDataAdapter da1 = new SqlDataAdapter(SQL1, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);

				da1.SelectCommand.Parameters.AddWithValue("@FECHA_INI", fecha_ini);
				da1.SelectCommand.Parameters.AddWithValue("@FECHA_FIN", fecha_fin);
				da1.SelectCommand.Parameters.AddWithValue("@TERMINAL", terminal);
				DataTable dt1 = new DataTable("Pedidos");
				ds.Tables.Add(dt1);
				da1.Fill(ds, "Pedidos");

				if (ds.Tables["Pedidos"].Rows.Count > 0)
				{
					foreach (DataRow item in ds.Tables["Pedidos"].Rows)
					{
						SQL2 = SQL2 + "'" + item[0] + "',";
					}
					SQL2 = SQL2.Trim(',') + ")";
					SqlDataAdapter da2 = new SqlDataAdapter(SQL2, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
					DataTable dt2 = new DataTable("Detalle");
					da2.SelectCommand.Parameters.AddWithValue("@TERMINAL", terminal);
					ds.Tables.Add(dt2);
					da2.Fill(ds, "Detalle");
					DataColumn[] dc_p = new DataColumn[2];
					DataColumn[] dc_c = new DataColumn[2];

					dc_p[0] = ds.Tables[0].Columns[0];
					dc_p[1] = ds.Tables[0].Columns[1];

					dc_c[0] = ds.Tables[1].Columns[0];
					dc_c[1] = ds.Tables[1].Columns[1];

					ds.Relations.Add("rel1", dc_p, dc_c);
				}
				else
				{
					ds.Tables.Clear();
				}
				return ds;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al consultar los pedidos: " + ex.Message);
			}
		}

		public DataSet ConsultarPedido(int consecutivo, byte terminal)
		{
			string SQL1 = "SELECT " +
							"pe_consecutivo, " +
							"pe_terminal, " +
							"pe_numero, " +
							"pe_fecha, " +
							"pe_co, " +
							"pe_id_cliente, " +
							"pe_nombre_cliente, " +
							"pe_apellidos_cliente, " +
							"pe_telefono, " +
							"pe_celular, " +
							"pe_email, " +
							"pe_direccion, " +
							"pe_barrio, " +
							"pe_ciudad, " +
							"pe_departamento, " +
							"pe_pais, " +
							"pe_nota, " +
							"pe_tipo_cliente, " +
							"ISNULL(pe_fuente,'') pe_fuente " +
						  "FROM " +
							"Pedido " +
						  "WHERE " +
							"pe_consecutivo=@CONSECUTIVO AND " +
							"pe_terminal=@TERMINAL AND " +
							"pe_estado IN(1)";

			string SQL2 = "SELECT " +
							"dp_consecutivo, " +
							"dp_terminal, " +
							"dp_item,  " +
							"dp_referencia, " +
							"dp_descripcion, " +
							"dp_um, " +
							"dp_pvp, " +
							"dp_dscto_porc, " +
							"dp_dscto_val, " +
							"dp_impuesto, " +
							"dp_val_bruto_pvp, " +
							"dp_cantidad, " +
							"dp_val_bruto, " +
							"dp_val_dscto, " +
							"dp_val_impuesto, " +
							"dp_total, " +
							"dp_nota " +
						  "FROM " +
							"DetallePedido " +
						  "WHERE " +
							"dp_consecutivo=@CONSECUTIVO AND " +
							"dp_terminal=@TERMINAL AND " +
							"dp_enviado=0";
			try
			{
				DataSet ds = new DataSet();

				SqlDataAdapter da1 = new SqlDataAdapter(SQL1, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);

				da1.SelectCommand.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);
				da1.SelectCommand.Parameters.AddWithValue("@TERMINAL", terminal);
				DataTable dt1 = new DataTable("Pedido");
				ds.Tables.Add(dt1);
				da1.Fill(ds, "Pedido");

				if (ds.Tables["Pedido"].Rows.Count > 0)
				{
					SqlDataAdapter da2 = new SqlDataAdapter(SQL2, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
					DataTable dt2 = new DataTable("Detalle");
					da2.SelectCommand.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);
					da2.SelectCommand.Parameters.AddWithValue("@TERMINAL", terminal);
					ds.Tables.Add(dt2);
					da2.Fill(ds, "Detalle");
				}
				else
				{
					ds.Tables.Clear();
				}
				return ds;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al cargar el pedido: " + ex.Message);
			}
		}

		public void ActualizarEstadoEnviado(int consecutivo, byte terminal, string item)
		{
			string SQL = "UPDATE " +
							"DetallePedido " +
						 "SET " +
							"dp_enviado=1 " +
						 "WHERE " +
							"dp_consecutivo=@CONSECUTIVO AND " +
							"dp_terminal=@TERMINAL AND " +
							"dp_item=@ITEM";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();

				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);
				cmd.Parameters.AddWithValue("@TERMINAL", terminal);
				cmd.Parameters.AddWithValue("@ITEM", item);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al actualizar el estado de enviado: " + ex.Message);
			}
		}

		public void ActualizarEstadoPedido(int consecutivo, byte terminal, byte estado)
		{
			string SQL = "UPDATE " +
							"Pedido " +
						 "SET " +
							"pe_estado=@ESTADO " +
						 "WHERE " +
							"pe_consecutivo=@CONSECUTIVO AND " +
							"pe_terminal=@TERMINAL";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();

				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@ESTADO", estado);
				cmd.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);
				cmd.Parameters.AddWithValue("@TERMINAL", terminal);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al actualizar el estado del pedido: " + ex.Message);
			}
		}

		public DataTable ObtenerPedidosCongelados()
		{
			string SQL = "SELECT " +
							"pe_consecutivo, " +
							"pe_terminal, " +
							"CONVERT(DATE,pe_fecha) AS pe_fecha, " +
							"pe_co, " +
							"pe_sucursal, " +
							"pe_id_cliente, " +
							"pe_nombre_cliente + ' ' + pe_apellidos_cliente AS nombre, " +
							"pe_nota, " +
							"isnull(pe_tipo_cliente,'')pe_tipo_cliente," +
							"isnull(pe_fuente,'') pe_fuente, " +
							"isnull(pe_ciudad_pedido, '') pe_ciudad_pedido, " +
							"isnull(pe_barrio_pedido, '') pe_barrio_pedido " +
						"FROM " +
							"Pedido " +
						"WHERE " +
							"pe_estado=0";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de pedidos congelados:" + ex.Message);
			}
			return dt;
		}

		public object[] ConsultarEncabezadoPedido(string consecutivo, string terminal, string identificacion)
		{
			string SQL = "select " +
							"pe_ciudad, " +
							"pe_barrio, " +
							"pe_direccion, " +
							"pe_telefono, " +
							"pe_agente, " +
							"pe_medio_pago, " +
							"pe_recoge_tienda, " +
							"pe_id_cliente_recoge, " +
							"pe_nombre_cliente_recoge, " +
							"pe_fecha_entrega " +
						"from " +
							"Pedido " +
						"where " +
							"pe_consecutivo = @consecutivo and " +
							"pe_terminal = @terminal and " +
							"pe_id_cliente = @cliente";

			object[] res = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@consecutivo", consecutivo);
				cmd.Parameters.AddWithValue("@terminal", terminal);
				cmd.Parameters.AddWithValue("@cliente", identificacion);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new object[10];
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
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al cargar el encabezado del pedido: " + ex.Message);
			}
			return res;
		}

		public DataTable ConsultarDetallePedido(int consecutivo, byte terminal)
		{
			string SQL = "SELECT " +
							"dp_consecutivo, " +
							"dp_terminal, " +
							"dp_item,  " +
							"dp_referencia, " +
							"dp_descripcion, " +
							"dp_um, " +
							"dp_pvp, " +
							"dp_dscto_porc, " +
							"dp_dscto_val, " +
							"dp_impuesto, " +
							"dp_val_bruto_pvp, " +
							"dp_cantidad, " +
							"dp_val_bruto, " +
							"dp_val_dscto, " +
							"dp_val_impuesto, " +
							"dp_total, " +
							"dp_nota, " +
							"dp_base_impuesto, " +
							"dp_rowidext, " +
							"dp_val_unit " +
						 "FROM " +
							"DetallePedido " +
						 "WHERE " +
							"dp_consecutivo=@CONSECUTIVO AND " +
							"dp_terminal=@TERMINAL";
			DataTable dt = null;
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(SQL, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				da.SelectCommand.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);
				da.SelectCommand.Parameters.AddWithValue("@TERMINAL", terminal);
				dt = new DataTable("Detalle");
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				throw new Exception("Error al cargar el detalle del pedido: " + ex.Message);
			}
			return dt;
		}

		public DataTable ListarPedidosEnviar()
		{
			string SQL = "SELECT DISTINCT " +
							"pe_consecutivo, " +
							"pe_terminal, " +
							"pe_numero " +
						"FROM " +
							"Pedido " +
						"INNER JOIN DetallePedido ON pe_terminal=dp_terminal AND pe_consecutivo=dp_consecutivo " +
						"WHERE " +
							"pe_estado IN(1, 2) and " +
							"dp_enviado=0 " +
						"ORDER BY 2";
			DataTable dt = null;
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(SQL, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				dt = new DataTable();
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				throw new Exception("Error al listar pedidos a enviar: " + ex.Message);
			}
			return dt;
		}

		public void GuardarLog(byte terminal, int consecutivo, bool congelado)
		{
			string SQL = "";
			if (congelado.Equals(true))
			{
				SQL = "INSERT INTO LogPedidos " +
						"(" +
							"lo_terminal_origen, " +
							"lo_consecutivo, " +
							"lo_fecha_origen" +
						") " +
						"VALUES" +
						"(" +
							"@TERMINAL_ORG, " +
							"@CONSECUTIVO, " +
							"GETDATE()" +
						")";
			}
			else
			{
				SQL = "INSERT INTO LogPedidos " +
					"( " +
						"lo_terminal_origen, " +
						"lo_consecutivo, " +
						"lo_fecha_origen, " +
						"lo_terminal_fin, " +
						"lo_fecha_fin " +
					") " +
					"VALUES" +
					"(" +
						"@TERMINAL_ORG, " +
						"@CONSECUTIVO, " +
						"GETDATE(), " +
						"@TERMINAL_FIN, " +
						"GETDATE()" +
					")";
			}

			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				if (congelado.Equals(true))
				{
					cmd.Parameters.AddWithValue("@TERMINAL_ORG", terminal);
					cmd.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);
				}
				else
				{
					cmd.Parameters.AddWithValue("@TERMINAL_ORG", terminal);
					cmd.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);
					cmd.Parameters.AddWithValue("@TERMINAL_FIN", terminal);
				}
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al guardar el log: " + ex.Message);
			}
		}

		public void GuardarLog(byte terminal_origen, int consecutivo, byte terminal_fin)
		{
			string SQL = "UPDATE " +
							"LogPedidos " +
						 "SET " +
							"lo_terminal_fin=@TERMINAL_FIN, " +
							"lo_fecha_fin=GETDATE() " +
						 "WHERE " +
							"lo_terminal_origen=@TERMINAL_ORG AND " +
							"lo_consecutivo=@CONSECUTIVO";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@TERMINAL_FIN", terminal_fin);
				cmd.Parameters.AddWithValue("@TERMINAL_ORG", terminal_origen);
				cmd.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);

				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al guardar el log: " + ex.Message);
			}
		}

		public void GuardarLogEnvio(int? consecutivo, string numero, byte? terminal, string id_cliente, string json_enviado, string json_recibido, string accion)
		{
			string SQL = "insert into " +
							"LogEnvio " +
						"( " +
							"le_consecutivo, " +
							"le_numero, " +
							"le_terminal, " +
							"le_id_cliente, " +
							"le_json_enviado, " +
							"le_json_recibido, " +
							"le_accion " +
						") " +
						"values " +
						"( " +
							"@consecutivo, " +
							"@numero, " +
							"@terminal, " +
							"@id_cliente, " +
							"@json_env, " +
							"@json_rec, " +
							"@accion " +
						")";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				if (consecutivo == null)
				{
					cmd.Parameters.AddWithValue("@consecutivo", DBNull.Value);
				}
				else
				{
					cmd.Parameters.AddWithValue("@consecutivo", consecutivo);
				}
				cmd.Parameters.AddWithValue("@numero", numero);
				if (terminal == null)
				{
					cmd.Parameters.AddWithValue("@terminal", DBNull.Value);
				}
				else
				{
					cmd.Parameters.AddWithValue("@terminal", terminal);
				}
				cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
				cmd.Parameters.AddWithValue("@json_env", json_enviado);
				cmd.Parameters.AddWithValue("@json_rec", json_recibido);
				cmd.Parameters.AddWithValue("@accion", accion);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al guardar el log de envio: " + ex.Message);
			}
		}

		public DataTable ConsultarPedidosCliente(string identificacion)
		{
			string SQL = "SELECT " +
							"pe_fecha, " +
							"pe_numero " +
						 "FROM " +
							"Pedido " +
						 "WHERE " +
							"pe_id_cliente=@ID_CLI AND " +
							"pe_estado=1";
			DataTable dt = null;
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(SQL, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				da.SelectCommand.Parameters.AddWithValue("@ID_CLI", identificacion);
				dt = new DataTable();
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				throw new Exception("Error al consultar los pedidos por cliente:" + ex.Message);
			}
			return dt;
		}

		public void CancelarPedido(int consecutivo, byte terminal)
		{
			string SQL = "UPDATE " +
							"Pedido " +
						 "SET " +
							"pe_estado=3 " +
						 "WHERE " +
							"pe_consecutivo=@CONSECUTIVO AND " +
							"pe_terminal=@TERMINAL";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);
				cmd.Parameters.AddWithValue("@TERMINAL", terminal);
				cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al cancelar el pedido: " + ex.Message);
			}
		}

		public void CancelarPedido()
		{
			string SQL = "UPDATE " +
							"Pedido " +
						 "SET " +
							"pe_estado=3 " +
						 "WHERE " +
							"CONVERT(DATE,pe_fecha) < CONVERT(DATE,getdate()) AND " +
							"pe_estado=0";

			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);

				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al cancelar los pedidos: " + ex.Message);
			}
		}

		public DataTable ObtenerUltimoPedido(string identificacion, DateTime fecha)
		{
			string SQL = "select " +
						   "pe_fecha, " +
						   "pe_id_cliente, " +
						   "pe_consecutivo, " +
						   "pe_terminal, " +
						   "pe_numero, " +
						   "dp_item, " +
						   "dp_referencia, " +
						   "dp_descripcion, " +
						   "rtrim(dp_um) dp_um, " +
						   "dp_pvp, " +
						   "dp_cantidad, " +
						   "dp_val_unit, " +
						   "dp_dscto_porc, " +
						   "dp_dscto_val," +
						   "dp_total, " +
						   "dp_rowidext " +
						 "from " +
							"pedido " +
						 "inner join " +
							"detallepedido on dp_consecutivo = pe_consecutivo and dp_terminal = pe_terminal " +
						 "where " +
							"pe_id_cliente = @id_cli " +
							"and pe_fecha = @fecha and pe_estado=1" +
						"order by 4";

			DataTable dt = null;
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(SQL, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				da.SelectCommand.Parameters.AddWithValue("@id_cli", identificacion);
				da.SelectCommand.Parameters.AddWithValue("@fecha", fecha);
				dt = new DataTable();
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				throw new Exception("Error al consultar el pedido por cliente:" + ex.Message);
			}
			return dt;
		}

		public List<string> ObtenerFechasUltimoPedido(string desde, string hasta, string identificacion)
		{
			string SQL = "select " +
						   "convert(varchar, pe_fecha, 25) pe_fecha " +
						 "from " +
							"pedido " +
						 "where " +
							"pe_id_cliente = @id_cli " +
							"and convert(date,pe_fecha) between @desde and @hasta and pe_estado=1" +
						"order by 1 desc";

			List<string> res = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@id_cli", identificacion);
				cmd.Parameters.AddWithValue("@desde", desde);
				cmd.Parameters.AddWithValue("@hasta", hasta);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new List<string>();
					while (dr.Read())
					{
						res.Add(dr.GetString(0));
					}
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener las fechas de pedidos:" + ex.Message);
			}
			return res;
		}

		public DataTable ObtenerDireccionesCliente(string id)
		{
			string SQL = "select distinct " +
						   "pe_barrio," +
						   "pe_direccion," +
						   "pe_ciudad " +
						 "from " +
							"pedido " +
						 "where " +
							"pe_id_cliente = @id ";
			DataTable res = null;
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(SQL, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				da.SelectCommand.Parameters.AddWithValue("@id", id);
				res = new DataTable();
				da.Fill(res);
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener las direcciones: " + ex.Message);
			}
			return res;
		}

		public string ConsultarEstadoPedido(int terminal, int consecutivo)
		{
			string SQL = @"select
								es_descripcion
							from
								Pedido
								inner join Estados on pe_estado=es_cod
							where
								pe_terminal=@terminal
								and pe_consecutivo=@consecutivo
								and es_cod in(1,4,5,6,7,8,9,10,11)";
			string res = "";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@terminal", terminal);
				cmd.Parameters.AddWithValue("@consecutivo", consecutivo);

				res = Convert.ToString(cmd.ExecuteScalar());

				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al consultar estado del pedido:" + ex.Message);
			}
			return res;
		}

		public string ConsultarEstadoPedidoTercero(string consecutivo)
		{
			string SQL = @"select
								es_descripcion
							from
								PedidoTercero
								inner join Estados on pt_estado=es_cod
							where
								pt_consecutivo=@consecutivo
								and es_cod in(1,4,5,6,7,8,9,10,11)";
			string res = "";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@consecutivo", consecutivo);

				res = Convert.ToString(cmd.ExecuteScalar());

				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al consultar estado del pedido:" + ex.Message);
			}
			return res;
		}
	}
}
