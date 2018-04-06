using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace FLAGSYSTEMPV_2017
{
    public partial class Extensiones : Form
    {
        public Extensiones()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Extensiones_Load(object sender, EventArgs e)
        {
            string[] files = System.IO.Directory.GetFiles(app.dir+"\\", "*.dll");
            foreach(string archivos in files)
            {
                listBox1.Items.Add(archivos.Replace(app.dir+"\\",""));
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(app.dir + "\\" + listBox1.SelectedItem.ToString());
                    Type type = assembly.GetType(listBox1.SelectedItem.ToString().Replace(".dll",""));
                    Form form = (Form)Activator.CreateInstance(type);
                    this.Close();
                    form.ShowDialog(); // Or Application.Run(form)
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else MessageBox.Show("No hay ninguna extensión seleccionada");
        }
    }
}
