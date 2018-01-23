using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FLAGSYSTEMPV_2017
{
    public partial class Pleasewait : Form
    {
        public Pleasewait()
        {
            InitializeComponent();
        }

        private void Pleasewait_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 8),
                      this.DisplayRectangle);  
        }
    }
}
