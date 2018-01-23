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
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        private void Clientes_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                         this.DisplayRectangle);      
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
            var bd = (BindingSource)dataGridView1.DataSource;
            var dt = (DataTable)bd.DataSource;
            dt.DefaultView.RowFilter = string.Format("CONVERT([Fecha],System.String) like '%{0}%' or CONVERT([Codigo],System.String) like '%{0}%'  or CONVERT([Descripcion],System.String) like '%{0}%' or CONVERT([Motivo],System.String) like '%{0}%' or CONVERT([Vendedor],System.String) like '%{0}%'", textBox1.Text.Trim().Replace("'", "''"));
            dataGridView1.Refresh();
           
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
