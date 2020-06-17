using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mabeعaty
{
    public partial class frmAddCat : Form
    {
        private frmMain formToControl = null;

        public frmAddCat(frmMain formToControl)
        {
            this.formToControl = formToControl;
            InitializeComponent();
        }

        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if(frmLogin.Stock.Insert("categories", "name", "'" + textBox1.Text + "'"))
                {

                    formToControl.refresh_cat();
                   
                    this.Hide();
                }
                else
                    MessageBox.Show("error invalid name");

            }
                
        }
    }
}
