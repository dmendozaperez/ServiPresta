using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Servicio
{
    public class Ent_Venta
    {      
        public string articulo { get; set; }
        public string marca { get; set; }
        public string color { get; set; }
        public decimal precio { get; set; }
        public decimal comision { get; set; }
        public decimal porc_comision { get; set; }
        public decimal total_pares { get; set; }
        public decimal total_linea { get; set; }
        public string ofe_tipo { get; set; }
        public decimal ofe_venArt { get; set; }
        public decimal ofe_id { get; set; }
        public decimal ofe_maxPares { get; set; }
        public decimal ofe_porc { get; set; }
        public decimal ofe_nroItem { get; set; }
        public decimal total_descto { get; set; }
        public string afec_percepcion { get; set; }
        public string foto { get; set; }
        public Boolean comision_activado { get; set; }
        public List<Ent_Venta_Talla> articulo_talla { get; set; }
    }
    public class Ent_Venta_Talla
    {
        public string talla { get; set; }
        public Decimal cantidad { get; set; }
        public Decimal stock { get; set; }
    }

    public class Ent_Venta_FormaPago
    {
        public string doc_tra_id { get; set; }
        public Int32 forma_items { get; set; }
        public string forma_pago_id { get; set; }
        public string forma_pago_nombre { get; set; }
        public string tarjeta_bines_ser { get; set; }
        public string tarjeta_bines_cod { get; set; }
        public string tarjeta_nombre { get; set; }
        public string tarjeta_numero { set; get; }
        public Decimal forma_monto { get; set; }
    }
    public class Ent_Venta_PagoNota
    {
        public string doc_tra_id { get; set; }
        public string nc_num { get; set; }
        public decimal total_nc { get; set; }
        public Boolean chknota { get; set; }
    }
    public class Ent_Cierre_Venta
    {
        public DateTime fecha_venta { get; set; }
        public Decimal inicio_caja { get; set; }
        public decimal total_venta { get; set; }
        public Decimal efectivo { get; set; }
        public decimal vuelto { get; set; }
        public decimal total_efectivo { get; set; }
        public decimal total_tarjeta { get; set; }
        public decimal total_caja { get; set; }
        public string banco_des { get; set; }
        public string nro_operacion { get; set; }
        public decimal monto_opera { get; set; }
    }
}
