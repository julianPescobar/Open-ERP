using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FLAGSYSTEMPV_2017
{
    class totalventa
    {
     //
        //VENTAS
        public static string totventa; 
        public static int idventa;
        public static DataTable detalle;
        //
        //COMPRAS
        public static string totcompra;
        public static string impuestoextra;
        public static int idcompra;
        public static DataTable detallecompra;
        public static string fechacompra;
        //
        //NOTAS DE CREDITO
        public static string totnotacred;
        public static int idnotacred;
        public static DataTable detallenotacred;
        //
        //NOTAS DE DEBITO
        public static string totnotadeb;
        public static int idnotadeb;
        public static DataTable detallenotadeb;
        //
        //USOS VARIOS
        public static decimal cantidad;         //lo uso para cuando se apreta f4 guardo la cantidad del prod en esta variable
        public static string codprodbuscado;    //lo uso para cuando se apreta f5 guardo el cod del prod en esta variable
        public static string compraoventa;      //lo uso para saber si estoy en form compra o venta, para no crear 2 forms uso el mismo pero uso una variable para mostrar una cosa o la otra
        public static string proveedcompra;     //lo uso para no crear 2 buscadores de articulos, le paso el proveedor y hago el where proveedor = este string
        public static float montocompra;        //lo uso para cuando se apreta f3 guarda el monto de compra del prod selecc
        //
    }
}
