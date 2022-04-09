﻿using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KimeraCS
{

    using Defines;

    using static FF7PModel;

    using static FF7TEXTexture;

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
                TEX itmTextureTEX;

                string[] rsdString;
                string rsdFileName = strFolderName + "\\" + in_res_file + ".RSD";

                string polyFileName = "";
                int rsdNTEXpos = 0;

                // Let's read RSD file into memory.
                // First check if exists
                if (!File.Exists(rsdFileName))
                {
                    throw new FileNotFoundException("Error opening RSD file '" + rsdFileName + ".RSD'.");
                }

                // Assign to struct name of resource file
                res_file = in_res_file;

                // Let's put the ID (@RSD) All .rsd files have this.
                ID = "@RSD940102";

                rsdString = File.ReadAllLines(rsdFileName);

                // Let's read PLY
                while (rsdString[rsdNTEXpos][0] != 'P') rsdNTEXpos++;

                // Let's read the P model
                polyFileName = (rsdString[rsdNTEXpos].Split('=')[1]).Substring(0, (rsdString[rsdNTEXpos].Split('=')[1]).IndexOf('.')) + ".P";
                Model = new PModel();
                LoadPModel(ref Model, strFolderName, polyFileName);

                // Let's read NTEX
                while (rsdString[rsdNTEXpos][0] != 'N') rsdNTEXpos++;

                // Let's get the num textures
                numTextures = Int32.Parse(rsdString[rsdNTEXpos].Split('=')[1]);
                textures = new List<TEX>();

                glTexParameterf(glTextureTarget.GL_TEXTURE_2D, glTextureParameter.GL_TEXTURE_MAG_FILTER, (float)glTextureMagFilter.GL_LINEAR);
                glTexParameterf(glTextureTarget.GL_TEXTURE_2D, glTextureParameter.GL_TEXTURE_MIN_FILTER, (float)glTextureMagFilter.GL_LINEAR);

                for (ti = 0; ti < numTextures; ti++)
                {
                    // Position to the "TEX[n]" line (check comments or lines not needed)
                    while (rsdString[rsdNTEXpos][0] != 'T') rsdNTEXpos++;

                    // Prepare itmTextureTEX var
                    itmTextureTEX = new TEX();

                    // Get each texture entry in RSD (TEX[n] entries)
                    itmTextureTEX.TEXfileName = (rsdString[rsdNTEXpos].Split('=')[1]).Substring(0, rsdString[rsdNTEXpos].Split('=')[1].IndexOf('.')) + ".TEX";

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



        //  ---------------------------------------------------------------------------------------------------
        //  ============================================= SAVING ==============================================
        //  ---------------------------------------------------------------------------------------------------
        public static void MergeFieldRSDResources(ref FieldRSDResource fRSDResourceOut, FieldRSDResource fRSDResourceIn)
        {
            int ti;

            MergePModels(ref fRSDResourceOut.Model, fRSDResourceIn.Model);

            // Merge textures
            for (ti = 0; ti < fRSDResourceIn.numTextures; ti++) fRSDResourceOut.textures.Add(fRSDResourceIn.textures[ti]);

            fRSDResourceOut.numTextures += fRSDResourceIn.numTextures;
        }

        public static void WriteRSDResource(FieldRSDResource Resource, string fileName)
        {
            int ti;
            string nameP;
            StringBuilder strRSDContent = new StringBuilder();

            try
            {
                strRSDContent.AppendLine(Resource.ID);
                nameP = Resource.Model.fileName.Substring(0, Resource.Model.fileName.Length - 2);

                strRSDContent.AppendLine("PLY=" + nameP + ".PLY");
                strRSDContent.AppendLine("MAT=" + nameP + ".MAT");
                strRSDContent.AppendLine("GRP=" + nameP + ".GRP");

                strRSDContent.AppendLine("NTEX=" + Resource.numTextures);

                for (ti =  0; ti < Resource.numTextures; ti++)
                {
                    strRSDContent.AppendLine("TEX[" + ti.ToString() + "]=" + 
                                             Resource.textures[ti].TEXfileName.Substring(0, Resource.textures[ti].TEXfileName.Length - 4) + ".TIM");
                    WriteTEXTexture(Resource.textures[ti], strGlobalPathSaveSkeletonFolder + "\\" + Resource.textures[ti].TEXfileName);
                }

                File.WriteAllText(fileName, strRSDContent.ToString());
            }
            catch
            {
                MessageBox.Show("Error saving RSD file " + fileName, "Error", MessageBoxButtons.OK);
            }
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





    }
}