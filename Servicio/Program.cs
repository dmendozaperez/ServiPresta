using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;

namespace Servicio
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            /*MODO PRODUCCION*/
            ////ServiceBase[] ServicesToRun;
            ////ServicesToRun = new ServiceBase[]
            ////{
            ////    new ServicioPresta()
            ////};
            ////ServiceBase.Run(ServicesToRun);

            /**/

            ////string path = @"C:\log.txt";
            //string path = ConfigurationManager.ConnectionStrings["CarpetaLog"].ConnectionString;
            //TextWriter tw = new StreamWriter(path, true);
            //tw.WriteLine("A fecha de : " + DateTime.Now.ToString() + ", Intervalo: ");

            //try
            //{
            //    tw.WriteLine("Inicio del Proceso." + DateTime.Now.ToString());
            //    LeerPedidos.ImportaDataPrestaShop(tw);
            //    tw.WriteLine("Fin del Proceso." + DateTime.Now.ToString());
            //    tw.Close();
            //}
            //catch (Exception ex)
            //{
            //    tw.WriteLine("Error del Proceso." + DateTime.Now.ToString());
            //    tw.WriteLine(ex.Message.ToString());
            //    tw.Close();
            //}


            /*MODO PRUEBAS*/
            if (Environment.UserInteractive)
            {
                ServicioPresta service1 = new ServicioPresta();
                service1.TestStartupAndStop();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new ServicioPresta()
                };
                ServiceBase.Run(ServicesToRun);
            }

        }
    }
}
