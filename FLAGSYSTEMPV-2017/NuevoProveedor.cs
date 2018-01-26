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
            if (createorupdate.status == "create")
            {

                Conexion.abrir();
                DataTable rubros = Conexion.Consultar("nombrerubro", "Rubros", "", "", new SqlCeCommand());
                Conexion.cerrar();
                for (int i = 0; i < rubros.Rows.Count; i++)
                {
                    comboBox1.Items.Add(rubros.Rows[i][0].ToString());
                }
            }
            if (createorupdate.status == "update")
            {
                button1.Text = "Guardar cambios";
                SqlCeCommand id = new SqlCeCommand();
                id.Parameters.AddWithValue("id", createorupdate.itemid);
                Conexion.abrir();
                DataTable data = Conexion.Consultar("*", "Proveedores", "WHERE idproveedor = @id", "", id);
                DataTable rubros = Conexion.Consultar("nombrerubro", "Rubros", "", "", new SqlCeCommand());
                Conexion.cerrar();
                for (int i = 0; i < rubros.Rows.Count; i++)
                {
                    comboBox1.Items.Add(rubros.Rows[i][0].ToString());
                }
                if (data.Rows.Count > 0)
                {
                        textBox2.Text = data.Rows[0][1].ToString();//nombre
                        textBox3.Text = data.Rows[0][3].ToString();//atenc
                        comboBox1.SelectedItem = data.Rows[0][2].ToString();//rubro
                        textBox4.Text = data.Rows[0][4].ToString();//direc
                        textBox5.Text = data.Rows[0][5].ToString();//locali
                        textBox6.Text = data.Rows[0][6].ToString();//prov
                        textBox7.Text = data.Rows[0][11].ToString();//cp
                        textBox8.Text = data.Rows[0][7].ToString();//tel
                        textBox9.Text = data.Rows[0][8].ToString();//email
                        textBox1.Text = data.Rows[0][10].ToString();//cuitno
                        comboBox2.SelectedItem = data.Rows[0][9].ToString();//cuittype
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
            if (createorupdate.status == "update")
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
                nuevoproveedor.Parameters.AddWithValue("id", createorupdate.itemid);
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
                Conexion.Actualizar("Proveedores", "nombre =@a ,atencion =@b,rubro =@c,direccion =@d,localidad =@e,provincia =@f,telefono =@h,mail =@i,numcuit =@j,tipocuit =@k,cp =@g", "WHERE idproveedor = @id","", nuevoproveedor);
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
