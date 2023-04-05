using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KimeraCS
{

    using Defines;

    using static FF7FieldSkeleton;
    using static FF7PModel;

    using static FF7TEXTexture;

    using static Utils;
    using static OpenGL32;
    using static GDI32;
    using static FileTools;

    public class FF7FieldRSDResource
    {
        public struct FieldRSDResource
        {

            public string ID;
            public string res_file;
            public PModel Model;
            public int numTextures;
            public List<TEX> textures;

            public FieldRSDResource(string in_res_file, ref List<TEX> textures_pool, string strFolderName)
            {
                int ti, ti_pool;
                bool tex_foundQ;
                DialogResult drPModelError;

                TEX itmTextureTEX;
                Model = new PModel();

                string[] rsdString;
                string rsdFileName = strFolderName + "\\" + in_res_file + ".RSD";

                string polyFileName;
                int rsdNTEXpos = 0;

                // Let's read RSD file into memory.

                // Let's put the ID (@RSD) All .rsd files have this.
                ID = "@RSD940102";

                // Assign to struct name of resource file
                res_file = in_res_file;
                
                numTextures = 0;
                textures = new List<TEX>();

                // First check if exists
                if (!File.Exists(rsdFileName))
                {
                    //throw new FileNotFoundException("Error opening RSD file '" + rsdFileName);
                    MessageBox.Show("File: " + in_res_file + ".RSD does not exists.");
                }
                else
                {
                    rsdString = File.ReadAllLines(rsdFileName);

                    // Let's read PLY
                    while (rsdString[rsdNTEXpos].Length == 0 || rsdString[rsdNTEXpos][0] != 'P') rsdNTEXpos++;

                    // Let's read the P model
                    //polyFileName = (rsdString[rsdNTEXpos].Split('=')[1]).Substring(0, (rsdString[rsdNTEXpos].Split('=')[1]).IndexOf('.')) + ".P";
                    polyFileName = Path.GetFileNameWithoutExtension(rsdString[rsdNTEXpos].Split('=')[1]) + ".P";

                    // Let's read the P model into memory.
                    // First check if exists
                    if (!File.Exists(strFolderName + "\\" + polyFileName))
                    {
                        // We maybe want to load the rest of the model.
                        // Here we give the choice to load it o cancel loading the model.
                        drPModelError = MessageBox.Show("The .P Model file '" + polyFileName + "' does not exists. " +
                                                        "Do you want to continue loading the other parts of the model?",
                                                        "Information", MessageBoxButtons.YesNo);

                        if (drPModelError == DialogResult.No)
                        {
                            // throw new FileNotFoundException("Error opening P file '" + polyFileName + ".P'.");
                            throw new FileNotFoundException();
                        }

                        // We can fill the name of the .P model. This will help to saving.
                        Model.fileName = polyFileName;
                    }
                    else
                    {
                        LoadPModel(ref Model, strFolderName, polyFileName, true);
                    }


                    // Let's read NTEX
                    while (rsdString[rsdNTEXpos].Length == 0 || rsdString[rsdNTEXpos][0] != 'N') rsdNTEXpos++;

                    // Let's get the num textures
                    numTextures = Int32.Parse(rsdString[rsdNTEXpos].Split('=')[1]);

                    glTexParameterf(GLTextureTarget.GL_TEXTURE_2D, GLTextureParameter.GL_TEXTURE_MAG_FILTER, (float)GLTextureMagFilter.GL_LINEAR);
                    glTexParameterf(GLTextureTarget.GL_TEXTURE_2D, GLTextureParameter.GL_TEXTURE_MIN_FILTER, (float)GLTextureMagFilter.GL_LINEAR);

                    for (ti = 0; ti < numTextures; ti++)
                    {
                        // Position to the "TEX[n]" line (check comments or lines not needed)
                        while (rsdString[rsdNTEXpos].Length == 0 || rsdString[rsdNTEXpos][0] != 'T') rsdNTEXpos++;

                        // Prepare itmTextureTEX var
                        itmTextureTEX = new TEX()
                        {
                            // Get each texture entry in RSD (TEX[n] entries)
                            TEXfileName = rsdString[rsdNTEXpos].Split('=')[1].
                                                Substring(0, rsdString[rsdNTEXpos].
                                                Split('=')[1].IndexOf('.')) + ".TEX",
                        };

                        // Position for next "TEX[n]" line
                        rsdNTEXpos++;

                        ti_pool = 0;
                        tex_foundQ = false;

                        while (ti_pool < textures_pool.Count && !tex_foundQ)
                        {
                            tex_foundQ = textures_pool[ti_pool].TEXfileName == itmTextureTEX.TEXfileName;
                            ti_pool++;
                        }

                        if (tex_foundQ)
                        {
                            itmTextureTEX = textures_pool[ti_pool - 1];
                        }
                        else
                        {
                            if (ReadTEXTexture(ref itmTextureTEX, strFolderName + "\\" + itmTextureTEX.TEXfileName) == 0)
                            {
                                // Load TEX Texture (or other format if existant).
                                LoadTEXTexture(ref itmTextureTEX);
                                // Prepare Bitmap from TEX Texture
                                LoadBitmapFromTEXTexture(ref itmTextureTEX);
                            }

                            // Add TEX Texture to the list of RSDResource textures
                            textures.Add(itmTextureTEX);
                        }
                    }
                }
            }
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ============================================= SAVING ==============================================
        //  ---------------------------------------------------------------------------------------------------
        public static void MergeFieldRSDResources(ref FieldRSDResource fRSDResourceOut, 
                                                  FieldRSDResource fRSDResourceIn)
        {
            int iTextureIdx;

            MergePModels(ref fRSDResourceOut.Model, fRSDResourceIn.Model);

            // Merge textures
            for (iTextureIdx = 0; iTextureIdx < fRSDResourceIn.numTextures; iTextureIdx++) 
                fRSDResourceOut.textures.Add(fRSDResourceIn.textures[iTextureIdx]);

            fRSDResourceOut.numTextures += fRSDResourceIn.numTextures;
        }

        public static int WriteRSDResource(FieldRSDResource fRSDResource, string fileName)
        {
            int ti, iWriteRSDResourceResult;
            string nameP;
            StringBuilder strRSDContent = new StringBuilder();

            try
            {
                strRSDContent.AppendLine(fRSDResource.ID);
                nameP = fRSDResource.Model.fileName.Substring(0, fRSDResource.Model.fileName.Length - 2).ToUpper();

                strRSDContent.AppendLine("PLY=" + nameP + ".PLY");
                strRSDContent.AppendLine("MAT=" + nameP + ".MAT");
                strRSDContent.AppendLine("GRP=" + nameP + ".GRP");

                strRSDContent.AppendLine("NTEX=" + fRSDResource.numTextures);

                for (ti =  0; ti < fRSDResource.numTextures; ti++)
                {
                    strRSDContent.AppendLine("TEX[" + ti.ToString() + "]=" +
                                             fRSDResource.textures[ti].TEXfileName.Substring(0, fRSDResource.textures[ti].TEXfileName.Length - 4).ToUpper() + ".TIM");
                    WriteTEXTexture(fRSDResource.textures[ti], strGlobalPathSaveSkeletonFolder + "\\" + fRSDResource.textures[ti].TEXfileName.ToUpper());
                }

                File.WriteAllText(fileName, strRSDContent.ToString());

                iWriteRSDResourceResult = 1;
            }
            catch
            {
                MessageBox.Show("Error saving RSD file " + fileName, "Error", MessageBoxButtons.OK);

                iWriteRSDResourceResult = -1;
            }

            return iWriteRSDResourceResult;
        }

        public static int WriteFullRSDResource(FieldBone infBone, string strFullFileName)
        {
            int iWriteRSDResourceResult;
            PModel tmpPModel;

            try
            {
                if (infBone.fRSDResources.Count > 0)
                {
                    // Write RSD Resource
                    WriteRSDResource(infBone.fRSDResources[0], strFullFileName);

                    if (infBone.fRSDResources[0].Model.Polys != null)
                    {
                        tmpPModel = infBone.fRSDResources[0].Model;
                        WriteGlobalPModel(ref tmpPModel, 
                            Path.GetDirectoryName(strFullFileName) + "\\" + tmpPModel.fileName.ToUpper());
                    }
                }

                iWriteRSDResourceResult = 1;
            }
            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;

                iWriteRSDResourceResult = -1;
            }

            return iWriteRSDResourceResult;
        }

        public static void CreateDListsFromRSDResource(ref FieldRSDResource Resource)
        {
            CreateDListsFromPModel(ref Resource.Model);
        }

        public static void DestroyRSDResources(ref FieldRSDResource Resource)
        {
            int ti;
            uint[] lstTexID = new uint[1];

            DestroyPModelResources(ref Resource.Model);

            for (ti = 0; ti < Resource.numTextures; ti++)
            {
                lstTexID[0] = Resource.textures[ti].texID;

                glDeleteTextures(1, lstTexID);
                DeleteDC(Resource.textures[ti].HDC);
                DeleteObject(Resource.textures[ti].HBMP);
            }

            Resource.textures.Clear();
        }



        //  --------------------------------------------------------------------------------------------------
        //  ============================================= UTILS ==============================================
        //  --------------------------------------------------------------------------------------------------
        public static FieldRSDResource CopyRSDResource(FieldRSDResource fRSDResourceIn)
        {
            FieldRSDResource tmpFieldRSDResource = new FieldRSDResource()
            {
                ID = fRSDResourceIn.ID,
                numTextures = fRSDResourceIn.numTextures,
                res_file = fRSDResourceIn.res_file,
                textures = fRSDResourceIn.textures,
                Model = CopyPModel(fRSDResourceIn.Model),
            };

            return tmpFieldRSDResource;
        }



    }
}
