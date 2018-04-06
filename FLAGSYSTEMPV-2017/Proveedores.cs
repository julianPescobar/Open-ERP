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
    public partial class Proveedores : Form
    {
        public Proveedores()
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
            if (Application.OpenForms.OfType<NuevoProveedor>().Count() == 1)
                Application.OpenForms.OfType<NuevoProveedor>().First().Focus();
            else
            {
                NuevoProveedor frm = new NuevoProveedor();
                frm.ShowDialog();
            }
        }

        private void Proveedores_Load(object sender, EventArgs e)
        {
            getprovs();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                var row = this.dataGridView1.Rows[rowIndex];
                string name = row.Cells["Nombre del Proveedor"].Value.ToString();
                string id = row.Cells["idproveedor"].Value.ToString();
                SqlCeCommand enuso = new SqlCeCommand();
                enuso.Parameters.AddWithValue("prov", name);
                enuso.Parameters.AddWithValue("activo", "Activo");
                Conexion.abrir();
                DataTable artsconeseprov = Conexion.Consultar("*", "Articulos", "Where proveedor = @prov and eliminado = @activo", "", enuso);
                Conexion.cerrar();
                if (artsconeseprov.Rows.Count > 0)
                {
                    MessageBox.Show("Hay " + artsconeseprov.Rows.Count.ToString() + " Articulos asignados bajo este Proveedor actualmente. Para poder eliminar el Proveedor primero debe cambiar de Proveedor a los Articulos y volver a intentar", "No se puede borrar un Proveedor activo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult borrar = MessageBox.Show("Está seguro de borrar a este proveedor?\n" + name, "Borrar?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (borrar == DialogResult.Yes)
                    {
                        Conexion.abrir();
                        SqlCeCommand del = new SqlCeCommand();
                        del.Parameters.AddWithValue("@id", id);
                        del.Parameters.AddWithValue("@el", "Eliminado");
                        //Conexion.Eliminar("Proveedores", "idproveedor = @id", del);
                        Conexion.Actualizar("Proveedores", "Eliminado = @el", "WHERE idproveedor = @id","", del);
                        Conexion.cerrar();

                    }
                    getprovs();
                }
            }
            else MessageBox.Show("No hay ningun proveedor seleccionado para borrar");
        }

        void getprovs()
        {
            Conexion.abrir();
            DataTable showprovs = Conexion.Consultar("idproveedor,nombre as [Nombre del Proveedor],atencion as [Atencion],telefono as Telefono,mail as [Correo Electrónico],direccion as [Direccion],localidad as Localidad,cp as CP", "Proveedores", "where Eliminado != 'Eliminado'", "", new SqlCeCommand());
            Conexion.cerrar();
            BindingSource SBind = new BindingSource();
            SBind.DataSource = showprovs;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = showprovs;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.DataSource = SBind;
            dataGridView1.Refresh();

            if (showprovs.Rows.Count > 0)
            {

                dataGridView1.DataSource = showprovs; //mostramos lo que hay
                button2.Enabled = true;
                button9.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button9.Enabled = false;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var bd = dataGridView1.DataSource;
            var dt = (DataTable)bd;
            dt.DefaultView.RowFilter = string.Format("[Nombre del Proveedor] like '%{0}%' or [Atencion] like '%{0}%'  or [Direccion] like '%{0}%' or [Telefono] like '%{0}%' or [CP] like '%{0}%'", textBox1.Text.Trim().Replace("'", "''"));
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
            if (dataGridView1.Rows.Count > 0)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                createorupdate.itemid = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                createorupdate.status = "update";
                if (Application.OpenForms.OfType<NuevoProveedor>().Count() == 1)
                    Application.OpenForms.OfType<NuevoProveedor>().First().Focus();
                else
                {
                    NuevoProveedor frm = new NuevoProveedor();
                    frm.ShowDialog();
                }
            }
            else MessageBox.Show("No hay ningun proveedor seleccionado para edición");
        }

        private void Proveedores_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1 && button1.Enabled == true)
                button1.PerformClick();

            if (e.KeyCode == Keys.F2 && button9.Enabled == true)
                button9.PerformClick();

            if (e.KeyCode == Keys.F3 && button2.Enabled == true)
                button2.PerformClick();

            if (e.KeyCode == Keys.F4) textBox1.Select();

            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                if (Application.OpenForms.OfType<Inicio>().Count() == 1)
                    Application.OpenForms.OfType<Inicio>().First().Focus();
            }
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
