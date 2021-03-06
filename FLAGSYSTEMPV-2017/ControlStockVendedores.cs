﻿using System;
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
    public partial class ControlStockVendedores : Form
    {
        public ControlStockVendedores()
        {
            InitializeComponent();
        }

        private void ControlStockVendedores_Load(object sender, EventArgs e)
        {
            Conexion.abrir();
            DataTable arts = Conexion.Consultar("idarticulo,codigoart as [Codigo],descripcion as [Nombre],marca as [Marca],proveedor as [Proveedor],stockactual as [Stock]", "Articulos", "WHERE eliminado != 'Eliminado' and tipo LIKE 'Producto%'", "order by proveedor", new SqlCeCommand());
            Conexion.cerrar();
            BindingSource SBind = new BindingSource();
            SBind.DataSource = arts;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = arts;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.DataSource = SBind;
            dataGridView1.Refresh();
            dataGridView1.Columns.Add("stockreal", "[F2] StockReal");
            foreach (DataGridViewColumn dc in dataGridView1.Columns)
            {
                if (dc.Index.Equals(6))
                {
                    dc.ReadOnly = false;
                }
                else
                {
                    dc.ReadOnly = true;
                }
            }
            if (arts.Rows.Count < 1) button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
               

                    BindingSource bd = (BindingSource)dataGridView1.DataSource;
                    DataTable dt = (DataTable)bd.DataSource;
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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int input = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
            }
            catch
            {
                dataGridView1.Rows[e.RowIndex].Cells[6].Value = "0";
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.IndianRed;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string teolvidasteuno = "no";
            SqlCeCommand sobfal = new SqlCeCommand();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].DefaultCellStyle.BackColor == Color.IndianRed || dataGridView1.Rows[i].Cells[6].Value == null || dataGridView1.Rows[i].Cells[6].Value.ToString().Length < 1)
                {
                    teolvidasteuno = "si";
                }
            }
            if (teolvidasteuno == "si")
            {
                MessageBox.Show("Usted se ha salteado el ingreso de stock real de uno o varios productos, o ha omitido el ingreso de stock real de algun producto por error. Por favor revise todos los articulos, si la linea es de color rojo debe corregirlo, si la linea es de color verde, no hay problemas","No se puede continuar",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    sobfal.Parameters.Clear();
                    sobfal.Parameters.AddWithValue("id", dataGridView1.Rows[i].Cells[0].Value.ToString());
                    sobfal.Parameters.AddWithValue("stkreal", dataGridView1.Rows[i].Cells[6].Value.ToString());
                    Conexion.abrir();
                    int stockreal = int.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());
                    int stock = int.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                    if (stockreal > stock)
                        Conexion.Actualizar("Articulos", "sobrante = @stkreal - stockactual", "where idarticulo = @id", "", sobfal);
                    if (stockreal < stock)
                        Conexion.Actualizar("Articulos", "faltante =  stockactual - @stkreal", "where idarticulo = @id", "", sobfal);

                }
                Conexion.cerrar();
                MessageBox.Show("El control de stock se ha guardado correctamente y se han generado los faltantes/sobrantes correspondientes","Control stock OK",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool encontrecambios = false;
            Conexion.abrir();
            DataTable refresh = Conexion.Consultar("idarticulo,stockactual", "Articulos", "where eliminado != 'Eliminado'", "", new SqlCeCommand());
            Conexion.cerrar();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                for (int j = 0; j < refresh.Rows.Count; j++)
                {
                    if (id == refresh.Rows[j][0].ToString())
                    {
                        //MessageBox.Show("encontre el id ahora me fijo si el stock cambió o no");
                        string stock = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        int stockactual = int.Parse(stock);
                        int stockrefresh = int.Parse(refresh.Rows[j][1].ToString());
                        if (stockactual != stockrefresh)
                        {
                            dataGridView1.Rows[i].Cells[5].Value = stockrefresh.ToString();
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                            encontrecambios = true;
                        }
                    }
                }
            }
            if (encontrecambios == true)
            {
                MessageBox.Show("Se encontraron cambios en el stock actual de productos, por favor revise las lineas de color amarillo y controle el stock nuevamente acorde al nuevo stock","Se encontraron cambios",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }
      
       
        private void ControlStockVendedores_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

            if (e.KeyCode == Keys.F1)
                textBox1.Focus();

            if (e.KeyCode == Keys.F5)
                button3.PerformClick();

            if (e.KeyCode == Keys.F2)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[6];
                    dataGridView1.Rows[0].Cells[6].Selected = true;
                    dataGridView1.BeginEdit(true);
                }
            }
            if (e.KeyCode == Keys.F3)
            {
                button2.PerformClick();
            }
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
