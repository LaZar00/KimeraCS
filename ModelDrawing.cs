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

    using static frmSkeletonEditor;
    using static frmPEditor;

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

    public class ModelDrawing
    {

        public static uint[] tex_ids = new uint[1];


        //  ---------------------------------------------------------------------------------------------------
        //  ================================== GENERIC FIELD/BATTLE DRAW  =====================================
        //  ---------------------------------------------------------------------------------------------------
        public static void DrawGroup(ref PGroup Group, ref PPolygon[] Polys, ref Point3D[] Verts,
                                     ref Color[] Vcolors, ref Point3D[] Normals, ref Point2D[] TexCoords, 
                                     ref PHundret Hundret, bool HideHiddenQ)
        {

            if (Group.HiddenQ && HideHiddenQ) return;

            long pi = 0, vi = 0;
            bool texEnabled;
            float x, y, z;

            texEnabled = glIsEnabled(glCapability.GL_TEXTURE_2D);

            try
            {
                glBegin(glDrawMode.GL_TRIANGLES);
                glColorMaterial(glFace.GL_FRONT_AND_BACK, glMaterialParameter.GL_AMBIENT_AND_DIFFUSE);

                for (pi = Group.offsetPoly; pi < Group.offsetPoly + Group.numPoly; pi++)
                {
                    for (vi = 0; vi < 3; vi++)
                    {
                        if (Hundret.blend_mode == 0 && !texEnabled)
                        {
                            glColor4f(Vcolors[Polys[pi].Verts[vi] + Group.offsetVert].R / 255.0f,
                                      Vcolors[Polys[pi].Verts[vi] + Group.offsetVert].G / 255.0f,
                                      Vcolors[Polys[pi].Verts[vi] + Group.offsetVert].B / 255.0f,
                                      0.5f);
                        }
                        else
                        {
                            glColor4f(Vcolors[Polys[pi].Verts[vi] + Group.offsetVert].R / 255.0f,
                                      Vcolors[Polys[pi].Verts[vi] + Group.offsetVert].G / 255.0f,
                                      Vcolors[Polys[pi].Verts[vi] + Group.offsetVert].B / 255.0f,
                                      1f);
                        }

                        if (Normals.Length > 0)
                            glNormal3f(Normals[Polys[pi].Normals[vi]].x,
                                       Normals[Polys[pi].Normals[vi]].y,
                                       Normals[Polys[pi].Normals[vi]].z);

                        if (Group.texFlag == 1)
                        {
                            x = TexCoords[Group.offsetTex + Polys[pi].Verts[vi]].x;
                            y = TexCoords[Group.offsetTex + Polys[pi].Verts[vi]].y;
                            glTexCoord2f(x, y);
                        }

                        x = Verts[Polys[pi].Verts[vi] + Group.offsetVert].x;
                        y = Verts[Polys[pi].Verts[vi] + Group.offsetVert].y;
                        z = Verts[Polys[pi].Verts[vi] + Group.offsetVert].z;
                        glVertex3f(x, y, z);
                    }
                }

                glEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in DrawGroup procedure.\nPolygon (pi): " + pi.ToString() +
                                "\nVertex (vi): " + vi.ToString() +
                                "\noffsetVertex: " + Group.offsetVert.ToString() +
                                "\noffsetPolygon: " + Group.offsetPoly.ToString(), 
                                "Exception error", MessageBoxButtons.OK);
            }
        }

        public static void DrawPModel(ref PModel Model, ref uint[] tex_ids, bool HideHiddenGroupsQ)
        {
            int gi;
            bool set_v_textured, v_textured, set_v_linearfilter, v_linearfilter, texEnabled;

            texEnabled = glIsEnabled(glCapability.GL_TEXTURE_2D);

            glShadeModel(glShadingModel.GL_SMOOTH);
            glEnable(glCapability.GL_COLOR_MATERIAL);

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                //  Set the render states acording to the hundrets information
                //  V_WIREFRAME
                if (!((Model.Hundrets[gi].field_8 & 0x1) == 0))
                {
                    if (!((Model.Hundrets[gi].field_C & 0x1) == 0))
                    {
                        glPolygonMode(glFace.GL_FRONT, glPolygon.GL_LINE);
                        glPolygonMode(glFace.GL_BACK, glPolygon.GL_LINE);
                    }
                    else
                    {
                        glPolygonMode(glFace.GL_FRONT, glPolygon.GL_FILL);
                        glPolygonMode(glFace.GL_BACK, glPolygon.GL_FILL);
                    }
                }

                //  V_TEXTRED
                set_v_textured = !((Model.Hundrets[gi].field_8 & 0x2) == 0);
                v_textured = !((Model.Hundrets[gi].field_C & 0x2) == 0);

                //  V_LINEARFILTER
                set_v_linearfilter = !((Model.Hundrets[gi].field_8 & 0x4) == 0);
                v_linearfilter = !((Model.Hundrets[gi].field_C & 0x4) == 0);

                //  V_NOCULL
                if (!((Model.Hundrets[gi].field_8 & 0x4000) == 0))
                {
                    if (!((Model.Hundrets[gi].field_C & 0x4000) == 0))
                    {
                        glDisable(glCapability.GL_CULL_FACE);
                    }
                    else
                    {
                        glEnable(glCapability.GL_CULL_FACE);
                    }
                }

                //  V_CULLFACE
                if (!((Model.Hundrets[gi].field_8 & 0x2000) == 0))
                {
                    if (!((Model.Hundrets[gi].field_C & 0x2000) == 0))
                    {
                        glCullFace(glFace.GL_FRONT);
                    }
                    else
                    {
                        glCullFace(glFace.GL_BACK);
                    }
                }

                //  Now let's set the blending state
                switch(Model.Hundrets[gi].blend_mode)
                {
                    case 0:
                        //  Average
                        glEnable(glCapability.GL_BLEND);
                        GLExt.glBlendEquation(glBlendEquationMode.GL_FUNC_ADD);
                        
                        if ((texEnabled && !(set_v_textured && !v_textured)) || (set_v_textured && v_textured))
                        {
                            glBlendFunc(glBlendFuncFactor.GL_SRC_ALPHA, glBlendFuncFactor.GL_ONE_MINUS_SRC_ALPHA);
                        }
                        else
                        {
                            glBlendFunc(glBlendFuncFactor.GL_SRC_ALPHA, glBlendFuncFactor.GL_SRC_ALPHA);
                        }

                        break;

                    case 1:
                        //  Additive
                        glEnable(glCapability.GL_BLEND);
                        GLExt.glBlendEquation(glBlendEquationMode.GL_FUNC_ADD);
                        glBlendFunc(glBlendFuncFactor.GL_ONE, glBlendFuncFactor.GL_ONE);
                        break;

                    case 2:
                        //  Subtractive
                        glEnable(glCapability.GL_BLEND);
                        GLExt.glBlendEquation(glBlendEquationMode.GL_FUNC_REVERSE_SUBTRACT);
                        glBlendFunc(glBlendFuncFactor.GL_ONE, glBlendFuncFactor.GL_ONE);
                        break;

                    case 3:
                        //  Unknown, let's disable blending
                        glDisable(glCapability.GL_BLEND);
                        break;

                    case 4:
                        //  No blending
                        glDisable(glCapability.GL_BLEND);

                        if (!((Model.Hundrets[gi].field_8 & 0x400) == 0))
                        {
                            if (!((Model.Hundrets[gi].field_C & 0x400) == 0))
                            {
                                glEnable(glCapability.GL_BLEND);
                                GLExt.glBlendEquation(glBlendEquationMode.GL_FUNC_ADD);
                                glBlendFunc(glBlendFuncFactor.GL_SRC_ALPHA, glBlendFuncFactor.GL_ONE_MINUS_SRC_ALPHA);
                            }
                        }

                        break;

                }

                if (Model.Groups[gi].texFlag == 1 && tex_ids.Length > 0 && v_textured)
                {
                    if(Model.Groups[gi].texID < tex_ids.Length)
                    {
                        if (glIsTexture(tex_ids[Model.Groups[gi].texID]))
                        {
                            if (set_v_textured)
                            {
                                if (v_textured) glEnable(glCapability.GL_TEXTURE_2D);
                                else glDisable(glCapability.GL_TEXTURE_2D);
                            }

                            glBindTexture(glTextureTarget.GL_TEXTURE_2D, tex_ids[Model.Groups[gi].texID]);

                            if (set_v_linearfilter)
                            {
                                if (v_linearfilter)
                                {
                                    glTexParameterf(glTextureTarget.GL_TEXTURE_2D, glTextureParameter.GL_TEXTURE_MAG_FILTER,
                                                             (float)glTextureMagFilter.GL_LINEAR);

                                    glTexParameterf(glTextureTarget.GL_TEXTURE_2D, glTextureParameter.GL_TEXTURE_MIN_FILTER,
                                                             (float)glTextureMagFilter.GL_LINEAR);
                                }
                                else
                                {
                                    glTexParameterf(glTextureTarget.GL_TEXTURE_2D, glTextureParameter.GL_TEXTURE_MAG_FILTER,
                                                             (float)glTextureMagFilter.GL_NEAREST);

                                    glTexParameterf(glTextureTarget.GL_TEXTURE_2D, glTextureParameter.GL_TEXTURE_MIN_FILTER,
                                                             (float)glTextureMagFilter.GL_NEAREST);
                                }
                            }
                        }
                    }
                }

                DrawGroup(ref Model.Groups[gi], ref Model.Polys, ref Model.Verts, ref Model.Vcolors,
                          ref Model.Normals, ref Model.TexCoords, ref Model.Hundrets[gi], HideHiddenGroupsQ);
                glDisable(glCapability.GL_TEXTURE_2D);
            }
            //  glPopMatrix();
        }

        public static void DrawGroupDList(ref PGroup Group)
        {
            glCallList((uint)Group.DListNum);
        }

        public static void DrawPModelDLists(ref PModel Model, ref uint[] tex_ids)
        {
            int gi;

            //glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            //glPushMatrix();

            //glScalef(Model.resizeX, Model.resizeY, Model.resizeZ);
            //glRotatef(Model.rotateAlpha, 1, 0, 0);
            //glRotatef(Model.rotateBeta, 0, 1, 0);
            //glRotatef(Model.rotateGamma, 0, 0, 1);
            //glTranslatef(Model.repositionX, Model.repositionY, Model.repositionZ);

            glShadeModel(glShadingModel.GL_SMOOTH);
            glPolygonMode(glFace.GL_FRONT, glPolygon.GL_FILL);
            glPolygonMode(glFace.GL_BACK, glPolygon.GL_FILL);
            glEnable(glCapability.GL_COLOR_MATERIAL);

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                if (Model.Groups[gi].texFlag==1 && tex_ids[0] > 0)
                {
                    if (Model.Groups[gi].texID <= tex_ids.Length)
                    {
                        if (glIsTexture(tex_ids[Model.Groups[gi].texID]))
                        {
                            glEnable(glCapability.GL_TEXTURE_2D);

                            glBindTexture(glTextureTarget.GL_TEXTURE_2D, tex_ids[Model.Groups[gi].texID]);
                            glTexParameterf(glTextureTarget.GL_TEXTURE_2D, glTextureParameter.GL_TEXTURE_MAG_FILTER, (float)glTextureMagFilter.GL_LINEAR);
                            glTexParameterf(glTextureTarget.GL_TEXTURE_2D, glTextureParameter.GL_TEXTURE_MIN_FILTER, (float)glTextureMagFilter.GL_LINEAR);
                        }
                    }                
                }

                DrawGroupDList(ref Model.Groups[gi]);
                glDisable(glCapability.GL_TEXTURE_2D);
            }
        }

        public static void DrawPModelBoundingBox(PModel Model)
        {
            glBegin(glDrawMode.GL_LINES);
            glDisable(glCapability.GL_DEPTH_TEST);

            DrawBox(Model.BoundingBox.max_x, Model.BoundingBox.max_y, Model.BoundingBox.max_z,
                    Model.BoundingBox.min_x, Model.BoundingBox.min_y, Model.BoundingBox.min_z,
                    1, 1, 0);

            glEnable(glCapability.GL_DEPTH_TEST);
            glEnd();
            // glPopMatrix();
        }

 

        //  ---------------------------------------------------------------------------------------------------
        //  ======================================== FIELD DRAW  ==============================================
        //  ---------------------------------------------------------------------------------------------------
        public static int MoveToFieldBone(FieldSkeleton fSkeleton, FieldFrame fFrame, int b_index)
        {
            int bi, jsp;
            string[] joint_stack = new string[fSkeleton.bones.Count];

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);

            jsp = 0;
            joint_stack[jsp] = fSkeleton.bones[0].joint_f;

            for (bi = 0; bi < b_index; bi++)
            {
                while (!(fSkeleton.bones[bi].joint_f == joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }
                glPushMatrix();

                glRotated(fFrame.rotations[bi].beta, 0, 1, 0);
                glRotated(fFrame.rotations[bi].alpha, 1, 0, 0);
                glRotated(fFrame.rotations[bi].gamma, 0, 0, 1);

                glTranslated(0, 0, -fSkeleton.bones[bi].len);

                jsp++;
                joint_stack[jsp] = fSkeleton.bones[bi].joint_i;
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

            glBegin(glDrawMode.GL_LINES);

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

            glDisable(glCapability.GL_DEPTH_TEST);
            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
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

            glEnable(glCapability.GL_DEPTH_TEST);
        }

        public static void DrawFieldBoneBoundingBox(FieldBone bone)
        {
            int ri;

            float max_x, max_y, max_z;
            float min_x, min_y, min_z;

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            glScalef(bone.resizeX, bone.resizeY, bone.resizeZ);

            if (bone.nResources == 0)
            {
                glDisable(glCapability.GL_DEPTH_TEST);
                
                glColor3f(1, 0, 0);
                glBegin(glDrawMode.GL_LINES);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, 0, -(float)bone.len);
                glEnd();

                glEnable(glCapability.GL_DEPTH_TEST);
            }
            else
            {
                max_x = -(float)INFINITY_SINGLE;
                max_y = -(float)INFINITY_SINGLE;
                max_z = -(float)INFINITY_SINGLE;

                min_x = (float)INFINITY_SINGLE;
                min_y = (float)INFINITY_SINGLE;
                min_z = (float)INFINITY_SINGLE;

                for (ri = 0; ri < bone.nResources; ri++)
                {
                    if (max_x < bone.fRSDResources[ri].Model.BoundingBox.max_x) max_x = bone.fRSDResources[ri].Model.BoundingBox.max_x;
                    if (max_y < bone.fRSDResources[ri].Model.BoundingBox.max_y) max_y = bone.fRSDResources[ri].Model.BoundingBox.max_y;
                    if (max_z < bone.fRSDResources[ri].Model.BoundingBox.max_z) max_z = bone.fRSDResources[ri].Model.BoundingBox.max_z;

                    if (min_x > bone.fRSDResources[ri].Model.BoundingBox.min_x) min_x = bone.fRSDResources[ri].Model.BoundingBox.min_x;
                    if (min_y > bone.fRSDResources[ri].Model.BoundingBox.min_x) min_y = bone.fRSDResources[ri].Model.BoundingBox.min_y;
                    if (min_z > bone.fRSDResources[ri].Model.BoundingBox.min_x) min_z = bone.fRSDResources[ri].Model.BoundingBox.min_z;
                }

                glDisable(glCapability.GL_DEPTH_TEST);
                DrawBox(max_x, max_y, max_z, min_x, min_y, min_z, 1, 0, 0);
                glEnable(glCapability.GL_DEPTH_TEST);
            }
        }

        public static void DrawFieldSkeletonBones(FieldSkeleton fSkeleton, FieldFrame fFrame)
        {
            int bi, jsp;
            string[] joint_stack = new string[fSkeleton.bones.Count + 1];
            double[] rot_mat = new double[16];

            jsp = 0;

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            glPushMatrix();

            glTranslated(fFrame.rootTranslationX, 0, 0);
            glTranslated(0, -fFrame.rootTranslationY, 0);
            glTranslated(0, 0, fFrame.rootTranslationZ);

            BuildRotationMatrixWithQuaternions(fFrame.rootRotationAlpha, fFrame.rootRotationBeta, fFrame.rootRotationGamma, ref rot_mat);

            glMultMatrixd(rot_mat);
            glPointSize(5f);

            joint_stack[jsp] = fSkeleton.bones[0].joint_f;

            //for (bi = 0; bi < fSkeleton.nBones; bi++)
            for (bi = 0; bi < fSkeleton.bones.Count; bi++)
            {
                while ((fSkeleton.bones[bi].joint_f != joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }

                glPushMatrix();

                // -- Commented in KimeraVB6
                //glRotated(fFrame.rotations[bi].beta, 0, 1, 0);
                //glRotated(fFrame.rotations[bi].alpha, 1, 0, 0);
                //glRotated(fFrame.rotations[bi].gamma, 0, 0, 1);
                BuildRotationMatrixWithQuaternions(fFrame.rotations[bi].alpha, fFrame.rotations[bi].beta, fFrame.rotations[bi].gamma, ref rot_mat);
                glMultMatrixd(rot_mat);

                glBegin(glDrawMode.GL_POINTS);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, 0, (float)-fSkeleton.bones[bi].len);
                glEnd();

                glBegin(glDrawMode.GL_LINES);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, 0, (float)-fSkeleton.bones[bi].len);
                glEnd();

                glTranslated(0, 0, -fSkeleton.bones[bi].len);

                jsp++;
                joint_stack[jsp] = fSkeleton.bones[bi].joint_i;
            }

            while (jsp > 0)
            {
                glPopMatrix();
                jsp--;
            }
            glPopMatrix();
        }

        public static void DrawRSDResource(FieldRSDResource fRSDResource, bool UseDLists)
        {
            int ti;
            uint[] tex_ids;
            double[] rot_mat = new double[16];

            tex_ids = new uint[fRSDResource.numTextures];

            for (ti = 0; ti < fRSDResource.numTextures; ti++)
            {
                tex_ids[ti] = fRSDResource.textures[ti].texID;
            }

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            glPushMatrix();

            glTranslatef(fRSDResource.Model.repositionX, fRSDResource.Model.repositionY, fRSDResource.Model.repositionZ);
            BuildMatrixFromQuaternion(fRSDResource.Model.rotationQuaternion, ref rot_mat);

            glMultMatrixd(rot_mat);

            glScalef(fRSDResource.Model.resizeX, fRSDResource.Model.resizeY, fRSDResource.Model.resizeZ);

            if (!UseDLists) DrawPModel(ref fRSDResource.Model, ref tex_ids, false); 
            else DrawPModelDLists(ref fRSDResource.Model, ref tex_ids);

            glPopMatrix();
        }

        public static void DrawFieldBone(FieldBone bone, bool UseDLists)
        {

            int ri;

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            glPushMatrix();

            glScalef(bone.resizeX, bone.resizeY, bone.resizeZ);

            for (ri = 0; ri < bone.nResources; ri++)
                DrawRSDResource(bone.fRSDResources[ri], UseDLists);

            glPopMatrix();
        }

        public static void DrawFieldSkeleton(FieldSkeleton fSkeleton, FieldFrame fFrame, bool UseDLists)
        {
            int bi;
            string[] joint_stack = new string[fSkeleton.bones.Count + 1];
            int jsp;
            double[] rot_mat = new double[16];

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);

            glPushMatrix();
            glTranslated(fFrame.rootTranslationX, 0, 0);
            glTranslated(0, -fFrame.rootTranslationY, 0);
            glTranslated(0, 0, fFrame.rootTranslationZ);

            BuildRotationMatrixWithQuaternions(fFrame.rootRotationAlpha, fFrame.rootRotationBeta, fFrame.rootRotationGamma, ref rot_mat);

            glMultMatrixd(rot_mat);

            jsp = 0;
            joint_stack[jsp] = fSkeleton.bones[0].joint_f;

            //for (bi = 0; bi < Skeleton.nBones; bi++)
            for (bi = 0; bi < fSkeleton.bones.Count; bi++)
            {
                while (!(fSkeleton.bones[bi].joint_f == joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }

                if (jsp == 0) SetDefaultOGLRenderState();

                glPushMatrix();

                BuildRotationMatrixWithQuaternions(fFrame.rotations[bi].alpha, fFrame.rotations[bi].beta, fFrame.rotations[bi].gamma, 
                                                         ref rot_mat);

                glMultMatrixd(rot_mat);

                DrawFieldBone(fSkeleton.bones[bi], UseDLists);

                glTranslated(0, 0, -fSkeleton.bones[bi].len);

                jsp++;
                joint_stack[jsp] = fSkeleton.bones[bi].joint_i;
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
            int bi, jsp, itmpbones;
            int[] joint_stack = new int[bSkeleton.nBones * 4];
            double[] rot_mat = new double[16];

            jsp = 0;
            joint_stack[jsp] = -1;

            if (bSkeleton.nBones > 1) itmpbones = 1;
            else itmpbones = 0;

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            glPushMatrix();
            glTranslated(bFrame.startX, bFrame.startY, bFrame.startZ);

            BuildRotationMatrixWithQuaternions(bFrame.bones[0].alpha, bFrame.bones[0].beta, bFrame.bones[0].gamma, ref rot_mat);
            glMultMatrixd(rot_mat);

            for (bi = 0; bi < boneIndex; bi++)
            {
                glPushName((uint)bi);

                while (!(bSkeleton.bones[bi].parentBone == joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }
                glPushMatrix();

                // -- Commented in KimeraVB6
                //  glRotated(bFrame.bones[bi + 1].beta, 0, 1, 0);
                //  glRotated(bFrame.bones[bi + 1].alpha, 1, 0, 0);
                //  glRotated(bFrame.bones[bi + 1].gamma, 0, 0, 1);

                BuildRotationMatrixWithQuaternions(bFrame.bones[bi + itmpbones].alpha, 
                                                       bFrame.bones[bi + itmpbones].beta, 
                                                       bFrame.bones[bi + itmpbones].gamma, ref rot_mat);
                glMultMatrixd(rot_mat);

                glTranslated(0, 0, bSkeleton.bones[bi].len);

                jsp++;
                joint_stack[jsp] = bi;

                glPopName();
            }

            while (!(bSkeleton.bones[bi].parentBone == joint_stack[jsp]) && jsp > 0)
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

        public static int MoveToBattleBoneEnd(BattleSkeleton bskeleton, BattleFrame bFrame, int boneIndex)
        {
            int iMoveToBattleBoneEndResult;

            iMoveToBattleBoneEndResult = MoveToBattleBone(bSkeleton, bFrame, boneIndex);
            glTranslated(0, 0, bSkeleton.bones[boneIndex].len);

            return iMoveToBattleBoneEndResult;
        }

        public static void DrawBattleBoneModelBoundingBox(BattleBone bBone, int partIndex)
        {
            double[] rot_mat = new double[16];

            glDisable(glCapability.GL_DEPTH_TEST);
            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            glScalef(bBone.resizeX, bBone.resizeY, bBone.resizeZ);

            glTranslatef(bBone.Models[partIndex].repositionX, bBone.Models[partIndex].repositionY, bBone.Models[partIndex].repositionZ);

            BuildMatrixFromQuaternion(bBone.Models[partIndex].rotationQuaternion, ref rot_mat);
            glMultMatrixd(rot_mat);

            glScalef(bBone.Models[partIndex].resizeX, bBone.Models[partIndex].resizeY, bBone.Models[partIndex].resizeZ);

            DrawBox(bBone.Models[partIndex].BoundingBox.max_x, bBone.Models[partIndex].BoundingBox.max_y, bBone.Models[partIndex].BoundingBox.max_z,
                    bBone.Models[partIndex].BoundingBox.min_x, bBone.Models[partIndex].BoundingBox.min_y, bBone.Models[partIndex].BoundingBox.min_z,
                    0, 1, 0);
            glEnable(glCapability.GL_DEPTH_TEST);
        }

        public static void DrawBattleBoneBoundingBox(BattleBone bBone)
        {
            double[] rot_mat = new double[16];

            glDisable(glCapability.GL_DEPTH_TEST);
            glMatrixMode(glMatrixModeList.GL_MODELVIEW);

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
                glEnable(glCapability.GL_DEPTH_TEST);
            }
            else
            {
                glColor3f(0, 1, 0);
                glBegin(glDrawMode.GL_LINES);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, 0, bBone.len);
                glEnd();
            }

            glEnable(glCapability.GL_DEPTH_TEST);
        }

        public static void DrawBattleWeaponBoundingBox(BattleSkeleton bSkeleton, BattleFrame wpFrame, int weaponIndex)
        {
            double[] rot_mat = new double[16];

            //if (weaponIndex > -1 && bSkeleton.nWeapons > 0)       // -- Commented in KimeraVB6
            if (bSkeleton.wpModels.Count > 0 && bAnimationsPack.WeaponAnimations.Count > 0)
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
            int bi, jsp, itmpbones;
            int[] joint_stack;
            double[] rot_mat = new double[16];

            if (bSkeleton.IsBattleLocation) return;

            joint_stack = new int[bSkeleton.nBones + 1];
            jsp = 0;
            joint_stack[jsp] = -1;

            if (bSkeleton.nBones > 1) itmpbones = 1;
            else itmpbones = 0;

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            glPointSize(5);
            glPushMatrix();
            glTranslated(bFrame.startX, bFrame.startY, bFrame.startZ);

            BuildRotationMatrixWithQuaternions(bFrame.bones[0].alpha, bFrame.bones[0].beta, bFrame.bones[0].gamma, ref rot_mat);
            glMultMatrixd(rot_mat);

            for (bi = 0; bi < bSkeleton.nBones; bi++)
            {
                while (!(bSkeleton.bones[bi].parentBone == joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }
                glPushMatrix();

                // -- Commented in KimeraVB6
                //glRotated(bFrame.bones[bi + 1].beta, 0, 1, 0);
                //glRotated(bFrame.bones[bi + 1].alpha, 1, 0, 0);
                //glRotated(bFrame.bones[bi + 1].gamma, 0, 0, 1);

                BuildRotationMatrixWithQuaternions(bFrame.bones[bi + itmpbones].alpha,
                                                   bFrame.bones[bi + itmpbones].beta,
                                                   bFrame.bones[bi + itmpbones].gamma,
                                                   ref rot_mat);
                glMultMatrixd(rot_mat);

                glBegin(glDrawMode.GL_POINTS);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, 0, bSkeleton.bones[bi].len);
                glEnd();

                glBegin(glDrawMode.GL_LINES);
                    glVertex3f(0, 0, 0);
                    glVertex3f(0, 0, bSkeleton.bones[bi].len);
                glEnd();

                glTranslated(0, 0, bSkeleton.bones[bi].len);

                jsp++;
                joint_stack[jsp] = bi;
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
            int mi;
            PModel tmpbModel = new PModel();

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            glPushMatrix();
            glScalef(bBone.resizeX, bBone.resizeY, bBone.resizeZ);

            if (bBone.hasModel > 0)
            {
                if (!bDListsEnable)
                {
                    for (mi = 0; mi < bBone.nModels; mi++)
                    {
                        glPushMatrix();
                        glTranslatef(bBone.Models[mi].repositionX, bBone.Models[mi].repositionY, bBone.Models[mi].repositionZ);

                        glRotated(bBone.Models[mi].rotateAlpha, 1, 0, 0);
                        glRotated(bBone.Models[mi].rotateBeta, 0, 1, 0);
                        glRotated(bBone.Models[mi].rotateGamma, 0, 0, 1);

                        glScalef(bBone.Models[mi].resizeX, bBone.Models[mi].resizeY, bBone.Models[mi].resizeZ);

                        tmpbModel = bBone.Models[mi];
                        DrawPModel(ref tmpbModel, ref texIDS, false);
                        bBone.Models[mi] = tmpbModel;

                        glPopMatrix();
                    }
                }
                else
                {
                    for (mi = 0; mi < bBone.nModels; mi++)
                    {
                        glPushMatrix();
                        glTranslatef(bBone.Models[mi].repositionX, bBone.Models[mi].repositionY, bBone.Models[mi].repositionZ);

                        glRotated(bBone.Models[mi].rotateAlpha, 1, 0, 0);
                        glRotated(bBone.Models[mi].rotateBeta, 0, 1, 0);
                        glRotated(bBone.Models[mi].rotateGamma, 0, 0, 1);

                        glScalef(bBone.Models[mi].resizeX, bBone.Models[mi].resizeY, bBone.Models[mi].resizeZ);

                        tmpbModel = bBone.Models[mi];
                        DrawPModelDLists(ref tmpbModel, ref texIDS);
                        bBone.Models[mi] = tmpbModel;

                        glPopMatrix();
                    }
                }
            }

            glPopMatrix();
        }

        public static void DrawBattleSkeleton(BattleSkeleton bSkeleton, BattleFrame bFrame, BattleFrame wpFrame,
                                              int weaponIndex, bool bDListsEnable)
        {
            int bi, jsp, itmpbones;
            int[] joint_stack = new int[bSkeleton.nBones + 1];
            double[] rot_mat = new double[16];

            jsp = 0;
            joint_stack[jsp] = -1;

            if (bSkeleton.nBones > 1) itmpbones = 1;
            else itmpbones = 0;

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            glPushMatrix();
            glTranslated(bFrame.startX, bFrame.startY, bFrame.startZ);

            // Debug.Print bFrame.bones[0].alpha; ", "; bFrame.bones[0].Beta; ", "; bFrame.bones[0].Gamma
            BuildRotationMatrixWithQuaternions(bFrame.bones[0].alpha, bFrame.bones[0].beta, bFrame.bones[0].gamma, ref rot_mat);
            glMultMatrixd(rot_mat);

            for (bi = 0; bi < bSkeleton.nBones; bi++)
            {
                if (bSkeleton.IsBattleLocation)
                {
                    DrawBattleSkeletonBone(bSkeleton.bones[bi], bSkeleton.TexIDS, false);
                }
                else
                {
                    while (!(bSkeleton.bones[bi].parentBone == joint_stack[jsp]) && jsp > 0)
                    {
                        glPopMatrix();
                        jsp--;
                    }

                    glPushMatrix();

                    // -- Commented in KimeraVB6
                    //glRotated(bFrame.bones[bi + 1].beta, 0, 1, 0);
                    //glRotated(bFrame.bones[bi + 1].alpha, 1, 0, 0);
                    //glRotated(bFrame.bones[bi + 1].gamma, 0, 0, 1);

                    BuildRotationMatrixWithQuaternions(bFrame.bones[bi + itmpbones].alpha,
                                                       bFrame.bones[bi + itmpbones].beta,
                                                       bFrame.bones[bi + itmpbones].gamma,
                                                       ref rot_mat);
                    glMultMatrixd(rot_mat);

                    DrawBattleSkeletonBone(bSkeleton.bones[bi], bSkeleton.TexIDS, bDListsEnable);

                    glTranslated(0, 0, bSkeleton.bones[bi].len);

                    jsp++;
                    joint_stack[jsp] = bi;
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
            if (bSkeleton.wpModels.Count > 0 && bAnimationsPack.WeaponAnimations.Count > 0)
            {
                glPushMatrix();
                glTranslated(wpFrame.startX, wpFrame.startY, wpFrame.startZ);

                // -- Commented in KimeraVB6
                //glRotated(wpFrame.bones[0].beta, 0, 1, 0);
                //glRotated(wpFrame.bones[0].alpha, 1, 0, 0);
                //glRotated(wpFrame.bones[0].gamma, 0, 0, 1);

                BuildRotationMatrixWithQuaternions(wpFrame.bones[0].alpha, wpFrame.bones[0].beta, wpFrame.bones[0].gamma, ref rot_mat);
                glMultMatrixd(rot_mat);

                glMatrixMode(glMatrixModeList.GL_MODELVIEW);
                glPushMatrix();

                glTranslatef(bSkeleton.wpModels[weaponIndex].repositionX,
                             bSkeleton.wpModels[weaponIndex].repositionY,
                             bSkeleton.wpModels[weaponIndex].repositionZ);

                glRotated(bSkeleton.wpModels[weaponIndex].rotateAlpha, 1, 0, 0);
                glRotated(bSkeleton.wpModels[weaponIndex].rotateBeta, 0, 1, 0);
                glRotated(bSkeleton.wpModels[weaponIndex].rotateGamma, 0, 0, 1);

                glScalef(bSkeleton.wpModels[weaponIndex].resizeX, bSkeleton.wpModels[weaponIndex].resizeY, bSkeleton.wpModels[weaponIndex].resizeZ);

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
        public static void DrawSkeletonModel(PictureBox inPicturebox, ComboBox cbBattleAnimation, ComboBox cbWeapon)
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
                                             frmSkeletonEditor.panX, frmSkeletonEditor.panY, (float)(frmSkeletonEditor.panZ + frmSkeletonEditor.DIST),
                                             (float)frmSkeletonEditor.alpha, (float)frmSkeletonEditor.beta, (float)frmSkeletonEditor.gamma, 1, 1, 1);

                        if (bShowGround)
                        {
                            glDisable(glCapability.GL_LIGHTING);
                            DrawGround(inPicturebox);
                            DrawShadow(ref p_min, ref p_max);
                        }

                        SetLights();

                        glMatrixMode(glMatrixModeList.GL_MODELVIEW);
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
                                             frmSkeletonEditor.panX, frmSkeletonEditor.panY, (float)(frmSkeletonEditor.panZ + frmSkeletonEditor.DIST),
                                             (float)frmSkeletonEditor.alpha, (float)frmSkeletonEditor.beta, (float)frmSkeletonEditor.gamma, 1, 1, 1);

                        if (bShowGround)
                        {
                            glDisable(glCapability.GL_LIGHTING);
                            DrawGround(inPicturebox);
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

                        SelectFieldBoneAndPiece(fSkeleton, fAnimation.frames[iCurrentFrameScroll], 
                                                SelectedBone, SelectedBonePiece);

                        if (bShowBones)
                        {                        
                            glDisable(glCapability.GL_DEPTH_TEST);
                            glDisable(glCapability.GL_LIGHTING);
                            glColor3f(0, 1, 0);

                            DrawFieldSkeletonBones(fSkeleton, fAnimation.frames[iCurrentFrameScroll]);

                            glEnable(glCapability.GL_DEPTH_TEST);
                        }
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        //if (!bSkeleton.IsBattleLocation)
                        //int     animIndex = Int32.Parse(cbBattleAnimation.Items[cbBattleAnimation.SelectedIndex].ToString());

                        ComputeBattleBoundingBox(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[iCurrentFrameScroll],
                                                 ref p_min, ref p_max);

                        SetCameraAroundModel(ref p_min, ref p_max,
                                             frmSkeletonEditor.panX, frmSkeletonEditor.panY, (float)(frmSkeletonEditor.panZ + frmSkeletonEditor.DIST), 
                                             (float)frmSkeletonEditor.alpha, (float)frmSkeletonEditor.beta, (float)frmSkeletonEditor.gamma, 1, 1, 1);

                        if (bShowGround)
                        {
                            glDisable(glCapability.GL_LIGHTING);
                            DrawGround(inPicturebox);
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

                        SelectBattleBoneAndModel(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[iCurrentFrameScroll],
                                                 tmpbFrame, ianimWeaponIndex, SelectedBone, SelectedBonePiece);

                        if (bShowBones)
                        {
                            glDisable(glCapability.GL_DEPTH_TEST);
                            glDisable(glCapability.GL_LIGHTING);
                            glColor3f(0, 1, 0);
                            DrawBattleSkeletonBones(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[iCurrentFrameScroll]);
                            glEnable(glCapability.GL_DEPTH_TEST);
                        }

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
        public static void DrawGround(PictureBox inPictureBox)
        {
            int gi, lw;
            float r, g, b;

            //  Draw plane
            glColor3f(0.9f, 0.9f, 1f);
            glDisable(glCapability.GL_DEPTH_TEST);
            glBegin(glDrawMode.GL_QUADS);
            glVertex4f(300f, 0f, 300f, 0.001f);
            glVertex4f(300f, 0f, -300f, 0.001f);
            glVertex4f(-300f, 0f, -300f, 0.001f);
            glVertex4f(-300f, 0f, 300f, 0.001f);
            glEnd();

            r = 0.9f;
            g = 0.9f;
            b = 1.0f;
            lw = 5;
            //glEnable(glCapability.GL_LINE_SMOOTH);

            for (gi = 10; gi >= 5; gi--)
            {
                glLineWidth(lw);
                glColor3f(r, g, b);
                glBegin(glDrawMode.GL_LINES);
                glVertex4f(0f, 0f, 50f, 0.0001f);
                glVertex4f(0f, 0f, -50f, 0.0001f);
                glVertex4f(-50f, 0f, 0f, 0.0001f);
                glVertex4f(50f, 0f, 0f, 0.0001f);
                glEnd();

                r = 0.9f - 0.9f / 10f * (10 - gi);
                g = 0.9f - 0.9f / 10f * (10 - gi);
                b = 1 - 1f / 10f * (10 - gi);
                lw = lw - 1;
            }

            glLineWidth(1f);
            //glDisable(glCapability.GL_LINE_SMOOTH);
        }

        public static void DrawShadow(ref Point3D p_min, ref Point3D p_max)
        {
            float ground_radius, sub_y, cx, cz;
            int numSegments, si;

            Point3D p_min_aux = new Point3D();
            Point3D p_max_aux = new Point3D();

            cx = 0;
            cz = 0;

            sub_y = p_max.y;
            p_min_aux = p_min;
            p_max_aux = p_max;
            p_min_aux.y = 0;
            p_max_aux.y = 0;

            cx = (p_min.x + p_max.x) / 2;
            cz = (p_min.z + p_max.z) / 2;
            ground_radius = CalculateDistance(p_min_aux, p_max_aux) / 2;

            // Draw Shadow
            numSegments = 20;
            glBegin(glDrawMode.GL_TRIANGLE_FAN);
                glColor4f(0.1f, 0.1f, 0.1f, 0.5f);
                glVertex3f(cx, 0, cz);

                for (si = 0; si <= numSegments; si++)
                {
                    glColor4f(0.1f, 0.1f, 0.1f, 0);
                    glVertex3f((float)(ground_radius * Math.Sin(si * 2 * PI / numSegments) + cx), 0,
                               (float)(ground_radius * Math.Cos(si * 2 * PI / numSegments) + cz));
                }
            glEnd();
            glEnable(glCapability.GL_DEPTH_TEST);
            glDisable(glCapability.GL_FOG);

            // Draw underlying box (just depth)
            glColorMask(GL_Boolean.GL_FALSE, GL_Boolean.GL_FALSE, GL_Boolean.GL_FALSE, GL_Boolean.GL_FALSE);
            glColor3f(1, 1, 1);
            glBegin(glDrawMode.GL_QUADS);
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

            MultiplyPoint3DByOGLMatrix(planeTransformation, planeOriginalPoint1, ref p1);
            MultiplyPoint3DByOGLMatrix(planeTransformation, planeOriginalPoint2, ref p2);
            MultiplyPoint3DByOGLMatrix(planeTransformation, planeOriginalPoint3, ref p3);
            MultiplyPoint3DByOGLMatrix(planeTransformation, planeOriginalPoint4, ref p4);

            glPolygonMode(glFace.GL_FRONT, glPolygon.GL_FILL);
            glPolygonMode(glFace.GL_BACK, glPolygon.GL_LINE);

            glColor4f(1, 0, 0, 0.10f);
            glEnable(glCapability.GL_BLEND);
            glBegin(glDrawMode.GL_QUADS);
            glVertex3f(p1.x, p1.y, p1.z);
            glVertex3f(p2.x, p2.y, p2.z);
            glVertex3f(p3.x, p3.y, p3.z);
            glVertex3f(p4.x, p4.y, p4.z);
            glEnd();
        }

        public static void DrawAxes(PictureBox pbIn)
        {
            float letterWidth, letterHeight;
            float max_x, max_y, max_z;

            Point3D pX = new Point3D();
            Point3D pY = new Point3D();
            Point3D pZ = new Point3D();

            Point3D p_max = new Point3D();
            Point3D p_min = new Point3D();

            glDisable(glCapability.GL_LIGHTING);
            ComputePModelBoundingBox(EditedPModel, ref p_min, ref p_max);

            max_x = Math.Abs(p_min.x) > Math.Abs(p_max.x) ? p_min.x : p_max.x;
            max_y = Math.Abs(p_min.y) > Math.Abs(p_max.y) ? p_min.y : p_max.y;
            max_z = Math.Abs(p_min.z) > Math.Abs(p_max.z) ? p_min.z : p_max.z;

            glBegin(glDrawMode.GL_LINES);
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
            glMatrixMode(glMatrixModeList.GL_PROJECTION);
            glLoadIdentity();
            gluOrtho2D(0, pbIn.ClientRectangle.Width, 0, pbIn.ClientRectangle.Height);
            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            glLoadIdentity();

            letterWidth = LETTER_SIZE;
            letterHeight = (float)(LETTER_SIZE * 1.5);
            glDisable(glCapability.GL_DEPTH_TEST);

            glBegin(glDrawMode.GL_LINES);
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

            glEnable(glCapability.GL_DEPTH_TEST);
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ======================================= PEDITOR DRAW  =============================================
        //  ---------------------------------------------------------------------------------------------------
        public static void DrawPModelPolys(PModel Model)
        {
            int gi, pi, vi;

            glShadeModel(glShadingModel.GL_FLAT);
            glPolygonMode(glFace.GL_FRONT, glPolygon.GL_LINE);
            glPolygonMode(glFace.GL_BACK, glPolygon.GL_FILL);
            glEnable(glCapability.GL_COLOR_MATERIAL);

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                if (!Model.Groups[gi].HiddenQ)
                {
                    for (pi = Model.Groups[gi].offsetPoly; pi < Model.Groups[gi].offsetPoly + Model.Groups[gi].numPoly; pi++)
                    {
                        glColor4f(Model.Pcolors[pi].R / 255.0f, Model.Pcolors[pi].G / 255.0f, Model.Pcolors[pi].B / 255.0f, Model.Pcolors[pi].A / 255.0f);
                        glColorMaterial(glFace.GL_FRONT_AND_BACK, glMaterialParameter.GL_AMBIENT_AND_DIFFUSE);

                        glBegin(glDrawMode.GL_TRIANGLES);
                        for (vi = 0; vi < 3; vi++)
                        {
                            glNormal3f(Model.Normals[Model.Polys[pi].Normals[vi]].x,
                                       Model.Normals[Model.Polys[pi].Normals[vi]].y,
                                       Model.Normals[Model.Polys[pi].Normals[vi]].z);

                            glVertex3f(Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].x,
                                       Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].y,
                                       Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].z);
                        }
                        glEnd();
                    }
                }
            }

            //  glPopMatrix();  -- Commented in KimeraVB6
        }

        public static void DrawPModelMesh(PModel Model)
        {
            int gi, pi, vi;

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

            glPolygonMode(glFace.GL_FRONT, glPolygon.GL_LINE);
            glPolygonMode(glFace.GL_BACK, glPolygon.GL_LINE);
            glColor3f(0, 0, 0);

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {

                if (!Model.Groups[gi].HiddenQ)
                {
                    for (pi = Model.Groups[gi].offsetPoly; pi < Model.Groups[gi].offsetPoly + Model.Groups[gi].numPoly; pi++)
                    {
                        glBegin(glDrawMode.GL_TRIANGLES);
                        for (vi = 0; vi < 3; vi++)
                        {
                            glVertex3f(Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].x,
                                       Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].y,
                                       Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].z);
                        }
                        glEnd();
                    }
                }
            }

            //  glPopMatrix();      -- Commented in KimeraVB6
        }

        public static void KillPrecalculatedLighting(PModel Model, ref pairIB[] translationTableVertex)
        {
            int ci;

            for (ci = 0; ci < Model.Header.numVerts; ci++)
            {
                translationTableVertex[ci].B = 1;
            }
        }

        public static void DrawPModelEditor(bool bEnableLighting)
        {
            double[] rot_mat = new double[16];

            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            ComputePModelBoundingBox(EditedPModel, ref p_min, ref p_max);

            SetCameraAroundModel(ref p_min, ref p_max,
                                 panXPE, panYPE, panZPE + DISTPE,
                                 alphaPE, betaPE, gammaPE, 1, 1, 1);

            if (bEnableLighting)
            {
                float modelDiameterNormalized;

                glDisable(glCapability.GL_LIGHT0);
                glDisable(glCapability.GL_LIGHT1);
                glDisable(glCapability.GL_LIGHT2);
                glDisable(glCapability.GL_LIGHT3);

                ComputePModelBoundingBox(EditedPModel, ref p_min, ref p_max);
                modelDiameterNormalized = (-2 * ComputeSceneRadius(p_min, p_max)) / frmPEditor.LIGHT_STEPS;

                SetLighting(glCapability.GL_LIGHT0, modelDiameterNormalized * iLightX,
                                                    modelDiameterNormalized * iLightY,
                                                    modelDiameterNormalized * iLightZ,
                                                    1, 1, 1, false);
            }
            else glDisable(glCapability.GL_LIGHTING);

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            glPushMatrix();

            glTranslatef(EditedPModel.repositionX,
                         EditedPModel.repositionY,
                         EditedPModel.repositionZ);

            BuildRotationMatrixWithQuaternionsXYZ(EditedPModel.rotateAlpha,
                                                  EditedPModel.rotateBeta,
                                                  EditedPModel.rotateGamma,
                                                  ref rot_mat);

            glMultMatrixd(rot_mat);
            glScalef(EditedPModel.resizeX,
                     EditedPModel.resizeY,
                     EditedPModel.resizeZ);

            switch (drawMode)
            {
                case K_MESH:
                    DrawPModelMesh(EditedPModel);
                    break;

                case K_PCOLORS:
                    glEnable(glCapability.GL_POLYGON_OFFSET_FILL);
                    glPolygonOffset(1, 1);
                    DrawPModelPolys(EditedPModel);
                    glDisable(glCapability.GL_POLYGON_OFFSET_FILL);

                    DrawPModelMesh(EditedPModel);
                    break;

                case K_VCOLORS:
                    DrawPModel(ref EditedPModel, ref tex_ids, true);
                    break;
            }

            glPopMatrix();
        }


        // -- BACKUP ORIGINAL KIMERAVB6
        //public static void DrawEditorPModel(frmPEditor frmPEdit)
        //{

        //    //double[] rot_mat = new double[16];

        //    //SetDefaultOGLRenderState();

        //    //SetCameraPModel(EditedPModel, panX, panY, panZ + DIST,
        //    //                alpha, beta, gamma, 1, 1, 1);

        //    //glMatrixMode(glMatrixModeList.GL_MODELVIEW);
        //    //glPushMatrix();

        //    //ConcatenateCameraModelView(repX, repY, repZ,
        //    //                           hsbRotateAlpha.Value, hsbRotateBeta.Value, hsbRotateGamma.Value,
        //    //                           rszX, rszY, rszZ);

        //    SetCameraPModel(EditedPModel, frmPEditor.panX, frmPEditor.panY, frmPEditor.panZ + frmPEditor.DIST,
        //                    frmPEditor.alpha, frmPEditor.beta, frmPEditor.gamma, 1, 1, 1);

        //    glMatrixMode(glMatrixModeList.GL_MODELVIEW);
        //    glPushMatrix();

        //    ConcatenateCameraModelView(repX, repY, repZ,
        //                               frmPEdit.hsbRotateAlpha.Value, frmPEdit.hsbRotateBeta.Value, frmPEdit.hsbRotateGamma.Value,
        //                               rszX, rszY, rszZ);

        //    if (frmPEdit.chkEnableLighting.Checked)
        //    {
        //        Point3D p_min = new Point3D();
        //        Point3D p_max = new Point3D();
        //        float modelDiameterNormalized;

        //        glDisable(glCapability.GL_LIGHT0);
        //        glDisable(glCapability.GL_LIGHT1);
        //        glDisable(glCapability.GL_LIGHT2);
        //        glDisable(glCapability.GL_LIGHT3);

        //        ComputePModelBoundingBox(EditedPModel, ref p_min, ref p_max);
        //        modelDiameterNormalized = (-2 * ComputeSceneRadius(p_min, p_max)) / frmPEditor.LIGHT_STEPS;

        //        SetLighting(glCapability.GL_LIGHT0, modelDiameterNormalized * frmPEdit.hsbLightX.Value,
        //                                            modelDiameterNormalized * frmPEdit.hsbLightY.Value,
        //                                            modelDiameterNormalized * frmPEdit.hsbLightZ.Value,
        //                                            1, 1, 1, false);
        //    }
        //    else glDisable(glCapability.GL_LIGHTING);

        //    SetDefaultOGLRenderState();

        //    switch (frmPEdit.drawMode)
        //    {
        //        case K_MESH:
        //            DrawPModelMesh(EditedPModel);
        //            break;

        //        case K_PCOLORS:
        //            glEnable(glCapability.GL_POLYGON_OFFSET_FILL);
        //            glPolygonOffset(1, 1);
        //            DrawPModelPolys(EditedPModel);
        //            glDisable(glCapability.GL_POLYGON_OFFSET_FILL);

        //            DrawPModelMesh(EditedPModel);
        //            break;

        //        case K_VCOLORS:
        //            DrawPModel(ref EditedPModel, ref tex_ids, true);
        //            break;
        //    }

        //    SetDefaultOGLRenderState();

        //    //glPopMatrix();
        //}

        public static int GetEqualGroupVertices(PModel Model, int iVertIdx, ref List<int> lstVerts)
        {
            int vi, iGroupIdx;
            Point3D tmpUP3DVert;

            tmpUP3DVert = Model.Verts[iVertIdx];
            iGroupIdx = GetVertexGroup(Model, iVertIdx);

            for (vi = Model.Groups[iGroupIdx].offsetVert; vi < Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert; vi++)
            {
                if (ComparePoints3D(Model.Verts[vi], tmpUP3DVert))
                {
                    lstVerts.Add(vi);
                    //  Debug.Print "Intended("; n_verts; ")"; Str$(vi)
                }
            }

            return lstVerts.Count;
        }

        public static int PaintVertex(ref PModel Model, int iGroupIdx, int iVertIdx, byte bR, byte bG, byte bB, bool bTextured)
        {
            List<int> lstVerts = new List<int>();
            int iPaintVertexResult = -1;

            Point3D tmpVert;
            Point2D tmpTexCoord = new Point2D();
            Color tmpColor;

            if (Model.Vcolors[iVertIdx + Model.Groups[iGroupIdx].offsetVert].R == bR &&
                Model.Vcolors[iVertIdx + Model.Groups[iGroupIdx].offsetVert].G == bG &&
                Model.Vcolors[iVertIdx + Model.Groups[iGroupIdx].offsetVert].B == bB)
                        iPaintVertexResult = iVertIdx;

            if (iPaintVertexResult == -1)
            {
                GetEqualGroupVertices(Model, iVertIdx + Model.Groups[iGroupIdx].offsetVert, ref lstVerts);
                foreach (int itmVert in lstVerts)
                {
                    //Debug.Print "Found("; vi; ")"; v_list(vi)
                    if (Model.Vcolors[itmVert].R == bR &&
                        Model.Vcolors[itmVert].G == bG &&
                        Model.Vcolors[itmVert].B == bB)
                    {
                        iPaintVertexResult = itmVert - Model.Groups[iGroupIdx].offsetVert;
                        break;
                    }
                }

                if (iPaintVertexResult == -1)
                {
                    tmpVert.x = Model.Verts[iVertIdx + Model.Groups[iGroupIdx].offsetVert].x;
                    tmpVert.y = Model.Verts[iVertIdx + Model.Groups[iGroupIdx].offsetVert].y;
                    tmpVert.z = Model.Verts[iVertIdx + Model.Groups[iGroupIdx].offsetVert].z;

                    tmpColor = Color.FromArgb(255, bR, bG, bB);

                    if (bTextured)
                        tmpTexCoord = Model.TexCoords[Model.Groups[iGroupIdx].offsetTex + iVertIdx];

                    iPaintVertexResult = AddVertex(ref Model, iGroupIdx, tmpVert, tmpColor) - Model.Groups[iGroupIdx].offsetVert;

                    if (glIsEnabled(glCapability.GL_LIGHTING))
                        Model.Normals[iPaintVertexResult] = Model.Normals[Model.Groups[iGroupIdx].offsetVert + iVertIdx];

                    if (bTextured)
                        Model.TexCoords[Model.Groups[iGroupIdx].offsetTex + iPaintVertexResult] = tmpTexCoord;

                    // -- Commented in KimeraVB6
                    // Model.Normals[iPaintVertexResult + Model.Groups[iGroupIdx].offsetVert] = Model.Normals[iVertIdx];
                }
                else
                {
                    // Debug.Print "Substituido por: " + Str$(PaintVertex);
                }
            }
            
            return iPaintVertexResult;
        }

        //  ------------------------------WARNINGS!----------------------------------
        //  -------*Can causes the Normals to be inconsistent (call ComputeNormals).--
        //  -------*Can causes inconsistent edges (call ComputeEdges).----------------
        //  -------*Can cause unused vertices (call KillUnusedVertices).--------------
        public static void PaintPolygon(ref PModel Model, int pIndex, byte bR, byte bG, byte bB)
        {
            int iGroupIdx, vi;

            iGroupIdx = GetPolygonGroup(Model, pIndex);

            for (vi = 0; vi < 3; vi++)
            {
                Model.Polys[pIndex].Verts[vi] = (short)PaintVertex(ref Model, iGroupIdx, Model.Polys[pIndex].Verts[vi], bR, bG, bB, 
                                                                   Model.Groups[iGroupIdx].texFlag != 0);
                //  'Debug.Print "Vert(:", .Verts(vi), ",", Group, ")", obj.Verts(.Verts(vi) + obj.Groups(Group).offVert).x, obj.Verts(.Verts(vi) + obj.Groups(Group).offVert).y, obj.Verts(.Verts(vi) + obj.Groups(Group).offVert).z

                Model.Pcolors[pIndex] = Color.FromArgb(255, bR, bG, bB);
            }
        }

        public static Color ComputePolyColor(PModel Model, int iPolyIdx)
        {
            int vi, iGroupIdx;
            int tmpA = 0, tmpR = 0, tmpG = 0, tmpB = 0;

            iGroupIdx = GetPolygonGroup(Model, iPolyIdx);

            for (vi = 0; vi < 3; vi++)
            {
                tmpA = (byte)(tmpA + Model.Vcolors[Model.Polys[iPolyIdx].Verts[vi] + Model.Groups[iGroupIdx].offsetVert].A);
                tmpR = (byte)(tmpR + Model.Vcolors[Model.Polys[iPolyIdx].Verts[vi] + Model.Groups[iGroupIdx].offsetVert].R);
                tmpG = (byte)(tmpG + Model.Vcolors[Model.Polys[iPolyIdx].Verts[vi] + Model.Groups[iGroupIdx].offsetVert].G);
                tmpB = (byte)(tmpB + Model.Vcolors[Model.Polys[iPolyIdx].Verts[vi] + Model.Groups[iGroupIdx].offsetVert].B);
            }

            return Color.FromArgb(tmpA / 3, tmpR / 3, tmpG / 3, tmpB / 3);
        }




    }
}
