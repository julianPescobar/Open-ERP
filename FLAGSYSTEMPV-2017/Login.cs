using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;


namespace FLAGSYSTEMPV_2017
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Login_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                           this.DisplayRectangle);      
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            
            string usuario = textBox1.Text.ToString();
            string clave = textBox2.Text.ToString();
            string jerarquia = "";
            string nombre = "";
            
            Conexion.abrir();
            DataTable user = new DataTable();
            SqlCeCommand userypass = new SqlCeCommand();
            userypass.Parameters.Clear();
            userypass.Parameters.AddWithValue("@a", usuario);
            userypass.Parameters.AddWithValue("@b", clave);
            userypass.Parameters.AddWithValue("elim", "Eliminado");
            user = Conexion.Consultar("login,clave,level,nombreusuario, eliminado, p_venta, p_compra,p_articulo,p_clientes,p_proveedores,p_gastos,p_stock,p_cierredia,p_diferencia,p_consultaC,p_consultaV,p_EScaja,p_informes,p_anular,p_notac,p_notad,p_abstock,p_config,p_empleados,p_enviarinforme,p_fiscalconfig,p_caja", "Usuarios", "WHERE login = @a AND clave = @b AND eliminado !=  @elim", "", userypass);
            DataTable turnos = Conexion.Consultar("*", "Turnos", "", "", new SqlCeCommand());
            Conexion.cerrar();
            if (user.Rows.Count > 0)
            {
                usuario = user.Rows[0][0].ToString();
                clave = user.Rows[0][1].ToString();
                jerarquia = user.Rows[0][2].ToString();
                nombre = user.Rows[0][3].ToString();
                registereduser.pventa = user.Rows[0][5].ToString();
                registereduser.pcompra = user.Rows[0][6].ToString();
                registereduser.particulo = user.Rows[0][7].ToString();
                registereduser.pclientes = user.Rows[0][8].ToString();
                registereduser.pproveedores = user.Rows[0][9].ToString();
                registereduser.pgastos = user.Rows[0][10].ToString();
                registereduser.pstock = user.Rows[0][11].ToString();
                registereduser.pcierredia = user.Rows[0][12].ToString();
                registereduser.pdiferencia = user.Rows[0][13].ToString();
                registereduser.pconsultaC = user.Rows[0][14].ToString();
                registereduser.pconsultaV = user.Rows[0][15].ToString();
                registereduser.pEScaja = user.Rows[0][16].ToString();
                registereduser.pinformes = user.Rows[0][17].ToString();
                registereduser.panular = user.Rows[0][18].ToString();
                registereduser.pnotac = user.Rows[0][19].ToString();
                registereduser.pnotad = user.Rows[0][20].ToString();
                registereduser.pabstock = user.Rows[0][21].ToString();
                registereduser.pconfig = user.Rows[0][22].ToString();
                registereduser.pempleados = user.Rows[0][23].ToString();
                registereduser.penviarinforme = user.Rows[0][24].ToString();
                registereduser.pfiscalconfig= user.Rows[0][25].ToString();
                registereduser.pcaja = user.Rows[0][26].ToString();
                if (turnos.Rows.Count > 0 && turnos.Rows[turnos.Rows.Count - 1][3].ToString() == "")
                {
                    if (turnos.Rows[turnos.Rows.Count - 1][1].ToString() == nombre)
                    {
                        DialogResult cerraryo = MessageBox.Show("No has cerrado correctamente tu turno anterior, desea cerrarlo?", "El turno anterior no fue cerrado correctamente", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (cerraryo == DialogResult.Yes)
                        {

                            closeturno(turnos.Rows[turnos.Rows.Count - 1][0].ToString());
                            addturno(nombre);
                            registereduser.reguser = nombre;
                            registereduser.level = jerarquia;
                            this.Close();
                            IngreseFecha fecha = new IngreseFecha();
                            fecha.ShowDialog();
                          //  Inicio inicio = new Inicio();
                           // inicio.Show();

                        }
                        if (cerraryo == DialogResult.No)
                        {

                            registereduser.reguser = nombre;
                            registereduser.level = jerarquia;
                            IngreseFecha fecha = new IngreseFecha();
                            fecha.ShowDialog();
                          //  Inicio inicio = new Inicio();
                           // inicio.Show();
                            this.Close();

                        }
                    }
                    else
                    {
                        DialogResult cerrar = MessageBox.Show("El usuario del turno anterior (" + turnos.Rows[turnos.Rows.Count - 1][1].ToString() + ") no cerró bien su turno, desea cerrarlo por el/ella?", "El turno anterior no fue cerrado correctamente", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        
                        if (cerrar == DialogResult.Yes)
                        {
                            closeturno(turnos.Rows[turnos.Rows.Count - 1][0].ToString());
                            addturno(nombre);
                           
                            registereduser.reguser = nombre;
                            registereduser.level = jerarquia;
                            this.Close();
                            IngreseFecha fecha = new IngreseFecha();
                            fecha.ShowDialog();
                            //Inicio inicio = new Inicio();
                            //inicio.Show();
                           

                        }
                        if (cerrar == DialogResult.No)
                        {
                            this.Close();
                            Login logagain = new Login();
                            logagain.ShowDialog();
                        }
                    }
                }

                if (turnos.Rows.Count == 0 || (turnos.Rows.Count > 0 && turnos.Rows[turnos.Rows.Count - 1][3].ToString() != ""))
                {
                    addturno(nombre);
                    registereduser.reguser = nombre;
                    registereduser.level = jerarquia;
                    this.Close();
                    IngreseFecha fecha = new IngreseFecha();
                    fecha.ShowDialog();
                   // Inicio inicio = new Inicio();
                    //inicio.Show();
                    
                }

            }
            else
            {
                MessageBox.Show("Usuario incorrecto", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void addturno(string user)
        {
            SqlCeCommand usr = new SqlCeCommand();
            usr.Parameters.AddWithValue("u", user);
            usr.Parameters.AddWithValue("fi", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            usr.Parameters.AddWithValue("tot", "0");

            Conexion.abrir();
            Conexion.Insertar("Turnos", "Usuario,FechaInicio,TotalVendido", "@u,@fi,@tot", usr);
            Conexion.cerrar();
        }
        private void closeturno(string id)
        {
            SqlCeCommand cierro = new SqlCeCommand();
            cierro.Parameters.AddWithValue("id", id);
            cierro.Parameters.AddWithValue("ff", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            Conexion.abrir();
            Conexion.Actualizar("Turnos", "FechaFin = @ff", "WHERE idturno = @id", "", cierro);
            Conexion.cerrar();
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void Login_Paint_1(object sender, PaintEventArgs e)
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


       
    }
}
