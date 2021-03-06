﻿using System;
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
    public partial class IngreseFecha : Form
    {
        public IngreseFecha()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Environment.Exit(0);
        }
        string ulfecha;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string fecha = Convert.ToDateTime(maskedTextBox1.Text).ToShortDateString() + " 00:00:00";
                SqlCeCommand mifecha = new SqlCeCommand();
                mifecha.Parameters.AddWithValue("fecha", fecha);
                Conexion.abrir();
                DataTable fechasdb = Conexion.Consultar("ultimafechatrabajo,fechatrabajoactual", "Configuracion", "", "", new SqlCeCommand());
                Conexion.cerrar();
                string ulfecha = fechasdb.Rows[0][0].ToString();
                string fechaac = fechasdb.Rows[0][1].ToString();
                if (ulfecha.Length < 1)
                {
                    Conexion.abrir();
                    Conexion.Actualizar("Configuracion", "ultimafechatrabajo = @fecha", "", "", mifecha);
                    Conexion.cerrar();
                    ulfecha = fecha;
                }
                if (fechaac.Length < 1)
                {
                    Conexion.abrir();
                    Conexion.Actualizar("Configuracion", "fechatrabajoactual = @fecha", "", "", mifecha);
                    Conexion.cerrar();
                    fechaac = fecha;
                }
                //string ulfecha = fechasdb.Rows[0][0].ToString() + " 00:00:00";
                //string fechaac = fechasdb.Rows[0][1].ToString() + " 00:00:00";
                if (registereduser.level == "Vendedor")
                {
                    DateTime fechaactual = DateTime.Parse(Convert.ToDateTime(maskedTextBox1.Text).ToShortDateString() + " 00:00:00");
                    DateTime ultimafecha = DateTime.Parse(ulfecha);
                    if (DateTime.Compare(fechaactual, ultimafecha) >= 0)
                    {
                        Conexion.abrir();
                        SqlCeCommand fechaS = new SqlCeCommand();
                        fechaS.Parameters.AddWithValue("ufs", fechaactual);
                        Conexion.Actualizar("Configuracion", "ultimafechatrabajo = @ufs, fechatrabajoactual = @ufs", "", "", fechaS);
                        Conexion.cerrar();
                        if (Application.OpenForms.OfType<Inicio>().Count() == 1)
                            Application.OpenForms.OfType<Inicio>().First().Focus();
                        else
                        {
                            app.hoy = fechaactual.ToShortDateString();
                            this.Close();
                            Inicio frm = new Inicio();
                            frm.Show();
                        }
                    }
                    else MessageBox.Show("No se puede volver a una fecha pasada, la ultima fecha de trabajo fue el " + ultimafecha.ToShortDateString() + ". Si se equivocó a la hora de ingresar la nueva fecha pídale a un supervisor que ingrese la fecha correspondiente.");
                }
                else
                {
                    string fechasupervisor = Convert.ToDateTime(maskedTextBox1.Text).ToShortDateString() + " 00:00:00";
                    Conexion.abrir();
                    SqlCeCommand fechaS = new SqlCeCommand();
                    fechaS.Parameters.AddWithValue("ufs", fechasupervisor);
                    Conexion.Actualizar("Configuracion", "ultimafechatrabajo = @ufs, fechatrabajoactual = @ufs", "", "", fechaS);
                    Conexion.cerrar();
                    if (Application.OpenForms.OfType<Inicio>().Count() == 1)
                        Application.OpenForms.OfType<Inicio>().First().Focus();
                    else
                    {
                        app.hoy = Convert.ToDateTime(fechasupervisor).ToShortDateString();
                        this.Close();
                        Inicio frm = new Inicio();
                        frm.Show();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Revise que la fecha esté bien ingresada");
            }
        }

        private void IngreseFecha_Load(object sender, EventArgs e)
        {
            Conexion.abrir();
            DataTable fechasdb = Conexion.Consultar("ultimafechatrabajo", "Configuracion", "", "", new SqlCeCommand());
            Conexion.cerrar();
              ulfecha = fechasdb.Rows[0][0].ToString();
            
             if (ulfecha.Length < 1)
             {
                 ulfecha = DateTime.Now.ToShortDateString();
             }
             label3.Text = "Ultima fecha de trabajo: " + Convert.ToDateTime(ulfecha).ToShortDateString();
             maskedTextBox1.Text = Convert.ToDateTime(ulfecha).ToShortDateString();
            

                    
        }

        private void IngreseFecha_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                         this.DisplayRectangle);    
        }
        

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            if (registereduser.tooltips == "si")
            {
                ToolTip tt = new ToolTip();
                tt.IsBalloon = false;

                tt.ShowAlways = true;
                tt.UseAnimation = true;
                tt.ToolTipTitle = "Tips fecha trabajo:";
                tt.Show("Usted puede adelantar la fecha de trabajo siendo vendedor o supervisor.\nSolo los Supervisores pueden atrasar la fecha de trabajo", maskedTextBox1);
            }
            }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void IngreseFecha_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyCode == Keys.Enter && button1.Focused == false) SendKeys.SendWait("{TAB}");
        }

        private void maskedTextBox1_Leave(object sender, EventArgs e)
        {
            try
            {
                maskedTextBox1.Text = Convert.ToDateTime(maskedTextBox1.Text + " 00:00:00").ToShortDateString();
            }
            catch(Exception)
            {
                maskedTextBox1.Text = ulfecha;
            }
        }

        private void maskedTextBox1_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate { SetMaskedTextBoxSelectAll((MaskedTextBox)sender); });
        }
        private void SetMaskedTextBoxSelectAll(MaskedTextBox txtbox)
        {
            txtbox.SelectAll();
            txtbox.Clear();
        }

    }
}
