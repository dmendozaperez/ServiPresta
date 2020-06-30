using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Servicio
{
    public class Dat_NotaCredito
    {
        public static DataSet leer_NC_tk(string _idnc)
        {
            string sqlquery = "USP_Leer_NotaCredito_Imprimir";
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
                cmd.Parameters.AddWithValue("@not_id", _idnc);
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
        public static string _anular_ncredito(Decimal varnumcred, Int32 _usu_mod)
        {
            string sqlquery = "USP_Anular_NotaCredito";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            string _valida = "";
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@not_id", varnumcred);
                cmd.Parameters.AddWithValue("@not_usu_mod", _usu_mod);
                cmd.Parameters.AddWithValue("@alm_id", 11/*Ent_Global._pvt_almaid*/);
                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                _valida = exc.Message;
            }
            return _valida;
        }
        public static DataTable dt_consulta_notacredito(Boolean _tipo, DateTime _fecha_ini, DateTime _fecha_fin, string _doc)
        {
            string sqlquery = "USP_Consultar_NCredito_Anular";
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
                cmd.Parameters.AddWithValue("@almid", 11/*Ent_Global._pvt_almaid*/);
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
        public static String[] saveReturnOrder(
          string RHN_COORDINATOR, string _ALMACEN, List<Ent_Nota_Dtl> listArticlesReturned, Int32 _usuing, string _codigo_estado)
        {
           
            string sqlquery = "USP_Insertar_NotaCredito";
            //string sqlquery = "[USP_Insertar_NotaCredito_Tmp]";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            DataTable dt = null;
            String[] results = new String[3];
            try
            {
                dt = new DataTable();
                dt.Columns.Add("Not_Det_Id", typeof(string));
                dt.Columns.Add("Not_Det_Item", typeof(Int32));
                dt.Columns.Add("Not_Det_VenId", typeof(string));
                dt.Columns.Add("Not_Det_ArtId", typeof(string));
                dt.Columns.Add("Not_Det_TalId", typeof(string));
                dt.Columns.Add("Not_Det_Cantidad", typeof(Int32));
                dt.Columns.Add("Not_Det_Calidad", typeof(string));

                Int32 item_nc = 0;

                foreach (Ent_Nota_Dtl item in listArticlesReturned)
                {
                    item_nc += 1;
                    dt.Rows.Add("", item_nc, item._RDV_INVOICE, item._RDV_ARTICLE, item._RDV_SIZE, item._RDN_QTY, item._CALIDAD);
                   
                }

                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@alm_id", _ALMACEN);
                cmd.Parameters.AddWithValue("@bas_id", RHN_COORDINATOR);
                cmd.Parameters.AddWithValue("@not_id", DbType.String);
                cmd.Parameters.AddWithValue("@TmpNc", dt);
                cmd.Parameters.AddWithValue("@usu_ing", _usuing);
                cmd.Parameters.AddWithValue("@not_estado_nc", _codigo_estado);
                cmd.Parameters["@not_id"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                results[0] = Convert.ToString(cmd.Parameters["@not_id"].Value);
                return results;
            }
            catch(Exception exc)
            {
                return null;
                //        connection.Close();
                //        return null;
            }
        }

        public static DataTable searchArticleInvoice(String _noInvoice, String _article, String _size, String _customer, string _calidad)
        {
            //DataTable dt = new DataTable();
            //return dt;
            ///
            //DataTable dtResult = new DataTable();
            string sqlquery = "USP_Buscar_ArticuloXVenta";
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
                cmd.Parameters.AddWithValue("@ven_id", _noInvoice);
                cmd.Parameters.AddWithValue("@art_id", _article);
                cmd.Parameters.AddWithValue("@tal_id", _size);
                cmd.Parameters.AddWithValue("@bas_id", _customer);
                cmd.Parameters.AddWithValue("@calidad", _calidad);
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
        public static DataSet getCoordinatorByPk(decimal _idCoord)
        {
            string sqlquery = "USP_Leer_Persona_Usuario";
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
                cmd.Parameters.AddWithValue("@bas_id", _idCoord);
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }
        public static DataSet getCoordinators(string _areaId)
        {
            string sqlquery = "USP_Leer_Promotor_Lider";
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
                cmd.Parameters.AddWithValue("@bas_are_id", _areaId);
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        public static Boolean getvalidaNota_DevTot(string _doc,Decimal _cant_total)
        {
            string sqlquery = "USP_NCreditVal_Total";
            Boolean valida = false;
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString()))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ndoc", _doc);
                        cmd.Parameters.AddWithValue("@cantT", _cant_total);
                        cmd.Parameters.Add("@valida", SqlDbType.Bit);
                        cmd.Parameters["@valida"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        valida =(Boolean) cmd.Parameters["@valida"].Value;

                    }
                }
            }
            catch (Exception)
            {
                valida = false;
            }
            return valida;
        }

        public static DataSet getStatusByModule(string _module)
        {
            string sqlquery = "USP_Leer_EstadoModulo";
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
                cmd.Parameters.AddWithValue("@est_mod_id", _module);
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);

                return ds;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }
    }
}
