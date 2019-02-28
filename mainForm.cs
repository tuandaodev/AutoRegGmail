using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AutoRegGmail
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            //InitializeComponent();

            main_console();
        }

        private void main_console()
        {
            //MobileController CMobile = new MobileController();
            WebController CWeb = new WebController();


            Console.Write("TEST");
            //CWeb.Start();
        }

        private void btn_Go_Click(object sender, EventArgs e)
        {

        }
    }
}
