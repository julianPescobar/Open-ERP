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
               
                    string rubro = textBox1.Text;
                    SqlCeCommand rubr = new SqlCeCommand();
                    rubr.Parameters.AddWithValue("ru", rubro);
                    Conexion.abrir();
                    Conexion.Insertar("Rubros", "nombrerubro, eliminado", "@ru,'Activo'", rubr);
                    Conexion.cerrar();
                    this.Close();
                    if (Application.OpenForms.OfType<Rubros>().Count() == 1)
                    {
                        Application.OpenForms.OfType<Rubros>().First().Close();
                        Rubros openagain = new Rubros();
                        openagain.Show();
                    }
                
               
            }
            else
                MessageBox.Show("Debe ingresar un nombre para el rubro");
            }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
