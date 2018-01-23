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
    public partial class NuevoArticulo : Form
    {
        public NuevoArticulo()
        {
            InitializeComponent();
        }

        private void NuevoArticulo_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                         this.DisplayRectangle);
        }

        private void NuevoArticulo_Load(object sender, EventArgs e)
        {
            Conexion.abrir();
            DataTable rubros = Conexion.Consultar("nombrerubro", "Rubros", "", "", new SqlCeCommand());
            DataTable proveedores = Conexion.Consultar("nombre", "Proveedores", "", "", new SqlCeCommand());
            Conexion.cerrar();
            for (int i = 0; i < rubros.Rows.Count; i++)
            {
                comboBox1.Items.Add(rubros.Rows[i][0].ToString());
            }
            for (int i = 0; i < proveedores.Rows.Count; i++)
            {
                comboBox2.Items.Add(proveedores.Rows[i][0].ToString());
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 1 ||
                textBox4.Text.Length < 1 ||
                textBox5.Text.Length < 1 ||
                textBox7.Text.Length < 1 ||
                textBox8.Text.Length < 1 ||
                textBox9.Text.Length < 1 ||
                comboBox1.SelectedIndex  < 0 ||
                comboBox2.SelectedIndex < 0)
            {
                MessageBox.Show("Debe completar los campos con asterisco obligatorios");
            }
            else
            {
                string codigo, descripcion, marca, rubro, precio, costo, iva, stkmin, porcent, compramin, proveed;
                descripcion = textBox2.Text;
                marca = textBox3.Text;
                rubro = comboBox1.SelectedItem.ToString();
                precio = textBox4.Text;
                costo = textBox5.Text;
                iva = textBox6.Text;
                stkmin = textBox7.Text;
                porcent = textBox8.Text;
                compramin = textBox9.Text;
                codigo = textBox1.Text;
                proveed = comboBox2.SelectedItem.ToString();
                SqlCeCommand nuevoarticulo = new SqlCeCommand();
                nuevoarticulo.Parameters.Clear();
                nuevoarticulo.Parameters.AddWithValue("@n", "0");
                nuevoarticulo.Parameters.AddWithValue("@o", "0");
                nuevoarticulo.Parameters.AddWithValue("@a", codigo);
                nuevoarticulo.Parameters.AddWithValue("@b", descripcion);
                nuevoarticulo.Parameters.AddWithValue("@c", marca);
                nuevoarticulo.Parameters.AddWithValue("@d", rubro);
                nuevoarticulo.Parameters.AddWithValue("@e", precio.ToString().Replace("$",""));
                nuevoarticulo.Parameters.AddWithValue("@f", costo.ToString().Replace("$", ""));
                nuevoarticulo.Parameters.AddWithValue("@g", iva);
                nuevoarticulo.Parameters.AddWithValue("@l", "0");
                nuevoarticulo.Parameters.AddWithValue("@h", stkmin);
                nuevoarticulo.Parameters.AddWithValue("@i", porcent);
                nuevoarticulo.Parameters.AddWithValue("@j", compramin);
                nuevoarticulo.Parameters.AddWithValue("@k", proveed);
                Conexion.abrir();
                Conexion.Insertar("Articulos", "faltante,sobrante,codigoart,descripcion,marca,rubro,precio,costo,iva,stockactual,stockminimo,porcentaje,compraminima,proveedor", "@n,@o,@a,@b,@c,@d,@e,@f,@g,@l,@h,@i,@j,@k", nuevoarticulo);
                Conexion.cerrar();
                if (Application.OpenForms.OfType<Articulos>().Count() == 1)
                {
                    Application.OpenForms.OfType<Articulos>().First().Close();
                    Articulos openagain = new Articulos();
                    openagain.Show();
                }
                this.Close();
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            try
            {
                float precio = float.Parse(textBox4.Text.Replace("$", ""));
                textBox4.Text = (precio > 0) ? precio.ToString("$0.00") : "";
                if (textBox4.Text.Length > 0)
                {
                    calcular();
                }
            }
            catch
            {
                textBox4.Text = "";
                label7.Text = "Precio con IVA:";
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            try
            {
                float costo = float.Parse(textBox5.Text.Replace("$",""));
                textBox5.Text = (costo > 0) ? costo.ToString("$0.00") : "" ;
                if (textBox5.Text.Length > 0)
                {
                    calcular();
                }
            }
            catch
            {
                textBox5.Text = "";
            }
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            try
            {
                float iva = float.Parse(textBox6.Text);
                textBox6.Text =  (iva >= 0) ? iva.ToString("0.00") : "";
                if (textBox6.Text.Length > 0)
                {
                    calcular();
                }
            }
            catch
            {
                textBox6.Text = "";
                label7.Text = "Precio con IVA:";
            }
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            try
            {
                int stockmin = int.Parse(textBox7.Text);
                textBox7.Text = (stockmin > 0) ? stockmin.ToString() : "";
                if (textBox7.Text.Length > 0)
                {
                    calcular();
                }
            }
            catch
            {
                textBox7.Text = "";
            }
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            try
            {
                float porcom = float.Parse(textBox8.Text);
                textBox8.Text = (porcom > 0) ? porcom.ToString("0.00") : "";
                if (textBox8.Text.Length > 0)
                {
                    calcular();
                }
            }
            catch
            {
                textBox8.Text = "";
            }
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            try
            {
                int compmin = int.Parse(textBox9.Text);
                textBox9.Text = (compmin > 0) ? compmin.ToString() : "";
                if (textBox9.Text.Length > 0)
                {
                    calcular();
                }
            }
            catch
            {
                textBox9.Text = "";
            }
        }

        void calcular()
        {
            try
            {
                float precio = float.Parse(textBox4.Text.Replace("$", ""));
                float compra = float.Parse(textBox5.Text.Replace("$", ""));
                float ivaprecio = float.Parse(textBox6.Text);
                float ivacompra = float.Parse(textBox8.Text);
                float costomasganancia = compra + (compra * (ivacompra / 100));
                float precioconiva = precio + (precio * (ivaprecio / 100));
                if (costomasganancia > precioconiva)
                {
                    DialogResult wrongprice = MessageBox.Show("El precio de compra del producto mas su porcentaje de ganancia de es mayor al precio de venta + iva que usted ingresó, desea actualizar el precio de venta para que pueda obtener mas ganancia?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (wrongprice == DialogResult.Yes)
                    {
                        textBox4.Text = costomasganancia.ToString("$0.00");
                        float nuevoprecio = costomasganancia + (costomasganancia * (ivaprecio / 100));
                        label7.Text = "Precio con IVA:" + nuevoprecio.ToString("$0.00");

                    }
                }
                else
                {
                    label7.Text = "Precio con IVA:" + (precio + (precio * (ivaprecio /100))).ToString("$0.00");
                }
            }
            catch(Exception)
            {
            }
            
           
           
           
        }
    }
}
