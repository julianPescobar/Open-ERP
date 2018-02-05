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
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }


        private void Config_Load(object sender, EventArgs e)
        {
            cargadata();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Config_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                          this.DisplayRectangle);            
        }
        private void cargadata()
        {
            if (Demo.EsDemo == false)
            {
                Conexion.abrir();
            DataTable datos = Conexion.Consultar("*", "Configuracion", "", "", new SqlCeCommand());
                    Conexion.cerrar();
                    if (datos.Rows.Count > 0)
                    {
                        textBox1.Text = datos.Rows[0][2].ToString();
                        textBox2.Text = datos.Rows[0][3].ToString();
                        textBox3.Text = datos.Rows[0][4].ToString();
                        textBox4.Text = datos.Rows[0][5].ToString();
                        textBox5.Text = datos.Rows[0][6].ToString();
                        textBox6.Text = datos.Rows[0][8].ToString();
                        textBox7.Text = float.Parse(datos.Rows[0][0].ToString()).ToString("$0.00");

                        textBox14.Text = datos.Rows[0][17].ToString();
                        textBox13.Text = datos.Rows[0][18].ToString();
                        textBox12.Text = datos.Rows[0][19].ToString();
                        textBox11.Text = datos.Rows[0][20].ToString();
                        textBox10.Text = datos.Rows[0][21].ToString();
                        textBox9.Text = datos.Rows[0][22].ToString();
                        textBox8.Text = datos.Rows[0][23].ToString();
                        textBox16.Text = datos.Rows[0][24].ToString();
                        
                    }
            }
            else
            {
                MessageBox.Show("Esta opcion es para usuarios de licencia solamente");
                this.Close();
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

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCeCommand inserto = new SqlCeCommand();
            inserto.Parameters.Clear();
            //inserto.Parameters.AddWithValue("a", textBox1.Text);
            inserto.Parameters.AddWithValue("b", textBox2.Text);
            inserto.Parameters.AddWithValue("c", textBox3.Text);
            inserto.Parameters.AddWithValue("d", textBox4.Text);
            inserto.Parameters.AddWithValue("e", textBox5.Text);
            //inserto.Parameters.AddWithValue("f", textBox6.Text);
            inserto.Parameters.AddWithValue("g", textBox7.Text.Replace("$",""));
            inserto.Parameters.AddWithValue("h", textBox14.Text);
            inserto.Parameters.AddWithValue("i", textBox13.Text);
            inserto.Parameters.AddWithValue("j", textBox12.Text);
            inserto.Parameters.AddWithValue("k", textBox11.Text);
            inserto.Parameters.AddWithValue("l", textBox10.Text);
            inserto.Parameters.AddWithValue("m", textBox9.Text);
            inserto.Parameters.AddWithValue("n", textBox8.Text);
            inserto.Parameters.AddWithValue("o", textBox16.Text);
            Conexion.abrir();
            Conexion.Actualizar("Configuracion", "DireccionFisica = @b,Email= @c,Telefono1= @d,Localidad= @e,SaldoInicial= @g,SMTP= @h,PUERTO= @i,SSL= @j,MAIL= @k,CLAVE= @l,PARA= @m,TITULO= @n,CUERPO= @o", "", "", inserto);
                Conexion.cerrar();
                registereduser.smtp = textBox14.ToString();
                registereduser.puerto = textBox13.ToString();
                registereduser.ssl = textBox12.ToString();
                registereduser.mail = textBox11.ToString();
                registereduser.clave = textBox10.ToString();
                registereduser.para = textBox9.ToString();
                registereduser.titulo = textBox8.ToString();
                registereduser.cuerpo = textBox16.ToString();    
            this.Close();
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("en construccion");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("en construccion");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("en construccion");
        }
    }
}
