using System;
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

    using static FF7Skeleton;

    using static FileTools;
    using static Utils;

    public partial class FrmStatistics : Form
    {

        public static string strFileName;

        public FrmStatistics()
        {
            InitializeComponent();
        }

        private void FrmStatistics_Load(object sender, EventArgs e)
        {
            int iBoneIdx, iRSDIdx, iGroupIdx, iPolyIdx, iVertIdx, iTexCount, iWpnIdx, iWpnCounted;
            int iTotalVerts, iTotalPolys, iTotalTexCoords, iRSDVertsUsage, iTotalVertsUsage;

            int[] arrVertexUsage;
            HashSet<string> hsTexList = new HashSet<string>();

            iTotalVerts = iTotalPolys = iTotalVertsUsage = iTotalTexCoords = iWpnCounted = 0;

            switch (modelType)
            {
                // FIELD SKELETON
                case K_HRC_SKELETON:
                    strFileName = fSkeleton.fileName + ".TXT";

                    rtbStats.Text = "Field Model:\t\t" + fSkeleton.fileName + "\n";
                    rtbStats.Text += "Number of Bones:\t" + fSkeleton.bones.Count + "\n\n";

                    rtbStats.Text += "BONES\t\t\t\n";

                    for (iBoneIdx = 0; iBoneIdx < fSkeleton.bones.Count; iBoneIdx++)
                    {
                        rtbStats.Text += "Bone " + iBoneIdx.ToString("00") + "\t\t\t" +
                                         "Joint:\t" + fSkeleton.bones[iBoneIdx].joint_i + " - " +
                                                      fSkeleton.bones[iBoneIdx].joint_f + "\n";
                        rtbStats.Text += "Number of RSD:\t\t" +
                                         fSkeleton.bones[iBoneIdx].fRSDResources.Count.ToString("00") +
                                         "\n";

                        if (fSkeleton.bones[iBoneIdx].fRSDResources.Count == 0) rtbStats.Text += "\n";

                        for (iRSDIdx = 0; iRSDIdx < fSkeleton.bones[iBoneIdx].fRSDResources.Count; iRSDIdx++)
                        {
                            rtbStats.Text += "Resource Name:\t\t" + fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].res_file + ".RSD" + "\n";
                            rtbStats.Text += "Model Name:\t\t" + fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].Model.fileName + "\n";
                            rtbStats.Text += "Vertices: " + fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].Model.Verts.Length.ToString("00000") + "\t" +
                                             "Triangles: " + fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].Model.Polys.Length.ToString("00000") + "\t\t" +
                                             "TexCoords: " + fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].Model.TexCoords.Length.ToString("00000") + "\n";

                            iTotalVerts += fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].Model.Verts.Length;
                            iTotalPolys += fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].Model.Polys.Length;
                            iTotalTexCoords += fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].Model.TexCoords.Length;

                            // Calculate the real usage of the vertices.
                            // Different polys can use the same vertex, so, this is more load for the GPU.
                            arrVertexUsage = new int[fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].Model.Verts.Length];
                            iRSDVertsUsage = 0;

                            for (iGroupIdx = 0;
                                 iGroupIdx < fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].Model.Header.numGroups;
                                 iGroupIdx++)
                            {

                                for (iPolyIdx = fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].Model.Groups[iGroupIdx].offsetPoly;
                                     iPolyIdx < fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].Model.Groups[iGroupIdx].numPoly;
                                     iPolyIdx++)
                                {
                                    for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                                    {
                                        arrVertexUsage[fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].
                                                                 Model.Polys[iPolyIdx].Verts[iVertIdx]] += 1;
                                    }
                                }
                            }

                            for (iVertIdx = 0; iVertIdx < arrVertexUsage.Length; iVertIdx++)
                            {
                                iRSDVertsUsage += arrVertexUsage[iVertIdx];
                            }

                            iTotalVertsUsage += iRSDVertsUsage;

                            rtbStats.Text += "Vertices Usage:\t" + iRSDVertsUsage.ToString() + "\n";

                            // We need to add the Textures info if any
                            if (fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].numTextures > 0)
                            {

                                rtbStats.Text += "Textures Used:\t\t";

                                for (iTexCount = 0;
                                     iTexCount < fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].numTextures;
                                     iTexCount++)
                                {
                                    rtbStats.Text += fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].textures[iTexCount].TEXfileName;

                                    if (iTexCount < fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].numTextures - 1) rtbStats.Text += ", ";

                                    hsTexList.Add(fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].textures[iTexCount].TEXfileName + "_" +
                                                  fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].textures[iTexCount].width.ToString() + " x " +
                                                  fSkeleton.bones[iBoneIdx].fRSDResources[iRSDIdx].textures[iTexCount].height.ToString());
                                }

                                rtbStats.Text += "\n\n";
                            }

                        }

                    }

                    // List of textures and its size
                    if (hsTexList.Count > 0)
                    {
                        rtbStats.Text += "\nTEXTURE LIST\n";

                        foreach (string itmTex in hsTexList)
                        {
                            rtbStats.Text += "Name:\t\t" + itmTex.Split('_')[0] + "\t\t\t" +
                                             "Size:\t" + itmTex.Split('_')[1] + "\n";
                        }

                        rtbStats.Text += "\n";
                    }


                    // TOTALS
                    rtbStats.Text += "TOTAL TRIANGLES:\t\t" + iTotalPolys.ToString() + "\n";
                    rtbStats.Text += "TOTAL VERTICES:\t\t" + iTotalVerts.ToString() + "\n";
                    rtbStats.Text += "TOTAL TEX COORDS (UV):\t" + iTotalTexCoords.ToString() + "\n";
                    rtbStats.Text += "TOTAL VERTICES USAGE:\t" + iTotalVertsUsage.ToString() + "\n";

                    break;

                // BATTLE/MAGIC SKELETON
                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    strFileName = bSkeleton.fileName;

                    // Put battle model type
                    if (modelType == K_MAGIC_SKELETON)
                        rtbStats.Text = "Magic Model:";
                    else if (bSkeleton.IsBattleLocation)
                        rtbStats.Text = "Battle Model (Location):";
                    else if (bSkeleton.fileName[0] + bSkeleton.fileName[1] >= 165)
                        rtbStats.Text = "Battle Model (Main):";
                    else
                        rtbStats.Text = "Battle Model (Enemy)";

                    rtbStats.Text += "\t\t" + bSkeleton.fileName + "\n";
                    rtbStats.Text += "Number of Bones:\t" + bSkeleton.nBones.ToString() + "\n\n";

                    rtbStats.Text += "BONES\t\t\t\n";

                    for (iBoneIdx = 0; iBoneIdx < bSkeleton.nBones; iBoneIdx++)
                    {
                        rtbStats.Text += "Bone " + iBoneIdx.ToString("00") + "\t\t\t";
                        rtbStats.Text += "Number of Models:\t\t" +
                                         bSkeleton.bones[iBoneIdx].nModels.ToString("00") + "\n";

                        if (bSkeleton.bones[iBoneIdx].hasModel == 1)
                        {

                            for (iRSDIdx = 0; iRSDIdx < bSkeleton.bones[iBoneIdx].nModels; iRSDIdx++)
                            {
                                rtbStats.Text +=
                                    "Model Name:\t\t" + bSkeleton.bones[iBoneIdx].Models[iRSDIdx].fileName + "\n";

                                rtbStats.Text += "Vertices: " + bSkeleton.bones[iBoneIdx].Models[iRSDIdx].Verts.Length.ToString("00000") + "\t" +
                                                 "Triangles: " + bSkeleton.bones[iBoneIdx].Models[iRSDIdx].Polys.Length.ToString("00000") + "\t\t" +
                                                 "TexCoords: " + bSkeleton.bones[iBoneIdx].Models[iRSDIdx].TexCoords.Length.ToString("00000") + "\n";

                                iTotalVerts += bSkeleton.bones[iBoneIdx].Models[iRSDIdx].Verts.Length;
                                iTotalPolys += bSkeleton.bones[iBoneIdx].Models[iRSDIdx].Polys.Length;
                                iTotalTexCoords += bSkeleton.bones[iBoneIdx].Models[iRSDIdx].TexCoords.Length;

                                // Calculate the real usage of the vertices.
                                // Different polys can use the same vertex, so, this is more load for the GPU.
                                arrVertexUsage = new int[bSkeleton.bones[iBoneIdx].Models[iRSDIdx].Verts.Length];
                                iRSDVertsUsage = 0;

                                for (iGroupIdx = 0;
                                        iGroupIdx < bSkeleton.bones[iBoneIdx].Models[iRSDIdx].Header.numGroups;
                                        iGroupIdx++)
                                {

                                    for (iPolyIdx = bSkeleton.bones[iBoneIdx].Models[iRSDIdx].Groups[iGroupIdx].offsetPoly;
                                            iPolyIdx < bSkeleton.bones[iBoneIdx].Models[iRSDIdx].Groups[iGroupIdx].numPoly +
                                                       bSkeleton.bones[iBoneIdx].Models[iRSDIdx].Groups[iGroupIdx].offsetPoly;
                                            iPolyIdx++)
                                    {
                                        for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                                        {
                                            arrVertexUsage[bSkeleton.bones[iBoneIdx].Models[iRSDIdx].
                                                                        Polys[iPolyIdx].Verts[iVertIdx]] += 1;
                                        }
                                    }
                                }

                                for (iVertIdx = 0; iVertIdx < arrVertexUsage.Length; iVertIdx++)
                                {
                                    iRSDVertsUsage += arrVertexUsage[iVertIdx];
                                }

                                iTotalVertsUsage += iRSDVertsUsage;

                                rtbStats.Text += "Vertices Usage:\t" + iRSDVertsUsage.ToString() + "\n";

                                // We need to add the Textures info if any
                                if (bSkeleton.bones[iBoneIdx].Models[iRSDIdx].TexCoords.Length > 0)
                                {
                                    rtbStats.Text += "Textures Used:\t\t";

                                    for (iGroupIdx = 0;
                                            iGroupIdx < bSkeleton.bones[iBoneIdx].Models[iRSDIdx].Groups.Length;
                                            iGroupIdx++)
                                    {
                                        if (bSkeleton.bones[iBoneIdx].Models[iRSDIdx].Groups[iGroupIdx].texFlag == 1)
                                        {
                                            rtbStats.Text += bSkeleton.textures[bSkeleton.bones[iBoneIdx].Models[iRSDIdx].
                                                                    Groups[iGroupIdx].texID].TEXfileName;

                                            if (iGroupIdx < bSkeleton.bones[iBoneIdx].Models[iRSDIdx].Groups.Length - 1)
                                                rtbStats.Text += ", ";

                                            hsTexList.Add(bSkeleton.textures[bSkeleton.bones[iBoneIdx].Models[iRSDIdx].
                                                                    Groups[iGroupIdx].texID].TEXfileName + "_" +
                                                            bSkeleton.textures[bSkeleton.bones[iBoneIdx].Models[iRSDIdx].
                                                                    Groups[iGroupIdx].texID].width.ToString() + " x " +
                                                            bSkeleton.textures[bSkeleton.bones[iBoneIdx].Models[iRSDIdx].
                                                                    Groups[iGroupIdx].texID].height.ToString());
                                        }
                                    }

                                    rtbStats.Text += "\n\n";
                                }
                                else rtbStats.Text += "\n";
                            }
                        }
                        else rtbStats.Text += "\n";

                    }

                    //// WEAPONS
                    // Let add the weapons if there are any
                    if (bSkeleton.nWeapons > 0)
                    {
                        rtbStats.Text += "\nWEAPONS BONES\n";
                        rtbStats.Text += "Number of Weapons:" + bSkeleton.nWeapons.ToString("00") + "\n\n";

                        for (iWpnIdx = 0; iWpnIdx < bSkeleton.wpModels.Count; iWpnIdx++)
                        {

                            if (bSkeleton.wpModels[iWpnIdx].Header.numGroups > 0)
                            {
                                iWpnCounted++;

                                rtbStats.Text +=
                                    "Model Name:\t\t" + bSkeleton.wpModels[iWpnIdx].fileName + "\n";

                                rtbStats.Text += "Vertices:\t" + bSkeleton.wpModels[iWpnIdx].Verts.Length.ToString() + "\t\t" +
                                                 "Triangles:\t" + bSkeleton.wpModels[iWpnIdx].Polys.Length.ToString() + "\t\t" +
                                                 "TexCoords:\t" + bSkeleton.wpModels[iWpnIdx].TexCoords.Length.ToString() + "\n";

                                iTotalVerts += bSkeleton.wpModels[iWpnIdx].Verts.Length;
                                iTotalPolys += bSkeleton.wpModels[iWpnIdx].Polys.Length;
                                iTotalTexCoords += bSkeleton.wpModels[iWpnIdx].TexCoords.Length;

                                // Calculate the real usage of the vertices.
                                // Different polys can use the same vertex, so, this is more load for the GPU.
                                arrVertexUsage = new int[bSkeleton.wpModels[iWpnIdx].Verts.Length];
                                iRSDVertsUsage = 0;

                                for (iGroupIdx = 0;
                                     iGroupIdx < bSkeleton.wpModels[iWpnIdx].Header.numGroups;
                                     iGroupIdx++)
                                {

                                    for (iPolyIdx = bSkeleton.wpModels[iWpnIdx].Groups[iGroupIdx].offsetPoly;
                                         iPolyIdx < bSkeleton.wpModels[iWpnIdx].Groups[iGroupIdx].numPoly +
                                                    bSkeleton.wpModels[iWpnIdx].Groups[iGroupIdx].offsetPoly;
                                         iPolyIdx++)
                                    {
                                        for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                                        {
                                            arrVertexUsage[bSkeleton.wpModels[iWpnIdx].
                                                                        Polys[iPolyIdx].Verts[iVertIdx]] += 1;
                                        }
                                    }
                                }

                                for (iVertIdx = 0; iVertIdx < arrVertexUsage.Length; iVertIdx++)
                                {
                                    iRSDVertsUsage += arrVertexUsage[iVertIdx];
                                }

                                iTotalVertsUsage += iRSDVertsUsage;

                                rtbStats.Text += "Vertices Usage:\t" + iRSDVertsUsage.ToString() + "\n";

                                // We need to add the Textures info if any
                                if (bSkeleton.wpModels[iWpnIdx].TexCoords.Length > 0)
                                {
                                    rtbStats.Text += "Textures Used:\t\t";

                                    for (iGroupIdx = 0;
                                         iGroupIdx < bSkeleton.wpModels[iWpnIdx].Groups.Length;
                                         iGroupIdx++)
                                    {
                                        if (bSkeleton.wpModels[iWpnIdx].Groups[iGroupIdx].texFlag == 1)
                                        {
                                            rtbStats.Text += bSkeleton.textures[bSkeleton.wpModels[iWpnIdx].
                                                                                Groups[iGroupIdx].texID].TEXfileName;

                                            if (iGroupIdx < bSkeleton.wpModels[iWpnIdx].Groups.Length - 1)
                                                rtbStats.Text += ", ";

                                            hsTexList.Add(bSkeleton.textures[bSkeleton.wpModels[iWpnIdx].
                                                                Groups[iGroupIdx].texID].TEXfileName + "_" +
                                                          bSkeleton.textures[bSkeleton.wpModels[iWpnIdx].
                                                                Groups[iGroupIdx].texID].width.ToString() + " x " +
                                                          bSkeleton.textures[bSkeleton.wpModels[iWpnIdx].
                                                                Groups[iGroupIdx].texID].height.ToString());
                                        }
                                    }

                                    rtbStats.Text += "\n\n";
                                }
                                else rtbStats.Text += "\n";
                            
                            }
                        }

                        if (iWpnCounted == 0)
                        {
                            rtbStats.Text += "----------------------------------------------------\n";
                            rtbStats.Text += "WARNING: There are NOT weapons loaded and the Battle " + 
                                             "Model should have weapons.\n";
                            rtbStats.Text += "----------------------------------------------------\n\n";
                        }

                        if (iWpnCounted > 0 && iWpnCounted != bSkeleton.nWeapons)
                        {
                            rtbStats.Text += "----------------------------------------------------\n";
                            rtbStats.Text += "WARNING: There number of weapons loaded are not the same " + 
                                             "as the number of weapons of the Battle Model.\n";
                            rtbStats.Text += "----------------------------------------------------\n\n";
                        }
                    }


                    // List of textures and its size
                    if (hsTexList.Count > 0)
                    {
                        rtbStats.Text += "\nTEXTURE LIST\n";

                        foreach (string itmTex in hsTexList)
                        {
                            rtbStats.Text += "Name:\t\t" + itmTex.Split('_')[0] + "\t\t\t" +
                                             "Size:\t" + itmTex.Split('_')[1] + "\n";
                        }

                        rtbStats.Text += "\n";
                    }


                    // TOTALS
                    rtbStats.Text += "TOTAL TRIANGLES:\t\t" + iTotalPolys.ToString() + "\n";
                    rtbStats.Text += "TOTAL VERTICES:\t\t" + iTotalVerts.ToString() + "\n";
                    rtbStats.Text += "TOTAL TEX COORDS (UV):\t" + iTotalTexCoords.ToString() + "\n";
                    rtbStats.Text += "TOTAL VERTICES USAGE:\t" + iTotalVertsUsage.ToString() + "\n";

                    break;

            }

            if (strFileName.Length > 0) Text += " - " + strFileName;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnSaveStats_Click(object sender, EventArgs e)
        {
            saveFile.Title = "Save Statistics As...";
            saveFile.Filter = "Plain Text file|*.TXT|All files|*.*";
            saveFile.FilterIndex = 1;
            saveFile.InitialDirectory = strGlobalPathFieldSkeletonFolder;

            saveFile.FileName = strFileName;

            try
            {
                // Process input if the user clicked OK.
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    rtbStats.SaveFile(saveFile.FileName, RichTextBoxStreamType.PlainText);
                }
            }
            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;

                MessageBox.Show("Error saving statistics file.", "Error");
                return;
            }
        }

    }
}
