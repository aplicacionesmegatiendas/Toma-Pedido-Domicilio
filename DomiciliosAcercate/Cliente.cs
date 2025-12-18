using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomiciliosEntrecaminos
{
	public class Cliente
	{
		public string[] ObtenerDatosCliente(string id, string tipo)
		{
			string SQL = "select distinct " +
							   "f9740_id, ";
			if (tipo.Equals("n"))
			{
				SQL += "f9740_nombre nombre, " +
					   "f9740_apellido_1 + ' ' + f9740_apellido_2 apellidos, ";
			}
			else if (tipo.Equals("j"))
			{
				SQL += "f9740_razon_social, ";
			}

			SQL += "f9740_direccion1 + ' | ' + f9740_direccion2 + ' | ' + f9740_direccion3 direccion, " +
			 "f9740_telefono, " +
			 "f9740_celular, " +
			 "f9740_email, " +
			 "isnull(f9740_id_barrio,'') barrio, " +
			 "f013_id id_ciudad, " +
			 "f013_descripcion ciudad, " +
			 "f012_descripcion departamento, " +
			 "f011_descripcion pais " +
		  "from " +
			 "t9740_pdv_clientes " +
			 "left outer join t013_mm_ciudades on f013_id_pais=f9740_id_pais and f013_id_depto=f9740_id_depto and f013_id=f9740_id_ciudad " +
			 "left outer join t012_mm_deptos on f012_id_pais=f9740_id_pais and f012_id=f9740_id_depto " +
			 "left outer join t011_mm_paises on f011_id=f9740_id_pais " +
		  "where " +
			 "f9740_id_cia=1 " +
			 "and f9740_id=@ID_CLIENTE";

			string[] res = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["unoee"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@ID_CLIENTE", id);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					if (tipo.Equals("n"))
					{
						res = new string[12];
						dr.Read();
						res[0] = dr.GetString(0);
						res[1] = dr.GetString(1);
						res[2] = dr.GetString(2);
						res[3] = dr.GetString(3);
						res[4] = dr.GetString(4);
						res[5] = dr.GetString(5);
						res[6] = dr.GetString(6);
						res[7] = dr.GetString(7);
						res[8] = dr.GetString(8);
						res[9] = dr.GetString(9);
						res[10] = dr.GetString(10);
						res[11] = dr.GetString(11);
					}
					else if (tipo.Equals("j"))
					{
						res = new string[11];
						dr.Read();
						res[0] = dr.GetString(0);
						res[1] = dr.GetString(1);
						res[2] = dr.GetString(2);
						res[3] = dr.GetString(3);
						res[4] = dr.GetString(4);
						res[5] = dr.GetString(5);
						res[6] = dr.GetString(6);
						res[7] = dr.GetString(7);
						res[8] = dr.GetString(8);
						res[9] = dr.GetString(9);
						res[10] = dr.GetString(10);
					}
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener los datos del Cliente: " + ex.Message);
			}
			return res;
		}
	}
}
