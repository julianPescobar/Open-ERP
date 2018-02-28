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
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }
        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;
        private void Gastos_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                       this.DisplayRectangle);   
        }

        private void Gastos_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = Convert.ToDateTime(app.hoy + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":59");
            SqlCeCommand dates = new SqlCeCommand();
            dates.Parameters.AddWithValue("d2", dateTimePicker1.Value.Month);
            dates.Parameters.AddWithValue("d3", dateTimePicker1.Value.Year);
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
            if(comboBox1.SelectedIndex >= 0 && textBox1.Text.Length > 0 && textBox2.Text.Length > 0)
            {
                SqlCeCommand gasto = new SqlCeCommand();
                gasto.Parameters.AddWithValue("fe", dateTimePicker1.Value);
                gasto.Parameters.AddWithValue("ti", comboBox1.SelectedItem.ToString());
                gasto.Parameters.AddWithValue("mo", textBox1.Text);
                gasto.Parameters.AddWithValue("im", textBox2.Text.Replace("$",""));
                Conexion.abrir();
                Conexion.Insertar("Gastos", "fecha,area,descripcion,importe", "@fe,@ti,@mo,@im", gasto);
                Conexion.cerrar();
                if (Application.OpenForms.OfType<Gastos>().Count() == 1)
                    Application.OpenForms.OfType<Gastos>().First().Close();

                    Gastos frm = new Gastos();
                    frm.ShowDialog();
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
            dates.Parameters.AddWithValue("d2", dateTimePicker1.Value.Month);
            dates.Parameters.AddWithValue("d3", dateTimePicker1.Value.Year);
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
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text.Replace(".", ",");
            textBox2.SelectionStart = textBox2.Text.Length;
        }
    }
}
