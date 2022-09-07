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

    public partial class frmBattleDB : Form
    {

        public static string strBattleFile = "", strBattleAnimFile = "";
        public static string strLocalEnemyModelName = "", strLocalLocationModelName = "", strLocalMainPCModelName = "";
        
        public static bool bSelectedBattleFileFromDB;


        public frmBattleDB()
        {
            InitializeComponent();
        }

        private void frmBattleDB_Load(object sender, EventArgs e)
        {
            int mi;

            bSelectedBattleFileFromDB = false;

            if (strBattleLGPPathSrc == "") strBattleLGPPathSrc = strGlobalPath;
            txtBattleDataDir.Text = strBattleLGPPathSrc;
            
            lbEnemies.Items.Clear();
            lbLocations.Items.Clear();
            lbMainPCs.Items.Clear();

            // Enemies
            if (bDBEnemiesLoaded)
            {
                for (mi = 0; mi < lstBattleEnemiesLGPRegisters.Count; mi++)
                    lbEnemies.Items.Add(lstBattleEnemiesLGPRegisters[mi].modelName);

                tcBattleDB.TabPages[0].Enabled = true;

                if (strLocalEnemyModelName == "")
                {
                    lbEnemies.SelectedIndex = 0;
                    lbEnemies.SelectedValue = 0;
                }
                else
                {
                    lbEnemies.SelectedIndex = lbEnemies.FindStringExact(strLocalEnemyModelName);
                }

                lbEnemies.Select();
            }
            else tcBattleDB.TabPages[0].Enabled = false;


            // Locations
            if (bDBLocationsLoaded)
            {
                for (mi = 0; mi < lstBattleLocationsLGPRegisters.Count; mi++)
                    lbLocations.Items.Add(lstBattleLocationsLGPRegisters[mi].modelName);

                tcBattleDB.TabPages[1].Enabled = true;

                if (strLocalLocationModelName == "")
                {
                    lbLocations.SelectedIndex = 0;
                    lbLocations.SelectedValue = 0;
                }
                else
                {
                    lbLocations.SelectedIndex = lbLocations.FindStringExact(strLocalLocationModelName);
                }

                lbLocations.Select();
            }
            else tcBattleDB.TabPages[1].Enabled = false;


            // Main PCs
            if (bDBMainPCsLoaded)
            {
                for (mi = 0; mi < lstBattleMainsLGPRegisters.Count; mi++)
                    lbMainPCs.Items.Add(lstBattleMainsLGPRegisters[mi].modelName);

                tcBattleDB.TabPages[2].Enabled = true;

                if (strLocalMainPCModelName == "")
                {
                    lbMainPCs.SelectedIndex = 0;
                    lbMainPCs.SelectedValue = 0;
                }
                else
                {
                    lbMainPCs.SelectedIndex = lbMainPCs.FindStringExact(strLocalMainPCModelName);
                }

                lbMainPCs.Select();
            }
            else tcBattleDB.TabPages[2].Enabled = false;
        }

        private void btnSelectDirBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogEX fbdBattleDataDirectory = new FolderBrowserDialogEX();

            // We must select the directory from where to read the files.
            fbdBattleDataDirectory.folderBrowser.Description =
                            "Select the Folder where BATTLE.LGP file has been extracted:";

            fbdBattleDataDirectory.folderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            fbdBattleDataDirectory.folderBrowser.ShowNewFolderButton = false;

            if (strBattleLGPPathSrc != "")
            {
                fbdBattleDataDirectory.folderBrowser.SelectedPath = strBattleLGPPathSrc;
            }
            else
            {
                fbdBattleDataDirectory.folderBrowser.SelectedPath = strGlobalPath;
            }

            fbdBattleDataDirectory.Tmr.Start();
            if (fbdBattleDataDirectory.folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (fbdBattleDataDirectory.folderBrowser.SelectedPath != "")
                {
                    // Put Global folder for input unswizzled.
                    strBattleLGPPathSrc = fbdBattleDataDirectory.folderBrowser.SelectedPath;
                    txtBattleDataDir.Text = fbdBattleDataDirectory.folderBrowser.SelectedPath;
                }
            }

            fbdBattleDataDirectory.Dispose();
        }

        private void lbEnemies_DoubleClick(object sender, EventArgs e)
        {
            if (lbEnemies.SelectedIndex > -1) btnLoadModelAnimation.PerformClick();
        }

        private void lbLocations_DoubleClick(object sender, EventArgs e)
        {
            if (lbEnemies.SelectedIndex > -1) btnLoadModelAnimation.PerformClick();
        }

        private void lbMainPCs_DoubleClick(object sender, EventArgs e)
        {
            if (lbEnemies.SelectedIndex > -1) btnLoadModelAnimation.PerformClick();
        }

        private void btnSaveBattleDataDir_Click(object sender, EventArgs e)
        {
            WriteCFGFile();
        }

        private void frmBattleDB_FormClosed(object sender, FormClosedEventArgs e)
        {
            switch(tcBattleDB.SelectedIndex)
            {
                case 0:
                    strLocalEnemyModelName = lbEnemies.Text;
                    break;
                case 1:
                    strLocalLocationModelName = lbLocations.Text;
                    break;
                case 2:
                    strLocalMainPCModelName = lbMainPCs.Text;
                    break;
            }

            if (!bSelectedBattleFileFromDB)
            {
                strLocalEnemyModelName = "";
                strLocalLocationModelName = "";
                strLocalMainPCModelName = "";
            }
        }

        private void btnLoadModelAnimation_Click(object sender, EventArgs e)
        {
            string strModelName = "";

            bSelectedBattleFileFromDB = false;

            switch (tcBattleDB.SelectedIndex)
            {
                case 0:
                    strModelName = lbEnemies.Text.Substring(1, 4);
                    break;
                case 1:
                    strModelName = lbLocations.Text.Substring(1, 4);
                    break;
                case 2:
                    strModelName = lbMainPCs.Text.Substring(1, 4);
                    break;
            }

            if (strModelName != "")
            {
                strBattleFile = txtBattleDataDir.Text + "\\" + strModelName;
            }
            else
            {
                MessageBox.Show("You have not selected any Battle Model.", "Information", MessageBoxButtons.OK);
                return;
            }

            strBattleAnimFile = txtBattleDataDir.Text + "\\" + strModelName.Substring(0, 2) + "DA";

            // Check existance of the files.
            if (!File.Exists(strBattleFile))
            {
                MessageBox.Show("The file selected as Battle Model " + strModelName + " does not exists.",
                                "Error");
                return;
            }

            if (tcBattleDB.SelectedIndex != 1)
            {
                if (!File.Exists(strBattleAnimFile))
                {
                    MessageBox.Show("The file supposed to be the Battle Animation Pack " + strModelName.Substring(0, 2) + "DA" + " does not exists.",
                                    "Error");
                    return;
                }
            }

            bSelectedBattleFileFromDB = true;

            Close();
        }

    }
}
