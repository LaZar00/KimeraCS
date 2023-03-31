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
    using static FolderBrowserDialogEX;

    public partial class FrmFieldDB : Form
    {

        public static string strFieldFile = "", strAnimFile = "", strLocalModelName = "", strLocalAnimName = "";
        public static bool bSelectedFileFromDB = false;

        public FrmFieldDB()
        {
            InitializeComponent();
        }

        private void FrmFieldDB_Load(object sender, EventArgs e)
        {
            int mi;

            bSelectedFileFromDB = false;

            if (strCharLGPPathSrc == "") strCharLGPPathSrc = strGlobalPath;

            txtFieldDataDir.Text = strCharLGPPathSrc;
            cbModel.Items.Clear();

            for (mi = 0; mi < lstCharLGPRegisters.Count; mi++)
            {
                cbModel.Items.Add(lstCharLGPRegisters[mi].fileName);
            }

            // Select cbModel index and value
            if (strLocalModelName == "")
            {
                cbModel.SelectedIndex = 0;
                cbModel.SelectedValue = 0;
            }

            // Select lbAnimation index and value
            if (strLocalAnimName == "")
            {
                lbAnimation.SelectedIndex = 0;
                lbAnimation.SelectedValue = 0;
            }
            
            if (strLocalModelName == "") cbModel.SelectedIndex = 0;
            else cbModel.SelectedIndex = cbModel.FindStringExact(strLocalModelName);

            if (strLocalAnimName == "") lbAnimation.SetSelected(0, true);
            else lbAnimation.SetSelected(lbAnimation.Items.IndexOf(strLocalAnimName), true);

            cbModel.Select();
            cbModel.SelectAll();
        }

        private void BtnSaveFieldDataDir_Click(object sender, EventArgs e)
        {
            WriteCFGFile();
        }

        private void BtnSelectDirBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogEX fbdFieldDataDirectory = new FolderBrowserDialogEX();

            // We must select the directory from where to read the files.
            fbdFieldDataDirectory.folderBrowser.Description =
                            "Select the Folder where CHAR.LGP file has been extracted:";

            fbdFieldDataDirectory.folderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            fbdFieldDataDirectory.folderBrowser.ShowNewFolderButton = false;

            if (strCharLGPPathSrc != "")
            {
                fbdFieldDataDirectory.folderBrowser.SelectedPath = strCharLGPPathSrc;
            }
            else
            {
                fbdFieldDataDirectory.folderBrowser.SelectedPath = strGlobalPath;
            }

            fbdFieldDataDirectory.Tmr.Start();
            if (fbdFieldDataDirectory.folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (fbdFieldDataDirectory.folderBrowser.SelectedPath != "")
                {
                    // Put Global folder for input unswizzled.
                    strCharLGPPathSrc = fbdFieldDataDirectory.folderBrowser.SelectedPath;
                    txtFieldDataDir.Text = fbdFieldDataDirectory.folderBrowser.SelectedPath;
                }
            }

            fbdFieldDataDirectory.Dispose();
        }

        public void UpdatelbAnimation()
        {
            int ai, ni;

            lbAnimation.Items.Clear();

            for (ai = 0; ai < lstCharLGPRegisters[cbModel.SelectedIndex].lstAnims.Count; ai++)
            {
                lbAnimation.Items.Add(lstCharLGPRegisters[cbModel.SelectedIndex].lstAnims[ai]);
            }

            lblModelNames.Text = "";
            for (ni = 0; ni < lstCharLGPRegisters[cbModel.SelectedIndex].lstNames.Count - 1; ni++)
            {
                lblModelNames.Text += lstCharLGPRegisters[cbModel.SelectedIndex].lstNames[ni] + 
                                      Environment.NewLine;
            }
            lblModelNames.Text += lstCharLGPRegisters[cbModel.SelectedIndex].lstNames[ni];

            strLocalModelName = cbModel.Text;
            lbAnimation.SetSelected(0, true);

            cbModel.Select();
            cbModel.SelectAll();
        }

        private void CbModel_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdatelbAnimation();
        }

        private void CbModel_TextChanged(object sender, EventArgs e)
        {
            int iModelIdx;

            iModelIdx = cbModel.Items.IndexOf(cbModel.Text.ToUpper());

            if (iModelIdx >= 0)
            {
                cbModel.SelectedIndex = iModelIdx;
            }
        }

        private void FrmFieldDB_FormClosed(object sender, FormClosedEventArgs e)
        {
            strLocalModelName = cbModel.Text;
            strLocalAnimName = lbAnimation.Text;

            if (!bSelectedFileFromDB)
            {
                strFieldFile = "";
                strAnimFile = "";
            }
        }

        private void BtnLoadModelAnimation_Click(object sender, EventArgs e)
        {

            bSelectedFileFromDB = false;

            if (cbModel.Text != "")
                strFieldFile = txtFieldDataDir.Text + "\\" + cbModel.Text + ".HRC";
            else
            {
                MessageBox.Show("You have not selected any Field Model.", "Information", MessageBoxButtons.OK);
                return;

            }

            if (lbAnimation.Text != "")
                strAnimFile = txtFieldDataDir.Text + "\\" + lbAnimation.Text + ".A";
            else
            {
                MessageBox.Show("You have not selected any Field Animation for the model.", "Information", MessageBoxButtons.OK);
                return;

            }

            // Check existance of the files.
            if (!File.Exists(strFieldFile))
            {
                MessageBox.Show("The file selected as Field Model " + cbModel.Text + " does not exists.",
                                "Error");
                return;
            }

            if (!File.Exists(strAnimFile))
            {
                MessageBox.Show("The file selected as Field Animation " + lbAnimation.SelectedItem + " does not exists.",
                                "Error");
                return;
            }

            bSelectedFileFromDB = true;

            Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }




    }
}
