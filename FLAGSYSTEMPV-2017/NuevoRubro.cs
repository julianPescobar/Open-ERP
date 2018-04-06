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
    public partial class NuevoRubro : Form
    {
        public NuevoRubro()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if(textBox1.Text.Length > 0)
            {
                SqlCeCommand checkexistance = new SqlCeCommand();
                checkexistance.Parameters.AddWithValue("code", textBox1.Text);
                Conexion.abrir();
                DataTable existira = Conexion.Consultar("nombrerubro", "Rubros", "where nombrerubro = @code and eliminado != 'Eliminado'", "", checkexistance);
                Conexion.cerrar();
                if (existira.Rows.Count > 0)
                {
                    MessageBox.Show("Ese nombre de rubro ya existe, use otro nombre por favor");
                    textBox1.Text = "";
                }
                else
                {
                    string rubro = textBox1.Text;
                    SqlCeCommand rubr = new SqlCeCommand();
                    rubr.Parameters.AddWithValue("ru", rubro);
                    Conexion.abrir();
                    Conexion.Insertar("Rubros", "nombrerubro, eliminado", "@ru,'Activo'", rubr);
                    Conexion.cerrar();
                    this.Close();
                    if (Application.OpenForms.OfType<Rubros>().Count() >= 1)
                    {
                        Application.OpenForms.OfType<Rubros>().First().Close();
                        
                    }
                    Rubros openagain = new Rubros();
                    openagain.Show();
                    textBox1.Select();
                }
             }
            else
                MessageBox.Show("Debe ingresar un nombre para el rubro");
            }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                SqlCeCommand checkexistance = new SqlCeCommand();
                checkexistance.Parameters.AddWithValue("code", textBox1.Text);
                Conexion.abrir();
                DataTable existira = Conexion.Consultar("nombrerubro", "Rubros", "where nombrerubro = @code and eliminado != 'Eliminado'", "", checkexistance);
                Conexion.cerrar();
                if (existira.Rows.Count > 0 )
                {
                    MessageBox.Show("Ese nombre de rubro ya existe, use otro nombre por favor");
                    textBox1.Text = "";
                }
            }
            
        }

        private void NuevoRubro_Load(object sender, EventArgs e)
        {
            textBox1.Select();
        }

        private void NuevoRubro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
            if (e.KeyCode == Keys.Enter) button1.PerformClick();
        }
    }
}
