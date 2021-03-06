﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using FiscalPrinterLib;
namespace FLAGSYSTEMPV_2017
{
    public partial class Compras : Form
    {
        public Compras()
        {
            InitializeComponent();
        }
        public string totventa;
        private void Compras_Load(object sender, EventArgs e)
        {
            textBox4.Select();
            totalventa.compraoventa = "Compras";
            if (dataGridView1.Rows.Count == 0)
            {
                float cero = 0;
                textBox5.Text = cero.ToString("$0.00");
            }
            Conexion.abrir();
            DataTable proveedores = Conexion.Consultar("nombre", "Proveedores", "WHERE Eliminado != 'Eliminado'", "", new SqlCeCommand());
            Conexion.cerrar();
            for (int i = 0; i < proveedores.Rows.Count; i++)
            {
                comboBox1.Items.Add(proveedores.Rows[i][0].ToString());
            }
            comboBox1.DroppedDown = true;
            comboBox1.Select();
            dateTimePicker1.Value = Convert.ToDateTime(app.hoy+" "+DateTime.Now.Hour.ToString()+":"+DateTime.Now.Minute.ToString()+":"+DateTime.Now.Second.ToString());
            Conexion.abrir();
            DataTable nextid = new DataTable();
            nextid = Conexion.Consultar("AUTOINC_NEXT", "INFORMATION_SCHEMA.COLUMNS", " WHERE (TABLE_NAME = 'Compras') AND (COLUMN_NAME = 'idcompra')", "", new SqlCeCommand());
            Conexion.cerrar();
            textBox1.Text = nextid.Rows[0][0].ToString();
            totalventa.idcompra = Convert.ToInt32(nextid.Rows[0][0].ToString());
            if (Demo.EsDemo == true) textBox2.Text = Demo.demouser;
            else textBox2.Text = registereduser.reguser;
            float idv = float.Parse(textBox1.Text);
            if (idv >= 100 && Demo.EsDemo == true)
            {
                this.Close();
                MessageBox.Show("Lo sentimos pero esta es la versión demo del producto y solo se permiten ingresar hasta 100 registros de compra. Si quiere habilitar esta opcion debe comprar la licencia.");

            }
           
            
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            if (Application.OpenForms.OfType<Inicio>().Count() == 1)
                Application.OpenForms.OfType<Inicio>().First().Focus();
        }

        

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    int  Cantidad = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[1].Value);
                    float costo = float.Parse(dataGridView1.Rows[rowIndex].Cells[7].Value.ToString().Replace("$", ""));
                    if (Cantidad > 0)
                    {
                        dataGridView1.Rows[rowIndex].Cells[1].Value = (Cantidad + 1).ToString();
                        dataGridView1.Rows[rowIndex].Cells[6].Value = ((Cantidad + 1) * costo).ToString("$0.00") ;
                        chequeartotal();
                    }
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    int Cantidad = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[1].Value);
                    float costo = float.Parse(dataGridView1.Rows[rowIndex].Cells[7].Value.ToString().Replace("$", ""));

                    if (Cantidad > 1)
                    {
                        dataGridView1.Rows[rowIndex].Cells[1].Value = (Cantidad - 1).ToString();
                        dataGridView1.Rows[rowIndex].Cells[6].Value = ((Cantidad - 1) * costo).ToString("$0.00");
                        chequeartotal();
                    }
                }
            }
           
            if (e.KeyCode == Keys.Up)
            {
                try
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    dataGridView1.Rows[rowIndex -1].Cells[1].Selected = true;
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
            
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView1.Rows.Count < 1 && textBox4.Text.Length == 0) MessageBox.Show("No hay ningún artículo para comprar");
                if (dataGridView1.Rows.Count > 0 && textBox4.Text.Length == 0)
                {

                    DataTable dt = new DataTable(); //agarramos el gridview y le metemos los datos a un datatable.
                    //no hay forma mejor que hacerlo de esta manera porque no funciona referenciando el DT del dgridview

                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        dt.Columns.Add("column" + i.ToString());
                    }
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            dr["column" + j.ToString()] = row.Cells[j].Value;
                        }

                        dt.Rows.Add(dr);
                    }
                    totalventa.detallecompra = dt;
                    totalventa.totcompra = textBox5.Text;
                    totalventa.fechacompra = dateTimePicker1.Value.ToShortDateString() + " " + dateTimePicker1.Value.ToShortTimeString();
                    Total total = new Total();
                    total.Show();
                   

                }//mostrar la pantalla que ingresa plata clente
                if (textBox4.Text.Length > 0)
                {
                    if (comboBox1.SelectedIndex >= 0)
                    {
                        totalventa.proveedcompra = comboBox1.SelectedItem.ToString();
                        Conexion.abrir();
                        SqlCeCommand metocodigo = new SqlCeCommand();
                        metocodigo.Parameters.AddWithValue("@cod", textBox4.Text);
                        metocodigo.Parameters.AddWithValue("@pro", totalventa.proveedcompra);
                        DataTable producto = Conexion.Consultar("*", "Articulos", "WHERE codigoart = @cod and proveedor = @pro", "", metocodigo);
                        Conexion.cerrar();
                        bool yaesta = false;
                        if (producto.Rows.Count > 0)
                        {
                            
                            for(int i = 0; i < dataGridView1.Rows.Count;i++)
                            {
                                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == producto.Rows[0][0].ToString())
                                {
                                    dataGridView1.Rows[i].Cells[1].Value = int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString())+1;
                                    dataGridView1.Rows[i].Cells[6].Value = (float.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()) * float.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString().Replace("$",""))).ToString("$0.00");
                                    yaesta = true;
                                    textBox4.Text = "";
                                    chequeartotal();
                                }
                            }
                            if (yaesta == false)
                            {
                                int cantidad = 1;
                                string idproducto = producto.Rows[0][0].ToString();
                                string codigo = producto.Rows[0][1].ToString();
                                string desc = producto.Rows[0][11].ToString();
                                string mca = producto.Rows[0][2].ToString();
                                float prec = float.Parse(producto.Rows[0][4].ToString());
                                float costo = float.Parse(producto.Rows[0][5].ToString());
                                float iva = float.Parse(producto.Rows[0][6].ToString());
                                float porcentaje = float.Parse(producto.Rows[0][12].ToString());
                                //añadir al table el costo y porcentaje y esconderlos, para asi poder hacer el porcentaje de mierda ese.
                                float total = costo * cantidad;
                                dataGridView1.Rows.Add(idproducto, cantidad, codigo, desc, mca, prec.ToString("$0.00"), total.ToString("$0.00"), costo.ToString("$0.00"), iva.ToString(), porcentaje.ToString());
                                textBox4.Text = "";
                                dataGridView1.Rows[(dataGridView1.Rows.Count - 1)].Cells[1].Selected = true;
                                //MessageBox.Show(costo.ToString() + "-" + porcentaje.ToString() );
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se ha encontrado el artículo");
                            textBox4.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe seleccionar proveedor primero");
                        textBox4.Text = "";
                        comboBox1.Select();
                        comboBox1.DroppedDown = true;
                    }
                }//chequear si existe el codigo de ese producto y de ser asi agregarlo al DGV.
            }
            if (e.KeyCode == Keys.F5)
            {
                //abrir busqueda de articulo
                if (comboBox1.SelectedIndex >= 0)
                {
                    totalventa.proveedcompra = comboBox1.SelectedItem.ToString();
                    totalventa.codprodbuscado = "";
                    Buscarticulo bus = new Buscarticulo();
                    bus.ShowDialog();
                    if (totalventa.codprodbuscado != "")
                    {

                        Conexion.abrir();
                        SqlCeCommand metocodigo = new SqlCeCommand();
                        metocodigo.Parameters.AddWithValue("@cod", totalventa.codprodbuscado);
                        DataTable producto = Conexion.Consultar("idarticulo,codigoart,descripcion,marca,precio,costo,iva,porcentaje", "Articulos", "WHERE codigoart = @cod", "", metocodigo);
                        Conexion.cerrar();
                        bool yaesta = false;
                        if (producto.Rows.Count > 0)
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == producto.Rows[0][0].ToString())
                                {
                                    dataGridView1.Rows[i].Cells[1].Value = int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()) + 1;
                                    dataGridView1.Rows[i].Cells[6].Value = (float.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()) * float.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString().Replace("$", ""))).ToString("$0.00");
                                    yaesta = true;
                                    textBox4.Text = "";
                                    chequeartotal();
                                }
                            }
                            if (yaesta == false)
                            {
                                int cantidad = 1;
                                string idproducto = producto.Rows[0][0].ToString();
                                string codigo = producto.Rows[0][1].ToString();
                                string desc = producto.Rows[0][2].ToString();
                                string mca = producto.Rows[0][3].ToString();
                                float prec = float.Parse(producto.Rows[0][5].ToString());
                                float total = prec * cantidad;
                                float costo = float.Parse(producto.Rows[0][5].ToString());
                                float iva = float.Parse(producto.Rows[0][6].ToString());
                                float porcentaje = float.Parse(producto.Rows[0][7].ToString());
                                dataGridView1.Rows.Add(idproducto, cantidad, codigo, desc, mca, prec.ToString("$0.00"), total.ToString("$0.00"), costo.ToString("$0.00"), iva.ToString(), porcentaje.ToString());
                                textBox4.Text = "";
                                dataGridView1.Rows[(dataGridView1.Rows.Count - 1)].Cells[1].Selected = true;
                                chequeartotal();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Para cargar articulos, debe seleccionar un proveedor");
                    comboBox1.DroppedDown = true;
                }
            }
            if (e.KeyCode == Keys.F4 && dataGridView1.Rows.Count > 0)
            {
                //abrir busqueda de articulo
                IngreseUnidades ing = new IngreseUnidades();
                ing.ShowDialog();
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                decimal Cantidad = totalventa.cantidad;
                float costo = float.Parse(dataGridView1.Rows[rowIndex].Cells[7].Value.ToString().Replace("$", ""));
                if (Cantidad > 0)
                {
                    dataGridView1.Rows[rowIndex].Cells[1].Value = Cantidad.ToString();
                    dataGridView1.Rows[rowIndex].Cells[6].Value = ((float.Parse(Cantidad.ToString())  * costo).ToString("$0.00"));
                    chequeartotal();
                }
            }
            if (e.KeyCode == Keys.F3 && dataGridView1.Rows.Count > 0)
            {
                //abrir busqueda de articulo
                IngreseMonto ing = new IngreseMonto();
                ing.ShowDialog();
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                float Monto = totalventa.montocompra;
               
                if (Monto > 0)
                {
                    float Cantidad = float.Parse(dataGridView1.Rows[rowIndex].Cells[1].Value.ToString());
                    dataGridView1.Rows[rowIndex].Cells[6].Value = ((Cantidad * Monto).ToString("$0.00"));
                    dataGridView1.Rows[rowIndex].Cells[7].Value = Monto.ToString("$0.00");
                    chequeartotal();
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;

                    dataGridView1.Rows.Remove(dataGridView1.Rows[rowIndex]);
                    
                }
            }
        }
        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            chequeartotal();
        }
        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            chequeartotal();
        }

        void chequeartotal()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                float cero = 0;
                textBox5.Text = cero.ToString("$0.00");
            }
            else
            {
                float totalgral = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var row = this.dataGridView1.Rows[i];
                    float total = float.Parse(row.Cells["Total"].Value.ToString().Replace("$", ""));
                    totalgral += total;
                }
                textBox5.Text = totalgral.ToString("$0.00");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox4.Focus();
            MessageBox.Show("Enter: Finaliza Compra (si se cargaron articulos)\nEscape: Cancela toda la compra si hay articulos cargados, de lo contrario cierra la ventana\nAbajo: Mueve el cursor de la lista de productos hacia abajo (si hay articulos cargados)\nArriba: Idem anterior pero para arriba.\nIzquierda: Disminuye en 1 las unidades del producto seleccionado.\nDerecha Aumenta en 1 las unidades del producto seleccionado.\nF5: Abre panel de busqueda de articulos.\nF4: abre una ventana en donde se puede escribir cuantas unidades asignar a un articulo.\nF3: Modifica el Costo de un producto (es util cuando el precio del proveedor varia sobre lo que figura en el sistema)","Atajos del teclado",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void rectangleShape1_Click(object sender, EventArgs e)
        {
            textBox4.Focus();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox4.Focus();
        }
        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox4.Focus();
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox4.Focus();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            textBox4.Focus();
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            textBox4.Focus();

            FiscalPrinterLib.HASAR prntr = new FiscalPrinterLib.HASAR();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
         
            DateTime fechadatabase = Convert.ToDateTime(app.hoy + " " + dateTimePicker1.Value.ToShortTimeString());
            if (Demo.EsDemo == false)
            {
                if (DateTime.Compare(dateTimePicker1.Value, fechadatabase) < 0 && registereduser.sololectura == "no")
                {

                    MessageBox.Show("No se puede ingresar una fecha anterior porque la opción \"Permitir cambios en fechas anteriores\" esta desactivada. Habilite esa opción si necesita cargar una compra con fecha anterior, esta opcion se encuentra en Administrador > Configuración.", "No se pudo cambiar la fecha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dateTimePicker1.Value = fechadatabase;
                }
            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            textBox4.Focus();
        }

        private void Compras_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Está seguro de cancelar la compra?", "Atención",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {

                        this.Close();
                        Compras frm = new Compras();
                        frm.Show();
                      
                       
                    }
                }
                else
                {
                    this.Close();
                    if (Application.OpenForms.OfType<Inicio>().Count() == 1)
                        Application.OpenForms.OfType<Inicio>().First().Focus();
                }
            }
           
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab) MessageBox.Show("test");
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1 && comboBox1.Enabled == true) MessageBox.Show("Tabulador deshabilitado para este control.");
        }

      

    }
}
