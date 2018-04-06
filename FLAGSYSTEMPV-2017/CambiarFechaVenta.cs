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
    public partial class CambiarFechaVenta : Form
    {
        public CambiarFechaVenta()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var bd = (BindingSource)dataGridView1.DataSource;
                var dt = (DataTable)bd.DataSource;
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

        private void CambiarFechaVenta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
            if (e.KeyCode == Keys.F1) textBox1.Select();
            if (e.KeyCode == Keys.F2) maskedTextBox1.Select();
            if (e.KeyCode == Keys.F3) button2.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CambiarFechaVenta_Load(object sender, EventArgs e)
        {
            string usuario = registereduser.reguser;
            SqlCeCommand consultar = new SqlCeCommand();
            consultar.Parameters.AddWithValue("ven",usuario);
            Conexion.abrir();
            DataTable fechas = Conexion.Consultar("idventa as id, vendedor as Vendedor, fechaventa as Fecha, total as Total, estadoventa as Estado, tipoFactura as Tipo", "Ventas", "where estadoventa != 'Anulada'", "order by fechaventa DESC", consultar);
            Conexion.cerrar();
            BindingSource SBind = new BindingSource();
            SBind.DataSource = fechas;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = fechas;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.DataSource = SBind;
            dataGridView1.Refresh();
            if (fechas.Rows.Count > 0)
            {

            }
        }

        private void maskedTextBox1_Leave(object sender, EventArgs e)
        {
            try
            {
                DateTime fecha = Convert.ToDateTime(maskedTextBox1.Text);
                maskedTextBox1.Text = fecha.ToShortDateString();
            }
            catch (Exception) {
                maskedTextBox1.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text.Length > 6 && dataGridView1.Rows.Count > 0)
            {

                DataGridViewSelectedRowCollection selectedItems = dataGridView1.SelectedRows;
                if (selectedItems.Count > 1)
                {
                    List<string> todoslosrows = new List<string>();
                    string message = "";
                    string fecha = maskedTextBox1.Text;
                    foreach (DataGridViewRow dgrow in selectedItems)
                    {
                        todoslosrows.Add(dgrow.Cells["id"].Value.ToString());
                        
                    }
                    DialogResult borrar = MessageBox.Show("Está seguro de cambiar la fecha de toda esta cantidad de ventas? (" + todoslosrows.Count + " ventas) por esta fecha?(" + fecha + ")", "Esta seguro de cambiar la fecha a todas estas ventas?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (borrar == DialogResult.Yes)
                    {
                        Conexion.abrir();
                        for (int i = 0; i < todoslosrows.Count; i++)
                        {
                            SqlCeCommand cambiofecha = new SqlCeCommand();
                            cambiofecha.Parameters.Clear();
                            cambiofecha.Parameters.AddWithValue("id", todoslosrows[i].ToString());
                            cambiofecha.Parameters.AddWithValue("newfecha", fecha);
                           
                            Conexion.Actualizar("Ventas", "fechaventa = @newfecha", "WHERE idventa = @id", "", cambiofecha);
                          
                        }
                        Conexion.cerrar();
                        MessageBox.Show("Las "+todoslosrows.Count+" ventas han sido cambiadas de fecha exitosamente");
                        if (Application.OpenForms.OfType<CambiarFechaVenta>().Count() > 0)
                        {
                            Application.OpenForms.OfType<CambiarFechaVenta>().First().Close();
                        }
                        CambiarFechaVenta fmr = new CambiarFechaVenta();
                        fmr.Show();
                    }
                }
                else
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    var row = this.dataGridView1.Rows[rowIndex];
                    string name = row.Cells["Fecha"].Value.ToString();
                    string id = row.Cells["id"].Value.ToString();
                    string fecha = maskedTextBox1.Text;
                    DialogResult borrar = MessageBox.Show("Está seguro de cambiar la fecha de esta venta (" + name + ") por esta fecha?(" + fecha + ")", "Esta seguro de cambiar la fecha?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (borrar == DialogResult.Yes)
                    {
                        SqlCeCommand cambiofecha = new SqlCeCommand();
                        cambiofecha.Parameters.AddWithValue("id", id);
                        cambiofecha.Parameters.AddWithValue("newfecha", fecha);
                        Conexion.abrir();
                        Conexion.Actualizar("Ventas", "fechaventa = @newfecha", "WHERE idventa = @id", "", cambiofecha);
                        Conexion.cerrar();
                        MessageBox.Show("La venta ha sido cambiada de fecha exitosamente");
                        if (Application.OpenForms.OfType<CambiarFechaVenta>().Count() > 0)
                        {
                            Application.OpenForms.OfType<CambiarFechaVenta>().First().Close();
                        }
                        CambiarFechaVenta fmr = new CambiarFechaVenta();
                        fmr.Show();
                    }
                }
            }
            else MessageBox.Show("No se pudo cambiar la fecha. Revise la venta seleccionada y la fecha ingresada");

        }
    }
}
