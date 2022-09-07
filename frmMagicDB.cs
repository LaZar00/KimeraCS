using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KimeraCS
{
    using static FileTools;

    public partial class frmMagicDB : Form
    {
        public static string strMagicFile = "", strMagicAnimFile = "";
        public static string strLocalMagicModelName = "";

        public static bool bSelectedMagicFileFromDB;

        public frmMagicDB()
        {
            InitializeComponent();
        }

        private void frmMagicDB_Load(object sender, EventArgs e)
        {
            int mi;

            bSelectedMagicFileFromDB = false;

            if (strMagicLGPPathSrc == "") strMagicLGPPathSrc = strGlobalPath;
            txtMagicDataDir.Text = strMagicLGPPathSrc;

            lbMagic.Items.Clear();

            if (bDBMagicLoaded)
            {
                for (mi = 0; mi < lstMagicLGPRegisters.Count; mi++)
                    lbMagic.Items.Add(lstMagicLGPRegisters[mi].modelName);

                if (strLocalMagicModelName == "")
                {
                    lbMagic.SelectedIndex = 0;
                }
                else
                {
                    lbMagic.SelectedIndex = lbMagic.FindStringExact(strLocalMagicModelName);
                }

                lbMagic.Select();
            }
        }

        private void btnSelectDirBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogEX fbdMagicDataDirectory = new FolderBrowserDialogEX();

            // We must select the directory from where to read the files.
            fbdMagicDataDirectory.folderBrowser.Description =
                            "Select the Folder where MAGIC.LGP file has been extracted:";

            fbdMagicDataDirectory.folderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            fbdMagicDataDirectory.folderBrowser.ShowNewFolderButton = false;

            if (strMagicLGPPathSrc != "")
            {
                fbdMagicDataDirectory.folderBrowser.SelectedPath = strMagicLGPPathSrc;
            }
            else
            {
                fbdMagicDataDirectory.folderBrowser.SelectedPath = strGlobalPath;
            }

            fbdMagicDataDirectory.Tmr.Start();
            if (fbdMagicDataDirectory.folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (fbdMagicDataDirectory.folderBrowser.SelectedPath != "")
                {
                    // Put Global folder for input unswizzled.
                    strMagicLGPPathSrc = fbdMagicDataDirectory.folderBrowser.SelectedPath;
                    txtMagicDataDir.Text = fbdMagicDataDirectory.folderBrowser.SelectedPath;
                }
            }

            fbdMagicDataDirectory.Dispose();
        }

        private void btnSaveMagicDataDir_Click(object sender, EventArgs e)
        {
            WriteCFGFile();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            strLocalMagicModelName = lbMagic.Text;

            if (!bSelectedMagicFileFromDB)
            {
                strLocalMagicModelName = "";
            }
        }

        private void lbMagic_DoubleClick(object sender, EventArgs e)
        {
            if (lbMagic.SelectedIndex > -1)
            {
                btnLoadModelAnimation.PerformClick();
            }
        }

        private void btnLoadModelAnimation_Click(object sender, EventArgs e)
        {

            bSelectedMagicFileFromDB = false;

            if (lbMagic.Text != "")
            {
                strMagicFile = txtMagicDataDir.Text + "\\" + lstMagicLGPRegisters[lbMagic.SelectedIndex].fileName + ".D";
            }
            else
            {
                MessageBox.Show("You have not selected any Magic Model.", "Information", MessageBoxButtons.OK);
                return;
            }

            strMagicAnimFile = txtMagicDataDir.Text + "\\" + lstMagicLGPRegisters[lbMagic.SelectedIndex].fileName + ".A00";

            // Check existance of the files.
            if (!File.Exists(strMagicFile))
            {
                MessageBox.Show("The file selected as Magic Model " + lstMagicLGPRegisters[lbMagic.SelectedIndex].fileName + " does not exists.",
                                "Error");
                return;
            }

            if (!File.Exists(strMagicAnimFile))
            {
                 MessageBox.Show("The file selected as Field Animation " + lstMagicLGPRegisters[lbMagic.SelectedIndex].fileName + ".A00" + " does not exists.",
                                 "Error");
                 return;
            }

            bSelectedMagicFileFromDB = true;

            Close();
        }

    }
}
