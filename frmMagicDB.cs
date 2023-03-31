using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace KimeraCS
{
    using static FileTools;

    public partial class FrmMagicDB : Form
    {
        public static string strMagicFile = "", strMagicAnimFile = "";
        public static string strLocalMagicModelName = "";

        public static bool bSelectedMagicFileFromDB;

        public FrmMagicDB()
        {
            InitializeComponent();
        }

        private void FrmMagicDB_Load(object sender, EventArgs e)
        {
            int mi;

            bSelectedMagicFileFromDB = false;

            if (strMagicLGPPathSrc == "") strMagicLGPPathSrc = strGlobalPath;
            txtMagicDataDir.Text = strMagicLGPPathSrc;


            //Set Double buffering on the Grid using reflection and the bindingflags enum.
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                                              BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null,
                                              dgvMagic, new object[] { true });

            // Magic
            if (bDBMagicLoaded)
            {
                dgvMagic.Rows.Clear();
                dgvMagic.Refresh();

                for (mi = 0; mi < lstMagicLGPRegisters.Count; mi++)
                {
                    dgvMagic.Rows.Insert(mi, mi,
                                             lstMagicLGPRegisters[mi].fileName,
                                             lstMagicLGPRegisters[mi].modelName);
                }

                if (strLocalMagicModelName == "")
                {
                    dgvMagic.Rows[0].Selected = true;
                }
                else
                {
                    DataGridViewRow dgvRow = dgvMagic.Rows
                                                .Cast<DataGridViewRow>()
                                                .Where(r => r.Cells[1].Value.ToString().Equals(strLocalMagicModelName))
                                                .First();

                    dgvMagic.CurrentCell = dgvMagic.Rows[dgvRow.Index].Cells[1];
                    dgvMagic.Rows[dgvRow.Index].Selected = true;
                }
            }
        }

        private void BtnSelectDirBrowser_Click(object sender, EventArgs e)
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

        private void BtnSaveMagicDataDir_Click(object sender, EventArgs e)
        {
            WriteCFGFile();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            strLocalMagicModelName = dgvMagic.Rows[dgvMagic.SelectedRows[0].Index].Cells[1].Value.ToString();

            if (!bSelectedMagicFileFromDB)
            {
                strLocalMagicModelName = "";
            }
        }

        private void DgvMagic_DoubleClick(object sender, EventArgs e)
        {
            if (dgvMagic.SelectedRows.Count > 0) btnLoadModelAnimation.PerformClick();
        }

        private void FrmMagicDB_FormClosed(object sender, FormClosedEventArgs e)
        {
            strLocalMagicModelName = dgvMagic.Rows[dgvMagic.SelectedRows[0].Index].Cells[1].Value.ToString();

            if (!bSelectedMagicFileFromDB)
            {
                strLocalMagicModelName = "";
            }
        }

        private void BtnLoadModelAnimation_Click(object sender, EventArgs e)
        {
            string strModelName;

            bSelectedMagicFileFromDB = false;

            if (dgvMagic.SelectedRows.Count > 0)
            {
                strModelName = dgvMagic.Rows[dgvMagic.SelectedRows[0].Index].Cells[1].Value.ToString();

                if (strModelName != "")
                {
                    strMagicFile = txtMagicDataDir.Text + "\\" + strModelName + ".D";
                }
                else
                {
                    MessageBox.Show("You have not selected any Magic Model.", "Information", MessageBoxButtons.OK);
                    return;
                }

                strMagicAnimFile = txtMagicDataDir.Text + "\\" + strModelName + ".A00";


                // Check existance of the files.
                if (!File.Exists(strMagicFile))
                {
                    MessageBox.Show("The file selected as Magic Model " + strModelName + ".D does not exists.",
                                    "Error");
                    return;
                }

                if (!File.Exists(strMagicAnimFile))
                {
                    MessageBox.Show("The file selected as Magic Model Animation " + strModelName + ".A00 does not exists.",
                                    "Error");
                    return;
                }

                bSelectedMagicFileFromDB = true;

                Close();
            }
        }



    }
}
