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
        private void Inicio_Load(object sender, EventArgs e)
        {
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
                button6.Visible = false;
                button6.Enabled = false;
                if (registereduser.level == "Supervisor" || registereduser.level == "Admin")
                {
                    toolStripMenuItem11.Enabled = true;
                    toolStripMenuItem15.Enabled = true;
                    toolStripMenuItem18.Enabled = true;
                    altasYBajasDeStockToolStripMenuItem.Enabled = true;
                   // diferenciaStockPorArtículoToolStripMenuItem.Enabled = true;
                    gestionarUsuariosToolStripMenuItem.Enabled = true;
                    configuraciónToolStripMenuItem.Enabled = true;
                    enviarInformeToolStripMenuItem.Enabled = true;
                    //mantenimientoToolStripMenuItem.Enabled = true;
                }
                else
                {
                    toolStripMenuItem11.Enabled = false;
                    toolStripMenuItem15.Enabled = false;
                    toolStripMenuItem18.Enabled = false;
                    altasYBajasDeStockToolStripMenuItem.Enabled = false;
                    
                    //diferenciaStockPorArtículoToolStripMenuItem.Enabled = false;
                    gestionarUsuariosToolStripMenuItem.Enabled = false;
                    configuraciónToolStripMenuItem.Enabled = false;
                    enviarInformeToolStripMenuItem.Enabled = false;
                   // mantenimientoToolStripMenuItem.Enabled = false;
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
                    Conexion.Actualizar("Turnos", "FechaFin = @ff", "WHERE idturno = @id", "", cierro);
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
                if (Application.OpenForms.OfType<Ventas>().Count() == 1)
                    Application.OpenForms.OfType<Ventas>().First().Focus();
                else
                {
                    Ventas frm = new Ventas();
                    frm.Show();
                }
            }
            if (e.KeyCode == Keys.F2)
            {
                if (Application.OpenForms.OfType<Compras>().Count() == 1)
                    Application.OpenForms.OfType<Compras>().First().Focus();
                else
                {
                    Compras frm = new Compras();
                    frm.Show();
                }
            }

            if (e.KeyCode == Keys.F3)
            {
                if (Application.OpenForms.OfType<Articulos>().Count() == 1)
                    Application.OpenForms.OfType<Articulos>().First().Focus();
                else
                {
                    Articulos frm = new Articulos();
                    frm.Show();
                }
            }
            if (e.KeyCode == Keys.F4)
            {
                if (Application.OpenForms.OfType<Caja>().Count() == 1)
                    Application.OpenForms.OfType<Caja>().First().Focus();
                else
                {
                    Caja frm = new Caja();
                    frm.Show();
                }
            }

            if (e.KeyCode == Keys.F5)
            {
                if (Application.OpenForms.OfType<Clientes>().Count() == 1)
                    Application.OpenForms.OfType<Clientes>().First().Focus();
                else
                {
                    Clientes frm = new Clientes();
                    frm.Show();
                }
            }


            if (e.KeyCode == Keys.F6)
            {
                if (Application.OpenForms.OfType<Proveedores>().Count() == 1)
                    Application.OpenForms.OfType<Proveedores>().First().Focus();
                else
                {
                    Proveedores frm = new Proveedores();
                    frm.Show();
                }
            }

            if (e.KeyCode == Keys.F7)
            {
                if (Application.OpenForms.OfType<Gastos>().Count() == 1)
                    Application.OpenForms.OfType<Gastos>().First().Focus();
                else
                {
                    Gastos frm = new Gastos();
                    frm.Show();
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
            cierro.Parameters.AddWithValue("ff", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            Conexion.abrir();
            Conexion.Actualizar("Turnos", "FechaFin = @ff", "WHERE idturno = @id", "", cierro);
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
               
                frm.ShowDialog();
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
                Conexion.data = "Compras";
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
    }
}
