using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integrado.Prestashop
{
    class TraerPedidos
    {
        MySqlConnection mysql;
        Conexion oConexionMySql = new Conexion();

        public DataTable PrepararPedidos()
        {
            DataTable Prestashop = new DataTable();
           
            try
            {

                mysql = oConexionMySql.getConexionMySQL();
                mysql.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = mysql;
                cmd.CommandText = "USP_LISTA_PEDIDOS";

                //asignar paramentros
                cmd.Parameters.AddWithValue("estado", 2);

                MySqlDataAdapter MySqlData = new MySqlDataAdapter(cmd);
                MySqlData.Fill(Prestashop);

            }
            catch (Exception)
            {
                Prestashop = null;
                
            }                       
            return Prestashop;

        }

        //public static void Main(string[] args)
        //{
        //    TraerPedidos ped = new TraerPedidos();
        //    DataTable imprimir = ped.PrepararPedidos();
        //    Console.ReadKey();
        //}
    }
}
