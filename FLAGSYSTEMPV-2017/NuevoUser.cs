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
    public partial class NuevoUser : Form
    {
        public NuevoUser()
        {
            InitializeComponent();
        }

        private void NuevoUser_Load(object sender, EventArgs e)
        {

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

        private void NuevoUser_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                      this.DisplayRectangle);      
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && textBox3.Text.Length > 0 && comboBox1.SelectedIndex >= 0)
            {
                string userlogin = textBox1.Text;
                string userpassw = textBox2.Text;
                string username = textBox3.Text;
                string usrlevel = comboBox1.SelectedItem.ToString();

                Conexion.abrir();
                SqlCeCommand nu = new SqlCeCommand();
                nu.Parameters.AddWithValue("lo", userlogin);
                nu.Parameters.AddWithValue("cl", userpassw);
                nu.Parameters.AddWithValue("le", usrlevel);
                nu.Parameters.AddWithValue("no", username);
                nu.Parameters.AddWithValue("act", "Activo");
                Conexion.Insertar("Usuarios", "login,clave,level,nombreusuario,eliminado", "@lo,@cl,@le,@no,@act", nu);
                Conexion.cerrar();
                this.Close();
                if (Application.OpenForms.OfType<CrearEmpleados>().Count() == 1)
                    Application.OpenForms.OfType<CrearEmpleados>().First().Close();
               
                    CrearEmpleados frm = new CrearEmpleados();
                    frm.Show();
                
            }
            else
            {
                MessageBox.Show("Debe completar todos los datos para poder agregar el usuario");
            }
        }

    }
}
