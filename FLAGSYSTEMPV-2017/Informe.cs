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
using System.IO;
using System.Data.OleDb;
namespace FLAGSYSTEMPV_2017
{
    public partial class Informe : Form
    {
        public Informe()
        {
            InitializeComponent();
        }

        public string extension = "";


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
            label3.Text = "Informe de "+Conexion.data;
            if (Conexion.data == "Articulos")
            {
                Conexion.abrir();
                DataTable arts = Conexion.Consultar("idarticulo,codigoart as [Codigo],descripcion as [Nombre],marca as [Marca],precio as [Precio],costo as [Costo],iva as [IVA],stockactual as [Stock],stockminimo as [Stockminimo],proveedor as [Proveedor],compraminima as [Compraminima] ,porcentaje as [porcentganancia]", "Articulos", "WHERE eliminado != 'Eliminado' and tipo LIKE 'Producto%'", "", new SqlCeCommand());
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
            if (Conexion.data == "Servicios")
            {
                Conexion.abrir();
                DataTable arts = Conexion.Consultar("idarticulo,codigoart as [Codigo],descripcion as [Nombre],marca as [Marca],precio as [Precio],costo as [Costo],iva as [IVA],proveedor as [Proveedor],porcentaje as [porcentganancia]", "Articulos", "WHERE eliminado != 'Eliminado' and tipo LIKE 'Servicio%'", "", new SqlCeCommand());
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
            if (Conexion.data == "ControlStock")
            {
                Conexion.abrir();
                DataTable arts = Conexion.Consultar("idarticulo,codigoart as [Codigo],descripcion as [Nombre],marca as [Marca],proveedor as [Proveedor],stockactual as [Stock]", "Articulos", "WHERE eliminado != 'Eliminado' and tipo LIKE 'Producto%'", "", new SqlCeCommand());
                Conexion.cerrar();
                BindingSource SBind = new BindingSource();
                SBind.DataSource = arts;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = arts;
                dataGridView1.Columns[0].Visible = false;
               
                dataGridView1.DataSource = SBind;
                dataGridView1.Refresh();
                dataGridView1.Columns.Add("stockreal", "StockReal");
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    dataGridView1.Rows[i].Cells[6].Value = " ";
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

                DataTable showv = Conexion.Consultar("nfactura as [NumFact], vendedor as Usuario, fechaventa as Fecha, total as Importe, estadoventa as Estado, tipoFactura as Factura", "Ventas", " order by nfactura desc", "", new SqlCeCommand());

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
                DataTable showacls = Conexion.Consultar("idcliente,nombre as [Nombre],atencion as [Atencion],direccion as Domicilio,telefono as Telefono,mail as Email, cuit as CUIT", "Clientes", "", "", new SqlCeCommand());
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
                DataTable showprovs = Conexion.Consultar("idproveedor,nombre as [Nombre],atencion as [Atencion],telefono as Telefono,mail as [Email],direccion as [Direccion],localidad as Localidad,cp as CP", "Proveedores", "", "", new SqlCeCommand());
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
                DataTable showv = Conexion.Consultar("nfactura as [NFactura], vendedor as Vendedor, fechacompra as Fecha,proveedor as Proveedor, totalfactura as Total,estadocompra as Estado ", "Compras", " order by nfactura desc", "", new SqlCeCommand());

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
                DataTable arts = Conexion.Consultar("idarticulo,descripcion as [Nombre],proveedor as [Proveedor],marca as [Marca],precio as [Precio],costo as [Costo],stockactual as [Stock],stockminimo as [StockMinimo],compraminima as [CompraMinima]", "Articulos","WHERE eliminado != 'Eliminado' and tipo LIKE 'Producto%'", " order by proveedor asc", new SqlCeCommand());
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
                 for (int i = 0; i < dataGridView1.Rows.Count; i++)
                 {
                     dataGridView1.Rows[i].Cells[9].Value = " ";
                 }
            }
            if (Conexion.data == "AutoPedido")
            {
                Conexion.abrir();
                DataTable arts = Conexion.Consultar("idarticulo,descripcion as [Nombre],proveedor as [Proveedor],marca as [Marca],precio as [Precio],costo as [Costo],stockactual as [Stock],stockminimo as [StockMinimo],compraminima as [CompraMinima], stockmax as [StockMaximo]", "Articulos","WHERE eliminado != 'Eliminado' and tipo LIKE 'Producto%'", " order by proveedor asc",  new SqlCeCommand());
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
                  float stockactual, stockminimo,compraminima,stockmax;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {

                    stockactual = Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value.ToString());
                    stockminimo = Convert.ToInt32(dataGridView1.Rows[i].Cells[7].Value.ToString());
                    compraminima = Convert.ToInt32(dataGridView1.Rows[i].Cells[8].Value.ToString());
                    stockmax = Convert.ToInt32(dataGridView1.Rows[i].Cells[9].Value.ToString());
                    if (stockactual < stockminimo)
                    {
                        if((stockactual + compraminima) > stockmax)
                            dataGridView1.Rows[i].Cells[10].Value = (stockmax - stockactual).ToString();
                        else
                        dataGridView1.Rows[i].Cells[10].Value = compraminima.ToString();
                    }
                    else dataGridView1.Rows[i].Cells[10].Value = "0";
                }
            }
            if (Conexion.data == "Faltantes")
            {
                Conexion.abrir();
                DataTable arts = Conexion.Consultar("idarticulo,descripcion as [Nombre],proveedor as [Proveedor],marca as [Marca],precio as [Precio],costo as [Costo],stockactual as [Stock],stockminimo as [StockMinimo],compraminima as [CompraMinima]", "Articulos", "where stockactual< stockminimo  and eliminado != 'Eliminado' and tipo LIKE 'Producto%'", "order by proveedor asc", new SqlCeCommand());
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
               
              
                if (comboBox1.SelectedItem.ToString() == "Excel")
                {
                    saveFileDialog1.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    extension = ".xlsx";
                }
                if (comboBox1.SelectedItem.ToString() == "TXT")
                {
                    saveFileDialog1.Filter = "Text Files (*.txt)|*.txt";
                    extension = ".txt";
                }
               
                saveFileDialog1.FileName = "Informe_" + Conexion.data + app.hoy.Replace("/","")+DateTime.Now.ToShortTimeString().Replace(":","")+extension;
                saveFileDialog1.ShowDialog();
            }
            else MessageBox.Show("Debe seleccionar un tipo de archivo");
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
                if (extension == ".txt")
                {

                    int columnas = dataGridView1.Columns.Count;
                    //MessageBox.Show(columnas.ToString());
                    string columns = "";
                    string rows = "";
                    for (int i = 0; i < columnas; i++)
                    {
                        columns += dataGridView1.Columns[i].Name.ToString() + "\t\t\t";
                    }
                    File.AppendAllText(saveFileDialog1.FileName, columns + "\r\n");
                    columns = "";
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        for (int j = 0; j < columnas; j++)
                        {
                            rows += dataGridView1.Rows[i].Cells[j].Value.ToString() + "\t\t\t";
                        }
                        File.AppendAllText(saveFileDialog1.FileName, rows + "\r\n");
                        rows = "";
                    }
                }
                if (extension == ".xlsx")
                {
                    try
                    {
                         string tempfilename = saveFileDialog1.FileName;
                         string xConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source="  +     tempfilename +".xls;Extended Properties='Excel 8.0;HDR=YES'";
                         string TabName ="Informe";
                         var conn = new OleDbConnection(xConnStr);
                         string columnas = "";
                         for (int i = 0; i < dataGridView1.Columns.Count; i++)
                         {
                            if(i == 0) columnas += "[" + dataGridView1.Columns[i].Name + "] varchar(255)";
                            else columnas += ", [" + dataGridView1.Columns[i].Name + "] varchar(255)";
                         }
 
                         string ColumnName = columnas;
                         conn.Open();
                         var cmd = new OleDbCommand("CREATE TABLE [" + TabName + "] (" + ColumnName + ")", conn);
                         cmd.ExecuteNonQuery();
                         for (int i = 0; i < dataGridView1.Rows.Count; i++)
                         {
                             string values = "";
                             for (int j = 0; j < dataGridView1.Columns.Count; j++)
                             {
                                 if(j == 0) values += "'" + dataGridView1.Rows[i].Cells[j].Value.ToString() + "'";
                                 else values += ", '" + dataGridView1.Rows[i].Cells[j].Value.ToString() + "'";
                             }
                             var insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)","") +") VALUES ("+values+")", conn);
                             insert.ExecuteNonQuery();
                         }
                         conn.Close();

 
                    }
                    catch (Exception erm)
                    {
                        this.Close();
                        MessageBox.Show("Hubo un error al exportar a excel:\n"+erm.Message);
                    }
                }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Conexion.data == "Ventas")
                {
                    var bd = dataGridView1.DataSource;
                    var dt = (DataTable)bd;
                    string formatstring = "";
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i == 0) formatstring += " CONVERT([" + dt.Columns[i].ColumnName + "],System.String) like '%{0}%' ";
                        else formatstring += " or CONVERT([" + dt.Columns[i].ColumnName + "],System.String) like '%{0}%' ";
                    }

                    //MessageBox.Show(formatstring);
                    dt.DefaultView.RowFilter = string.Format(formatstring, textBox1.Text.Trim().Replace("'", "''"));
                    dataGridView1.Refresh();
                }
                else
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
            }
            catch (Exception) { }
        }

        private void Informe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

           
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
        }
    }
}
