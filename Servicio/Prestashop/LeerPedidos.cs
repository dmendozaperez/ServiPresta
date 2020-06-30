//using Bukimedia.PrestaSharp.Entities;
//using CapaDato.Bll.Ecommerce;
//using CapaEntidad.Bll.Ecommerce;
//using CapaEntidad.Bll.Util;
using Integrado.Prestashop;
using MySql.Data.MySqlClient;
//using Servicios.Ecommerce;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Servicio
{
    public class LeerPedidos
    {
        #region<EXTRAE LOS PEDIDOS PENDIENTES DE PRESTASHOP>

        public static DataTable PrepararPedidos()
        {
            DataTable Prestashop = new DataTable();

            try
            {
                MySqlConnection mysql;
                Conexion oConexionMySql = new Conexion();
                mysql = oConexionMySql.getConexionMySQL();
                mysql.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = mysql;
                cmd.CommandText = "USP_LISTA_PEDIDOS_xTda";

                //asignar paramentros
                cmd.Parameters.AddWithValue("codTda", "");

                MySqlDataAdapter MySqlData = new MySqlDataAdapter(cmd);
                MySqlData.Fill(Prestashop);

            }
            catch (Exception)
            {
                Prestashop = null;
                throw;
            }
            return Prestashop;

        }

        /// <summary>
        /// Se usa para obtener los registros de Pagos de los Pedidos
        /// </summary>
        /// <returns></returns>
        public static DataTable PrepararPedidos_Pagos()
        {
            DataTable Prestashop = new DataTable();

            try
            {
                MySqlConnection mysql;
                Conexion oConexionMySql = new Conexion();
                mysql = oConexionMySql.getConexionMySQL();
                mysql.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = mysql;
                cmd.CommandText = "USP_LISTA_PEDIDOS_PAGOS";

                //asignar paramentros
                cmd.Parameters.AddWithValue("estado", 2);

                MySqlDataAdapter MySqlData = new MySqlDataAdapter(cmd);
                MySqlData.Fill(Prestashop);

            }
            catch (Exception)
            {
                Prestashop = null;
                throw;
            }
            return Prestashop;

        }

        public static string ImportaDataPrestaShop(TextWriter log)
        {
            string _error = "";
            DataTable dtpedidos = null;
            DataTable dtpedidospag = null;
            try
            {
                dtpedidos = PrepararPedidos();
                dtpedidospag = PrepararPedidos_Pagos();

                if (dtpedidos != null && dtpedidospag != null)
                {
                    Dat_PrestaShop update_psth = new Dat_PrestaShop();


                    /*agrupamos los pedidos*/
                    var grupo_pedido = from item in dtpedidos.AsEnumerable()
                                           //where item.Field<string>("ped_id") == "73" //|| item.Field<Int32>("ped_id") ==40
                                       group item by
                                       new
                                       {
                                           ped_id = Convert.ToInt32(item["ped_id"]),
                                           // Modificado por : Henry Morales - 01/04/2019
                                           // Se agregó dato de Tienda
                                           tda_id = item["tda_id"].ToString(),
                                           ped_ref = item["ped_ref"].ToString(),
                                           ped_fecha = Convert.ToDateTime(item["ped_fecha"]),
                                           ped_ubigeo_ent = item["ped_ubigeo_ent"].ToString(),
                                           ped_dir_ent = item["ped_dir_ent"].ToString(),
                                           ped_ref_ent = item["ped_ref_ent"].ToString(),
                                           // Modificado por : Henry Morales - 21/05/2018
                                           // Se agergaron los campos de nombre y telefono de referencia para la entrega
                                           ped_nom_ent = item["ped_nom_ent"].ToString(),
                                           ped_tel_ent = item["ped_tel_ent"].ToString(),
                                           //det_peso =Convert.ToDecimal(item["det_peso"]),
                                           ped_total_sigv = Convert.ToDecimal(item["ped_total_sigv"]),
                                           ped_total_cigv = Convert.ToDecimal(item["ped_total_cigv"]),
                                           ped_dcto_sigv = Convert.ToDecimal(item["ped_dcto_sigv"]),
                                           ped_dcto_cigv = Convert.ToDecimal(item["ped_dcto_cigv"]),
                                           cli_id = item["cli_id"].ToString(),
                                           cli_nombres = item["cli_nombres"].ToString(),
                                           cli_apellidos = item["cli_apellidos"].ToString(),
                                           cli_email = item["cli_email"].ToString(),
                                           cli_direc = item["cli_direc"].ToString(),
                                           cli_telf = item["cli_telf"].ToString(),
                                           cli_telf_mov = item["cli_telf_mov"].ToString(),
                                           cli_dni = item["cli_dni"].ToString(),
                                           cli_ubigeo = item["cli_ubigeo"].ToString(),
                                           ped_ship_sigv = Convert.ToDecimal(item["ped_ship_sigv"]),
                                           name_courier = Convert.ToString(item["name_courier"]),
                                           // Modificado por : Henry Morales - 19/06/2018
                                           // Se modificó para tomar los pagos en diferentes formas de pago (DataTable dtpedidospag)
                                           /*pag_metodo = item["pag_metodo"].ToString(),
                                           pag_nro_trans = item["pag_nro_trans"].ToString(),
                                           pag_fecha = Convert.ToDateTime(item["pag_fecha"]),
                                           pag_monto = Convert.ToDecimal(item["pag_monto"]),*/
                                       }
                                       into G
                                       select new
                                       {
                                           pedido = G.Key.ped_id,
                                           tda_id = G.Key.tda_id,
                                           ped_ref = G.Key.ped_ref,
                                           ped_fecha = G.Key.ped_fecha,
                                           ped_ubigeo_ent = G.Key.ped_ubigeo_ent,
                                           ped_dir_ent = G.Key.ped_dir_ent,
                                           ped_ref_ent = G.Key.ped_ref_ent,
                                           // Modificado por : Henry Morales - 21/05/2018
                                           // Se agergaron los campos de nombre y telefono de referencia para la entrega
                                           ped_nom_ent = G.Key.ped_nom_ent,
                                           ped_tel_ent = G.Key.ped_tel_ent,
                                           cli_telf_mov = G.Key.cli_telf_mov,
                                           //det_peso=G.Key.det_peso,
                                           ped_total_sigv = G.Key.ped_total_sigv,
                                           ped_total_cigv = G.Key.ped_total_cigv,
                                           ped_dcto_sigv = G.Key.ped_dcto_sigv,
                                           ped_dcto_cigv = G.Key.ped_dcto_cigv,
                                           cli_id = G.Key.cli_id,
                                           cli_nombres = G.Key.cli_nombres,
                                           cli_apellidos = G.Key.cli_apellidos,
                                           cli_email = G.Key.cli_email,
                                           cli_direc = G.Key.cli_direc,
                                           cli_telef = G.Key.cli_telf,
                                           cli_dni = G.Key.cli_dni,
                                           cli_ubigeo = G.Key.cli_ubigeo,
                                           ped_ship_sigv = G.Key.ped_ship_sigv,
                                           name_courier = G.Key.name_courier,
                                           // Modificado por : Henry Morales - 19/06/2018
                                           // Se modificó para tomar los pagos en diferentes formas de pago (DataTable dtpedidospag)
                                           /*pag_metodo = G.Key.pag_metodo,
                                           pag_nro_trans = G.Key.pag_nro_trans,
                                           pag_fecha = G.Key.pag_fecha,
                                           pag_monto = G.Key.pag_monto*/
                                       };


                    /*recorremos los pedidos para agregar al pedido*/
                    foreach (var key in grupo_pedido)
                    {

                        /*verifica si pedido existe*/
                        Boolean _existe = update_psth.Existe_Pedido_Prestashop(key.pedido.ToString());
                        if (_existe)
                        {
                            /*Si ya existe pedido y no ha facturado*/
                        }
                        else
                        {
                            /*capturamos el detalle */
                            var ped_det = from item in dtpedidos.AsEnumerable()
                                          where item.Field<string>("ped_id") == Convert.ToString(key.pedido)
                                          //&& item.Field<string>("det_artic_ref").Length == 11
                                          select new
                                          {
                                              det_artic_ref = item["det_artic_ref"].ToString(),
                                              det_desc_artic = Convert.ToString(item["det_desc_artic"]),
                                              det_cant = Convert.ToInt32(item["det_cant"]),
                                              det_prec_sigv = Convert.ToDecimal(item["det_prec_sigv"]), // SQL : Liq_Det_Precio
                                              det_peso = Convert.ToDecimal(item["det_peso"]),
                                              //det_prec_cigv = Convert.ToDecimal(item["det_prec_cigv"]),
                                              det_dcto_sigv = Convert.ToDecimal(item["det_dcto_sigv"]), // SQL : Liq_Det_comision


                                          };
                            /*Recorremos el detalle*/
                            List<Order_Dtl> items_det = new List<Order_Dtl>();



                            Decimal _tot_peso = 0;
                            foreach (var key_det in ped_det)
                            {
                                Order_Dtl dtl = new Order_Dtl();
                                string articulo_talla = key_det.det_artic_ref.ToString().Trim().Replace("-", "");
                                string articulo = articulo_talla.Substring(0, articulo_talla.Length - 2);
                                string talla = articulo_talla.Substring(articulo_talla.Length - 2, 2);

                                dtl._code = articulo;
                                dtl._size = talla;

                                dtl._qty = Convert.ToInt32(key_det.det_cant);
                                dtl._priceigv = key_det.det_prec_sigv;
                                dtl._price = Convert.ToDecimal(Math.Round(Convert.ToDouble(key_det.det_prec_sigv), 2, MidpointRounding.AwayFromZero));
                                dtl._commissionPctg = 0;
                                dtl._commissionigv = 0;
                                dtl._det_dcto_sigv = Math.Round(key_det.det_dcto_sigv, 2, MidpointRounding.AwayFromZero);
                                dtl._commission = Convert.ToDecimal(Math.Round(Convert.ToDouble((dtl._det_dcto_sigv * dtl._qty)), 2, MidpointRounding.AwayFromZero));
                                dtl._ofe_porc = 0;
                                dtl._dsctoVale = 0;
                                dtl._ofe_id = 0;
                                dtl._art_des = key_det.det_desc_artic;
                                dtl._art_peso = key_det.det_peso;
                                _tot_peso += key_det.det_peso * key_det.det_cant;
                                items_det.Add(dtl);
                            }

                            #region <AJUSTE DESCUENTO>
                            Decimal ajuste_subtotal = key.ped_total_sigv;
                            var subtot = items_det.Sum(a => (a._price * a._qty) - a._commission);

                            subtot += key.ped_ship_sigv;

                            Decimal saldo = ajuste_subtotal - subtot;

                            if (saldo != 0)
                            {


                                if (items_det[0]._commission != 0)
                                {
                                    items_det[0]._commission = items_det[0]._commission - saldo;
                                }
                                else
                                {
                                    Int32 item_ult = items_det.Count() - 1;
                                    items_det[item_ult]._price = items_det[item_ult]._price + saldo;
                                }

                                // ¨****** Verificar
                                // Cambiar si el Saldo es diferente de 0,sumarlo al Pet_Det_Precio (dtl._price) 
                            }

                            #endregion

                            /*si esta lleno el list entonces agregamos el pedido en este ,metodo*/
                            if (items_det.Count > 0)
                            {
                                /*datos del cliente*/
                                Cliente cl = new Cliente();
                                cl.cli_nombres = key.cli_nombres;
                                cl.cli_apellidos = key.cli_apellidos;
                                cl.cli_email = key.cli_email;
                                cl.cli_ubigeo = key.cli_ubigeo;
                                cl.cli_direc = key.cli_direc;
                                cl.cli_telf = key.cli_telef;
                                cl.cli_telf_mov = key.cli_telf_mov;
                                cl.cli_dni = key.cli_dni;
                                /*********************/
                                /*metodo de pago*/
                                Pagos pg = new Pagos();
                                // Modificado por : Henry Morales - 19/06/2018
                                // Se modificó para tomar los pagos en diferentes formas de pago (DataTable dtpedidospag)
                                /*pg.pag_metodo = key.pag_metodo;
                                pg.pag_nro_trans = key.pag_nro_trans;
                                pg.pag_fecha = key.pag_fecha;
                                pg.pag_monto = key.pag_monto;*/
                                DataTable pago_ped = new DataTable();
                                pago_ped = dtpedidospag.Clone();
                                pago_ped.Clear();

                                foreach (DataRow row in dtpedidospag.Rows)
                                {
                                    if (row["ped_id"].ToString() == key.pedido.ToString())
                                    {
                                        pago_ped.ImportRow(row);
                                    }
                                }
                                /**/

                                decimal igv_monto = key.ped_dcto_cigv - key.ped_dcto_sigv;
                                //string[] pedido_update=

                                // Modificado por : Henry Morales - 19/06/2018
                                // Se agergo la tabla dtpedidospag, para enviar la información de diferentes formas de pago
                                // Modificado por : Henry Morales - 21/05/2018
                                // Se agergaron los campos de nombre y telefono de referencia para la entrega ( key.ped_nom_ent ; key.ped_tel_ent)
                                string[] result = update_psth.Update_Pedido_Prestashop(1/*Ent_Global._bas_id_codigo*/, key.tda_id , 9219, "", 0, 0, "", "", items_det, 0, 1, "", "", 0, 0, "", "", 0, null,
                                              false, 0, null, key.pedido, key.ped_ref, key.ped_ship_sigv, cl, pg, key.ped_fecha, key.ped_total_cigv, key.ped_ubigeo_ent,
                                              key.ped_dir_ent, key.ped_ref_ent, key.ped_nom_ent, key.ped_tel_ent, _tot_peso, pago_ped, key.name_courier);
                                if (result[0].ToString() == "-1")
                                {
                                    _error += result[1].ToString();
                                }
                                else
                                {
                                    // Se agrego para que procese luego de lectura de PS
                                    // Guia de Remisión
                                    Int32 _valida_guia;
                                    string cod_liq = result[0].ToString();
                                    Dat_ConfigGuia.insertar_guia("0", 9, cod_liq, out _valida_guia, key.tda_id);

                                    // Genera paquete
                                    //decimal _paq_id;
                                    //_paq_id = Dat_Venta.insertar_leer_paquete(cod_liq);

                                    // Generación de Facturación 
                                    string _error_venta = "";
                                    string grabar_numerodoc = Dat_Venta.insertar_venta(cod_liq, ref _error_venta);
                                    if (grabar_numerodoc != "-1")
                                    {

                                        //aca generamos el codigo hash
                                        string _codigo_hash = "";
                                        string _error2 = "";
                                        string _url_pdf = "";

                                        Facturacion_Electronica.ejecutar_factura_electronica(grabar_numerodoc.Substring(0, 1), grabar_numerodoc, ref _codigo_hash, ref _error2, ref _url_pdf);

                                        //Facturacion_Electronica.ejecutar_factura_electronica_ws(grabar_numerodoc.Substring(0, 1), grabar_numerodoc, ref _codigo_hash, ref _error,ref _url_pdf);

                                        if (_codigo_hash.Length == 0 || _codigo_hash == null)
                                        {
                                            Facturacion_Electronica.ejecutar_factura_electronica(grabar_numerodoc.Substring(0, 1), grabar_numerodoc, ref _codigo_hash, ref _error2, ref _url_pdf);
                                        }
                                        if (_codigo_hash.Length == 0 || _codigo_hash == null)
                                        {
                                            _error = "ERROR HASH:";
                                        }

                                        if (_error.Length > 0)
                                        {
                                            log.WriteLine(_error+ " " + _error2 + " En Documento: "+ grabar_numerodoc);
                                            /*Error*/
                                        }

                                        Dat_Venta.insertar_codigo_hash(grabar_numerodoc, _codigo_hash, "V", _url_pdf);
                                        UpdaEstado updateestado = new UpdaEstado();
                                        string[] valida = updateestado.ActualizaEstadoPS(key.ped_ref, 33);

                                        if(key.name_courier.Contains("iend"))
                                        {
                                            string[] valida2 = updateestado.ActualizaEstadoPS(key.ped_ref, 3);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception exc)
            {
                throw exc;
                _error = exc.Message;
            }
            return _error;
        }

        #endregion

        #region<EXTRAE LOS PEDIDO DE ALMACEN>

    
        public static DataTable PrepararPedidos_Alm()
        {
            DataTable Prestashop = new DataTable();

            try
            {
                MySqlConnection mysql;
                Conexion oConexionMySql = new Conexion();
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
            catch (Exception exc)
            {
                Prestashop = null;
                throw;
            }
            return Prestashop;

        }

        public static string ImportaDataPrestaShop_Alm(TextWriter log)
        {
            string _error = "";
            DataTable dtpedidos = null;
            DataTable dtpedidospag = null;
            ActTmpPresta upd_tmp = null;
            Dat_PrestaShop error_presta = null;
            try
            {
                error_presta = new Dat_PrestaShop();
                dtpedidos = PrepararPedidos_Alm();
                dtpedidospag = PrepararPedidos_Pagos();

                upd_tmp = new ActTmpPresta();

                if (dtpedidos != null && dtpedidospag != null)
                {
                    Dat_PrestaShop update_psth = new Dat_PrestaShop();


                    /*agrupamos los pedidos*/
                    var grupo_pedido = from item in dtpedidos.AsEnumerable()
                                           //where item.Field<string>("ped_id") == "73" //|| item.Field<Int32>("ped_id") ==40
                                       group item by
                                       new
                                       {
                                           ped_id = Convert.ToInt32(item["ped_id"]),
                                           ped_ref = item["ped_ref"].ToString(),
                                           ped_fecha = Convert.ToDateTime(item["ped_fecha"]),
                                           ped_ubigeo_ent = item["ped_ubigeo_ent"].ToString(),
                                           ped_dir_ent = item["ped_dir_ent"].ToString(),
                                           ped_ref_ent = item["ped_ref_ent"].ToString(),
                                           // Modificado por : Henry Morales - 21/05/2018
                                           // Se agergaron los campos de nombre y telefono de referencia para la entrega
                                           ped_nom_ent = item["ped_nom_ent"].ToString(),
                                           ped_tel_ent = item["ped_tel_ent"].ToString(),
                                           //det_peso =Convert.ToDecimal(item["det_peso"]),
                                           ped_total_sigv = Convert.ToDecimal(item["ped_total_sigv"]),
                                           ped_total_cigv = Convert.ToDecimal(item["ped_total_cigv"]),
                                           ped_dcto_sigv = Convert.ToDecimal(item["ped_dcto_sigv"]),
                                           ped_dcto_cigv = Convert.ToDecimal(item["ped_dcto_cigv"]),
                                           cli_id = item["cli_id"].ToString(),
                                           cli_nombres = item["cli_nombres"].ToString(),
                                           cli_apellidos = item["cli_apellidos"].ToString(),
                                           cli_email = item["cli_email"].ToString(),
                                           cli_direc = item["cli_direc"].ToString(),
                                           cli_telf = item["cli_telf"].ToString(),
                                           cli_telf_mov = item["cli_telf_mov"].ToString(),
                                           cli_dni = item["cli_dni"].ToString(),
                                           cli_ubigeo = item["cli_ubigeo"].ToString(),
                                           ped_ship_sigv = Convert.ToDecimal(item["ped_ship_sigv"]),
                                           name_courier = Convert.ToString(item["name_courier"]),
                                           // Modificado por : Henry Morales - 19/06/2018
                                           // Se modificó para tomar los pagos en diferentes formas de pago (DataTable dtpedidospag)
                                           /*pag_metodo = item["pag_metodo"].ToString(),
                                           pag_nro_trans = item["pag_nro_trans"].ToString(),
                                           pag_fecha = Convert.ToDateTime(item["pag_fecha"]),
                                           pag_monto = Convert.ToDecimal(item["pag_monto"]),*/
                                       }
                                       into G
                                       select new
                                       {
                                           pedido = G.Key.ped_id,
                                           ped_ref = G.Key.ped_ref,
                                           ped_fecha = G.Key.ped_fecha,
                                           ped_ubigeo_ent = G.Key.ped_ubigeo_ent,
                                           ped_dir_ent = G.Key.ped_dir_ent,
                                           ped_ref_ent = G.Key.ped_ref_ent,
                                           // Modificado por : Henry Morales - 21/05/2018
                                           // Se agergaron los campos de nombre y telefono de referencia para la entrega
                                           ped_nom_ent = G.Key.ped_nom_ent,
                                           ped_tel_ent = G.Key.ped_tel_ent,
                                           cli_telf_mov = G.Key.cli_telf_mov,
                                           //det_peso=G.Key.det_peso,
                                           ped_total_sigv = G.Key.ped_total_sigv,
                                           ped_total_cigv = G.Key.ped_total_cigv,
                                           ped_dcto_sigv = G.Key.ped_dcto_sigv,
                                           ped_dcto_cigv = G.Key.ped_dcto_cigv,
                                           cli_id = G.Key.cli_id,
                                           cli_nombres = G.Key.cli_nombres,
                                           cli_apellidos = G.Key.cli_apellidos,
                                           cli_email = G.Key.cli_email,
                                           cli_direc = G.Key.cli_direc,
                                           cli_telef = G.Key.cli_telf,
                                           cli_dni = G.Key.cli_dni,
                                           cli_ubigeo = G.Key.cli_ubigeo,
                                           ped_ship_sigv = G.Key.ped_ship_sigv,
                                           name_courier = G.Key.name_courier
                                           // Modificado por : Henry Morales - 19/06/2018
                                           // Se modificó para tomar los pagos en diferentes formas de pago (DataTable dtpedidospag)
                                           /*pag_metodo = G.Key.pag_metodo,
                                           pag_nro_trans = G.Key.pag_nro_trans,
                                           pag_fecha = G.Key.pag_fecha,
                                           pag_monto = G.Key.pag_monto*/
                                       };


                    /*recorremos los pedidos para agregar al pedido*/
                    foreach (var key in grupo_pedido)
                    {

                        /*verifica si pedido existe*/
                        Boolean _existe = update_psth.Existe_Pedido_Prestashop(key.pedido.ToString());
                        if (!_existe)
                        {
                            /*capturamos el detalle */
                            var ped_det = from item in dtpedidos.AsEnumerable()
                                          where item.Field<string>("ped_id") == Convert.ToString(key.pedido)
                                          //&& item.Field<string>("det_artic_ref").Length == 11
                                          select new
                                          {
                                              det_artic_ref = item["det_artic_ref"].ToString(),
                                              det_desc_artic = Convert.ToString(item["det_desc_artic"]),
                                              det_cant = Convert.ToInt32(item["det_cant"]),
                                              det_prec_sigv = Convert.ToDecimal(item["det_prec_sigv"]),
                                              det_peso = Convert.ToDecimal(item["det_peso"]),
                                              //det_prec_cigv = Convert.ToDecimal(item["det_prec_cigv"]),
                                              det_dcto_sigv = Convert.ToDecimal(item["det_dcto_sigv"]),


                                          };
                            /*Recorremos el detalle*/
                            List<Order_Dtl> items_det = new List<Order_Dtl>();



                            Decimal _tot_peso = 0;
                            foreach (var key_det in ped_det)
                            {
                                Order_Dtl dtl = new Order_Dtl();
                                string articulo_talla = key_det.det_artic_ref.ToString().Trim().Replace("-", "");
                                //string articulo = articulo_talla.Substring(0, articulo_talla.Length - 2);
                                //string talla = articulo_talla.Substring(articulo_talla.Length - 2, 2);
                                int vsubstrlenfin = 0;
                                string articulo = articulo_talla.Substring(0, 7);
                                vsubstrlenfin = articulo_talla.Length - articulo.Length;
                                string talla = articulo_talla.Substring(articulo.Length, vsubstrlenfin);

                                dtl._code = articulo;
                                dtl._size = talla;
                                dtl._qty = Convert.ToInt32(key_det.det_cant);
                                dtl._priceigv = key_det.det_prec_sigv;
                                dtl._price = Convert.ToDecimal(Math.Round(Convert.ToDouble(key_det.det_prec_sigv), 2, MidpointRounding.AwayFromZero));
                                dtl._commissionPctg = 0;
                                dtl._commissionigv = 0;
                                dtl._det_dcto_sigv = Math.Round(key_det.det_dcto_sigv, 2, MidpointRounding.AwayFromZero);
                                dtl._commission = Convert.ToDecimal(Math.Round(Convert.ToDouble((dtl._det_dcto_sigv * dtl._qty)), 2, MidpointRounding.AwayFromZero));
                                dtl._ofe_porc = 0;
                                dtl._dsctoVale = 0;
                                dtl._ofe_id = 0;
                                dtl._art_des = key_det.det_desc_artic;
                                dtl._art_peso = key_det.det_peso;
                                _tot_peso += key_det.det_peso * key_det.det_cant;
                                items_det.Add(dtl);
                            }

                            #region <AJUSTE DESCUENTO>
                            Decimal ajuste_subtotal = key.ped_total_sigv;
                            var subtot = items_det.Sum(a => (a._price * a._qty) - a._commission);

                            subtot += key.ped_ship_sigv;

                            Decimal saldo = ajuste_subtotal - subtot;

                            if (saldo != 0)
                            {


                                if (items_det[0]._commission != 0)
                                {
                                    items_det[0]._commission = items_det[0]._commission - saldo;
                                }
                                else
                                {
                                    Int32 item_ult = items_det.Count() - 1;
                                    items_det[item_ult]._price = items_det[item_ult]._price + saldo;
                                }

                                // ¨****** Verificar
                                // Cambiar si el Saldo es diferente de 0,sumarlo al Pet_Det_Precio (dtl._price) 
                            }

                            #endregion


                            /*si esta lleno el list entonces agregamos el pedido en este ,metodo*/
                            if (items_det.Count > 0)
                            {
                                /*datos del cliente*/
                                Cliente cl = new Cliente();
                                cl.cli_nombres = key.cli_nombres;
                                cl.cli_apellidos = key.cli_apellidos;
                                cl.cli_email = key.cli_email;
                                cl.cli_ubigeo = key.cli_ubigeo;
                                cl.cli_direc = key.cli_direc;
                                cl.cli_telf = key.cli_telef;
                                cl.cli_telf_mov = key.cli_telf_mov;
                                cl.cli_dni = key.cli_dni;
                                /*********************/
                                /*metodo de pago*/
                                Pagos pg = new Pagos();
                                // Modificado por : Henry Morales - 19/06/2018
                                // Se modificó para tomar los pagos en diferentes formas de pago (DataTable dtpedidospag)
                                /*pg.pag_metodo = key.pag_metodo;
                                pg.pag_nro_trans = key.pag_nro_trans;
                                pg.pag_fecha = key.pag_fecha;
                                pg.pag_monto = key.pag_monto;*/
                                DataTable pago_ped = new DataTable();
                                pago_ped = dtpedidospag.Clone();
                                pago_ped.Clear();

                                foreach (DataRow row in dtpedidospag.Rows)
                                {
                                    if (row["ped_id"].ToString() == key.pedido.ToString())
                                    {
                                        pago_ped.ImportRow(row);
                                    }
                                }
                                /**/

                                decimal igv_monto = key.ped_dcto_cigv - key.ped_dcto_sigv;
                                //string[] pedido_update=

                                // Modificado por : Henry Morales - 19/06/2018
                                // Se agergo la tabla dtpedidospag, para enviar la información de diferentes formas de pago
                                // Modificado por : Henry Morales - 21/05/2018
                                // Se agergaron los campos de nombre y telefono de referencia para la entrega ( key.ped_nom_ent ; key.ped_tel_ent)
                                string[] result = update_psth.Update_Pedido_Prestashop_Alm(1, 9219, "", 0, 0, "", "", items_det, 0, 1, "", "", 0, 0, "", "", 0, null,
                                              false, 0, null, key.pedido, key.ped_ref, key.ped_ship_sigv, cl, pg, key.ped_fecha, key.ped_total_cigv, key.ped_ubigeo_ent,
                                              key.ped_dir_ent, key.ped_ref_ent, key.ped_nom_ent, key.ped_tel_ent, _tot_peso, pago_ped, key.name_courier);
                                if (result[0].ToString() == "-1")
                                {                                    
                                    _error += result[1].ToString();
                                    error_presta.insertar_error(key.pedido.ToString(), result[1].ToString());
                                }
                                else
                                {
                                    upd_tmp.Insertar_ped_temp(key.pedido);
                                }

                            }
                        }
                        else /*pedido se guardan en el temporal*/
                        {
                            upd_tmp.Insertar_ped_temp(key.pedido);
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                error_presta.insertar_error("CONEXION", exc.Message);
                _error = exc.Message;
            }
            return _error;
        }

        #endregion

    }
}
