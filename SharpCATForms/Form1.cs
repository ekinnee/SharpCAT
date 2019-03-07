using System.Windows.Forms;
using SharpCATLib;

namespace SharpCATForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            SharpCAT sharpCAT = new SharpCAT();
            
            InitializeComponent();

            ComPortListBox.DataSource = sharpCAT.PortNames;
        }
    }
}