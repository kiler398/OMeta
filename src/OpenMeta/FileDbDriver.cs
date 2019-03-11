using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MyMeta
{
    internal class FileDbDriver : InternalDriver
    {
        private string connPrefix;
        private string connPostfix;
        private string filemask;

        internal FileDbDriver(Type factory, string connPrefix, string connFileName, string connPostfix, string filemask)
            : base(factory, "", true)
        {
            this.connPrefix = connPrefix;
            this.connPostfix = connPostfix;
            this.filemask = filemask;

            this.LastFileName = connFileName;
        }

        private string lastFileName;

        public string LastFileName
        {
            get { return lastFileName; }
            set
            {
                lastFileName = value;
                ConnectString = connPrefix + LastFileName + connPostfix;
            }
        }

        public override string BrowseConnectionString(string connstr)
        {
            return BrowseFileConnectionString(connstr);
        }

        protected string BrowseFileConnectionString(string connstr)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if ((connstr.StartsWith(connPrefix) && (connstr.EndsWith(connPostfix))))
            {
                string filename = connstr.Substring(connPrefix.Length, connstr.Length - connPrefix.Length - connPostfix.Length);
                dlg.FileName = filename;

            }
            else
                dlg.FileName = LastFileName;

            dlg.Filter = this.filemask;
            dlg.ValidateNames = true;
            dlg.AddExtension = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LastFileName = dlg.FileName;
                return this.ConnectString;
            }

            return null;
        }



    }
}
