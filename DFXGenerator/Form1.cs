using DFXGenerator.JasonHelper;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DFXGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void butAutoCadDXF_Click(object sender, EventArgs e)
        {
            try
            {

                var nestedPanels = NestingManager.GetNestingPanels("Ahmet", 6);

                DXFManager dXFManager = new DXFManager(nestedPanels);
                dXFManager.GenerateScriptPanels(Application.ProductName);

                // In your code you have use ActiveProject object to get file name.
                // according to your code you can call this method like this

                //dXFManager.GenerateScriptPanels(ActiveProject.Name);

                //dXFManager.GenerateScriptPanels("SampleDXF");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Failed to Generate DXF");
            }
        }
    }
}
