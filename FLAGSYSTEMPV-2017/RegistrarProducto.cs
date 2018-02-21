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
            string cliente, organizacion, telefono, mail, cuit, tipocuit, serial, claveserial,fechaini,detallepc;
            MessageBox.Show("probando conectar al servidor sql");
            SqlConnection myConnection = new SqlConnection("user id=sa;" + "password=FlagInformatica2018;server=192.168.0.23\\FLAGSYSTEMPV;" +" database=UsuariosSoftware; " + "connection timeout=30");
            myConnection.Open();
            SqlCommand command = new SqlCommand("Select * from [licencias] WHERE serial = @ser and claveserial = @cla and serialconsumido = 'no'", myConnection);
            command.Parameters.AddWithValue("ser", textBox1.Text);
            command.Parameters.AddWithValue("cla", textBox3.Text);
            // int result = command.ExecuteNonQuery();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    MessageBox.Show(String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", reader[0], reader[1], reader[2], reader[3], reader[4], reader[5], reader[6], reader[7], reader[8], reader[9], reader[10], reader[11]));
                    cliente = reader[1].ToString();
                    organizacion = reader[2].ToString();
                    telefono = reader[3].ToString();
                    mail = reader[4].ToString();
                    cuit = reader[5].ToString();
                    tipocuit = reader[6].ToString();
                    serial = reader[7].ToString();
                    claveserial = reader[8].ToString();
                    fechaini = reader[10].ToString();
                    detallepc = reader[11].ToString();
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
                else
                {
                    MessageBox.Show("La Licencia es invalida o ya fue activada previamente. Si es usted el dueño/la dueña de la licencia ingresada entonces comuníquese a nuestras oficinas para resolver el inconveniente.");
                }
            }
            myConnection.Close();
        }
    }
}
