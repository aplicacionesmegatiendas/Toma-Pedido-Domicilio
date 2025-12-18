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
	public class CiudadCentroOperacion
	{
		public void GuardarCiudad(string nombre)
		{
			string SQL = "if not exists " +
						"( " +
							"select " +
								"* " +
							"from " +
								"Ciudad " +
							"where " +
								"ci_nombre = @nombre " +
						") " +
						"begin " +
							"insert into " +
								"Ciudad " +
								"(ci_nombre) " +
							"values " +
								"( " +
									"@nombre " +
								") " +
					   "end";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@nombre", nombre);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al guardar la ciudad:" + ex.Message);
			}
		}

		public DataTable ListarCiudades()
		{
			string SQL = "select " +
							"ci_id," +
							"ci_nombre " +
						"from " +
							"Ciudad " +
					   "order by 2";
			DataTable res = null;
			try
			{
				using (SqlDataAdapter da = new SqlDataAdapter(SQL, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString))
				{
					res = new DataTable();
					da.Fill(res);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error al listar las ciudades: " + ex.Message);
			}
			return res;
		}

		public void GuardarBarrio(string nombre, int ciudad, bool activo, string cod_dom, string cod_dom_express)
		{
			string SQL = "if not exists " +
							"( " +
								"select " +
									"* " +
								"from " +
									"Barrio " +
								"where " +
									"br_nombre = @nombre and " +
									"br_id_ciudad = @ciudad " +
							") " +
							"begin " +
								"insert into " +
								"Barrio " +
								"( " +
									"br_nombre, " +
									"br_id_ciudad, " +
									"br_activo," +
									"br_cod_domicilio," +
									"br_cod_express " +
								") " +
								"values " +
								"( " +
									"@nombre, " +
									"@ciudad, " +
									"@activo," +
									"@cod_dom," +
									"@cod_dom_express " +
								") " +
						   "end ";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@nombre", nombre);
				cmd.Parameters.AddWithValue("@ciudad", ciudad);
				cmd.Parameters.AddWithValue("@activo", activo);
				cmd.Parameters.AddWithValue("@cod_dom", cod_dom);
				cmd.Parameters.AddWithValue("@cod_dom_express", cod_dom_express);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al guardar el barrio:" + ex.Message);
			}
		}

		public void EditarBarrio(int id, bool activo, string cod_dom, string cod_dom_express)
		{
			string SQL = "update " +
							"Barrio " +
						"set " +
							"br_activo = @activo," +
							"br_cod_domicilio=@cod_dom, " +
							"br_cod_express=@cod_dom_express " +
						"where " +
							"br_id = @id";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@id", id);
				cmd.Parameters.AddWithValue("@activo", activo);
				cmd.Parameters.AddWithValue("@cod_dom", cod_dom);
				cmd.Parameters.AddWithValue("@cod_dom_express", cod_dom_express);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al editar el barrio:" + ex.Message);
			}
		}

		public DataTable ListarBarriosGeneral()
		{
			string SQL = "select " +
							"br_id, " +
							"br_nombre, " +
							"ci_nombre, " +
							"ci_id, " +
							"br_activo," +
							"br_cod_domicilio," +
							"br_cod_express " +
						"from " +
							"Barrio " +
							"inner join Ciudad on br_id_ciudad = ci_id " +
						"order by 3, 2";
			DataTable res = null;
			try
			{
				using (SqlDataAdapter da = new SqlDataAdapter(SQL, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString))
				{
					res = new DataTable();
					da.Fill(res);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error al listar los barrios: " + ex.Message);
			}
			return res;
		}

		public DataTable ListarBarriosCiudad(int ciudad)
		{
			string SQL = "select " +
							"br_id, " +
							"br_nombre " +
						"from " +
							"Ciudad " +
							"inner join Barrio on ci_id = br_id_ciudad " +
						"where " +
							"ci_id = @ciudad and br_activo = 1 " +
							"order by 2";
			DataTable res = null;
			try
			{
				using (SqlDataAdapter da = new SqlDataAdapter(SQL, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString))
				{
					da.SelectCommand.Parameters.AddWithValue("@ciudad", ciudad);
					res = new DataTable();
					da.Fill(res);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error al listar los barrios: " + ex.Message);
			}
			return res;
		}

		public DataTable ObtenerBarriosDisponibles(int ciudad)
		{
			string SQL = "select " +
							"br_id, " +
							"br_nombre " +
						"from " +
							"Barrio " +
						"where " +
							"br_id not in(select zc_barrio from Cobertura where zc_barrio = br_id) and " +
							"br_id_ciudad = @ciudad and " +
							"br_activo=1 " +
						"order by 2";
			DataTable res = null;
			try
			{
				using (SqlDataAdapter da = new SqlDataAdapter(SQL, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString))
				{
					da.SelectCommand.Parameters.AddWithValue("@ciudad", ciudad);
					res = new DataTable();
					da.Fill(res);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error al listar los barrios disponibles: " + ex.Message);
			}
			return res;
		}

		public DataTable ObtenerCoberturaGeneral()
		{
			string SQL = "select " +
							"zc_id, " +
							"zc_co, " +
							"ci_nombre, " +
							"br_nombre " +
						"from " +
							"Cobertura " +
							"inner join Barrio on zc_barrio = br_id " +
							"inner join Ciudad on br_id_ciudad = ci_id " +
						"where br_activo=1 " +
						"order by 2, 4";
			DataTable res = null;
			try
			{
				using (SqlDataAdapter da = new SqlDataAdapter(SQL, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString))
				{
					res = new DataTable();
					da.Fill(res);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener cobertura general: " + ex.Message);
			}
			return res;
		}

		public object[] ObternerCoBarrio(int barrio)
		{
			string SQL = "select " +
							"zc_id_co, " +
							"zc_co " +
						"from " +
							"Cobertura " +
						"where " +
							"zc_barrio = @barrio";
			object[] res = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@barrio", barrio);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new object[2];
					dr.Read();
					res[0] = dr[0];
					res[1] = dr[1];
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el CO del barrio:" + ex.Message);
			}
			return res;
		}

		public string[] ObternerCodigoDomicilioBarrio(int barrio)
		{
			string SQL = "select " +
							"br_cod_domicilio," +
							"isnull(br_cod_express,'') br_cod_express " +
						"from " +
							"Barrio " +
						"where " +
							"br_id = @barrio";
			string[] res = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@barrio", barrio);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new string[2];
					dr.Read();
					res[0] = dr.GetString(0);
					res[1] = dr.GetString(1);
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener códigos de domicilio del barrio:" + ex.Message);
			}
			return res;
		}

		public void GuardarCobertura(List<int> barrios, string co, int id_co)
		{
			string SQL = "insert into " +
							"Cobertura " +
							"( " +
								"zc_barrio, " +
								"zc_id_co, " +
								"zc_co " +
							") " +
						"values " +
							"( " +
								"@barrio, " +
								"@id_co, " +
								"@co " +
							")";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				for (int i = 0; i < barrios.Count; i++)
				{
					cmd.Parameters.AddWithValue("@barrio", barrios[i]);
					cmd.Parameters.AddWithValue("@id_co", id_co);
					cmd.Parameters.AddWithValue("@co", co);
					cmd.ExecuteNonQuery();
					cmd.Parameters.Clear();
				}
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al guardar la cobertura:" + ex.Message);
			}
		}

		public void EliminarCobertura(int id)
		{
			string SQL = "delete " +
							"Cobertura " +
						"where " +
							"zc_id = @id";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);

				cmd.Parameters.AddWithValue("@id", id);
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();

				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al eliminar la cobertura:" + ex.Message);
			}
		}

		public void EliminarBarrio(int id)
		{
			string SQL = "delete " +
							"barrio " +
						"where " +
							"br_id = @id";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);

				cmd.Parameters.AddWithValue("@id", id);
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();

				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al eliminar el barrio:" + ex.Message);
			}
		}
		public DataTable ObtenerDatosCentroOperacion(string co)
		{
			string SQL = "select " +
							"f9750_id_co, " +
							"f9750_rowid_tercero, " +//id del 99999999
							"f200_id, " +
							"f200_razon_social, " +
							"f9750_id_suc_cliente, " +
							"f201_descripcion_sucursal, " +
							"min (f9750_rowid) as tpv " +
						"from " +
							"t9750_pdv_tpv " +
							"inner join t201_mm_clientes on f201_id_cia=f9750_id_cia and f201_rowid_tercero=f9750_rowid_tercero and f201_id_sucursal=f9750_id_suc_cliente " +
							"inner join t200_mm_terceros on f200_id_cia=f201_id_cia and f200_rowid=f201_rowid_tercero " +
						"where " +
							"f9750_id_cia=1 " +
							"and f9750_id_co=" + co +
						" group by " +
						   "f9750_id_co, " +
						   "f9750_rowid_tercero, " +
						   "f200_id, " +
						   "f200_razon_social, " +
						   "f9750_id_suc_cliente, " +
						   "f201_descripcion_sucursal;";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["unoee"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable();
				da.Fill(dt);

				conn.Close();

				return dt;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener los datos del Centro de Operación: " + ex.Message);
			}
		}

		public DataTable ListarCentrosOperacion(string co = "")
		{
			string SQL;
			if (co.Equals(""))
			{
				SQL = "SELECT f285_id, f285_descripcion " +
							  "FROM unoee_invercomer.dbo.t285_co_centro_op AS t285_co_centro_op_1 " +
							  "WHERE f285_id_portafolio IS NOT NULL AND f285_id_cia=1";
			}
			else
			{
				string[] lista = co.Split(',');
				co = "";
				foreach (string item in lista)
				{
					co += "'" + item + "',";
				}
				co = co.Trim(',');
				SQL = "SELECT f285_id, f285_descripcion " +
							 "FROM unoee_invercomer.dbo.t285_co_centro_op AS t285_co_centro_op_1 " +
							 "WHERE f285_id_portafolio IS NOT NULL AND f285_id IN(" + co + ") AND f285_id_cia=1";
			}

			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["unoee"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable();
				da.Fill(dt);

				conn.Close();

				return dt;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de Centros de Operación: " + ex.Message);
			}
		}
	}
}
