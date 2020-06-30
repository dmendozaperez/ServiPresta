using Servicio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyPrueba
{
    public partial class Prueba : Form
    {
        public Prueba()
        {
            InitializeComponent();
        }

        private void btnalm_Click(object sender, EventArgs e)
        {
            string path = ConfigurationManager.ConnectionStrings["CarpetaLog"].ConnectionString;
            TextWriter tw = new StreamWriter(path, true);
            tw.WriteLine("Inicio del Proceso." + DateTime.Now.ToString());
            LeerPedidos.ImportaDataPrestaShop_Alm(tw);
            tw.WriteLine("Fin del Proceso." + DateTime.Now.ToString());
            tw.Close();
        }
    }
}
