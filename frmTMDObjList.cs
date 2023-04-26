using System;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KimeraCS
{
    using static FrmSkeletonEditor;

    using static FF7Skeleton;

    using static FF7PModel;
    using static FF7TMDModel;

    using static Utils;
    using static FileTools;

    public partial class FrmTMDObjList : Form
    {
        readonly private FrmSkeletonEditor frmSkelEdit;


        public FrmTMDObjList(FrmSkeletonEditor frmSkelEdit)
        {
            InitializeComponent();

            this.frmSkelEdit = frmSkelEdit;
            Owner = frmSkelEdit;
        }

        public void RepositionTMD()
        {
            Location = new Point(Owner.Location.X + Owner.Width - Width,
                                 Owner.Location.Y + Owner.Height - Height);
        }

        private void FrmTMDObjList_Load(object sender, EventArgs e)
        {
            RepositionTMD();

            lbTMDObjectList.SelectedIndex = 0;

            if (bConverted2Float) cbConvertToFloat.Checked = true;
            else cbConvertToFloat.Checked = false;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void PopulateTMDObjList()
        {
            string strObjectName;

            lbTMDObjectList.Items.Clear();
            
            for (int i = 0; i < mTMDModel.TMDHeader.nObjects; i++)
            {
                strObjectName = "Object " + (i + 1).ToString("000");

                if (TMDHasTextureUVs(mTMDModel.TMDObjectList[i].TMDPrimitiveList[0]))
                {
                    strObjectName += " (TX)";
                }

                lbTMDObjectList.Items.Add(strObjectName);
            }
        }

        private void LbTMDObjectList_DoubleClick(object sender, EventArgs e)
        {
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            if (lbTMDObjectList.SelectedIndex > -1)
            {
                // Close previous P Editor if any.
                if (FindWindowOpened("FrmPEditor")) frmSkelEdit.frmPEdit.Close();

                // Now let's convert the TMD model into P model.
                DestroyPModelResources(ref fPModel);
                fPModel = new PModel()
                {
                    fileName = Path.GetFileNameWithoutExtension(strGlobalTMDModelName) + "_" + (lbTMDObjectList.SelectedIndex + 1).ToString("000"),
                };

                ConvertTMD2PModel(ref fPModel, mTMDModel, lbTMDObjectList.SelectedIndex);

                ComputePModelBoundingBox(fPModel, ref p_min, ref p_max);
                diameter = ComputeDiameter(fPModel.BoundingBox);

                // Set frame values in frame editor groupbox...
                frmSkelEdit.SetFrameEditorFields();

                // Enable texture group if TMD object has texture coordinates
                //if (TMDHasTextureUVs(mTMDModel.TMDObjectList[lbTMDObjectList.SelectedIndex].TMDPrimitiveList[0]))
                //{
                //    // Set texture values in texture editor groupbox...
                //    frmSkelEdit.gbTexturesFrame.Visible = true;
                //    frmSkelEdit.gbTexturesFrame.Enabled = true;
                //    frmSkelEdit.gbTexturesFrame.Location = new Point(frmSkelEdit.gbTexturesFrame.Location.X,
                //                                                     0);
                //    frmSkelEdit.SetTextureEditorFields();

                //}
                //else
                //{
                //    frmSkelEdit.gbTexturesFrame.Visible = false;
                //    frmSkelEdit.gbTexturesFrame.Enabled = false;
                //    frmSkelEdit.gbTexturesFrame.Location = new Point(frmSkelEdit.gbTexturesFrame.Location.X,
                //                                                     193);
                //    frmSkelEdit.SetTextureEditorFields();
                //}

                // PostLoadModelPreparations
                frmSkelEdit.PostLoadModelPreparations(ref p_min, ref p_max);

                // We can draw the model in panel
                frmSkelEdit.PanelModel_Paint(null, null);
            }
        }

        private void FrmTMDObjList_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                Hide();
                e.Cancel = true;
            }
        }

        private void BtnSaveInfo_Click(object sender, EventArgs e)
        {
            WriteTMDLOG();
        }

        private void BtnSaveTMD_Click(object sender, EventArgs e)
        {
            //string saveAnimationFileName;
            int iSaveResult;

            // Set filter options and filter index depending on modelType
            // Check Initial Directory
            saveFile.Title = "Save TMD File As...";
            saveFile.Filter = "TMD|*.TMD|All files|*.*";

            if (strGlobalPathTMDModelFolder == "")
                strGlobalPathTMDModelFolder = strGlobalPath;

            saveFile.FileName = strGlobalTMDModelName.ToUpper();
            saveFile.FilterIndex = 1;
            saveFile.InitialDirectory = strGlobalPathTMDModelFolder;
            
            try
            {
                // Process input if the user clicked OK.
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    if (IsTMDModel)
                    {
                        // I don't think it is needed when saving
                        //AddStateToBuffer(this);

                        strGlobalPathSaveTMDFolder = Path.GetDirectoryName(saveFile.FileName);
                        saveFile.FileName = strGlobalPathSaveTMDFolder + "\\" + Path.GetFileName(saveFile.FileName).ToUpper();
                        
                        // We save the TMD Object Pack.
                        iSaveResult = WriteTMD(saveFile.FileName);

                        if (iSaveResult == 1)
                        {
                            MessageBox.Show("The TMD file " + Path.GetFileName(saveFile.FileName).ToUpper() + " has been saved.",
                                            "Information");
                        }
                        else
                        {
                            MessageBox.Show("Error saving the TMD file " + Path.GetFileName(saveFile.FileName).ToUpper() + ".",
                                            "Error");
                            return;
                        }
                    }

                    WriteCFGFile();
                }
            }
            catch
            {
                MessageBox.Show("Error exception saving the TMD file as... " + Path.GetFileName(saveFile.FileName) + ".",
                                "Error");
                return;
            }
        }

        private void BtnCommitPModel_Click(object sender, EventArgs e)
        {
            if (lbTMDObjectList.SelectedIndex > -1)
            {
                ConvertPModel2TMD(fPModel, lbTMDObjectList.SelectedIndex);

                LbTMDObjectList_DoubleClick(lbTMDObjectList, EventArgs.Empty);
            }
        }

        private void CbConvertToFloat_CheckedChanged(object sender, EventArgs e)
        {
            bConverted2Float = cbConvertToFloat.Checked;

            if (bConverted2Float) mTMDModel.TMDHeader.version = 0xFF;
            else mTMDModel.TMDHeader.flags = 0x41;

            RecalculateOffsets();
        }
    }
}
