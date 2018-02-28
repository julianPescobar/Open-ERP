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
    public partial class AgregarAltabaja : Form
    {
        public AgregarAltabaja()
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
            if (Application.OpenForms.OfType<NuevoArticulo>().Count() == 1)
                Application.OpenForms.OfType<NuevoArticulo>().First().Focus();
            else
            {
                NuevoArticulo frm = new NuevoArticulo();
                frm.ShowDialog();
            }
        }

        void getarts()
        {
            Conexion.abrir();
            DataTable showarts = Conexion.Consultar("idarticulo,codigoart as [Codigo de Articulo],descripcion as [Descripcion del Articulo],proveedor as Proveedor,precio as Precio,costo as Costo,stockactual as Stock,stockminimo as [Stock Minimo],iva as IVA", "Articulos", "WHERE eliminado != 'Eliminado' and tipo NOT LIKE 'Servicio%'", "", new SqlCeCommand());
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
            }
            else
            {
                label2.Visible = true; //mostramos que no hay registros  
                button1.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                numericUpDown2.Enabled = false;
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0 &&  comboBox2.SelectedIndex >= 0 && numericUpDown2.Value > 0)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                string pid = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                string cod = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
                string des = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
                SqlCeCommand nuevaaltabaja = new SqlCeCommand();
                nuevaaltabaja.Parameters.AddWithValue("prid", pid);
                nuevaaltabaja.Parameters.AddWithValue("vend", registereduser.reguser);
                nuevaaltabaja.Parameters.AddWithValue("codi", cod);
                nuevaaltabaja.Parameters.AddWithValue("desc", des);
                nuevaaltabaja.Parameters.AddWithValue("cantidad", numericUpDown2.Value);
                nuevaaltabaja.Parameters.AddWithValue("moti", comboBox1.SelectedItem.ToString());
                nuevaaltabaja.Parameters.AddWithValue("fecha", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                Conexion.abrir();
                if (comboBox2.SelectedItem.ToString() == "Alta")
                {
                    Conexion.Insertar("Altasbajas", "fecha,codigo,descripcion,altas,bajas,motivo,vendedor", "@fecha,@codi,@desc,@cantidad,'0',@moti, @vend", nuevaaltabaja);
                    Conexion.Actualizar("Articulos", "stockactual = (stockactual + @cantidad)", "WHERE idarticulo = @prid","", nuevaaltabaja);

                }
                else
                {
                    Conexion.Insertar("Altasbajas", "fecha,codigo,descripcion,bajas,altas,motivo,vendedor", "@fecha,@codi,@desc,@cantidad,'0',@moti, @vend", nuevaaltabaja);
                    Conexion.Actualizar("Articulos", "stockactual = (stockactual - @cantidad)", "WHERE idarticulo = @prid", "", nuevaaltabaja);

                }
                Conexion.cerrar();
                this.Close();
                if (Application.OpenForms.OfType<ABStock>().Count() == 1)
                Application.OpenForms.OfType<ABStock>().First().Close();
                ABStock frm = new ABStock();
                frm.Show();
            }
            else MessageBox.Show("Debe completar todos los datos para poder cargar un alta o baja de stock");
        }

        private void AgregarAltabaja_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
    }
}
