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
    public partial class Caja : Form
    {
        public Caja()
        {
            InitializeComponent();
        }

       
       

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            if (Application.OpenForms.OfType<Inicio>().Count() == 1)
                Application.OpenForms.OfType<Inicio>().First().Focus();
        }

        private void Caja_Load(object sender, EventArgs e)
        {
            maskedTextBox1.Text = app.hoy;
            maskedTextBox2.Text = app.hoy;
            //dateTimePicker2.Text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            getdebehaber();
            calculardht();
            getTotal();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            getdebehaber();
            calculardht();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            getdebehaber();
            calculardht();
        }

        void calculardht()
        {
            if (dataGridView2.Rows.Count > 0) //haber
            {
               
                    float total = 0;
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        var row = this.dataGridView2.Rows[i];
                        float t = float.Parse(row.Cells[2].Value.ToString().Replace("$", ""));
                        total += t;
                    }
                    textBox2.Text = total.ToString("$0.00");
                
            }
            else
            {
                float total = 0;
                textBox2.Text = total.ToString("$0.00");
                textBox2.BackColor = Color.Gold;
            }
            
            if (dataGridView1.Rows.Count > 0) //debe
            {

                float total = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var row = this.dataGridView1.Rows[i];
                    float t = float.Parse(row.Cells["Total"].Value.ToString().Replace("$", ""));
                    total += t;
                }
                textBox3.Text = total.ToString("$0.00");

            }
            else
            {
                float total = 0;
                textBox3.Text = total.ToString("$0.00");
            }
        }
       
        void getdebehaber()
        {
            
            SqlCeCommand dates = new SqlCeCommand();
            dates.Parameters.Clear();
            dates.Parameters.AddWithValue("d1", Convert.ToDateTime(maskedTextBox1.Text+" 00:00:00"));
            dates.Parameters.AddWithValue("d2", Convert.ToDateTime(maskedTextBox2.Text + " 23:59:59"));
            dates.Parameters.AddWithValue("an", "Anulada");
            Conexion.abrir();
            DataTable showdebe = Conexion.Consultar("area as [Tipo], descripcion as Motivo, importe as Total", "Gastos", "WHERE fecha BETWEEN @d1 AND @d2;", "", dates);
            DataTable showhaber = Conexion.Consultar("tipoFactura as [Tipo], nfactura as [N°] ,total as [Importe]", "Ventas", " WHERE estadoventa != @an AND fechaventa BETWEEN @d1 AND @d2 ;", "", dates);
            DataTable addIngresos = Conexion.Consultar("tipo,motivo,total", "EntradaCaja", "WHERE fecha BETWEEN @d1 AND @d2;", "", dates);
            DataTable addSalidas = Conexion.Consultar("tipo,motivo,total", "SalidaCaja", " WHERE fecha BETWEEN @d1 AND @d2;", "", dates);
            
            Conexion.cerrar();
            for (int i = 0; i < addIngresos.Rows.Count; i++)
            {
                string tipo = addIngresos.Rows[i][0].ToString();
                string motivo = addIngresos.Rows[i][1].ToString();
                string importe = addIngresos.Rows[i][2].ToString();
                showhaber.Rows.Add(tipo+"("+motivo+")", 0, importe);
            }

            BindingSource SBind = new BindingSource();
            SBind.DataSource = showhaber;
            dataGridView2.AutoGenerateColumns = true;
            dataGridView2.DataSource = showhaber;
            dataGridView2.Columns[2].DefaultCellStyle.Format = "c";
            dataGridView2.DataSource = SBind;
            dataGridView2.Refresh();

            for (int i = 0; i < addSalidas.Rows.Count; i++)
            {
                string tipo = addSalidas.Rows[i][0].ToString();
                string motivo = addSalidas.Rows[i][1].ToString();
                float importe = float.Parse(addSalidas.Rows[i][2].ToString());
                showdebe.Rows.Add(tipo, motivo, importe.ToString("0.00"));
            }
            BindingSource SBind2 = new BindingSource();
            SBind2.DataSource = showdebe;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = showdebe;
            dataGridView1.Columns[2].DefaultCellStyle.Format = "c";
            dataGridView1.DataSource = SBind2;
            dataGridView1.Refresh();
            
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void getTotal()
        {
            SqlCeCommand notdeleted = new SqlCeCommand();
            notdeleted.Parameters.AddWithValue("an", "Anulada");
            Conexion.abrir();
            DataTable ventas = Conexion.Consultar("SUM(total)", "Ventas", "WHERE estadoventa != @an", "", notdeleted);
            DataTable entrada = Conexion.Consultar("SUM(total)", "EntradaCaja", "", "", new SqlCeCommand());
            DataTable salida = Conexion.Consultar("SUM(total)", "SalidaCaja", "", "", new SqlCeCommand());
            DataTable gastos = Conexion.Consultar("SUM(importe)", "Gastos", "", "", new SqlCeCommand());
            Conexion.cerrar();
            float saldoinicial;

            if (Demo.EsDemo == true)
            saldoinicial = 0;
            else
            saldoinicial = registereduser.saldoinicial;
            float tventas, tentrada, tgastos, tsalida;
            if (ventas.Rows[0][0].ToString().Length < 1) tventas = 0; else tventas = float.Parse(ventas.Rows[0][0].ToString());
            if (entrada.Rows[0][0].ToString().Length < 1) tentrada = 0; else tentrada = float.Parse(entrada.Rows[0][0].ToString());
            if (gastos.Rows[0][0].ToString().Length < 1) tgastos = 0; else tgastos = float.Parse(gastos.Rows[0][0].ToString());
            if (salida.Rows[0][0].ToString().Length < 1) tsalida = 0; else tsalida = float.Parse(salida.Rows[0][0].ToString());

            float haber = tventas +tentrada;
            float debe =tgastos +tsalida;
            textBox1.Text = (saldoinicial + haber - debe).ToString("$0.00");
            if (float.Parse(textBox1.Text.ToString().Replace("$", "")) >= 0)
            {
                textBox1.BackColor = Color.LightGreen;
            }
            else
            {
                textBox1.BackColor = Color.IndianRed;
            }
        }
        private void dateTimePicker2_ValueChanged_1(object sender, EventArgs e)
        {
            getdebehaber();
            calculardht();
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                if (Application.OpenForms.OfType<Inicio>().Count() == 1)
                    Application.OpenForms.OfType<Inicio>().First().Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Informe>().Count() == 1)
                Application.OpenForms.OfType<Informe>().First().Focus();
            else
            {
                Informe frm = new Informe();
                Conexion.data = "Caja";
                Conexion.desde = Convert.ToDateTime(maskedTextBox1.Text+ " 00:00:00").ToString();
                Conexion.hasta = Convert.ToDateTime(maskedTextBox2.Text + " 23:59:59").ToString();
                frm.Show();
            }
        }

        private void Caja_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                if (Application.OpenForms.OfType<Inicio>().Count() == 1)
                    Application.OpenForms.OfType<Inicio>().First().Focus();
            }
                if (e.KeyCode == Keys.Enter && button1.Focused == false && button2.Focused == false)
                SendKeys.SendWait("{TAB}");
            if (e.KeyCode == Keys.F1) maskedTextBox1.Focus();
            if (e.KeyCode == Keys.F2) maskedTextBox2.Focus();
            if (e.KeyCode == Keys.F3)
            {
                if (maskedTextBox1.Text.Length > 6 && maskedTextBox2.Text.Length > 6)
                    button1.PerformClick();
                else
                    MessageBox.Show("Para abrir el informe debe completar las dos fechas");
            }
        }

        private void maskedTextBox1_Leave(object sender, EventArgs e)
        {
             try
            {
                DateTime lafecha = DateTime.Parse(maskedTextBox1.Text);
                maskedTextBox1.Text = lafecha.ToShortDateString();
            getdebehaber();
            calculardht();
            }
             catch (Exception)
             {
                 maskedTextBox1.Text = app.hoy;
                 getdebehaber();
                 calculardht();
             }
        }

        private void maskedTextBox2_Leave(object sender, EventArgs e)
        {

            try
            {
                DateTime lafecha = DateTime.Parse(maskedTextBox2.Text);
                maskedTextBox2.Text = lafecha.ToShortDateString();
                getdebehaber();
                calculardht();
            }
            catch (Exception)
            {
                maskedTextBox2.Text = app.hoy;
                getdebehaber();
                calculardht();
            }
        }

        private void maskedTextBox1_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate { SetMaskedTextBoxSelectAll((MaskedTextBox)sender); });
        }
        private void SetMaskedTextBoxSelectAll(MaskedTextBox txtbox)
        {
            txtbox.SelectAll();
            txtbox.Clear();
        }

        private void maskedTextBox2_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate { SetMaskedTextBoxSelectAll((MaskedTextBox)sender); });
        }
      
    }
}
