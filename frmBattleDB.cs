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

    public partial class FrmBattleDB : Form
    {

        public static string strBattleFile = "", strBattleAnimFile = "";
        public static string strLocalEnemyModelName = "", 
                             strLocalLocationModelName = "", 
                             strLocalMainPCModelName = "";
        
        public static bool bSelectedBattleFileFromDB;

        public string typedChars = string.Empty;
        public static int iTabPageSelected;

        DataGridViewColumn tmpdgvEnemiesSortColumn;
        SortOrder tmpdgvEnemiesSortOrder;

        public FrmBattleDB()
        {
            InitializeComponent();
        }

        private void FrmBattleDB_Load(object sender, EventArgs e)
        {
            int mi;

            bSelectedBattleFileFromDB = false;

            if (strBattleLGPPathSrc == "") strBattleLGPPathSrc = strGlobalPath;
            txtBattleDataDir.Text = strBattleLGPPathSrc;
            

            //Set Double buffering on the Grid using reflection and the bindingflags enum.
            typeof(DataGridView).InvokeMember("DoubleBuffered", 
                                              BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null,
                                              dgvEnemies, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                                              BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null,
                                              dgvLocations, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                                              BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null,
                                              dgvMainPCs, new object[] { true });

            // Enemies
            if (bDBEnemiesLoaded)
            {
                dgvEnemies.Rows.Clear();
                dgvEnemies.Refresh();

                for (mi = 0; mi < lstBattleEnemiesLGPRegisters.Count; mi++)
                {
                    dgvEnemies.Rows.Insert(mi, lstBattleEnemiesLGPRegisters[mi].fileName,
                                               lstBattleEnemiesLGPRegisters[mi].modelName);

                    dgvEnemies.Rows[mi].HeaderCell.Value = mi.ToString();
                }
                    

                tcBattleDB.TabPages[0].Enabled = true;

                if (strLocalEnemyModelName == "")
                {
                    dgvEnemies.Rows[0].Selected = true;
                }
                else
                {
                    DataGridViewRow dgvRow = dgvEnemies.Rows
                                                .Cast<DataGridViewRow>()
                                                .Where(r => r.Cells[0].Value.ToString().Equals(strLocalEnemyModelName))
                                                .First();

                    dgvEnemies.CurrentCell = dgvEnemies.Rows[dgvRow.Index].Cells[0];
                    dgvEnemies.Rows[dgvRow.Index].Selected = true;
                }

            }
            else tcBattleDB.TabPages[0].Enabled = false;


            // Locations
            if (bDBLocationsLoaded)
            {
                dgvLocations.Rows.Clear();
                dgvLocations.Refresh();

                for (mi = 0; mi < lstBattleLocationsLGPRegisters.Count; mi++)
                {
                    dgvLocations.Rows.Insert(mi, lstBattleLocationsLGPRegisters[mi].fileName,
                                                 lstBattleLocationsLGPRegisters[mi].modelName);

                    dgvLocations.Rows[mi].HeaderCell.Value = mi.ToString();
                }

                tcBattleDB.TabPages[1].Enabled = true;

                if (strLocalLocationModelName == "")
                {
                    dgvLocations.Rows[0].Selected = true;
                }
                else
                {
                    DataGridViewRow dgvRow = dgvLocations.Rows
                                                .Cast<DataGridViewRow>()
                                                .Where(r => r.Cells[0].Value.ToString().Equals(strLocalLocationModelName))
                                                .First();

                    dgvLocations.CurrentCell = dgvLocations.Rows[dgvRow.Index].Cells[0];
                    dgvLocations.Rows[dgvRow.Index].Selected = true;
                }
            }
            else tcBattleDB.TabPages[1].Enabled = false;


            // Main PCs
            if (bDBMainPCsLoaded)
            {
                dgvMainPCs.Rows.Clear();
                dgvMainPCs.Refresh();

                for (mi = 0; mi < lstBattleMainPCsLGPRegisters.Count; mi++)
                {
                    dgvMainPCs.Rows.Insert(mi, lstBattleMainPCsLGPRegisters[mi].fileName,
                                               lstBattleMainPCsLGPRegisters[mi].modelName);

                    dgvMainPCs.Rows[mi].HeaderCell.Value = mi.ToString();
                }

                tcBattleDB.TabPages[2].Enabled = true;

                if (strLocalMainPCModelName == "")
                {
                    dgvMainPCs.Rows[0].Selected = true;
                }
                else
                {
                    DataGridViewRow dgvRow = dgvMainPCs.Rows
                                                .Cast<DataGridViewRow>()
                                                .Where(r => r.Cells[0].Value.ToString().Equals(strLocalMainPCModelName))
                                                .First();

                    dgvMainPCs.CurrentCell = dgvMainPCs.Rows[dgvRow.Index].Cells[0];
                    dgvMainPCs.Rows[dgvRow.Index].Selected = true;
                }
            }
            else tcBattleDB.TabPages[2].Enabled = false;

            tcBattleDB.TabIndex = iTabPageSelected;

            if (iTabPageSelected == 0)
            {
                if (tmpdgvEnemiesSortColumn != null && tmpdgvEnemiesSortColumn.Index == 1 )
                {
                    if (tmpdgvEnemiesSortOrder == SortOrder.Ascending)
                        dgvEnemies.Sort(dgvEnemies.SortedColumn, ListSortDirection.Ascending);
                    else if (tmpdgvEnemiesSortOrder == SortOrder.Descending)
                        dgvEnemies.Sort(dgvEnemies.SortedColumn, ListSortDirection.Descending);
                }
                
                dgvEnemies.Select();
            }
        }

        private void BtnSelectDirBrowser_Click(object sender, EventArgs e)
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

        private void DgvLocations_DoubleClick(object sender, EventArgs e)
        {
            if (dgvLocations.SelectedRows.Count > 0) btnLoadModelAnimation.PerformClick();
        }

        private void DgvMainPCs_DoubleClick(object sender, EventArgs e)
        {
            if (dgvMainPCs.SelectedRows.Count > 0) btnLoadModelAnimation.PerformClick();
        }

        private void DgvEnemies_DoubleClick(object sender, EventArgs e)
        {
            if (dgvEnemies.SelectedRows.Count > 0) btnLoadModelAnimation.PerformClick();
        }

        private void DgvEnemies_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) || Char.IsWhiteSpace(e.KeyChar))
            {
                typedChars += e.KeyChar.ToString().ToUpper();

                List<DataGridViewRow> dgvRows = dgvEnemies.Rows
                                                    .Cast<DataGridViewRow>()
                                                    .Where(r => r.Cells[1].Value.ToString().ToUpper()
                                                    .Contains(typedChars)).ToList();

                if (dgvRows.Count > 0)
                {
                    dgvEnemies.CurrentCell = dgvEnemies.Rows[dgvRows[0].Index].Cells[0];
                    dgvEnemies.Rows[dgvRows[0].Index].Selected = true;
                }
            }
            else
            {
                typedChars = string.Empty;
                return;
            }
        }

        private void TcBattleDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            iTabPageSelected = tcBattleDB.TabIndex;
        }

        private void DgvEnemies_Click(object sender, EventArgs e)
        {
            typedChars = "";
        }

        private void DgvEnemies_Sorted(object sender, EventArgs e)
        {
            tmpdgvEnemiesSortColumn = dgvEnemies.SortedColumn;
            tmpdgvEnemiesSortOrder = dgvEnemies.SortOrder;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            strLocalEnemyModelName = dgvEnemies.Rows[dgvEnemies.SelectedRows[0].Index].Cells[0].Value.ToString();
            strLocalLocationModelName = dgvLocations.Rows[dgvLocations.SelectedRows[0].Index].Cells[0].Value.ToString();
            strLocalMainPCModelName = dgvMainPCs.Rows[dgvMainPCs.SelectedRows[0].Index].Cells[0].Value.ToString();

            if (!bSelectedBattleFileFromDB)
            {
                strLocalEnemyModelName = "";
                strLocalLocationModelName = "";
                strLocalMainPCModelName = "";
            }
        }

        private void BtnSaveBattleDataDir_Click(object sender, EventArgs e)
        {
            WriteCFGFile();
        }

        private void FrmBattleDB_FormClosed(object sender, FormClosedEventArgs e)
        {
            switch(tcBattleDB.SelectedIndex)
            {
                case 0:
                    strLocalEnemyModelName = dgvEnemies.Rows[dgvEnemies.SelectedRows[0].Index].Cells[0].Value.ToString();
                    break;
                case 1:
                    strLocalLocationModelName = dgvLocations.Rows[dgvLocations.SelectedRows[0].Index].Cells[0].Value.ToString();
                    break;
                case 2:
                    strLocalMainPCModelName = dgvMainPCs.Rows[dgvMainPCs.SelectedRows[0].Index].Cells[0].Value.ToString();
                    break;
            }

            if (!bSelectedBattleFileFromDB)
            {
                strLocalEnemyModelName = "";
                strLocalLocationModelName = "";
                strLocalMainPCModelName = "";
            }
        }

        private void BtnLoadModelAnimation_Click(object sender, EventArgs e)
        {
            string strModelName = "";

            bSelectedBattleFileFromDB = false;

            switch (tcBattleDB.SelectedIndex)
            {
                case 0:
                    strModelName = dgvEnemies.Rows[dgvEnemies.SelectedRows[0].Index].Cells[0].Value.ToString();
                    break;
                case 1:
                    strModelName = dgvLocations.Rows[dgvLocations.SelectedRows[0].Index].Cells[0].Value.ToString();
                    break;
                case 2:
                    strModelName = dgvMainPCs.Rows[dgvMainPCs.SelectedRows[0].Index].Cells[0].Value.ToString();
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
