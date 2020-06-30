using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Configuration;

namespace Servicio
{
    public class Dat_Venta
    {


        #region<REGION DE VENTA ALMACEN CHORRILLOS>

        public static Decimal getprecio_sinigv(string articulo,string tipo)
        {
            string sqlquery = "Devolver_Precio_SinIgv";
            Decimal precio = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString()))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@art_id", SqlDbType.VarChar).Value = articulo;
                        cmd.Parameters.Add("@tip", SqlDbType.VarChar).Value = tipo;

                        SqlParameter precio_sql = new SqlParameter("@precio",SqlDbType.Money);
                        precio_sql.Direction = ParameterDirection.ReturnValue;

                        cmd.Parameters.Add(precio_sql);

                        cmd.ExecuteNonQuery();

                        precio =Convert.ToDecimal(precio_sql.Value);


                    }
                }
            }
            catch 
            {
                precio = 0;
            }
            return precio;
        }
        public static DataSet leer_venta_tk(string _idventa)
        {
            string sqlquery = "USP_Leer_Venta_Imprimir";
            DataSet ds = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ven_id", _idventa);
                cmd.Parameters.AddWithValue("@pvt_id", 1/*Ent_Global._pvt_id*/);
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch
            {
                return null;
            }
        }
        public static string _anular_venta(string _ven_id)
        {
            string sqlquery = "USP_Anular_Venta";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            string _error = "";
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ven_id", _ven_id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }
        
        public static DataTable dt_consulta_venta(Boolean _tipo, DateTime _fecha_ini, DateTime _fecha_fin, string _doc)
        {
            string sqlquery = "USP_Consultar_Documento_Anular";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", _tipo);
                cmd.Parameters.AddWithValue("@fechaini", _fecha_ini);
                cmd.Parameters.AddWithValue("@fechafin", _fecha_fin);
                cmd.Parameters.AddWithValue("@doc", _doc);
                cmd.Parameters.AddWithValue("@alm_id", 11/*Ent_Global._pvt_almaid*/);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }
        public static DataSet leer_venta_guia(string _ven_id)
        {
            string sqlquery = "USP_Leer_Guia_Venta";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet ds = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ven_id", _ven_id);
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch
            {
                return null;
            }
        }
        public static Int32 valida_guia(string _idventa)
        {
            string sqlquery = "USP_Valida_Guia_Venta";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ven_id", _idventa);
                cmd.Parameters.Add("@valida_banco", SqlDbType.Int);
                cmd.Parameters["@valida_banco"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                return Convert.ToInt32(cmd.Parameters["@valida_banco"].Value.ToString());
            }
            catch
            {
                return 0;
            }
        }
        public static void insertar_codigo_hash(string _ven_id, string _hash, string _estado,string url_pdf)
        {
            string sqlquery = "USP_Insertar_Codigo_Hash";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ven_id", _ven_id);
                cmd.Parameters.AddWithValue("@codigo_hash", _hash);
                cmd.Parameters.AddWithValue("@Estado", _estado);
                cmd.Parameters.AddWithValue("@url_pdf", url_pdf);
                cmd.ExecuteNonQuery();
            }
            catch
            {
            }
            if (cn.State == ConnectionState.Open) cn.Close();
        }
        public static string insertar_venta(string _liq,ref string _error)
        {
            string sqlquery = "USP_Insertar_Venta_Tda";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            string num_doc = "";
            try
            {


                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@liq_id", _liq);
                cmd.Parameters.AddWithValue("@usu_creacion", 1/*Ent_Global._bas_id_codigo*/);
                cmd.Parameters.Add("@numero_venta", SqlDbType.VarChar, 20);
                cmd.Parameters["@numero_venta"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                num_doc = cmd.Parameters["@numero_venta"].Value.ToString();
            }
            catch(Exception exc) 
            {
                num_doc = "-1";
                _error = exc.Message;
                //throw;
            }
            return num_doc;
        }
        public static string borrar_lineapaquete(decimal _paqid, string _articulo, string _talla)
        {
            string sqlquery = "USP_Borrar_LineaPaqueteDetalle";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@paq_id", _paqid);
                cmd.Parameters.AddWithValue("@art_id", _articulo);
                cmd.Parameters.AddWithValue("@talla", _talla);
                cmd.ExecuteNonQuery();
                return "1";
            }
            catch
            {
                return "-1";
            }
        }
        public static DataTable Datos_art_tallaemp(string _liqid, string _art)
        {
            string sqlquery = "USP_Leer_ArtTallaLiqNoEmpaque";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@liq_id", _liqid);
                cmd.Parameters.AddWithValue("@art_id", _art);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
        }
        public static DataTable leer_articulopacking_paquete(string _liqid, Decimal _paqid)
        {
            string sqlquery = "USP_LeerArtPackinPaquete";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@liq_id", _liqid);
                cmd.Parameters.AddWithValue("@Paq_Id", _paqid);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
        }
        public static Decimal insertar_leer_paquete(string _liqid)
        {
            string sqlquery = "USP_InsertarLeer_LiqPaquete";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            Decimal id_paquete = 0;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@paq_liqid", _liqid);
                cmd.Parameters.Add("@paq_id", SqlDbType.Decimal);
                cmd.Parameters["@paq_id"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                id_paquete = Convert.ToInt32(cmd.Parameters["@paq_id"].Value);
            }
            catch
            {               
                id_paquete = 0;
                if (cn.State == ConnectionState.Open) cn.Close();
                throw;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return id_paquete;
        }
        public static string insertar_articulopaq(Decimal _paq_id, string _liqid, string _artid, string _talla, Decimal _cantidad, string _calidad, string _barra)
        {
            string sqlquery = "USP_InsertarArticuloPaq";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            string valor = "";
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@paq_id", _paq_id);
                cmd.Parameters.AddWithValue("@paq_liqid", _liqid);
                cmd.Parameters.AddWithValue("@art_id", _artid);
                cmd.Parameters.AddWithValue("@tal_id", _talla);
                cmd.Parameters.AddWithValue("@Cant", _cantidad);
                cmd.Parameters.AddWithValue("@calidad", _calidad);
                cmd.Parameters.AddWithValue("@barra", _barra);
                //cmd.Parameters.AddWithValue("@Cant", _cantidad);
                cmd.Parameters.Add("@resulado", SqlDbType.VarChar, 2);
                cmd.Parameters["@resulado"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                valor = cmd.Parameters["@resulado"].Value.ToString();
            }
            catch(Exception exc)
            {
                valor = "-1";
                throw;                
            }
            return valor;
        }
        public static decimal leer_maxnopaqliq(string _liq)
        {
            string sqlquery = "USP_Leer_MaxNoPaqLiq";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@liq_id", _liq);
                cmd.Parameters.Add("@nopaquete", SqlDbType.Decimal);
                cmd.Parameters["@nopaquete"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                return Convert.ToDecimal(cmd.Parameters["@nopaquete"].Value);
            }
            catch
            {
                if (cn.State == ConnectionState.Open) cn.Close();
                return 0;
            }
           
        }
        public static DataTable leerarticulopaqliq(string _liq)
        {
            string sqlquery = "USP_LeerArticulo_PaqLiq";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@liq_id", _liq);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
        }
        #endregion


        #region<REGION FUNCIONES PARA E-COMMERCE>
        /// <summary>
        /// Función que Actualiza el estado de envío a Almacen, para que vuelva a enviarlo como anulado
        /// </summary>
        /// <param name="_tipo">determina si es BO o FA</param>
        /// <param name="_ven_id">código que identifica el documento</param>
        /// <returns></returns>
        public static string _act_estado_anular_venta(string _tipo, string _ven_id)
        {
            string sqlquery = "USP_ActualizaVentas_PS";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            string _error = "";
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", _ven_id);
                cmd.Parameters.AddWithValue("@estado", "");
                cmd.Parameters.AddWithValue("@tipo", _tipo);
                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }
        #endregion

        #region <FACTURACION ELECTRONICA>
        public static string _leer_formato_electronico(string _tipo_doc, string _num_doc, ref string _error)
        {
            string _formato_doc = "";
            string sqlquery = "[USP_Leer_Formato_Electronico]";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tip", _tipo_doc);
                cmd.Parameters.AddWithValue("@doc_id", _num_doc);
                cmd.Parameters.Add("@formato_txt", SqlDbType.NVarChar, -1);
                cmd.Parameters["@formato_txt"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                _formato_doc = cmd.Parameters["@formato_txt"].Value.ToString();
            }
            catch (Exception exc)
            {
                _formato_doc = "";
                _error = exc.Message;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return _formato_doc;
        }
        #endregion
        public static string _leer_formato_electronico_PAPERLESS(string _tipo_doc, string _num_doc, ref string _error,ref string return_numdoc)
        {
            string _formato_doc = "";
            string sqlquery = "[USP_Leer_Formato_Electronico_Paperless]";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tip", _tipo_doc);
                cmd.Parameters.AddWithValue("@doc_id", _num_doc);
                cmd.Parameters.Add("@formato_txt", SqlDbType.NVarChar, -1);
                cmd.Parameters["@formato_txt"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("@num_doc_return", SqlDbType.VarChar, 15);
                cmd.Parameters["@num_doc_return"].Direction = ParameterDirection.Output;


                cmd.ExecuteNonQuery();
                _formato_doc = cmd.Parameters["@formato_txt"].Value.ToString();
                return_numdoc= cmd.Parameters["@num_doc_return"].Value.ToString();
            }
            catch (Exception exc)
            {
                _formato_doc = "";
                _error = exc.Message;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return _formato_doc;
        }
        #region<FACTURACION ELECTRONICA PAPERLESS>

        #endregion

        #region<REGION DE FACTURACION DIRECTA>


        private ObservableCollection<Dat_Venta> datventa_art_talla = new ObservableCollection<Dat_Venta>();

        public string articulo_venta { get; set; }
        public string articulo_talla { get; set; }
        public string articulo_calidad { get; set; }
        public Decimal articulo_cantidad { get; set; }
        public string  articulo_ofe_tipo { get; set; }
        public Decimal articulo_Ofe_ArtVenta { get; set; }
        public Decimal articulo_ofe_id { get; set; }
        public Decimal articulo_ofe_MaxPares { get; set; }
        public Decimal articulo_Ofe_Porce{ get; set; }
        public Decimal articulo_stock_cant { get; set; }
        public Decimal preciosinigv { get; set; }
        public decimal preciosigv { get; set; }
        public string articulo_marca { get; set; } 
        public string articulo_color { get; set; }     
        public Boolean articulo_comi_bool { get; set; }

        public decimal articulo_precio_sinigv { get; set; }
        public string articulo_foto { get; set; }
        public string articulo_afec_percepcion { get; set; }


        public List<Vent_Talla_Cant> ven_tall { get; set; }  

        public Boolean BuscarProductoStock(string articulo,string talla,string barra,Boolean bus_barra,decimal bas_id,ref  ObservableCollection<Dat_Venta> articulo_stock)
        {
            Boolean valida = false;
            string sqlquery = "USP_LeerStockArticulo_Venta";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString()))
                {
                 
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idalmacen", 11/*Ent_Global._pvt_almaid*/);
                        cmd.Parameters.AddWithValue("@articulo", articulo);
                        cmd.Parameters.AddWithValue("@talla", talla);
                        cmd.Parameters.AddWithValue("@barra", barra);
                        cmd.Parameters.AddWithValue("@bus_barra", bus_barra);
                        cmd.Parameters.AddWithValue("@bas_id", bas_id);
                        if (bus_barra)
                        {
                            if (cn.State == 0) cn.Open();
                            SqlDataReader dr = cmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    articulo_venta = dr["articulo"].ToString();
                                    articulo_marca = dr["marca"].ToString();
                                    articulo_color = dr["color"].ToString();
                                    articulo_foto = dr["foto"].ToString();
                                    articulo_comi_bool =Convert.ToBoolean(dr["comision"]);
                                    articulo_precio_sinigv =Convert.ToDecimal(dr["preciosinigv"]);
                                    articulo_talla = dr["talla"].ToString();
                                    articulo_cantidad = Convert.ToDecimal(dr["cantidad"]);
                                    articulo_ofe_tipo = dr["Ofe_tipo"].ToString();
                                    articulo_Ofe_ArtVenta = Convert.ToDecimal(dr["Ofe_ArtVenta"]);
                                    articulo_ofe_id = Convert.ToDecimal(dr["Ofe_Id"]);
                                    articulo_ofe_MaxPares = Convert.ToDecimal(dr["Ofe_MaxPares"]);
                                    articulo_Ofe_Porce = Convert.ToDecimal(dr["cantidad"]);
                                    articulo_stock_cant = Convert.ToDecimal(dr["Ofe_Porc"]);
                                    articulo_afec_percepcion= dr["Afec_Percepcion"].ToString();
                                    valida = true;
                                }
                            }
                        }
                        else
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                if (dt.Rows.Count>0)
                                {
                                    var querydisarticulo = (from dfila in dt.AsEnumerable()
                                                            select new
                                                            {
                                                                articulo = dfila["articulo"],
                                                                marca = dfila["marca"],
                                                                color=dfila["color"],
                                                                precio=Convert.ToDecimal(dfila["preciosinigv"]),
                                                                comi_activa=Convert.ToBoolean(dfila["comision"]),
                                                                foto= dfila["foto"],
                                                                afec_percepcion= dfila["Afec_Percepcion"].ToString(),
                                                                precio_igv= Convert.ToDecimal(dfila["preciosigv"]),
                                                                articulo_ofe_tipo = dfila["Ofe_tipo"].ToString(),
                                                                articulo_Ofe_ArtVenta = Convert.ToDecimal(dfila["Ofe_ArtVenta"]),
                                                                articulo_ofe_id = Convert.ToDecimal(dfila["Ofe_Id"]),
                                                                articulo_ofe_MaxPares = Convert.ToDecimal(dfila["Ofe_MaxPares"]),
                                                                articulo_Ofe_Porce = Convert.ToDecimal(dfila["Ofe_Porc"]),
                                                            }).Distinct();

                                    foreach(var row in querydisarticulo)
                                    {
                                        string _art = row.articulo.ToString();

                                        List<Vent_Talla_Cant> talla_arti = new List<Vent_Talla_Cant>();
                                        talla_arti = ((from myfila in dt.AsEnumerable()
                                                       where myfila.Field<string>("articulo") == _art
                                                       select new Vent_Talla_Cant
                                                       {
                                                           _talla = myfila["talla"].ToString(),
                                                           _cant = Convert.ToDecimal(myfila["cantidad"])
                                                       })).ToList<Vent_Talla_Cant>();

                                        datventa_art_talla.Add(new Dat_Venta
                                        {
                                            articulo_venta = row.articulo.ToString(),
                                            articulo_marca = row.marca.ToString(),
                                            articulo_color = row.color.ToString(),
                                            articulo_comi_bool = row.comi_activa,
                                            articulo_afec_percepcion=row.afec_percepcion.ToString(),
                                            ven_tall = talla_arti,
                                            preciosinigv=row.precio,
                                            preciosigv=row.precio_igv,
                                            articulo_foto =row.foto.ToString(),
                                            articulo_ofe_tipo = row.articulo_ofe_tipo,
                                            articulo_Ofe_ArtVenta = row.articulo_Ofe_ArtVenta,
                                            articulo_ofe_id = row.articulo_ofe_id,
                                            articulo_ofe_MaxPares = row.articulo_ofe_MaxPares,
                                            articulo_Ofe_Porce = row.articulo_Ofe_Porce,
                                        }
                                        );
                                    }

                                    articulo_stock = datventa_art_talla;
                                    valida = true;
                                }
                                
                            }
                        }
                    }
                }
            }
            catch(Exception exc)
            {
                valida = false;
            }
            return valida;
        }

        #endregion

        #region<REGION DE PEDIDO - URBANO>

        //
        public static DataTable dt_consulta_pedido_urbano(Boolean _tipo, DateTime _fecha_ini, DateTime _fecha_fin, string _doc)
        {
            string sqlquery = "USP_Consultar_Pedido_Urbano";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", _tipo);
                cmd.Parameters.AddWithValue("@fechaini", _fecha_ini);
                cmd.Parameters.AddWithValue("@fechafin", _fecha_fin);
                cmd.Parameters.AddWithValue("@doc", _doc);
                cmd.Parameters.AddWithValue("@alm_id", 11/*Ent_Global._pvt_almaid*/);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }
        public static DataTable dt_consulta_pedido(Boolean _tipo, DateTime _fecha_ini, DateTime _fecha_fin, string _doc)
        {
            string sqlquery = "USP_Consultar_Pedidos_Pendientes";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", _tipo);
                cmd.Parameters.AddWithValue("@fechaini", _fecha_ini);
                cmd.Parameters.AddWithValue("@fechafin", _fecha_fin);
                cmd.Parameters.AddWithValue("@doc", _doc);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }
        public static Int32 valida_pedido(string _id)
        {
            string sqlquery = "USP_Valida_Pedido";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", _id);
                cmd.Parameters.Add("@valida", SqlDbType.Int);
                cmd.Parameters["@valida"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                return Convert.ToInt32(cmd.Parameters["@valida"].Value.ToString());
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Consulta de liquidacion e informacion adicional
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_noLiq"></param>
        /// <returns></returns>
        public static DataSet getLiquidationHdrInfo(string _noLiq)
        {
            string sqlquery = "USP_Leer_Liquidacion_Reporte_EC";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet ds = null;
            try
            {

                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@liq_id", _noLiq);
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_noLiq"></param>
        /// <returns></returns>
        public static DataSet getDtlPicking(string _noLiq)
        {
            string sqlquery = "USP_Leer_Empaque_EC";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet ds = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@liq_id", _noLiq);
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }
        #endregion

    }
    public class Vent_Talla_Cant
    {
        public string _talla { get; set; }
        public Decimal _cant { get; set; }

        public decimal _edit_talla { get; set; }
    }

    public class Dat_Venta_Directa
    {
        public static string insertar_venta_directa(decimal _bas_id,Decimal _igv_monto,decimal _comisionp,decimal _percepcion_m,
                                                    decimal _percepcion_p, ObservableCollection<Ent_Venta> venta_det,
                                                    List<Ent_Venta_FormaPago> forma_pago , ref string _error)
        {
            string sqlquery = "[USP_Insertar_Venta_Directa]";            
            string num_doc = "";
            DataTable Tmp_Venta_Detalle = null;
            DataTable Tmp_PagoDirecto = null;
            try
            {

                /*tabla detalle de venta*/
                Tmp_Venta_Detalle = new DataTable();
                Tmp_Venta_Detalle.Columns.Add("Ven_Det_Item",typeof(decimal));
                Tmp_Venta_Detalle.Columns.Add("Ven_Det_ArtId",typeof(string));
                Tmp_Venta_Detalle.Columns.Add("Ven_Det_TalId",typeof(string));
                Tmp_Venta_Detalle.Columns.Add("Ven_Det_Cantidad",typeof(decimal));
                Tmp_Venta_Detalle.Columns.Add("Ven_Det_Precio",typeof(decimal));
                Tmp_Venta_Detalle.Columns.Add("Ven_Det_ComisionM",typeof(decimal));
                Tmp_Venta_Detalle.Columns.Add("Ven_Det_Calidad",typeof(string));
                Tmp_Venta_Detalle.Columns.Add("Ven_Det_OfertaM",typeof(decimal));
                Tmp_Venta_Detalle.Columns.Add("Ven_Det_OfertaP",typeof(decimal));
                Tmp_Venta_Detalle.Columns.Add("Ven_Det_OfeID",typeof(decimal));
                /*************************/

                /*tabla detalle de forma de pago*/
                Tmp_PagoDirecto = new DataTable();
                Tmp_PagoDirecto.Columns.Add("Doc_Tra_Id", typeof(string));
                Tmp_PagoDirecto.Columns.Add("Con_Id",typeof(string));
                Tmp_PagoDirecto.Columns.Add("Bin_Tar_Ser", typeof(string));
                Tmp_PagoDirecto.Columns.Add("Bin_Tar_Cod", typeof(string));
                Tmp_PagoDirecto.Columns.Add("NumeroTarjeta",typeof(string));
                Tmp_PagoDirecto.Columns.Add("Monto",typeof(decimal));
                /*****************************/
                Int32 item_det = 0;
                foreach (Ent_Venta vd in venta_det)
                {
                    item_det += 1;
                    string articulo = vd.articulo.ToString(); decimal precio = vd.precio;
                    string calidad = "1"; Decimal oferta = 0; decimal OfertaP = 0; decimal OfeID = 0;
                    Decimal OfeDescTotal = Convert.ToDecimal(vd.total_descto);
                    Decimal nroItem = Convert.ToDecimal(vd.ofe_nroItem);
                    int contItem = Convert.ToInt32(vd.ofe_nroItem);
                    OfertaP = vd.ofe_porc;

                    //if (OfertaP == 100) {
                    //    contItem = contItem * 2;
                    //    nroItem = nroItem * 2;
                    //}

                    OfeID = vd.ofe_id;
                    if (OfeDescTotal > 0) { oferta = OfeDescTotal / nroItem; };                    

                    foreach (Ent_Venta_Talla vd_talla in vd.articulo_talla)
                    {
                        if (contItem <= 0) { oferta = 0; };

                        string talla = vd_talla.talla;
                        decimal cantidad = vd_talla.cantidad;
                        decimal comision =Math.Round((cantidad * precio) * (_comisionp / 100),2,MidpointRounding.AwayFromZero);

                        Tmp_Venta_Detalle.Rows.Add(item_det,articulo,talla,cantidad,precio,comision,calidad,oferta,OfertaP,OfeID);
                        contItem = contItem - 1;
                    }
                }

                foreach(Ent_Venta_FormaPago vforma in forma_pago)
                {
                    string conid = vforma.forma_pago_id;string tar_bines_ser = vforma.tarjeta_bines_ser;string tar_bines_cod = vforma.tarjeta_bines_cod; string numerotarjeta = vforma.tarjeta_numero;
                    string doc_tra_id = vforma.doc_tra_id;
                    decimal montopago =vforma.forma_monto;
                    Tmp_PagoDirecto.Rows.Add(doc_tra_id, conid, tar_bines_ser, tar_bines_cod, numerotarjeta, montopago);

                }

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString()))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ven_bas_id", _bas_id);
                        cmd.Parameters.AddWithValue("@ven_igv", _igv_monto);
                        cmd.Parameters.AddWithValue("@ven_comisionp", _comisionp);
                        cmd.Parameters.AddWithValue("@ven_percepcioonm", _percepcion_m);
                        cmd.Parameters.AddWithValue("@ven_percepcionp", _percepcion_p);
                        cmd.Parameters.AddWithValue("@pvt_id", 1/*Ent_Global._pvt_id*/);
                        cmd.Parameters.AddWithValue("@usu_creacion", 0/*Ent_Global._bas_id_codigo*/);
                        

                        cmd.Parameters.AddWithValue("@tmpventa_detalle", Tmp_Venta_Detalle);
                        cmd.Parameters.AddWithValue("@tmppago_directo", Tmp_PagoDirecto);                    


                        cmd.Parameters.Add("@numero_venta", SqlDbType.VarChar, 20);
                        cmd.Parameters["@numero_venta"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                        num_doc = cmd.Parameters["@numero_venta"].Value.ToString();
                    }
                }
            }
            catch(Exception exc)
            {
                num_doc = "-1";
                _error = exc.Message;
            }
            return num_doc;
        }

        public List<Ent_Venta_PagoNota> leer_formapago_nota(Decimal _bas_id)
        {
            string sqlquery = "[USP_LeerPagoForma_NotaCredito]";
            List<Ent_Venta_PagoNota> formapagonc = null;            
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString()))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idalmacen",11/*Ent_Global._pvt_almaid*/);
                        cmd.Parameters.AddWithValue("@bas_id", _bas_id);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            formapagonc = new List<Ent_Venta_PagoNota>();
                            while (dr.Read())
                            {
                                Ent_Venta_PagoNota nc_row = new Ent_Venta_PagoNota();
                                nc_row.doc_tra_id = dr["Doc_Tra_Id"].ToString();
                                nc_row.nc_num = dr["nc_num"].ToString();
                                nc_row.total_nc = Convert.ToDecimal(dr["totalnc"]);
                                nc_row.chknota = false;
                                formapagonc.Add(nc_row);
                            }
                        }

                    }
                }
            }
            catch
            {
                formapagonc = null;
            }
            return formapagonc;
        }
    }

    public class Dat_Cierre_Venta
    {       
        public Ent_Cierre_Venta leer_data_cierre(DateTime _fechav)
        {
            Ent_Cierre_Venta get_cierre = null;
            String sqlquery = "USP_CierreLeerReport";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString()))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idalmacen", 11/*Ent_Global._pvt_almaid*/);
                        cmd.Parameters.AddWithValue("@fechav", _fechav);

                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            get_cierre = new Ent_Cierre_Venta();
                            while (dr.Read())
                            {
                                get_cierre.fecha_venta = Convert.ToDateTime(dr["fecha_venta"]);
                                get_cierre.inicio_caja = Convert.ToDecimal(dr["inicio_caja"]);
                                get_cierre.total_venta = Convert.ToDecimal(dr["total_venta"]); 
                                get_cierre.efectivo = Convert.ToDecimal(dr["efectivo"]); 
                                get_cierre.vuelto = Convert.ToDecimal(dr["vuelto"]); 
                                get_cierre.total_efectivo = Convert.ToDecimal(dr["total_efectivo"]); 
                                get_cierre.total_tarjeta = Convert.ToDecimal(dr["total_tarjeta"]); 
                                get_cierre.total_caja = Convert.ToDecimal(dr["total_caja"]);
                                get_cierre.banco_des = dr["banco_des"].ToString();
                                get_cierre.nro_operacion= dr["nro_opera"].ToString();
                                get_cierre.monto_opera =Convert.ToDecimal(dr["monto_opera"]);
                            }
                        }
                    }
                }
            }
            catch
            {
                get_cierre = null;
            }
            return get_cierre;
        }
    }
}
