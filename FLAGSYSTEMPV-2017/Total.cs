using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EPSON_Impresora_Fiscal; //EPSON
using FiscalPrinterLib;       //HASAR
using System.IO.Ports;
using System.Data.SqlServerCe;
namespace FLAGSYSTEMPV_2017
{
    public partial class Total : Form
    {
        public Total()
        {
            InitializeComponent();
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
        void vender()
        {
            //guardo en base de datos
            SqlCeCommand item = new SqlCeCommand();
            Conexion.abrir();
            for (int i = 0; i < totalventa.detalle.Rows.Count; i++)
            {
                item.Parameters.Clear();
                item.Parameters.AddWithValue("nf", totalventa.idventa);
                item.Parameters.AddWithValue("idprod", totalventa.detalle.Rows[i][0].ToString());
                item.Parameters.AddWithValue("cp", totalventa.detalle.Rows[i][2].ToString());
                item.Parameters.AddWithValue("dp", totalventa.detalle.Rows[i][3].ToString());
                item.Parameters.AddWithValue("mc", totalventa.detalle.Rows[i][4].ToString());
                item.Parameters.AddWithValue("ca", totalventa.detalle.Rows[i][1].ToString());
                item.Parameters.AddWithValue("pp", totalventa.detalle.Rows[i][5].ToString().Replace("$", ""));
                item.Parameters.AddWithValue("to", totalventa.detalle.Rows[i][6].ToString().Replace("$", ""));
                item.Parameters.AddWithValue("ti", totalventa.detalle.Rows[i][7].ToString().Replace("$", ""));
                string prodser = totalventa.detalle.Rows[i][7].ToString();
                Conexion.Insertar("DetalleVentas", "nfactura,idproducto, codigoproducto , descripproducto, marcaproducto, cantidproducto, precioproducto, totalproducto,tipo", "@nf,@idprod,@cp,@dp,@mc,@ca,@pp,@to,@ti", item);
                if(prodser.Contains("Producto"))
                Conexion.Actualizar("Articulos", "stockactual = stockactual - @ca", "WHERE idarticulo = @idprod", "", item);
            }
            if (Demo.EsDemo == true)
            {
                item.Parameters.AddWithValue("ve", Demo.demouser);
            }
            else
            {
                item.Parameters.AddWithValue("ve", registereduser.reguser);
            }
            item.Parameters.AddWithValue("fv", app.hoy + " " + DateTime.Now.ToShortTimeString());
            item.Parameters.AddWithValue("tt", totalventa.totventa.Replace("$", ""));
            item.Parameters.AddWithValue("ev", "Finalizado");
            item.Parameters.AddWithValue("tf", "FC");
            Conexion.Insertar("Ventas", "nfactura, vendedor, fechaventa, total , estadoventa  , tipoFactura ", "@nf,@ve,@fv,@tt,@ev,@tf", item);
            Conexion.cerrar();
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
        private void Total_Load(object sender, EventArgs e)
        {
            if (totalventa.compraoventa == "Ventas")
            {
                float total = float.Parse(totalventa.totventa.Replace("$", ""));
                textBox1.Text = total.ToString("$0.00");
                textBox3.Text = (0 - total).ToString("$0.00");
                if (registereduser.alwaysprint == "si")
                    checkBox1.Checked = true;
                else
                    checkBox1.Checked = false;
                textBox1.Focus();
                textBox2.Focus();
                textBox3.Focus();
                //MessageBox.Show(totalventa.detalle.Rows.Count.ToString());
            }
            if (totalventa.compraoventa == "Compras")
            {
                label1.Text = "Subtotal";
                label2.Text = "Total EXACTO de la factura:";
                label3.Text = "Impuestos extra";
                float total = float.Parse(totalventa.totcompra.Replace("$", ""));
                textBox1.Text = total.ToString("$0.00");
                textBox3.Text = (0 - total).ToString("$0.00");
                checkBox1.Enabled = false;
                checkBox1.Visible = false;
                textBox1.Focus();
                textBox2.Focus();
                textBox3.Focus();
                //MessageBox.Show(totalventa.detalle.Rows.Count.ToString());
            }
            if (totalventa.compraoventa == "NC")
            {
                float total = float.Parse(totalventa.totnotacred.Replace("$", ""));
                textBox1.Text = (total).ToString("$0.00");
                textBox3.Text = (0 + total).ToString("$0.00");
                checkBox1.Enabled = false;
                checkBox1.Visible = false;
                textBox1.Focus();
                textBox2.Focus();
                textBox3.Focus();
                //MessageBox.Show(totalventa.detalle.Rows.Count.ToString());
            }
            if (totalventa.compraoventa == "ND")
            {
                float total = float.Parse(totalventa.totnotadeb.Replace("$", ""));
                textBox1.Text = (total).ToString("$0.00");
                textBox3.Text = (0 + total).ToString("$0.00");
                checkBox1.Enabled = false;
                checkBox1.Visible = false;
                textBox1.Focus();
                textBox2.Focus();
                textBox3.Focus();
                //MessageBox.Show(totalventa.detalle.Rows.Count.ToString());
            }
        }

        private void Total_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                        this.DisplayRectangle);      
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (totalventa.compraoventa == "NC" || totalventa.compraoventa == "ND")
            {
                textBox2.Focus();
                //textBox2.Text = float.Parse(textBox2.Text.ToString()).ToString("0");
                if (textBox2.Text == "")
                {
                    float total = float.Parse(textBox1.Text.Replace("$", ""));
                    textBox3.Text = (0 + (total * -1)).ToString("$0.00");
                }
                else
                {
                    try
                    {
                        float total = float.Parse(textBox1.Text.Replace("$", ""));
                        float mipalta = float.Parse(textBox2.Text.Replace("$", ""));
                        textBox3.Text = (mipalta - total * -1).ToString("$0.00");
                    }
                    catch (Exception)
                    {
                        textBox2.Text = "";
                    }
                }
            }
            if(totalventa.compraoventa == "Ventas" || totalventa.compraoventa == "Compras")
            {
                textBox2.Focus();
                //textBox2.Text = float.Parse(textBox2.Text.ToString()).ToString("0");
                if (textBox2.Text == "")
                {
                    float total = float.Parse(textBox1.Text.Replace("$", ""));
                    textBox3.Text = (0 + total ).ToString("$0.00");
                }
                else
                {
                    try
                    {
                        float total = float.Parse(textBox1.Text.Replace("$", ""));
                        float mipalta = float.Parse(textBox2.Text.Replace("$", ""));
                        textBox3.Text = (mipalta - total ).ToString("$0.00");
                    }
                    catch (Exception)
                    {
                        textBox2.Text = "";
                    }
                }
            }
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox2.Text == "") textBox2.Text = textBox1.Text.Replace("$", "");
                float t2 = float.Parse(textBox2.Text.ToString());
                float t1 = float.Parse(textBox1.Text.ToString().Replace("$", ""));
                float t3 = t2 - t1;
                if (t2 >= t1)
                {
                    if (totalventa.compraoventa == "Ventas") //si estamos en ventas
                    {
                        if (ConfigFiscal.usaImpFiscal == "si") //si tenemos impre fiscal configurada
                        {
                            if (ConfigFiscal.marca == "EPSON") //si es epson
                            {
                                EPSON_Impresora_Fiscal.PrinterFiscal epson = new PrinterFiscal();
                                epson.PortNumber = ConfigFiscal.comport;
                                epson.BaudRate = "9600";
                                epson.MessagesOn = true;
                               // epson.SetGetDateTime("S", "200120", "100505");
                                //epson.OpenTicket("C");
                                //MessageBox.Show(epson.PrinterStatus + "\n" + epson.FiscalStatus);
                                try
                                {
                                    bool status;
                                    textBox2.Enabled = false;
                                    Pleasewait reg = new Pleasewait();
                                    reg.ShowDialog();
                                    status = epson.OpenTicket("C");
                                    if (Application.OpenForms.OfType<Pleasewait>().Count() == 1)
                                        Application.OpenForms.OfType<Pleasewait>().First().Close();
                                    textBox2.Enabled = true;
                                    textBox2.Focus();
                                    if (status == true) //imprimo a impresora fiscal
                                    {
                                        string pagocon = textBox2.Text.ToString().Replace("$", "");
                                        if (!pagocon.Contains(","))
                                        {
                                            pagocon = pagocon + ",00";
                                        }
                                        for (int i = 0; i < totalventa.detalle.Rows.Count; i++)
                                        {
                                            string descripcion = totalventa.detalle.Rows[i][3].ToString();
                                            string cantidad = totalventa.detalle.Rows[i][1].ToString();
                                            string precio = totalventa.detalle.Rows[i][5].ToString().Replace("$", "");
                                            // MessageBox.Show(descripcion + " - " + cantidad + " - " + precio);
                                            epson.SendTicketItem(descripcion, cantidad.PadRight(4, '0'), precio.ToString().Substring(0, precio.Length - 3).PadLeft(7, '0') + precio.ToString().Substring(precio.Length - 3, 3).Replace(",", ""), "1", "M", "1", "1", "1");
                                        }
                                        epson.SendTicketPayment("PAGO CON ", pagocon.Substring(0, pagocon.Length - 3).PadLeft(7, '0') + pagocon.Substring(pagocon.Length - 2, 2), "T");
                                        epson.CloseTicket();
                                    }
                                    vender();
                                }
                                catch (Exception m)
                                {
                                    MessageBox.Show("error en impresora fiscal.\n" + m.Message);
                                    this.Close();
                                }
                            }
                            if (ConfigFiscal.marca == "HASAR") //si es hasar
                            {
                                HASAR hasar = new HASAR();
                                //lo hacemos con hasar
                            }
                            if (ConfigFiscal.marca == "NCR") //si es ncr
                            {
                                //lo hacemos con ncr
                            }
                            if (ConfigFiscal.marca == "OLIVETTI") //si es oliveti
                            {
                                //lo hacemos con oliv
                            }

                            if (ConfigFiscal.marca == "SAMSUNG") //si es una sanguang
                            {
                                //lo hacemos con sam
                            }
                        }
                        else //si no tiene imp fiscal pasamos a facturar nomas
                        {
                            vender();
                        }
                    }//todo esto es la parte de Ventas

                 
                    if (totalventa.compraoventa == "Compras")
                    {
                        totalventa.impuestoextra = t3.ToString();
                        float porcentajefactura = float.Parse(totalventa.impuestoextra.ToString()) * 100 / float.Parse(totalventa.totcompra.ToString().Replace("$",""));
                        //MessageBox.Show(porcentajefactura.ToString("0.00"));
                        
                        
                        //guardo en base de datos
                        SqlCeCommand item = new SqlCeCommand();
                        Conexion.abrir();
                        for (int i = 0; i < totalventa.detallecompra.Rows.Count; i++)
                        {
                            float Precio = float.Parse(totalventa.detallecompra.Rows[i][5].ToString().Replace("$", ""));
                            float IVA = float.Parse(totalventa.detallecompra.Rows[i][8].ToString().Replace("$", ""));
                            float PrecioconIva = (Precio + (Precio * (IVA / 100))); 
                            float PorcentajeCosto = float.Parse(totalventa.detallecompra.Rows[i][9].ToString().Replace("$", ""));
                            float Costo = float.Parse(totalventa.detallecompra.Rows[i][7].ToString().Replace("$", ""));
                            float nuevocosto = (Costo + (Costo * (porcentajefactura / 100)));
                            float nuevocostoconporcentaje = float.Parse(Round(double.Parse((nuevocosto + (nuevocosto * (PorcentajeCosto / 100))).ToString()),double.Parse(registereduser.redondeo)).ToString());

                            item.Parameters.Clear();
                            item.Parameters.AddWithValue("nf", totalventa.idcompra);
                            item.Parameters.AddWithValue("idprod", totalventa.detallecompra.Rows[i][0].ToString());
                            item.Parameters.AddWithValue("cp", totalventa.detallecompra.Rows[i][2].ToString());
                            item.Parameters.AddWithValue("dp", totalventa.detallecompra.Rows[i][3].ToString());
                            item.Parameters.AddWithValue("mc", totalventa.detallecompra.Rows[i][4].ToString());
                            item.Parameters.AddWithValue("ca", Convert.ToInt32(totalventa.detallecompra.Rows[i][1].ToString()));
                            item.Parameters.AddWithValue("pp", totalventa.detallecompra.Rows[i][5].ToString().Replace("$", ""));
                            item.Parameters.AddWithValue("to", totalventa.detallecompra.Rows[i][6].ToString().Replace("$", ""));
                            item.Parameters.AddWithValue("ncosto", nuevocosto);
                            item.Parameters.AddWithValue("porcent", nuevocostoconporcentaje);
                           // MessageBox.Show(porcentajefactura.ToString("0.00"));
                            //item.Parameters.AddWithValue("costo", totalventa.detallecompra.Rows[i][7].ToString());
                            //MessageBox.Show(totalventa.detallecompra.Rows[i][8].ToString() + "-" + totalventa.detallecompra.Rows[i][7].ToString());
                            //MessageBox.Show(totalventa.detallecompra.Rows[i][1].ToString());
                            
                            if ( nuevocostoconporcentaje > Precio)
                            {

                            //MessageBox.Show("el nuevo costo es mayor que el precio actual.\n"+nuevocostoconporcentaje.ToString("$0.00")+" > "+PrecioconIva.ToString("$0.00"));
                            Conexion.Actualizar("Articulos", " precio = @porcent, costo = @ncosto  ", "WHERE idarticulo = @idprod", "", item);
                            
                            }

                            Conexion.Insertar("DetalleCompras", "nfactura,idproducto, codigoproducto , descripproducto, marcaproducto, cantidproducto, precioproducto, totalproducto", "@nf,@idprod,@cp,@dp,@mc,@ca,@pp,@to", item);
                            Conexion.Actualizar("Articulos", " stockactual = stockactual + " + Convert.ToInt32(totalventa.detallecompra.Rows[i][1].ToString()) + ", costo = @ncosto ", "WHERE idarticulo = @idprod", "", item);
                        }
                        if (Demo.EsDemo == true)
                        {
                            item.Parameters.AddWithValue("ve", Demo.demouser);
                        }
                        else
                        {
                            item.Parameters.AddWithValue("ve", registereduser.reguser);
                        }
                        item.Parameters.AddWithValue("fv", totalventa.fechacompra);
                        item.Parameters.AddWithValue("tt", totalventa.totcompra.Replace("$", ""));
                        item.Parameters.AddWithValue("ev", "Finalizado");
                        item.Parameters.AddWithValue("pro", totalventa.proveedcompra);
                        Conexion.Insertar("Compras", "nfactura, vendedor, fechacompra, totalfactura , estadocompra,proveedor ", "@nf,@ve,@fv,@tt,@ev,@pro", item);
                        Conexion.cerrar();
                    }//todo esto es la parte de Compras

                    if(totalventa.compraoventa == "NC")
                    {
                        //guardo en base de datos
                        SqlCeCommand item = new SqlCeCommand();
                        Conexion.abrir();
                        for (int i = 0; i < totalventa.detallenotacred.Rows.Count; i++)
                        {
                            item.Parameters.Clear();
                            item.Parameters.AddWithValue("nf", totalventa.idnotacred);
                            item.Parameters.AddWithValue("idprod", totalventa.detallenotacred.Rows[i][1].ToString());
                            item.Parameters.AddWithValue("cp", totalventa.detallenotacred.Rows[i][2].ToString());
                            item.Parameters.AddWithValue("dp", totalventa.detallenotacred.Rows[i][3].ToString());
                            item.Parameters.AddWithValue("mc", totalventa.detallenotacred.Rows[i][4].ToString());
                            item.Parameters.AddWithValue("ca", totalventa.detallenotacred.Rows[i][1].ToString());
                            item.Parameters.AddWithValue("pp", totalventa.detallenotacred.Rows[i][5].ToString().Replace("$", ""));
                            item.Parameters.AddWithValue("to", totalventa.detallenotacred.Rows[i][6].ToString().Replace("$", ""));
                            Conexion.Insertar("DetalleNotaCreds", "nfactura,idproducto, codigoproducto , descripproducto, marcaproducto, cantidproducto, precioproducto, totalproducto", "@nf,@idprod,@cp,@dp,@mc,@ca,@pp,@to", item);
                        }
                        if (Demo.EsDemo == true)
                        {
                            item.Parameters.AddWithValue("ve", Demo.demouser);
                        }
                        else
                        {
                            item.Parameters.AddWithValue("ve", registereduser.reguser);
                        }
                        item.Parameters.AddWithValue("fv", app.hoy + " " + DateTime.Now.ToShortTimeString());
                        item.Parameters.AddWithValue("tt", totalventa.totnotacred.Replace("$", ""));
                        item.Parameters.AddWithValue("ev", "Finalizado");
                        item.Parameters.AddWithValue("tf", "NC");
                        Conexion.Insertar("NotaCreds", "nfactura, vendedor, fechaventa, total , estadoventa  , tipoFactura ", "@nf,@ve,@fv,@tt,@ev,@tf", item);
                        Conexion.cerrar();
                    }

                    if (totalventa.compraoventa == "ND")
                    {
                        //guardo en base de datos
                        SqlCeCommand item = new SqlCeCommand();
                        Conexion.abrir();
                        for (int i = 0; i < totalventa.detallenotadeb.Rows.Count; i++)
                        {
                            item.Parameters.Clear();
                            item.Parameters.AddWithValue("nf", totalventa.idnotadeb);
                            item.Parameters.AddWithValue("idprod", totalventa.detallenotadeb.Rows[i][1].ToString());
                            item.Parameters.AddWithValue("cp", totalventa.detallenotadeb.Rows[i][2].ToString());
                            item.Parameters.AddWithValue("dp", totalventa.detallenotadeb.Rows[i][3].ToString());
                            item.Parameters.AddWithValue("mc", totalventa.detallenotadeb.Rows[i][4].ToString());
                            item.Parameters.AddWithValue("ca", totalventa.detallenotadeb.Rows[i][1].ToString());
                            item.Parameters.AddWithValue("pp", totalventa.detallenotadeb.Rows[i][5].ToString().Replace("$", ""));
                            item.Parameters.AddWithValue("to", totalventa.detallenotadeb.Rows[i][6].ToString().Replace("$", ""));
                            Conexion.Insertar("DetalleNotaDebs", "nfactura,idproducto, codigoproducto , descriproducto, marcaproducto, cantidproducto, precioproducto, totalproducto", "@nf,@idprod,@cp,@dp,@mc,@ca,@pp,@to", item);
                        }
                        if (Demo.EsDemo == true)
                        {
                            item.Parameters.AddWithValue("ve", Demo.demouser);
                        }
                        else
                        {
                            item.Parameters.AddWithValue("ve", registereduser.reguser);
                        }
                        item.Parameters.AddWithValue("fv", app.hoy + " " + DateTime.Now.ToShortTimeString());
                        item.Parameters.AddWithValue("tt", totalventa.totnotadeb.Replace("$", ""));
                        item.Parameters.AddWithValue("ev", "Finalizado");
                        item.Parameters.AddWithValue("tf", "NC");
                        Conexion.Insertar("NotaDebs", "nfactura, vendedor, fechaventa, total , estadoventa  , tipoFactura ", "@nf,@ve,@fv,@tt,@ev,@tf", item);
                        Conexion.cerrar();
                    }
                    if (totalventa.compraoventa == "Ventas")
                    {
                        this.Close();
                        if (Application.OpenForms.OfType<Ventas>().Count() == 1)
                        Application.OpenForms.OfType<Ventas>().First().Close();
                        Ventas abrirventa = new Ventas();
                        abrirventa.Show();
                    }
                    if (totalventa.compraoventa == "NC")
                    {
                        this.Close();
                        if (Application.OpenForms.OfType<NotaDeCredito>().Count() == 1)
                            Application.OpenForms.OfType<NotaDeCredito>().First().Close();
                        NotaDeCredito abrirventa = new NotaDeCredito();
                        abrirventa.Show();
                    }

                    if (totalventa.compraoventa == "ND")
                    {
                        this.Close();
                        if (Application.OpenForms.OfType<NotaDeDebito>().Count() == 1)
                            Application.OpenForms.OfType<NotaDeDebito>().First().Close();
                        NotaDeDebito abrirventa = new NotaDeDebito();
                        abrirventa.Show();
                    }
                    if (totalventa.compraoventa == "Compras")
                    {
                        this.Close();
                        if (Application.OpenForms.OfType<Compras>().Count() == 1)
                        Application.OpenForms.OfType<Compras>().First().Close();
                        Compras abrirventa = new Compras();
                        abrirventa.Show();
                    } 
               }
                else
                {
                    MessageBox.Show("El pago del cliente no puede ser menor al total de venta.");
                }
                //MessageBox.Show("impresora fiscal aca");
            }
        }
    }
}
