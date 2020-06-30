using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Servicio
{
    public class Facturacion_Electronica
    {

        public static string _ws_ruc;
        public static string _ws_login;
        public static string _ws_password;
        public static string pr_facturador;
        /*public static void FE_QR(string _tipo_doc, string _num_doc, ref Byte[] img_qr, ref string _error)
        {
            string _formato_doc = "";
            try
            {
                _formato_doc = Dat_Venta._leer_formato_electronico(_tipo_doc, _num_doc, ref _error);
                GeneratorCdp generatorCdp = new GeneratorCdp();
                if (_tipo_doc == "B" || _tipo_doc == "F")
                {
                    img_qr = generatorCdp.GetImageQrCodeForInvoiceCdp(_formato_doc);

                }
                else
                {
                    img_qr = generatorCdp.GetImageQrCodeForNoteCdp(_formato_doc);
                }

            }
            catch (Exception exc)
            {

                _error = exc.Message;
            }
        }*/

        #region<METODO ESTATICA PARA LA FACTURACION ELECTRONICA>
        /// <summary>
        /// configuracion de la facturacion electronica paperless
        /// </summary>
        public static void config_ws_fe()
        {
            string sqlquery = "USP_LeerConfig_FE";
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_ECOM"].ToString()))
                {
                    try
                    {
                        if (cn.State == 0) cn.Open();
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@ws_ruc", SqlDbType.VarChar, 20);
                            cmd.Parameters.Add("@ws_login", SqlDbType.VarChar, 20);
                            cmd.Parameters.Add("@ws_password", SqlDbType.VarChar, 20);
                            cmd.Parameters.Add("@pr_factura", SqlDbType.VarChar, 1);

                            cmd.Parameters["@ws_ruc"].Direction = ParameterDirection.Output;
                            cmd.Parameters["@ws_login"].Direction = ParameterDirection.Output;
                            cmd.Parameters["@ws_password"].Direction = ParameterDirection.Output;
                            cmd.Parameters["@pr_factura"].Direction = ParameterDirection.Output;

                            cmd.ExecuteNonQuery();

                            _ws_ruc = cmd.Parameters["@ws_ruc"].Value.ToString();
                            _ws_login = cmd.Parameters["@ws_login"].Value.ToString();
                            _ws_password = cmd.Parameters["@ws_password"].Value.ToString();
                            pr_facturador = cmd.Parameters["@pr_factura"].Value.ToString();
                        }

                    }
                    catch (Exception exc)
                    {


                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch
            {


            }
        }

        #endregion
        public static void ejecutar_factura_electronica(string _tipo_doc, string _num_doc, ref string cod_hash, ref string _error, ref string url_pdf)
        {
            string _formato_doc = "";

            try
            {

                /*if (Ent_Global._canal_venta == "AQ")
                {
                    if (Ent_Conexion._Base_Datos != "BdAquarella")
                    {
                        cod_hash = "prueba";
                        return;
                    } 
                }*/
                /*if (Ent_Global._canal_venta == "BA")
                {
                    if (Ent_Conexion._Base_Datos != "BD_ECOMMERCE")
                    {
                        cod_hash = "prueba";
                        return;
                    }
                        
                }*/

                /*QUIERE DECIR QUE QUE SE USA LA FACTURACION ELECTRONICA DE CARVAJAL*/
                /*if (Ent_Global.pr_facturador=="C")
                { 
                    _formato_doc =Dat_Venta._leer_formato_electronico(_tipo_doc, _num_doc, ref _error);
                    GeneratorCdp generatorCdp = new GeneratorCdp();
                    //XmlDocument xmlDoc = new XmlDocument();
                    //xmlDoc.Load("C:\\carvajal\\xml\\20101951872_07_F030_22.xml");

                    //byte[] _valor=generatorCdp.GetImageBarCodeForNoteCdp(_formato_doc);

                    if (_tipo_doc == "B" || _tipo_doc == "F")
                    {
                        cod_hash = generatorCdp.GetHashForInvoiceCdp(_formato_doc);
                    }
                    else
                    {
                        cod_hash = generatorCdp.GetHashForNoteCdp(_formato_doc);
                    }
                }*/
                /*ESTA CONDICION ES EL PROVEEDOR PAPERLESS*/
                /*if (Ent_Global.pr_facturador == "P")
                {
                */
                config_ws_fe();

                string return_numdoc = "";

                _formato_doc = Dat_Venta._leer_formato_electronico_PAPERLESS(_tipo_doc, _num_doc, ref _error, ref return_numdoc);
                string ruc_empresa = _ws_ruc; string ws_login = _ws_login; string ws_pass = _ws_password; Int32 tipofoliacion = 1;
                Int32 id_tipo_doc = 0;
                switch (_tipo_doc)
                {
                    case "B":
                    case "F":
                        id_tipo_doc = (_num_doc.Substring(0, 1) == "B" ? 3 : 1);
                        break;
                    case "N":
                        id_tipo_doc = 7;
                        break;
                }
                ///*0 = ID asignado
                //1 = URL del XML
                //2 = URL del PDF
                //3 = Estado en la SUNAT
                //4 = Folio Asignado(Serie - Correlativo)
                //5 = Bytes del PDF en Base64
                //6 = PDF417(Cadena de texto a imprimir en el PDF 417)
                //7 = HASH(Cadena de texto)*/
                /* htt p://200.121.128.110:8080/axis2/services/Online?wsdl */
                FEBata.OnlinePortTypeClient gen_fe = new FEBata.OnlinePortTypeClient();
      

                string consulta = gen_fe.OnlineGeneration(ruc_empresa, ws_login, ws_pass, _formato_doc, tipofoliacion, 7);

                consulta = consulta.Replace("&", "amp;");

                var doc = XDocument.Parse(consulta);
                var result = from factura in doc.Descendants("Respuesta")
                             select new
                             {
                                 Codigo = factura.Element("Codigo").Value,
                                 Mensaje = factura.Element("Mensaje").Value.Replace("amp;", "&"),
                             };

                foreach (var item in result)
                {
                    if (item.Codigo != "0")
                    {
                        _error = item.Mensaje;
                        break;
                    }
                    else
                    {
                        cod_hash = item.Mensaje;

                        /*SI LA GENERACION ES EXITOSA ENTONCES EXTRAEMOS EL PDF URL*/
                        consulta = gen_fe.OnlineRecovery(ruc_empresa, ws_login, ws_pass, id_tipo_doc, return_numdoc, 2);
                        consulta = consulta.Replace("&", "amp;");
                        var docpdf = XDocument.Parse(consulta);
                        var resultpdf = from factura in docpdf.Descendants("Respuesta")
                                        select new
                                        {
                                            Codigo = factura.Element("Codigo").Value,
                                            Mensaje = factura.Element("Mensaje").Value.Replace("amp;", "&"),
                                        };
                        foreach (var itempdf in resultpdf)
                        {
                            url_pdf = itempdf.Mensaje;
                        }
                        /*******/
                    }
                }
                /*}*/
                //enviar_xml_webservice bata===>>>




            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
        }

        public static void ejecutar_factura_electronica_ws(string _tipo_doc, string _num_doc, ref string cod_hash, ref string _error, ref string url_pdf)
        {
            string _formato_doc = "";

            try
            {
                config_ws_fe();

                string return_numdoc = "";
                _formato_doc = Dat_Venta._leer_formato_electronico_PAPERLESS(_tipo_doc, _num_doc, ref _error, ref return_numdoc);
                string ruc_empresa = _ws_ruc; string ws_login = _ws_login; string ws_pass = _ws_password; Int32 tipofoliacion = 1;

                ///*0 = ID asignado
                //1 = URL del XML
                //2 = URL del PDF
                //3 = Estado en la SUNAT
                //4 = Folio Asignado(Serie - Correlativo)
                //5 = Bytes del PDF en Base64
                //6 = PDF417(Cadena de texto a imprimir en el PDF 417)
                //7 = HASH(Cadena de texto)*/

                FEBata.OnlinePortTypeClient gen_fe = new FEBata.OnlinePortTypeClient();

                string consulta = gen_fe.OnlineGeneration(ruc_empresa, ws_login, ws_pass, _formato_doc, tipofoliacion, 7);

                consulta = consulta.Replace("&", "amp;");

                var doc = XDocument.Parse(consulta);
                var result = from factura in doc.Descendants("Respuesta")
                             select new
                             {
                                 Codigo = factura.Element("Codigo").Value,
                                 Mensaje = factura.Element("Mensaje").Value.Replace("amp;", "&"),
                             };

                foreach (var item in result)
                {
                    if (item.Codigo != "0")
                    {
                        _error = item.Mensaje;
                        break;
                    }
                    else
                    {
                        cod_hash = item.Mensaje;

                        /*SI LA GENERACION ES EXITOSA ENTONCES EXTRAEMOS EL PDF URL*/
                        consulta = gen_fe.OnlineGeneration(ruc_empresa, ws_login, ws_pass, _formato_doc, tipofoliacion, 2);
                        consulta = consulta.Replace("&", "amp;");
                        var resultpdf = from factura in doc.Descendants("Respuesta")
                                        select new
                                        {
                                            Codigo = factura.Element("Codigo").Value,
                                            Mensaje = factura.Element("Mensaje").Value.Replace("amp;", "&"),
                                        };
                        foreach (var itempdf in resultpdf)
                        {
                            url_pdf = itempdf.Mensaje;
                        }
                        /*******/
                    }
                }

                //GeneratorCdp generatorCdp = new GeneratorCdp();
                //XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load("C:\\carvajal\\xml\\20101951872_07_F030_22.xml");

                //byte[] _valor=generatorCdp.GetImageBarCodeForNoteCdp(_formato_doc);

                //if (_tipo_doc == "B" || _tipo_doc == "F")
                //{
                //    cod_hash = generatorCdp.GetHashForInvoiceCdp(_formato_doc);
                //}
                //else
                //{
                //    cod_hash = generatorCdp.GetHashForNoteCdp(_formato_doc);
                //}
                //enviar_xml_webservice bata===>>>




            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
        }
    }
}
