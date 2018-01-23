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
    public partial class Anular : Form
    {


        public Anular()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Consultas_Load(object sender, EventArgs e)
        {
            this.Focus();
            

                label1.Text = "Listado de Ventas de hoy";
                
                Conexion.abrir();
                SqlCeCommand fechahoy = new SqlCeCommand();
                fechahoy.Parameters.AddWithValue("hoy", DateTime.Now.ToShortDateString());
                fechahoy.Parameters.AddWithValue("anulada", "Anulada");
                DataTable showv = Conexion.Consultar("nfactura as [N° Fact.], vendedor as Usuario, fechaventa as Fecha, total as Importe, estadoventa as Estado, tipoFactura as Factura", "Ventas","WHERE datediff(day,fechaventa,@hoy) =  0 AND estadoventa != @anulada", " order by nfactura desc", fechahoy);

                Conexion.cerrar();
                BindingSource SBind = new BindingSource();
                SBind.DataSource = showv;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = showv;
                dataGridView1.DataSource = SBind;
                dataGridView1.Columns[4].DefaultCellStyle.Format = "c";
                dataGridView1.Refresh();

                if (showv.Rows.Count > 0)
                    dataGridView1.DataSource = showv; //mostramos lo que hay
                textBox1.Focus();
                if (dataGridView1.Rows.Count < 1)
                {
                    button2.Enabled = false;
                }
            
        }

        void get(string what1, string fromwhere1, string where,string valuedata, DataGridView whatview1)
        {
            Conexion.abrir();
            
            DataTable showv = Conexion.Consultar(what1, fromwhere1, where, "", new SqlCeCommand());
            
            Conexion.cerrar();
            BindingSource SBind = new BindingSource();
            SBind.DataSource = showv;
            whatview1.AutoGenerateColumns = true;
            whatview1.DataSource = showv;
            whatview1.DataSource = SBind;
            //whatview1.Columns[3].DefaultCellStyle.Format = "c";
            whatview1.Refresh();

            if (showv.Rows.Count > 0)
                whatview1.DataSource = showv; //mostramos lo que hay
           
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            
            
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

        private void Consultas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                           this.DisplayRectangle);      
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            if (Conexion.data == "Ventas")
            {
                try
                {
                var bd = dataGridView1.DataSource;
                var dt = (DataTable)bd;
                dt.DefaultView.RowFilter = string.Format("CONVERT([N° Fact.],System.String) like '%{0}%' or CONVERT([Fecha],System.String) like '%{0}%'  or CONVERT([Importe],System.String) like '%{0}%' or [Usuario] like '%{0}%'", textBox1.Text.Trim().Replace("'", "''"));
                dataGridView1.Refresh();
                }
                catch (Exception) { }
            }
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
           string id =  dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
           DialogResult seguro =  MessageBox.Show("Está seguro de anular esta venta?\nFactura n°:"+id,"Anular esta factura?",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
           if (seguro == DialogResult.Yes)
           {
               Conexion.abrir();
               SqlCeCommand anular = new SqlCeCommand();
               anular.Parameters.AddWithValue("anul", "Anulada");
               anular.Parameters.AddWithValue("id", id);
               Conexion.abrir();
               Conexion.Actualizar("Ventas", "estadoventa = @anul", "WHERE nfactura = @id", "", anular);
               Conexion.cerrar();
               Anular refresh = new Anular();
             
               refresh.Show();
               refresh.Focus();
               this.Close();
               
           }
        }
      
    }
}
