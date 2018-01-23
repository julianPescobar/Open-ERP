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
    public partial class Buscarticulo : Form
    {
        public Buscarticulo()
        {
            InitializeComponent();
        }

        private void Buscarticulo_Load(object sender, EventArgs e)
        {
            if (totalventa.compraoventa == "Ventas" || totalventa.compraoventa == "NC" || totalventa.compraoventa == "ND")
            {
                Conexion.abrir();
                DataTable showarts = Conexion.Consultar("idarticulo,codigoart as [Codigo Articulo],descripcion as [Descripcion del Articulo],proveedor as Proveedor,precio as Precio,costo as Costo,stockactual as Stock,stockminimo as [Stock Mínimo],iva as IVA", "Articulos", "", "", new SqlCeCommand());
                Conexion.cerrar();
                BindingSource SBind = new BindingSource();
                SBind.DataSource = showarts;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = showarts;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[4].DefaultCellStyle.Format = "c";
                dataGridView1.Columns[5].DefaultCellStyle.Format = "c";
                dataGridView1.DataSource = SBind;
                dataGridView1.Refresh();
            }
            if (totalventa.compraoventa == "Compras")
            {
                Conexion.abrir();
                SqlCeCommand prove = new SqlCeCommand();
                prove.Parameters.AddWithValue("pr", totalventa.proveedcompra);
                DataTable showarts = Conexion.Consultar("idarticulo,codigoart as [Codigo Articulo],descripcion as [Descripcion del Articulo],proveedor as Proveedor,precio as Precio,costo as Costo,stockactual as Stock,stockminimo as [Stock Mínimo],iva as IVA", "Articulos", "WHERE proveedor = @pr", "", prove);
                Conexion.cerrar();
                BindingSource SBind = new BindingSource();
                SBind.DataSource = showarts;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = showarts;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[4].DefaultCellStyle.Format = "c";
                dataGridView1.Columns[5].DefaultCellStyle.Format = "c";
                dataGridView1.DataSource = SBind;
                dataGridView1.Refresh();
            }

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                try
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    dataGridView1.Rows[rowIndex - 1].Cells[1].Selected = true;
                }
                catch (Exception)
                {
                }

            }
            if (e.KeyCode == Keys.Down)
            {
                try
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    dataGridView1.Rows[rowIndex + 1].Cells[1].Selected = true;
                }
                catch (Exception)
                {
                }

            }
            if (e.KeyCode == Keys.Escape)
            {
                totalventa.codprodbuscado = "";
                    this.Close();
            }
            if (e.KeyCode == Keys.Enter)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;

                string cod = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
                totalventa.codprodbuscado = cod;
                this.Close();
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
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var bd = (BindingSource)dataGridView1.DataSource;
            var dt = (DataTable)bd.DataSource;
            dt.DefaultView.RowFilter = string.Format("[Codigo Articulo] like '%{0}%' or [Descripcion del Articulo] like '%{0}%' or [Proveedor] like '%{0}%'", textBox1.Text.Trim().Replace("'", "''"));
            dataGridView1.Refresh();
        }

        private void Buscarticulo_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                        this.DisplayRectangle);      
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Focus();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Focus();
        }
    }
}
