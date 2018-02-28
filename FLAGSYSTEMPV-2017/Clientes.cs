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
    public partial class Clientes : Form
    {
        public Clientes()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void Clientes_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                         this.DisplayRectangle);      
        }

        private void button1_Click(object sender, EventArgs e)
        {
            createorupdate.status = "create";
            if (Application.OpenForms.OfType<NuevoCliente>().Count() == 1)
                Application.OpenForms.OfType<NuevoCliente>().First().Focus();
            else
            {
                NuevoCliente frm = new NuevoCliente();
                frm.Show();
            }
        }

        private void Clientes_Load(object sender, EventArgs e)
        {
            this.Focus();
              
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
            if (showacls.Rows.Count > 0)
            {
                button2.Enabled = true;
                button9.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button9.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            var row = this.dataGridView1.Rows[rowIndex];
            string name = row.Cells["Nombre del cliente"].Value.ToString();
            string id = row.Cells["idcliente"].Value.ToString();

            DialogResult borrar = MessageBox.Show("Está segudo de borrar este Cliente?\n" + name, "Borrar?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (borrar == DialogResult.Yes)
            {
                Conexion.abrir();
                SqlCeCommand del = new SqlCeCommand();
                del.Parameters.AddWithValue("@id", id);
                Conexion.Eliminar("Clientes", "idcliente = @id", del);
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
                if (showacls.Rows.Count > 0)
                {
                    button2.Enabled = true;
                    button9.Enabled = true;
                }
                else
                {
                    button2.Enabled = false;
                    button9.Enabled = false;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var bd = (BindingSource)dataGridView1.DataSource;
            var dt = (DataTable)bd.DataSource;
            dt.DefaultView.RowFilter = string.Format("[Nombre del cliente] like '%{0}%' or [Atencion] like '%{0}%'  or [Domicilio] like '%{0}%' or [Telefono] like '%{0}%' or [CUIT] like '%{0}%'", textBox1.Text.Trim().Replace("'", "''"));
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
            if (Application.OpenForms.OfType<NuevoCliente>().Count() == 1)
                Application.OpenForms.OfType<NuevoCliente>().First().Focus();
            else
            {
                NuevoCliente frm = new NuevoCliente();
                frm.Show();
            }
        }

        private void Clientes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

            if (e.KeyCode == Keys.F1 && button1.Enabled == true)
                button1.PerformClick();
            if (e.KeyCode == Keys.F2 && button9.Enabled == true)
                button9.PerformClick();
            if (e.KeyCode == Keys.F3 && button2.Enabled == true)
                button2.PerformClick();
            
            if (e.KeyCode == Keys.F5)
                textBox1.Select();

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
