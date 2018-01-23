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
    public partial class NuevoProveedor : Form
    {
        public NuevoProveedor()
        {
            InitializeComponent();
        }

        private void NuevoProveedor_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                         this.DisplayRectangle);      
        }

        private void NuevoProveedor_Load(object sender, EventArgs e)
        {
            Conexion.abrir();
           DataTable rubros =  Conexion.Consultar("nombrerubro", "Rubros", "", "", new SqlCeCommand());
           Conexion.cerrar();
           for (int i = 0; i < rubros.Rows.Count; i++)
           {
               comboBox1.Items.Add(rubros.Rows[i][0].ToString());
           }
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length < 1 || comboBox1.SelectedItem.ToString().Length < 1)
            {
                MessageBox.Show("Debe completar los campos con asterisco obligatorios");
            }
            else
            {
                string nombre, atencion, rubro, direccion, localidad, provincia, cp, tel, email, cuit, tcuit;
                nombre = textBox2.Text;
                atencion = textBox3.Text;
                rubro = comboBox1.SelectedItem.ToString();
                direccion = textBox4.Text;
                localidad = textBox5.Text;
                provincia = textBox6.Text;
                cp = textBox7.Text;
                tel = textBox8.Text;
                email = textBox9.Text;
                cuit = textBox1.Text;
                tcuit = comboBox2.SelectedItem.ToString();
                
                SqlCeCommand nuevoproveedor = new SqlCeCommand();
                nuevoproveedor.Parameters.Clear();
                nuevoproveedor.Parameters.AddWithValue("@a", nombre);
                nuevoproveedor.Parameters.AddWithValue("@b", atencion);
                nuevoproveedor.Parameters.AddWithValue("@c", rubro);
                nuevoproveedor.Parameters.AddWithValue("@d", direccion);
                nuevoproveedor.Parameters.AddWithValue("@e", localidad);
                nuevoproveedor.Parameters.AddWithValue("@f", provincia);
                nuevoproveedor.Parameters.AddWithValue("@g", cp);
                nuevoproveedor.Parameters.AddWithValue("@h", tel);
                nuevoproveedor.Parameters.AddWithValue("@i", email);
                nuevoproveedor.Parameters.AddWithValue("@j", cuit);
                nuevoproveedor.Parameters.AddWithValue("@k", tcuit);
                Conexion.abrir();
                Conexion.Insertar("Proveedores", "nombre,atencion,rubro,direccion,localidad,provincia,telefono,mail,numcuit,tipocuit,cp", "@a,@b,@c,@d,@e,@f,@h,@i,@j,@k,@g", nuevoproveedor);
                Conexion.cerrar();
                if (Application.OpenForms.OfType<Proveedores>().Count() == 1)
                {
                    Application.OpenForms.OfType<Proveedores>().First().Close();
                    Proveedores openagain = new Proveedores();
                    openagain.Show();
                }

                this.Close();
            }
        }
    }
}
