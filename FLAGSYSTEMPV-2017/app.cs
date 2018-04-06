using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
namespace FLAGSYSTEMPV_2017
{
    class app
    {
        public static string dir = Directory.GetCurrentDirectory();
        public static string hoy;
        public static void chequearconfigmail()
        {
            if (registereduser.smtp.ToString().Length <= 0 || registereduser.puerto.ToString().Length <= 0 || registereduser.mail.ToString().Length <= 0 || registereduser.clave.ToString().Length <= 0 || registereduser.para.ToString().Length <= 0 || registereduser.titulo.ToString().Length <= 0 || registereduser.cuerpo.ToString().Length <= 0)
            {
                MessageBox.Show(
                    registereduser.smtp
                    +
                    "\n"
                    +
                    registereduser.puerto
                    +
                    "\n"
                    +
                    registereduser.mail
                    +
                    "\n"
                    +
                registereduser.clave
                    +
                    "\n"
                    +
                    registereduser.para
                    +
                    "\n"
                    +
                    registereduser.titulo
                    +
                    "\n"
                    +
                    registereduser.cuerpo
                    +
                    "\n");

                MessageBox.Show("DEBE CONFIGURAR TODOS LOS DATOS DE ENVIO DE EMAIL PARA PODER ENVIAR EMAILS.\nPara ello deberá dirijirse a Administración>Configuración y luego configurar todo el panel de email","Datos incompletos",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            }
    }
}
