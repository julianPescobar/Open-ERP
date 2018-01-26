using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.SqlServerCe;
namespace FLAGSYSTEMPV_2017
{
    public partial class Informe : Form
    {
        public Informe()
        {
            InitializeComponent();
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




        private void InformeVentas_Load(object sender, EventArgs e)
        {
            if (Conexion.data == "Articulos")
            {
                Conexion.abrir();
                DataTable arts = Conexion.Consultar("idarticulo,codigoart as [Codigo],descripcion as [Nombre],marca as [Marca],precio as [Precio],costo as [Costo],iva as [IVA],stockactual as [Stock],stockminimo as [Stock mín.],proveedor as [Proveedor],compraminima as [Compra mín.] ,porcentaje as [%]", "Articulos", "", "", new SqlCeCommand());
                Conexion.cerrar();
                BindingSource SBind = new BindingSource();
                SBind.DataSource = arts;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = arts;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[4].DefaultCellStyle.Format = "c";
                dataGridView1.Columns[5].DefaultCellStyle.Format = "c";
                dataGridView1.DataSource = SBind;
                dataGridView1.Refresh();
            }
            if (Conexion.data == "Caja")
            {
                SqlCeCommand notdeleted = new SqlCeCommand();
                notdeleted.Parameters.AddWithValue("an", "Anulada");
                Conexion.abrir();
                DataTable ventas = Conexion.Consultar("fechaventa,nfactura,total", "Ventas", "WHERE estadoventa != @an", "", notdeleted);
                DataTable entrada = Conexion.Consultar("fecha,motivo,total", "EntradaCaja", "", "", new SqlCeCommand());
                DataTable salida = Conexion.Consultar("fecha,motivo,total", "SalidaCaja", "", "", new SqlCeCommand());
                DataTable gastos = Conexion.Consultar("fecha,descripcion,importe", "Gastos", "", "", new SqlCeCommand());
                Conexion.cerrar();

                DataTable arts = new DataTable();
                arts.Columns.Add("Fecha", typeof(string));
                arts.Columns.Add("Tipo de transaccion", typeof(string));
                arts.Columns.Add("Motivo o N Factura", typeof(string));
                arts.Columns.Add("Importe", typeof(string));
                for(int i = 0; i < ventas.Rows.Count;i++)
                {
                    DataRow iventa = arts.NewRow();
                    iventa[0] = ventas.Rows[i][0].ToString();
                    iventa[1] = "Venta";
                    iventa[2] = ventas.Rows[i][1].ToString();
                    iventa[3] = float.Parse(ventas.Rows[i][2].ToString()).ToString("$0.00");
                    arts.Rows.Add(iventa);
                }
                for (int i = 0; i < entrada.Rows.Count; i++)
                {
                    DataRow iventa = arts.NewRow();
                    iventa[0] = entrada.Rows[i][0].ToString();
                    iventa[1] = "Entrada de Caja";
                    iventa[2] = entrada.Rows[i][1].ToString();
                    iventa[3] = float.Parse(entrada.Rows[i][2].ToString()).ToString("$0.00");
                    arts.Rows.Add(iventa);
                }
                for (int i = 0; i < salida.Rows.Count; i++)
                {
                    DataRow iventa = arts.NewRow();
                    iventa[0] = salida.Rows[i][0].ToString();
                    iventa[1] = "Salida de Caja";
                    iventa[2] = salida.Rows[i][1].ToString();
                    iventa[3] = float.Parse(salida.Rows[i][2].ToString()).ToString("$0.00");
                    arts.Rows.Add(iventa);
                }
                for (int i = 0; i < gastos.Rows.Count; i++)
                {
                    DataRow iventa = arts.NewRow();
                    iventa[0] = gastos.Rows[i][0].ToString();
                    iventa[1] = "Gasto";
                    iventa[2] = gastos.Rows[i][1].ToString();
                    iventa[3] = float.Parse(gastos.Rows[i][2].ToString()).ToString("$0.00");
                    arts.Rows.Add(iventa);
                }
                BindingSource SBind = new BindingSource();
                SBind.DataSource = arts;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = arts;
                //dataGridView1.Columns[0].Visible = false;
                //dataGridView1.Columns[3].DefaultCellStyle.Format = "c";
               // dataGridView1.Columns[5].DefaultCellStyle.Format = "c";
                dataGridView1.DataSource = SBind;
                dataGridView1.Refresh();
            }
            if (Conexion.data == "Ventas")
            {
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
            }
            if (Conexion.data == "Clientes")
            {
                Conexion.abrir();
                DataTable showacls = Conexion.Consultar("idcliente,nombre as [Nombre del cliente],atencion as [Atencion],direccion as Domicilio,telefono as Telefono,mail as Email, cuit as CUIT", "Clientes", "", "", new SqlCeCommand());
                Conexion.cerrar();
                BindingSource SBind = new BindingSource();
                SBind.DataSource = showacls;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = showacls;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.DataSource = SBind;
                dataGridView1.Refresh();
            }
            if (Conexion.data == "Proveedores")
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
            }
            if (Conexion.data == "Compras")
            {
                Conexion.abrir();
                DataTable showv = Conexion.Consultar("nfactura as [N° Factura], vendedor as Vendedor, fechacompra as Fecha,proveedor as Proveedor, totalfactura as Total,estadocompra as Estado ", "Compras", " order by nfactura desc", "", new SqlCeCommand());

                Conexion.cerrar();
                BindingSource SBind = new BindingSource();
                SBind.DataSource = showv;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = showv;
                dataGridView1.DataSource = SBind;

                dataGridView1.Refresh();
                Conexion.data = "";
            }
            if (Conexion.data == "Pedido")
            {
                Conexion.abrir();
                DataTable arts = Conexion.Consultar("idarticulo,descripcion as [Nombre],proveedor as [Proveedor],marca as [Marca],precio as [Precio],costo as [Costo],stockactual as [Stock],stockminimo as [Stock min.],compraminima as [Compra min.]", "Articulos", " order by proveedor asc", "", new SqlCeCommand());
                Conexion.cerrar();
                BindingSource SBind = new BindingSource();
                SBind.DataSource = arts;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = arts;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[4].DefaultCellStyle.Format = "c";
                dataGridView1.Columns[5].DefaultCellStyle.Format = "c";
                
               
                dataGridView1.DataSource = SBind;
               
                dataGridView1.Refresh();
                 dataGridView1.Columns.Add("pedir", "ingrese pedido");
              
            }
            if (Conexion.data == "AutoPedido")
            {
                Conexion.abrir();
                DataTable arts = Conexion.Consultar("idarticulo,descripcion as [Nombre],proveedor as [Proveedor],marca as [Marca],precio as [Precio],costo as [Costo],stockactual as [Stock],stockminimo as [Stock min.],compraminima as [Compra min.]", "Articulos", " order by proveedor asc", "", new SqlCeCommand());
                Conexion.cerrar();
                BindingSource SBind = new BindingSource();
                SBind.DataSource = arts;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = arts;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[4].DefaultCellStyle.Format = "c";
                dataGridView1.Columns[5].DefaultCellStyle.Format = "c";


                dataGridView1.DataSource = SBind;

                dataGridView1.Refresh();
                dataGridView1.Columns.Add("pedir", "Pedir");
                  float stockactual, stockminimo,compraminima;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {

                    stockactual = Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value.ToString());
                    stockminimo = Convert.ToInt32(dataGridView1.Rows[i].Cells[7].Value.ToString());
                    compraminima = Convert.ToInt32(dataGridView1.Rows[i].Cells[8].Value.ToString());

                    if (stockactual < stockminimo) dataGridView1.Rows[i].Cells[9].Value = compraminima.ToString();
                    else dataGridView1.Rows[i].Cells[9].Value = "0";
                }
            }
            if (Conexion.data == "Faltantes")
            {
                Conexion.abrir();
                DataTable arts = Conexion.Consultar("idarticulo,descripcion as [Nombre],proveedor as [Proveedor],marca as [Marca],precio as [Precio],costo as [Costo],stockactual as [Stock],stockminimo as [Stock min.],compraminima as [Compra min.]", "Articulos", "where stockactual< stockminimo order by proveedor asc", "", new SqlCeCommand());
                Conexion.cerrar();
                BindingSource SBind = new BindingSource();
                SBind.DataSource = arts;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = arts;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[4].DefaultCellStyle.Format = "c";
                dataGridView1.Columns[5].DefaultCellStyle.Format = "c";


                dataGridView1.DataSource = SBind;

                dataGridView1.Refresh();
                

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
