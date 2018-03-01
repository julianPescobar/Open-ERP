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
            if (createorupdate.status == "create")
            {
                Conexion.abrir();
                DataTable rubros = Conexion.Consultar("nombrerubro", "Rubros", "where eliminado = 'Activo'", "", new SqlCeCommand());
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
            if (createorupdate.status == "update")
            {
                SqlCeCommand idprod = new SqlCeCommand();
                idprod.Parameters.AddWithValue("id", createorupdate.itemid);
                Conexion.abrir();
                DataTable rubros = Conexion.Consultar("nombrerubro", "Rubros", "where eliminado = 'Activo'", "", new SqlCeCommand());
                DataTable proveedores = Conexion.Consultar("nombre", "Proveedores", "", "", new SqlCeCommand());
                DataTable datosprod = Conexion.Consultar("codigoart,descripcion,marca,rubro,precio,costo,iva,stockminimo,porcentaje,compraminima,proveedor,tipo,stockmax", "Articulos", "WHERE idarticulo = @id", "", idprod);
                
                Conexion.cerrar();
                for (int i = 0; i < rubros.Rows.Count; i++)
                {
                    comboBox1.Items.Add(rubros.Rows[i][0].ToString());
                }
                for (int i = 0; i < proveedores.Rows.Count; i++)
                {
                    comboBox2.Items.Add(proveedores.Rows[i][0].ToString());
                }
                button1.Text = "Guardar cambios";
                if(datosprod.Rows.Count > 0){
                    textBox1.Text = datosprod.Rows[0][0].ToString();
                    textBox2.Text = datosprod.Rows[0][1].ToString();
                    textBox3.Text = datosprod.Rows[0][2].ToString();
                    comboBox1.SelectedItem = datosprod.Rows[0][3].ToString();
                    textBox4.Text = float.Parse(datosprod.Rows[0][4].ToString()).ToString("$0.00");
                    textBox5.Text = float.Parse(datosprod.Rows[0][5].ToString()).ToString("$0.00");
                    textBox6.Text = float.Parse(datosprod.Rows[0][6].ToString()).ToString("0.00");
                    textBox7.Text = datosprod.Rows[0][7].ToString();
                    textBox8.Text = float.Parse(datosprod.Rows[0][8].ToString()).ToString("0.00");
                    textBox9.Text = datosprod.Rows[0][9].ToString();
                    comboBox2.SelectedItem = datosprod.Rows[0][10].ToString();
                    comboBox3.SelectedItem = datosprod.Rows[0][11].ToString();
                    textBox10.Text = datosprod.Rows[0][12].ToString();
                    calcular();
                }
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
            if (createorupdate.status == "create")
            {
                if (textBox1.Text.Length < 1 ||
                    textBox2.Text.Length < 1 ||
                    textBox4.Text.Length < 1 ||
                    textBox5.Text.Length < 1 ||
                     textBox6.Text.Length < 1 ||
                    textBox7.Text.Length < 1 ||
                    textBox8.Text.Length < 1 ||
                    textBox9.Text.Length < 1 ||
                     textBox10.Text.Length < 1 ||
                    comboBox1.SelectedIndex < 0 ||
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
                    nuevoarticulo.Parameters.AddWithValue("@sm", textBox10.Text.ToString());
                    nuevoarticulo.Parameters.AddWithValue("@n", "0");
                    nuevoarticulo.Parameters.AddWithValue("@o", "0");
                    nuevoarticulo.Parameters.AddWithValue("@a", codigo);
                    nuevoarticulo.Parameters.AddWithValue("@b", descripcion);
                    nuevoarticulo.Parameters.AddWithValue("@c", marca);
                    nuevoarticulo.Parameters.AddWithValue("@d", rubro);
                    nuevoarticulo.Parameters.AddWithValue("@e", precio.ToString().Replace("$", ""));
                    nuevoarticulo.Parameters.AddWithValue("@f", costo.ToString().Replace("$", ""));
                    nuevoarticulo.Parameters.AddWithValue("@g", iva);
                    nuevoarticulo.Parameters.AddWithValue("@l", "0");
                    nuevoarticulo.Parameters.AddWithValue("@h", stkmin);
                    nuevoarticulo.Parameters.AddWithValue("@i", porcent);
                    nuevoarticulo.Parameters.AddWithValue("@j", compramin);
                    nuevoarticulo.Parameters.AddWithValue("@k", proveed);
                    nuevoarticulo.Parameters.AddWithValue("@p", "Activo");
                    nuevoarticulo.Parameters.AddWithValue("@q", comboBox3.SelectedItem.ToString());
                    Conexion.abrir();
                    Conexion.Insertar("Articulos", "stockmax,faltante,sobrante,codigoart,descripcion,marca,rubro,precio,costo,iva,stockactual,stockminimo,porcentaje,compraminima,proveedor,eliminado,tipo", "@sm,@n,@o,@a,@b,@c,@d,@e,@f,@g,@l,@h,@i,@j,@k,@p,@q", nuevoarticulo);
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
            if (createorupdate.status == "update")
            {
                if (textBox1.Text.Length < 1 ||
                    textBox2.Text.Length < 1 ||
                    textBox4.Text.Length < 1 ||
                    textBox5.Text.Length < 1 ||
                     textBox6.Text.Length < 1 ||
                    textBox7.Text.Length < 1 ||
                    textBox8.Text.Length < 1 ||
                    textBox9.Text.Length < 1 ||
                     textBox10.Text.Length < 1 ||
                    comboBox1.SelectedIndex < 0 ||
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
                    nuevoarticulo.Parameters.AddWithValue("@id", createorupdate.itemid);
                    nuevoarticulo.Parameters.AddWithValue("@sm", textBox10.Text.ToString());
                    nuevoarticulo.Parameters.AddWithValue("@a", codigo);
                    nuevoarticulo.Parameters.AddWithValue("@b", descripcion);
                    nuevoarticulo.Parameters.AddWithValue("@c", marca);
                    nuevoarticulo.Parameters.AddWithValue("@d", rubro);
                    nuevoarticulo.Parameters.AddWithValue("@e", precio.ToString().Replace("$", ""));
                    nuevoarticulo.Parameters.AddWithValue("@f", costo.ToString().Replace("$", ""));
                    nuevoarticulo.Parameters.AddWithValue("@g", iva);
                    nuevoarticulo.Parameters.AddWithValue("@h", stkmin);
                    nuevoarticulo.Parameters.AddWithValue("@i", porcent);
                    nuevoarticulo.Parameters.AddWithValue("@j", compramin);
                    nuevoarticulo.Parameters.AddWithValue("@k", proveed);
                    nuevoarticulo.Parameters.AddWithValue("@p", "Activo");
                    nuevoarticulo.Parameters.AddWithValue("@q", comboBox3.SelectedItem.ToString());
                    Conexion.abrir();
                    Conexion.Actualizar("Articulos", " stockmax = @sm, eliminado = @p, tipo = @q, codigoart = @a,descripcion = @b,marca = @c,rubro = @d,precio = @e,costo = @f,iva = @g,stockminimo = @h,porcentaje = @i,compraminima = @j,proveedor = @k",  "WHERE idarticulo = @id", "", nuevoarticulo);

                    //Conexion.Insertar("Articulos", "faltante,sobrante,codigoart,descripcion,marca,rubro,precio,costo,iva,stockactual,stockminimo,porcentaje,compraminima,proveedor", "@n,@o,@a,@b,@c,@d,@e,@f,@g,@l,@h,@i,@j,@k", nuevoarticulo);
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
                textBox8.Text = (porcom >= 0) ? porcom.ToString("0.00") : "";
                if (textBox8.Text.Length >= 0)
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
                if (costomasganancia > precio)
                {
                    DialogResult wrongprice = MessageBox.Show("El precio de compra del producto mas su porcentaje de ganancia de es mayor al precio de venta + iva que usted ingresó, desea actualizar el precio de venta para que pueda obtener mas ganancia?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (wrongprice == DialogResult.Yes)
                    {
                        double rounded;
                        if(Demo.EsDemo == true)
                            rounded = Round(costomasganancia, 0);
                        else
                        rounded= Round(costomasganancia, float.Parse(registereduser.redondeo));
                        //MessageBox.Show(rounded.ToString());
                        costomasganancia = float.Parse(rounded.ToString()); 
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
                //MessageBox.Show(eex.Message);
            }
            
           
           
           
        }
        public static Double Round(Double passednumber, Double roundto)
        {
            // 105.5 up to nearest 1 = 106
            // 105.5 up to nearest 10 = 110
            // 105.5 up to nearest 7 = 112
            // 105.5 up to nearest 100 = 200
            // 105.5 up to nearest 0.2 = 105.6
            // 105.5 up to nearest 0.3 = 105.6

            //if no rounto then just pass original number back
            if (roundto == 0)
            {
                return passednumber;
            }
            else
            {
                return Math.Ceiling(passednumber / roundto) * roundto;
            }
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            try{
                
                textBox10.Text = float.Parse(textBox10.Text).ToString(); 
            }
                catch(Exception)
            {
                textBox10.Text = "";
                }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                SqlCeCommand checkexistance = new SqlCeCommand();
                checkexistance.Parameters.AddWithValue("code", textBox1.Text);
                Conexion.abrir();
                DataTable existira = Conexion.Consultar("codigoart", "Articulos", "where codigoart = @code and eliminado != 'Eliminado'", "", checkexistance);
                Conexion.cerrar();
                if (existira.Rows.Count > 0 && createorupdate.status == "create")
                {
                    MessageBox.Show("Ese codigo de producto ya existe, use otro codigo por favor");
                    textBox1.Text = "";
                }
            
            
            }
        }

        private void NuevoArticulo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.Text = textBox4.Text.Replace(".", ",");
            textBox4.SelectionStart = textBox4.Text.Length;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox5.Text = textBox5.Text.Replace(".", ",");
            textBox5.SelectionStart = textBox5.Text.Length;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            textBox6.Text = textBox6.Text.Replace(".", ",");
            textBox6.SelectionStart = textBox6.Text.Length;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            textBox8.Text = textBox8.Text.Replace(".", ",");
            textBox8.SelectionStart = textBox8.Text.Length;
        }

       
    }
}
