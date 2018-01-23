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
    public partial class DiferenciaStock : Form
    {
        public DiferenciaStock()
        {
            InitializeComponent();
        }
        private void DiferenciaStock_Load(object sender, EventArgs e)
        {
            Conexion.abrir();
            DataTable proveedores = Conexion.Consultar("nombre", "Proveedores", "", "", new SqlCeCommand());
            Conexion.cerrar();
            for (int i = 0; i < proveedores.Rows.Count; i++)
            {
                comboBox1.Items.Add(proveedores.Rows[i][0].ToString());
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
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void DiferenciaStock_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                         this.DisplayRectangle);      
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getarts(comboBox1.SelectedItem.ToString());
        }
        void getarts(string prov)
        {
            float faltante, sobrante, diferencia;
            Conexion.abrir();
            SqlCeCommand proveedor = new SqlCeCommand();
            proveedor.Parameters.AddWithValue("pr", prov);
            DataTable showarts = Conexion.Consultar("idarticulo,descripcion as [Descripcion del Articulo],proveedor as Proveedor,precio as Precio,costo as Costo,stockactual as Stock,faltante as [Faltante],sobrante as Sobrante", "Articulos", " WHERE proveedor = @pr", "", proveedor);
            Conexion.cerrar();
            BindingSource SBind = new BindingSource();
            SBind.DataSource = showarts;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = showarts;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[3].DefaultCellStyle.Format = "c";
            dataGridView1.Columns[4].DefaultCellStyle.Format = "c";
            dataGridView1.DataSource = SBind;
            dataGridView1.Refresh();
            if (showarts.Rows.Count > 0)
            {
                button2.Enabled = true; //mostramos que no hay registros
                button1.Enabled = true;
                float tsob, tfal;
                sobrante = 0;
                faltante = 0;
                tsob = 0;
                tfal = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    tfal += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString().Replace("$","")) * int.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());
                    tsob += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString().Replace("$","")) * int.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString());
                    
                }
                textBox1.Text = tfal.ToString("$0.00");
                textBox2.Text = tsob.ToString("$0.00");
                textBox3.Text = (tsob - tfal).ToString("$0.00");
            }
            else
            {
                button2.Enabled = false; //mostramos que no hay registros
                button1.Enabled = false;
                faltante = 0;
                sobrante = 0;
                diferencia = faltante - sobrante;
            }
        }
    }
}
