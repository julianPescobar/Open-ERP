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
            if (dataGridView1.Rows.Count > 0)
            {
                getpermisos();
            }
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

      
        private void getpermisos()
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
           // var row = this.dataGridView1.Rows[rowIndex];
            string id = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
            SqlCeCommand getperms = new SqlCeCommand();
            getperms.Parameters.AddWithValue("id", id);
            Conexion.abrir();
            DataTable myperms = Conexion.Consultar("p_venta,p_compra,p_articulo,p_caja,p_clientes,p_proveedores,p_gastos,p_stock,p_cierredia,p_diferencia,p_consultaC,p_consultaV,p_EScaja,p_informes,p_anular,p_notac,p_notad,p_abstock,p_config,p_empleados,p_enviarinforme,p_fiscalconfig,p_rubro", "Usuarios", "Where iduser = @id", "", getperms);
            Conexion.cerrar();
            if (myperms.Rows[0][0].ToString() == "si") checkBox1.Checked = true; else checkBox1.Checked = false;
            if (myperms.Rows[0][1].ToString() == "si") checkBox2.Checked = true; else checkBox2.Checked = false;
            if (myperms.Rows[0][2].ToString() == "si") checkBox4.Checked = true; else checkBox4.Checked = false;
            if (myperms.Rows[0][3].ToString() == "si") checkBox3.Checked = true; else checkBox3.Checked = false;
            if (myperms.Rows[0][4].ToString() == "si") checkBox8.Checked = true; else checkBox8.Checked = false;
            if (myperms.Rows[0][5].ToString() == "si") checkBox7.Checked = true; else checkBox7.Checked = false;
            if (myperms.Rows[0][6].ToString() == "si") checkBox6.Checked = true; else checkBox6.Checked = false;
            if (myperms.Rows[0][7].ToString() == "si") checkBox5.Checked = true; else checkBox5.Checked = false;
            if (myperms.Rows[0][8].ToString() == "si") checkBox12.Checked = true; else checkBox12.Checked = false;
            if (myperms.Rows[0][9].ToString() == "si") checkBox9.Checked = true; else checkBox9.Checked = false;
            if (myperms.Rows[0][10].ToString() == "si") checkBox24.Checked = true; else checkBox24.Checked = false;
            if (myperms.Rows[0][11].ToString() == "si") checkBox23.Checked = true; else checkBox23.Checked = false;
            if (myperms.Rows[0][12].ToString() == "si") checkBox22.Checked = true; else checkBox22.Checked = false;
            if (myperms.Rows[0][13].ToString() == "si") checkBox21.Checked = true; else checkBox21.Checked = false;
            if (myperms.Rows[0][14].ToString() == "si") checkBox20.Checked = true; else checkBox20.Checked = false;
            if (myperms.Rows[0][15].ToString() == "si") checkBox19.Checked = true; else checkBox19.Checked = false;
            if (myperms.Rows[0][16].ToString() == "si") checkBox18.Checked = true; else checkBox18.Checked = false;
            if (myperms.Rows[0][17].ToString() == "si") checkBox17.Checked = true; else checkBox17.Checked = false;
            if (myperms.Rows[0][18].ToString() == "si") checkBox16.Checked = true; else checkBox16.Checked = false;
            if (myperms.Rows[0][19].ToString() == "si") checkBox15.Checked = true; else checkBox15.Checked = false;
            if (myperms.Rows[0][20].ToString() == "si") checkBox14.Checked = true; else checkBox14.Checked = false;
            if (myperms.Rows[0][21].ToString() == "si") checkBox13.Checked = true; else checkBox13.Checked = false;
            if (myperms.Rows[0][22].ToString() == "si") checkBox10.Checked = true; else checkBox10.Checked = false;
        }
        private void setpermisos()
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            // var row = this.dataGridView1.Rows[rowIndex];
            string id = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();


            string a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w;
            if (checkBox1.Checked == true) a = "si"; else a = "no";
            if( checkBox2.Checked == true) b = "si" ; else b = "no";
            if(  checkBox4.Checked == true)c = "si" ; else c = "no";
            if( checkBox3.Checked == true)d = "si"; else d = "no";
            if( checkBox8.Checked == true) e = "si"; else e="no";
            if(  checkBox7.Checked == true) f="si"; else  f="no";
            if(  checkBox6.Checked == true) g="si"; else  g="no";
            if(  checkBox5.Checked == true) h="si"; else  h="no";
            if ( checkBox12.Checked == true) i="si"; else  i="no";
            if  (checkBox9.Checked == true) j="si"; else j="no";
            if (checkBox24.Checked == true) k="si"; else  k="no";
            if ( checkBox23.Checked == true) l = "si"; else  l="no";
            if ( checkBox22.Checked == true) m="si"; else m="no";
            if ( checkBox21.Checked == true) n="si"; else n="no";
            if ( checkBox20.Checked == true) o="si"; else  o="no";
            if ( checkBox19.Checked == true) p="si"; else  p="no";
            if ( checkBox18.Checked == true) q="si"; else q="no";
            if ( checkBox17.Checked == true) r="si"; else r="no";
            if (checkBox16.Checked == true) s="si"; else  s="no";
            if ( checkBox15.Checked == true) t="si"; else t="no";
            if (checkBox14.Checked == true) u = "si"; else u = "no";
            if (checkBox13.Checked == true) v = "si"; else v = "no";
            if (checkBox10.Checked == true) w = "si"; else w = "no";
            SqlCeCommand getperms = new SqlCeCommand();
            getperms.Parameters.AddWithValue("id", id);
            getperms.Parameters.AddWithValue("a", a);
             getperms.Parameters.AddWithValue("b", b);
             getperms.Parameters.AddWithValue("c", c);
             getperms.Parameters.AddWithValue("d", d);
             getperms.Parameters.AddWithValue("e", e);
             getperms.Parameters.AddWithValue("f", f);
             getperms.Parameters.AddWithValue("g", g);
            getperms.Parameters.AddWithValue("h", h);
             getperms.Parameters.AddWithValue("i", i);
             getperms.Parameters.AddWithValue("j", j);
             getperms.Parameters.AddWithValue("k", k);
             getperms.Parameters.AddWithValue("l", l);
             getperms.Parameters.AddWithValue("m", m);
             getperms.Parameters.AddWithValue("n", n);
            getperms.Parameters.AddWithValue("o", o);
             getperms.Parameters.AddWithValue("p", p);
             getperms.Parameters.AddWithValue("q", q);
             getperms.Parameters.AddWithValue("r", r);
             getperms.Parameters.AddWithValue("s", s);
             getperms.Parameters.AddWithValue("t", t);
             getperms.Parameters.AddWithValue("u", u);
             getperms.Parameters.AddWithValue("v", v);
             getperms.Parameters.AddWithValue("w", w);
            Conexion.abrir();
            Conexion.Actualizar("Usuarios", "p_rubro = @w, p_venta =@a,p_compra =@b,p_articulo =@c,p_caja = @d,p_clientes =@e,p_proveedores =@f,p_gastos =@g,p_stock =@h,p_cierredia =@i,p_diferencia =@j,p_consultaC =@k,p_consultaV =@l,p_EScaja =@m,p_informes =@n,p_anular =@o,p_notac =@p,p_notad =@q,p_abstock =@r,p_config =@s,p_empleados =@t,p_enviarinforme =@u,p_fiscalconfig =@v", "Where iduser = @id","", getperms);
            Conexion.cerrar();
            
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                getpermisos();
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                setpermisos();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("error "+ex.Message);
            }
        }

        

      

        

    
        
        
    }
}
