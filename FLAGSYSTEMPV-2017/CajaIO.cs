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
    public partial class CajaIO : Form
    {
        public CajaIO()
        {
            InitializeComponent();
        }

        private void CajaIO_Load(object sender, EventArgs e)
        {
            maskedTextBox1.Text = Convert.ToDateTime(app.hoy).ToShortDateString();
            float total = 0;
            textBox2.Text = total.ToString("0.00");
            if (CIO.entradaosalida == "Entrada")
            {
                CajaIO.ActiveForm.Text = "Entradas de caja";
                button1.Text = "Agregar Entrada";
                button2.Text = "Ver Entradas";
            }
            if (CIO.entradaosalida == "Salida")
            {
                CajaIO.ActiveForm.Text = "Salidas de Caja";
                button1.Text = "Agregar Salida";
                button2.Text = "Ver Salidas";
            }
        }


        

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
       

                try
                {
                    float total = float.Parse(textBox2.Text);
                    textBox2.Text = total.ToString("$0.00");
                }
                catch (Exception)
                {
                    textBox2.Text = "";
                }
            
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length < 1 || textBox1.Text.Length < 1)
            {
                MessageBox.Show("Debe completar el motivo y/o el precio");
            }
            else
            {
                DateTime fecha = Convert.ToDateTime(maskedTextBox1.Text);
                string motivo = textBox1.Text;
                float monto = float.Parse(textBox2.Text.Replace("$", ""));
                SqlCeCommand inserto = new SqlCeCommand();

                inserto.Parameters.AddWithValue("fe",fecha);
                inserto.Parameters.AddWithValue("mot",motivo);
                inserto.Parameters.AddWithValue("mon",monto);
                if (CIO.entradaosalida == "Entrada")
                {
                    inserto.Parameters.AddWithValue("tipo", "EC");
                    Conexion.abrir();
                    Conexion.Insertar("EntradaCaja", "tipo,fecha,motivo,total", "@tipo,@fe,@mot,@mon", inserto);
                    Conexion.cerrar();
                }
                if (CIO.entradaosalida == "Salida")
                {
                    inserto.Parameters.AddWithValue("tipo", "SC");
                    Conexion.abrir();
                    Conexion.Insertar("SalidaCaja", "tipo,fecha,motivo,total", "@tipo,@fe,@mot,@mon", inserto);
                    Conexion.cerrar();
                }
                this.Close();
                if (Application.OpenForms.OfType<CajaIO>().Count() == 1)
                    Application.OpenForms.OfType<CajaIO>().First().Dispose();
                CajaIO cajaio = new CajaIO();
                cajaio.Show();
                
            }
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (Application.OpenForms.OfType<VerCajaIO>().Count() == 1)
                Application.OpenForms.OfType<VerCajaIO>().First().Dispose();
            VerCajaIO abrirventa = new VerCajaIO();
            abrirventa.Show();
        }

        private void CajaIO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

            if (e.KeyCode == Keys.Enter)
                SendKeys.SendWait("{TAB}");
            if (e.KeyCode == Keys.F1)
                button1.PerformClick();

            if (e.KeyCode == Keys.F2)
                button2.PerformClick();

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
                DateTime fecha = Convert.ToDateTime(maskedTextBox1.Text);
                maskedTextBox1.Text = fecha.ToShortDateString();
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

     
    }
}
