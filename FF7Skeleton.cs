using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KimeraCS
{
    
    using Defines;

    using static FrmSkeletonEditor;

    using static FF7FieldSkeleton;
    using static FF7FieldAnimation;

    using static FF7BattleSkeleton;
    using static FF7BattleAnimationsPack;
    using static FF7BattleAnimation;

    using static FF7FieldRSDResource;
    using static FF7PModel;
    using static FF7TMDModel;

    using static FF7TEXTexture;

    using static Lighting;

    using static OpenGL32;
    using static GDI32;
    using static Utils;
    using static FileTools;


    public static class FF7Skeleton
    {
        // This will tell to all the tool which type of skeleton/model we have loaded
        // Possible values:
        // -1: Error (file not exists, error opening file...)
        //  0: Field P Model
        //  1: Battle P Model
        //  2: Magic P Model
        //  3: Field Skeleton
        //  4: Battle Skeleton
        //  5: Magic Skeleton
        //  6: 3DS Model
        public const int K_P_FIELD_MODEL = 0;
        public const int K_P_BATTLE_MODEL = 1;
        public const int K_P_MAGIC_MODEL = 2;
        public const int K_HRC_SKELETON = 3;
        public const int K_AA_SKELETON = 4;
        public const int K_MAGIC_SKELETON = 5;
        public const int K_3DS_MODEL = 6;

        public static int modelType = -1;

        // Animation constants for skeleton
        public const int K_FRAME_BONE_ROTATION = 0;
        public const int K_FRAME_ROOT_ROTATION = 1;
        public const int K_FRAME_ROOT_TRANSLATION = 2;

        // Global vars
        public static FieldSkeleton fSkeleton;
        public static FieldAnimation fAnimation;

        public static BattleSkeleton bSkeleton;
        public static BattleAnimationsPack bAnimationsPack;

        public static PModel fPModel;
        public static TMDModel mTMDModel;

        public static bool IsTMDModel;
        public static bool IsRSDResource;


        //
        // Global Skeleton/Model functions/procedures
        //
        public static int LoadSkeleton(string strFileName, bool loadGeometryQ)
        {
            int iloadSkeletonResult = 1;

            try
            {

                // First we destroy the previous loaded Field Skeleton.
                if (bLoaded && (modelType >= 3 && modelType <= 5))
                {
                    if (DestroySkeleton() != 1) iloadSkeletonResult = -2;
                }

                modelType = GetSkeletonType(strFileName);

                if (modelType >= 3 && modelType <= 5)
                {
                    // LOAD Skeleton
                    // We load the Field Skeleton into memory.
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            string strAnimationName = "";

                            // Field Skeleton (.hrc)
                            fSkeleton = new FieldSkeleton(strFileName, loadGeometryQ);

                            // We try to find some compatible Field Animation for the Field Skeleton.
                            // If there is no compatible field animation we have this var:   strGlobalFieldAnimationName = ""
                            iloadSkeletonResult = SearchFirstCompatibleFieldAnimationFileName(fSkeleton, 
                                                                                              Path.GetDirectoryName(strFileName), 
                                                                                              ref strAnimationName);

                            if (iloadSkeletonResult == 1)
                                fAnimation = new FieldAnimation(fSkeleton,
                                                                Path.GetDirectoryName(strFileName) + "\\" + strAnimationName, 
                                                                strAnimationName != "DUMMY.A");

                            break;

                        case K_AA_SKELETON:
                            // Battle Skeleton (aa)
                            bSkeleton = new BattleSkeleton(strFileName, CanHaveLimitBreak(Path.GetFileNameWithoutExtension(strFileName).ToUpper()), true);

                            // Normally we will have the ??DA file with the Animation Pack.
                            // Location Battle Models has NOT ??DA file.
                            // But editing models, it is possible we work without it. So, we will make something
                            // similiar as we did with Field Models, but we will check if ??DA file for the model exists.
                            bAnimationsPack = new BattleAnimationsPack(bSkeleton, strFileName);

                            break;

                        case K_MAGIC_SKELETON:
                            // Magic Skeleton (.d)
                            bSkeleton = new BattleSkeleton(strFileName, true);

                            // Normally we will have the *.A00 file with the Animation Pack.
                            // But editing models, it is possible we work without it. So, we will make something
                            // similiar as we did with Field Models, but we will check if *.A00 file for the model exists.
                            bAnimationsPack = new BattleAnimationsPack(bSkeleton, strFileName);

                            break;
                    }
                }
                else
                {
                    iloadSkeletonResult = 0;  // No known skeleton
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK);
                iloadSkeletonResult = -1;  // Error loading skeleton
            }

            return iloadSkeletonResult;
        }

        public static int LoadFieldSkeletonFromDB(string strFileName, 
                                                  string strAnimFileName, 
                                                  bool loadGeometryQ)
        {
            int iloadSkeletonResult = 1;

            try
            {

                // First we destroy the previous loaded Field Skeleton.
                if (bLoaded && (modelType >= 3 && modelType <= 5))
                {
                    if (DestroySkeleton() != 1) iloadSkeletonResult = -2;
                }

                modelType = GetSkeletonType(strFileName);

                if (modelType >= 3 && modelType <= 5)
                {
                    // LOAD Skeleton
                    switch (modelType)
                    {
                        case K_HRC_SKELETON:
                            // We load the Field Skeleton into memory.

                            // Field Skeleton (.hrc)
                            fSkeleton = new FieldSkeleton(strFileName, loadGeometryQ);

                            iloadSkeletonResult = LoadAnimationFromDB(strAnimFileName);
                            break;

                        case K_AA_SKELETON:
                            // Battle Skeleton (aa)
                            bSkeleton = new BattleSkeleton(strFileName, CanHaveLimitBreak(Path.GetFileNameWithoutExtension(strFileName).ToUpper()), true);

                            // Normally we will have the ??DA file with the Animation Pack.
                            // Location Battle Models has NOT ??DA file.
                            // But editing models, it is possible we work without it. So, we will make something
                            // similiar as we did with Field Models, but we will check if ??DA file for the model exists.
                            bAnimationsPack = new BattleAnimationsPack(bSkeleton, strFileName);
                            break;

                        case K_MAGIC_SKELETON:
                            // Magic Skeleton (.d)
                            bSkeleton = new BattleSkeleton(strFileName, true);

                            // Normally we will have the ??DA file with the Animation Pack.
                            // Location Battle Models has NOT ??DA file.
                            // But editing models, it is possible we work without it. So, we will make something
                            // similiar as we did with Field Models, but we will check if ??DA file for the model exists.
                            bAnimationsPack = new BattleAnimationsPack(bSkeleton, strFileName);
                            break;
                    }
                }
                else
                {
                    iloadSkeletonResult = 0;  // No known skeleton
                }
            }
            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;

                iloadSkeletonResult = -1;  // Error loading skeleton
            }

            return iloadSkeletonResult;
        }

        public static int WriteSkeleton(string strFileName)
        {
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();
            bool compileMultiPBones = false;
            BattleFrame tmpwpFrame;

            int isaveSkeletonResult = 0;

            try
            {
                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        ComputeFieldBoundingBox(fSkeleton, fAnimation.frames[iCurrentFrameScroll], ref p_min, ref p_max);

                        SetCameraAroundModel(ref p_min, ref p_max, 0, 0, -2 * ComputeSceneRadius(p_min, p_max),
                                             0, 0, 0, 1, 1, 1);

                        SetLights();

                        if (MessageBox.Show("Merge multi PModels bones in a single file?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            compileMultiPBones = true;

                        ApplyFieldChanges(ref fSkeleton, fAnimation.frames[iCurrentFrameScroll], compileMultiPBones);

                        WriteFieldSkeleton(ref fSkeleton, strFileName);
                        //  WriteFieldAnimation(fAnimation, saveFile.FileName);
                        CreateDListsFromFieldSkeleton(ref fSkeleton);

                        isaveSkeletonResult = 1;
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        if (bSkeleton.IsBattleLocation && ianimIndex > 0) ianimIndex = 0;

                        ComputeBattleBoundingBox(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[iCurrentFrameScroll], ref p_min, ref p_max);

                        SetCameraAroundModel(ref p_min, ref p_max, 0, 0, -2 * ComputeSceneRadius(p_min, p_max),
                                             0, 0, 0, 1, 1, 1);

                        SetLights();

                        tmpwpFrame = new BattleFrame();
                        if (bSkeleton.nsWeaponsAnims > 0) tmpwpFrame = bAnimationsPack.WeaponAnimations[0].frames[0];

                        ApplyBattleChanges(ref bSkeleton, bAnimationsPack.SkeletonAnimations[0].frames[0], tmpwpFrame);

                        if (modelType == K_AA_SKELETON)
                        {
                            // Battle model (*AA)
                            WriteBattleSkeleton(ref bSkeleton, strFileName);
                        }
                        else
                        {
                            // Magic model (*.D)
                            WriteMagicSkeleton(ref bSkeleton, strFileName);
                        }

                        //  WriteBattleAnimationsPack(bAnimationsPack, strFileNameAnimationsPack);
                        //  CheckWriteBattleAnimationsPack(bAnimationsPack, strFileNameAnimationsPack);
                        CreateDListsFromBattleSkeleton(ref bSkeleton);

                        isaveSkeletonResult = 1;
                        break;
                }
            }
            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;

                isaveSkeletonResult = -1;
            }

            return isaveSkeletonResult;
        }

        public static int DestroySkeleton()
        {
            int iDestroySkeletonResult = 1;

            try
            {
                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        DestroyFieldSkeleton(fSkeleton);
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        DestroyBattleSkeleton(bSkeleton);
                        break;
                }

                // Clear PanelModel PictureBox
                bLoaded = false;
                ClearPanel();
                glFlush();
                SwapBuffers(panelModelDC);
            }
            catch
            {
                iDestroySkeletonResult = -1;
            }

            return iDestroySkeletonResult;
        }

        public static int GetSkeletonType(string strFileName)
        {
            int iSkeletonType = -1;
            string tmpFileName;

            if (strFileName.Length > 0)
            {
                switch (Path.GetExtension(strFileName).ToUpper())
                {
                    case ".HRC":
                        iSkeletonType = 3;

                        strGlobalFieldSkeletonName = Path.GetFileNameWithoutExtension(strFileName).ToUpper();
                        strGlobalFieldSkeletonFileName = Path.GetFileName(strFileName).ToUpper();

                        break;

                    case "":
                        tmpFileName = Path.GetFileNameWithoutExtension(strFileName).ToUpper();
                        if (tmpFileName.Length > 2)
                        {
                            if (tmpFileName[tmpFileName.Length - 1] == 'A' && tmpFileName[tmpFileName.Length - 2] == 'A')
                            {
                                strGlobalBattleSkeletonName = Path.GetFileNameWithoutExtension(strFileName).ToUpper();
                                strGlobalBattleSkeletonFileName = Path.GetFileName(strFileName).ToUpper();

                                iSkeletonType = 4;
                            }
                        }
                        break;

                    case ".D":
                        iSkeletonType = 5;

                        strGlobalMagicSkeletonName = Path.GetFileNameWithoutExtension(strFileName).ToUpper();
                        strGlobalMagicSkeletonFileName = Path.GetFileName(strFileName).ToUpper();

                        break;
                }
            }

            return iSkeletonType;
        }

        public static int WritePModel(string strFileName)
        {
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            int isaveModelResult = 0;

            try
            {
                switch (modelType)
                {
                    case K_P_FIELD_MODEL:
                    case K_P_BATTLE_MODEL:
                    case K_P_MAGIC_MODEL:
                    case K_3DS_MODEL:
                        glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
                        glPushMatrix();

                        SetCameraModelViewQuat(fPModel.repositionX, fPModel.repositionY, fPModel.repositionZ,
                                               fPModel.rotationQuaternion,
                                               fPModel.resizeX, fPModel.resizeY, fPModel.resizeZ);

                        ApplyPChanges(ref fPModel, Path.GetExtension(strFileName).ToUpper() != ".P");

                        ComputePModelBoundingBox(fPModel, ref p_min, ref p_max);
                        SetCameraAroundModel(ref p_min, ref p_max,
                                             0, 0, -2 * ComputeSceneRadius(p_min, p_max),
                                             0, 0, 0, 1, 1, 1);

                        SetLights();

                        if (glIsEnabled(GLCapability.GL_LIGHTING)) ApplyCurrentVColors(ref fPModel);

                        glPopMatrix();
                        WriteGlobalPModel(ref fPModel, strFileName);
                        CreateDListsFromPModel(ref fPModel);

                        isaveModelResult = 1;
                        break;
                }
            }
            catch
            {
                isaveModelResult = -1;
            }

            return isaveModelResult;
        }

        public static int LoadRSDResourceModel(string strRSDFolder, string strRSDName)
        {
            int iLoadRSDResourceModelResult = 0;
            string strfAnimation = "";

            FieldBone tmpfBone;
            FieldRSDResource tmpfRSDResource;
            List<TEX> textures_pool = new List<TEX>();

            try
            {
                // Create fSkeleton with 1 bone
                fSkeleton = new FieldSkeleton()
                {
                    nBones = 1,
                    fileName = strRSDName,
                    name = strRSDName,

                    bones = new List<FieldBone>(),
                };

                // Load RSD Resource
                tmpfBone = new FieldBone()
                {
                    len = 1,
                    joint_f = "null",
                    joint_i = "root",
                    nResources = 1,
                    fRSDResources = new List<FieldRSDResource>(),

                    resizeX = 1,
                    resizeY = 1,
                    resizeZ = 1,
                };

                tmpfRSDResource = new FieldRSDResource(strRSDName, ref textures_pool, strRSDFolder);
                tmpfBone.fRSDResources.Add(tmpfRSDResource);

                fSkeleton.bones.Add(tmpfBone);

                // Create the Dummy animation (normally individual RSD Resource has not own animation)
                fAnimation = new FieldAnimation(fSkeleton,
                                                strRSDFolder + "\\" + strfAnimation,
                                                false);

                modelType = K_HRC_SKELETON;
                IsRSDResource = true;
            }
            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;

                MessageBox.Show("There has been some error loading RSD Resource: " + strRSDName + ".", "Error");
                
                iLoadRSDResourceModelResult = -1;
            }

            return iLoadRSDResourceModelResult;
        }



        ////////////////////////////////////////////////////////////////////////////////////////////////
        // Global Animation functions/procedures
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public static int WriteAnimation(string strFileName)
        {
            int isaveAnimationResult = 0;

            try
            {
                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        WriteFieldAnimation(fAnimation, strFileName);

                        isaveAnimationResult = 1;
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        isaveAnimationResult = WriteBattleAnimationsPack(ref bAnimationsPack, strFileName);
                        break;
                }
            }
            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;

                isaveAnimationResult = -1;
            }

            return isaveAnimationResult;
        }

        public static bool NumAnimFramesIsOne()
        {
            bool iNumAnimFramesIsOneResult = false;
            switch (modelType)
            {
                case K_HRC_SKELETON:
                    if (fAnimation.nFrames == 1) iNumAnimFramesIsOneResult = true;
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    if (bAnimationsPack.SkeletonAnimations[ianimIndex].numFramesShort == 1) iNumAnimFramesIsOneResult = true;
                    break;
            }

            return iNumAnimFramesIsOneResult;
        }

        public static int ReadFrameData(string strFileName, bool bMerge)
        {
            int iinputFrameData = 0;

            try
            {
                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        ReadFieldFrameData(fSkeleton, ref fAnimation, strFileName, bMerge);

                        iinputFrameData = 1;
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        //iinputFrameData = WriteBattleFrameDataPack(ref bAnimationsPack, strFileName);
                        break;
                }
            }
            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;

                iinputFrameData = -1;
            }

            return iinputFrameData;
        }

        public static int WriteFrameData(string strFileName)
        {
            int ioutputFrameData = 0;

            try
            {
                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        WriteFieldFrameData(fSkeleton, fAnimation, strFileName);

                        ioutputFrameData = 1;
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        //ioutputFrameData = WriteBattleAnimationsPack(ref bAnimationsPack, strFileName);
                        break;
                }
            }
            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;

                ioutputFrameData = -1;
            }

            return ioutputFrameData;
        }

        public static int ReadFrameDataSelective(string strFileName)
        {
            int iinputFrameData = 0;

            try
            {
                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        ReadFieldFrameDataSelective(fSkeleton, ref fAnimation, strFileName);

                        iinputFrameData = 1;
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        //iinputFrameData = WriteBattleFrameDataPack(ref bAnimationsPack, strFileName);
                        break;
                }
            }
            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;

                iinputFrameData = -1;
            }

            return iinputFrameData;
        }



    }
}
