using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FLAGSYSTEMPV_2017
{
    public partial class IngreseMonto : Form
    {
        public IngreseMonto()
        {
            InitializeComponent();
        }

        private void IngreseUnidades_Load(object sender, EventArgs e)
        {
         
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //abrir busqueda de articulo
                try
                {
                    totalventa.montocompra = float.Parse(textBox1.Text.ToString());
                    this.Close();
                }
                catch (Exception)
                {
                    textBox1.Text = "";
                }
                

            }
            if (e.KeyCode == Keys.Escape)
            {
                //abrir busqueda de articulo
                this.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Replace(".", ",");
            textBox1.SelectionStart = textBox1.Text.Length;
        }

       

      

       
    }
}
