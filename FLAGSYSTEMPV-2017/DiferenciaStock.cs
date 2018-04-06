using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;
using System.Data.OleDb;
namespace FLAGSYSTEMPV_2017
{
    public partial class DiferenciaStock : Form
    {
        public DiferenciaStock()
        {
            InitializeComponent();
        }
        public string extension = "";
        private void DiferenciaStock_Load(object sender, EventArgs e)
        {
            Conexion.abrir();
            DataTable proveedores = Conexion.Consultar("nombre", "Proveedores", "WHERE Eliminado != 'Eliminado'", "", new SqlCeCommand());
            Conexion.cerrar();
            for (int i = 0; i < proveedores.Rows.Count; i++)
            {
                comboBox1.Items.Add(proveedores.Rows[i][0].ToString());
            }
        }
     
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getarts(comboBox1.SelectedItem.ToString());
            }
            catch (Exception) { }
        }
        void getarts(string prov)
        {
            float faltante, sobrante, diferencia;
            Conexion.abrir();
            SqlCeCommand proveedor = new SqlCeCommand();
            proveedor.Parameters.AddWithValue("pr", prov);
            DataTable showarts = Conexion.Consultar("idarticulo,descripcion as [Descripcion del Articulo],proveedor as Proveedor,precio as Precio,costo as Costo,stockactual as Stock,faltante as [Faltante],sobrante as Sobrante", "Articulos", " WHERE proveedor = @pr and eliminado != 'Eliminado' and tipo LIKE 'Producto%'", "", proveedor);
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

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult seguro = MessageBox.Show("Está seguro de poner faltantes y sobrantes en cero?", "Seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (seguro == DialogResult.Yes)
            {
                if (dataGridView1.Rows.Count > 0 && comboBox1.SelectedIndex >= 0)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        SqlCeCommand id = new SqlCeCommand();
                        id.Parameters.AddWithValue("id", dataGridView1.Rows[i].Cells[0].Value.ToString());
                        Conexion.abrir();
                        Conexion.Actualizar("Articulos", "faltante = '0', sobrante = '0'", "where idarticulo = @id", "", id);
                        Conexion.cerrar();
                    }
                    if (Application.OpenForms.OfType<DiferenciaStock>().Count() == 1)
                        Application.OpenForms.OfType<DiferenciaStock>().First().Close();
                    DiferenciaStock frm = new DiferenciaStock();
                    frm.Show();
                    frm.BringToFront();
                }
                else
                {
                    MessageBox.Show("Debe elegir un provedor y asegurarse de que posea articulos");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0 && dataGridView1.Rows.Count > 0 && comboBox2.SelectedIndex >= 0)
            {

                if (comboBox2.SelectedItem.ToString() == "Excel")
                {
                    saveFileDialog1.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    extension = ".xlsx";
                }
                if (comboBox2.SelectedItem.ToString() == "TXT")
                {
                    saveFileDialog1.Filter = "Text Files (*.txt)|*.txt";
                    extension = ".txt";
                }

                saveFileDialog1.FileName = "DiferenciaStock" + Conexion.data + DateTime.Now.ToShortDateString().Replace("/", "") + DateTime.Now.ToShortTimeString().Replace(":", "") + extension;
                saveFileDialog1.ShowDialog();
            }
            else MessageBox.Show("Para exportar debe seleccionar un tipo de archivo y un proveedor.");
        }
        private void copyAlltoClipboard()
        {
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (extension == ".txt")
            {

                int columnas = dataGridView1.Columns.Count;
                //MessageBox.Show(columnas.ToString());
                string columns = "";
                string rows = "";
                for (int i = 0; i < columnas; i++)
                {
                    columns += dataGridView1.Columns[i].Name.ToString() + "\t\t\t";
                }
                File.AppendAllText(saveFileDialog1.FileName, columns + "\r\n");
                columns = "";
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < columnas; j++)
                    {
                        rows += dataGridView1.Rows[i].Cells[j].Value.ToString() + "\t\t\t";
                    }
                    File.AppendAllText(saveFileDialog1.FileName, rows + "\r\n");
                    rows = "";
                }
            }
            if (extension == ".xlsx")
            {
                try
                {

                    string tempfilename = saveFileDialog1.FileName;
                    string xConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + tempfilename + ".xls;Extended Properties='Excel 8.0;HDR=YES'";

                    string TabName = "Informe";
                    var conn = new OleDbConnection(xConnStr);
                    string columnas = "";
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        if (i == 0) columnas += "[" + dataGridView1.Columns[i].Name + "] varchar(255)";
                        else columnas += ", [" + dataGridView1.Columns[i].Name + "] varchar(255)";
                    }
                    //MessageBox.Show(columnas);
                    string ColumnName = columnas;
                    conn.Open();
                    var cmd = new OleDbCommand("CREATE TABLE [" + TabName + "] (" + ColumnName + ")", conn);
                    cmd.ExecuteNonQuery();
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        string values = "";
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            if (j == 0) values += "'" + dataGridView1.Rows[i].Cells[j].Value.ToString() + "'";
                            else values += ", '" + dataGridView1.Rows[i].Cells[j].Value.ToString() + "'";
                        }
                        var insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)", "") + ") VALUES (" + values + ")", conn);
                        insert.ExecuteNonQuery();
                    }
                    conn.Close();

                }
                catch (Exception ee)
                {
                    this.Close();
                    MessageBox.Show("Hubo un error al querer exportar a excel:\n"+ee.Message);
                }
            }
        }

        private void DiferenciaStock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();


            if (e.KeyCode == Keys.F1)
            {
                comboBox1.DroppedDown = true;
                comboBox1.Select();
            }

            if (e.KeyCode == Keys.F2)
            {
                comboBox2.DroppedDown = true;
                comboBox2.Select();
            }

            if (e.KeyCode == Keys.F3)
            {
                button1.PerformClick();
            }

            if (e.KeyCode == Keys.F4)
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
