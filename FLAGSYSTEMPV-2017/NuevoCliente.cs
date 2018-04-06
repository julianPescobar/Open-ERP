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
                    nuevoc.Parameters.AddWithValue("@k", "Activo");
                    Conexion.abrir();
                    Conexion.Insertar("Clientes", "nombre,atencion,direccion,localidad,provincia,cp,telefono,mail,cuit,tipocuit,eliminado", "@a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k", nuevoc);
                    Conexion.cerrar();
                    this.Close();
                    if (Application.OpenForms.OfType<Clientes>().Count() == 1)
                        Application.OpenForms.OfType<Clientes>().First().Close();

                    
                   
                    Clientes openagain = new Clientes();
                    openagain.Show();
                    if (Application.OpenForms.OfType<Clientes>().Count() > 0)
                    {
                        Application.OpenForms.OfType<Clientes>().First().Focus();

                    }
                    




                }
                else MessageBox.Show("Debe completar todos los campos con asterisco para poder dar de alta el cliente");
            }
            if (createorupdate.status == "update")
            {if (textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && textBox8.Text != "")
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

                this.Close();
                
                if (Application.OpenForms.OfType<Clientes>().Count() > 0)
                {
                    Application.OpenForms.OfType<Clientes>().First().Dispose();

                }


                Clientes clnts = new Clientes();
                clnts.Show();


               
                    
                }
            else MessageBox.Show("Debe completar todos los campos con asterisco para poder dar de alta el cliente");
           
            }
        }

        private void NuevoCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                if (Application.OpenForms.OfType<Clientes>().Count() > 0)
                {
                    Application.OpenForms.OfType<Clientes>().First().Focus();

                }
            }
            if (e.KeyCode == Keys.F1) button1.PerformClick();
            if (e.KeyCode == Keys.Enter && button1.Focused == false && button2.Focused == false)
            {
                SendKeys.SendWait("{TAB}");
            }

        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
            {
                SqlCeCommand checkexistance = new SqlCeCommand();
                checkexistance.Parameters.AddWithValue("code", textBox2.Text);
                Conexion.abrir();
                DataTable existira = Conexion.Consultar("nombre", "Clientes", "where nombre = @code and eliminado != 'Eliminado'", "", checkexistance);
                Conexion.cerrar();
                if (existira.Rows.Count > 0 && createorupdate.status == "create")
                {
                    MessageBox.Show("Ese nombre de cliente ya existe, use otro nombre por favor");
                    textBox2.Text = "";
                }
            }
        }

        private void comboBox2_Enter(object sender, EventArgs e)
        {
            comboBox2.DroppedDown = true;
            comboBox2.Select();
        }
    }
}
