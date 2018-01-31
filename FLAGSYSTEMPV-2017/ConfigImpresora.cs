using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Data.SqlServerCe;

namespace FLAGSYSTEMPV_2017
{
    public partial class ConfigImpresora : Form
    {
        public ConfigImpresora()
        {
            InitializeComponent();
        }

        private void ConfigImpresora_Load(object sender, EventArgs e)
        {
            if (Demo.EsDemo == false)
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    comboBox1.Items.Add(port);
                }
                if (ConfigFiscal.usaImpFiscal == "no") checkBox1.Checked = true;
                if (ConfigFiscal.usaImpFiscal == "si")
                {
                    checkBox1.Checked = false;
                    comboBox1.Text = "COM" + ConfigFiscal.comport.ToString();
                    comboBox2.Text = ConfigFiscal.marca.ToString();
                    comboBox3.Text = ConfigFiscal.modelo.ToString();
                }
            }
            else
            {
                MessageBox.Show("No se puede utilizar la impresora fiscal en la version Demo");
                this.Close();
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

        private void ConfigImpresora_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                          this.DisplayRectangle);      
        }

        private List<string> showmodelos()
        {
            List<string> modelos = new List<string>();
            if (comboBox2.Text == "EPSON")
            {
                modelos.Add("EPSON TM 300 AF");
                modelos.Add("EPSON TMU 950 F");
                modelos.Add("EPSON TM 2000 AF");
                modelos.Add("EPSON TM 2002 AF+");
                modelos.Add("EPSON TMU 220 AF/AF II");
                modelos.Add("EPSON LX 300 F");
                modelos.Add("EPSON LX 300 F+");
                modelos.Add("EPSON FX 880 F");
                modelos.Add("EPSON (VE) PF 200");
                modelos.Add("EPSON (VE) PF 220");
                modelos.Add("EPSON (VE) TM 950 PF");
                modelos.Add("EPSON (VE) TM 675 PF");
                modelos.Add("EPSON (VE) PF 300");
                modelos.Add("EPSON (VE) PF 300 II");
            }
            if (comboBox2.Text == "HASAR")
            {
                modelos.Add("HASAR SMH/P-615F");
                modelos.Add("HASAR SMH/P-PR4F");
                modelos.Add("HASAR SMH/P-PR5F");
                modelos.Add("HASAR SMH/P-930F");
                modelos.Add("HASAR SMH/P-951F");
                modelos.Add("HASAR SMH/P-715F");
                modelos.Add("HASAR SMH/P-715F v2.00");
                modelos.Add("HASAR SMH/P-441F");
                modelos.Add("HASAR SMH/P-320F");
                modelos.Add("HASAR SMH/P-321F");
                modelos.Add("HASAR SMH/P-322F");
                modelos.Add("HASAR SMH/P-322F v2.01");
                modelos.Add("HASAR SMH/P-330F");
                modelos.Add("HASAR SMH/P-1120F");
                modelos.Add("HASAR SMH/PL-8F");
                modelos.Add("HASAR SMH/PL-8F v2.01");
                modelos.Add("HASAR SMH/PL-23F");
            }
            if (comboBox2.Text == "NCR")
            {
                modelos.Add("NCR 2008");
                modelos.Add("NCR 3140 (EPSON TM2002 MODE)");
                modelos.Add("NCR 3140 (HASAR 615F MODE)");
                modelos.Add("NCR 3140 (HASAR 715F MODE)");
                
            }
            if (comboBox2.Text == "OLIVETTI")
            {
                modelos.Add("OLIVETTI PR4F");
                modelos.Add("OLIVETTI 320F");
                modelos.Add("OLIVETTI ARJET 20F");

            }
            if (comboBox2.Text == "SAMSUNG")
            {
                modelos.Add("BIXOLON SRP-250");
                modelos.Add("BIXOLON SRP-270 DF");
               
            }
            return modelos;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            List<string> modelos = showmodelos();
            foreach (string modelo in modelos)
            {
                comboBox3.Items.Add(modelo);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Length > 0 && comboBox2.Text.Length > 0 && comboBox3.Text.Length > 0 && checkBox1.Checked == false)
            {
                Conexion.abrir();
                SqlCeCommand grabodatosIF = new SqlCeCommand();
                grabodatosIF.Parameters.AddWithValue("@pu", comboBox1.Text);
                grabodatosIF.Parameters.AddWithValue("@ma", comboBox2.Text);
                grabodatosIF.Parameters.AddWithValue("@mo", comboBox3.Text);
                grabodatosIF.Parameters.AddWithValue("@uif", "si");
                Conexion.Actualizar("Configuracion", "PUERTO_IF = @pu ,MARCA_IF = @ma ,MODELO_IF = @mo, usaimpfiscal = @uif", "", "", grabodatosIF);
                Conexion.cerrar();
                ConfigFiscal.baudios = 9600;
                ConfigFiscal.comport = short.Parse(comboBox1.Text.Replace("COM",""));
                ConfigFiscal.marca = comboBox2.Text;
                ConfigFiscal.modelo = comboBox3.Text;
                ConfigFiscal.usaImpFiscal = "si";
                MessageBox.Show("Datos actualizados correctamente.\nPuerto:" + ConfigFiscal.comport + "\nMarca:" + ConfigFiscal.marca + "\nModelo:" + ConfigFiscal.modelo);
            }
           
            if (checkBox1.Checked == true)
            {
                Conexion.abrir();
                SqlCeCommand grabodatosIF = new SqlCeCommand();
                grabodatosIF.Parameters.AddWithValue("@pu", "0");
                grabodatosIF.Parameters.AddWithValue("@ma", "");
                grabodatosIF.Parameters.AddWithValue("@mo", "");
                grabodatosIF.Parameters.AddWithValue("@uif", "no");
                Conexion.Actualizar("Configuracion", "PUERTO_IF = @pu ,MARCA_IF = @ma ,MODELO_IF = @mo, usaimpfiscal = @uif", "", "", grabodatosIF);
                Conexion.cerrar();
                ConfigFiscal.baudios = 0;
                ConfigFiscal.comport = 0;
                ConfigFiscal.marca = " ";
                ConfigFiscal.modelo = " ";
                ConfigFiscal.usaImpFiscal = "no";
                MessageBox.Show("Datos actualizados correctamente.\nPuerto:" + ConfigFiscal.comport + "\nMarca:" + ConfigFiscal.marca + "\nModelo:" + ConfigFiscal.modelo);

                if (comboBox1.Text.Length < 1 && comboBox2.Text.Length <1  && comboBox3.Text.Length <1 && checkBox1.Checked == false)
                {
                    MessageBox.Show("Faltan ingresar mas datos");
                }
            }
        }
    }
}
