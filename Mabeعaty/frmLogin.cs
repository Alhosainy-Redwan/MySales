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
    public partial class frmLogin : Form
    {
        public static Sql_interface Stock { get; set; }
        public frmLogin()
        {
            InitializeComponent();
        }
        //establishing database  stock connection
        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dictionary<String, String> opt = new Dictionary<String, String>();
            opt.Add("Data Source", "database.db3");
            opt.Add("Version", "3");
            Stock = new Sql_interface(opt);
            Dictionary<String, String> fields = new Dictionary<String, String>();
            fields.Add("NAME", "TEXT");
            if(!Stock.create_newTable("categories", fields))
            {
                MessageBox.Show("error in column or table name");
                
            }
            Dictionary<String, String> fld = new Dictionary<String, String>();
            fld.Add("NAME", "TEXT");
            fld.Add("PRICE", "TEXT");
            fld.Add("QTY", "TEXT");
            fld.Add("BARCODE", "TEXT");
            fld.Add("CAT", "TEXT");

            if (!Stock.create_newTable("PRODUCTS", fld))
            {
                MessageBox.Show("error in column or table name");
            }

            new Mabeعaty.frmMain().Show();
           
        }
    }
}
