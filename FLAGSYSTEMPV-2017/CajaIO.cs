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
            float total = 0;
            textBox2.Text = total.ToString("0.00");
            if (CIO.entradaosalida == "Entrada")
            {
                button1.Text = "Agregar Entrada";
                button2.Text = "Ver Entradas";
            }
            if (CIO.entradaosalida == "Salida")
            {
                button1.Text = "Agregar Salida";
                button2.Text = "Ver Salidas";
            }
        }

             protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        private void CajaIO_Paint(object sender, PaintEventArgs e)
        {
               e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                        this.DisplayRectangle);      
        
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
                DateTime fecha = dateTimePicker1.Value;
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
                    Application.OpenForms.OfType<CajaIO>().First().Close();
                CajaIO cajaio = new CajaIO();
                cajaio.ShowDialog();
                
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
                Application.OpenForms.OfType<VerCajaIO>().First().Close();
            VerCajaIO abrirventa = new VerCajaIO();
            abrirventa.ShowDialog();
        }

     
    }
}
