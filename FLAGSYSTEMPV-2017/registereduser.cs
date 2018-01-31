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
