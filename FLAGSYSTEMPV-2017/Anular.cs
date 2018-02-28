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
            if (ConfigFiscal.usaImpFiscal == "no" || Demo.EsDemo == true)
            {
                this.Focus();
                label1.Text = "Listado de Ventas de hoy";
                Conexion.abrir();
                SqlCeCommand fechahoy = new SqlCeCommand();
                fechahoy.Parameters.AddWithValue("hoy", app.hoy);
                if(Demo.EsDemo == true)
                    fechahoy.Parameters.AddWithValue("ven", Demo.demouser);
                else
                fechahoy.Parameters.AddWithValue("ven", registereduser.reguser);
                fechahoy.Parameters.AddWithValue("anulada", "Anulada");
                DataTable showv = new DataTable();
                if(Demo.EsDemo == true || registereduser.level == "Admin" || registereduser.level == "Supervisor")
                showv = Conexion.Consultar("nfactura as [N° Fact.], vendedor as Usuario, fechaventa as Fecha, total as Importe, estadoventa as Estado, tipoFactura as Factura", "Ventas", "WHERE datediff(day,fechaventa,@hoy) =  0 AND estadoventa = 'Finalizado' ", " order by nfactura desc", fechahoy);
                else
                    showv = Conexion.Consultar("nfactura as [N° Fact.], vendedor as Usuario, fechaventa as Fecha, total as Importe, estadoventa as Estado, tipoFactura as Factura", "Ventas", "WHERE datediff(day,fechaventa,@hoy) =  0 AND estadoventa = 'Finalizado' and vendedor =@ven ", " order by nfactura desc", fechahoy);
                
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
            else
            {
                MessageBox.Show("Solo se pueden anular ventas cuando el sistema no hace uso de la facturación fiscal");
                    this.Close();
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
            try
            {


                var bd = dataGridView1.DataSource;
                var dt = (DataTable)bd;
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
              DataTable detalle =  Conexion.Consultar("idproducto,cantidproducto,tipo","DetalleVentas","where nfactura = @id","", anular);
              for (int i = 0; i < detalle.Rows.Count; i++)
              {
                  string idprod = detalle.Rows[i][0].ToString();
                  string cantidad = detalle.Rows[i][1].ToString();
                  string tipo = detalle.Rows[i][2].ToString();
                  SqlCeCommand stockreturn = new SqlCeCommand();
                  stockreturn.Parameters.Clear();
                  stockreturn.Parameters.AddWithValue("id", idprod);
                  stockreturn.Parameters.AddWithValue("x", cantidad);
                  if (tipo.Contains("Producto"))
                      Conexion.Actualizar("Articulos", "stockactual = (stockactual + @x) ", "WHERE idarticulo = @id", "", stockreturn);
              }
               Conexion.cerrar();
               Anular refresh = new Anular();
               refresh.Show();
               refresh.Focus();
               this.Close();
           }
        }

        private void Anular_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
      
    }
}
