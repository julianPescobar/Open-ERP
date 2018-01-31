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
            //todo, hacer que llene los datos del form.
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
                MessageBox.Show("Mail Enviado");
            }

            catch (SmtpException ex)
            {
                throw new ApplicationException
                  ("Ocurrio un error al enviar el mail. Motivo: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fecha = dateTimePicker1.Value.ToShortDateString();
            sendmail(app.dir + "\\Cierre" + fecha.Replace("/","") + ".txt");
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
    }
}
