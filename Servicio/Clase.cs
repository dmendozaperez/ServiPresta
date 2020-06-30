using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio
{
    public class Clase
    {
        public static void Main()
        {
            //string path = @"C:\log.txt";
            string path = ConfigurationManager.ConnectionStrings["CarpetaLog"].ConnectionString;
            TextWriter tw = new StreamWriter(path, true);
            tw.WriteLine("A fecha de : " + DateTime.Now.ToString() + ", Intervalo: " );

            try
            {
                tw.WriteLine("Inicio del Proceso." + DateTime.Now.ToString());
                LeerPedidos.ImportaDataPrestaShop(tw);
                tw.WriteLine("Fin del Proceso." + DateTime.Now.ToString());
                tw.Close();
            }
            catch (Exception ex)
            {
                tw.WriteLine("Error del Proceso." + DateTime.Now.ToString());
                tw.WriteLine(ex.Message.ToString());
                tw.Close();
            }
        }
    }
}