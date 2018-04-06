using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.Diagnostics;

namespace FLAGSYSTEMPV_2017
{
    public partial class impnofiscal : Form
    {
        public impnofiscal()
        {
            InitializeComponent();
        }

        private void impnofiscal_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                comboBox1.Items.Add(printer);
            }
            //SqlCeCommand saconombre = new SqlCeCommand();
            Conexion.abrir();
            DataTable myimpresora = Conexion.Consultar("nombreimpnofiscal", "Configuracion", "", "", new SqlCeCommand());
            Conexion.cerrar();
            if (myimpresora.Rows.Count > 0)
            {
                ImpresionNOFISCAL.NONFISCALPRINTERNAME = myimpresora.Rows[0][0].ToString();
                //Messa
                //MessageBox.Show(ImpresionNOFISCAL.NONFISCALPRINTERNAME);
                try
                {
                    comboBox1.SelectedItem = ImpresionNOFISCAL.NONFISCALPRINTERNAME;
                }
                catch (Exception) { }
            }
            comboBox1.DroppedDown = true;
            comboBox1.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                SqlCeCommand saconombre = new SqlCeCommand();
                saconombre.Parameters.AddWithValue("pr", comboBox1.SelectedItem.ToString());
                Conexion.abrir();
                Conexion.Actualizar("Configuracion", "nombreimpnofiscal = @pr", "", "", saconombre);
                    Conexion.cerrar();
                    ImpresionNOFISCAL.NONFISCALPRINTERNAME = comboBox1.SelectedItem.ToString(); 
                    MessageBox.Show("Impresora seleccionada:\n" + comboBox1.SelectedItem.ToString());
                    this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "rundll32.exe";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "shell32.dll,SHHelpShortcuts_RunDLL AddPrinter";
            p.StartInfo.UseShellExecute = true;
            p.EnableRaisingEvents = true;
            p.SynchronizingObject = this;
            p.Exited += new EventHandler(process_Exited);
            p.Start();
            this.Enabled = false;
        }
        void process_Exited(object sender, EventArgs e)
        {
            this.Enabled = true;
            if (Application.OpenForms.OfType<impnofiscal>().Count() >= 1)
            {
                Application.OpenForms.OfType<impnofiscal>().First().Close();
                impnofiscal openagain = new impnofiscal();
                openagain.Show();
            }
        }
    }
}
