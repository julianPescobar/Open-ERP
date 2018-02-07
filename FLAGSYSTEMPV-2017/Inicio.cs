using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;
using EPSON_Impresora_Fiscal;
namespace FLAGSYSTEMPV_2017
{
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }
        public bool b1wasclicked = false;
        public bool b2wasclicked = false;
        
        public static void generarResumenFinal()
        {
            Conexion.abrir();
            SqlCeCommand hoy = new SqlCeCommand();
            hoy.Parameters.AddWithValue("hoy", app.hoy + " 00:00:00");
            hoy.Parameters.AddWithValue("hoy2", app.hoy + " 23:59:59");
            DataTable ventas = Conexion.Consultar("nfactura,vendedor,total", "Ventas", "Where estadoVenta != 'Anulada' and fechaventa between @hoy and @hoy2", "", hoy);
            DataTable compras = Conexion.Consultar("nfactura,vendedor,CAST(totalfactura as FLOAT)", "Compras", "Where fechacompra between @hoy and @hoy2", "", hoy);
            DataTable gastos = Conexion.Consultar("area,descripcion,importe", "Gastos", "Where fecha between @hoy and @hoy2", "", hoy);
            DataTable entradas = Conexion.Consultar("tipo,motivo,total", "EntradaCaja", "Where fecha between @hoy and @hoy2", "", hoy);
            DataTable salidas = Conexion.Consultar("tipo,motivo,total", "SalidaCaja", "Where fecha between @hoy and @hoy2", "", hoy);
            string[] Ventas = new string[ventas.Rows.Count];
             string[] Compras = new string[compras.Rows.Count];
             string[] Gastos = new string[gastos.Rows.Count];
             string[] Entradas = new string[entradas.Rows.Count];
             string[] Salidas = new string[salidas.Rows.Count];
            float tv = 0;
                float tc = 0;
                    float tg = 0;
                        float te = 0;
                            float ts = 0;

                            if (ventas.Rows.Count > 0)
                            {
                                for(int i = 0; i < ventas.Rows.Count ; i++)
                                {
                                tv += float.Parse(ventas.Rows[i][2].ToString());
                                Ventas[i] = ventas.Rows[i][0].ToString() + "\t" + ventas.Rows[i][1].ToString() + "\t" + tv.ToString("$0.00");
                                }
                            }
                            else tv = 0;

                            if (compras.Rows.Count > 0 )
                            {
                                for(int i = 0; i < compras.Rows.Count ; i++)
                                {
                                tc += float.Parse(compras.Rows[i][2].ToString());
                                Compras[i] = compras.Rows[i][0].ToString() + "\t" + compras.Rows[i][1].ToString() + "\t" + tc.ToString("$0.00") ;
                                }
                            }
                            else tc = 0;

                            if (gastos.Rows.Count > 0 )
                            {
                                for(int i = 0; i < gastos.Rows.Count ; i++)
                                {
                                tg += float.Parse(gastos.Rows[i][2].ToString());
                                Gastos[i] = gastos.Rows[i][0].ToString() + "\t" + gastos.Rows[i][1].ToString() + "\t" + tg.ToString("$0.00") ;
                                }
                            }
                            else tg = 0;

                            if (entradas.Rows.Count > 0 )
                            {
                                for(int i = 0; i < entradas.Rows.Count ; i++)
                                {
                                te += float.Parse(entradas.Rows[i][2].ToString());
                                Entradas[i] = entradas.Rows[i][0].ToString() +"\t"+ entradas.Rows[i][1].ToString() +"\t"+ te.ToString("$0.00");
                                }
                            }
                            else te = 0;

                            if (salidas.Rows.Count > 0 )
                            {
                                for(int i = 0; i < salidas.Rows.Count ; i++)
                                {
                                ts += float.Parse(salidas.Rows[i][2].ToString());
                                Salidas[i] = salidas.Rows[i][0].ToString() +"\t"+ salidas.Rows[i][1].ToString() +"\t"+ ts.ToString("$0.00");
                                }
                            }
                            else ts = 0;

                            File.WriteAllText(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", registereduser.registeredlicense + "\r\n" + "Informe de cierre del dia " + DateTime.Now.ToShortDateString() + "\r\nTotal Ventas:\t" + tv.ToString("$0.00") + "\r\nTotal Compras:\t" + tc.ToString("$0.00") + "\r\nTotal Gastos:\t" + tg.ToString("$0.00") + "\r\nTotal Entrada Caja:\t" + te.ToString("$0.00") + "\r\nTotal Salida Caja:\t" + ts.ToString("$0.00") + "\r\nTotal del Día:\t" + ((tv+tc+te)-(tg+ts)).ToString("$0.00"));
                            File.AppendAllText(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", "\r\n");
                            File.AppendAllText(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", "\r\nDetalle de Ventas:\r\nN° Venta\tVendedor\tTotal\t\r\n");
                            File.AppendAllLines(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", Ventas);
                            File.AppendAllText(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", "\r\nDetalle de Compras:\r\nN° Compra\tVendedor\tTotal\t\r\n");
                            File.AppendAllLines(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", Compras);
                            File.AppendAllText(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", "\r\nDetalle de Gastos:\r\nArea\tDescripcion\tImporte\t\r\n");
                            File.AppendAllLines(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", Gastos);
                            File.AppendAllText(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", "\r\nDetalle de Entradas de Caja:\r\nTipo\tMotivo\tTotal\t\r\n");
                            File.AppendAllLines(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", Entradas);
                            File.AppendAllText(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", "\r\nDetalle de Salidas de Caja:\r\nTipo\tMotivo\tTotal\t\r\n");
                            File.AppendAllLines(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", Salidas);
                           
        }
        
        private void Inicio_Load(object sender, EventArgs e)
        {
            label4.Text = "Fecha de Trabajo: " + app.hoy;
            label2.BringToFront();
            label5.BringToFront();
            button6.BringToFront();
            //DEBUG CARGO CONFIG IMPRE FISCAL: MessageBox.Show(ConfigFiscal.comport.ToString() + "\n" + ConfigFiscal.marca.ToString() + "\n" + ConfigFiscal.modelo.ToString() + "\n");
            if (Demo.EsDemo == true)
            {
                label3.Text = Demo.demolicense;
                label2.Text = "Conectado como: Usuario Demo";
                label5.Text = "Jerarquía : Operador";
                toolStripMenuItem11.Enabled = false;
                toolStripMenuItem15.Enabled = false;
                toolStripMenuItem18.Enabled = false;
                altasYBajasDeStockToolStripMenuItem.Enabled = false;
                // diferenciaStockPorArtículoToolStripMenuItem.Enabled = false;
                gestionarUsuariosToolStripMenuItem.Enabled = false;
                configuraciónToolStripMenuItem.Enabled = false;
                enviarInformeToolStripMenuItem.Enabled = false;
                // mantenimientoToolStripMenuItem.Enabled = false;
                button6.Visible = true;
                button6.Enabled = true;
                toolStripButton1.Visible = false;
            }
            else
            {
                toolStripButton1.Text = "Cerrar Turno de " + registereduser.reguser;
                label3.Text = "Registrado por: " + registereduser.getRegLicense();
                label2.Text = "Conectado como: " + registereduser.reguser;
                label5.Text = "Jerarquía: " + registereduser.level;
                button6.Visible = false; //el boton de comprar licencia lo sacamos
                button6.Enabled = false; //y lo desactivamos
                if (registereduser.level == "Supervisor" || registereduser.level == "Admin")
                {
                    toolStripMenuItem11.Enabled = true;
                    toolStripMenuItem15.Enabled = true;
                    toolStripMenuItem18.Enabled = true;
                    altasYBajasDeStockToolStripMenuItem.Enabled = true;
                    gestionarUsuariosToolStripMenuItem.Enabled = true;
                    configuraciónToolStripMenuItem.Enabled = true;
                    enviarInformeToolStripMenuItem.Enabled = true;
                    
                }
                else
                {
                    //toolStripMenuItem11.Enabled = false;
                    //toolStripMenuItem15.Enabled = false;
                    //toolStripMenuItem18.Enabled = false;
                    //altasYBajasDeStockToolStripMenuItem.Enabled = false;
                    //gestionarUsuariosToolStripMenuItem.Enabled = false;
                    //configuraciónToolStripMenuItem.Enabled = false;
                    //enviarInformeToolStripMenuItem.Enabled = false;
                    setearPermisos();
                  
                }
            }
        }

        private void Inicio_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                            this.DisplayRectangle);              
        }

        private void button1_Click(object sender, EventArgs e)
        {
            b1wasclicked = true;
            DialogResult salir = MessageBox.Show("Está seguro de salir del sistema?", "Salir?", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (salir == DialogResult.Yes)
            {
                try
                {
                    Conexion.abrir();
                    DataTable turnos = Conexion.Consultar("*", "Turnos", "", "", new SqlCeCommand());
                    Conexion.cerrar();
                    SqlCeCommand cierro = new SqlCeCommand();
                    cierro.Parameters.AddWithValue("id", turnos.Rows[turnos.Rows.Count - 1][0].ToString());
                    cierro.Parameters.AddWithValue("ff", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                    Conexion.abrir();
                    string total;
                    SqlCeCommand paraeltotal = new SqlCeCommand();
                    paraeltotal.Parameters.Add("ven", registereduser.reguser);
                    paraeltotal.Parameters.Add("fecA", app.hoy + " 00:00:00");
                    paraeltotal.Parameters.Add("fecB", app.hoy + " " + DateTime.Now.ToShortTimeString());
                    DataTable totalvendido = Conexion.Consultar("SUM(total)", "Ventas", "Where vendedor = @ven and estadoventa = 'Finalizado' and fechaventa between @fecA and @fecB", "", paraeltotal);
                    if (totalvendido.Rows.Count > 0)
                    {
                        total = totalvendido.Rows[0][0].ToString();
                    }
                    else
                        total = "0";
                    cierro.Parameters.AddWithValue("to", total);
                    Conexion.Actualizar("Turnos", "FechaFin = @ff, TotalVendido = @to", "WHERE idturno = @id", "", cierro);
                    Conexion.cerrar();
                    Application.Exit();
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
        }

        private void toolStrip1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                           this.DisplayRectangle);      
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.flag.com.ar");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto://info@flag.com.ar");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            RegistrarProducto reg = new RegistrarProducto();
            reg.ShowDialog();
        }

        private void toolStripSplitButton8_ButtonClick(object sender, EventArgs e)
        {
            if (toolStripSplitButton8.IsOnDropDown == false)
                toolStripSplitButton8.ShowDropDown();
        }

       

        

        private void toolStripSplitButton5_ButtonClick(object sender, EventArgs e)
        {
            if (toolStripSplitButton5.IsOnDropDown == false)
                toolStripSplitButton5.ShowDropDown();
        }

        private void toolStripSplitButton4_ButtonClick(object sender, EventArgs e)
        {
            if (toolStripSplitButton4.IsOnDropDown == false)
                toolStripSplitButton4.ShowDropDown();
        }

        private void toolStripSplitButton3_ButtonClick(object sender, EventArgs e)
        {
            if (toolStripSplitButton3.IsOnDropDown == false)
                toolStripSplitButton3.ShowDropDown();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            if (toolStripSplitButton1.IsOnDropDown == false)
                toolStripSplitButton1.ShowDropDown();
        }

        private void toolStripSplitButton10_ButtonClick(object sender, EventArgs e)
        {
            if (toolStripSplitButton10.IsOnDropDown == false)
                toolStripSplitButton10.ShowDropDown();
        }

        private void toolStripSplitButton9_ButtonClick(object sender, EventArgs e)
        {
            if (toolStripSplitButton9.IsOnDropDown == false)
                toolStripSplitButton9.ShowDropDown();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Application.OpenForms.OfType<Ventas>().Count() == 1)
                    Application.OpenForms.OfType<Ventas>().First().Focus();
                else
                {
                    Ventas frm = new Ventas();
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem22_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + "\\faqs.html");
            }
            catch (Exception err) { MessageBox.Show(err.Message); }
        }

        private void toolStripMenuItem23_Click(object sender, EventArgs e)
        {
             try
            {
            System.Diagnostics.Process.Start("www.flag.com.ar");
            }
             catch (Exception err) { MessageBox.Show(err.Message); }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Ventas>().Count() == 1)
                Application.OpenForms.OfType<Ventas>().First().Focus();
            else
            {
                Ventas frm = new Ventas();
                frm.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Articulos>().Count() == 1)
                Application.OpenForms.OfType<Articulos>().First().Focus();
            else
            {
                Articulos frm = new Articulos();
                frm.Show();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Clientes>().Count() == 1)
                Application.OpenForms.OfType<Clientes>().First().Focus();
            else
            {
                Clientes frm = new Clientes();
                frm.Show();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Proveedores>().Count() == 1)
                Application.OpenForms.OfType<Proveedores>().First().Focus();
            else
            {
                Proveedores frm = new Proveedores();
                frm.Show();
            }
        }

        private void Inicio_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (b1wasclicked == true && b2wasclicked == false)
            {
                DialogResult salir = MessageBox.Show("Cerrar el sistema?", "Cerrar?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (salir == DialogResult.Yes)
                {
                    Conexion.abrir();
                    DataTable turnos = Conexion.Consultar("*", "Turnos", "", "", new SqlCeCommand());
                    Conexion.cerrar();


                    SqlCeCommand cierro = new SqlCeCommand();
                    cierro.Parameters.AddWithValue("id", turnos.Rows[turnos.Rows.Count - 1][0].ToString());
                    cierro.Parameters.AddWithValue("ff", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                    Conexion.abrir();
                    Conexion.Actualizar("Turnos", "FechaFin = @ff", "WHERE idturno = @id", "", cierro);
                    Conexion.cerrar();
                    e.Cancel = false;
                    Application.Exit();
                }
            }
            if(b1wasclicked == false && b2wasclicked == true)
            {
                DialogResult salirg = MessageBox.Show("Cerrar el sistema?", "Cerrar?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (salirg == DialogResult.Yes)
                {
                    Conexion.abrir();
                    DataTable turnos = Conexion.Consultar("*", "Turnos", "", "", new SqlCeCommand());
                    Conexion.cerrar();
                    SqlCeCommand cierro = new SqlCeCommand();
                    cierro.Parameters.AddWithValue("id", turnos.Rows[turnos.Rows.Count - 1][0].ToString());
                    cierro.Parameters.AddWithValue("ff", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                    Conexion.abrir();
                    Conexion.Actualizar("Turnos", "FechaFin = @ff", "WHERE idturno = @id", "", cierro);
                    Conexion.cerrar();
                    e.Cancel = false;
                    Application.Exit();
                }
                else
                    e.Cancel = true;
            }
            
        }

        private void Inicio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (button2.Enabled == true)
                {
                    if (Application.OpenForms.OfType<Ventas>().Count() == 1)
                        Application.OpenForms.OfType<Ventas>().First().Focus();
                    else
                    {
                        Ventas frm = new Ventas();
                        frm.Show();
                    }
                }
            }
            if (e.KeyCode == Keys.F2)
            {
                if (button3.Enabled == true)
                {
                    if (Application.OpenForms.OfType<Compras>().Count() == 1)
                        Application.OpenForms.OfType<Compras>().First().Focus();
                    else
                    {
                        Compras frm = new Compras();
                        frm.Show();
                    }
                }
            }

            if (e.KeyCode == Keys.F3)
            {
                if (button4.Enabled == true)
                {
                    if (Application.OpenForms.OfType<Articulos>().Count() == 1)
                        Application.OpenForms.OfType<Articulos>().First().Focus();
                    else
                    {
                        Articulos frm = new Articulos();
                        frm.Show();
                    }
                }
            }
            if (e.KeyCode == Keys.F4)
            {
                if (button5.Enabled == true)
                {
                    if (Application.OpenForms.OfType<Caja>().Count() == 1)
                        Application.OpenForms.OfType<Caja>().First().Focus();
                    else
                    {
                        Caja frm = new Caja();
                        frm.Show();
                    }
                }
            }

            if (e.KeyCode == Keys.F5)
            {
                if (button9.Enabled == true)
                {
                    if (Application.OpenForms.OfType<Clientes>().Count() == 1)
                        Application.OpenForms.OfType<Clientes>().First().Focus();
                    else
                    {
                        Clientes frm = new Clientes();
                        frm.Show();
                    }
                }
            }


            if (e.KeyCode == Keys.F6)
            {
                if (button8.Enabled == true)
                {
                    if (Application.OpenForms.OfType<Proveedores>().Count() == 1)
                        Application.OpenForms.OfType<Proveedores>().First().Focus();
                    else
                    {
                        Proveedores frm = new Proveedores();
                        frm.Show();
                    }
                }
            }

            if (e.KeyCode == Keys.F7)
            {
                if (button7.Enabled == true)
                {
                    if (Application.OpenForms.OfType<Gastos>().Count() == 1)
                        Application.OpenForms.OfType<Gastos>().First().Focus();
                    else
                    {
                        Gastos frm = new Gastos();
                        frm.Show();
                    }
                }
            }
            if (e.KeyCode == Keys.F8)
            {
               button1.PerformClick();
            }


        }

        private void impresoraFiscalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ConfigImpresora>().Count() == 1)
                Application.OpenForms.OfType<ConfigImpresora>().First().Focus();
            else
            {
                ConfigImpresora frm = new ConfigImpresora();
                frm.Show();
            }
        }

        private void toolStripMenuItem19_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Articulos>().Count() == 1)
                Application.OpenForms.OfType<Articulos>().First().Focus();
            else
            {
                Articulos frm = new Articulos();
                frm.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Caja>().Count() == 1)
                Application.OpenForms.OfType<Caja>().First().Focus();
            else
            {
                Caja frm = new Caja();
                frm.Show();
            }
        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Clientes>().Count() == 1)
                Application.OpenForms.OfType<Clientes>().First().Focus();
            else
            {
                Clientes frm = new Clientes();
                frm.Show();
            }
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Proveedores>().Count() == 1)
                Application.OpenForms.OfType<Proveedores>().First().Focus();
            else
            {
                Proveedores frm = new Proveedores();
                frm.Show();
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Caja>().Count() == 1)
                Application.OpenForms.OfType<Caja>().First().Focus();
            else
            {
                Caja frm = new Caja();
                frm.Show();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Gastos>().Count() == 1)
                Application.OpenForms.OfType<Gastos>().First().Focus();
            else
            {
                Gastos frm = new Gastos();
                frm.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Compras>().Count() == 1)
                Application.OpenForms.OfType<Compras>().First().Focus();
            else
            {
                Compras frm = new Compras();
                frm.Show();
            }
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Compras>().Count() == 1)
                Application.OpenForms.OfType<Compras>().First().Focus();
            else
            {
                Compras frm = new Compras();
                frm.Show();
            }
        }

        private void cargaModificaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Informe>().Count() == 1)
                Application.OpenForms.OfType<Informe>().First().Focus();
            else
            {
                Informe frm = new Informe();
                Conexion.data = "Ventas";
                frm.Show();
            }
        }

        private void artículosVendidosDelDíaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Informe>().Count() == 1)
                Application.OpenForms.OfType<Informe>().First().Focus();
            else
            {
                Informe frm = new Informe();
                Conexion.data = "Articulos";
                frm.Show();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           // if (Application.OpenForms.OfType<Inicio>().Count() == 1)
             //   Application.OpenForms.OfType<Inicio>().First().Close();
          /* EPSON_Impresora_Fiscal.PrinterFiscal epson = new PrinterFiscal();
                            epson.PortNumber = ConfigFiscal.comport;
                            epson.BaudRate = "9600";
                            epson.MessagesOn = true;
                            epson.SetGetDateTime("S", "200120", "100505");

           
           
            epson.CloseJournal("Z", "P");*/
            Conexion.abrir();
            DataTable turnos = Conexion.Consultar("*", "Turnos", "", "", new SqlCeCommand());
            Conexion.cerrar();
            SqlCeCommand cierro = new SqlCeCommand();
            cierro.Parameters.AddWithValue("id", turnos.Rows[turnos.Rows.Count - 1][0].ToString());
            cierro.Parameters.AddWithValue("ff", app.hoy + " " + DateTime.Now.ToShortTimeString());
            string total = "";
            Conexion.abrir();
            SqlCeCommand paraeltotal = new SqlCeCommand();
            paraeltotal.Parameters.Add("ven",registereduser.reguser);
            paraeltotal.Parameters.Add("fecA",app.hoy+" 00:00:00");
            paraeltotal.Parameters.Add("fecB",app.hoy + " " + DateTime.Now.ToShortTimeString());
            DataTable totalvendido = Conexion.Consultar("SUM(total)", "Ventas", "Where vendedor = @ven and estadoventa = 'Finalizado' and fechaventa between @fecA and @fecB", "", paraeltotal);
            if (totalvendido.Rows.Count > 0)
            {
                total = totalvendido.Rows[0][0].ToString();
            }
            else
                total = "0";
            cierro.Parameters.AddWithValue("to",total);
            Conexion.Actualizar("Turnos", "FechaFin = @ff, TotalVendido = @to", "WHERE idturno = @id", "", cierro);
            Conexion.cerrar();
            
            Login lgn = new Login();
            lgn.Show();
            this.Close();
        }

        private void toolStripMenuItem24_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sistema para manejo y gestión de comercios, puntos de venta, kioscos, maxikioscos, supermercados, almacenes, locutorios, etc.\n\nAdvertencia: Este programa esta protegido por las leyes de derechos de autor y otros tratados internacionales. La distribucion o reproduccion ilicitas de este programa o de cualquier parte del mismo esta penada por la ley con severas sanciones civiles y penales y sera objeto de todas las acciones judiciales que correspondan.\nCOPYRIGHT 1216 ISBN 30092008280129-1216", "Acerca de Flag System PV Punto de Venta",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<DiferenciaStock>().Count() == 1)
                Application.OpenForms.OfType<DiferenciaStock>().First().Focus();
            else
            {
                DiferenciaStock frm = new DiferenciaStock();
                frm.Show();
            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Consultas>().Count() == 1)
                Application.OpenForms.OfType<Consultas>().First().Focus();
            else
            {
                Conexion.data = "Ventas";
                Consultas frm = new Consultas();
                frm.Show();
            }
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Consultas>().Count() == 1)
                Application.OpenForms.OfType<Consultas>().First().Focus();
            else
            {
                Conexion.data = "Compras";
                Consultas frm = new Consultas();
                frm.Show();
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<CajaIO>().Count() == 1)
                Application.OpenForms.OfType<CajaIO>().First().Focus();
            else
            {
                CIO.entradaosalida = "Entrada";
                CajaIO frm = new CajaIO();
                frm.Show();
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<CajaIO>().Count() == 1)
                Application.OpenForms.OfType<CajaIO>().First().Focus();
            else
            {
                CIO.entradaosalida = "Salida";
                CajaIO frm = new CajaIO();
                frm.Show();
            }
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {

            if (Application.OpenForms.OfType<Ventaturnos>().Count() == 1)
                Application.OpenForms.OfType<Ventaturnos>().First().Focus();
            else
            {
                Ventaturnos frm = new Ventaturnos();
                frm.Show();
            }
        }

        private void diferenciasStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Informe>().Count() == 1)
                Application.OpenForms.OfType<Informe>().First().Focus();
            else
            {
                Informe frm = new Informe();
                Conexion.data = "Clientes";
                frm.Show();
            }
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Informe>().Count() == 1)
                Application.OpenForms.OfType<Informe>().First().Focus();
            else
            {
                Informe frm = new Informe();
                Conexion.data = "Proveedores";
                frm.Show();
            }
        }

        private void comprasDelDíaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Informe>().Count() == 1)
                Application.OpenForms.OfType<Informe>().First().Focus();
            else
            {
                Informe frm = new Informe();
                Conexion.data = "ControlStock";
                frm.Show();
            }
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Anular>().Count() == 1)
                Application.OpenForms.OfType<Anular>().First().Focus();
            else
            {
                Anular frm = new Anular();
               
                frm.Show();
            }
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<NotaDeCredito>().Count() == 1)
                Application.OpenForms.OfType<NotaDeCredito>().First().Focus();
            else
            {
                NotaDeCredito frm = new NotaDeCredito();

                frm.Show();
            }
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<NotaDeDebito>().Count() == 1)
                Application.OpenForms.OfType<NotaDeDebito>().First().Focus();
            else
            {
                NotaDeDebito frm = new NotaDeDebito();
                frm.Show();
            }
        }

        private void gestionarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<CrearEmpleados>().Count() == 1)
                Application.OpenForms.OfType<CrearEmpleados>().First().Focus();
            else
            {
                CrearEmpleados frm = new CrearEmpleados();
                frm.Show();
            }
        }

        private void altasYBajasDeStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ABStock>().Count() == 1)
                Application.OpenForms.OfType<ABStock>().First().Focus();
            else
            {
                ABStock frm = new ABStock();
                frm.Show();
            }
        }

        private void configuraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Config>().Count() == 1)
                Application.OpenForms.OfType<Config>().First().Focus();
            else
            {
                Config frm = new Config();
                frm.Show();
            }
        }

        private void pedidosAProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Informe>().Count() == 1)
                Application.OpenForms.OfType<Informe>().First().Focus();
            else
            {
                Informe frm = new Informe();
                Conexion.data = "Pedido";
                frm.Show();
            }
        }

        private void previsiónDeComprasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Informe>().Count() == 1)
                Application.OpenForms.OfType<Informe>().First().Focus();
            else
            {
                Informe frm = new Informe();
                Conexion.data = "AutoPedido";
                frm.Show();
            }
        }

        private void faltantesDeStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Informe>().Count() == 1)
                Application.OpenForms.OfType<Informe>().First().Focus();
            else
            {
                Informe frm = new Informe();
                Conexion.data = "Faltantes";
                frm.Show();
            }
        }

        private void enviarInformeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<EnviarMail>().Count() == 1)
                Application.OpenForms.OfType<EnviarMail>().First().Focus();
            else
            {
                EnviarMail frm = new EnviarMail();
                frm.Show();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            DialogResult continuar =  MessageBox.Show("Esta funcion genera el informe final, lo envia por email y cierra la aplicación para que se ingrese un nuevo día. Continuar?","Advertencia",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (continuar == DialogResult.Yes)
            {
                generarResumenFinal();
                Conexion.abrir();
                DataTable turnos = Conexion.Consultar("*", "Turnos", "", "", new SqlCeCommand());
                Conexion.cerrar();
                SqlCeCommand cierro = new SqlCeCommand();
                cierro.Parameters.AddWithValue("id", turnos.Rows[turnos.Rows.Count - 1][0].ToString());
                cierro.Parameters.AddWithValue("ff", app.hoy + " " + DateTime.Now.ToShortTimeString());
                Conexion.abrir();
                Conexion.Actualizar("Turnos", "FechaFin = @ff", "WHERE idturno = @id", "", cierro);
                Conexion.cerrar();

                Login lgn = new Login();
                lgn.Show();
                this.Close();
            }

        }

        private void setearPermisos()
        {
            if (registereduser.pventa == "si") button2.Enabled = true; else button2.Enabled = false;
            if (registereduser.pcaja == "si") button5.Enabled = true; else button5.Enabled = false;
            if (registereduser.pcompra == "si") button3.Enabled = true; else button3.Enabled = false;
            if (registereduser.particulo == "si") button4.Enabled = true; else button4.Enabled = false;
            if (registereduser.pclientes == "si") button9.Enabled = true; else button9.Enabled = false;
            if (registereduser.pproveedores == "si") button8.Enabled = true; else button8.Enabled = false;
            if (registereduser.pgastos == "si") button7.Enabled = true; else button7.Enabled = false;
            if (registereduser.pstock == "si") button12.Enabled = true; else button12.Enabled = false;
            if (registereduser.pcierredia == "si") button11.Enabled = true; else button11.Enabled = false;
            if (registereduser.pdiferencia == "si") toolStripMenuItem21.Enabled = true; else toolStripMenuItem21.Enabled = false;
            if (registereduser.pconsultaC == "si") toolStripMenuItem12.Enabled = true; else toolStripMenuItem12.Enabled = false;
            if (registereduser.pconsultaV == "si") toolStripMenuItem8.Enabled = true; else toolStripMenuItem8.Enabled = false;
            if (registereduser.pEScaja == "si") toolStripSplitButton3.Enabled = true; else toolStripSplitButton3.Enabled = false;
            if (registereduser.pinformes == "si") toolStripSplitButton1.Enabled = true; else toolStripSplitButton1.Enabled = false;
            if (registereduser.panular == "si") toolStripMenuItem11.Enabled = true; else toolStripMenuItem11.Enabled = false;
            if (registereduser.pnotac == "si") toolStripMenuItem15.Enabled = true; else toolStripMenuItem15.Enabled = false;
            if (registereduser.pnotad == "si") toolStripMenuItem18.Enabled = true; else toolStripMenuItem18.Enabled = false;
            if (registereduser.pabstock == "si") altasYBajasDeStockToolStripMenuItem.Enabled = true; else altasYBajasDeStockToolStripMenuItem.Enabled = false;
            if (registereduser.pconfig == "si") configuraciónToolStripMenuItem.Enabled = true; else configuraciónToolStripMenuItem.Enabled = false;
            if (registereduser.pempleados == "si") gestionarUsuariosToolStripMenuItem.Enabled = true; else gestionarUsuariosToolStripMenuItem.Enabled = false;
            if (registereduser.penviarinforme == "si") enviarInformeToolStripMenuItem.Enabled = true; else enviarInformeToolStripMenuItem.Enabled = false;
            if (registereduser.pfiscalconfig == "si") impresoraFiscalToolStripMenuItem.Enabled = true; else impresoraFiscalToolStripMenuItem.Enabled = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ControlStockVendedores>().Count() == 1)
                Application.OpenForms.OfType<ControlStockVendedores>().First().Focus();
            else
            {
                ControlStockVendedores frm = new ControlStockVendedores();
                
                frm.Show();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Rubros>().Count() == 1)
                Application.OpenForms.OfType<Rubros>().First().Focus();
            else
            {
                Rubros frm = new Rubros();

                frm.Show();
            }
        }

    }
}
