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
    public partial class Rubros : Form
    {
        public Rubros()
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
            
            if (Application.OpenForms.OfType<NuevoRubro>().Count() == 1)
                Application.OpenForms.OfType<NuevoRubro>().First().Focus();
            else
            {
                NuevoRubro frm = new NuevoRubro();
                frm.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            var row = this.dataGridView1.Rows[rowIndex];
            string name = row.Cells[1].Value.ToString();
            string id = row.Cells[0].Value.ToString();

            DialogResult borrar = MessageBox.Show("Está seguro de borrar este rubro?\n"+name, "Borrar?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (borrar == DialogResult.Yes)
            {
                Conexion.abrir();
                SqlCeCommand del = new SqlCeCommand();
                del.Parameters.AddWithValue("@id", id);
                del.Parameters.AddWithValue("@el", "Eliminado");
                Conexion.Actualizar("Rubros","eliminado = @el", "WHERE idrubro = @id","", del);
                 Conexion.cerrar();
            }
            getarts();
        }

        void getarts()
        {
            SqlCeCommand elim = new SqlCeCommand();
            elim.Parameters.AddWithValue("el","Eliminado");
            Conexion.abrir();
            DataTable showarts = Conexion.Consultar("idrubro,nombrerubro as [Nombre]", "Rubros", "WHERE eliminado != @el", "", elim);
            Conexion.cerrar();
            BindingSource SBind = new BindingSource();
            SBind.DataSource = showarts;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = showarts;
            dataGridView1.Columns[0].Visible = false;
            
            dataGridView1.DataSource = SBind;
            dataGridView1.Refresh();

            if (showarts.Rows.Count > 0)
            {
                label2.Visible = false; //sacamos label
                dataGridView1.DataSource = showarts; //mostramos lo que hay
                button2.Enabled = true;
               
            
                //button6.Enabled = true;
               
               
                //button9.Enabled = true;
               
            }
            else
            {
            
                label2.Visible = true; //mostramos que no hay registros
                button2.Enabled = false;
               // button9.Enabled = false;
              
                
                //button6.Enabled = false;
              
               
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
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

       
    }
}
