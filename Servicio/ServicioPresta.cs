using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace Servicio
{
    public partial class ServicioPresta : ServiceBase
    {
        Timer tmservicio = null;

        Timer tmservicio_alm = null;
        private Int32 var_alm = 0;
        public ServicioPresta()
        {
            double minutos = 2;//5
            InitializeComponent();
            tmservicio = new Timer(minutos*60000);
            tmservicio.Elapsed += new ElapsedEventHandler(tmpServicio_Elapsed);

            #region/*insertar almacen*/
            double minutos_alm = 1;//5

            tmservicio_alm = new Timer(minutos_alm * 60000);
            tmservicio_alm.Elapsed += new ElapsedEventHandler(tmpServicio_Alm_Elapsed);
            #endregion

        }
        void tmpServicio_Elapsed(object sender, ElapsedEventArgs e)
        {
            //string path = @"C:\log.txt";
            string path = ConfigurationManager.ConnectionStrings["CarpetaLog"].ConnectionString;
            TextWriter tw = new StreamWriter(path, true);
            tw.WriteLine("A fecha de : " + DateTime.Now.ToString() + ", Intervalo: " + tmservicio.Interval.ToString());
            
            try
            {
                tw.WriteLine("Inicio del Proceso."+ DateTime.Now.ToString());
                //tw.WriteLine("Leer Pedido." + DateTime.Now.ToString());
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

        void tmpServicio_Alm_Elapsed(object sender, ElapsedEventArgs e)
        {
            //string path = @"C:\log.txt";
            TextWriter tw = null;

            try
            {

                if (var_alm == 0)
                {
                    var_alm = 1;
                    string path = ConfigurationManager.ConnectionStrings["CarpetaLogAlm"].ConnectionString;
                    tw = new StreamWriter(path, true);
                    tw.WriteLine("A fecha de : " + DateTime.Now.ToString() + ", Intervalo: " + tmservicio.Interval.ToString());

                    tw.WriteLine("Inicio del Proceso." + DateTime.Now.ToString());
                    //tw.WriteLine("Leer Pedido." + DateTime.Now.ToString());
                    LeerPedidos.ImportaDataPrestaShop_Alm(tw);
                    tw.WriteLine("Fin del Proceso." + DateTime.Now.ToString());
                    tw.Close();
                    var_alm = 0;
                }
                
            }
            catch (Exception ex)
            {
                tw.WriteLine("Error del Proceso." + DateTime.Now.ToString());
                tw.WriteLine(ex.Message.ToString());
                tw.Close();
                var_alm = 0;
            }
        }

        protected override void OnStart(string[] args)
        {
            tmservicio.Start();
            tmservicio_alm.Start();
        }

        protected override void OnStop()
        {
            tmservicio.Stop();
            tmservicio_alm.Stop();
        }

        /*MODO EN PRUEBAS*/
        internal void TestStartupAndStop()
        {
            string[] arg = new string[] { }; ;
            this.OnStart(arg);
            Console.ReadLine();
            this.OnStop();
        }

    }
}
