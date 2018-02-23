using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
namespace FLAGSYSTEMPV_2017
{
    class registereduser
    {
        public static string reguser;
        public static string level;
        public static string registeredlicense = getRegLicense();
        public static float saldoinicial;
        public static string smtp, puerto, ssl, mail, clave, para, titulo, cuerpo;
        //permisos del usuario
        public static string pventa,pcaja, pcompra, particulo, pclientes, pproveedores, pgastos, pstock, pcierredia, pdiferencia, pconsultaC, pconsultaV, pEScaja, pinformes, panular, pnotac, pnotad, pabstock, pconfig, pempleados, penviarinforme, pfiscalconfig, prubro;
        public static string redondeo, closeandbkp, sololectura, alwaysprint, tooltips;
       
        public static string getRegLicense()
        {
            Conexion.abrir();
            DataTable license= Conexion.Consultar("NombreEmpresa","Configuracion","","",new SqlCeCommand());
            Conexion.cerrar();
            string reglicense = license.Rows[0][0].ToString();
            return reglicense;

        }
    }
}
