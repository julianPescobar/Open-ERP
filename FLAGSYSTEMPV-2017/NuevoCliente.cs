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
    public partial class NuevoCliente : Form
    {
        public NuevoCliente()
        {
            InitializeComponent();
        }

        private void NuevoCliente_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                         this.DisplayRectangle);      
        }

        private void NuevoCliente_Load(object sender, EventArgs e)
        {
            if (createorupdate.status == "update")
            {
                SqlCeCommand idprod = new SqlCeCommand();
                idprod.Parameters.AddWithValue("id", createorupdate.itemid);
                Conexion.abrir();
                DataTable datosprod = Conexion.Consultar("*", "Clientes", "WHERE idcliente = @id", "", idprod);

                Conexion.cerrar();
                
                button1.Text = "Guardar cambios";
                if (datosprod.Rows.Count > 0)
                {
                    textBox2.Text = datosprod.Rows[0][1].ToString();
                    textBox3.Text = datosprod.Rows[0][2].ToString();
                    textBox4.Text = datosprod.Rows[0][3].ToString();
                    textBox5.Text = datosprod.Rows[0][4].ToString();
                    textBox6.Text = datosprod.Rows[0][5].ToString();
                    textBox7.Text = datosprod.Rows[0][6].ToString();
                    textBox8.Text = datosprod.Rows[0][7].ToString();
                    textBox9.Text = datosprod.Rows[0][8].ToString();
                    textBox1.Text = datosprod.Rows[0][9].ToString();
                    comboBox2.SelectedItem = datosprod.Rows[0][10].ToString();
                }
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
            if (createorupdate.status == "create")
            {
                if (textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && textBox8.Text != "")
                {

                    string aa, bb, cc, dd, ee, ff, gg, hh, ii, jj;
                    aa = textBox2.Text;
                    bb = textBox3.Text;
                    cc = textBox4.Text;
                    dd = textBox5.Text;
                    ee = textBox6.Text;
                    ff = textBox7.Text;
                    gg = textBox8.Text;
                    hh = textBox9.Text;
                    ii = textBox1.Text;
                    jj = comboBox2.Text;
                    SqlCeCommand nuevoc = new SqlCeCommand();
                    nuevoc.Parameters.Clear();
                    nuevoc.Parameters.AddWithValue("@a", aa);
                    nuevoc.Parameters.AddWithValue("@b", bb);
                    nuevoc.Parameters.AddWithValue("@c", cc);
                    nuevoc.Parameters.AddWithValue("@d", dd);
                    nuevoc.Parameters.AddWithValue("@e", ee);
                    nuevoc.Parameters.AddWithValue("@f", ff);
                    nuevoc.Parameters.AddWithValue("@g", gg);
                    nuevoc.Parameters.AddWithValue("@h", hh);
                    nuevoc.Parameters.AddWithValue("@i", ii);
                    nuevoc.Parameters.AddWithValue("@j", jj);
                    Conexion.abrir();
                    Conexion.Insertar("Clientes", "nombre,atencion,direccion,localidad,provincia,cp,telefono,mail,cuit,tipocuit", "@a,@b,@c,@d,@e,@f,@g,@h,@i,@j", nuevoc);
                    Conexion.cerrar();

                    if (Application.OpenForms.OfType<Clientes>().Count() == 1)
                    {
                        Application.OpenForms.OfType<Clientes>().First().Close();


                    }
                    Clientes openagain = new Clientes();
                    openagain.Show();
                    this.Close();




                }
            }
            if (createorupdate.status == "update")
            {
                string aa, bb, cc, dd, ee, ff, gg, hh, ii, jj;
                aa = textBox2.Text;
                bb = textBox3.Text;
                cc = textBox4.Text;
                dd = textBox5.Text;
                ee = textBox6.Text;
                ff = textBox7.Text;
                gg = textBox8.Text;
                hh = textBox9.Text;
                ii = textBox1.Text;
                jj = comboBox2.Text;
                SqlCeCommand nuevoc = new SqlCeCommand();
                nuevoc.Parameters.Clear();
                nuevoc.Parameters.AddWithValue("@id", createorupdate.itemid);
                nuevoc.Parameters.AddWithValue("@a", aa);
                nuevoc.Parameters.AddWithValue("@b", bb);
                nuevoc.Parameters.AddWithValue("@c", cc);
                nuevoc.Parameters.AddWithValue("@d", dd);
                nuevoc.Parameters.AddWithValue("@e", ee);
                nuevoc.Parameters.AddWithValue("@f", ff);
                nuevoc.Parameters.AddWithValue("@g", gg);
                nuevoc.Parameters.AddWithValue("@h", hh);
                nuevoc.Parameters.AddWithValue("@i", ii);
                nuevoc.Parameters.AddWithValue("@j", jj);
                Conexion.abrir();
                Conexion.Actualizar("Clientes", "nombre = @a ,atencion = @b,direccion = @c,localidad = @d,provincia = @e,cp = @f,telefono = @g,mail = @h,cuit = @i,tipocuit = @j", "WHERE idcliente = @id", "", nuevoc);
                Conexion.cerrar();

                if (Application.OpenForms.OfType<Clientes>().Count() == 1)
                {
                    Application.OpenForms.OfType<Clientes>().First().Close();


                }
                Clientes openagain = new Clientes();
                openagain.Show();
                this.Close();
            }
        }

        private void NuevoCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

        }
    }
}
