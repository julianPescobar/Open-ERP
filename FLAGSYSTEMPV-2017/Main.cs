using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;
using System.Management;
using System.Security.Principal;
using System.DirectoryServices;
using System.Security.Cryptography;
namespace FLAGSYSTEMPV_2017
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

          //      SecurityIdentifier asd = GetComputerSid();
           //   string sid =  asd.AccountDomainSid.ToString();
          //  string modelNo = identifier("Win32_DiskDrive", "Model");
         
          //  string userID  = sid + modelNo ; 
          
         
            //MessageBox.Show("Encripted:" );
              this.Visible = false;
            
            Conexion.abrir();
            DataTable registered = Conexion.Consultar("*", "Configuracion", "", "", new SqlCeCommand());
            if (registered.Rows.Count > 0)
            {
                SqlCeCommand myid = new SqlCeCommand();
               // myid.Parameters.AddWithValue("id", userID);
                DataTable consultaTest = Conexion.Consultar("*", "Configuracion",/* "WHERE master_user_id = @id or slavea_user_id = @id or slaveb_user_id = @id or slavec_user_id = @id "*/"", "", myid);

                if (consultaTest.Rows.Count >= 1)
                {/*
                string usaimpfis = consultaTest.Rows[0][21].ToString();
                if (usaimpfis == "si") ConfigFiscal.usaImpFiscal = "si";
                if (usaimpfis == "no") ConfigFiscal.usaImpFiscal = "no";
                if (ConfigFiscal.usaImpFiscal == "si")
                {
                    ConfigFiscal.comport = short.Parse(consultaTest.Rows[0][18].ToString().Replace("COM", ""));
                    ConfigFiscal.marca = consultaTest.Rows[0][19].ToString();
                    ConfigFiscal.modelo = consultaTest.Rows[0][20].ToString();
                }
                */

                    registereduser.saldoinicial = float.Parse(consultaTest.Rows[0][0].ToString());
                    Login lgn = new Login();
                    lgn.Show();
                }
                else
                {
                    MessageBox.Show("Esta base de datos está protegida por la licencia. Si sos el dueño de esta licencia contactanos para hacer el cambio de dispositivo asociado a la licencia");

                }
            }
            else
            {
                RegistrarProducto reg = new RegistrarProducto();
                reg.ShowDialog();
            }
           

            Conexion.cerrar();
        }

        string getprocid()
        {
            ManagementObjectCollection mbsList = null;
    ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
    mbsList = mbs.Get();
    string id = "";
    foreach (ManagementObject mo in mbsList)
    {
        id = mo["ProcessorID"].ToString();
    }
    return id;
        }

        public static SecurityIdentifier GetComputerSid()
        {
            return new SecurityIdentifier((byte[])new DirectoryEntry(string.Format("WinNT://{0},Computer", Environment.MachineName)).Children.Cast<DirectoryEntry>().First().InvokeGet("objectSID"), 0).AccountDomainSid;
        }



        private string identifier(string wmiClass, string wmiProperty)
        //Return a hardware identifier
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                //Only get the first one
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            return result;
        }
      


    }
}
