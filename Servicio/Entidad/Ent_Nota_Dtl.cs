using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Servicio
{
    public class Ent_Nota_Dtl
    {

        #region < ATRIBUTOS >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        //public static string _conn = Constants.OrcleStringConn;

        /// <summary>
        /// 
        /// </summary>
        String RDV_CO;

        /// <summary>
        /// 
        /// </summary>
        String RDV_RETURN;

        /// <summary>
        /// 
        /// </summary>
        Decimal RDN_LINE;

        /// <summary>
        /// 
        /// </summary>
        String RDV_INVOICE;

        /// <summary>
        /// 
        /// </summary>
        String RDV_ARTICLE;

        /// <summary>
        /// 
        /// </summary>
        String RDV_SIZE;

        /// <summary>
        /// 
        /// </summary>
        Decimal RDN_QTY;

        /// <summary>
        /// 
        /// </summary>
        Decimal RDN_SELLPRICE;

        /// <summary>
        /// 
        /// </summary>
        Decimal RDN_DISSCOUNT_LIN;

        /// <summary>
        /// 
        /// </summary>
        Decimal RDN_COMMISSION;

        /// <summary>
        /// 
        /// </summary>
        Decimal RDN_HANDLING;

        /// <summary>
        /// 
        /// </summary>
        Decimal RDN_DISSCOUNT_GEN;

        /// <summary>
        /// 
        /// </summary>
        Decimal RDN_TAXES;

        /// <summary>
        /// 
        /// </summary>
        String RDV_STORAGE;

        String CALIDAD;

        #endregion


        #region < CONSTRUCTORES >

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_RDV_CO"></param>
        /// <param name="_RDV_RETURN"></param>
        /// <param name="_RDN_LINE"></param>
        /// <param name="_RDV_INVOICE"></param>
        /// <param name="_RDV_ARTICLE"></param>
        /// <param name="_RDV_SIZE"></param>
        /// <param name="_RDN_QTY"></param>
        /// <param name="_RDN_SELLPRICE"></param>
        /// <param name="_RDN_DISSCOUNT_LIN"></param>
        /// <param name="_RDN_COMMISSION"></param>
        /// <param name="_RDN_HANDLING"></param>
        /// <param name="_RDN_DISSCOUNT_GEN"></param>
        /// <param name="_RDN_TAXES"></param>
        public Ent_Nota_Dtl(String RDV_CO, String RDV_RETURN, Decimal RDN_LINE, String RDV_INVOICE,
            String RDV_ARTICLE, String RDV_SIZE, Decimal RDN_QTY, Decimal RDN_SELLPRICE,
            Decimal RDN_DISSCOUNT_LIN, Decimal RDN_COMMISSION, Decimal RDN_HANDLING,
            Decimal RDN_DISSCOUNT_GEN, Decimal RDN_TAXES)
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
            this.RDV_CO = RDV_CO;
            this.RDV_RETURN = RDV_RETURN;
            this.RDN_LINE = RDN_LINE;
            this.RDV_INVOICE = RDV_INVOICE;
            this.RDV_ARTICLE = RDV_ARTICLE;
            this.RDV_SIZE = RDV_SIZE;
            this.RDN_QTY = RDN_QTY;
            this.RDN_SELLPRICE = RDN_SELLPRICE;
            this.RDN_DISSCOUNT_LIN = RDN_DISSCOUNT_LIN;
            this.RDN_COMMISSION = RDN_COMMISSION;
            this.RDN_HANDLING = RDN_HANDLING;
            this.RDN_DISSCOUNT_GEN = RDN_DISSCOUNT_GEN;
            this.RDN_TAXES = RDN_TAXES;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RDV_ARTICLE"></param>
        /// <param name="RDV_SIZE"></param>
        /// <param name="RDN_QTY"></param>
        public Ent_Nota_Dtl(String RDV_INVOICE, String RDV_ARTICLE, String RDV_SIZE, Decimal RDN_QTY)
        {
            this._RDV_INVOICE = RDV_INVOICE;
            this._RDV_ARTICLE = RDV_ARTICLE;
            this._RDV_SIZE = RDV_SIZE;
            this._RDN_QTY = RDN_QTY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RDV_ARTICLE"></param>
        /// <param name="RDV_SIZE"></param>
        /// <param name="RDN_QTY"></param>
        public Ent_Nota_Dtl(String RDV_INVOICE, String RDV_ARTICLE, String RDV_SIZE, Decimal RDN_QTY, string RDV_STORAGE, String CALIDAD)
        {
            this._RDV_INVOICE = RDV_INVOICE;
            this._RDV_ARTICLE = RDV_ARTICLE;
            this._RDV_SIZE = RDV_SIZE;
            this._RDN_QTY = RDN_QTY;
            this._RDV_STORAGE = RDV_STORAGE;
            this._CALIDAD = CALIDAD;
        }


        #endregion


        #region < PROPIEDADES >
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string _CALIDAD
        {
            get { return CALIDAD; }
            set { CALIDAD = value; }
        }
        public String _RDV_CO
        {
            get { return RDV_CO; }
            set { RDV_CO = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String _RDV_RETURN
        {
            get { return RDV_RETURN; }
            set { RDV_RETURN = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Decimal _RDN_LINE
        {
            get { return RDN_LINE; }
            set { RDN_LINE = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String _RDV_INVOICE
        {
            get { return RDV_INVOICE; }
            set { RDV_INVOICE = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String _RDV_ARTICLE
        {
            get { return RDV_ARTICLE; }
            set { RDV_ARTICLE = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String _RDV_SIZE
        {
            get { return RDV_SIZE; }
            set { RDV_SIZE = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Decimal _RDN_QTY
        {
            get { return RDN_QTY; }
            set { RDN_QTY = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Decimal _RDN_SELLPRICE
        {
            get { return RDN_SELLPRICE; }
            set { RDN_SELLPRICE = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Decimal _RDN_DISSCOUNT_LIN
        {
            get { return RDN_DISSCOUNT_LIN; }
            set { RDN_DISSCOUNT_LIN = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Decimal _RDN_COMMISSION
        {
            get { return RDN_COMMISSION; }
            set { RDN_COMMISSION = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Decimal _RDN_HANDLING
        {
            get { return RDN_HANDLING; }
            set { RDN_HANDLING = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Decimal _RDN_DISSCOUNT_GEN
        {
            get { return RDN_DISSCOUNT_GEN; }
            set { RDN_DISSCOUNT_GEN = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Decimal _RDN_TAXES
        {
            get { return RDN_TAXES; }
            set { RDN_TAXES = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String _RDV_STORAGE
        {
            get { return RDV_STORAGE; }
            set { RDV_STORAGE = value; }
        }

        #endregion
    }
}
