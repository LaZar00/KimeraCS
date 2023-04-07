using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KimeraCS
{

    using Defines;

    using static FF7Skeleton;
    using static FF7FieldAnimation;
    using static FF7FieldRSDResource;

    using static FF7TEXTexture;
    using static FF7PModel;

    using static ModelDrawing;

    using static OpenGL32;
    using static Utils;
    using static FileTools;

    public class FF7FieldSkeleton
    {

        //
        // Field Skeleton Structure
        //
        public struct FieldSkeleton
        {
            public string fileName;
            public string name;
            public int nBones;

            public List<FieldBone> bones;
            public List<TEX> textures_pool;

            public FieldSkeleton(string strfileName, bool loadGeometryQ)
            {
                string strFileDirectoryName = Path.GetDirectoryName(strfileName);

                textures_pool = new List<TEX>();

                fileName = Path.GetFileName(strfileName).ToUpper();

                // Let's read HRC file into memory.
                string[] hrcString = File.ReadAllLines(strfileName);

                // Let's process the lines.
                // Skeleton name
                name = hrcString[1].Substring(10);

                // Skeleton number of bones
                nBones = Int32.Parse(hrcString[2].Substring(7));
                // Check model without skeleton

                bones = new List<FieldBone>();

                // Populate bones.
                // There is always "root" even there are no bones in some models.

                //  We will get the rest of bones rows checking if we are reading a correct line from
                //  .HRC file. There are some Field Models that can have '#' lines not useful among other things maybe.
                int i = 4;
                int numLine = 0;
                string rowOne = "", rowTwo = "", rowThree = "", rowFour = "";

                while (i < hrcString.Length)
                {
                    hrcString[i] = hrcString[i].Trim();

                    if (hrcString[i] != "" && hrcString[i][0] != '#' && hrcString[i][0] != ' ')
                    {
                        switch (numLine)
                        {
                            case 0:
                                rowOne = hrcString[i];
                                numLine++;
                                break;
                            case 1:
                                rowTwo = hrcString[i];
                                numLine++;
                                break;
                            case 2:
                                rowThree = hrcString[i];
                                numLine++;
                                break;
                            case 3:
                                rowFour = hrcString[i];
                                numLine = 0;
                                break;
                        }
                        
                        if (numLine == 0)
                        {
                            bones.Add(new FieldBone(rowOne,
                                                    rowTwo,
                                                    rowThree,
                                                    rowFour,
                                                    ref textures_pool,
                                                    loadGeometryQ,
                                                    strFileDirectoryName));
                        }
                    }

                    i++;
                }
            }
        }

        //
        // Field Skeleton Bone Structure
        //
        public struct FieldBone
        {
            public int nResources;
            public List<FieldRSDResource> fRSDResources;
            public double len;
            public string joint_i;
            public string joint_f;
            // added attributes
            public float resizeX;
            public float resizeY;
            public float resizeZ;

            public FieldBone(string inJointI, string inJointF, string inLen, string inRSDLine,
                             ref List<TEX> texturesPool, bool loadgeometryQ, string strFolderName)
            {
                string[] rsdRes = inRSDLine.Split();
                int i;

                len = Double.Parse(inLen, CultureInfo.InvariantCulture);

                joint_i = "";
                joint_f = "";

                resizeX = 0;
                resizeY = 0;
                resizeZ = 0;

                nResources = 0;

                fRSDResources = new List<FieldRSDResource>();

                if (loadgeometryQ)
                {
                    if (inRSDLine.Length > 2)
                        nResources = Int32.Parse(rsdRes[0].Substring(0, inRSDLine.IndexOf(" ")));

                    joint_i = inJointI;
                    joint_f = inJointF;

                    resizeX = 1;
                    resizeY = 1;
                    resizeZ = 1;

                    // Populate resources (RSD files)
                    if (nResources > 0)
                    {
                        if (nResources > rsdRes.Length - 1) nResources = rsdRes.Length - 1;

                        for (i = 0; i < nResources && rsdRes[i + 1] != null; i++)
                        {
                            fRSDResources.Add(new FieldRSDResource(rsdRes[i + 1], ref texturesPool, strFolderName));
                        }
                    }
                }
            }
        }


        //
        // Field Skeleton functions
        //
        public static void ComputeFieldBoneBoundingBox(FieldBone bone, ref Point3D p_min, ref Point3D p_max)
        {
            short ri;
            Point3D p_min_part = new Point3D();
            Point3D p_max_part = new Point3D();

            if (bone.nResources == 0)
            {
                p_max.x = 0;
                p_max.y = 0;
                p_max.z = 0;

                p_min.x = 0;
                p_min.y = 0;
                p_min.z = 0;
            }
            else
            {
                p_max.x = -(float)INFINITY_SINGLE;
                p_max.y = -(float)INFINITY_SINGLE;
                p_max.z = -(float)INFINITY_SINGLE;

                p_min.x = (float)INFINITY_SINGLE;
                p_min.y = (float)INFINITY_SINGLE;
                p_min.z = (float)INFINITY_SINGLE;

                for (ri = 0; ri < bone.nResources; ri++)
                {
                    ComputePModelBoundingBox(bone.fRSDResources[ri].Model, ref p_min_part, ref p_max_part);

                    if (p_max.x < p_max_part.x) p_max.x = p_max_part.x;
                    if (p_max.y < p_max_part.y) p_max.y = p_max_part.y;
                    if (p_max.z < p_max_part.z) p_max.z = p_max_part.z;

                    if (p_min.x > p_min_part.x) p_min.x = p_min_part.x;
                    if (p_min.y > p_min_part.y) p_min.y = p_min_part.y;
                    if (p_min.z > p_min_part.z) p_min.z = p_min_part.z;

                }
            }
        }

        public static void ComputeFieldBoundingBox(FieldSkeleton fSkeleton, FieldFrame fFrame,
                                                   ref Point3D p_min_field, ref Point3D p_max_field)
        {
            string[] joint_stack;
            int jsp;
            double[] rot_mat = new double[16];
            double[] MV_matrix = new double[16];
            int bi;

            Point3D p_max_bone = new Point3D();
            Point3D p_min_bone = new Point3D();

            Point3D p_max_bone_trans = new Point3D();
            Point3D p_min_bone_trans = new Point3D();

            //joint_stack = new string[Skeleton.nBones + 1];
            joint_stack = new string[fSkeleton.bones.Count + 1];
            jsp = 0;

            joint_stack[jsp] = fSkeleton.bones[0].joint_f;

            p_max_field.x = -(float)INFINITY_SINGLE;
            p_max_field.y = -(float)INFINITY_SINGLE;
            p_max_field.z = -(float)INFINITY_SINGLE;

            p_min_field.x = (float)INFINITY_SINGLE;
            p_min_field.y = (float)INFINITY_SINGLE;
            p_min_field.z = (float)INFINITY_SINGLE;

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPushMatrix();
            glLoadIdentity();

            glTranslated(fFrame.rootTranslationX, 0, 0);
            glTranslated(0, -fFrame.rootTranslationY, 0);
            glTranslated(0, 0, fFrame.rootTranslationZ);

            BuildRotationMatrixWithQuaternions(fFrame.rootRotationAlpha, fFrame.rootRotationBeta, fFrame.rootRotationGamma, ref rot_mat);
            glMultMatrixd(rot_mat);

            for (bi = 0; bi < fSkeleton.bones.Count; bi++)
            {
                while (!(fSkeleton.bones[bi].joint_f == joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }
                glPushMatrix();

                BuildRotationMatrixWithQuaternions(fFrame.rotations[bi].alpha, fFrame.rotations[bi].beta,
                                                         fFrame.rotations[bi].gamma, ref rot_mat);
                glMultMatrixd(rot_mat);

                ComputeFieldBoneBoundingBox(fSkeleton.bones[bi], ref p_min_bone, ref p_max_bone);

                glGetDoublev((uint)GLCapability.GL_MODELVIEW_MATRIX, MV_matrix);

                ComputeTransformedBoxBoundingBox(MV_matrix, ref p_min_bone, ref p_max_bone, ref p_min_bone_trans, ref p_max_bone_trans);

                if (p_max_field.x < p_max_bone_trans.x) p_max_field.x = p_max_bone_trans.x;
                if (p_max_field.y < p_max_bone_trans.y) p_max_field.y = p_max_bone_trans.y;
                if (p_max_field.z < p_max_bone_trans.z) p_max_field.z = p_max_bone_trans.z;

                if (p_min_field.x > p_min_bone_trans.x) p_min_field.x = p_min_bone_trans.x;
                if (p_min_field.y > p_min_bone_trans.y) p_min_field.y = p_min_bone_trans.y;
                if (p_min_field.z > p_min_bone_trans.z) p_min_field.z = p_min_bone_trans.z;

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

        public static float ComputeFieldBoneDiameter(FieldBone bone)
        {
            int iResourceIdx;
            float computeDiameter = 0;

            Point3D p_max = new Point3D();
            Point3D p_min = new Point3D();

            if (bone.nResources > 0)
            {
                p_max.x = -(float)INFINITY_SINGLE;
                p_max.y = -(float)INFINITY_SINGLE;
                p_max.z = -(float)INFINITY_SINGLE;

                p_min.x = (float)INFINITY_SINGLE;
                p_min.y = (float)INFINITY_SINGLE;
                p_min.z = (float)INFINITY_SINGLE;

                for (iResourceIdx = 0; iResourceIdx < bone.nResources; iResourceIdx++)
                {
                    if (p_max.x < bone.fRSDResources[iResourceIdx].Model.BoundingBox.max_x)
                        p_max.x = bone.fRSDResources[iResourceIdx].Model.BoundingBox.max_x;
                    if (p_max.y < bone.fRSDResources[iResourceIdx].Model.BoundingBox.max_y)
                        p_max.y = bone.fRSDResources[iResourceIdx].Model.BoundingBox.max_y;
                    if (p_max.z < bone.fRSDResources[iResourceIdx].Model.BoundingBox.max_z)
                        p_max.z = bone.fRSDResources[iResourceIdx].Model.BoundingBox.max_z;

                    if (p_min.x > bone.fRSDResources[iResourceIdx].Model.BoundingBox.min_x)
                        p_min.x = bone.fRSDResources[iResourceIdx].Model.BoundingBox.min_x;
                    if (p_min.y > bone.fRSDResources[iResourceIdx].Model.BoundingBox.min_y)
                        p_min.y = bone.fRSDResources[iResourceIdx].Model.BoundingBox.min_y;
                    if (p_min.z > bone.fRSDResources[iResourceIdx].Model.BoundingBox.min_z)
                        p_min.z = bone.fRSDResources[iResourceIdx].Model.BoundingBox.min_z;
                }

                computeDiameter = CalculateDistance(p_max, p_min);
            }

            return computeDiameter;
        }

        public static float ComputeFieldDiameter(FieldSkeleton Skeleton)
        {
            int iBoneIdx;
            float aux_diam;
            float fcomputeFieldDiameterResult = 0;

            //for (bi = 0; bi < Skeleton.nBones; bi++)
            for (iBoneIdx = 0; iBoneIdx < Skeleton.bones.Count; iBoneIdx++)
            {
                aux_diam = ComputeFieldBoneDiameter(Skeleton.bones[iBoneIdx]);

                fcomputeFieldDiameterResult += aux_diam;
            }

            return fcomputeFieldDiameterResult;
        }

        public static void SelectFieldBoneAndPiece(FieldSkeleton fSkeleton, FieldFrame fFrame, int b_index, int p_index)
        {
            int i, jsp;
            double[] rot_mat = new double[16];

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPushMatrix();

            glTranslated(fFrame.rootTranslationX, 0, 0);
            glTranslated(0, -fFrame.rootTranslationY, 0);
            glTranslated(0, 0, fFrame.rootTranslationZ);

            BuildRotationMatrixWithQuaternions(fFrame.rootRotationAlpha, fFrame.rootRotationBeta, fFrame.rootRotationGamma, ref rot_mat);
            glMultMatrixd(rot_mat);

            if (b_index > -1)
            {
                jsp = MoveToFieldBone(fSkeleton, fFrame, b_index);
                DrawFieldBoneBoundingBox(fSkeleton.bones[b_index]);

                if (p_index > -1)
                    DrawFieldBonePieceBoundingBox(fSkeleton.bones[b_index], p_index);

                for (i = 0; i <= jsp; i++) glPopMatrix();
            }

            glPopMatrix();
        }

        public static int GetClosestFieldBone(FieldSkeleton fSkeleton, FieldFrame fFrame,
                                              int px, int py)
        {
            int iGetClosestFieldBoneResult;

            int bi, nBones, height, jsp;
            float min_z;

            int[] vp = new int[4];
            double[] P_matrix = new double[16];
            double[] rot_mat = new double[16];

            int[] selBuff = new int[fSkeleton.bones.Count * 4];

            string[] joint_stack = new string[fSkeleton.bones.Count * 4];

            jsp = 0;
            joint_stack[jsp] = fSkeleton.bones[0].joint_f;

            glSelectBuffer(fSkeleton.bones.Count * 4, selBuff);
            glInitNames();

            glRenderMode(GLRenderingMode.GL_SELECT);

            glMatrixMode(GLMatrixModeList.GL_PROJECTION);
            glPushMatrix();
            glGetDoublev((uint)GLCapability.GL_PROJECTION_MATRIX, P_matrix);
            glLoadIdentity();

            glGetIntegerv((uint)GLCapability.GL_VIEWPORT, vp);
            height = vp[3];

            gluPickMatrix(px - 1, height - py + 1, 3, 3, vp);
            //gluPerspective(60, (float)width / height, 0.1, 10000); //'max(0.1 - DIST, 0.1), ComputeHRCDiameter(obj) * 2 - DIST
            glMultMatrixd(P_matrix);

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPushMatrix();

            glTranslated(fFrame.rootTranslationX, 0, 0);
            glTranslated(0, -fFrame.rootTranslationY, 0);
            glTranslated(0, 0, fFrame.rootTranslationZ);

            BuildRotationMatrixWithQuaternions(fFrame.rootRotationAlpha, fFrame.rootRotationBeta, fFrame.rootRotationGamma, ref rot_mat);
            glMultMatrixd(rot_mat);

            for (bi = 0; bi < fSkeleton.bones.Count; bi++)
            {
                glPushName((uint)bi);

                while (!(fSkeleton.bones[bi].joint_f == joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }
                glPushMatrix();

                //            'glRotated Frame.Rotations(bi).Beta, 0#, 1#, 0#
                //            'glRotated Frame.Rotations(bi).Alpha, 1#, 0#, 0#
                //            'glRotated Frame.Rotations(bi).Gamma, 0#, 0#, 1#

                BuildRotationMatrixWithQuaternions(fFrame.rotations[bi].alpha, fFrame.rotations[bi].beta, fFrame.rotations[bi].gamma, ref rot_mat);
                glMultMatrixd(rot_mat);

                DrawFieldBone(fSkeleton.bones[bi], false);

                glTranslated(0, 0, -fSkeleton.bones[bi].len);
                jsp++;
                joint_stack[jsp] = fSkeleton.bones[bi].joint_i;

                glPopName();
            }

            while (jsp > 0)
            {
                glPopMatrix();
                jsp--;
            }
            glPopMatrix();

            glMatrixMode(GLMatrixModeList.GL_PROJECTION);
            glPopMatrix();

            nBones = glRenderMode(GLRenderingMode.GL_RENDER);

            iGetClosestFieldBoneResult = -1;
            min_z = -1;

            for (bi = 0; bi < nBones; bi++)
            {
                if (CompareLongs((long)min_z, selBuff[bi * 4 + 1]))
                {
                    min_z = selBuff[bi * 4 + 1];
                    iGetClosestFieldBoneResult = selBuff[bi * 4 + 3];
                }
            }

            return iGetClosestFieldBoneResult;
        }

        public static int GetClosestFieldBonePiece(FieldSkeleton fSkeleton, FieldFrame fFrame,
                                                   int iBoneSelected, int px, int py)
        {
            int iGetClosestFieldBonePieceResult;

            int iBoneIdx, iPolyIdx, iNumPieces, width, height, jsp;
            float min_z;

            int[] vp = new int[4];
            double[] P_matrix = new double[16];
            double[] rot_mat = new double[16];

            int[] selBuff = new int[fSkeleton.bones[iBoneSelected].nResources * 4];

            //string[] joint_stack = new string[Skeleton.nBones];
            string[] joint_stack = new string[fSkeleton.bones.Count];
            jsp = 0;

            glSelectBuffer(fSkeleton.bones[iBoneSelected].nResources * 4, selBuff);
            glInitNames();

            glRenderMode(GLRenderingMode.GL_SELECT);

            glMatrixMode(GLMatrixModeList.GL_PROJECTION);
            glPushMatrix();
            glGetDoublev((uint)GLCapability.GL_PROJECTION_MATRIX, P_matrix);
            glLoadIdentity();

            glGetIntegerv((uint)GLCapability.GL_VIEWPORT, vp);
            width = vp[2];
            height = vp[3];

            gluPickMatrix(px - 1, height - py + 1, 3, 3, vp);
            gluPerspective(60, (float)width / height, 0.1, 10000);        // max(0.1 - DIST, 0.1) , ComputeFieldDiameter(ref Skeleton) * 2 - DIST);
            //glMultMatrixd(P_matrix);

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPushMatrix();

            glTranslated(fFrame.rootTranslationX, 0, 0);
            glTranslated(0, -fFrame.rootTranslationY, 0);
            glTranslated(0, 0, fFrame.rootTranslationZ);

            BuildRotationMatrixWithQuaternions(fFrame.rootRotationAlpha, fFrame.rootRotationBeta, fFrame.rootRotationGamma, ref rot_mat);
            glMultMatrixd(rot_mat);

            for (iBoneIdx = 0; iBoneIdx < iBoneSelected; iBoneIdx++)
            {
                while (!(fSkeleton.bones[iBoneIdx].joint_f == joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }
                glPushMatrix();

                //  glRotated Frame.Rotations(bi).Beta, 0#, 1#, 0#
                //  glRotated Frame.Rotations(bi).Alpha, 1#, 0#, 0#
                //  glRotated Frame.Rotations(bi).Gamma, 0#, 0#, 1#

                BuildRotationMatrixWithQuaternions(fFrame.rotations[iBoneIdx].alpha, 
                                                   fFrame.rotations[iBoneIdx].beta, 
                                                   fFrame.rotations[iBoneIdx].gamma, ref rot_mat);
                glMultMatrixd(rot_mat);

                glTranslated(0, 0, -fSkeleton.bones[iBoneIdx].len);

                jsp++;
                joint_stack[jsp] = fSkeleton.bones[iBoneIdx].joint_i;
            }

            while (!(fSkeleton.bones[iBoneSelected].joint_f == joint_stack[jsp]) && jsp > 0)
            {
                glPopMatrix();
                jsp--;
            }
            glPushMatrix();

            glRotated(fFrame.rotations[iBoneSelected].beta, 0, 1, 0);
            glRotated(fFrame.rotations[iBoneSelected].alpha, 1, 0, 0);
            glRotated(fFrame.rotations[iBoneSelected].gamma, 0, 0, 1);
            jsp++;

            for (iPolyIdx = 0; iPolyIdx < fSkeleton.bones[iBoneSelected].nResources; iPolyIdx++)
            {
                glPushName((uint)iPolyIdx);

                DrawRSDResource(fSkeleton.bones[iBoneSelected].fRSDResources[iPolyIdx], false);

                glPopName();
            }

            while (jsp > 0)
            {
                glPopMatrix();
                jsp--;
            }
            glPopMatrix();
            glMatrixMode(GLMatrixModeList.GL_PROJECTION);
            glPopMatrix();

            iNumPieces = glRenderMode(GLRenderingMode.GL_RENDER);
            iGetClosestFieldBonePieceResult = -1;
            min_z = -1;

            for (iPolyIdx = 0; iPolyIdx < iNumPieces; iPolyIdx++)
            {
                if (CompareLongs((long)min_z, selBuff[iPolyIdx * 4 + 1]))
                {
                    min_z = selBuff[iPolyIdx * 4 + 1];
                    iGetClosestFieldBonePieceResult = selBuff[iPolyIdx * 4 + 3];
                }
            }
            //  Debug.Print GetClosestHRCBonePiece, nPieces

            return iGetClosestFieldBonePieceResult;
        }

        public static void AddFieldBone(ref FieldBone fBone, ref PModel Model)
        {
            FieldRSDResource tmpRSDResource = new FieldRSDResource()
            {
                ID = "@RSD940102",
                textures = new List<TEX>(),
                Model = Model,
            };

            if (fBone.nResources > 0)
            {
                tmpRSDResource.Model.fileName = fBone.fRSDResources[0].Model.fileName.Substring(0, 4).ToUpper() + 
                                                fBone.nResources.ToString() +
                                                ".P";

                tmpRSDResource.res_file = fBone.fRSDResources[0].res_file.ToUpper() + fBone.nResources.ToString();
            }
            else
            {
                tmpRSDResource.numTextures = 0;

                if (tmpRSDResource.res_file == null)
                {
                    tmpRSDResource.res_file = Model.fileName.Substring(0, 4).ToUpper();
                }
                else
                {
                    tmpRSDResource.res_file = tmpRSDResource.res_file.Substring(0, 4).ToUpper();
                }
                
            }

            fBone.fRSDResources.Add(tmpRSDResource);
            fBone.nResources++;
        }

        public static void RemoveFieldBone(ref FieldBone fBone, ref int b_index)
        {
            FieldRSDResource tmpfRSDResource;

            string tmpRSDResourceName, tmpModelName;
            int iRSDCounter;

            tmpModelName = fBone.fRSDResources[0].Model.fileName.Substring(0, 4).ToUpper();
            tmpRSDResourceName = fBone.fRSDResources[0].res_file.Substring(0, 4).ToUpper();

            if (b_index < fBone.nResources)
            {
                fBone.fRSDResources.RemoveAt(b_index);
                fBone.nResources--;
            }


            // Let's assign the correct names.
            for (iRSDCounter = 0; iRSDCounter < fBone.nResources; iRSDCounter++)
            {
                tmpfRSDResource = fBone.fRSDResources[iRSDCounter];

                tmpfRSDResource.Model.fileName = tmpModelName;
                tmpfRSDResource.res_file = tmpRSDResourceName;

                if (iRSDCounter > 0)
                {
                    tmpfRSDResource.Model.fileName += iRSDCounter.ToString();
                    tmpfRSDResource.res_file += iRSDCounter.ToString();
                }

                tmpfRSDResource.Model.fileName += ".P";

                fBone.fRSDResources[iRSDCounter] = tmpfRSDResource;
            }
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ============================================= SAVING ==============================================
        //  ---------------------------------------------------------------------------------------------------
        public static void MergeResources(ref FieldBone fBone)
        {
            int iResourceIdx;
            FieldRSDResource tmpRSDResource;

            for (iResourceIdx = 1; iResourceIdx < fBone.nResources; iResourceIdx++)
            {
                tmpRSDResource = fBone.fRSDResources[0];
                MergeFieldRSDResources(ref tmpRSDResource, fBone.fRSDResources[iResourceIdx]);
                fBone.fRSDResources[0] = tmpRSDResource;
            }
        }

        public static void ApplyFieldBoneChanges(ref FieldBone fBone, bool merge)
        {
            int ri;
            FieldRSDResource tmpRSDResource;

            for (ri = 0; ri < fBone.nResources; ri++)
            {
                //  Debug.Print "File=", bone.Resources(ri).res_file, bone.Resources(ri).Model.fileName
                if (glIsEnabled(GLCapability.GL_LIGHTING))
                {
                    tmpRSDResource = fBone.fRSDResources[ri];
                    ApplyCurrentVColors(ref tmpRSDResource.Model);
                    fBone.fRSDResources[ri] = tmpRSDResource;
                }

                glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
                glPushMatrix();

                SetCameraModelViewQuat(fBone.fRSDResources[ri].Model.repositionX, fBone.fRSDResources[ri].Model.repositionY, fBone.fRSDResources[ri].Model.repositionZ,
                                       fBone.fRSDResources[ri].Model.rotationQuaternion,
                                       fBone.fRSDResources[ri].Model.resizeX, fBone.fRSDResources[ri].Model.resizeY, fBone.fRSDResources[ri].Model.resizeZ);

                glScalef(fBone.resizeX, fBone.resizeY, fBone.resizeZ);

                tmpRSDResource = fBone.fRSDResources[ri];
                ApplyPChanges(ref tmpRSDResource.Model, false);
                fBone.fRSDResources[ri] = tmpRSDResource;

                glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
                glPopMatrix();
            }

            if (merge)
            {
                MergeResources(ref fBone);

                if (fBone.nResources > 1)
                {
                    fBone.nResources = 1;
                    while (fBone.fRSDResources.Count > 1) fBone.fRSDResources.RemoveAt(fBone.fRSDResources.Count - 1);

                    tmpRSDResource = fBone.fRSDResources[0];
                    ComputeBoundingBox(ref tmpRSDResource.Model);
                    fBone.fRSDResources[0] = tmpRSDResource;
                }
            }
        }

        public static void ApplyFieldChanges(ref FieldSkeleton fSkeleton, FieldFrame fFrame, bool merge)
        {
            int bi, jsp;
            //string[] joint_stack = new string[fSkeleton.nBones];
            string[] joint_stack = new string[fSkeleton.bones.Count + 1];

            FieldBone tmpfBone;

            jsp = 0;
            joint_stack[jsp] = fSkeleton.bones[0].joint_f;

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);

            //for (bi = 0; bi < fSkeleton.nBones; bi++)
            for (bi = 0; bi < fSkeleton.bones.Count; bi++)
            {
                while ((fSkeleton.bones[bi].joint_f != joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }
                glPushMatrix();

                glRotated(fFrame.rotations[bi].beta, 0, 1, 0);
                glRotated(fFrame.rotations[bi].alpha, 1, 0, 0);
                glRotated(fFrame.rotations[bi].gamma, 0, 0, 1);

                //  Debug.Print bi, obj.Bones(bi).joint_f, obj.Bones(bi).NumResources
                tmpfBone = fSkeleton.bones[bi];
                ApplyFieldBoneChanges(ref tmpfBone, merge);
                fSkeleton.bones[bi] = tmpfBone;

                glTranslated(0, 0, -fSkeleton.bones[bi].len);

                jsp++;
                joint_stack[jsp] = fSkeleton.bones[bi].joint_i;
            }

            while (jsp > 0)
            {
                glPopMatrix();
                jsp--;
            }
        }

        public static void WriteFieldBone(ref StringBuilder strHRCContent, ref FieldBone fBone,
                                          string strDirectoryPath)
        {
            int ri;
            string strRSDList;

            FieldRSDResource tmpRSDResource;

            strHRCContent.AppendLine("");
            strHRCContent.AppendLine(fBone.joint_i);
            strHRCContent.AppendLine(fBone.joint_f);
            strHRCContent.AppendLine(fBone.len.ToString("0.0######", CultureInfo.InvariantCulture));

            strRSDList = fBone.nResources.ToString();

            if (fBone.nResources > 0)
            {
                // Write resources (if there is number involved it begins with 1, if we use "0" as first number there are issues).
                //                  first resource has no number.
                for (ri = 0; ri < fBone.nResources; ri++)
                {
                    tmpRSDResource = fBone.fRSDResources[ri];

                    strRSDList = strRSDList + " " + tmpRSDResource.res_file.ToUpper();

                    WriteRSDResource(tmpRSDResource, strDirectoryPath + "\\" + fBone.fRSDResources[ri].res_file.ToUpper() + ".RSD");

                    if (tmpRSDResource.Model.Polys != null)
                        WriteGlobalPModel(ref tmpRSDResource.Model, strDirectoryPath + "\\" + fBone.fRSDResources[ri].Model.fileName.ToUpper());

                    fBone.fRSDResources[ri] = tmpRSDResource;
                }
            }
            else strRSDList += " ";

            strHRCContent.AppendLine(strRSDList);
        }

        public static void WriteFieldSkeleton(ref FieldSkeleton fSkeleton, string fileName)
        {
            int bi;
            StringBuilder strHRCContent = new StringBuilder();
            FieldBone tmpfBone;

            strHRCContent.AppendLine(":HEADER_BLOCK 2");
            strHRCContent.AppendLine(":SKELETON " + fSkeleton.name);
            strHRCContent.AppendLine(":BONES " + fSkeleton.nBones);

            //for (bi = 0; bi < fSkeleton.nBones; bi++)
            for (bi = 0; bi < fSkeleton.bones.Count; bi++)
            {
                tmpfBone = fSkeleton.bones[bi];
                WriteFieldBone(ref strHRCContent, ref tmpfBone, Path.GetDirectoryName(fileName));
                fSkeleton.bones[bi] = tmpfBone;
            }

            File.WriteAllText(fileName.ToUpper(), strHRCContent.ToString());
        }

        public static void CreateDListsFromFieldSkeletonBone(ref FieldBone fBone)
        {
            int ri;
            FieldRSDResource tmpRSDResource;

            for (ri = 0; ri < fBone.nResources; ri++)
            {
                tmpRSDResource = fBone.fRSDResources[ri];
                CreateDListsFromRSDResource(ref tmpRSDResource);
                fBone.fRSDResources[ri] = tmpRSDResource;
            }
        }

        public static void CreateDListsFromFieldSkeleton(ref FieldSkeleton fSkeleton)
        {
            int bi;
            FieldBone tmpfBone;

            //  THIS DOES NOT NEED fAnimation.nBonesCount CHANGE
            for (bi = 0; bi < fSkeleton.bones.Count; bi++)
            {
                tmpfBone = fSkeleton.bones[bi];
                CreateDListsFromFieldSkeletonBone(ref tmpfBone);
                fSkeleton.bones[bi] = tmpfBone;
            }
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ============================================= DESTROY =============================================
        //  ---------------------------------------------------------------------------------------------------
        public static void DestroyFieldBoneResources(ref FieldBone fBone)
        {
            int ri;
            FieldRSDResource tmpRSDResource;

            for (ri = 0; ri < fBone.nResources; ri++)
            {
                tmpRSDResource = fBone.fRSDResources[ri];
                DestroyRSDResources(ref tmpRSDResource);
                fBone.fRSDResources[ri] = tmpRSDResource;
            }

            if (fBone.fRSDResources != null) fBone.fRSDResources.Clear();
        }

        public static void DestroyFieldSkeleton(FieldSkeleton fSkeleton)
        {
            int bi;
            FieldBone tmpfBone;

            //for (bi = 0; bi < fSkeleton.nBones; bi++)
            if (fSkeleton.name != null)
            {
                //  THIS DOES NOT NEED fAnimation.nBonesCount CHANGE
                for (bi = 0; bi < fSkeleton.bones.Count; bi++)
                {
                    tmpfBone = fSkeleton.bones[bi];
                    DestroyFieldBoneResources(ref tmpfBone);
                    fSkeleton.bones[bi] = tmpfBone;
                }

                if (fSkeleton.bones != null) fSkeleton.bones.Clear();
            }
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ========================================== COPY SKELETON ==========================================
        //  ---------------------------------------------------------------------------------------------------
        public static FieldRSDResource CopyfRSDResource(FieldRSDResource fResourceIn)
        {
            FieldRSDResource fResourceOut;

            fResourceOut = new FieldRSDResource()
            {
                ID = fResourceIn.ID,
                numTextures = fResourceIn.numTextures,
                res_file = fResourceIn.res_file,

                textures = new List<TEX>(fResourceIn.textures),

                Model = CopyPModel(fResourceIn.Model),
            };

            foreach (TEX itmTex in fResourceIn.textures) fResourceOut.textures.Add(itmTex);

            return fResourceOut;
        }

        public static FieldBone CopyfBone(FieldBone fBoneIn)
        {
            FieldBone fBoneOut;

            fBoneOut = new FieldBone()
            {
                joint_f = fBoneIn.joint_f,
                joint_i = fBoneIn.joint_i,
                len = fBoneIn.len,
                nResources = fBoneIn.nResources,

                resizeX = fBoneIn.resizeX,
                resizeY = fBoneIn.resizeY,
                resizeZ = fBoneIn.resizeZ,
            };

            fBoneOut.fRSDResources = new List<FieldRSDResource>();

            foreach (FieldRSDResource itmfRSDResource in fBoneIn.fRSDResources) fBoneOut.fRSDResources.Add(CopyfRSDResource(itmfRSDResource));

            return fBoneOut;
        }

        public static FieldSkeleton CopyfSkeleton(FieldSkeleton fSkeletonIn)
        {
            FieldSkeleton fSkeletonOut;

            fSkeletonOut = new FieldSkeleton()
            {
                fileName = fSkeletonIn.fileName,
                name = fSkeletonIn.name,
                nBones = fSkeletonIn.nBones,

                bones = new List<FieldBone>(),
            };

            foreach (FieldBone itmfBone in fSkeletonIn.bones) fSkeletonOut.bones.Add(CopyfBone(itmfBone));

            return fSkeletonOut;
        }




    }
}
