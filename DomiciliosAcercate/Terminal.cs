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
	public class Terminal
	{
		public int ObtenerConsecutivo(byte terminal)
		{
			string SQL = "SELECT cn_numero FROM Consecutivo WHERE cn_terminal=@TERMINAL";
			int res = -1;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@TERMINAL", terminal);
				res = Convert.ToInt32(cmd.ExecuteScalar());
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el consecutivo: " + ex.Message);
			}
			return res;
		}

		public object[] ObtenerDatosUsuario(string usuario)
		{
			string SQL = "select " +
							"cn_terminal," +
							"cn_descripcion," +
							"cn_tipo " +
						"from " +
							"consecutivo " +
						"where " +
							"cn_usuario = @usuario";
			object[] res = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@usuario", usuario);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new object[3];
					dr.Read();
					res[0] = dr[0];
					res[1] = dr[1];
					res[2] = dr[2];
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el número de la terminal: " + ex.Message);
			}
			return res;
		}

		public DataTable ListarTerminales()
		{
			string SQL = "select " +
							"cn_terminal, " +
							"cn_numero, " +
							"cn_usuario, " +
							"cn_descripcion " +
						"from " +
							"consecutivo ";
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
				throw new Exception("Error al obtener el listado de terminales:" + ex.Message);
			}
			return dt;
		}

		public void CrearTerminal(int numero, string descripcion)
		{
			string SQL = "insert into " +
							"consecutivo " +
						"values " +
						"( " +
							"@terminal, " +
							"1, " +
							"@usuario, " +
							"@descripcion, " +
							"2 " +
						")";

			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@terminal", numero);
				cmd.Parameters.AddWithValue("@usuario", "usuario" + numero.ToString());
				cmd.Parameters.AddWithValue("@descripcion", descripcion);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al crear la terminal: " + ex.Message);
			}
		}

		public void ActualizarDescripcionTerminal(int numero, string descripcion)
		{
			string SQL = "update " +
							"consecutivo " +
						"set " +
							"cn_descripcion = @descripcion " +
						"where " +
							"cn_terminal = @terminal";
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@terminal", numero);
				cmd.Parameters.AddWithValue("@descripcion", descripcion);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al actualizar la terminal: " + ex.Message);
			}
		}

	}
}
