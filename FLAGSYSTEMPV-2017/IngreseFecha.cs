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

        private void button1_Click(object sender, EventArgs e)
        {
            string fecha = dateTimePicker1.Value.ToShortDateString() + " 00:00:00";
            SqlCeCommand mifecha = new SqlCeCommand();
            mifecha.Parameters.AddWithValue("fecha",fecha);
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
                DateTime fechaactual = DateTime.Parse(dateTimePicker1.Value.ToShortDateString()+" 00:00:00");
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
                        Inicio frm = new Inicio();
                        frm.Show();
                    }
                }
                else MessageBox.Show("No se puede volver a una fecha pasada, la ultima fecha de trabajo fue el "+ultimafecha+". Si se equivocó a la hora de ingresar la nueva fecha pídale a un supervisor que ingrese la fecha correspondiente.");
            }
            else
            {
                    string fechasupervisor = dateTimePicker1.Value.ToShortDateString() + " 00:00:00";
                    Conexion.abrir();
                    SqlCeCommand fechaS = new SqlCeCommand();
                    fechaS.Parameters.AddWithValue("ufs",fechasupervisor);
                    Conexion.Actualizar("Configuracion","ultimafechatrabajo = @ufs, fechatrabajoactual = @ufs","","", fechaS);
                    Conexion.cerrar();
                    if (Application.OpenForms.OfType<Inicio>().Count() == 1)
                        Application.OpenForms.OfType<Inicio>().First().Focus();
                    else
                    {
                        Inicio frm = new Inicio();
                        frm.Show();
                    }
            }
        }

        private void IngreseFecha_Load(object sender, EventArgs e)
        {
            Conexion.abrir();
            DataTable fechasdb = Conexion.Consultar("ultimafechatrabajo", "Configuracion", "", "", new SqlCeCommand());
            Conexion.cerrar();
             string ulfecha = fechasdb.Rows[0][0].ToString();
            
             if (ulfecha.Length < 1)
             {
                 ulfecha = "Nunca";
             }
             label3.Text = "Ultima fecha de trabajo: " + ulfecha;
        }

        private void IngreseFecha_Paint(object sender, PaintEventArgs e)
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

    }
}
