using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab14
{
    public partial class Phones : Form
    {
        public Phones()
        {
            InitializeComponent();
        }

        private void Phones_Load(object sender, EventArgs e)
        {
        }
         public void getString(string[] arr)
        {
             foreach(string q in arr)
             {
                 comboBox1.Items.Add(q);
             }
        }
    }
}
