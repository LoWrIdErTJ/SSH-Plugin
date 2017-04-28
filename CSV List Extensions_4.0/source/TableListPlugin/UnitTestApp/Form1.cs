using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TableListCommands;

namespace UnitTestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string value = @"Hallo;Peter;Maier;Guenter;Peter;Herbert
test;123"; 
            //File.ReadAllText("sample.txt");
            PluginInfo pinfo = new PluginInfo();
            List<string> lstValue = pinfo.NormalizeString(value);
            TableCSVGrid _wizardWindow = new TableCSVGrid();
            _wizardWindow.csvFileContent = value;
            _wizardWindow.rows = lstValue;
            _wizardWindow.containsHeader = true;
            _wizardWindow.delimiter = ';';
            _wizardWindow.Title = "Title goes here";
            _wizardWindow.Show();
            //TableRowInsert dlg = new TableRowInsert();
            //AddNewColumn dlg = new AddNewColumn();
//            dlg.containsHeader = false;
//            dlg.delimiter = ',';
//            dlg.MakeReadOnly(false);
            
//            dlg.csvFileContent = @"h1,h2,h3
//            l1,l2,l3
//            l4,l5,l6";
            
//            dlg.Show();
            
        }
    }
}
