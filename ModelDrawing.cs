using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace KimeraCS
{
    using Defines;

    using static FrmSkeletonEditor;
    using static FrmPEditor;

    using static FF7Skeleton;
    using static FF7FieldSkeleton;
    using static FF7FieldAnimation;
    using static FF7FieldRSDResource;

    using static FF7BattleSkeleton;
    using static FF7BattleAnimationsPack;
    using static FF7BattleAnimation;

    using static FF7PModel;

    using static Lighting;
   
    using static Utils;
    using static OpenGL32;
    using static GLExt;

    public class ModelDrawing
    {

        public static uint[] tex_ids = new uint[1];


        //  ---------------------------------------------------------------------------------------------------
        //  ================================== GENERIC FIELD/BATTLE DRAW  =====================================
        //  ---------------------------------------------------------------------------------------------------
        public static void ShowNormals(PGroup Group, PPolygon[] Polys, Point3D[] Verts, 
                                       Point3D[] Normals, int[] NormalsIndex)
        {
            long iPolyIdx, iVertIdx;
            float x, y, z, xn, yn, zn, fRed, fGreen, fBlue;
            bool bLightingEnabled = false;

            if (Group.HiddenQ) return;

            fRed = fGreen = fBlue = 0.0f;

            if ((iNormalsColor & 0x1) == 0x1) fRed = 1.0f;
            if ((iNormalsColor & 0x2) == 0x2) fGreen = 1.0f;
            if ((iNormalsColor & 0x4) == 0x4) fBlue = 1.0f;

            glColor4f(fRed, fGreen, fBlue, 1.0f);

            glDisable(GLCapability.GL_TEXTURE_2D);

            if (glIsEnabled(GLCapability.GL_LIGHTING)) bLightingEnabled = true;
            glDisable(GLCapability.GL_LIGHTING);

            glBegin(GLDrawMode.GL_LINES);

            if (Normals.Length > 0)
            {
                for (iPolyIdx = Group.offsetPoly; iPolyIdx < Group.offsetPoly + Group.numPoly; iPolyIdx++)
                {
                    if (bShowVertexNormals)
                    {
                        for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                        {
                            x = Verts[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert].x;
                            y = Verts[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert].y;
                            z = Verts[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert].z;
                            glVertex3f(x, y, z);

                            xn = x + Normals[NormalsIndex[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert]].x * fNormalsScale;
                            yn = y + Normals[NormalsIndex[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert]].y * fNormalsScale;
                            zn = z + Normals[NormalsIndex[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert]].z * fNormalsScale;
                            glVertex3f(xn, yn, zn);
                        }
                    }
                    else if (bShowFaceNormals)
                    {
                        Point3D p3DCenteroid, p3DNormal;

                        p3DCenteroid = CalculateCenteroid(Verts[Polys[iPolyIdx].Verts[0] + Group.offsetVert],
                                                          Verts[Polys[iPolyIdx].Verts[1] + Group.offsetVert],
                                                          Verts[Polys[iPolyIdx].Verts[2] + Group.offsetVert]);

                        p3DNormal = CalculateNormal(Verts[Polys[iPolyIdx].Verts[0] + Group.offsetVert],
                                                    Verts[Polys[iPolyIdx].Verts[1] + Group.offsetVert],
                                                    Verts[Polys[iPolyIdx].Verts[2] + Group.offsetVert]);
                        p3DNormal = Normalize(p3DNormal);

                        glVertex3f(p3DCenteroid.x, p3DCenteroid.y, p3DCenteroid.z);

                        glVertex3f(p3DCenteroid.x + (-p3DNormal.x * fNormalsScale),
                                   p3DCenteroid.y + (-p3DNormal.y * fNormalsScale),
                                   p3DCenteroid.z + (-p3DNormal.z * fNormalsScale));
                    }
                }
            }

            glEnd();

            if (bLightingEnabled)
                glEnable(GLCapability.GL_LIGHTING);
        }

        public static void DrawGroup(PGroup Group, PPolygon[] Polys, Point3D[] Verts,
                                     Color[] Vcolors, Point3D[] Normals, int[] NormalsIndex,
                                     Point2D[] TexCoords, PHundret Hundret, bool HideHiddenQ)
        {

            if (Group.HiddenQ && HideHiddenQ) return;

            int iPolyIdx = 0, iVertIdx = 0;
            float x, y, z;

            try
            {
                glBegin(GLDrawMode.GL_TRIANGLES);
                glColorMaterial(GLFace.GL_FRONT_AND_BACK, GLMaterialParameter.GL_AMBIENT_AND_DIFFUSE);

                for (iPolyIdx = Group.offsetPoly; iPolyIdx < Group.offsetPoly + Group.numPoly; iPolyIdx++)
                {
                    for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                    {
                        if (Hundret.blend_mode == 0 && !(Hundret.shademode == 1) && !bSkeleton.IsBattleLocation)
                            glColor4f(Vcolors[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert].R / 255.0f,
                                      Vcolors[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert].G / 255.0f,
                                      Vcolors[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert].B / 255.0f,
                                      0.5f);
                        else
                            glColor4f(Vcolors[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert].R / 255.0f,
                                      Vcolors[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert].G / 255.0f,
                                      Vcolors[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert].B / 255.0f,
                                      1.0f);

                        if (Normals.Length > 0)
                            glNormal3f(Normals[NormalsIndex[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert]].x,
                                       Normals[NormalsIndex[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert]].y,
                                       Normals[NormalsIndex[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert]].z);

                        if (TexCoords != null)
                            if (Group.texFlag == 1 && TexCoords.Length > 0)
                            {
                                x = TexCoords[Group.offsetTex + Polys[iPolyIdx].Verts[iVertIdx]].x;
                                y = TexCoords[Group.offsetTex + Polys[iPolyIdx].Verts[iVertIdx]].y;
                                glTexCoord2f(x, y);
                            }

                        x = Verts[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert].x;
                        y = Verts[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert].y;
                        z = Verts[Polys[iPolyIdx].Verts[iVertIdx] + Group.offsetVert].z;
                        glVertex3f(x, y, z);
                    }
                }

                glEnd();

                // Let's try here to render the normals
                if (bShowVertexNormals || bShowFaceNormals)
                    ShowNormals(Group, Polys, Verts, Normals, NormalsIndex);

            }

            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;

                MessageBox.Show("Exception in DrawGroup procedure.\nGroup: " + Group.realGID.ToString() +
                                "\nPolygon (iPolyIdx): " + iPolyIdx.ToString() +
                                "\nVertex (iVertIdx): " + iVertIdx.ToString() +
                                "\noffsetVertex: " + Group.offsetVert.ToString() +
                                "\noffsetPolygon: " + Group.offsetPoly.ToString(), 
                                "Exception error", MessageBoxButtons.OK);
            }
        }

        public static bool IsColorKey(PModel Model, int iGroupIdx)
        {
            bool bIsColorKey = false;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    if (fSkeleton.textures_pool != null)
                        if (fSkeleton.textures_pool.Count > 0 && Model.Groups[iGroupIdx].texID >= 0)
                            if (fSkeleton.textures_pool[Model.Groups[iGroupIdx].texID].ColorKeyFlag == 1)
                                bIsColorKey = true;

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    if (bSkeleton.textures.Count > 0 && Model.Groups[iGroupIdx].texID >= 0)
                        if (bSkeleton.textures[Model.Groups[iGroupIdx].texID].ColorKeyFlag == 1)
                            bIsColorKey = true;

                    break;
            }

            return bIsColorKey;
        }

        public static void DrawPModel(ref PModel Model, ref uint[] tex_ids, bool HideHiddenGroupsQ)
        {
            int iGroupIdx;
            //bool set_v_textured, v_textured, set_v_linearfilter, v_linearfilter, texEnabled;
            //texEnabled = glIsEnabled(GLCapability.GL_TEXTURE_2D);

            glEnable(GLCapability.GL_COLOR_MATERIAL);

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                //  Set the render states acording to the hundrets information
                //  V_WIREFRAME
                if ((Model.Hundrets[iGroupIdx].field_C & 0x1) != 0)
                {
                    if ((Model.Hundrets[iGroupIdx].field_8 & 0x1) != 0)
                        glPolygonMode(GLFace.GL_FRONT_AND_BACK, GLPolygon.GL_LINE);
                    else
                        glPolygonMode(GLFace.GL_FRONT_AND_BACK, GLPolygon.GL_FILL);
                }

                //  V_LINEAR (Linear filter)
                if ((Model.Hundrets[iGroupIdx].field_C & 0x4) != 0)
                {
                    if ((Model.Hundrets[iGroupIdx].field_8 & 0x4) != 0)
                    {
                        glTexParameterf(GLTextureTarget.GL_TEXTURE_2D, GLTextureParameter.GL_TEXTURE_MIN_FILTER,
                                        (float)GLTextureMagFilter.GL_LINEAR);
                        glTexParameterf(GLTextureTarget.GL_TEXTURE_2D, GLTextureParameter.GL_TEXTURE_MAG_FILTER,
                                        (float)GLTextureMagFilter.GL_LINEAR);
                    }
                    else
                    {
                        glTexParameterf(GLTextureTarget.GL_TEXTURE_2D, GLTextureParameter.GL_TEXTURE_MIN_FILTER,
                                        (float)GLTextureMagFilter.GL_NEAREST);
                        glTexParameterf(GLTextureTarget.GL_TEXTURE_2D, GLTextureParameter.GL_TEXTURE_MAG_FILTER,
                                        (float)GLTextureMagFilter.GL_NEAREST);
                    }
                }

                ////  V_CULLFACE
                if ((Model.Hundrets[iGroupIdx].field_C & 0x2000) != 0)
                {
                    glEnable(GLCapability.GL_CULL_FACE);

                    if ((Model.Hundrets[iGroupIdx].field_8 & 0x2000) != 0)                       
                        glCullFace(GLFace.GL_FRONT);
                    else
                        glCullFace(GLFace.GL_BACK);

                }


                ////  V_NOCULL
                if (!((Model.Hundrets[iGroupIdx].field_C & 0x4000) == 0))
                {
                    if (!((Model.Hundrets[iGroupIdx].field_8 & 0x4000) == 0))
                    {
                        glDisable(GLCapability.GL_CULL_FACE);
                    }
                    else
                    {
                        glEnable(GLCapability.GL_CULL_FACE);
                        glCullFace(GLFace.GL_FRONT);
                    }
                }


                //// V_DEPTH_TEST
                if ((Model.Hundrets[iGroupIdx].field_C & 0x8000) != 0)
                {
                    if ((Model.Hundrets[iGroupIdx].field_8 & 0x8000) != 0)
                    {
                        glEnable(GLCapability.GL_DEPTH_TEST);
                    }
                    else
                    {
                        glDisable(GLCapability.GL_DEPTH_TEST);
                    }
                }

                //// V_DEPTH_MASK
                if ((Model.Hundrets[iGroupIdx].field_C & 0x10000) != 0)
                {
                    if ((Model.Hundrets[iGroupIdx].field_8 & 0x10000) != 0)
                    {
                        glDepthMask((byte)GL_Boolean.GL_TRUE);
                    }
                    else
                    {
                        glDepthMask((byte)GL_Boolean.GL_FALSE);
                    }
                }


                //// V_SHADEMODE
                if ((Model.Hundrets[iGroupIdx].field_C & 0x020000) != 0)
                {
                    if ((Model.Hundrets[iGroupIdx].field_8 & 0x020000) != 0)
                    {
                        if (Model.Hundrets[iGroupIdx].shademode == 1 &&
                            !bSkeleton.IsBattleLocation)
                        {
                            glShadeModel(GLShadingModel.GL_FLAT);
                        }
                        else if (Model.Hundrets[iGroupIdx].shademode == 2 ||
                                 bSkeleton.IsBattleLocation)
                        {
                            glShadeModel(GLShadingModel.GL_SMOOTH);
                        }
                        else
                        {
                            MessageBox.Show("Not shade mode assigned to the Group " + iGroupIdx.ToString("00") + ".\n" +
                                            "I will assign shade mode 1 by default.", "Warning");

                            glShadeModel(GLShadingModel.GL_FLAT);
                            Model.Hundrets[iGroupIdx].shademode = 1;
                        }
                    }
                    else
                    {
                        glShadeModel(GLShadingModel.GL_FLAT);
                    }
                }


                //// V_ALPHABLEND
                if ((Model.Hundrets[iGroupIdx].field_C & 0x400) != 0)
                {
                    if ((Model.Hundrets[iGroupIdx].field_8 & 0x400) != 0)
                    {                        
                        SetBlendMode((BLEND_MODE)Model.Hundrets[iGroupIdx].blend_mode);
                    }
                    else
                    {
                        // BLEND NONE
                        SetBlendMode(BLEND_MODE.BLEND_NONE);
                    }
                }
                else
                {
                    if (Model.Hundrets[iGroupIdx].blend_mode == 0)
                        SetBlendMode(BLEND_MODE.BLEND_AVG);
                    else
                        SetBlendMode(BLEND_MODE.BLEND_DISABLED);
                }


                // This option enables somehow some few zSort order for specific Battle Stage model.
                // So, it is possible to know if the model is well rendered or it has any flaws,
                // like meshes wrong rendered or drawn.
                if (bSkeleton.IsBattleLocation)
                {
                    if (Model.fileName.Substring(Model.fileName.Length - 2, 2) == "AO")
                        glDepthFunc(GLFunc.GL_ALWAYS);
                    else
                        glDepthFunc(GLFunc.GL_LEQUAL);
                }


                if ((Model.Hundrets[iGroupIdx].field_C & 0x2) != 0)
                {
                    if(Model.Groups[iGroupIdx].texID < tex_ids.Length &&
                       Model.Groups[iGroupIdx].texFlag == 1)
                    {
                        if ((Model.Hundrets[iGroupIdx].field_8 & 0x2) != 0)
                        {
                            glEnable(GLCapability.GL_TEXTURE_2D);

                            if (glIsTexture(tex_ids[Model.Groups[iGroupIdx].texID]))
                            {
                                glBindTexture(GLTextureTarget.GL_TEXTURE_2D, 
                                              tex_ids[Model.Groups[iGroupIdx].texID]);
                            }
                        }
                    }
                    else
                    {
                        glDisable(GLCapability.GL_TEXTURE_2D);
                    }
                }


                // Use Group Reposition/Resize/Rotate changes.
                double[] rot_mat = new double[16];

                glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
                glPushMatrix();

                glTranslatef(Model.Groups[iGroupIdx].repGroupX,
                             Model.Groups[iGroupIdx].repGroupY,
                             Model.Groups[iGroupIdx].repGroupZ);

                BuildRotationMatrixWithQuaternionsXYZ(Model.Groups[iGroupIdx].rotGroupAlpha,
                                                      Model.Groups[iGroupIdx].rotGroupBeta,
                                                      Model.Groups[iGroupIdx].rotGroupGamma,
                                                      ref rot_mat);

                glMultMatrixd(rot_mat);
                glScalef(Model.Groups[iGroupIdx].rszGroupX,
                         Model.Groups[iGroupIdx].rszGroupY,
                         Model.Groups[iGroupIdx].rszGroupZ);

                DrawGroup(Model.Groups[iGroupIdx], Model.Polys, Model.Verts, Model.Vcolors,
                          Model.Normals, Model.NormalIndex, Model.TexCoords, 
                          Model.Hundrets[iGroupIdx], HideHiddenGroupsQ);

                glDisable(GLCapability.GL_TEXTURE_2D);

                glPopMatrix();

            }
        }

        public static void DrawGroupDList(ref PGroup Group)
        {
            glCallList((uint)Group.DListNum);
        }

        public static void DrawPModelDLists(ref PModel Model, ref uint[] tex_ids)
        {
            int iGroupIdx;

            //glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            //glPushMatrix();

            //glScalef(Model.resizeX, Model.resizeY, Model.resizeZ);
            //glRotatef(Model.rotateAlpha, 1, 0, 0);
            //glRotatef(Model.rotateBeta, 0, 1, 0);
            //glRotatef(Model.rotateGamma, 0, 0, 1);
            //glTranslatef(Model.repositionX, Model.repositionY, Model.repositionZ);

            glPolygonMode(GLFace.GL_FRONT, GLPolygon.GL_FILL);
            glPolygonMode(GLFace.GL_BACK, GLPolygon.GL_FILL);

            glEnable(GLCapability.GL_COLOR_MATERIAL);

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {

                if (Model.Hundrets[iGroupIdx].shademode == 1) glShadeModel(GLShadingModel.GL_FLAT);
                else glShadeModel(GLShadingModel.GL_SMOOTH);

                glDisable(GLCapability.GL_TEXTURE_2D);

                if (tex_ids.Length > 0)
                {
                    if (Model.Groups[iGroupIdx].texFlag == 1 && tex_ids[0] > 0)
                    {
                        if (Model.Groups[iGroupIdx].texID <= tex_ids.Length)
                        {
                            if (glIsTexture(tex_ids[Model.Groups[iGroupIdx].texID]))
                            {
                                glEnable(GLCapability.GL_TEXTURE_2D);

                                glBindTexture(GLTextureTarget.GL_TEXTURE_2D, tex_ids[Model.Groups[iGroupIdx].texID]);
                                glTexParameterf(GLTextureTarget.GL_TEXTURE_2D, GLTextureParameter.GL_TEXTURE_MAG_FILTER, (float)GLTextureMagFilter.GL_LINEAR);
                                glTexParameterf(GLTextureTarget.GL_TEXTURE_2D, GLTextureParameter.GL_TEXTURE_MIN_FILTER, (float)GLTextureMagFilter.GL_LINEAR);
                            }
                        }
                    }
                }
                else
                {
                    if (Model.Groups[iGroupIdx].texFlag == 1)
                    {
                        DialogResult drYesNo;
                        drYesNo = 
                            MessageBox.Show("The part: " + Model.fileName + " has not any texture assigned but it has " +
                                            "the texture flags enabled in group: " + iGroupIdx.ToString() + ".\n" +
                                            "Do you want to disable the texture flag?", "Warning", 
                                            MessageBoxButtons.YesNo);

                        if (drYesNo == DialogResult.Yes)
                        {
                            Model.Groups[iGroupIdx].texFlag = 0;
                        }
                    }
                }

                DrawGroupDList(ref Model.Groups[iGroupIdx]);

                glDisable(GLCapability.GL_TEXTURE_2D);
            }
        }

        public static void DrawPModelBoundingBox(PModel Model)
        {
            glBegin(GLDrawMode.GL_LINES);
            glDisable(GLCapability.GL_DEPTH_TEST);

            DrawBox(Model.BoundingBox.max_x, Model.BoundingBox.max_y, Model.BoundingBox.max_z,
                    Model.BoundingBox.min_x, Model.BoundingBox.min_y, Model.BoundingBox.min_z,
                    1, 1, 0);

            glEnable(GLCapability.GL_DEPTH_TEST);
            glEnd();
            // glPopMatrix();
        }

 

        //  ---------------------------------------------------------------------------------------------------
        //  ======================================== FIELD DRAW  ==============================================
        //  ---------------------------------------------------------------------------------------------------
        public static int MoveToFieldBone(FieldSkeleton fSkeleton, FieldFrame fFrame, int b_index)
        {
            int iBoneIdx, jsp;
            string[] joint_stack = new string[fSkeleton.bones.Count];

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);

            jsp = 0;
            joint_stack[jsp] = fSkeleton.bones[0].joint_f;

            for (iBoneIdx = 0; iBoneIdx < b_index; iBoneIdx++)
            {
                while (!(fSkeleton.bones[iBoneIdx].joint_f == joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }
                glPushMatrix();

                glRotated(fFrame.rotations[iBoneIdx].beta, 0, 1, 0);
                glRotated(fFrame.rotations[iBoneIdx].alpha, 1, 0, 0);
                glRotated(fFrame.rotations[iBoneIdx].gamma, 0, 0, 1);

                glTranslated(0, 0, -fSkeleton.bones[iBoneIdx].len);

                jsp++;
                joint_stack[jsp] = fSkeleton.bones[iBoneIdx].joint_i;
            }

            while (!(fSkeleton.bones[b_index].joint_f == joint_stack[jsp]) && jsp > 0)
            {
                glPopMatrix();
                jsp--;
            }
            glPushMatrix();

            glRotated(fFrame.rotations[b_index].beta, 0, 1, 0);
            glRotated(fFrame.rotations[b_index].alpha, 1, 0, 0);
            glRotated(fFrame.rotations[b_index].gamma, 0, 0, 1);

            return jsp + 1;
        }

        public static void DrawBox(float max_x, float max_y, float max_z,
                                   float min_x, float min_y, float min_z,
                                   float red, float green, float blue)
        {
            glColor3f(red, green, blue);

            glBegin(GLDrawMode.GL_LINES);

            glVertex3f(max_x, max_y, max_z);
            glVertex3f(max_x, max_y, min_z);
            glVertex3f(max_x, max_y, max_z);
            glVertex3f(max_x, min_y, max_z);
            glVertex3f(max_x, max_y, max_z);
            glVertex3f(min_x, max_y, max_z);

            glVertex3f(min_x, min_y, min_z);
            glVertex3f(min_x, min_y, max_z);
            glVertex3f(min_x, min_y, min_z);
            glVertex3f(min_x, max_y, min_z);
            glVertex3f(min_x, min_y, min_z);
            glVertex3f(max_x, min_y, min_z);

            glVertex3f(max_x, min_y, min_z);
            glVertex3f(max_x, max_y, min_z);
            glVertex3f(max_x, min_y, min_z);
            glVertex3f(max_x, min_y, max_z);

            glVertex3f(min_x, max_y, min_z);
            glVertex3f(min_x, max_y, max_z);
            glVertex3f(min_x, max_y, min_z);
            glVertex3f(max_x, max_y, min_z);

            glVertex3f(min_x, min_y, max_z);
            glVertex3f(min_x, max_y, max_z);
            glVertex3f(min_x, min_y, max_z);
            glVertex3f(max_x, min_y, max_z);

            glEnd();
        }

        public static void DrawFieldBonePieceBoundingBox(FieldBone bone, int p_index)
        {
            double[] rot_mat = new double[16];

            glDisable(GLCapability.GL_DEPTH_TEST);
            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glScalef(bone.resizeX, bone.resizeY, bone.resizeZ);

            glTranslatef(bone.fRSDResources[p_index].Model.repositionX,
                                  bone.fRSDResources[p_index].Model.repositionY,
                                  bone.fRSDResources[p_index].Model.repositionZ);

            BuildMatrixFromQuaternion(bone.fRSDResources[p_index].Model.rotationQuaternion, ref rot_mat);

            glMultMatrixd(rot_mat);
            glScalef(bone.fRSDResources[p_index].Model.resizeX,
                              bone.fRSDResources[p_index].Model.resizeY,
                              bone.fRSDResources[p_index].Model.resizeZ);

            DrawBox(bone.fRSDResources[p_index].Model.BoundingBox.max_x,
                    bone.fRSDResources[p_index].Model.BoundingBox.max_y,
                    bone.fRSDResources[p_index].Model.BoundingBox.max_z,
                    bone.fRSDResources[p_index].Model.BoundingBox.min_x,
                    bone.fRSDResources[p_index].Model.BoundingBox.min_y,
                    bone.fRSDResources[p_index].Model.BoundingBox.min_z,
                    0, 1, 0);

            glEnable(GLCapability.GL_DEPTH_TEST);
        }

        public static void DrawFieldBoneBoundingBox(FieldBone bone)
        {
            int iResourceIdx;

            float max_x, max_y, max_z;
            float min_x, min_y, min_z;

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glScalef(bone.resizeX, bone.resizeY, bone.resizeZ);

            if (bone.nResources == 0)
            {
                glDisable(GLCapability.GL_DEPTH_TEST);
                
                glColor3f(1, 0, 0);
                glBegin(GLDrawMode.GL_LINES);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, 0, -(float)bone.len);
                glEnd();

                glEnable(GLCapability.GL_DEPTH_TEST);
            }
            else
            {
                max_x = -(float)INFINITY_SINGLE;
                max_y = -(float)INFINITY_SINGLE;
                max_z = -(float)INFINITY_SINGLE;

                min_x = (float)INFINITY_SINGLE;
                min_y = (float)INFINITY_SINGLE;
                min_z = (float)INFINITY_SINGLE;

                for (iResourceIdx = 0; iResourceIdx < bone.nResources; iResourceIdx++)
                {
                    if (max_x < bone.fRSDResources[iResourceIdx].Model.BoundingBox.max_x) 
                            max_x = bone.fRSDResources[iResourceIdx].Model.BoundingBox.max_x;
                    if (max_y < bone.fRSDResources[iResourceIdx].Model.BoundingBox.max_y) 
                            max_y = bone.fRSDResources[iResourceIdx].Model.BoundingBox.max_y;
                    if (max_z < bone.fRSDResources[iResourceIdx].Model.BoundingBox.max_z) 
                            max_z = bone.fRSDResources[iResourceIdx].Model.BoundingBox.max_z;

                    if (min_x > bone.fRSDResources[iResourceIdx].Model.BoundingBox.min_x) 
                            min_x = bone.fRSDResources[iResourceIdx].Model.BoundingBox.min_x;
                    if (min_y > bone.fRSDResources[iResourceIdx].Model.BoundingBox.min_x) 
                            min_y = bone.fRSDResources[iResourceIdx].Model.BoundingBox.min_y;
                    if (min_z > bone.fRSDResources[iResourceIdx].Model.BoundingBox.min_x) 
                            min_z = bone.fRSDResources[iResourceIdx].Model.BoundingBox.min_z;
                }

                glDisable(GLCapability.GL_DEPTH_TEST);
                DrawBox(max_x, max_y, max_z, min_x, min_y, min_z, 1, 0, 0);
                glEnable(GLCapability.GL_DEPTH_TEST);
            }
        }

        public static void DrawFieldSkeletonBones(FieldSkeleton fSkeleton, FieldFrame fFrame)
        {
            int iBoneIdx, jsp;
            string[] joint_stack = new string[fSkeleton.bones.Count + 1];
            double[] rot_mat = new double[16];

            jsp = 0;

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPushMatrix();

            glTranslated(fFrame.rootTranslationX, 0, 0);
            glTranslated(0, -fFrame.rootTranslationY, 0);
            glTranslated(0, 0, fFrame.rootTranslationZ);

            BuildRotationMatrixWithQuaternions(fFrame.rootRotationAlpha, fFrame.rootRotationBeta, fFrame.rootRotationGamma, ref rot_mat);

            glMultMatrixd(rot_mat);
            glPointSize(5f);

            joint_stack[jsp] = fSkeleton.bones[0].joint_f;

            for (iBoneIdx = 0; iBoneIdx < fSkeleton.bones.Count; iBoneIdx++)
            {
                while ((fSkeleton.bones[iBoneIdx].joint_f != joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }

                glPushMatrix();

                // -- Commented in KimeraVB6
                //glRotated(fFrame.rotations[bi].beta, 0, 1, 0);
                //glRotated(fFrame.rotations[bi].alpha, 1, 0, 0);
                //glRotated(fFrame.rotations[bi].gamma, 0, 0, 1);
                BuildRotationMatrixWithQuaternions(fFrame.rotations[iBoneIdx].alpha, 
                                                   fFrame.rotations[iBoneIdx].beta, 
                                                   fFrame.rotations[iBoneIdx].gamma, 
                                                   ref rot_mat);
                glMultMatrixd(rot_mat);

                glBegin(GLDrawMode.GL_POINTS);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, 0, (float)-fSkeleton.bones[iBoneIdx].len);
                glEnd();

                glBegin(GLDrawMode.GL_LINES);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, 0, (float)-fSkeleton.bones[iBoneIdx].len);
                glEnd();

                glTranslated(0, 0, -fSkeleton.bones[iBoneIdx].len);

                jsp++;
                joint_stack[jsp] = fSkeleton.bones[iBoneIdx].joint_i;
            }

            while (jsp > 0)
            {
                glPopMatrix();
                jsp--;
            }
            glPopMatrix();
        }

        public static void DrawRSDResource(FieldRSDResource fRSDResource, bool bDListsEnable)
        {
            int iTextureIdx;
            uint[] tex_ids;
            double[] rot_mat = new double[16];

            tex_ids = new uint[fRSDResource.numTextures];

            for (iTextureIdx = 0; iTextureIdx < fRSDResource.numTextures; iTextureIdx++)
            {
                tex_ids[iTextureIdx] = fRSDResource.textures[iTextureIdx].texID;
            }

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPushMatrix();

            glTranslatef(fRSDResource.Model.repositionX, fRSDResource.Model.repositionY, fRSDResource.Model.repositionZ);
            BuildMatrixFromQuaternion(fRSDResource.Model.rotationQuaternion, ref rot_mat);

            glMultMatrixd(rot_mat);

            glScalef(fRSDResource.Model.resizeX, fRSDResource.Model.resizeY, fRSDResource.Model.resizeZ);

            SetDefaultOGLRenderState();

            if (!bDListsEnable) DrawPModel(ref fRSDResource.Model, ref tex_ids, false);
            else DrawPModelDLists(ref fRSDResource.Model, ref tex_ids);

            glPopMatrix();
        }

        public static void DrawFieldBone(FieldBone bone, bool bDListsEnable)
        {

            int iResourceIdx;

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPushMatrix();

            glScalef(bone.resizeX, bone.resizeY, bone.resizeZ);

            for (iResourceIdx = 0; iResourceIdx < bone.nResources; iResourceIdx++)
                DrawRSDResource(bone.fRSDResources[iResourceIdx], bDListsEnable);

            glPopMatrix();
        }

        public static void DrawFieldSkeleton(FieldSkeleton fSkeleton, FieldFrame fFrame, bool bDListsEnable)
        {
            int iBoneIdx;
            string[] joint_stack = new string[fSkeleton.bones.Count + 1];
            int jsp;
            double[] rot_mat = new double[16];

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);

            glPushMatrix();
            glTranslated(fFrame.rootTranslationX, 0, 0);
            glTranslated(0, -fFrame.rootTranslationY, 0);
            glTranslated(0, 0, fFrame.rootTranslationZ);

            BuildRotationMatrixWithQuaternions(fFrame.rootRotationAlpha, fFrame.rootRotationBeta, fFrame.rootRotationGamma, ref rot_mat);

            glMultMatrixd(rot_mat);

            jsp = 0;
            joint_stack[jsp] = fSkeleton.bones[0].joint_f;

            //for (bi = 0; bi < Skeleton.nBones; bi++)
            for (iBoneIdx = 0; iBoneIdx < fSkeleton.bones.Count; iBoneIdx++)
            {
                while (!(fSkeleton.bones[iBoneIdx].joint_f == joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }

                //if (jsp == 0) SetDefaultOGLRenderState();

                glPushMatrix();

                BuildRotationMatrixWithQuaternions(fFrame.rotations[iBoneIdx].alpha, 
                                                   fFrame.rotations[iBoneIdx].beta, 
                                                   fFrame.rotations[iBoneIdx].gamma, 
                                                   ref rot_mat);

                glMultMatrixd(rot_mat);

                DrawFieldBone(fSkeleton.bones[iBoneIdx], bDListsEnable);

                glTranslated(0, 0, -fSkeleton.bones[iBoneIdx].len);

                jsp++;
                joint_stack[jsp] = fSkeleton.bones[iBoneIdx].joint_i;
            }

            while (jsp > 0)
            {
                glPopMatrix();
                jsp--;
            }
            glPopMatrix();
        }


        //  ---------------------------------------------------------------------------------------------------
        //  ======================================== BATTLE DRAW  =============================================
        //  ---------------------------------------------------------------------------------------------------
        public static int MoveToBattleBone(BattleSkeleton bSkeleton, BattleFrame bFrame, int boneIndex)
        {
            int iBoneIdx, jsp, itmpbones;
            int[] joint_stack = new int[bSkeleton.nBones * 4];
            double[] rot_mat = new double[16];

            jsp = 0;
            joint_stack[jsp] = -1;

            if (bSkeleton.nBones > 1) itmpbones = 1;
            else itmpbones = 0;

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPushMatrix();
            glTranslated(bFrame.startX, bFrame.startY, bFrame.startZ);

            BuildRotationMatrixWithQuaternions(bFrame.bones[0].alpha, bFrame.bones[0].beta, bFrame.bones[0].gamma, ref rot_mat);
            glMultMatrixd(rot_mat);

            for (iBoneIdx = 0; iBoneIdx < boneIndex; iBoneIdx++)
            {
                glPushName((uint)iBoneIdx);

                while (!(bSkeleton.bones[iBoneIdx].parentBone == joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }
                glPushMatrix();

                // -- Commented in KimeraVB6
                //  glRotated(bFrame.bones[bi + 1].beta, 0, 1, 0);
                //  glRotated(bFrame.bones[bi + 1].alpha, 1, 0, 0);
                //  glRotated(bFrame.bones[bi + 1].gamma, 0, 0, 1);

                BuildRotationMatrixWithQuaternions(bFrame.bones[iBoneIdx + itmpbones].alpha, 
                                                       bFrame.bones[iBoneIdx + itmpbones].beta, 
                                                       bFrame.bones[iBoneIdx + itmpbones].gamma, ref rot_mat);
                glMultMatrixd(rot_mat);

                glTranslated(0, 0, bSkeleton.bones[iBoneIdx].len);

                jsp++;
                joint_stack[jsp] = iBoneIdx;

                glPopName();
            }

            while (!(bSkeleton.bones[iBoneIdx].parentBone == joint_stack[jsp]) && jsp > 0)
            {
                glPopMatrix();
                jsp--;
            }

            // -- Commented in KimeraVB6
            // glPopMatrix();
            //  glRotated(bFrame.bones[boneIndex + itmpbones].beta, 0, 1, 0);
            //  glRotated(bFrame.bones[boneIndex + itmpbones].alpha, 1, 0, 0);
            //  glRotated(bFrame.bones[boneIndex + itmpbones].gamma, 0, 0, 1);

            BuildRotationMatrixWithQuaternions(bFrame.bones[boneIndex + itmpbones].alpha,
                                               bFrame.bones[boneIndex + itmpbones].beta,
                                               bFrame.bones[boneIndex + itmpbones].gamma,
                                               ref rot_mat);
            glMultMatrixd(rot_mat);

            return jsp + 1;
        }

        public static int MoveToBattleBoneMiddle(BattleSkeleton bSkeleton, BattleFrame bFrame, int boneIndex)
        {
            int iMoveToBattleBoneMiddleResult;

            iMoveToBattleBoneMiddleResult = MoveToBattleBone(bSkeleton, bFrame, boneIndex);
            glTranslated(0, 0, bSkeleton.bones[boneIndex].len / 2);

            return iMoveToBattleBoneMiddleResult;
        }

        public static int MoveToBattleBoneEnd(BattleSkeleton bSkeleton, BattleFrame bFrame, int boneIndex)
        {
            int iMoveToBattleBoneEndResult;

            iMoveToBattleBoneEndResult = MoveToBattleBone(bSkeleton, bFrame, boneIndex);
            glTranslated(0, 0, bSkeleton.bones[boneIndex].len);

            return iMoveToBattleBoneEndResult;
        }

        public static void DrawBattleBoneModelBoundingBox(BattleBone bBone, int partIndex)
        {
            double[] rot_mat = new double[16];

            glDisable(GLCapability.GL_DEPTH_TEST);
            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glScalef(bBone.resizeX, bBone.resizeY, bBone.resizeZ);

            glTranslatef(bBone.Models[partIndex].repositionX, bBone.Models[partIndex].repositionY, bBone.Models[partIndex].repositionZ);

            BuildMatrixFromQuaternion(bBone.Models[partIndex].rotationQuaternion, ref rot_mat);
            glMultMatrixd(rot_mat);

            glScalef(bBone.Models[partIndex].resizeX, bBone.Models[partIndex].resizeY, bBone.Models[partIndex].resizeZ);

            DrawBox(bBone.Models[partIndex].BoundingBox.max_x, bBone.Models[partIndex].BoundingBox.max_y, bBone.Models[partIndex].BoundingBox.max_z,
                    bBone.Models[partIndex].BoundingBox.min_x, bBone.Models[partIndex].BoundingBox.min_y, bBone.Models[partIndex].BoundingBox.min_z,
                    0, 1, 0);
            glEnable(GLCapability.GL_DEPTH_TEST);
        }

        public static void DrawBattleBoneBoundingBox(BattleBone bBone)
        {

            glDisable(GLCapability.GL_DEPTH_TEST);
            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);

            glScalef(bBone.resizeX, bBone.resizeY, bBone.resizeZ);

            if (bBone.hasModel == 1)
            {
                // -- Commented in KimeraVB6
                //glTranslatef(bBone.Models[0].repositionX, bBone.Models[0].repositionY, bBone.Models[0].repositionZ);
                //BuildMatrixFromQuaternion(ref bBone.Models[0].rotationQuaternion, ref rot_mat);
                //glMultMatrixd(rot_mat);
                //glScalef(bBone.Models[0].resizeX, bBone.Models[0].resizeY, bBone.Models[0].resizeZ);

                DrawBox(bBone.Models[0].BoundingBox.max_x, bBone.Models[0].BoundingBox.max_y, bBone.Models[0].BoundingBox.max_z,
                        bBone.Models[0].BoundingBox.min_x, bBone.Models[0].BoundingBox.min_y, bBone.Models[0].BoundingBox.min_z,
                        0, 1, 0);
                glEnable(GLCapability.GL_DEPTH_TEST);
            }
            else
            {
                glColor3f(0, 1, 0);
                glBegin(GLDrawMode.GL_LINES);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, 0, bBone.len);
                glEnd();
            }

            glEnable(GLCapability.GL_DEPTH_TEST);
        }

        public static void DrawBattleWeaponBoundingBox(BattleSkeleton bSkeleton, BattleFrame wpFrame, int weaponIndex)
        {
            double[] rot_mat = new double[16];

            //if (weaponIndex > -1 && bSkeleton.nWeapons > 0)       // -- Commented in KimeraVB6
            if (ianimWeaponIndex > -1 && bSkeleton.wpModels.Count > 0 && bAnimationsPack.WeaponAnimations.Count > 0)
            {
                glPushMatrix();
                glTranslated(wpFrame.startX, wpFrame.startY, wpFrame.startZ);

                // -- Commented in KimeraVB6
                //glRotated(wpFrame.bones[0].beta, 0, 1, 0);
                //glRotated(wpFrame.bones[0].alpha, 1, 0, 0);
                //glRotated(wpFrame.bones[0].gamma, 0, 0, 1);

                BuildRotationMatrixWithQuaternions(wpFrame.bones[0].alpha, wpFrame.bones[0].beta, wpFrame.bones[0].gamma, ref rot_mat);
                glMultMatrixd(rot_mat);

                glPushMatrix();

                glTranslatef(bSkeleton.wpModels[weaponIndex].repositionX,
                             bSkeleton.wpModels[weaponIndex].repositionY,
                             bSkeleton.wpModels[weaponIndex].repositionZ);
                
                glRotated(bSkeleton.wpModels[weaponIndex].rotateBeta, 0, 1, 0);
                glRotated(bSkeleton.wpModels[weaponIndex].rotateAlpha, 1, 0, 0);
                glRotated(bSkeleton.wpModels[weaponIndex].rotateGamma, 0, 0, 1);
                
                glScalef(bSkeleton.wpModels[weaponIndex].resizeX, bSkeleton.wpModels[weaponIndex].resizeY, bSkeleton.wpModels[weaponIndex].resizeZ);

                DrawPModelBoundingBox(bSkeleton.wpModels[weaponIndex]);

                glPopMatrix();
                glPopMatrix();
            }
        }

        public static void DrawBattleSkeletonBones(BattleSkeleton bSkeleton, BattleFrame bFrame)
        {
            int iBoneIdx, jsp, itmpbones;
            int[] joint_stack;
            double[] rot_mat = new double[16];

            if (bSkeleton.IsBattleLocation) return;

            joint_stack = new int[bSkeleton.nBones + 1];
            jsp = 0;
            joint_stack[jsp] = -1;

            if (bSkeleton.nBones > 1) itmpbones = 1;
            else itmpbones = 0;

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPointSize(5);
            glPushMatrix();
            glTranslated(bFrame.startX, bFrame.startY, bFrame.startZ);

            BuildRotationMatrixWithQuaternions(bFrame.bones[0].alpha, bFrame.bones[0].beta, bFrame.bones[0].gamma, ref rot_mat);
            glMultMatrixd(rot_mat);

            for (iBoneIdx = 0; iBoneIdx < bSkeleton.nBones; iBoneIdx++)
            {
                while (!(bSkeleton.bones[iBoneIdx].parentBone == joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }
                glPushMatrix();

                // -- Commented in KimeraVB6
                //glRotated(bFrame.bones[bi + 1].beta, 0, 1, 0);
                //glRotated(bFrame.bones[bi + 1].alpha, 1, 0, 0);
                //glRotated(bFrame.bones[bi + 1].gamma, 0, 0, 1);

                BuildRotationMatrixWithQuaternions(bFrame.bones[iBoneIdx + itmpbones].alpha,
                                                   bFrame.bones[iBoneIdx + itmpbones].beta,
                                                   bFrame.bones[iBoneIdx + itmpbones].gamma,
                                                   ref rot_mat);
                glMultMatrixd(rot_mat);

                glBegin(GLDrawMode.GL_POINTS);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, 0, bSkeleton.bones[iBoneIdx].len);
                glEnd();

                glBegin(GLDrawMode.GL_LINES);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, 0, bSkeleton.bones[iBoneIdx].len);
                glEnd();

                glTranslated(0, 0, bSkeleton.bones[iBoneIdx].len);

                jsp++;
                joint_stack[jsp] = iBoneIdx;
            }

            if (!bSkeleton.IsBattleLocation)
            {
                while (jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }
            }
            glPopMatrix();
        }

        public static void DrawBattleSkeletonBone(BattleBone bBone, uint[] texIDS, bool bDListsEnable)
        {
            int iModelIdx;
            PModel tmpbModel = new PModel();

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPushMatrix();
            glScalef(bBone.resizeX, bBone.resizeY, bBone.resizeZ);

            if (bBone.hasModel > 0)
            {

                if (!bDListsEnable)
                {
                    for (iModelIdx = 0; iModelIdx < bBone.nModels; iModelIdx++)
                    {

                        glPushMatrix();
                        glTranslatef(bBone.Models[iModelIdx].repositionX, 
                                     bBone.Models[iModelIdx].repositionY, 
                                     bBone.Models[iModelIdx].repositionZ);

                        glRotated(bBone.Models[iModelIdx].rotateAlpha, 1, 0, 0);
                        glRotated(bBone.Models[iModelIdx].rotateBeta, 0, 1, 0);
                        glRotated(bBone.Models[iModelIdx].rotateGamma, 0, 0, 1);

                        glScalef(bBone.Models[iModelIdx].resizeX, 
                                 bBone.Models[iModelIdx].resizeY, 
                                 bBone.Models[iModelIdx].resizeZ);
                        
                        SetDefaultOGLRenderState();

                        tmpbModel = bBone.Models[iModelIdx];
                        DrawPModel(ref tmpbModel, ref texIDS, false);
                        bBone.Models[iModelIdx] = tmpbModel;

                        glPopMatrix();
                    }
                }
                else
                {
                    for (iModelIdx = 0; iModelIdx < bBone.nModels; iModelIdx++)
                    {
                        glPushMatrix();
                        glTranslatef(bBone.Models[iModelIdx].repositionX, 
                                     bBone.Models[iModelIdx].repositionY, 
                                     bBone.Models[iModelIdx].repositionZ);

                        glRotated(bBone.Models[iModelIdx].rotateAlpha, 1, 0, 0);
                        glRotated(bBone.Models[iModelIdx].rotateBeta, 0, 1, 0);
                        glRotated(bBone.Models[iModelIdx].rotateGamma, 0, 0, 1);

                        glScalef(bBone.Models[iModelIdx].resizeX, 
                                 bBone.Models[iModelIdx].resizeY, 
                                 bBone.Models[iModelIdx].resizeZ);

                        tmpbModel = bBone.Models[iModelIdx];
                        DrawPModelDLists(ref tmpbModel, ref texIDS);
                        bBone.Models[iModelIdx] = tmpbModel;

                        glPopMatrix();
                    }
                }
            }

            glPopMatrix();
        }

        public static void DrawBattleSkeleton(BattleSkeleton bSkeleton, BattleFrame bFrame, BattleFrame wpFrame,
                                              int weaponIndex, bool bDListsEnable)
        {
            int iBoneIdx, jsp, itmpbones;
            int[] joint_stack = new int[bSkeleton.nBones + 1];
            double[] rot_mat = new double[16];

            jsp = 0;
            joint_stack[jsp] = -1;

            if (bSkeleton.nBones > 1) itmpbones = 1;
            else itmpbones = 0;

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPushMatrix();
            glTranslated(bFrame.startX, bFrame.startY, bFrame.startZ);

            // Debug.Print bFrame.bones[0].alpha; ", "; bFrame.bones[0].Beta; ", "; bFrame.bones[0].Gamma
            BuildRotationMatrixWithQuaternions(bFrame.bones[0].alpha, bFrame.bones[0].beta, bFrame.bones[0].gamma, ref rot_mat);
            glMultMatrixd(rot_mat);

            for (iBoneIdx = 0; iBoneIdx < bSkeleton.nBones; iBoneIdx++)
            {
                if (bSkeleton.IsBattleLocation)
                {
                    DrawBattleSkeletonBone(bSkeleton.bones[iBoneIdx], bSkeleton.TexIDS, false);
                }
                else
                {
                    while (!(bSkeleton.bones[iBoneIdx].parentBone == joint_stack[jsp]) && jsp > 0)
                    {
                        glPopMatrix();
                        jsp--;
                    }

                    glPushMatrix();

                    // -- Commented in KimeraVB6
                    //glRotated(bFrame.bones[bi + 1].beta, 0, 1, 0);
                    //glRotated(bFrame.bones[bi + 1].alpha, 1, 0, 0);
                    //glRotated(bFrame.bones[bi + 1].gamma, 0, 0, 1);

                    BuildRotationMatrixWithQuaternions(bFrame.bones[iBoneIdx + itmpbones].alpha,
                                                       bFrame.bones[iBoneIdx + itmpbones].beta,
                                                       bFrame.bones[iBoneIdx + itmpbones].gamma,
                                                       ref rot_mat);
                    glMultMatrixd(rot_mat);

                    DrawBattleSkeletonBone(bSkeleton.bones[iBoneIdx], bSkeleton.TexIDS, bDListsEnable);

                    glTranslated(0, 0, bSkeleton.bones[iBoneIdx].len);

                    jsp++;
                    joint_stack[jsp] = iBoneIdx;
                }
            }

            if (!bSkeleton.IsBattleLocation)
            {
                while (jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }
            }
            glPopMatrix();

            //if (weaponIndex > -1 && bSkeleton.nWeapons > 0)       // -- Commented in KimeraVB6
            if (ianimWeaponIndex > -1 && bSkeleton.wpModels.Count > 0 && bAnimationsPack.WeaponAnimations.Count > 0)
            {
                glPushMatrix();
                glTranslated(wpFrame.startX, wpFrame.startY, wpFrame.startZ);

                // -- Commented in KimeraVB6
                //glRotated(wpFrame.bones[0].beta, 0, 1, 0);
                //glRotated(wpFrame.bones[0].alpha, 1, 0, 0);
                //glRotated(wpFrame.bones[0].gamma, 0, 0, 1);

                BuildRotationMatrixWithQuaternions(wpFrame.bones[0].alpha, wpFrame.bones[0].beta, wpFrame.bones[0].gamma, ref rot_mat);
                glMultMatrixd(rot_mat);

                glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
                glPushMatrix();

                glTranslatef(bSkeleton.wpModels[weaponIndex].repositionX,
                             bSkeleton.wpModels[weaponIndex].repositionY,
                             bSkeleton.wpModels[weaponIndex].repositionZ);

                glRotated(bSkeleton.wpModels[weaponIndex].rotateAlpha, 1, 0, 0);
                glRotated(bSkeleton.wpModels[weaponIndex].rotateBeta, 0, 1, 0);
                glRotated(bSkeleton.wpModels[weaponIndex].rotateGamma, 0, 0, 1);

                glScalef(bSkeleton.wpModels[weaponIndex].resizeX, bSkeleton.wpModels[weaponIndex].resizeY, bSkeleton.wpModels[weaponIndex].resizeZ);

                SetDefaultOGLRenderState();

                PModel tmpwpModel = new PModel();
                if (bDListsEnable)
                {
                    tmpwpModel = bSkeleton.wpModels[weaponIndex];
                    DrawPModelDLists(ref tmpwpModel, ref bSkeleton.TexIDS);
                    bSkeleton.wpModels[weaponIndex] = tmpwpModel;
                }
                else
                {
                    tmpwpModel = bSkeleton.wpModels[weaponIndex];
                    DrawPModel(ref tmpwpModel, ref bSkeleton.TexIDS, false);
                    bSkeleton.wpModels[weaponIndex] = tmpwpModel;
                }
                glPopMatrix();

                glPopMatrix();
            }
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ======================================= SKELETON DRAW  ============================================
        //  ---------------------------------------------------------------------------------------------------
        public static void DrawSkeletonModel(bool bDListsEnable)
        {

            double[] rot_mat = new double[16];
            BattleFrame tmpbFrame;

            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            try
            {
                switch (modelType)
                {
                    case K_3DS_MODEL:
                    case K_P_FIELD_MODEL:
                    case K_P_BATTLE_MODEL:
                    case K_P_MAGIC_MODEL:
                        ComputePModelBoundingBox(fPModel, ref p_min, ref p_max);

                        SetCameraAroundModel(ref p_min, ref p_max,
                                             panX, panY, (float)(panZ + DIST),
                                             (float)alpha, (float)beta, (float)gamma, 1, 1, 1);

                        if (bShowGround)
                        {
                            glDisable(GLCapability.GL_LIGHTING);
                            DrawGround();
                            DrawShadow(ref p_min, ref p_max);
                        }

                        SetLights();

                        glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
                        glPushMatrix();

                        glTranslatef(fPModel.repositionX,
                                     fPModel.repositionY,
                                     fPModel.repositionZ);

                        BuildRotationMatrixWithQuaternionsXYZ(fPModel.rotateAlpha,
                                                              fPModel.rotateBeta,
                                                              fPModel.rotateGamma,
                                                              ref rot_mat);

                        glMultMatrixd(rot_mat);
                        glScalef(fPModel.resizeX,
                                 fPModel.resizeY,
                                 fPModel.resizeZ);

                        DrawPModel(ref fPModel, ref tex_ids, false);

                        glPopMatrix();

                        break;

                    case K_HRC_SKELETON:
                        ComputeFieldBoundingBox(fSkeleton, fAnimation.frames[iCurrentFrameScroll], 
                                                ref p_min, ref p_max);

                        SetCameraAroundModel(ref p_min, ref p_max, 
                                             panX, panY, (float)(panZ + DIST),
                                             (float)alpha, (float)beta, (float)gamma, 1, 1, 1);

                        if (bShowGround)
                        {
                            glDisable(GLCapability.GL_LIGHTING);
                            DrawGround();
                            DrawShadow(ref p_min, ref p_max);
                        }

                        SetLights();

                        DrawFieldSkeleton(fSkeleton, fAnimation.frames[iCurrentFrameScroll], bDListsEnable);

                        if (bShowLastFrameGhost)
                        {

                            glColorMask(GL_Boolean.GL_TRUE, GL_Boolean.GL_TRUE, GL_Boolean.GL_FALSE, GL_Boolean.GL_TRUE);
                            if (iCurrentFrameScroll == 0)
                                DrawFieldSkeleton(fSkeleton, fAnimation.frames[fAnimation.nFrames - 1], 
                                                  bDListsEnable);
                            else
                                DrawFieldSkeleton(fSkeleton, fAnimation.frames[iCurrentFrameScroll - 1], 
                                                  bDListsEnable);

                            glColorMask(GL_Boolean.GL_TRUE, GL_Boolean.GL_TRUE, GL_Boolean.GL_TRUE, GL_Boolean.GL_TRUE);
                        }

                        glDisable(GLCapability.GL_LIGHTING);

                        if (bShowBones)
                        {                        
                            glDisable(GLCapability.GL_DEPTH_TEST);
                            glColor3f(0, 1, 0);

                            DrawFieldSkeletonBones(fSkeleton, fAnimation.frames[iCurrentFrameScroll]);

                            glEnable(GLCapability.GL_DEPTH_TEST);
                        }

                        SelectFieldBoneAndPiece(fSkeleton, fAnimation.frames[iCurrentFrameScroll],
                                                SelectedBone, SelectedBonePiece);
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        //if (!bSkeleton.IsBattleLocation)
                        //int     animIndex = Int32.Parse(cbBattleAnimation.Items[cbBattleAnimation.SelectedIndex].ToString());

                        ComputeBattleBoundingBox(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[iCurrentFrameScroll],
                                                 ref p_min, ref p_max);

                        SetCameraAroundModel(ref p_min, ref p_max,
                                             panX, panY, (float)(panZ + DIST), 
                                             (float)alpha, (float)beta, (float)gamma, 1, 1, 1);

                        if (bShowGround)
                        {
                            glDisable(GLCapability.GL_LIGHTING);
                            DrawGround();
                            DrawShadow(ref p_min, ref p_max);
                        }

                        SetLights();

                        tmpbFrame = new BattleFrame();
                        if (bSkeleton.wpModels.Count > 0 && bAnimationsPack.WeaponAnimations.Count > 0)
                        {
                            tmpbFrame = bAnimationsPack.WeaponAnimations[ianimIndex].frames[iCurrentFrameScroll];
                        }

                        DrawBattleSkeleton(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[iCurrentFrameScroll],
                                           tmpbFrame, ianimWeaponIndex, bDListsEnable);

                        if (bShowLastFrameGhost && !bSkeleton.IsBattleLocation)
                        {
                            glColorMask(GL_Boolean.GL_TRUE, GL_Boolean.GL_TRUE, GL_Boolean.GL_FALSE, GL_Boolean.GL_TRUE);
                            
                            if (iCurrentFrameScroll == 0)
                            {
                                DrawBattleSkeleton(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[iCurrentFrameScroll],
                                                   tmpbFrame, ianimWeaponIndex, bDListsEnable);
                            }
                            else
                                DrawBattleSkeleton(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[iCurrentFrameScroll - 1],
                                                   tmpbFrame, ianimWeaponIndex, bDListsEnable);

                            glColorMask(GL_Boolean.GL_TRUE, GL_Boolean.GL_TRUE, GL_Boolean.GL_TRUE, GL_Boolean.GL_TRUE);
                        }

                        glDisable(GLCapability.GL_LIGHTING);

                        if (bShowBones)
                        {
                            glDisable(GLCapability.GL_DEPTH_TEST);
                            glColor3f(0, 1, 0);
                            DrawBattleSkeletonBones(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[iCurrentFrameScroll]);
                            glEnable(GLCapability.GL_DEPTH_TEST);
                        }

                        SelectBattleBoneAndModel(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[iCurrentFrameScroll],
                                                 tmpbFrame, ianimWeaponIndex, SelectedBone, SelectedBonePiece);
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error Drawing current model: " + e.Message + ".", "Error");
            }
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ============================ DRAWING EXTENSIONS (GROUND/SHADOW)  ==================================
        //  ---------------------------------------------------------------------------------------------------
        public static void DrawGround()
        {
            int gi, lw;
            float r, g, b;

            //  Draw plane
            glColor3f(0.9f, 0.9f, 1f);
            glDisable(GLCapability.GL_DEPTH_TEST);
            
            glBegin(GLDrawMode.GL_QUADS);
                glVertex4f(300f, 0f, 300f, 0.001f);
                glVertex4f(300f, 0f, -300f, 0.001f);
                glVertex4f(-300f, 0f, -300f, 0.001f);
                glVertex4f(-300f, 0f, 300f, 0.001f);
            glEnd();

            r = 0.9f;
            g = 0.9f;
            b = 1.0f;
            lw = 5;
            //glEnable(GLCapability.GL_LINE_SMOOTH);

            for (gi = 10; gi >= 5; gi--)
            {
                glLineWidth(lw);
                glColor3f(r, g, b);
                glBegin(GLDrawMode.GL_LINES);
                    glVertex4f(0f, 0f, 50f, 0.0001f);
                    glVertex4f(0f, 0f, -50f, 0.0001f);
                    glVertex4f(-50f, 0f, 0f, 0.0001f);
                    glVertex4f(50f, 0f, 0f, 0.0001f);
                glEnd();

                r = 0.9f - 0.9f / 10f * (10 - gi);
                g = 0.9f - 0.9f / 10f * (10 - gi);
                b = 1 - 1f / 10f * (10 - gi);
                lw--;
            }

            glLineWidth(1f);
            //glDisable(GLCapability.GL_LINE_SMOOTH);
        }

        public static void DrawShadow(ref Point3D p_min, ref Point3D p_max)
        {
            float ground_radius, sub_y, cx, cz;
            int numSegments, si;

            Point3D p_min_aux, p_max_aux;

            sub_y = p_max.y;
            p_min_aux = p_min;
            p_max_aux = p_max;
            p_min_aux.y = 0;
            p_max_aux.y = 0;

            cx = (p_min.x + p_max.x) / 2;
            cz = (p_min.z + p_max.z) / 2;
            ground_radius = CalculateDistance(p_min_aux, p_max_aux) / 2;

            // Draw Shadow
            SetBlendMode(BLEND_MODE.BLEND_AVG);

            numSegments = 20;
            glBegin(GLDrawMode.GL_TRIANGLE_FAN);
                glColor4f(0.1f, 0.1f, 0.1f, 0.5f);
                glVertex3f(cx, 0, cz);

                for (si = 0; si <= numSegments; si++)
                {
                    glColor4f(0.1f, 0.1f, 0.1f, 0);
                    glVertex3f((float)(ground_radius * Math.Sin(si * 2 * PI / numSegments) + cx), 0,
                               (float)(ground_radius * Math.Cos(si * 2 * PI / numSegments) + cz));
                }
            glEnd();

            glEnable(GLCapability.GL_DEPTH_TEST);
            glDisable(GLCapability.GL_FOG);

            // Draw underlying box (just depth)
            glColorMask(GL_Boolean.GL_FALSE, GL_Boolean.GL_FALSE, GL_Boolean.GL_FALSE, GL_Boolean.GL_FALSE);
            glColor3f(1, 1, 1);
            glBegin(GLDrawMode.GL_QUADS);
                glVertex3f(p_max.x, 0, p_max.z);
                glVertex3f(p_max.x, 0, p_min.z);
                glVertex3f(p_min.x, 0, p_min.z);
                glVertex3f(p_min.x, 0, p_max.z);

                glVertex3f(p_max.x, 0, p_max.z);
                glVertex3f(p_max.x, sub_y, p_max.z);
                glVertex3f(p_max.x, sub_y, p_min.z);
                glVertex3f(p_max.x, 0, p_min.z);

                glVertex3f(p_max.x, 0, p_min.z);
                glVertex3f(p_max.x, sub_y, p_min.z);
                glVertex3f(p_min.x, sub_y, p_min.z);
                glVertex3f(p_min.x, 0, p_min.z);

                glVertex3f(p_min.x, sub_y, p_max.z);
                glVertex3f(p_min.x, 0, p_max.z);
                glVertex3f(p_min.x, 0, p_min.z);
                glVertex3f(p_min.x, sub_y, p_min.z);

                glVertex3f(p_max.x, sub_y, p_max.z);
                glVertex3f(p_max.x, 0, p_max.z);
                glVertex3f(p_min.x, 0, p_max.z);
                glVertex3f(p_min.x, sub_y, p_max.z);
            glEnd();
            glColorMask(GL_Boolean.GL_TRUE, GL_Boolean.GL_TRUE, GL_Boolean.GL_TRUE, GL_Boolean.GL_TRUE);
        }

        public static void DrawPlane(ref double[] planeTransformation, ref Point3D planeOriginalPoint1,
                                                                       ref Point3D planeOriginalPoint2,
                                                                       ref Point3D planeOriginalPoint3,
                                                                       ref Point3D planeOriginalPoint4)
        {
            Point3D p1 = new Point3D();
            Point3D p2 = new Point3D();
            Point3D p3 = new Point3D();
            Point3D p4 = new Point3D();

            glDisable(GLCapability.GL_CULL_FACE);

            SetBlendMode(BLEND_MODE.BLEND_AVG);

            MultiplyPoint3DByOGLMatrix(planeTransformation, planeOriginalPoint1, ref p1);
            MultiplyPoint3DByOGLMatrix(planeTransformation, planeOriginalPoint2, ref p2);
            MultiplyPoint3DByOGLMatrix(planeTransformation, planeOriginalPoint3, ref p3);
            MultiplyPoint3DByOGLMatrix(planeTransformation, planeOriginalPoint4, ref p4);

            glPolygonMode(GLFace.GL_FRONT, GLPolygon.GL_FILL);
            glPolygonMode(GLFace.GL_BACK, GLPolygon.GL_LINE);

            glColor4f(1, 0, 0, 0.10f);

            glBegin(GLDrawMode.GL_QUADS);
                glVertex3f(p1.x, p1.y, p1.z);
                glVertex3f(p2.x, p2.y, p2.z);
                glVertex3f(p3.x, p3.y, p3.z);
                glVertex3f(p4.x, p4.y, p4.z);
            glEnd();
        }

        public static void DrawAxes(PictureBox pbIn, int iFrame)
        {
            float letterWidth, letterHeight;
            float max_x, max_y, max_z;

            Point3D pX = new Point3D();
            Point3D pY = new Point3D();
            Point3D pZ = new Point3D();

            Point3D p_max = new Point3D();
            Point3D p_min = new Point3D();

            glDisable(GLCapability.GL_LIGHTING);

            switch(modelType)
            {
                case K_HRC_SKELETON:
                    ComputeFieldBoundingBox(fSkeleton, fAnimation.frames[iFrame],
                                            ref p_min, ref p_max);
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    ComputeBattleBoundingBox(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[iFrame],
                                             ref p_min, ref p_max);

                    break;

                case K_3DS_MODEL:
                case K_P_BATTLE_MODEL:
                case K_P_FIELD_MODEL:
                case K_P_MAGIC_MODEL:
                    ComputePModelBoundingBox(fPModel, ref p_min, ref p_max);

                    break;

            }

            //max_x = Math.Abs(p_min.x) > Math.Abs(p_max.x) ? p_min.x : p_max.x;
            //max_y = Math.Abs(p_min.y) > Math.Abs(p_max.y) ? p_min.y : p_max.y;
            //max_z = Math.Abs(p_min.z) > Math.Abs(p_max.z) ? p_min.z : p_max.z;

            max_x = Math.Abs(p_max.x - p_min.x);
            max_y = Math.Abs(p_max.y - p_min.y);
            max_z = Math.Abs(p_max.z - p_min.z);

            if (max_x > max_y && max_x > max_z) max_y = max_z = max_x;
            if (max_y > max_x && max_y > max_z) max_x = max_z = max_y;
            if (max_z > max_x && max_z > max_y) max_x = max_y = max_z;

            glBegin(GLDrawMode.GL_LINES);
                glColor3f(1, 0, 0);
                glVertex3f(0, 0, 0);
                glVertex3f(max_x, 0, 0);

                if (bSkeleton.IsBattleLocation)
                {
                    glColor3f(0, 1, 0);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, -max_y, 0);

                    glColor3f(0, 0, 1);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, 0, max_z);
                }
                else 
                {
                    glColor3f(0, 0, 1);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, -max_y, 0);

                    glColor3f(0, 1, 0);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, 0, max_z);
                }
            glEnd();

            //  Get projected end of the X axis
            pX.x = max_x;
            pX.y = 0;
            pX.z = 0;
            pX = GetProjectedCoords(pX);

            //  Get projected end of the Y axis
            pY.x = 0;
            pY.y = -max_y;
            pY.z = 0;
            pY = GetProjectedCoords(pY);

            //  Get projected end of the Z axis
            pZ.x = 0;
            pZ.y = 0;
            pZ.z = max_z;
            pZ = GetProjectedCoords(pZ);


            //  Set 2D mode to draw letters
            glMatrixMode(GLMatrixModeList.GL_PROJECTION);
            glLoadIdentity();
            gluOrtho2D(0, pbIn.ClientRectangle.Width, 0, pbIn.ClientRectangle.Height);
            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glLoadIdentity();

            letterWidth = LETTER_SIZE;
            letterHeight = (float)(LETTER_SIZE * 1.5);
            glDisable(GLCapability.GL_DEPTH_TEST);

            glBegin(GLDrawMode.GL_LINES);
                //  Draw X
                glColor3f(0, 0, 0);
                glVertex2f(pX.x - letterWidth, pX.y - letterHeight);
                glVertex2f(pX.x + letterWidth, pX.y + letterHeight);
                glVertex2f(pX.x - letterWidth, pX.y + letterHeight);
                glVertex2f(pX.x + letterWidth, pX.y - letterHeight);

                if (bSkeleton.IsBattleLocation)
                {
                    //  Draw Y
                    glColor3f(0, 0, 0);
                    glVertex2f(pY.x - letterWidth, pY.y - letterHeight);
                    glVertex2f(pY.x + letterWidth, pY.y + letterHeight);
                    glVertex2f(pY.x - letterWidth, pY.y + letterHeight);
                    glVertex2f(pY.x, pY.y);

                    //  Draw Z
                    glColor3f(0, 0, 0);
                    glVertex2f(pZ.x + letterWidth, pZ.y + letterHeight);
                    glVertex2f(pZ.x - letterWidth, pZ.y + letterHeight);

                    glVertex2f(pZ.x + letterWidth, pZ.y + letterHeight);
                    glVertex2f(pZ.x - letterWidth, pZ.y - letterHeight);

                    glVertex2f(pZ.x + letterWidth, pZ.y - letterHeight);
                    glVertex2f(pZ.x - letterWidth, pZ.y - letterHeight);
                }
                else 
                {
                    //  Draw Y
                    glColor3f(0, 0, 0);
                    glVertex2f(pZ.x - letterWidth, pZ.y - letterHeight);
                    glVertex2f(pZ.x + letterWidth, pZ.y + letterHeight);
                    glVertex2f(pZ.x - letterWidth, pZ.y + letterHeight);
                    glVertex2f(pZ.x, pZ.y);

                    //  Draw Z
                    glColor3f(0, 0, 0);
                    glVertex2f(pY.x + letterWidth, pY.y + letterHeight);
                    glVertex2f(pY.x - letterWidth, pY.y + letterHeight);

                    glVertex2f(pY.x + letterWidth, pY.y + letterHeight);
                    glVertex2f(pY.x - letterWidth, pY.y - letterHeight);

                    glVertex2f(pY.x + letterWidth, pY.y - letterHeight);
                    glVertex2f(pY.x - letterWidth, pY.y - letterHeight);
                }
            glEnd();

            glEnable(GLCapability.GL_DEPTH_TEST);
        }




        //  ---------------------------------------------------------------------------------------------------
        //  ======================================= PEDITOR DRAW  =============================================
        //  ---------------------------------------------------------------------------------------------------
        public static void DrawPModelPolys(PModel Model)
        {
            int iGroupIdx, iPolyIdx, iVertIdx;

            glShadeModel(GLShadingModel.GL_FLAT);

            glPolygonMode(GLFace.GL_FRONT, GLPolygon.GL_LINE);
            glPolygonMode(GLFace.GL_BACK, GLPolygon.GL_FILL);
            glEnable(GLCapability.GL_COLOR_MATERIAL);

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                if (!Model.Groups[iGroupIdx].HiddenQ)
                {

                    // We will apply Group update values for Reposition/Resize/Rotate.
                    double[] rot_mat = new double[16];

                    glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
                    glPushMatrix();

                    glTranslatef(Model.Groups[iGroupIdx].repGroupX,
                                 Model.Groups[iGroupIdx].repGroupY,
                                 Model.Groups[iGroupIdx].repGroupZ);

                    BuildRotationMatrixWithQuaternionsXYZ(Model.Groups[iGroupIdx].rotGroupAlpha,
                                                          Model.Groups[iGroupIdx].rotGroupBeta,
                                                          Model.Groups[iGroupIdx].rotGroupGamma,
                                                          ref rot_mat);

                    glMultMatrixd(rot_mat);
                    glScalef(Model.Groups[iGroupIdx].rszGroupX,
                             Model.Groups[iGroupIdx].rszGroupY,
                             Model.Groups[iGroupIdx].rszGroupZ);

                    for (iPolyIdx = Model.Groups[iGroupIdx].offsetPoly;
                         iPolyIdx < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly;
                         iPolyIdx++)
                    {
                        glColor4f(Model.Pcolors[iPolyIdx].R / 255.0f, 
                                  Model.Pcolors[iPolyIdx].G / 255.0f, 
                                  Model.Pcolors[iPolyIdx].B / 255.0f, 
                                  Model.Pcolors[iPolyIdx].A / 255.0f);

                        glColorMaterial(GLFace.GL_FRONT_AND_BACK, GLMaterialParameter.GL_AMBIENT_AND_DIFFUSE);

                        glBegin(GLDrawMode.GL_TRIANGLES);
                        for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                        {
                            //glNormal3f(Model.Normals[Model.Polys[iPolyIdx].Normals[iVertIdx]].x,
                            //           Model.Normals[Model.Polys[iPolyIdx].Normals[iVertIdx]].y,
                            //           Model.Normals[Model.Polys[iPolyIdx].Normals[iVertIdx]].z);

                            glNormal3f(Model.Normals[Model.NormalIndex[Model.Polys[iPolyIdx].Verts[iVertIdx] +
                                                                       Model.Groups[iGroupIdx].offsetVert]].x,
                                       Model.Normals[Model.NormalIndex[Model.Polys[iPolyIdx].Verts[iVertIdx] +
                                                                       Model.Groups[iGroupIdx].offsetVert]].y,
                                       Model.Normals[Model.NormalIndex[Model.Polys[iPolyIdx].Verts[iVertIdx] +
                                                                       Model.Groups[iGroupIdx].offsetVert]].z);

                            glVertex3f(Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + 
                                                   Model.Groups[iGroupIdx].offsetVert].x,
                                       Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + 
                                                   Model.Groups[iGroupIdx].offsetVert].y,
                                       Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + 
                                                   Model.Groups[iGroupIdx].offsetVert].z);
                        }
                        glEnd();
                    }

                    glPopMatrix();
                }

                // Let's try here to render the normals
                if (bShowVertexNormals || bShowFaceNormals)
                    ShowNormals(Model.Groups[iGroupIdx], Model.Polys, Model.Verts, 
                                Model.Normals, Model.NormalIndex);
            }

            //  glPopMatrix();  -- Commented in KimeraVB6
        }

        public static void DrawPModelMesh(PModel Model)
        {
            int iGroupIdx, iPolyIdx, iVertIdx;

            // -- Commented in KimeraVB6
            //  glMatrixMode GL_MODELVIEW
            //  glPushMatrix
            //  With obj
            //      glScalef .ResizeX, .ResizeY, .ResizeZ
            //      glRotatef .RotateAlpha, 1, 0, 0
            //      glRotatef .RotateBeta, 0, 1, 0
            //      glRotatef .RotateGamma, 0, 0, 1
            //      glTranslatef .RepositionX, .RepositionY, .RepositionZ
            //  End With

            glPolygonMode(GLFace.GL_FRONT, GLPolygon.GL_LINE);
            glPolygonMode(GLFace.GL_BACK, GLPolygon.GL_LINE);
            glColor3f(0, 0, 0);

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {

                if (!Model.Groups[iGroupIdx].HiddenQ)
                {

                    // We will apply Group update values for Reposition/Resize/Rotate.
                    double[] rot_mat = new double[16];

                    glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
                    glPushMatrix();

                    glTranslatef(Model.Groups[iGroupIdx].repGroupX,
                                 Model.Groups[iGroupIdx].repGroupY,
                                 Model.Groups[iGroupIdx].repGroupZ);

                    BuildRotationMatrixWithQuaternionsXYZ(Model.Groups[iGroupIdx].rotGroupAlpha,
                                                          Model.Groups[iGroupIdx].rotGroupBeta,
                                                          Model.Groups[iGroupIdx].rotGroupGamma,
                                                          ref rot_mat);

                    glMultMatrixd(rot_mat);
                    glScalef(Model.Groups[iGroupIdx].rszGroupX,
                             Model.Groups[iGroupIdx].rszGroupY,
                             Model.Groups[iGroupIdx].rszGroupZ);

                    for (iPolyIdx = Model.Groups[iGroupIdx].offsetPoly; 
                         iPolyIdx < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly;
                         iPolyIdx++)
                    {
                        glBegin(GLDrawMode.GL_TRIANGLES);
                        for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                        {
                            glVertex3f(Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + 
                                                   Model.Groups[iGroupIdx].offsetVert].x,
                                       Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + 
                                                   Model.Groups[iGroupIdx].offsetVert].y,
                                       Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + 
                                                   Model.Groups[iGroupIdx].offsetVert].z);
                        }
                        glEnd();
                    }

                    glPopMatrix();
                }
            }

            //  glPopMatrix();      -- Commented in KimeraVB6
        }

        public static void KillPrecalculatedLighting(PModel Model, ref PairIB[] translationTableVertex)
        {
            int iVColorIdx;

            for (iVColorIdx = 0; iVColorIdx < Model.Header.numVerts; iVColorIdx++)
            {
                translationTableVertex[iVColorIdx].B = 1;
            }
        }

        public static void DrawPModelEditor(bool bEnableLighting, PictureBox pbIn)
        {

            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            glViewport(0, 0, pbIn.ClientRectangle.Width,
                             pbIn.ClientRectangle.Height);
            ClearPanel();

            //SetCameraPModel(EditedPModel,
            //                panXPE, panYPE, panZPE + DISTPE,
            //                alphaPE, betaPE, gammaPE,
            //                1, 1, 1);

            SetCameraPModel(EditedPModel,
                            panXPE, panYPE, panZPE + DISTPE,
                            alphaPE, betaPE, gammaPE,
                            rszXPE, rszYPE, rszZPE);

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPushMatrix();

            //ConcatenateCameraModelView(repXPE, repYPE, repZPE,
            //                           rotateAlpha, rotateBeta, rotateGamma,
            //                           rszXPE, rszYPE, rszZPE);

            if (bEnableLighting)
            {
                float modelDiameterNormalized;

                glDisable(GLCapability.GL_LIGHT0);
                glDisable(GLCapability.GL_LIGHT1);
                glDisable(GLCapability.GL_LIGHT2);
                glDisable(GLCapability.GL_LIGHT3);

                ComputePModelBoundingBox(EditedPModel, ref p_min, ref p_max);
                modelDiameterNormalized = (-2 * ComputeSceneRadius(p_min, p_max)) / FrmPEditor.LIGHT_STEPS;

                SetLighting(GLCapability.GL_LIGHT0, modelDiameterNormalized * FrmPEditor.iLightX,
                                                    modelDiameterNormalized * FrmPEditor.iLightY,
                                                    modelDiameterNormalized * FrmPEditor.iLightZ,
                                                    1, 1, 1, false);
            }
            else glDisable(GLCapability.GL_LIGHTING);

            SetDefaultOGLRenderState();

            switch (drawMode)
            {
                case K_MESH:
                    DrawPModelMesh(EditedPModel);
                    break;

                case K_PCOLORS:
                    glEnable(GLCapability.GL_POLYGON_OFFSET_FILL);
                    glPolygonOffset(1, 1);
                    DrawPModelPolys(EditedPModel);
                    glDisable(GLCapability.GL_POLYGON_OFFSET_FILL);

                    DrawPModelMesh(EditedPModel);
                    break;

                case K_VCOLORS:
                    DrawPModel(ref EditedPModel, ref tex_ids, true);
                    break;
            }            
        }

        public static void DrawAxesPE(PictureBox pbIn)
        {
            float letterWidth, letterHeight;
            float max_x, max_y, max_z;

            Point3D pX = new Point3D();
            Point3D pY = new Point3D();
            Point3D pZ = new Point3D();

            Point3D p_max = new Point3D();
            Point3D p_min = new Point3D();

            glDisable(GLCapability.GL_LIGHTING);
            ComputePModelBoundingBox(EditedPModel, ref p_min, ref p_max);

            max_x = Math.Abs(p_min.x) > Math.Abs(p_max.x) ? p_min.x : p_max.x;
            max_y = Math.Abs(p_min.y) > Math.Abs(p_max.y) ? p_min.y : p_max.y;
            max_z = Math.Abs(p_min.z) > Math.Abs(p_max.z) ? p_min.z : p_max.z;

            glBegin(GLDrawMode.GL_LINES);
                glColor3f(1, 0, 0);
                glVertex3f(0, 0, 0);
                glVertex3f(2 * max_x, 0, 0);

                glColor3f(0, 1, 0);
                glVertex3f(0, 0, 0);
                glVertex3f(0, 2 * max_y, 0);

                glColor3f(0, 0, 1);
                glVertex3f(0, 0, 0);
                glVertex3f(0, 0, 2 * max_z);
            glEnd();

            //  Get projected end of the X axis
            pX.x = 2 * max_x;
            pX.y = 0;
            pX.z = 0;
            pX = GetProjectedCoords(pX);

            //  Get projected end of the Y axis
            pY.x = 0;
            pY.y = 2 * max_y;
            pY.z = 0;
            pY = GetProjectedCoords(pY);

            //  Get projected end of the Z axis
            pZ.x = 0;
            pZ.y = 0;
            pZ.z = 2 * max_z;
            pZ = GetProjectedCoords(pZ);


            //  Set 2D mode to draw letters
            glMatrixMode(GLMatrixModeList.GL_PROJECTION);
            glLoadIdentity();
            gluOrtho2D(0, pbIn.ClientRectangle.Width, 0, pbIn.ClientRectangle.Height);
            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glLoadIdentity();

            letterWidth = LETTER_SIZE;
            letterHeight = (float)(LETTER_SIZE * 1.5);
            glDisable(GLCapability.GL_DEPTH_TEST);

            glBegin(GLDrawMode.GL_LINES);
                //  Draw X
                glColor3f(0, 0, 0);
                glVertex2f(pX.x - letterWidth, pX.y - letterHeight);
                glVertex2f(pX.x + letterWidth, pX.y + letterHeight);
                glVertex2f(pX.x - letterWidth, pX.y + letterHeight);
                glVertex2f(pX.x + letterWidth, pX.y - letterHeight);

                //  Draw Y
                glColor3f(0, 0, 0);
                glVertex2f(pY.x - letterWidth, pY.y - letterHeight);
                glVertex2f(pY.x + letterWidth, pY.y + letterHeight);
                glVertex2f(pY.x - letterWidth, pY.y + letterHeight);
                glVertex2f(pY.x, pY.y);

                //  Draw Z
                glColor3f(0, 0, 0);
                glVertex2f(pZ.x + letterWidth, pZ.y + letterHeight);
                glVertex2f(pZ.x - letterWidth, pZ.y + letterHeight);

                glVertex2f(pZ.x + letterWidth, pZ.y + letterHeight);
                glVertex2f(pZ.x - letterWidth, pZ.y - letterHeight);

                glVertex2f(pZ.x + letterWidth, pZ.y - letterHeight);
                glVertex2f(pZ.x - letterWidth, pZ.y - letterHeight);
            glEnd();

            glEnable(GLCapability.GL_DEPTH_TEST);
        }

        public static int GetEqualGroupVertices(PModel Model, int iActualVertIdx, ref List<int> lstVerts)
        {
            int iGroupIdx, iVertIdx;
            Point3D tmpUP3DVert;

            tmpUP3DVert = Model.Verts[iActualVertIdx];
            iGroupIdx = GetVertexGroup(Model, iActualVertIdx);

            for (iVertIdx = Model.Groups[iGroupIdx].offsetVert; 
                 iVertIdx < Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert;
                 iVertIdx++)
            {
                if (ComparePoints3D(Model.Verts[iVertIdx], tmpUP3DVert))
                {
                    lstVerts.Add(iVertIdx);
                    //  Debug.Print "Intended("; n_verts; ")"; Str$(vi)
                }
            }

            return lstVerts.Count;
        }

        public static int AddPaintVertex(ref PModel Model, int iGroupIdx, Point3D vPoint3D, Color vColor)
        {
            int iAddVertexResult = -1;
            //  -------- Warning! Causes the Normals to be inconsistent if lights are disabled.------------------
            //  --------------------------------Must call ComputeNormals ----------------------------------------
            int iVertIdx, iNormalIdx, iTexCoordIdx, baseVerts, baseNormals, baseTexCoords;
            int iNextGroup;

            Model.Header.numVerts++;
            Array.Resize(ref Model.Verts, Model.Header.numVerts);
            Array.Resize(ref Model.Vcolors, Model.Header.numVerts);

            if (Model.Groups[iGroupIdx].texFlag == 1)
            {
                Model.Header.numTexCs++;
                Array.Resize(ref Model.TexCoords, Model.Header.numTexCs);
            }

            Model.Header.numNormals = Model.Header.numVerts;
            Array.Resize(ref Model.Normals, Model.Header.numNormals);
            Model.Header.numNormIdx = Model.Header.numVerts;
            Array.Resize(ref Model.NormalIndex, Model.Header.numNormIdx);
            Model.NormalIndex[Model.Header.numNormIdx - 1] = Model.Header.numNormIdx - 1;

            iNextGroup = GetNextGroup(Model, iGroupIdx);
            //if (iGroupIdx < Model.Header.numGroups - 1)
            if (iNextGroup != -1)
            {
                //baseVerts = Model.Groups[iGroupIdx + 1].offsetVert;
                baseVerts = Model.Groups[iNextGroup].offsetVert;

                for (iVertIdx = Model.Header.numVerts - 1; iVertIdx >= baseVerts; iVertIdx--)
                {
                    Model.Verts[iVertIdx] = Model.Verts[iVertIdx - 1];
                    Model.Vcolors[iVertIdx] = Model.Vcolors[iVertIdx - 1];
                }

                //if (glIsEnabled(GLCapability.GL_LIGHTING))
                //{
                if (Model.Groups[iGroupIdx].texFlag == 1)
                    {
                        //baseNormals = Model.Groups[iGroupIdx + 1].offsetVert;
                        baseNormals = Model.Groups[iNextGroup].offsetVert;

                        for (iNormalIdx = Model.Header.numNormals - 1; iNormalIdx >= baseNormals; iNormalIdx--)
                        {
                            Model.Normals[iNormalIdx] = Model.Normals[iNormalIdx - 1];
                        }
                    }
                //}

                if (Model.Groups[iGroupIdx].texFlag == 1)
                {
                    baseTexCoords = Model.Groups[iGroupIdx].offsetTex + Model.Groups[iGroupIdx].numVert;

                    for (iTexCoordIdx = Model.Header.numTexCs - 1; iTexCoordIdx >= baseTexCoords; iTexCoordIdx--)
                    {
                        Model.TexCoords[iTexCoordIdx] = Model.TexCoords[iTexCoordIdx - 1];
                    }
                }

                while (iNextGroup != -1)
                {
                    Model.Groups[iNextGroup].offsetVert++;

                    if ((Model.Groups[iGroupIdx].texFlag == 1) && (Model.Groups[iNextGroup].texFlag == 1))
                    {
                        Model.Groups[iNextGroup].offsetTex++;
                    }

                    iNextGroup = GetNextGroup(Model, iNextGroup);
                }

            }

            if (iGroupIdx < Model.Header.numGroups)
            {
                Model.Verts[Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert] = vPoint3D;
                Model.Vcolors[Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert] = vColor;
                iAddVertexResult = Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert;
                Model.Groups[iGroupIdx].numVert++;
            }

            return iAddVertexResult;
        }

        public static int PaintVertex(ref PModel Model, int iGroupIdx, int iPolyIdx, int iVertIdx,
                                      byte bR, byte bG, byte bB, bool bTextured)
        {

            int iPaintVertexResult = -1;

            Point3D tmpVert;
            Color tmpColor;

            if (Model.Vcolors[Model.Polys[iPolyIdx].Verts[iVertIdx] + 
                              Model.Groups[iGroupIdx].offsetVert].R == bR &&
                Model.Vcolors[Model.Polys[iPolyIdx].Verts[iVertIdx] +
                              Model.Groups[iGroupIdx].offsetVert].G == bG &&
                Model.Vcolors[Model.Polys[iPolyIdx].Verts[iVertIdx] +
                              Model.Groups[iGroupIdx].offsetVert].B == bB)
                        iPaintVertexResult = iVertIdx;

            if (iPaintVertexResult == -1)
            {

                tmpVert.x = Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] +
                                        Model.Groups[iGroupIdx].offsetVert].x;
                tmpVert.y = Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] +
                                        Model.Groups[iGroupIdx].offsetVert].y;
                tmpVert.z = Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] +
                                        Model.Groups[iGroupIdx].offsetVert].z;

                tmpColor = Color.FromArgb(255, bR, bG, bB);

                iPaintVertexResult = AddPaintVertex(ref Model, iGroupIdx, tmpVert, tmpColor) - Model.Groups[iGroupIdx].offsetVert;

                Model.Normals[Model.NormalIndex[iPaintVertexResult]] =
                    new Point3D(Model.Normals[Model.NormalIndex[Model.Polys[iPolyIdx].Verts[iVertIdx]]+
                                                                Model.Groups[iGroupIdx].offsetVert].x,
                                Model.Normals[Model.NormalIndex[Model.Polys[iPolyIdx].Verts[iVertIdx]] +
                                                                Model.Groups[iGroupIdx].offsetVert].y,
                                Model.Normals[Model.NormalIndex[Model.Polys[iPolyIdx].Verts[iVertIdx]] +
                                                                Model.Groups[iGroupIdx].offsetVert].z);

                if (bTextured)
                {
                    Model.TexCoords[Model.Groups[iGroupIdx].offsetTex + iPaintVertexResult] = 
                        new Point2D(Model.TexCoords[iVertIdx + Model.Groups[iGroupIdx].offsetTex].x,
                                    Model.TexCoords[iVertIdx + Model.Groups[iGroupIdx].offsetTex].y);
                }

                //    // -- Commented in KimeraVB6
                //    // Model.Normals[iPaintVertexResult + Model.Groups[iGroupIdx].offsetVert] = Model.Normals[iVertIdx];
                //}
                //else
                //{
                //    // Debug.Print "Substituido por: " + Str$(PaintVertex);
                //}
            }
            
            return iPaintVertexResult;
        }

        //  ------------------------------WARNINGS!----------------------------------
        //  -------*Can causes the Normals to be inconsistent (call ComputeNormals).--
        //  -------*Can causes inconsistent edges (call ComputeEdges).----------------
        //  -------*Can cause unused vertices (call KillUnusedVertices).--------------
        public static void PaintPolygon(ref PModel Model, int iPolyIdx, byte bR, byte bG, byte bB)
        {
            int iGroupIdx, iVertIdx;

            iGroupIdx = GetPolygonGroup(Model, iPolyIdx);

            for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
            {
                Model.Polys[iPolyIdx].Verts[iVertIdx] = 
                    (ushort)PaintVertex(ref Model, iGroupIdx, iPolyIdx, iVertIdx, bR, bG, bB,
                                        Model.Groups[iGroupIdx].texFlag == 1);
                //  'Debug.Print "Vert(:", .Verts(vi), ",", Group, ")", obj.Verts(.Verts(vi) + obj.Groups(Group).offVert).x, obj.Verts(.Verts(vi) + obj.Groups(Group).offVert).y, obj.Verts(.Verts(vi) + obj.Groups(Group).offVert).z
            }

            Model.Pcolors[iPolyIdx] = Color.FromArgb(Model.Pcolors[iPolyIdx].A, bR, bG, bB);
        }

        public static Color ComputePolyColor(PModel Model, int iPolyIdx)
        {
            int iGroupIdx, iVertIdx;
            int iTmpA = 0, iTmpR = 0, iTmpG = 0, iTmpB = 0;

            iGroupIdx = GetPolygonGroup(Model, iPolyIdx);

            for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
            {

                iTmpA += Model.Vcolors[Model.Polys[iPolyIdx].Verts[iVertIdx] +
                                       Model.Groups[iGroupIdx].offsetVert].A;
                iTmpR += Model.Vcolors[Model.Polys[iPolyIdx].Verts[iVertIdx] +
                                       Model.Groups[iGroupIdx].offsetVert].R;
                iTmpG += Model.Vcolors[Model.Polys[iPolyIdx].Verts[iVertIdx] +
                                       Model.Groups[iGroupIdx].offsetVert].G;
                iTmpB += Model.Vcolors[Model.Polys[iPolyIdx].Verts[iVertIdx] +
                                       Model.Groups[iGroupIdx].offsetVert].B;
            }

            return Color.FromArgb(iTmpA / 3, iTmpR / 3, iTmpG / 3, iTmpB / 3);
        }




    }
}

