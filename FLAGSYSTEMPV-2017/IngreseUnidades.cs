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
    public partial class IngreseUnidades : Form
    {
        public IngreseUnidades()
        {
            InitializeComponent();
        }

        private void IngreseUnidades_Load(object sender, EventArgs e)
        {
            numericUpDown1.Select(0, 1);
        }

        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //abrir busqueda de articulo
                totalventa.cantidad = numericUpDown1.Value;
                this.Close();

            }
            if (e.KeyCode == Keys.Escape)
            {
                //abrir busqueda de articulo
                this.Close();
            }
        }

       
    }
}
