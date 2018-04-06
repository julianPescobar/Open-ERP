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
    public partial class Gastos : Form
    {
        public Gastos()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
      
       
        private void Gastos_Load(object sender, EventArgs e)
        {

            maskedTextBox1.Text = app.hoy;
            SqlCeCommand dates = new SqlCeCommand();
            dates.Parameters.AddWithValue("d2", Convert.ToDateTime(app.hoy+" 00:00:00").Month);
            dates.Parameters.AddWithValue("d3", Convert.ToDateTime(app.hoy+" 00:00:00").Year);
            Conexion.abrir();
            DataTable showdebe = Conexion.Consultar("area as [Area], descripcion as Motivo, importe as Total", "Gastos", "WHERE DATEPART(month,fecha) = @d2 AND DATEPART(year,fecha) = @d3;", "", dates);
            Conexion.cerrar();
            if (showdebe.Rows.Count > 0)
            {
                float toto = 0;
                for (int i = 0; i < showdebe.Rows.Count; i++)
                {
                    toto += float.Parse(showdebe.Rows[i][2].ToString());
                }
                textBox3.Text = toto.ToString("$0.00");
            }
            else
            {
                float tot = 0;
                textBox3.Text = tot.ToString("$0.00");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex >= 0 && textBox1.Text.Length > 0 && textBox2.Text.Length > 0 )
            {
                string fechaingresada = maskedTextBox1.Text;
                SqlCeCommand gasto = new SqlCeCommand();
                gasto.Parameters.AddWithValue("fe", Convert.ToDateTime(fechaingresada+" 00:00:00"));
                gasto.Parameters.AddWithValue("ti", comboBox1.SelectedItem.ToString());
                gasto.Parameters.AddWithValue("mo", textBox1.Text);
                gasto.Parameters.AddWithValue("im", textBox2.Text.Replace("$",""));
                Conexion.abrir();
                Conexion.Insertar("Gastos", "fecha,area,descripcion,importe", "@fe,@ti,@mo,@im", gasto);
                Conexion.cerrar();
                if (Application.OpenForms.OfType<Gastos>().Count() == 1)
                    Application.OpenForms.OfType<Gastos>().First().Close();
                    Gastos frm = new Gastos();
                    frm.Show();
            }
            else
            MessageBox.Show("Debe completar todos los datos para poder agregar el gasto");
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            try
            {
                float importe = float.Parse(textBox2.Text.ToString());
                textBox2.Text = importe.ToString("$0.00");
            }
            catch (Exception)
            {
                textBox2.Text = "";
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SqlCeCommand dates = new SqlCeCommand();
            dates.Parameters.AddWithValue("d2", Convert.ToDateTime(maskedTextBox1).Month);
            dates.Parameters.AddWithValue("d3", Convert.ToDateTime(maskedTextBox1).Year);
            Conexion.abrir();
            DataTable showdebe = Conexion.Consultar("area as [Area], descripcion as Motivo, importe as Total", "Gastos", "WHERE DATEPART(month,fecha) = @d2 AND DATEPART(year,fecha) = @d3;", "", dates);
            Conexion.cerrar();
            if (showdebe.Rows.Count > 0)
            {
                float toto = 0;
                for (int i = 0; i < showdebe.Rows.Count; i++)
                {
                    toto += float.Parse(showdebe.Rows[i][2].ToString());
                }
                textBox3.Text = toto.ToString("$0.00");
            }
            else
            {
                float tot = 0;
                textBox3.Text = tot.ToString("$0.00");
            }
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
          
        }

        private void Gastos_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape) this.Close();
            if (e.KeyCode == Keys.F1) button1.PerformClick();
            if (e.KeyCode == Keys.F2) button3.PerformClick();
            if (e.KeyCode == Keys.Enter && button1.Focused == false && button2.Focused == false) SendKeys.SendWait("{TAB}"); 
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text.Replace(".", ",");
            textBox2.SelectionStart = textBox2.Text.Length;
        }

        private void maskedTextBox1_Leave(object sender, EventArgs e)
        {
            try
            {
                SqlCeCommand dates = new SqlCeCommand();
                string fechaingresada = maskedTextBox1.Text;
                dates.Parameters.AddWithValue("d2", Convert.ToDateTime(fechaingresada + " 00:00:00").Month);
                dates.Parameters.AddWithValue("d3", Convert.ToDateTime(fechaingresada + " 00:00:00").Year);
                Conexion.abrir();
                DataTable showdebe = Conexion.Consultar("area as [Area], descripcion as Motivo, importe as Total", "Gastos", "WHERE DATEPART(month,fecha) = @d2 AND DATEPART(year,fecha) = @d3;", "", dates);
                Conexion.cerrar();
                if (showdebe.Rows.Count > 0)
                {
                    float toto = 0;
                    for (int i = 0; i < showdebe.Rows.Count; i++)
                    {
                        toto += float.Parse(showdebe.Rows[i][2].ToString());
                    }
                    textBox3.Text = toto.ToString("$0.00");
                }
                else
                {
                    float tot = 0;
                    textBox3.Text = tot.ToString("$0.00");
                }
            }
            catch (Exception)
            {
                maskedTextBox1.Text = app.hoy;
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Informe>().Count() == 1)
                Application.OpenForms.OfType<Informe>().First().Focus();
            else
            {
                Informe frm = new Informe();
                Conexion.data = "Gastos";
                frm.Show();
            }
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            comboBox1.DroppedDown = true;
            comboBox1.Select();
        }

       
    }
}
