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
            if (Application.OpenForms.OfType<Inicio>().Count() == 1)
                Application.OpenForms.OfType<Inicio>().First().Select();
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
            DataTable showacls = Conexion.Consultar("idcliente,nombre as [Nombre del cliente],atencion as [Atencion],direccion as Domicilio,telefono as Telefono,mail as Email, cuit as CUIT", "Clientes", "where eliminado != 'Eliminado'", "", new SqlCeCommand());
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
            if (dataGridView1.Rows.Count > 0)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                var row = this.dataGridView1.Rows[rowIndex];
                string name = row.Cells["Nombre del cliente"].Value.ToString();
                string id = row.Cells["idcliente"].Value.ToString();

                DialogResult borrar = MessageBox.Show("Está seguro de borrar este Cliente?\n" + name, "Borrar?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (borrar == DialogResult.Yes)
                {
                    Conexion.abrir();
                    SqlCeCommand del = new SqlCeCommand();
                    del.Parameters.AddWithValue("@id", id);
                    del.Parameters.AddWithValue("@el", "Eliminado");
                    //Conexion.Eliminar("Clientes", "idcliente = @id", del);
                    Conexion.Actualizar("Clientes", "eliminado = @el", "where idcliente = @id", "", del);
                    Conexion.abrir();
                    DataTable showacls = Conexion.Consultar("idcliente,nombre as [Nombre del cliente],atencion as [Atencion],direccion as Domicilio,telefono as Telefono,mail as Email, cuit as CUIT", "Clientes", "where eliminado != 'Eliminado'", "", new SqlCeCommand());
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
            else MessageBox.Show("No hay ningun cliente seleccionado para borrar");
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
                if (Application.OpenForms.OfType<Inicio>().Count() > 0)
                {
                    Application.OpenForms.OfType<Inicio>().First().Focus();
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                createorupdate.itemid = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                createorupdate.status = "update";
                if (Application.OpenForms.OfType<NuevoCliente>().Count() == 1)
                    Application.OpenForms.OfType<NuevoCliente>().First().Focus();
                else
                {
                    NuevoCliente frm = new NuevoCliente();
                    frm.ShowDialog();
                }
            }
            else MessageBox.Show("No hay ningun cliente seleccionado para edición");
        }

        private void Clientes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                if (Application.OpenForms.OfType<Inicio>().Count() == 1)
                    Application.OpenForms.OfType<Inicio>().First().Focus();
            }

            if (e.KeyCode == Keys.F1 && button1.Enabled == true)
                button1.PerformClick();
            if (e.KeyCode == Keys.F2 && button9.Enabled == true)
                button9.PerformClick();
            if (e.KeyCode == Keys.F3 && button2.Enabled == true)
                button2.PerformClick();
            
            if (e.KeyCode == Keys.F4)
                textBox1.Select();

            if (e.KeyCode == Keys.Up && dataGridView1.Focused == false)
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
            if (e.KeyCode == Keys.Down && dataGridView1.Focused == false)
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
