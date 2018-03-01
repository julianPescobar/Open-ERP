using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Data.SqlServerCe;
using System.Data.OleDb;
namespace FLAGSYSTEMPV_2017
{
    public partial class EnviarMail : Form
    {
        public EnviarMail()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EnviarMail_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                          this.DisplayRectangle);    
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

        private void EnviarMail_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = Convert.ToDateTime(app.hoy);
            string fecha = dateTimePicker1.Value.ToShortDateString();
            if (File.Exists(app.dir + "\\Cierre" + fecha.Replace("/","") + ".txt") == true)
            {
                getdata();
                button1.Enabled = true;
                button3.Visible = false;
            }
            else
            {
                button3.Visible = true;
                button1.Enabled = false;
            }
        }
        private void getdata()
        {
           
              string fecha = dateTimePicker1.Value.ToShortDateString();
            string[] data = File.ReadAllLines(app.dir + "\\Cierre" + fecha.Replace("/", "") + ".txt");
            float venta = float.Parse(data[2].ToString().Replace("Total Ventas:", "").Replace("$", ""));
            float compra = float.Parse(data[3].ToString().Replace("Total Compras:", "").Replace("$", ""));
            float gasto = float.Parse(data[4].ToString().Replace("Total Gastos:", "").Replace("$", ""));
            float entrada = float.Parse(data[5].ToString().Replace("Total Entrada Caja:", "").Replace("$", ""));
            float salida = float.Parse(data[6].ToString().Replace("Total Salida Caja:", "").Replace("$", ""));
            float total = float.Parse(data[7].ToString().Replace("Total del Día:", "").Replace("$", ""));
            textBox1.Text = venta.ToString("$0.00");
            textBox2.Text = compra.ToString("$0.00");
            textBox3.Text = gasto.ToString("$0.00");
            textBox4.Text = entrada.ToString("$0.00");
            textBox5.Text = salida.ToString("$0.00");
            textBox6.Text = total.ToString("$0.00");
        }
        private void sendmail(string attachmentFilename)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(registereduser.smtp.ToString());
                mail.From = new MailAddress(registereduser.mail.ToString());
                mail.To.Add(registereduser.para.ToString());
                mail.Subject = registereduser.titulo.ToString()+"-"+registereduser.registeredlicense.ToString();
                mail.Body = registereduser.cuerpo.ToString();
                if (attachmentFilename != null)
                {
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(attachmentFilename);
                    mail.Attachments.Add(attachment);
                }
                SmtpServer.Port = int.Parse(registereduser.puerto.ToString());
                SmtpServer.Credentials = new System.Net.NetworkCredential(registereduser.mail.ToString(), registereduser.clave.ToString());
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                this.Close();
                MessageBox.Show("Mail Enviado Correctamente.");
            }

            catch (SmtpException ex)
            {
                throw new ApplicationException
                  ("Ocurrio un error al enviar el mail. Motivo: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error al intentar enviar el Email. Revise todos los datos de configuración del Email en Administrador>Configuración y tambien revise que tenga conexión a Internet.\nDetalle del error: "+ex.Message);
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fecha = dateTimePicker1.Value.ToShortDateString();
            sendmail(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".xlsx.xls");
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string fecha = dateTimePicker1.Value.ToShortDateString();
            if (File.Exists(app.dir + "\\Cierre" + fecha.Replace("/", "") + ".txt") == true)
            {
                getdata();
                button1.Enabled = true;
                button3.Visible = false;
            }
            else
            {
                button3.Visible = true;
                button1.Enabled = false;
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";

            }
        }
        public static void generarResumenFinal()
        {
            Conexion.abrir();
            SqlCeCommand hoy = new SqlCeCommand();
            hoy.Parameters.AddWithValue("hoy", app.hoy + " 00:00:00");
            hoy.Parameters.AddWithValue("hoy2", app.hoy + " 23:59:59");
            DataTable ventas = Conexion.Consultar("nfactura,vendedor,total", "Ventas", "Where estadoVenta != 'Anulada' and fechaventa between @hoy and @hoy2", "", hoy);
            DataTable compras = Conexion.Consultar("nfactura,vendedor,CAST(totalfactura as FLOAT) as total", "Compras", "Where fechacompra between @hoy and @hoy2", "", hoy);
            DataTable gastos = Conexion.Consultar("area,descripcion,importe", "Gastos", "Where fecha between @hoy and @hoy2", "", hoy);
            DataTable entradas = Conexion.Consultar("tipo,motivo,total", "EntradaCaja", "Where fecha between @hoy and @hoy2", "", hoy);
            DataTable salidas = Conexion.Consultar("tipo,motivo,total", "SalidaCaja", "Where fecha between @hoy and @hoy2", "", hoy);
            string[] Ventas = new string[ventas.Rows.Count];
            string[] Compras = new string[compras.Rows.Count];
            string[] Gastos = new string[gastos.Rows.Count];
            string[] Entradas = new string[entradas.Rows.Count];
            string[] Salidas = new string[salidas.Rows.Count];
            float tv = 0;
            float tc = 0;
            float tg = 0;
            float te = 0;
            float ts = 0;

            if (ventas.Rows.Count > 0)
            {
                for (int i = 0; i < ventas.Rows.Count; i++)
                {
                    tv += float.Parse(ventas.Rows[i][2].ToString());
                    Ventas[i] = ventas.Rows[i][0].ToString() + "\t" + ventas.Rows[i][1].ToString() + "\t" + tv.ToString("$0.00");
                }
            }
            else tv = 0;

            if (compras.Rows.Count > 0)
            {
                for (int i = 0; i < compras.Rows.Count; i++)
                {
                    tc += float.Parse(compras.Rows[i][2].ToString());
                    Compras[i] = compras.Rows[i][0].ToString() + "\t" + compras.Rows[i][1].ToString() + "\t" + tc.ToString("$0.00");
                }
            }
            else tc = 0;

            if (gastos.Rows.Count > 0)
            {
                for (int i = 0; i < gastos.Rows.Count; i++)
                {
                    tg += float.Parse(gastos.Rows[i][2].ToString());
                    Gastos[i] = gastos.Rows[i][0].ToString() + "\t" + gastos.Rows[i][1].ToString() + "\t" + tg.ToString("$0.00");
                }
            }
            else tg = 0;

            if (entradas.Rows.Count > 0)
            {
                for (int i = 0; i < entradas.Rows.Count; i++)
                {
                    te += float.Parse(entradas.Rows[i][2].ToString());
                    Entradas[i] = entradas.Rows[i][0].ToString() + "\t" + entradas.Rows[i][1].ToString() + "\t" + te.ToString("$0.00");
                }
            }
            else te = 0;

            if (salidas.Rows.Count > 0)
            {
                for (int i = 0; i < salidas.Rows.Count; i++)
                {
                    ts += float.Parse(salidas.Rows[i][2].ToString());
                    Salidas[i] = salidas.Rows[i][0].ToString() + "\t" + salidas.Rows[i][1].ToString() + "\t" + ts.ToString("$0.00");
                }
            }
            else ts = 0;

            File.WriteAllText(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", registereduser.registeredlicense + "\r\n" + "Informe de cierre del dia " + DateTime.Now.ToShortDateString() + "\r\nTotal Ventas:\t" + tv.ToString("$0.00") + "\r\nTotal Compras:\t" + tc.ToString("$0.00") + "\r\nTotal Gastos:\t" + tg.ToString("$0.00") + "\r\nTotal Entrada Caja:\t" + te.ToString("$0.00") + "\r\nTotal Salida Caja:\t" + ts.ToString("$0.00") + "\r\nTotal del Día:\t" + ((tv + tc + te) - (tg + ts)).ToString("$0.00"));
            File.AppendAllText(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", "\r\n");
            File.AppendAllText(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", "\r\nDetalle de Ventas:\r\nN° Venta\tVendedor\tTotal\t\r\n");
            File.AppendAllLines(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", Ventas);
            File.AppendAllText(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", "\r\nDetalle de Compras:\r\nN° Compra\tVendedor\tTotal\t\r\n");
            File.AppendAllLines(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", Compras);
            File.AppendAllText(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", "\r\nDetalle de Gastos:\r\nArea\tDescripcion\tImporte\t\r\n");
            File.AppendAllLines(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", Gastos);
            File.AppendAllText(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", "\r\nDetalle de Entradas de Caja:\r\nTipo\tMotivo\tTotal\t\r\n");
            File.AppendAllLines(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", Entradas);
            File.AppendAllText(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", "\r\nDetalle de Salidas de Caja:\r\nTipo\tMotivo\tTotal\t\r\n");
            File.AppendAllLines(app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".txt", Salidas);

            try
            {
                string tempfilename = app.dir + "\\Cierre" + app.hoy.Replace("/", "") + ".xlsx.xls";
                string xConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + tempfilename + ";Extended Properties='Excel 8.0;HDR=YES'";
                string TabName = "";
                var conn = new OleDbConnection(xConnStr);
                //agarramos las columnas de ventas
                string ventascolumnas = "";
                for (int i = 0; i < ventas.Columns.Count; i++)
                {
                    if (i == 0) ventascolumnas += "[" + ventas.Columns[i].ColumnName + "] varchar(255)";
                    else ventascolumnas += ", [" + ventas.Columns[i].ColumnName + "] varchar(255)";
                }
                //agarramos las columnas de compras
                string comprascolumnas = "";
                for (int i = 0; i < compras.Columns.Count; i++)
                {
                    if (i == 0) comprascolumnas += "[" + compras.Columns[i].ColumnName + "] varchar(255)";
                    else comprascolumnas += ", [" + compras.Columns[i].ColumnName + "] varchar(255)";
                }
                //agarramos las columnas de gasts
                string gastoscolumnas = "";
                for (int i = 0; i < gastos.Columns.Count; i++)
                {
                    if (i == 0) gastoscolumnas += "[" + gastos.Columns[i].ColumnName + "] varchar(255)";
                    else gastoscolumnas += ", [" + gastos.Columns[i].ColumnName + "] varchar(255)";
                }
                //agarramos las columnas de ents
                string entradascolumnas = "";
                for (int i = 0; i < entradas.Columns.Count; i++)
                {
                    if (i == 0) entradascolumnas += "[" + entradas.Columns[i].ColumnName + "] varchar(255)";
                    else entradascolumnas += ", [" + entradas.Columns[i].ColumnName + "] varchar(255)";
                }
                //agarramos las columnas de sals
                string salidascolumnas = "";
                for (int i = 0; i < salidas.Columns.Count; i++)
                {
                    if (i == 0) salidascolumnas += "[" + salidas.Columns[i].ColumnName + "] varchar(255)";
                    else salidascolumnas += ", [" + salidas.Columns[i].ColumnName + "] varchar(255)";
                }


                //hacemos una libro para el total
                string ColumnName = "[a] varchar(255), [b] varchar(255), [c] varchar(255)";
                conn.Open();
                TabName = "Totales";
                var cmd = new OleDbCommand("CREATE TABLE [" + TabName + "] (" + ColumnName + ")", conn);
                cmd.ExecuteNonQuery();
                var insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)", "") + ") VALUES (" + "'Fecha:','" + app.hoy + "','" + registereduser.registeredlicense + "'" + ")", conn);
                insert.ExecuteNonQuery();
                insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)", "") + ") VALUES (" + "'Totales:',' ',' '" + ")", conn);
                insert.ExecuteNonQuery();
                insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)", "") + ") VALUES (" + "'Total Ventas:','" + tv.ToString("$0.00") + "',' '" + ")", conn);
                insert.ExecuteNonQuery();
                insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)", "") + ") VALUES (" + "'Total Compras:','" + tc.ToString("$0.00") + "',' '" + ")", conn);
                insert.ExecuteNonQuery();
                insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)", "") + ") VALUES (" + "'Total Gastos:','" + tg.ToString("$0.00") + "',' '" + ")", conn);
                insert.ExecuteNonQuery();
                insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)", "") + ") VALUES (" + "'Total Entradas:','" + te.ToString("$0.00") + "',' '" + ")", conn);
                insert.ExecuteNonQuery();
                insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)", "") + ") VALUES (" + "'Total Salidas:','" + ts.ToString("$0.00") + "',' '" + ")", conn);
                insert.ExecuteNonQuery();
                insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)", "") + ") VALUES (" + "'Total del día:','" + ((tv + tc + te) - (tg + ts)).ToString("$0.00") + "',' '" + ")", conn);
                insert.ExecuteNonQuery();
                conn.Close();
                //terminamos el libro total


                //hacemos una libro para esto
                ColumnName = ventascolumnas;
                conn.Open();
                TabName = "Ventas";
                cmd = new OleDbCommand("CREATE TABLE [" + TabName + "] (" + ColumnName + ")", conn);
                cmd.ExecuteNonQuery();
                for (int i = 0; i < ventas.Rows.Count; i++)
                {
                    string values = "";
                    for (int j = 0; j < ventas.Columns.Count; j++)
                    {
                        if (j == 0) values += "'" + ventas.Rows[i][j].ToString() + "'";
                        else values += ", '" + ventas.Rows[i][j].ToString() + "'";
                    }
                    insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)", "") + ") VALUES (" + values + ")", conn);
                    insert.ExecuteNonQuery();
                }
                conn.Close();
                //terminamos el libro


                //hacemos una libro para esto
                ColumnName = comprascolumnas;
                conn.Open();
                TabName = "Compras";
                cmd = new OleDbCommand("CREATE TABLE [" + TabName + "] (" + ColumnName + ")", conn);
                cmd.ExecuteNonQuery();
                for (int i = 0; i < compras.Rows.Count; i++)
                {
                    string values = "";
                    for (int j = 0; j < compras.Columns.Count; j++)
                    {
                        if (j == 0) values += "'" + compras.Rows[i][j].ToString() + "'";
                        else values += ", '" + compras.Rows[i][j].ToString() + "'";
                    }
                    insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)", "") + ") VALUES (" + values + ")", conn);
                    insert.ExecuteNonQuery();
                }
                conn.Close();
                //terminamos el libro


                //hacemos una libro para esto
                ColumnName = gastoscolumnas;
                conn.Open();
                TabName = "Gastos";
                cmd = new OleDbCommand("CREATE TABLE [" + TabName + "] (" + ColumnName + ")", conn);
                cmd.ExecuteNonQuery();
                for (int i = 0; i < gastos.Rows.Count; i++)
                {
                    string values = "";
                    for (int j = 0; j < gastos.Columns.Count; j++)
                    {
                        if (j == 0) values += "'" + gastos.Rows[i][j].ToString() + "'";
                        else values += ", '" + gastos.Rows[i][j].ToString() + "'";
                    }
                    insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)", "") + ") VALUES (" + values + ")", conn);
                    insert.ExecuteNonQuery();
                }
                conn.Close();
                //terminamos el libro

                //hacemos una libro para esto
                ColumnName = entradascolumnas;
                conn.Open();
                TabName = "entradas";
                cmd = new OleDbCommand("CREATE TABLE [" + TabName + "] (" + ColumnName + ")", conn);
                cmd.ExecuteNonQuery();
                for (int i = 0; i < entradas.Rows.Count; i++)
                {
                    string values = "";
                    for (int j = 0; j < entradas.Columns.Count; j++)
                    {
                        if (j == 0) values += "'" + entradas.Rows[i][j].ToString() + "'";
                        else values += ", '" + entradas.Rows[i][j].ToString() + "'";
                    }
                    insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)", "") + ") VALUES (" + values + ")", conn);
                    insert.ExecuteNonQuery();
                }
                conn.Close();
                //terminamos el libro


                //hacemos una libro para esto
                ColumnName = salidascolumnas;
                conn.Open();
                TabName = "salidas";
                cmd = new OleDbCommand("CREATE TABLE [" + TabName + "] (" + ColumnName + ")", conn);
                cmd.ExecuteNonQuery();
                for (int i = 0; i < salidas.Rows.Count; i++)
                {
                    string values = "";
                    for (int j = 0; j < salidas.Columns.Count; j++)
                    {
                        if (j == 0) values += "'" + salidas.Rows[i][j].ToString() + "'";
                        else values += ", '" + salidas.Rows[i][j].ToString() + "'";
                    }
                    insert = new OleDbCommand("INSERT INTO [" + TabName + "] (" + ColumnName.Replace(" varchar(255)", "") + ") VALUES (" + values + ")", conn);
                    insert.ExecuteNonQuery();
                }
                conn.Close();
                //terminamos el libro
            }
            catch (Exception erm)
            {

                MessageBox.Show("Hubo un error al exportar a excel:\n" + erm.Message);
            }



        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                generarResumenFinal();
                string fecha = dateTimePicker1.Value.ToShortDateString();
                if (File.Exists(app.dir + "\\Cierre" + fecha.Replace("/", "") + ".txt") == true)
                {
                    getdata();
                    button1.Enabled = true;
                    button3.Visible = false;
                }
                else
                {
                    button3.Visible = true;
                    button1.Enabled = false;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                }
            }
            catch (Exception ec) { MessageBox.Show(ec.Message); }
        }
        private void createfile()
        {
            Conexion.abrir();
            SqlCeCommand hoy = new SqlCeCommand();
            hoy.Parameters.AddWithValue("hoy", dateTimePicker1.Value.ToShortDateString()+ " 00:00:00");
            hoy.Parameters.AddWithValue("hoy2", dateTimePicker1.Value.ToShortDateString() + " 23:59:59");
            DataTable ventas = Conexion.Consultar("nfactura,vendedor,total", "Ventas", "Where estadoVenta != 'Anulada' and fechaventa between @hoy and @hoy2", "", hoy);
            DataTable compras = Conexion.Consultar("nfactura,vendedor,CAST(totalfactura as FLOAT)", "Compras", "Where fechacompra between @hoy and @hoy2", "", hoy);
            DataTable gastos = Conexion.Consultar("area,descripcion,importe", "Gastos", "Where fecha between @hoy and @hoy2", "", hoy);
            DataTable entradas = Conexion.Consultar("tipo,motivo,total", "EntradaCaja", "Where fecha between @hoy and @hoy2", "", hoy);
            DataTable salidas = Conexion.Consultar("tipo,motivo,total", "SalidaCaja", "Where fecha between @hoy and @hoy2", "", hoy);
            string[] Ventas = new string[ventas.Rows.Count];
            string[] Compras = new string[compras.Rows.Count];
            string[] Gastos = new string[gastos.Rows.Count];
            string[] Entradas = new string[entradas.Rows.Count];
            string[] Salidas = new string[salidas.Rows.Count];
            float tv = 0;
            float tc = 0;
            float tg = 0;
            float te = 0;
            float ts = 0;

            if (ventas.Rows.Count > 0)
            {
                for (int i = 0; i < ventas.Rows.Count; i++)
                {
                    tv += float.Parse(ventas.Rows[i][2].ToString());
                    Ventas[i] = ventas.Rows[i][0].ToString() + "\t" + ventas.Rows[i][1].ToString() + "\t" + tv.ToString("$0.00");
                }
            }
            else tv = 0;

            if (compras.Rows.Count > 0)
            {
                for (int i = 0; i < compras.Rows.Count; i++)
                {
                    tc += float.Parse(compras.Rows[i][2].ToString());
                    Compras[i] = compras.Rows[i][0].ToString() + "\t" + compras.Rows[i][1].ToString() + "\t" + tc.ToString("$0.00");
                }
            }
            else tc = 0;

            if (gastos.Rows.Count > 0)
            {
                for (int i = 0; i < gastos.Rows.Count; i++)
                {
                    tg += float.Parse(gastos.Rows[i][2].ToString());
                    Gastos[i] = gastos.Rows[i][0].ToString() + "\t" + gastos.Rows[i][1].ToString() + "\t" + tg.ToString("$0.00");
                }
            }
            else tg = 0;

            if (entradas.Rows.Count > 0)
            {
                for (int i = 0; i < entradas.Rows.Count; i++)
                {
                    te += float.Parse(entradas.Rows[i][2].ToString());
                    Entradas[i] = entradas.Rows[i][0].ToString() + "\t" + entradas.Rows[i][1].ToString() + "\t" + te.ToString("$0.00");
                }
            }
            else te = 0;

            if (salidas.Rows.Count > 0)
            {
                for (int i = 0; i < salidas.Rows.Count; i++)
                {
                    ts += float.Parse(salidas.Rows[i][2].ToString());
                    Salidas[i] = salidas.Rows[i][0].ToString() + "\t" + salidas.Rows[i][1].ToString() + "\t" + ts.ToString("$0.00");
                }
            }
            else ts = 0;

            File.WriteAllText(app.dir + "\\Cierre" + dateTimePicker1.Value.ToShortDateString().Replace("/", "") + ".txt", registereduser.registeredlicense + "\r\n" + "Informe de cierre del dia " + dateTimePicker1.Value.ToShortDateString() + "\r\nTotal Ventas:\t" + tv.ToString("$0.00") + "\r\nTotal Compras:\t" + tc.ToString("$0.00") + "\r\nTotal Gastos:\t" + tg.ToString("$0.00") + "\r\nTotal Entrada Caja:\t" + te.ToString("$0.00") + "\r\nTotal Salida Caja:\t" + ts.ToString("$0.00") + "\r\nTotal del Día:\t" + ((tv + tc + te) - (tg + ts)).ToString("$0.00"));
            File.AppendAllText(app.dir + "\\Cierre" + dateTimePicker1.Value.ToShortDateString().Replace("/", "") + ".txt", "\r\n");
            File.AppendAllText(app.dir + "\\Cierre" + dateTimePicker1.Value.ToShortDateString().Replace("/", "") + ".txt", "\r\nDetalle de Ventas:\r\nN° Venta\tVendedor\tTotal\t\r\n");
            File.AppendAllLines(app.dir + "\\Cierre" + dateTimePicker1.Value.ToShortDateString().Replace("/", "") + ".txt", Ventas);
            File.AppendAllText(app.dir + "\\Cierre" + dateTimePicker1.Value.ToShortDateString().Replace("/", "") + ".txt", "\r\nDetalle de Compras:\r\nN° Compra\tVendedor\tTotal\t\r\n");
            File.AppendAllLines(app.dir + "\\Cierre" + dateTimePicker1.Value.ToShortDateString().Replace("/", "") + ".txt", Compras);
            File.AppendAllText(app.dir + "\\Cierre" + dateTimePicker1.Value.ToShortDateString().Replace("/", "") + ".txt", "\r\nDetalle de Gastos:\r\nArea\tDescripcion\tImporte\t\r\n");
            File.AppendAllLines(app.dir + "\\Cierre" + dateTimePicker1.Value.ToShortDateString().Replace("/", "") + ".txt", Gastos);
            File.AppendAllText(app.dir + "\\Cierre" + dateTimePicker1.Value.ToShortDateString().Replace("/", "") + ".txt", "\r\nDetalle de Entradas de Caja:\r\nTipo\tMotivo\tTotal\t\r\n");
            File.AppendAllLines(app.dir + "\\Cierre" + dateTimePicker1.Value.ToShortDateString().Replace("/", "") + ".txt", Entradas);
            File.AppendAllText(app.dir + "\\Cierre" + dateTimePicker1.Value.ToShortDateString().Replace("/", "") + ".txt", "\r\nDetalle de Salidas de Caja:\r\nTipo\tMotivo\tTotal\t\r\n");
            File.AppendAllLines(app.dir + "\\Cierre" + dateTimePicker1.Value.ToShortDateString().Replace("/", "") + ".txt", Salidas);
                   
        }

        private void EnviarMail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
    }
}
