﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio
{
    class Conexion
    {
        /*rpeuab*/

        /// <summary>
        /// Obtiene Conexión Principal y donde guarda Log de Procesos
        /// </summary>
        /// <returns></returns>
        public SqlConnection getConexion()
        {
            SqlConnection sqllog = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ConnectionString);//"Data Source=ecommerce.bgr.pe;Initial Catalog=BD_ECOMMERCE;Integrated Security=False;User ID=ecommerce;Password=Bata2018.*@=?++;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            return sqllog;
        }

        /// <summary>
        /// método que obtiene la Conexión para Obtener datos de Stock
        /// </summary>
        /// <returns>SQL Server Conexión - obtener Stock</returns>
        public SqlConnection getConexionSQL()
        {
            SqlConnection sql;
            DataTable result = new DataTable();

            using (SqlConnection con = getConexion())
            {
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("USP_Obtiene_DatosConexion", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar).Value = "03";

                    da.Fill(result);
                    con.Close();

                    sql = new SqlConnection("Data Source=" + result.Rows[0]["Url"].ToString() + ";Initial Catalog=" + result.Rows[0]["BaseDatos"].ToString() + ";User ID=" + result.Rows[0]["Usuario"].ToString() + ";Password=" + result.Rows[0]["Contrasena"].ToString() + ";TrustServerCertificate=" + result.Rows[0]["Trusted_Conection"].ToString() + "");

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return sql;
        }

        /// <summary>
        /// método que obtiene la Conexión para PrestaShop
        /// </summary>
        /// <returns>MySQL Conexion - PrestaShop</returns>
        public MySqlConnection getConexionMySQL()
        {
            MySqlConnection mysql;
            DataTable result = new DataTable();

            using (SqlConnection con = getConexion())//Conexión Principal
            {
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("USP_Obtiene_DatosConexion", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar).Value = "01";

                    da.Fill(result);
                    con.Close();

                    mysql = new MySqlConnection("Server=" + result.Rows[0]["Url"].ToString() + ";Database=" + result.Rows[0]["BaseDatos"].ToString() + ";Uid=" + result.Rows[0]["Usuario"].ToString() + ";Password=" + result.Rows[0]["Contrasena"].ToString() + "");

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return mysql;
        }

        /// <summary>
        /// método que obtiene los datos para la conexión al WebService de PrestaShop
        /// </summary>
        /// <returns>WebService Conexion - PrestaShop / URL - usuario - contrasena</returns>
        public DataTable getConexionWSPresta()
        {
            DataTable result = new DataTable();

            using (SqlConnection con = getConexion())//Conexión Principal
            {
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("USP_Obtiene_DatosConexion", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@Id", SqlDbType.VarChar).Value = "09";

                    da.Fill(result);
                    con.Close();
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return result;
        }
    }
}
