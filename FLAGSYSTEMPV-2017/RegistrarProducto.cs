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
        public static string id;
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
            MessageBox.Show("La licencia se ha activado correctamente\nRecuerde que esta licencia tiene validez para 3 PCs. En caso de rotura de una de sus PCs contáctese con servicio técnico para reestablecer su licencia", "La licencia se ha activado con exito!");
        }

        private void RegistrarProducto_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.flag.com.ar");
        }
        public  string cliente, organizacion, telefono, mail, cuit, tipocuit, serial, claveserial, fechaini, detallepc;

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && textBox3.Text.Length > 0)
            {
                try
                {
                    SqlConnection myConnection = new SqlConnection("user id=sa;" + "password=FlagInformatica2018;server=192.168.0.23\\FLAGSYSTEMPV;" + " database=UsuariosSoftware; " + "connection timeout=30");
                    myConnection.Open();
                    SqlCommand command = new SqlCommand("Select * from [licencias] WHERE serial = @ser and claveserial = @cla and serialconsumido = 'no'", myConnection);
                    command.Parameters.AddWithValue("ser", textBox1.Text);
                    command.Parameters.AddWithValue("cla", textBox3.Text);
                    bool existe = false;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            existe = true;
                            cliente = reader[1].ToString();
                            organizacion = reader[2].ToString();
                            telefono = reader[3].ToString();
                            mail = reader[4].ToString();
                            cuit = reader[5].ToString();
                            tipocuit = reader[6].ToString();
                            serial = reader[7].ToString();
                            claveserial = reader[8].ToString();
                           // fechaini = reader[10].ToString();
                           // detallepc = reader[11].ToString();
                        }
                        else
                        {
                            MessageBox.Show("La Licencia es invalida o ya fue activada previamente. Si es usted el dueño/la dueña de la licencia ingresada entonces comuníquese a nuestras oficinas para resolver el inconveniente.");
                            Environment.Exit(1);
                        }
                        reader.Close();
                        myConnection.Close();
                    }
                    if (existe == true)
                    {
                        detallepc = id;
                        myConnection.Open();
                        Conexion.abrir();
                        DataTable haylicencia = Conexion.Consultar("*", "Configuracion", "", "", new SqlCeCommand());
                        Conexion.cerrar();
                        try
                        {
                            if (haylicencia.Rows.Count < 1)
                            {
                                SqlCeCommand f = new SqlCeCommand();
                                f.Parameters.AddWithValue("saldoini", "0");
                                f.Parameters.AddWithValue("fia", DateTime.Now.ToShortDateString()+" "+DateTime.Now.Hour.ToString()+":"+DateTime.Now.Minute.ToString()+":"+DateTime.Now.Second.ToString());
                                f.Parameters.AddWithValue("nomb", organizacion);
                                f.Parameters.AddWithValue("dire", " ");
                                f.Parameters.AddWithValue("mail", mail);
                                f.Parameters.AddWithValue("telA", telefono);
                                f.Parameters.AddWithValue("loca", " ");
                                f.Parameters.AddWithValue("cuite", cuit);
                                f.Parameters.AddWithValue("usaimpfi", "no");
                                f.Parameters.AddWithValue("puerto", "0");
                                f.Parameters.AddWithValue("masteruser", detallepc);
                                f.Parameters.AddWithValue("smtp", " ");
                                f.Parameters.AddWithValue("puertomail", " ");
                                f.Parameters.AddWithValue("mmail", " ");
                                f.Parameters.AddWithValue("mclave", " ");
                                f.Parameters.AddWithValue("mpara", " ");
                                f.Parameters.AddWithValue("mtitulo", " ");
                                f.Parameters.AddWithValue("mcuerpo", " ");
                                f.Parameters.AddWithValue("uft", DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString());
                                f.Parameters.AddWithValue("fta", DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString());
                                f.Parameters.AddWithValue("redondo", "0");
                                f.Parameters.AddWithValue("backupearsiempre", "no");
                                f.Parameters.AddWithValue("modoreadonly", "no");
                                f.Parameters.AddWithValue("alwaysp", "no");
                                f.Parameters.AddWithValue("tooltipz", "si");
                                Conexion.abrir();
                                Conexion.Insertar("Configuracion", "SaldoInicial,FechaInicioActividades,NombreEmpresa,DireccionFisica,Email,Telefono1,Localidad,CUIT,usaimpfiscal,PUERTO_IF,master_user_id,SMTP,PUERTO,MAIL,CLAVE,PARA,TITULO,CUERPO,ultimafechatrabajo,fechatrabajoactual,redondeo,backupearsiemprealcerrardia,nopermitircambiosendiasanteriores,siempreimprimirtickets,tooltipsON", "@saldoini,@fia,@nomb,@dire,@mail,@telA,@loca,@cuite,@usaimpfi,@puerto,@masteruser,@smtp,@puertomail,@mmail,@mclave,@mpara,@mtitulo,@mcuerpo,@uft,@fta,@redondo,@backupearsiempre,@modoreadonly,@alwaysp,@tooltipz", f);
                                Conexion.Insertar("Usuarios", "login,clave,level,nombreusuario,eliminado", "'admin','admin','Admin','Administrador','Activo'", new SqlCeCommand());
                                SqlCommand command2 = new SqlCommand("update licencias set serialconsumido = @consumido , fechainicializacion = @fechaini , detallePC = @infopc WHERE serial = @ser and claveserial = @cla", myConnection);
                                command2.Parameters.AddWithValue("ser", textBox1.Text);
                                command2.Parameters.AddWithValue("cla", textBox3.Text);
                                command2.Parameters.AddWithValue("consumido", "si");
                                command2.Parameters.AddWithValue("fechaini", DateTime.Now.ToShortDateString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString());
                                command2.Parameters.AddWithValue("infopc", id);
                                // int result = command.ExecuteNonQuery();
                                command2.ExecuteNonQuery();
                                Conexion.cerrar();
                                MessageBox.Show("La licencia se ha activado correctamente!, tu usuario Admin es\nUsuario: admin\nClave: admin\n Si olvidas la clave contactanos por favor.\nVuelva a ejecutar el sistema para poder usar su licencia.\nGracias por habernos elegido!", "LICENCIA ACTIVADA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Environment.Exit(0);
                            }
                        }
                        catch (Exception exce)
                        {
                            MessageBox.Show(exce.Message);
                        }
                        myConnection.Close();
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
            else { MessageBox.Show("Debe ingresar el serial y la clave serial obligatoriamente"); }
            }
       
    }
}
