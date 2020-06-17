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
    public partial class frmModifyCat : Form
    {
        private frmMain formToControl = null;
        public frmModifyCat(frmMain formToControl)
        {
            this.formToControl = formToControl;
            InitializeComponent();
            //prepare the existing categories in the category combobox
            List<string> sel = new List<string>();
            sel = frmLogin.Stock.select_all(1, "categories");
            for (int i = 0; i < sel.Count(); i++)
            {
                cmbCat.Items.Add(sel[i]);
            }
        }
        //update category name in database
        private void btnModify_Click(object sender, EventArgs e)
        {
            string selCat = "'" + cmbCat.SelectedItem.ToString() + "'";
            string newcat = "'" + textBox1.Text + "'";
            //update categories table
            if (frmLogin.Stock.Update("categories", "NAME=" + newcat, "NAME=" + selCat))
                formToControl.refresh_cat();
            else
                MessageBox.Show("error invalid inputs");
            //update products table
            if (frmLogin.Stock.Update("PRODUCTS", "CAT=" + newcat, "CAT=" + selCat))
                this.Hide();
            else
                MessageBox.Show("error invalid inputs");

        }
    }
}
