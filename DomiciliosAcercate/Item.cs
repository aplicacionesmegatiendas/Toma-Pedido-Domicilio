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
	public class Item
	{
		public string[] BuscarItemsId(string id)
		{
			string SQL = "select " +
						"f120_rowid, " +
						"f120_id, " +
						"f120_referencia, " +
						"f120_descripcion " +
					"from " +
						"t120_mc_items " +
						"inner join t121_mc_items_extensiones on f120_rowid=f121_rowid_item and f120_id_cia=f121_id_cia " +
					"where " +
						"f120_id= @ID " +
						"and f120_id_cia=1 " +
						"and f121_ind_estado=1";
			string[] res = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["unoee"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@ID", id);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new string[4];
					dr.Read();
					res[0] = Convert.ToString(dr.GetInt32(0));
					res[1] = Convert.ToString(dr.GetInt32(1));
					res[2] = dr.GetString(2);
					res[3] = dr.GetString(3);

				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de items: " + ex.Message);
			}
			return res;
		}

		public string[] BuscarItemsBarra(string barra)
		{
			string SQL = "select " +
							"f120_rowid, " +
							"f120_id, " +
							"f120_referencia, " +
							"f120_descripcion " +
						"from " +
							"t120_mc_items " +
							"inner join t121_mc_items_extensiones on f120_rowid=f121_rowid_item and f120_id_cia=f121_id_cia " +
							"INNER JOIN t131_mc_items_barras on f121_rowid=f131_rowid_item_ext and f121_id_cia=f131_id_cia " +
						"where " +
							"f131_id= @BARRA " +
							"and f120_id_cia=1 " +
							"and f121_ind_estado=1";
			string[] res = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["unoee"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@BARRA", barra);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new string[4];
					dr.Read();
					res[0] = Convert.ToString(dr.GetInt32(0));
					res[1] = Convert.ToString(dr.GetInt32(1));
					res[2] = dr.GetString(2);
					res[3] = dr.GetString(3);

				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de items: " + ex.Message);
			}
			return res;
		}

		public DataTable BuscarItemsDescripcion(string descripcion, string co)
		{
			string SQL = "select distinct " +
							"f120_rowid, " +
							"f120_id, " +
							"f120_referencia, " +
							"f120_descripcion," +
							"f132_cant_existencia_1 " +
						"from " +
							"t120_mc_items " +
							"inner join t121_mc_items_extensiones on f120_rowid=f121_rowid_item and f120_id_cia=f121_id_cia " +
							"inner join t132_mc_items_instalacion on f132_id_cia = f121_id_cia and f132_rowid_item_ext = f121_rowid " +
						"where " +
							"f120_descripcion like'%" + descripcion + "%' and " +
							"f120_id_cia=1 and " +
							"f121_ind_estado=1 and " +
							"f132_cant_existencia_1>0 and " +
							"f132_id_instalacion = " + co + " " +
							"order by f132_cant_existencia_1 asc";
			DataTable dt;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["unoee"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de items: " + ex.Message);
			}
			return dt;
		}

		/// <summary>
		/// Obtiene las unidades de medida y el rowiditemext apartir del rowid de un item.
		/// </summary>
		/// <param name="rowid">rowid del item</param>
		/// <returns></returns>
		public DataTable ObtenerUnidadesRowiditemExt(string rowid, string rowid_tercero)
		{
			string SQL = "create table #temp  " +
							"( " +
								"ext1_det varchar(100), ext2_det varchar(100), und_desc varchar(100), rowid_item_ext varchar(100), id_und varchar(100), item_resumen varchar(100), fact_um varchar(100), " +
								"dec_um varchar(100), und_inv varchar(100), und_adic varchar(100), fact_adic varchar(100), dec_um_adic varchar(100), dec_um_inv varchar(100), ind_concesion varchar(100), " +
								"rowid_foto varchar(100), ind_tipo_gifcard varchar(100), saldo_datafono varchar(100), device_datafono varchar(100), val_carga varchar(100), pago_serv varchar(100), " +
								"ind_tipo_serv varchar(100), id_prov_serv varchar(100), ind_monto_fijo varchar(100), precio_vta varchar(100), f_ind_cap_ref int, f_ind_paquete int, f_precio_calculado decimal(18,2))" +
							"declare @p3 nvarchar(9) " +
							"set @p3=N'' " +
							"declare @p4 nvarchar(9) " +
							"set @p4=N'' " +
							"declare @p5 nvarchar(100) " +
							"set @p5=N'' " +
							"insert #temp " +
							"exec sp_items_consulta_ext_pos @p_id_cia=1,@p_rowid_item=@ROWID,@p_ext1_desc_corta=@p3 output,@p_ext2_desc_corta=@p4 output,@p_titulo_consulta=@p5 output,@p_ind_funcion_preliquidar=0,@p_ind_existencias=0,@p_rowid_bodega=0,@p_rowid_tercero_perf=@ROWID_TER,@p_id_suc_perf=N'001',@p_ind_etiqueta_item=0 " +
							"select rowid_item_ext,RTRIM(id_und) id_und, precio_vta from #temp order by 3 desc " +
							"drop table #temp;";
			DataTable dt;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["unoee"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@ROWID", rowid);
				cmd.Parameters.AddWithValue("@ROWID_TER", rowid_tercero);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);

				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener las unidades del items: " + ex.Message);
			}
			return dt;
		}

		/// <summary>
		/// Obtiene la existencia de un item apartir del rowiditem_ext. 
		/// </summary>
		/// <param name="rowid_tpv">rowid de la TPV.</param>
		/// <param name="rowid_item_ext">rowid_item_ext del item.</param>
		/// <param name="id_sucursal_perfil">id de la sucursal.</param>
		/// <returns></returns>
		public string[] ObtenerExistencia(string rowid_tpv, string rowid_item_ext, string id_sucursal_perfil, string rowid_tercero)
		{
			string SQL = "create table #temp " +
						"(" +
						   "rowid_item varchar(100), ind_tipo_item varchar(100), ind_concesion varchar(100), rowid_tercero varchar(100), id_suc_cli varchar(100), id_lista_prec varchar(100), " +
						   "id_tipo_cli varchar(100), ind_pronto_pago varchar(100), ind_impuesto varchar(100), ind_tipo_lista varchar(100), ind_impto_asum varchar(100), ind_solo_val varchar(100), " +
						   "ind_nat varchar(100), ind_obseq varchar(100), rowid_bodega varchar(100), id_instalacion varchar(100), ind_tipo_gifcard varchar(100), ind_saldo_datafono varchar(100), " +
						   "device_datafono varchar(100), ind_val_carga varchar(100), ind_pago_serv varchar(100), id_tipo_serv varchar(100), ind_prov_serv varchar(100), ind_monto_fijo varchar(100), " +
						   "cant_exist varchar(100), f_ind_cap_ref int" +
						") " +
						"insert #temp " +
						"exec sp_pdv_d_mvto_dcto_leer_liq @p_id_cia=1,@p_rowid_tpv=@ID_TPV,@p_id_clase_docto=1231,@p_rowid_tercero='0',@p_id_sucursal='',@p_rowid_item_ext=@ITEM_EXT,@p_id_concepto=1201,@p_id_motivo=N'01',@p_rowid_tercero_perfil=@ROWID_TER,@p_id_sucursal_perfil=@SUC_PER,@p_id_tarjeta_fiel=NULL,@p_ind_propina=NULL,@p_rowid_bodega_cap=NULL,@p_id_tipo_tarjeta=NULL " +
						"select id_suc_cli, id_lista_prec, id_instalacion, cant_exist from #temp " +
						"drop table #temp";
			string[] res = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["unoee"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@ID_TPV", rowid_tpv);
				cmd.Parameters.AddWithValue("@ITEM_EXT", rowid_item_ext);
				cmd.Parameters.AddWithValue("@SUC_PER", id_sucursal_perfil);
				cmd.Parameters.AddWithValue("@ROWID_TER", rowid_tercero);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new string[4];
					dr.Read();
					res[0] = Convert.ToString(dr[0]);
					res[1] = Convert.ToString(dr[1]);
					res[2] = Convert.ToString(dr[2]);
					res[3] = Convert.ToDecimal(dr[3].ToString().Replace('.', Configuracion.separador).Replace(',', Configuracion.separador)).ToString("0.#");
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener la existencia: " + ex.Message);
			}
			return res;
		}

		public decimal ObtenerExistenciaItem(string item, string co)
		{
			string SQL = "select " +
							"f132_cant_existencia_1 " +
						"from " +
							"t120_mc_items " +
						"inner join " +
							"t121_mc_items_extensiones on f121_id_cia = f120_id_cia and f121_rowid_item = f120_rowid " +
						"inner join " +
							"t132_mc_items_instalacion on f132_id_cia = f121_id_cia and f132_rowid_item_ext = f121_rowid " +
						"where " +
							"f120_id = @item and f120_id_cia = '1' and f132_id_instalacion = @co";
			decimal res = -1;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["unoee"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@item", item);
				cmd.Parameters.AddWithValue("@co", co);
				decimal.TryParse(Convert.ToString(cmd.ExecuteScalar()), out res);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener la existencia : " + ex.Message);
			}
			return res;
		}

		public string[] ObtenerPrecio(string id_lista_prec, string rowid_item_ext, string id_um)
		{
			string SQL = "declare @p6 int " +
						"set @p6=0 " +
					   "declare @p7 int " +
					   "set @p7=0 " +
					   "declare @p8 money " +
					   "set @p8=0.000 " +
					   "declare @p9 money " +
					   "set @p9=0.0000 " +
					   "declare @p10 money " +
					   "set @p10=0.0000 " +
					   "declare @p11 money " +
					   "set @p11=0.0000 " +
					   "declare @p12 int " +
					   "set @p12=0 " +
					   "declare @p13 smallint " +
					   "set @p13=0 " +
					   "declare @FECHA datetime " +
					   "set @FECHA=convert(date,getdate()) " +
						"exec sp_generico_hallar_prec_vta @p_cia=1,@p_id_lista_prec=@ID_LISTA_PREC,@p_rowid_item_ext=@ROW_ID_ITEM_EXT,@p_fecha_docto=@FECHA,@p_id_um=@UM,@p_rowid_promo_dscto=@p6 output,@p_rowid_lista_prec=@p7 output,@p_precio=@p8 output,@p_precio_min=@p9 output,@p_precio_max=@p10 output,@p_precio_sug=@p11 output,@p_puntos_fiel=@p12 output,@p_ind_oferta=@p13 output " +
						"select @p7 rowid_lista_prec, @p8 precio";
			string[] res = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["unoee"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@ID_LISTA_PREC", id_lista_prec);
				cmd.Parameters.AddWithValue("@ROW_ID_ITEM_EXT", rowid_item_ext);
				//cmd.Parameters.AddWithValue("@FECHA", ObtenerFechaActual());
				cmd.Parameters.AddWithValue("@UM", id_um);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new string[2];
					dr.Read();
					res[0] = Convert.ToDecimal(dr[0].ToString().Replace('.', Configuracion.separador).Replace(',', Configuracion.separador)).ToString("0.#");
					res[1] = Convert.ToDecimal(dr[1].ToString().Replace('.', Configuracion.separador).Replace(',', Configuracion.separador)).ToString("0.#");
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el precio: " + ex.Message);
			}
			return res;
		}

		public string[] ObtenerDescuento(string rowid_item_ext, string id_sucursal_cli, string id_lista_precios, string cantidad, string id_cliente_pos, string rowid_tercero, string id_co_docto)
		{
			string SQL = "declare @p18 smallint " +
						   "set @p18=0 " +
						   "declare @p19 float " +
						   "set @p19=0 " +
						   "declare @p20 money " +
						   "set @p20=0 " +
						   "declare @p21 int " +
						   "set @p21=0 " +
						   "declare @p22 datetime " +
						   "set @p22=getdate() " +
						   "declare @p23 datetime " +
						   "set @p23=NULL " +
						   "declare @p24 int " +
						   "set @p24=0 " +
						   "declare @p25 nvarchar(40) " +
						   "set @p25='' " +
						   "declare @p27 smallint " +
						   "set @p27=0 " +
						   "declare @FECHA datetime " +
						   "set @FECHA=convert(date,getdate()) " +
						   "exec sp_pdv_d_movto_docto_dctleer @p_id_cia=1,@p_guid_movto=NULL,@p_ind_pronto_pago=0,@p_rowid_item_ext=@ROWIDITEM_EXT,@p_rowid_tercero_cli=@ROWID_TER,@p_id_sucursal_cli=@SUC_CLI, " +
						   "@p_rowid_punto_envio=0,@p_id_tipo_cli=N'001 ',@p_id_lista_precios=@ID_LIST_PREC,@p_id_cond_pago=N'00A',@p_id_moneda=N'COP',@p_fecha=@FECHA,@p_cant1=@CANTIDAD, " +
						   "@p_cant2=0,@p_valor_venta=0,@p_id_concepto=1201,@p_id_motivo=N'01',@p_cont_exclusivos=@p18 output,@p_porc_max_manual=@p19 output,@p_vlr_max_manual=@p20 output, " +
						   "@p_rowid_promo_dscto_linea=@p21 output,@p_fecha_inicial=@p22 output,@p_fecha_final=@p23 output,@p_id_promo_dscto=@p24 output,@p_desc_promo_dscto=@p25 output,@p_id_co_docto=@ID_CO_DOCTO, " +
						   "@p_ind_control_valor_acum=@p27 output,@p_orden_dscto_max=0,@p_id_cliente_pos=@CLIENTE,@p_id_tarjeta_fiel=NULL,@p_ind_redencion_puntos=0,@p_ind_exclusivo_2x1=0,@p_ind_oferta=0";

			string[] res = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["unoee"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn)
				{
					CommandTimeout = 60,
					CommandType = CommandType.Text
				};
				cmd.Parameters.AddWithValue("@ROWIDITEM_EXT", rowid_item_ext);
				cmd.Parameters.AddWithValue("@SUC_CLI", id_sucursal_cli);
				cmd.Parameters.AddWithValue("@ID_LIST_PREC", id_lista_precios);
				cmd.Parameters.AddWithValue("@CANTIDAD", Convert.ToDecimal(cantidad));
				cmd.Parameters.AddWithValue("@CLIENTE", id_cliente_pos);
				cmd.Parameters.AddWithValue("@ROWID_TER", rowid_tercero);
				cmd.Parameters.AddWithValue("@ID_CO_DOCTO", id_co_docto);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new string[5];
					dr.Read();
					res[0] = Convert.ToDecimal(dr[2].ToString().Replace('.', Configuracion.separador).Replace(',', Configuracion.separador)).ToString("0.#");
					res[1] = Convert.ToDecimal(dr[3].ToString().Replace('.', Configuracion.separador).Replace(',', Configuracion.separador)).ToString("0.#");
					res[2] = Convert.ToString(dr[7]);
					res[3] = Convert.ToString(dr[8]);
					res[4] = Convert.ToString(dr[10]);
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el descuento: " + ex.Message);
			}
			return res;
		}

		public string[] ObtenerImpuesto(string rowid_item, string id_co, string rowid_tercero, string id_sucursal, string rowid_item_lista_precio)
		{
			string SQL = "declare @p10 smallint " +
						"set @p10=0 " +
						"exec sp_pdv_d_movto_docto_impleer @p_cia=1,@p_rowid_item=@ROW_ID,@p_id_co=@ID_CO,@p_rowidtercero=@ROWID_TER,@p_id_sucursal=@ID_SUC,@p_concepto=1201,@p_guid_movto=NULL,@p_rowid_item_lista_precio=@ROWID_LISTA,@p_motivo=N'01',@p_ind_gravado=@p10 output " +
						"select @p10";

			string[] res = null;
			try
			{
				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["unoee"].ConnectionString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);

				cmd.Parameters.AddWithValue("@ROW_ID", rowid_item);
				cmd.Parameters.AddWithValue("@ID_CO", id_co);
				cmd.Parameters.AddWithValue("@ROWID_TER", rowid_tercero);
				cmd.Parameters.AddWithValue("@ID_SUC", id_sucursal);
				cmd.Parameters.AddWithValue("@ROWID_LISTA", rowid_item_lista_precio);

				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new string[2];
					dr.Read();
					res[0] = Convert.ToDecimal(dr[4].ToString().Replace('.', Configuracion.separador).Replace(',', Configuracion.separador)).ToString("0.#");
					res[1] = Convert.ToDecimal(dr[5].ToString().Replace('.', Configuracion.separador).Replace(',', Configuracion.separador)).ToString("0.#");
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el impuesto: " + ex.Message);
			}
			return res;
		}
	}
}
