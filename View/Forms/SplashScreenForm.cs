/*============================================================
*
* File:     NewFileForm.cs
* Authors:  Damian Gabriel-Mihai
* Purpose:  A form that appears briefly before startup, acting
*           as a decorative splash screen.
*
===========================================================*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View.Forms
{
    public partial class SplashScreenForm : Form
    {
        public SplashScreenForm()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
