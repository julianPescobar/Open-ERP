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
    public partial class ABStock : Form
    {
        public ABStock()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
      

        private void button1_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<AgregarAltabaja>().Count() == 1)
                Application.OpenForms.OfType<AgregarAltabaja>().First().Focus();
            else
            {
                AgregarAltabaja frm = new AgregarAltabaja();
                frm.Show();
            }
        }

        private void Clientes_Load(object sender, EventArgs e)
        {
            this.Focus();
            Conexion.abrir();
            DataTable showacls = Conexion.Consultar("idab,fecha as Fecha,codigo as Codigo,descripcion as Descripcion,altas as Altas,bajas as Bajas,motivo as Motivo,vendedor as Vendedor", "Altasbajas", "", "", new SqlCeCommand());
            Conexion.cerrar();
            BindingSource SBind = new BindingSource();
            SBind.DataSource = showacls;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = showacls;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.DataSource = SBind;
            dataGridView1.Refresh();
           
        }

        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
               

                    BindingSource bd = (BindingSource)dataGridView1.DataSource;
                    DataTable dt = (DataTable)bd.DataSource;
                    string formatstring = "";
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i == 0) formatstring += " CONVERT([" + dt.Columns[i].ColumnName + "],System.String) like '%{0}%' ";
                        else formatstring += " or CONVERT([" + dt.Columns[i].ColumnName + "],System.String) like '%{0}%' ";

                    }


                    dt.DefaultView.RowFilter = string.Format(formatstring, textBox1.Text.Trim().Replace("'", "''"));
                    dataGridView1.Refresh();
                
            }
            catch (Exception) { }
           
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.F1)
            {
                button1.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                textBox1.Select();
            }
        }
    }
}
