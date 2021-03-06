﻿using System;
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
using System.Diagnostics;
using System.Reflection;
namespace FLAGSYSTEMPV_2017
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);

            //Loop through the running processes in with the same name 
            foreach (Process process in processes)
            {
                //Ignore the current process 
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file. 
                    if (Assembly.GetExecutingAssembly().Location.
                         Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //Return the other process instance.  
                        return process;

                    }
                }
            }
            //No other instance was found, return null.  
            return null;
        }


        private void Main_Load(object sender, EventArgs e)
        {

            if (Main.RunningInstance() != null)
            {
                MessageBox.Show("Ya hay un proceso de Flag System PV actualmente ejecutándose. ","No se pueden abrir varias instancias de Flag System PV",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Environment.Exit(0);
            } 
            SecurityIdentifier asd = GetComputerSid();
             string sid =  asd.AccountDomainSid.ToString();
            string modelNo = identifier("Win32_DiskDrive", "Model");
         
            string id  = sid + modelNo ;
            RegistrarProducto.id = id;
           
         
          //MessageBox.Show("Encripted:" );
              this.Visible = false;
            
            Conexion.abrir();
            DataTable registered = Conexion.Consultar("*", "Configuracion", "", "", new SqlCeCommand());
            if (registered.Rows.Count > 0)
            {
                SqlCeCommand myid = new SqlCeCommand();
                myid.Parameters.AddWithValue("id", id);
                DataTable consultaTest = Conexion.Consultar("*", "Configuracion", "WHERE master_user_id = @id or slavea_user_id = @id or slaveb_user_id = @id or slavec_user_id = @id ", "", myid);
                if (consultaTest.Rows.Count >= 1)
                {
                string usaimpfis = consultaTest.Rows[0][12].ToString();
                if (usaimpfis == "si") ConfigFiscal.usaImpFiscal = "si";
                if (usaimpfis == "no") ConfigFiscal.usaImpFiscal = "no";
                if (ConfigFiscal.usaImpFiscal == "si")
                {
                    ConfigFiscal.comport = short.Parse(consultaTest.Rows[0][9].ToString().Replace("COM", ""));
                    ConfigFiscal.marca = consultaTest.Rows[0][10].ToString();
                    ConfigFiscal.modelo = consultaTest.Rows[0][11].ToString();
                }
                registereduser.smtp = consultaTest.Rows[0][17].ToString();
                registereduser.puerto = consultaTest.Rows[0][18].ToString();
                registereduser.ssl = consultaTest.Rows[0][19].ToString();
                registereduser.mail = consultaTest.Rows[0][20].ToString();
                registereduser.clave = consultaTest.Rows[0][21].ToString();
                registereduser.para = consultaTest.Rows[0][22].ToString();
                registereduser.titulo = consultaTest.Rows[0][23].ToString();
                registereduser.cuerpo = consultaTest.Rows[0][24].ToString();
                registereduser.redondeo = consultaTest.Rows[0][27].ToString();
                ImpresionNOFISCAL.NONFISCALPRINTERNAME = consultaTest.Rows[0][32].ToString();
                
                if (consultaTest.Rows[0][28].ToString() == "si") registereduser.closeandbkp = "si"; else registereduser.closeandbkp = "no";
                if (consultaTest.Rows[0][29].ToString() == "si") registereduser.sololectura = "si"; else registereduser.sololectura = "no";
                if (consultaTest.Rows[0][30].ToString() == "si") registereduser.alwaysprint = "si"; else registereduser.alwaysprint = "no";
                if (consultaTest.Rows[0][31].ToString() == "si") registereduser.tooltips= "si"; else registereduser.tooltips = "no";

                    registereduser.saldoinicial = float.Parse(consultaTest.Rows[0][0].ToString());
                    Login lgn = new Login();
                    lgn.Show();
                }
                else
                {
                    MessageBox.Show("Este sistema está protegido por la licencia que actualmente tiene activada. Si usted es el dueño de esta licencia contáctenos para hacer el cambio de hardware asociado a la licencia.\nTelefonos: 4307-5103 / 5192\ne-mail:info@flag.com.ar\nHorarios:Lunes a Viernes de 09 a 18 hs.","El sistema no puede ejecutarse en esta PC",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    Environment.Exit(1);
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
