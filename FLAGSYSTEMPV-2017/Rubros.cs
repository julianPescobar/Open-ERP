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
             if (Application.OpenForms.OfType<Inicio>().Count() >= 1)
                    
                        Application.OpenForms.OfType<Inicio>().First().Focus();
        }

        private void Articulos_Load(object sender, EventArgs e)
        {
            getarts();
            textBox1.Select();
            this.Select(); 
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
            if (dataGridView1.Rows.Count > 0)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                var row = this.dataGridView1.Rows[rowIndex];
                string name = row.Cells[1].Value.ToString();
                string id = row.Cells[0].Value.ToString();

                SqlCeCommand existen = new SqlCeCommand();
                existen.Parameters.AddWithValue("nom", name);
                existen.Parameters.AddWithValue("activo", "Activo");
                Conexion.abrir();
                DataTable hayprovs = Conexion.Consultar("*", "Articulos", "where rubro = @nom and Eliminado = @activo", "", existen);
                DataTable hayproveeds = Conexion.Consultar("*", "Proveedores", "where rubro = @nom and Eliminado = @activo", "", existen);
                Conexion.cerrar();
                if (hayprovs.Rows.Count > 0 || hayproveeds.Rows.Count > 0)
                {
                    MessageBox.Show("Hay " + hayprovs.Rows.Count.ToString() + " Articulo(s) y "+hayproveeds.Rows.Count.ToString()+" Proveedor(es) asignados bajo este rubro actualmente. Para poder eliminar el rubro primero debe cambiar de rubro a los productos y volver a intentar", "No se puede borrar un rubro activo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult borrar = MessageBox.Show("Está seguro de borrar este rubro?\n" + name, "Borrar?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (borrar == DialogResult.Yes)
                    {
                        Conexion.abrir();
                        SqlCeCommand del = new SqlCeCommand();
                        del.Parameters.AddWithValue("@id", id);
                        del.Parameters.AddWithValue("@el", "Eliminado");
                        Conexion.Actualizar("Rubros", "eliminado = @el", "WHERE idrubro = @id", "", del);
                        Conexion.cerrar();
                    }
                    getarts();
                }
            }
            else MessageBox.Show("No hay ningún rubro seleccionado para borrar");
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
                if (Application.OpenForms.OfType<Inicio>().Count() >= 1)

                    Application.OpenForms.OfType<Inicio>().First().Focus();

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

        private void Rubros_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

            if (e.KeyCode == Keys.F1 && button1.Enabled == true)
                button1.PerformClick();
            if (e.KeyCode == Keys.F2 && button2.Enabled == true)
                button2.PerformClick();
          
           
            if (e.KeyCode == Keys.F3)
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
