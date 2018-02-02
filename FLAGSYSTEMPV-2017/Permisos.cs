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
    public partial class Permisos : Form
    {
        public Permisos()
        {
            InitializeComponent();
        }

        private void Permisos_Load(object sender, EventArgs e)
        {
            this.Focus();
            Conexion.abrir();
            SqlCeCommand notelim = new SqlCeCommand();
            notelim.Parameters.AddWithValue("elim", "Eliminado");
            DataTable showacls = Conexion.Consultar("iduser,login as [Nombre Usuario],level as [Jerarquía],nombreusuario as [Nombre Real]", "Usuarios", " WHERE eliminado != @elim and level = 'Vendedor'", "", notelim);
            Conexion.cerrar();
            BindingSource SBind = new BindingSource();
            SBind.DataSource = showacls;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = showacls;
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.DataSource = SBind;
            dataGridView1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
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

        private void Permisos_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                   this.DisplayRectangle);      
        }

        
    }
}
