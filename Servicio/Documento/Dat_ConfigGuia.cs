using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace Servicio
{
    public class Dat_ConfigGuia
    {
        public static DataTable leertrasnportador()
        {
            string sqlquery = "USP_Leer_Transportadora";
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
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);

            }
            catch 
            {               
                dt = null;
                throw;
            }
            return dt;
        }
        public static string guiasecuencia()
        {
            string sqlquery = "USP_GuiaRemision_Secuencia";
            string nguia = "";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {

                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@numguia", SqlDbType.VarChar, 20);
                cmd.Parameters["@numguia"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                nguia = Convert.ToString(cmd.Parameters["@numguia"].Value);
            }
            catch
            {                
                nguia = "";
                throw;
            }
            return nguia;
        }
        public static void insertar_guia(string _guino, Int32 _trans, string _liq, out Int32 _valida, string tda_id)
        {
            string sqlquery = "USP_Insertar_Guia";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tra_gui_no", _guino);
                cmd.Parameters.AddWithValue("@tra_gui_traid", _trans);
                cmd.Parameters.AddWithValue("@liq_id", _liq);
                cmd.Parameters.Add("@valida_guia", SqlDbType.Int);
                cmd.Parameters["@valida_guia"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@tda_id", tda_id);
                cmd.ExecuteNonQuery();
                _valida = Convert.ToInt32(cmd.Parameters["@valida_guia"].Value);

            }
            catch (Exception exc)
            {               
                _valida = 1;
                throw;
            }
        }
    }
}
