﻿using System;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;

namespace KimeraCS
{

    using static FF7Skeleton;
    using static FF7FieldSkeleton;

    using static Utils;
    using static FileTools;

    public class FF7FieldAnimation
    {

        public struct FieldRotation
        {
            public float alpha;
            public float beta;
            public float gamma;

            public FieldRotation(float in_alpha, float in_beta, float in_gamma)
            {
                alpha = in_alpha;
                beta = in_beta;
                gamma = in_gamma;
            }
        }

        //  A Frame format by Mirex and Aali
        //  http://wiki.qhimm.com/FF7/Field_Module#.22A.22_Field_Animation_Files_for_PC_by_Mirex_.28Edits_by_Aali.29
        public struct FieldFrame
        {
            public float rootRotationAlpha;
            public float rootRotationBeta;
            public float rootRotationGamma;
            public float rootTranslationX;
            public float rootTranslationY;
            public float rootTranslationZ;
            public List<FieldRotation> rotations;

            public FieldFrame(float rotationAlpha, float rotationBeta, float rotationGamma,
                              float translationX, float translationY, float translationZ, List<FieldRotation> in_rotations)
            {
                rootRotationAlpha = rotationAlpha;
                rootRotationBeta = rotationBeta;
                rootRotationGamma = rotationGamma;

                rootTranslationX = translationX;
                rootTranslationY = translationY;
                rootTranslationZ = translationZ;

                rotations = in_rotations;
            }
        }

        //  A Animation format by Mirex and Aali
        //  http://wiki.qhimm.com/FF7/Field_Module#.22A.22_Field_Animation_Files_for_PC_by_Mirex_.28Edits_by_Aali.29
        //
        // Field Animation Structure
        //
        public struct FieldAnimation
        {
            public int version;        //  Must be '1' for FF7 to load it
            public int nFrames;
            public int nBones;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] rotationOrder;       //  Rotation order determines in which order rotations
                                               //  are applied, 0 means alpha rotation, 1 beta rotation and
                                               //  2 gamma rotation
            public byte unused;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] runtime_data;
            public List<FieldFrame> frames;
            // Extra vars
            public string strFieldAnimationFile;

            public FieldAnimation(FieldSkeleton fSkeleton, string strFullFileName, bool bDirectProcessing)
            {
                // Preliminar initialization of vars.
                version = 0;
                nFrames = 0;
                nBones = 0;
                rotationOrder = null;
                unused = 0;
                runtime_data = null;
                frames = new List<FieldFrame>();

                strFieldAnimationFile = "";

                try
                {
                    // Now we populate animation. We will check if the animation has been found for the model or not.
                    if (bDirectProcessing)
                    {
                        // Case were we have found a compatible animation for the opened model.
                        strFieldAnimationFile = Path.GetFileName(strFullFileName);
                        strGlobalFieldAnimationName = strFieldAnimationFile;

                        ReadFieldAnimation(strFullFileName);
                        FixFieldAnimation(fSkeleton, this);
                    }
                    else
                    {
                        // Case were we didn't found a compatible animation for the opened model.
                        strFieldAnimationFile = "DUMMY.A";
                        strGlobalFieldAnimationName = strFieldAnimationFile;

                        if (Path.GetFileName(strFullFileName) == strFieldAnimationFile)
                            MessageBox.Show("There is no animation file that fits the model in the same folder.", "Info");

                        CreateCompatibleFieldAnimation();
                    }
                }
                catch (Exception ex)
                {
                    // Ok. If loading the animation we have problems,
                    // we will create a compatible animation.
                    MessageBox.Show("Error exception reading Field Animation: " + strFieldAnimationFile.ToUpper() + ".\n" +
                                    "Exception message: " + ex.Message + ".",
                                    "Error", MessageBoxButtons.OK);
                }
            }

            public void ReadFieldAnimation(string strFullFileName)
            {
                int fi, bi;
                float alpha, beta, gamma, rotationAlpha, rotationBeta, rotationGamma, translationX, translationY, translationZ;
                List<FieldRotation> fieldRotations;

                byte[] fileBuffer = File.ReadAllBytes(strFullFileName);

                using (var fileMemory = new MemoryStream(fileBuffer))
                {
                    using (var memReader = new BinaryReader(fileMemory))
                    {
                        version = memReader.ReadInt32();
                        nFrames = memReader.ReadInt32();

                        nBones = memReader.ReadInt32();

                        rotationOrder = new byte[3];
                        rotationOrder[0] = memReader.ReadByte();
                        rotationOrder[1] = memReader.ReadByte();
                        rotationOrder[2] = memReader.ReadByte();

                        unused = memReader.ReadByte();

                        runtime_data = new int[5];
                        runtime_data[0] = memReader.ReadInt32();
                        runtime_data[1] = memReader.ReadInt32();
                        runtime_data[2] = memReader.ReadInt32();
                        runtime_data[3] = memReader.ReadInt32();
                        runtime_data[4] = memReader.ReadInt32();

                        //                        fieldAnimation.frames = new List<FieldFrame>();

                        for (fi = 0; fi < nFrames; fi++)
                        {
                            rotationAlpha = memReader.ReadSingle();
                            rotationBeta = memReader.ReadSingle();
                            rotationGamma = memReader.ReadSingle();

                            translationX = memReader.ReadSingle();
                            translationY = memReader.ReadSingle();
                            translationZ = memReader.ReadSingle();

                            fieldRotations = new List<FieldRotation>();

                            alpha = 0;
                            beta = 0;
                            gamma = 0;

                            if (nBones > 0)
                            {
                                for (bi = 0; bi < nBones; bi++)
                                {
                                    alpha = memReader.ReadSingle();
                                    beta = memReader.ReadSingle();
                                    gamma = memReader.ReadSingle();

                                    fieldRotations.Add(new FieldRotation(alpha, beta, gamma));
                                }
                            }
                            else
                            {
                                fieldRotations.Add(new FieldRotation(alpha, beta, gamma));
                            }

                            frames.Add(new FieldFrame(rotationAlpha, rotationBeta, rotationGamma, 
                                                      translationX, translationY, translationZ, fieldRotations));
                        }
                    }
                }
            }

            public int CreateCompatibleFieldAnimation()
            {
                int iResult = 1;

                FieldFrame fframe;
                FieldRotation frotation;

                // Local vars
                short bi;
                //string last_joint;
                string[] joint_stack;
                int jsp;

                short stageIndex = 0;
                float hipArmAngle = 0, c1, c2;

                version = 1;
                nBones = fSkeleton.nBones;

                //joint_stack = new string[fSkeleton.nBones + 1];
                joint_stack = new string[fSkeleton.bones.Count + 1];
                
                jsp = 0;
                joint_stack[jsp] = fSkeleton.bones[0].joint_f;

                // Add frames
                nFrames = 1;

                frotation = new FieldRotation();
                frotation.alpha = 90;
                frotation.beta = 0;
                frotation.gamma = 180;

                fframe = new FieldFrame();
                fframe.rootRotationAlpha = 0;
                fframe.rootRotationBeta = 0;
                fframe.rootRotationGamma = 0;
                fframe.rootTranslationX = 0;
                fframe.rootTranslationY = 0;
                fframe.rootTranslationZ = 0;

                // Add List of frame rotations
                fframe.rotations = new List<FieldRotation> { frotation };
                //fframe.rotations.Add(frotation);

                // Add List of frame animations
                frames = new List<FieldFrame> { fframe };
                //frames.Add(fframe);

                // Add rotationOrder
                rotationOrder = new byte[3];

                // Add runtime_data
                runtime_data = new int[5];

                // Preprocess from 1 bone, we've just done bone 0
                //for (bi = 1; bi < fSkeleton.nBones; bi++)
                for (bi = 1; bi < fSkeleton.bones.Count; bi++)
                {
                    while (!(fSkeleton.bones[bi].joint_f == joint_stack[jsp]) && jsp > 0) jsp--;

                    if ((stageIndex < 7 && fSkeleton.bones[bi].joint_f == "hip") ||
                        (stageIndex >= 7 && fSkeleton.bones[bi].joint_f == "root"))
                        stageIndex++;

                    frotation = new FieldRotation();

                    switch (stageIndex)
                    {
                        case 1:
                            frotation.alpha = 0;
                            frotation.beta = 0;
                            frotation.gamma = 0;

                            if (bi == 2) stageIndex = 2;
                            break;

                        case 2:
                            frotation.alpha = -145;
                            frotation.beta = 0;
                            frotation.gamma = 0;

                            stageIndex = 3;
                            break;

                        case 3:
                            if (jsp > 1)
                            {
                                frotation.alpha = 0;
                                frotation.beta = 0;
                                frotation.gamma = 0;
                            }
                            else
                            {
                                c1 = (float)(fSkeleton.bones[1].len * 0.9);
                                if (c1 > fSkeleton.bones[bi].len)
                                    c1 = (float)(fSkeleton.bones[bi].len - (fSkeleton.bones[bi].len * 0.01));

                                c2 = (float)(Math.Sqrt(Math.Pow(fSkeleton.bones[bi].len, 2) - Math.Pow(c1, 2)));

                                hipArmAngle = (float)(Math.Atan(c2 / c1) / PI_180);

                                frotation.alpha = 0;
                                frotation.beta = -hipArmAngle;
                                frotation.gamma = 0;

                                stageIndex = 5;
                            }
                            break;

                        case 4:
                            c1 = (float)(fSkeleton.bones[1].len * 0.9);
                            if (c1 > fSkeleton.bones[bi].len)
                                c1 = (float)(fSkeleton.bones[bi].len - (fSkeleton.bones[bi].len * 0.01));

                            c2 = (float)(Math.Sqrt(Math.Pow(fSkeleton.bones[bi].len, 2) - Math.Pow(c1, 2)));

                            hipArmAngle = (float)(Math.Atan(c2 / c1) / PI_180);

                            frotation.alpha = 0;
                            frotation.beta = -hipArmAngle;
                            frotation.gamma = 0;

                            stageIndex = 5;
                            break;

                        case 5:
                            frotation.alpha = 0;
                            frotation.beta = -90 + hipArmAngle;
                            frotation.gamma = 180;

                            stageIndex = 6;
                            break;

                        case 6:
                            frotation.alpha = 0;
                            frotation.beta = 0;
                            frotation.gamma = 0;
                            break;

                        case 7:
                            frotation.alpha = 0;
                            frotation.beta = hipArmAngle;
                            frotation.gamma = 0;

                            stageIndex = 8;
                            break;

                        case 8:
                            frotation.alpha = 0;
                            frotation.beta = -hipArmAngle + 90;
                            frotation.gamma = 180;

                            stageIndex = 9;
                            break;

                        case 9:
                            frotation.alpha = 0;
                            frotation.beta = 0;
                            frotation.gamma = 0;
                            break;

                        case 10:
                            frotation.alpha = 0;
                            frotation.beta = 90;
                            frotation.gamma = 90;

                            stageIndex = 11;
                            break;

                        case 11:
                            frotation.alpha = 0;
                            frotation.beta = 60;
                            frotation.gamma = 0;

                            stageIndex = 12;
                            break;

                        case 12:
                            frotation.alpha = 0;
                            frotation.beta = 0;
                            frotation.gamma = 0;

                            stageIndex = 13;
                            break;

                        case 13:
                            frotation.alpha = -90;
                            frotation.beta = 0;
                            frotation.gamma = 0;
                            break;

                        case 14:
                            frotation.alpha = 0;
                            frotation.beta = -90;
                            frotation.gamma = -90;

                            stageIndex = 15;
                            break;

                        case 15:
                            frotation.alpha = 0;
                            frotation.beta = -60;
                            frotation.gamma = 0;

                            stageIndex = 16;
                            break;

                        case 16:
                            frotation.alpha = 0;
                            frotation.beta = 0;
                            frotation.gamma = 0;

                            stageIndex = 17;
                            break;

                        case 17:
                            frotation.alpha = -90;
                            frotation.beta = 0;
                            frotation.gamma = 0;
                            break;

                        case 18:
                            frotation.alpha = 90;
                            frotation.beta = 0;
                            frotation.gamma = 0;
                            break;

                        default:
                            frotation.alpha = 0;
                            frotation.beta = 0;
                            frotation.gamma = 0;
                            break;

                    }

                    frames[0].rotations.Add(frotation);

                    jsp++;
                    joint_stack[jsp] = fSkeleton.bones[bi].joint_i;
                }

                return iResult;
            }

        }

        public static int LoadAnimationFromDB(string strAnimFileName)
        {
            int iloadAnimationFromDBResult = 1;
            FieldAnimation tmpfAnimation;

            try
            {
                if (strAnimFileName != "")
                {
                    strGlobalFieldAnimationName = Path.GetFileName(strAnimFileName);

                    tmpfAnimation = new FieldAnimation(fSkeleton, strAnimFileName, true);
                    //FixFieldAnimation(fSkeleton, tmpfAnimation);

                    if (tmpfAnimation.nBones != fSkeleton.nBones && tmpfAnimation.nBones != 0)
                    {

                        // Ok. If loading the animation we have problems,
                        // we will create a compatible animation.
                        MessageBox.Show("The animation selected has different number of bones. " +
                                        "Using a compatible animation.", "Warning", MessageBoxButtons.OK);

                        strGlobalFieldAnimationName = "--";
                        fAnimation = new FieldAnimation(fSkeleton, "--", false);
                    }
                    else
                    {
                        fAnimation = tmpfAnimation;
                    }
                }
                else
                {
                    // Ok. If loading the animation we have problems,
                    // we will create a compatible animation.
                    MessageBox.Show("There is not selected animation (should not happen). " +
                                    " Using a compatible animation.", "Warning", MessageBoxButtons.OK);

                    strGlobalFieldAnimationName = "--";
                    fAnimation = new FieldAnimation(fSkeleton, "--", false);
                }
            }
            catch
            {
                iloadAnimationFromDBResult = -3;
            }

            return iloadAnimationFromDBResult;
        }


        public static int SearchFirstCompatibleFieldAnimationFileName(FieldSkeleton fieldSkeleton, string strFileFullPath, ref string strAnimationName)
        {
            int iResult = 1, iCounter = 0;
            bool bFound = false;
            string[] lstFieldAnimsFiles;

            try
            {
                strGlobalPathFieldAnimationFolder = strFileFullPath;

                if (strGlobalPathFieldAnimationFolder == "")
                {
                    if (strGlobalPathFieldSkeletonFolder != "")
                        strGlobalPathFieldAnimationFolder = strGlobalPathFieldSkeletonFolder;
                    else strGlobalPathFieldAnimationFolder = strGlobalPath;
                }

                lstFieldAnimsFiles = Directory.GetFiles(strGlobalPathFieldAnimationFolder, "*.A", SearchOption.TopDirectoryOnly);

                if (lstFieldAnimsFiles != null && lstFieldAnimsFiles.Length > 0)
                {
                    while (iCounter < lstFieldAnimsFiles.Length && !bFound)
                    {
                        if (SameFieldAnimNumBones(lstFieldAnimsFiles[iCounter], fieldSkeleton))
                        {
                            strAnimationName = Path.GetFileName(lstFieldAnimsFiles[iCounter]);

                            bFound = true;
                        }

                        iCounter++;
                    }
                }
            }
            catch (Exception e)
            {
                iResult = -1;
            }

            if (!bFound) strAnimationName = "DUMMY.A";

            return iResult;
        }

        public static bool IsBrokenFieldRotation(ref FieldRotation fRotation)
        {
            bool bIsBrokenFieldRotationResult = false;

            if (double.IsNaN(fRotation.alpha)) bIsBrokenFieldRotationResult = true;
            else if (fRotation.alpha > 9999f) bIsBrokenFieldRotationResult = true;
            else if (fRotation.alpha < -9999f) bIsBrokenFieldRotationResult = true;

            if (double.IsNaN(fRotation.beta)) bIsBrokenFieldRotationResult = true;
            else if (fRotation.beta > 9999f) bIsBrokenFieldRotationResult = true;
            else if (fRotation.beta < -9999f) bIsBrokenFieldRotationResult = true;

            if (double.IsNaN(fRotation.gamma)) bIsBrokenFieldRotationResult = true;
            else if (fRotation.gamma > 9999f) bIsBrokenFieldRotationResult = true;
            else if (fRotation.gamma < -9999f) bIsBrokenFieldRotationResult = true;

            return bIsBrokenFieldRotationResult;
        }

        public static bool IsBrokenFieldFrame(ref FieldFrame fFrame, int nBones)
        {
            int bi;
            bool bIsBrokenFieldFrameResult = false;
            FieldRotation tmpFieldRotation;

            if (double.IsNaN(fFrame.rootTranslationX)) bIsBrokenFieldFrameResult = true;
            if (double.IsNaN(fFrame.rootTranslationY)) bIsBrokenFieldFrameResult = true;
            if (double.IsNaN(fFrame.rootTranslationZ)) bIsBrokenFieldFrameResult = true;

            if (double.IsNaN(fFrame.rootRotationAlpha)) bIsBrokenFieldFrameResult = true;
            else if (fFrame.rootRotationAlpha > 9999f) bIsBrokenFieldFrameResult = true;
            else if (fFrame.rootRotationAlpha < -9999f) bIsBrokenFieldFrameResult = true;

            if (double.IsNaN(fFrame.rootRotationBeta)) bIsBrokenFieldFrameResult = true;
            else if (fFrame.rootRotationBeta > 9999f) bIsBrokenFieldFrameResult = true;
            else if (fFrame.rootRotationBeta < -9999f) bIsBrokenFieldFrameResult = true;

            if (double.IsNaN(fFrame.rootRotationGamma)) bIsBrokenFieldFrameResult = true;
            else if (fFrame.rootRotationGamma > 9999f) bIsBrokenFieldFrameResult = true;
            else if (fFrame.rootRotationGamma < -9999f) bIsBrokenFieldFrameResult = true;

            if (!bIsBrokenFieldFrameResult)
            {
                for (bi = 0; bi < nBones; bi++)
                {
                    tmpFieldRotation = fFrame.rotations[bi];
                    bIsBrokenFieldFrameResult = IsBrokenFieldRotation(ref tmpFieldRotation);
                    fFrame.rotations[bi] = tmpFieldRotation;

                    if (bIsBrokenFieldFrameResult) break;
                }
            }

            return bIsBrokenFieldFrameResult;
        }

        public static bool IsFrameBrokenFieldAnimation(ref FieldAnimation fAnimation, int frame_index)
        {
            bool bIsFrameBrokenFieldAnimationResult;
            
            FieldFrame tmpFrame = fAnimation.frames[frame_index];
            bIsFrameBrokenFieldAnimationResult = IsBrokenFieldFrame(ref tmpFrame, fAnimation.nBones);
            fAnimation.frames[frame_index] = tmpFrame;

            return bIsFrameBrokenFieldAnimationResult;
        }

        public static bool RemoveFrameFieldAnimation(ref FieldAnimation fAnimation, int frame_index)
        {
            bool bRemoveFrameFieldAnimationResult = false;
            int fi;

            if (fAnimation.nFrames > 1)
            {
                for (fi = frame_index; fi < fAnimation.nFrames - 1; fi++)
                {
                    fAnimation.frames[fi] = fAnimation.frames[fi + 1];
                }

                fAnimation.frames.RemoveAt(fAnimation.nFrames - 1);
                fAnimation.nFrames--;

                bRemoveFrameFieldAnimationResult = true;
            }

            return bRemoveFrameFieldAnimationResult;
        }

        public static void FixFieldAnimation(FieldSkeleton fSkeleton, FieldAnimation fAnimation)
        {
            int fi, numBrokenFrames;

            fi = 0;

            while (fi < fAnimation.nFrames)
            {
                numBrokenFrames = 0;

                while (IsFrameBrokenFieldAnimation(ref fAnimation, fi))
                {
                    RemoveFrameFieldAnimation(ref fAnimation, fi);
                    numBrokenFrames++;
                }

                if (numBrokenFrames > 0)
                {
                    if (fi == 0) fi++;
                    else
                    {
                        InterpolateFramesFieldAnimation(ref fSkeleton, ref fAnimation, fi - 1, numBrokenFrames);
                        fi += numBrokenFrames;
                    }
                }
                else fi++;
            }
        }


        //  ---------------------------------------------------------------------------------------------------
        //  ============================================== SAVING =============================================
        //  ---------------------------------------------------------------------------------------------------
        public static void WriteFieldAnimation(FieldAnimation fAnimation, string fileName)
        {
            int fi, bi;
            FileStream writeStream;

            using (writeStream = new FileStream(fileName, FileMode.Create))
            {
                using (BinaryWriter fileWriter = new BinaryWriter(writeStream))
                {
                    fileWriter.Write(fAnimation.version);
                    fileWriter.Write(fAnimation.nFrames);
                    fileWriter.Write(fAnimation.nBones);

                    //  Is there any animation using another rotation order?
                    fAnimation.rotationOrder[0] = 1;
                    fAnimation.rotationOrder[1] = 0;
                    fAnimation.rotationOrder[2] = 2;

                    fileWriter.Write(fAnimation.rotationOrder);
                    fileWriter.Write(fAnimation.unused);

                    foreach (uint lItem in fAnimation.runtime_data) fileWriter.Write(lItem);

                    for (fi = 0; fi < fAnimation.nFrames; fi++)
                    {
                        fileWriter.Write(fAnimation.frames[fi].rootRotationAlpha);
                        fileWriter.Write(fAnimation.frames[fi].rootRotationBeta);
                        fileWriter.Write(fAnimation.frames[fi].rootRotationGamma);

                        fileWriter.Write(fAnimation.frames[fi].rootTranslationX);
                        fileWriter.Write(fAnimation.frames[fi].rootTranslationY);
                        fileWriter.Write(fAnimation.frames[fi].rootTranslationZ);

                        for (bi = 0; bi < fAnimation.nBones; bi++)
                        {
                            fileWriter.Write(fAnimation.frames[fi].rotations[bi].alpha);
                            fileWriter.Write(fAnimation.frames[fi].rotations[bi].beta);
                            fileWriter.Write(fAnimation.frames[fi].rotations[bi].gamma);
                        }
                    }
                }
            }
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ========================================== INTERPOLATION ==========================================
        //  ---------------------------------------------------------------------------------------------------
        public static void GetTwoFieldFramesInterpolation(FieldSkeleton fSkeleton, FieldFrame fFrameA, FieldFrame fFrameB,
                                                          float alpha, ref FieldFrame fFrameOut)
        {
            int bi, jsp;

            string[] joint_stack;
            Quaternion[] rotations_stack_a;
            Quaternion[] rotations_stack_b;
            Quaternion[] rotations_stack_acum;

            Quaternion quat_a;
            Quaternion quat_b;
            Quaternion quat_acum_a = new Quaternion();
            Quaternion quat_acum_b = new Quaternion();
            Quaternion quat_acum_inverse;
            Quaternion quat_interp;
            Quaternion quat_interp_final = new Quaternion();

            Point3D euler_res;
            float alpha_inv;
            int nBones;
            double[] mat = new double[16];

            quat_a = GetQuaternionFromEulerYXZr(fFrameA.rootRotationAlpha, fFrameA.rootRotationBeta, fFrameA.rootRotationGamma);
            quat_b = GetQuaternionFromEulerYXZr(fFrameB.rootRotationAlpha, fFrameB.rootRotationBeta, fFrameB.rootRotationGamma);

            quat_interp = QuaternionSlerp2(ref quat_a, ref quat_b, alpha);

            BuildMatrixFromQuaternion(quat_interp, ref mat);
            euler_res = GetEulerYXZrFromMatrix(mat);

            fFrameOut.rootRotationAlpha = euler_res.y;
            fFrameOut.rootRotationBeta = euler_res.x;
            fFrameOut.rootRotationGamma = euler_res.z;

            alpha_inv = 1f - alpha;

            fFrameOut.rootTranslationX = fFrameA.rootTranslationX * alpha_inv + fFrameB.rootTranslationX * alpha;
            fFrameOut.rootTranslationY = fFrameA.rootTranslationY * alpha_inv + fFrameB.rootTranslationY * alpha;
            fFrameOut.rootTranslationZ = fFrameA.rootTranslationZ * alpha_inv + fFrameB.rootTranslationZ * alpha;


            //nBones = fSkeleton.nBones + 1;
            nBones = fSkeleton.bones.Count + 1;

            joint_stack = new string[nBones + 1];
            rotations_stack_a = new Quaternion[nBones];
            rotations_stack_b = new Quaternion[nBones];
            rotations_stack_acum = new Quaternion[nBones];
            fFrameOut.rotations = new List<FieldRotation>();

            jsp = 1;

            rotations_stack_a[0] = quat_a;
            rotations_stack_b[0] = quat_b;
            rotations_stack_acum[0] = quat_interp;

            joint_stack[jsp] = fSkeleton.bones[0].joint_f;

            //for (bi = 0; bi < fSkeleton.nBones; bi++)
            for (bi = 0; bi < fSkeleton.bones.Count; bi++)
            {
                while (!(fSkeleton.bones[bi].joint_f == joint_stack[jsp]) && jsp > 0) jsp--;

                quat_a = GetQuaternionFromEulerYXZr(fFrameA.rotations[bi].alpha, fFrameA.rotations[bi].beta, fFrameA.rotations[bi].gamma);
                MultiplyQuaternions(rotations_stack_a[jsp - 1], quat_a, ref quat_acum_a);
                NormalizeQuaternion(ref quat_acum_a);
                rotations_stack_a[jsp] = quat_acum_a;

                quat_b = GetQuaternionFromEulerYXZr(fFrameB.rotations[bi].alpha, fFrameB.rotations[bi].beta, fFrameB.rotations[bi].gamma);
                MultiplyQuaternions(rotations_stack_b[jsp - 1], quat_b, ref quat_acum_b);
                NormalizeQuaternion(ref quat_acum_b);
                rotations_stack_b[jsp] = quat_acum_b;

                quat_interp = QuaternionSlerp2(ref quat_acum_a, ref quat_acum_b, alpha);
                rotations_stack_acum[jsp] = quat_interp;
                quat_acum_inverse = GetQuaternionConjugate(ref rotations_stack_acum[jsp - 1]);
                MultiplyQuaternions(quat_acum_inverse, quat_interp, ref quat_interp_final);
                NormalizeQuaternion(ref quat_interp_final);
                BuildMatrixFromQuaternion(quat_interp_final, ref mat);
                euler_res = GetEulerYXZrFromMatrix(mat);

                fFrameOut.rotations.Add(new FieldRotation(euler_res.y, euler_res.x, euler_res.z));

                jsp++;
                joint_stack[jsp] = fSkeleton.bones[bi].joint_i;
            }
        }

        public static void InterpolateFramesFieldAnimation(ref FieldSkeleton fSkeleton, ref FieldAnimation fAnimation,
                                                           int base_frame, int numInterpolatedFrames)
        {
            int fi;
            float alpha;
            //FieldFrame tmpFrameA, tmpFrameB, tmpFrameOut;
            FieldFrame tmpFrame = new FieldFrame();

            // Create new frames
            fAnimation.nFrames += numInterpolatedFrames;
            for (fi = 0; fi < numInterpolatedFrames; fi++) fAnimation.frames.Add(new FieldFrame());

            // Move the original frames into their new positions
            for (fi = fAnimation.nFrames - 1; fi > base_frame + numInterpolatedFrames; fi--)
            {
                fAnimation.frames[fi] = fAnimation.frames[fi - numInterpolatedFrames];
            }

            // Interpolate the new Frames
            for (fi = 1; fi <= numInterpolatedFrames; fi++)
            {
                alpha = (float)fi / (numInterpolatedFrames + 1);

                GetTwoFieldFramesInterpolation(fSkeleton, fAnimation.frames[base_frame], fAnimation.frames[base_frame + numInterpolatedFrames + 1],
                                               alpha, ref tmpFrame);
                fAnimation.frames[base_frame + fi] = tmpFrame;

            }
        }

        public static void InterpolateFieldAnimation(ref FieldSkeleton fSkeleton, ref FieldAnimation fAnimation,
                                                     int numInterpolatedFrames, bool isLoop)
        {
            int i, fi, ifi, nextElemDiff, frameOffset, baseFinalFrame;
            float alpha;

            FieldFrame fFrame = new FieldFrame();

            nextElemDiff = numInterpolatedFrames + 1;
            frameOffset = 0;

            if (!isLoop) frameOffset = numInterpolatedFrames;

            //  If the animation has 1 frame we will do nothing
            if (fAnimation.nFrames == 1) return;

            //  Create new frames
            fAnimation.nFrames = fAnimation.nFrames * (numInterpolatedFrames + 1) - frameOffset;

            for (i = 0; i < fAnimation.nFrames; i++) fAnimation.frames.Add(fFrame);

            //  Move the original frames into their new positions
            for (fi = fAnimation.nFrames - (1 + numInterpolatedFrames - frameOffset); fi >= 0; fi -= nextElemDiff)
                fAnimation.frames[fi] = fAnimation.frames[fi / (numInterpolatedFrames + 1)];

            //  Interpolate the new frames
            for (fi = 0; fi <= fAnimation.nFrames - (1 + nextElemDiff + numInterpolatedFrames - frameOffset); fi += nextElemDiff)
            {
                for (ifi = 1; ifi <= numInterpolatedFrames; ifi++)
                {
                    alpha = (float)ifi / (numInterpolatedFrames + 1);

                    fFrame = fAnimation.frames[fi + ifi];
                    GetTwoFieldFramesInterpolation(fSkeleton, fAnimation.frames[fi],
                                                   fAnimation.frames[fi + numInterpolatedFrames + 1], alpha, ref fFrame);
                    fAnimation.frames[fi + ifi] = fFrame;
                }
            }

            //  Looped animation
            if (isLoop)
            {
                baseFinalFrame = fAnimation.nFrames - numInterpolatedFrames - 1;

                for (ifi = 1; ifi <= numInterpolatedFrames; ifi++)
                {
                    alpha = (float)ifi / (numInterpolatedFrames + 1);

                    fFrame = fAnimation.frames[baseFinalFrame + ifi];
                    GetTwoFieldFramesInterpolation(fSkeleton, fAnimation.frames[baseFinalFrame],
                                                   fAnimation.frames[0], alpha, ref fFrame);
                    fAnimation.frames[baseFinalFrame + ifi] = fFrame;
                }
            }
        }




            //  ---------------------------------------------------------------------------------------------------
            //  ================================= COPY FIELD ANIMATION (OR PARTS) =================================
            //  ---------------------------------------------------------------------------------------------------
            public static FieldFrame CopyfFrame(FieldFrame fFrameIn)
        {
            FieldFrame fFrameOut;
            FieldRotation tmpfRotation;
            int ri;

            fFrameOut = new FieldFrame();

            fFrameOut.rootRotationAlpha = fFrameIn.rootRotationAlpha;
            fFrameOut.rootRotationBeta = fFrameIn.rootRotationBeta;
            fFrameOut.rootRotationGamma = fFrameIn.rootRotationGamma;

            fFrameOut.rootTranslationX = fFrameIn.rootTranslationX;
            fFrameOut.rootTranslationY = fFrameIn.rootTranslationY;
            fFrameOut.rootTranslationZ = fFrameIn.rootTranslationZ;

            fFrameOut.rotations = new List<FieldRotation>();

            for (ri = 0; ri < fFrameIn.rotations.Count; ri++)
            {
                tmpfRotation = new FieldRotation();
                tmpfRotation.alpha = fFrameIn.rotations[ri].alpha;
                tmpfRotation.beta = fFrameIn.rotations[ri].beta;
                tmpfRotation.gamma = fFrameIn.rotations[ri].gamma;

                fFrameOut.rotations.Add(tmpfRotation);
            }

            return fFrameOut;
        }

        public static FieldAnimation CopyfAnimation(FieldAnimation fAnimSource)
        {
            FieldAnimation fAnimDest;

            fAnimDest = new FieldAnimation();

            fAnimDest.version = fAnimSource.version;
            fAnimDest.nFrames = fAnimSource.nFrames;
            fAnimDest.nBones = fAnimSource.nBones;

            fAnimDest.rotationOrder = new byte[3];
            fAnimSource.rotationOrder.CopyTo(fAnimDest.rotationOrder, 0);

            fAnimDest.unused = fAnimSource.unused;

            fAnimDest.runtime_data = new int[5];
            fAnimSource.runtime_data.CopyTo(fAnimDest.runtime_data, 0);

            fAnimDest.strFieldAnimationFile = fAnimSource.strFieldAnimationFile;

            fAnimDest.frames = new List<FieldFrame>();
            foreach (FieldFrame itmfFrame in fAnimSource.frames) fAnimDest.frames.Add(CopyfFrame(itmfFrame));

            //for (fi = 0; fi < fAnimSource.frames.Count; fi++)
            //{
            //    tmpfFrame = new FieldFrame();
            //    CopyfFrame(ref tmpfFrame, fAnimSource.frames[fi]);
            //    fAnimDest.frames.Add(tmpfFrame);
            //}

            return fAnimDest;
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ============================================= HELPERS =============================================
        //  ---------------------------------------------------------------------------------------------------
        public static int GetNumFieldBones(string fileAnimFullPath)
        {
            int numBones = -1;
            byte[] fileBuffer;
            string strFileName;

            strFileName = Path.GetFileName(fileAnimFullPath).ToUpper();

            // First check if exists
            if (!File.Exists(fileAnimFullPath))
            {
                MessageBox.Show("The Field Animation file " + strFileName + " does not exists. Animation not loaded.",
                                "Error");
                return numBones;
            }

            // Read All *.A Model file into memory for get numBones
            fileBuffer = File.ReadAllBytes(fileAnimFullPath);

            if (fileBuffer.Length > 0 && Path.GetFileNameWithoutExtension(fileAnimFullPath).Length > 0)
            {
                using (var fileMemory = new MemoryStream(fileBuffer))
                {
                    using (var memReader = new BinaryReader(fileMemory))
                    {
                        memReader.BaseStream.Position = 0x8;
                        numBones = memReader.ReadInt32();
                    }
                }
            }

            return numBones;
        }

        public static bool SameFieldAnimNumBones(string strFileFieldAnimation, FieldSkeleton fSkeleton)
        {
            int numFieldAnimBones;

            numFieldAnimBones = GetNumFieldBones(strFileFieldAnimation);

            if ((fSkeleton.nBones == 1 && numFieldAnimBones == 0) || 
                (fSkeleton.nBones == numFieldAnimBones)) return true;
            else return false;
        }


    }
}
