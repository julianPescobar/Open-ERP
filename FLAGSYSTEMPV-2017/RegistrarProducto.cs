using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.Data.SqlClient;

namespace FLAGSYSTEMPV_2017
{
    public partial class RegistrarProducto : Form
    {
        public RegistrarProducto()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Demo.activardemo();
            if (Application.OpenForms.OfType<Inicio>().Count() == 1)
                Application.OpenForms.OfType<Inicio>().First().Focus();
            else
            {
                Inicio frm = new Inicio();
                frm.Show();
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("La licencia se ha activado correctamente\nRecuerde que esta licencia tiene validez para 3 PCs. En caso de rotura de una de sus PCs contáctese con servicio técnico para reestablecer su licencia","La licencia se ha activado con exito!");
        }

        private void RegistrarProducto_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.flag.com.ar");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Conexion.abrir();
            DataTable haylicencia = Conexion.Consultar("*", "Configuracion", "", "", new SqlCeCommand());
            Conexion.cerrar();
            try
            {
                if (haylicencia.Rows.Count < 1)
                {
                    SqlCeCommand f = new SqlCeCommand();
                    f.Parameters.AddWithValue("saldoini", "0");
                    f.Parameters.AddWithValue("fia", DateTime.Now);
                    f.Parameters.AddWithValue("nomb", "flag informatica");
                    f.Parameters.AddWithValue("dire", "guanahani 146");
                    f.Parameters.AddWithValue("mail", "info@flag.com.ar");
                    f.Parameters.AddWithValue("telA", "43075103");
                    f.Parameters.AddWithValue("loca", "capital federal");
                    f.Parameters.AddWithValue("cuite", "30709523097");
                    f.Parameters.AddWithValue("usaimpfi", "no");
                    f.Parameters.AddWithValue("puerto", "0");
                    f.Parameters.AddWithValue("masteruser", "ni idea q poner");
                    f.Parameters.AddWithValue("smtp", "smtp.gmail.com");
                    f.Parameters.AddWithValue("puertomail", "587");
                    f.Parameters.AddWithValue("mmail", "empleado@gmail.com");
                    f.Parameters.AddWithValue("mclave", "test");
                    f.Parameters.AddWithValue("mpara", "paramisupervisor@gmail.com");
                    f.Parameters.AddWithValue("mtitulo", "titulo del mail");
                    f.Parameters.AddWithValue("mcuerpo", "cuerpo del mail");
                    f.Parameters.AddWithValue("uft", DateTime.Now);
                    f.Parameters.AddWithValue("fta", DateTime.Now);
                    f.Parameters.AddWithValue("redondo", "0");
                    f.Parameters.AddWithValue("backupearsiempre", "no");
                    f.Parameters.AddWithValue("modoreadonly", "no");
                    f.Parameters.AddWithValue("alwaysp", "no");
                    f.Parameters.AddWithValue("tooltipz", "si");
                    Conexion.abrir();
                    Conexion.Insertar("Configuracion", "SaldoInicial,FechaInicioActividades,NombreEmpresa,DireccionFisica,Email,Telefono1,Localidad,CUIT,usaimpfiscal,PUERTO_IF,master_user_id,SMTP,PUERTO,MAIL,CLAVE,PARA,TITULO,CUERPO,ultimafechatrabajo,fechatrabajoactual,redondeo,backupearsiemprealcerrardia,nopermitircambiosendiasanteriores,siempreimprimirtickets,tooltipsON", "@saldoini,@fia,@nomb,@dire,@mail,@telA,@loca,@cuite,@usaimpfi,@puerto,@masteruser,@smtp,@puertomail,@mmail,@mclave,@mpara,@mtitulo,@mcuerpo,@uft,@fta,@redondo,@backupearsiempre,@modoreadonly,@alwaysp,@tooltipz", f);
                    Conexion.Insertar("Usuarios", "login,clave,level,nombreusuario,eliminado", "'q','q','Admin','Administrador','Activo'", new SqlCeCommand());
                    Conexion.cerrar();
                    MessageBox.Show("se creo la licencia, tu usuario Admin es\nUser: q\nPassw: q\n Si olvidas la clave contactanos por favor.\nVuelva a ejecutar el sistema para poder usar su licencia.\nGracias por habernos elegido!");
                    Environment.Exit(0);
                }
            }
            catch (Exception exce)
            {
                MessageBox.Show(exce.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("probando conectar al servidor sql");
            SqlConnection myConnection = new SqlConnection("user id=sa;" +
                                       "password=JULIAN2018flag!;server=WIN-65EONEJMKT1;" +
                                       "Trusted_Connection=yes;" +
                                       "database=flaginformaticasoftware; " +
                                       "connection timeout=30");
            myConnection.Open();
            myConnection.Close();

        }
    }
}
