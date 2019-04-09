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
using RazorEngine;
using RazorEngine.Templating;

namespace OMeta.WindowsFormsApp
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string template = File.ReadAllText("DataBaseDoc.cshtml");
            var result =
                Engine.Razor.RunCompile(template, "templateKey", null, new { database = Program.OMeta.DefaultDatabase });

            File.WriteAllText("数据库文档.docx", result);

            this.tsslbottom.Text = "数据库文档生成成功！";
        }
    }
}
