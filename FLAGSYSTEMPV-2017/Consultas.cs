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
    public partial class Consultas : Form
    {


        public Consultas()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Consultas_Load(object sender, EventArgs e)
        {
          
            if (Conexion.data == "Ventas")
            {
                label1.Text = "Listado de Ventas";
                label2.Text = "Detalle de venta seleccionada";
                Conexion.abrir();
                DataTable showv = Conexion.Consultar("nfactura as [N° Fact.], vendedor as Usuario, fechaventa as Fecha, total as Importe, estadoventa as Estado, tipoFactura as Factura", "Ventas", " order by nfactura desc", "", new SqlCeCommand());
                Conexion.cerrar();
                BindingSource SBind = new BindingSource();
                SBind.DataSource = showv;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = showv;
                dataGridView1.DataSource = SBind;
                dataGridView1.Columns[3].DefaultCellStyle.Format = "c";
                dataGridView1.Refresh();
                if (showv.Rows.Count > 0)
                dataGridView1.DataSource = showv; //mostramos lo que hay
                textBox1.Focus();
            }
            if (Conexion.data == "Compras")
            {
                label1.Text = "Listado de Compras";
                label2.Text = "Detalle de compra seleccionada";
                Conexion.abrir();
                DataTable showv = Conexion.Consultar("nfactura as [N° Factura], vendedor as Vendedor, fechacompra as Fecha,proveedor as Proveedor, totalfactura as Total,estadocompra as Estado ", "Compras", " order by nfactura desc", "", new SqlCeCommand());
                Conexion.cerrar();
                BindingSource SBind = new BindingSource();
                SBind.DataSource = showv;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = showv;
                dataGridView1.DataSource = SBind;
                dataGridView1.Refresh();
                if (showv.Rows.Count > 0)
                    dataGridView1.DataSource = showv; //mostramos lo que hay
                textBox1.Focus();
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
            if (Conexion.data == "Ventas")
            {
                try
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    string nfactura = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                    Conexion.abrir();
                    SqlCeCommand data = new SqlCeCommand();
                    data.Parameters.AddWithValue("@nf", nfactura);
                    DataTable showv = Conexion.Consultar("codigoproducto as Codigo, descripproducto as Descripcion,marcaproducto as Marca,cantidproducto as Cantidad, precioproducto as Precio, totalproducto as Total", "DetalleVentas", "WHERE nfactura = @nf", "", data);

                    Conexion.cerrar();
                    BindingSource SBind = new BindingSource();
                    SBind.DataSource = showv;
                    dataGridView2.AutoGenerateColumns = true;

                    dataGridView2.DataSource = showv;
                   
                    dataGridView2.DataSource = SBind;
                   
                    dataGridView2.Refresh();
                    
                    if (showv.Rows.Count > 0)
                        dataGridView2.DataSource = showv; //mostramos lo que hay
                }
                catch (Exception) { }
            }
            if (Conexion.data == "Compras")
            {
                try
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    string nfactura = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                    Conexion.abrir();
                    SqlCeCommand data = new SqlCeCommand();
                    data.Parameters.AddWithValue("@nf", nfactura);
                    DataTable showv = Conexion.Consultar("nfactura as [N° Factura],descripproducto as Descripcion, marcaproducto as Marca, cantidproducto as Cantidad, precioproducto as [Precio U], totalproducto as Total", "DetalleCompras", "WHERE nfactura = @nf", "", data);

                    Conexion.cerrar();
                    BindingSource SBind = new BindingSource();
                    SBind.DataSource = showv;
                    dataGridView2.AutoGenerateColumns = true;
                    //dataGridView1.Columns[2].DefaultCellStyle.Format = "c";
                    //dataGridView2.Columns[4].DefaultCellStyle.Format = "c";
                    //dataGridView2.Columns[5].DefaultCellStyle.Format = "c";
                    dataGridView2.DataSource = showv;

                    dataGridView2.DataSource = SBind;

                    dataGridView2.Refresh();

                    if (showv.Rows.Count > 0)
                        dataGridView2.DataSource = showv; //mostramos lo que hay
                }
                catch (Exception)
                { }
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
            if (Conexion.data == "Compras")
            {
                try
                {
                    var bd = dataGridView1.DataSource;
                    var dt = (DataTable)bd;
                    dt.DefaultView.RowFilter = string.Format("CONVERT([N° Factura],System.String) like '%{0}%' or CONVERT([Vendedor],System.String) like '%{0}%' or CONVERT([Fecha],System.String) like '%{0}%' or CONVERT([Proveedor],System.String) like '%{0}%' or CONVERT([Total],System.String) like '%{0}%' or CONVERT([Estado],System.String) like '%{0}%'", textBox1.Text.Trim().Replace("'", "''"));
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
      
    }
}
