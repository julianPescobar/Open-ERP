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
    public partial class Articulos : Form
    {
        public Articulos()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Articulos_Load(object sender, EventArgs e)
        {
            getarts();

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

        private void Articulos_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                           this.DisplayRectangle);      
        }

        private void button1_Click(object sender, EventArgs e)
        {
            createorupdate.status = "create";
            if (Application.OpenForms.OfType<NuevoArticulo>().Count() == 1)
                Application.OpenForms.OfType<NuevoArticulo>().First().Focus();
            else
            {
                NuevoArticulo frm = new NuevoArticulo();
                frm.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            var row = this.dataGridView1.Rows[rowIndex];
            string name = row.Cells[2].Value.ToString();
            string id = row.Cells[0].Value.ToString();

            DialogResult borrar = MessageBox.Show("Está seguro de borrar este artículo?\n"+name, "Borrar?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (borrar == DialogResult.Yes)
            {
                Conexion.abrir();
                SqlCeCommand del = new SqlCeCommand();
                del.Parameters.AddWithValue("@id", id);
                del.Parameters.AddWithValue("@el", "Eliminado");
                Conexion.Actualizar("Articulos","eliminado = @el", "WHERE idarticulo = @id","", del);
                 Conexion.cerrar();
            }
            getarts();
        }

        void getarts()
        {
            SqlCeCommand elim = new SqlCeCommand();
            elim.Parameters.AddWithValue("el","Eliminado");
            Conexion.abrir();
            DataTable showarts = Conexion.Consultar("idarticulo,codigoart as [Codigo prod.],descripcion as [Descripcion del Articulo],proveedor as Proveedor,precio as [Pr Venta],costo as [Pr Compra],stockactual as Stock,stockminimo as [Stock Minimo],iva as IVA", "Articulos", "WHERE eliminado != @el", "", elim);
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

            if (showarts.Rows.Count > 0)
            {
                label2.Visible = false; //sacamos label
                dataGridView1.DataSource = showarts; //mostramos lo que hay
                button2.Enabled = true;
               
                button5.Enabled = true;
                //button6.Enabled = true;
               
               
                button9.Enabled = true;
                Conexion.abrir();
                DataTable valorizacion = Conexion.Consultar("precio,costo,stockactual", "Articulos", "WHERE eliminado != 'Eliminado'", "", new SqlCeCommand());
                Conexion.cerrar();
                float precio = 0;
                float costo = 0;
                for (int i = 0; i < valorizacion.Rows.Count; i++)
                {
                    precio += float.Parse(valorizacion.Rows[i][0].ToString()) * float.Parse(valorizacion.Rows[i][2].ToString());
                    costo += float.Parse(valorizacion.Rows[i][1].ToString()) * float.Parse(valorizacion.Rows[i][2].ToString());
                    }
                textBox3.Text = precio.ToString("$0.00");
                textBox2.Text = costo.ToString("$0.00");
            }
            else
            {
                textBox2.Text = "$0.00";
                textBox3.Text = "$0.00";
                label2.Visible = true; //mostramos que no hay registros
                button2.Enabled = false;
                button9.Enabled = false;
              
                button5.Enabled = false;
                //button6.Enabled = false;
              
               
            }
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

                //MessageBox.Show(formatstring);
                dt.DefaultView.RowFilter = string.Format(formatstring, textBox1.Text.Trim().Replace("'", "''"));
                dataGridView1.Refresh();
            }
            catch (Exception)
            {
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            createorupdate.itemid = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
           
            createorupdate.status = "update";
            if (Application.OpenForms.OfType<NuevoArticulo>().Count() == 1)
                Application.OpenForms.OfType<NuevoArticulo>().First().Focus();
            else
            {
                NuevoArticulo frm = new NuevoArticulo();
                frm.ShowDialog();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            createorupdate.itemid = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
            if (Application.OpenForms.OfType<ActualizaPrecios>().Count() == 1)
                Application.OpenForms.OfType<ActualizaPrecios>().First().Focus();
            else
            {
                ActualizaPrecios frm = new ActualizaPrecios();
                frm.ShowDialog();
            }
        }
    }
}
