﻿using System;
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
    public partial class Proveedores : Form
    {
        public Proveedores()
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

        private void Proveedores_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                         this.DisplayRectangle);      
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<NuevoProveedor>().Count() == 1)
                Application.OpenForms.OfType<NuevoProveedor>().First().Focus();
            else
            {
                NuevoProveedor frm = new NuevoProveedor();
                frm.ShowDialog();
            }
        }

        private void Proveedores_Load(object sender, EventArgs e)
        {
            getprovs();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            var row = this.dataGridView1.Rows[rowIndex];
            string name = row.Cells["Nombre del Proveedor"].Value.ToString();
            string id = row.Cells["idproveedor"].Value.ToString();

            DialogResult borrar = MessageBox.Show("Está segudo de borrar a este proveedor?\n"+name, "Borrar?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (borrar == DialogResult.Yes)
            {
                Conexion.abrir();
                SqlCeCommand del = new SqlCeCommand();
                del.Parameters.AddWithValue("@id",id);
                Conexion.Eliminar("Proveedores", "idproveedor = @id", del);
                 Conexion.cerrar();
                
            }
            getprovs();
        }

        void getprovs()
        {
            Conexion.abrir();
            DataTable showprovs = Conexion.Consultar("idproveedor,nombre as [Nombre del Proveedor],atencion as [Atencion],telefono as Telefono,mail as [Correo Electrónico],direccion as [Direccion],localidad as Localidad,cp as CP", "Proveedores", "", "", new SqlCeCommand());
            Conexion.cerrar();
            BindingSource SBind = new BindingSource();
            SBind.DataSource = showprovs;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = showprovs;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.DataSource = SBind;
            dataGridView1.Refresh();

            if (showprovs.Rows.Count > 0)
            {

                dataGridView1.DataSource = showprovs; //mostramos lo que hay
                button2.Enabled = true;
                button9.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button9.Enabled = false;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var bd = dataGridView1.DataSource;
            var dt = (DataTable)bd;
            dt.DefaultView.RowFilter = string.Format("[Nombre del Proveedor] like '%{0}%' or [Atencion] like '%{0}%'  or [Direccion] like '%{0}%' or [Telefono] like '%{0}%' or [CP] like '%{0}%'", textBox1.Text.Trim().Replace("'", "''"));
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