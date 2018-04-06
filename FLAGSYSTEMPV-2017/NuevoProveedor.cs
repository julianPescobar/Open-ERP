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
        public string nombreviejo;
        private void NuevoProveedor_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                         this.DisplayRectangle);      
        }

        private void NuevoProveedor_Load(object sender, EventArgs e)
        {
           
            if (createorupdate.status == "create")
            {
                comboBox1.Items.Clear();
                Conexion.abrir();
                DataTable rubros = Conexion.Consultar("nombrerubro", "Rubros", "where eliminado = 'Activo'", "", new SqlCeCommand());
                Conexion.cerrar();
                for (int i = 0; i < rubros.Rows.Count; i++)
                {
                    comboBox1.Items.Add(rubros.Rows[i][0].ToString());
                }
            }
            if (createorupdate.status == "update")
            {
               
                comboBox1.Items.Clear();
                button1.Text = "Guardar cambios";
                SqlCeCommand id = new SqlCeCommand();
                id.Parameters.AddWithValue("id", createorupdate.itemid);
                Conexion.abrir();
                DataTable data = Conexion.Consultar("*", "Proveedores", "WHERE idproveedor = @id", "", id);
                DataTable rubros = Conexion.Consultar("nombrerubro", "Rubros", "where eliminado = 'Activo'", "", new SqlCeCommand());
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
                        nombreviejo = textBox2.Text;
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
                if (textBox2.Text.Length < 1 || comboBox1.SelectedIndex < 0 || comboBox2.SelectedIndex < 0)
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
                    nuevoproveedor.Parameters.AddWithValue("@l",nombreviejo);
                    nuevoproveedor.Parameters.AddWithValue("@m", "Activo");
                    Conexion.abrir();
                    Conexion.Insertar("Proveedores", "nombre,atencion,rubro,direccion,localidad,provincia,telefono,mail,numcuit,tipocuit,cp,Eliminado", "@a,@b,@c,@d,@e,@f,@h,@i,@j,@k,@g,@m", nuevoproveedor);
                   
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
                if (textBox2.Text.Length < 1 || comboBox1.SelectedIndex < 0 || comboBox2.SelectedIndex < 0)
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
                    nuevoproveedor.Parameters.AddWithValue("@l", nombreviejo);
                    
                    Conexion.abrir();
                    Conexion.Actualizar("Proveedores", "nombre =@a ,atencion =@b,rubro =@c,direccion =@d,localidad =@e,provincia =@f,telefono =@h,mail =@i,numcuit =@j,tipocuit =@k,cp =@g", "WHERE idproveedor = @id", "", nuevoproveedor);
                    if (nombre != nombreviejo)
                    {
                        
                        Conexion.Actualizar("Articulos", "proveedor = @a", "where proveedor = @l", "", nuevoproveedor);
                    }
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

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            Conexion.abrir();
            DataTable rubros = Conexion.Consultar("nombrerubro", "Rubros", "where eliminado = 'Activo'", "", new SqlCeCommand());
            Conexion.cerrar();
            for (int i = 0; i < rubros.Rows.Count; i++)
            {
                comboBox1.Items.Add(rubros.Rows[i][0].ToString());
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0 && createorupdate.status == "create")
            {
                SqlCeCommand existe = new SqlCeCommand();
                existe.Parameters.AddWithValue("nombrex", textBox2.Text);
                Conexion.abrir();
                DataTable existira = Conexion.Consultar("nombre", "Proveedores", "Where nombre = @nombrex and Eliminado != 'Eliminado'", "",existe);
                Conexion.cerrar();
                if (existira.Rows.Count > 0)
                {
                    MessageBox.Show("Ese nombre de proveedor ya existe, intente con otro nombre");
                    textBox2.Text = "";
                    textBox2.Focus();
                }
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && createorupdate.status == "create")
            {
                SqlCeCommand existe = new SqlCeCommand();
                existe.Parameters.AddWithValue("cuitx", textBox1.Text);
                Conexion.abrir();
                DataTable existira = Conexion.Consultar("numcuit", "Proveedores", "Where numcuit = @cuitx", "", existe);
                Conexion.cerrar();
                if (existira.Rows.Count > 0)
                {
                    MessageBox.Show("Ese CUIT de proveedor ya existe, no se admiten clientes duplicados en el sistema.");
                    textBox1.Text = "";
                    textBox1.Focus();
                }
            }
        }

        private void NuevoProveedor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
            if (e.KeyCode == Keys.F1) button1.PerformClick();
            if (e.KeyCode == Keys.Enter && button1.Focused == false && button2.Focused == false)
            {
                SendKeys.SendWait("{TAB}"); ;
            }
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            comboBox1.DroppedDown = true;
            comboBox1.Select();
        }

        private void comboBox2_Enter(object sender, EventArgs e)
        {
            comboBox2.DroppedDown = true;
            comboBox2.Select();
        }
    }
}
