//  STRUCT BATTLE ANIMATION ??DA
//
//  MAIN ??DA HEADER
//  4 bytes                 NumAnims
//  NumAnims*               ANIM HEADER
//  
//  ANIM HEADER
//  4 bytes                 NumBones + 1
//  4 bytes                 numFrames
//  4 bytes                 blockSize;
//  ANIM HEADER SHORT (Array has Padding to 4 bytes)
//  2 bytes                 numFramesShort
//  2 bytes                 blockSizeShort
//  1 byte                  key
//  blockSizeShort* byte    framesRawData;
//  n * bytes               padding at 4 bytes      padding calculation = (4 - (blockSizeShort + 5) % 4)) % 4    - the padding is of 4 bytes
//                                                  padding reading = (blockSize - blockSizeShort) - 5




using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KimeraCS
{

    using Defines;

    using static FF7Skeleton;
    using static FF7BattleSkeleton;
    using static FF7BattleAnimation;

    using static Utils;
    using static FileTools;

    public class FF7BattleAnimationsPack
    {
        //  Battle animations notes by L.Spiro and Qhimm:
        //  http://wiki.qhimm.com/FF7/Battle/Battle_Animation_(PC)
        //  Weapon animations notes by seb:
        //  http://forums.qhimm.com/index.php?topic=7185.0

        public const int MAX_BATTLEANIMATION_SIZE = 0xFFFFF;

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct BattleAnimationsPack
        {

            public int nAnimations;
            public int nbSkeletonAnims;
            public int nbWeaponAnims;
            public List<BattleAnimation> SkeletonAnimations;
            public List<BattleAnimation> WeaponAnimations;
            public bool IsLimit;

            public BattleAnimationsPack(BattleSkeleton bSkeleton, string strFileName)
            {
                string strBattleAnimPackFileName;
                string strAnimsPackFullFileName;                             

                nAnimations = 0;
                nbSkeletonAnims = 0;
                nbWeaponAnims = 0;
                IsLimit = false;

                SkeletonAnimations = new List<BattleAnimation>();
                WeaponAnimations = new List<BattleAnimation>();

                if (bSkeleton.IsBattleLocation)
                {
                    CreateEmptyBattleAnimationsPack(ref this, bSkeleton.nBones + 1);
                    strGlobalBattleAnimationName = "--";
                }
                else
                {
                    switch (modelType)
                    {
                        case K_AA_SKELETON:
                            if (Path.GetExtension(strFileName).Length == 4)
                                strBattleAnimPackFileName = Path.GetFileName(strFileName).ToUpper();
                            else
                            {
                                // Let's check if we are opening all the model or only loading an animation
                                if (Path.GetFileName(strFileName).ToUpper().EndsWith("AA"))
                                    strBattleAnimPackFileName = Path.GetFileNameWithoutExtension(strFileName).Substring(0, 2).ToUpper() + "DA";
                                else
                                    strBattleAnimPackFileName = Path.GetFileNameWithoutExtension(strFileName).ToUpper();
                            }


                            strGlobalBattleAnimationName = strBattleAnimPackFileName;
                            break;

                        default:
                            strBattleAnimPackFileName = Path.GetFileNameWithoutExtension(strFileName).ToUpper() + ".A00";

                            strGlobalMagicAnimationName = strBattleAnimPackFileName;
                            break;
                    }


                    strAnimsPackFullFileName = Path.GetDirectoryName(strFileName) + "\\" + strBattleAnimPackFileName;

                    if (File.Exists(strAnimsPackFullFileName))
                        if (bSkeleton.CanHaveLimitBreak && Path.GetExtension(strFileName).Length == 4)
                        {
                            IsLimit = true;
                            LoadBattleAnimationsPack(strAnimsPackFullFileName, bSkeleton.nBones, 8, 8, ref this);
                        }
                        else
                            LoadBattleAnimationsPack(strAnimsPackFullFileName, bSkeleton.nBones,
                                                     bSkeleton.nsSkeletonAnims, bSkeleton.nsWeaponsAnims, ref this);
                    else
                        CreateCompatibleBattleAnimationsPack(bSkeleton, ref this);
                }
            }
        }

        public static void LoadBattleAnimationsPack(string strAnimsPackFullFileName, int nsSkeletonBones,
                                                    int nsSkeletonAnims, int nsWeaponsAnims, ref BattleAnimationsPack bAnimationsPack)
        {
            int ai;
            byte[] fileBuffer;
            //  Debug.Print "Loadng animations pack " + fileName
            //  Debug.Print "Reading pack "; fileName

            // We put into memory the file
            fileBuffer = File.ReadAllBytes(strAnimsPackFullFileName);

            try
            {
                // Work with memory file
                using (var fileMemory = new MemoryStream(fileBuffer))
                {
                    using (var memReader = new BinaryReader(fileMemory))
                    {
                        bAnimationsPack.nAnimations = memReader.ReadInt32();

                        if (nsSkeletonAnims > bAnimationsPack.nAnimations)
                        {
                            MessageBox.Show("Warning. The number of animations of the Battle Animation Pack " +
                                            "is lower than the number of animations of the Battle Skeleton " +
                                            "header. FIXING.", "Warning", MessageBoxButtons.OK);

                            nsSkeletonAnims = bAnimationsPack.nAnimations;

                            if (!bAnimationsPack.IsLimit) bSkeleton.nsSkeletonAnims = nsSkeletonAnims;
                        }


                        bAnimationsPack.nbSkeletonAnims = nsSkeletonAnims;
                        bAnimationsPack.nbWeaponAnims = nsWeaponsAnims;

                        bAnimationsPack.SkeletonAnimations = new List<BattleAnimation>();
                        bAnimationsPack.WeaponAnimations = new List<BattleAnimation>();
                        //  Debug.Print "Loading "; .NumAnimations; " animations."

                        for (ai = 0; ai < bAnimationsPack.nbSkeletonAnims; ai++)
                        {
                            //  Debug.Print "anim "; ai
                            //  Debug.Print "Body Animation "; Str$(ai)
                            //iVectorBones = 1;
                            //if (nBones > 1) iVectorBones = nBones + 1;
                            //bAnimationsPack.SkeletonAnimations.Add(new BattleAnimation(memReader, fileBuffer, iVectorBones));
                            bAnimationsPack.SkeletonAnimations.Add(new BattleAnimation(memReader, nsSkeletonBones));
                        }

                        for (ai = 0; ai < bAnimationsPack.nbWeaponAnims; ai++)
                        {
                            //  Debug.Print "anim "; ai
                            //  Debug.Print "Weapon Animation "; Str$(ai)
                            //iVectorBones = 1;
                            //if (nBones > 1) iVectorBones = nBones + 1;
                            //bAnimationsPack.WeaponAnimations.Add(new BattleAnimation(memReader, fileBuffer, 1));
                            bAnimationsPack.WeaponAnimations.Add(new BattleAnimation(memReader, 1));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;

                MessageBox.Show("Error reading Battle Animation Pack file " + Path.GetFileName(strAnimsPackFullFileName) + ".",
                                "Error", MessageBoxButtons.OK);
            }

        }



        //  ---------------------------------------------------------------------------------------------------
        //  ================================== CREATE EMPTY BATTLE ANIMATION ==================================
        //  ---------------------------------------------------------------------------------------------------
        public static void CreateEmptyBattleAnimationsPack(ref BattleAnimationsPack bAnimationsPack, int nBones)
        {
            BattleAnimation tmpbAnimation;

            bAnimationsPack.nAnimations = 1;
            bAnimationsPack.nbSkeletonAnims = 1;
            bAnimationsPack.nbWeaponAnims = 1;

            tmpbAnimation = new BattleAnimation();
            CreateEmptyBattleAnimation(ref tmpbAnimation, nBones);

            bAnimationsPack.SkeletonAnimations.Add(tmpbAnimation);
            bAnimationsPack.WeaponAnimations.Add(tmpbAnimation);
        }



        //  ---------------------------------------------------------------------------------------------------
        //  =============================== CREATE COMPATIBLE BATTLE ANIMATION ================================
        //  ---------------------------------------------------------------------------------------------------
        public static void CreateCompatibleBattleAnimationsPack(BattleSkeleton bSkeleton, ref BattleAnimationsPack bAnimationsPack)
        {
            BattleAnimation tmpbAnimation;

            bAnimationsPack.nAnimations = 1;
            bAnimationsPack.nbSkeletonAnims = 1;
            bAnimationsPack.nbWeaponAnims = 1;

            tmpbAnimation = new BattleAnimation();
            CreateCompatibleBattleAnimation(bSkeleton, ref tmpbAnimation);
            bAnimationsPack.SkeletonAnimations.Add(tmpbAnimation);

            tmpbAnimation = new BattleAnimation();
            CreateCompatibleBattleAnimation(bSkeleton, ref tmpbAnimation);
            bAnimationsPack.WeaponAnimations.Add(tmpbAnimation);
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ============================================== SAVING =============================================
        //  ---------------------------------------------------------------------------------------------------
        public static void NormalizeBattleAnimationsPack(ref BattleAnimationsPack bAnimationsPack)
        {
            int ai, bi;
            BattleFrameBone tmpbFrameBone;
            BattleAnimation tmpbAnimation;

            for (ai = 0; ai < bAnimationsPack.nbSkeletonAnims; ai++)
            {
                if (bAnimationsPack.SkeletonAnimations[ai].blockSize >= 11 &&
                    bAnimationsPack.SkeletonAnimations[ai].numFramesShort > 0)
                {
                    for (bi = 0; bi < bAnimationsPack.SkeletonAnimations[ai].frames[0].bones.Count; bi++)
                    {

                        tmpbFrameBone = bAnimationsPack.SkeletonAnimations[ai].frames[0].bones[bi];
                        tmpbFrameBone.alpha = NormalizeAngle180(tmpbFrameBone.alpha);
                        tmpbFrameBone.beta = NormalizeAngle180(tmpbFrameBone.beta);
                        tmpbFrameBone.gamma = NormalizeAngle180(tmpbFrameBone.gamma);
                        bAnimationsPack.SkeletonAnimations[ai].frames[0].bones[bi] = tmpbFrameBone;
                    }

                    tmpbAnimation = bAnimationsPack.SkeletonAnimations[ai];
                    NormalizeBattleAnimation(ref tmpbAnimation);
                    bAnimationsPack.SkeletonAnimations[ai] = tmpbAnimation;
                }
            }

            for (ai = 0; ai < bAnimationsPack.nbWeaponAnims; ai++)
            {
                if (bAnimationsPack.WeaponAnimations[ai].blockSize >= 11 && 
                    bAnimationsPack.WeaponAnimations[ai].numFramesShort > 0)
                {
                    tmpbFrameBone = bAnimationsPack.WeaponAnimations[ai].frames[0].bones[0];
                    tmpbFrameBone.alpha = NormalizeAngle180(tmpbFrameBone.alpha);
                    tmpbFrameBone.beta = NormalizeAngle180(tmpbFrameBone.beta);
                    tmpbFrameBone.gamma = NormalizeAngle180(tmpbFrameBone.gamma);
                    bAnimationsPack.WeaponAnimations[ai].frames[0].bones[0] = tmpbFrameBone;
                }

                tmpbAnimation = bAnimationsPack.WeaponAnimations[ai];
                NormalizeBattleAnimation(ref tmpbAnimation);
                bAnimationsPack.WeaponAnimations[ai] = tmpbAnimation;
            }
        }

        public static int WriteBattleAnimationsPack(ref BattleAnimationsPack bAnimationsPack, string strFileName)
        {
            int ai;
            byte[] fileBuffer = new byte[MAX_BATTLEANIMATION_SIZE * bAnimationsPack.nAnimations];  // We DON'T know the size of the Battle Animation if there are new frames.
                                                                     // This value is estimate.
            int ifileBufferSize = 0;
            bool bBlockOverSize = false;
            BattleAnimationsPack tmpbAnimationsPack;

            // Let's make a backup of the animations.
            tmpbAnimationsPack = CopybAnimationsPack(bAnimationsPack);

            //  Since we're using signed data there is no way we can store values outside the[-180°, 180°]
            //  (shoudln't matter though, due to angular equivalences)
            //  Normalize just to be safe
            NormalizeBattleAnimationsPack(ref bAnimationsPack);

            //  Debug.Print "Writting pack "; fileName

            //  Write Battle Animations Pack
            using (MemoryStream writeStream = new MemoryStream(fileBuffer))
            {
                using (BinaryWriter memWriter = new BinaryWriter(writeStream))
                {
                    memWriter.Write(bAnimationsPack.nAnimations);
                    ifileBufferSize = 4;

                    ai = 0;
                    while (ai < bAnimationsPack.nbSkeletonAnims && !bBlockOverSize)
                    {
                        bBlockOverSize = 
                            WriteBattleAnimation(memWriter, bAnimationsPack.SkeletonAnimations[ai], ref ifileBufferSize);

                        ai++;
                    }

                    if (!bBlockOverSize && modelType == K_AA_SKELETON)
                    {
                        ai = 0;
                        while (ai < bAnimationsPack.nbWeaponAnims && !bBlockOverSize)
                        {
                            //  Debug.Print "Weapon Animation "; Str$(ai)
                            bBlockOverSize =
                                WriteBattleAnimation(memWriter, bAnimationsPack.WeaponAnimations[ai], ref ifileBufferSize);

                            ai++;
                        }
                    }
                }
            }

            if (bBlockOverSize)
            {
                MessageBox.Show("The size of the Battle Animation is higher than the maximum " +
                                "size (65535) expected by FF7 format. Animation not saved.", "Error", MessageBoxButtons.OK);
                
                // Restore Battle Animations Pack 
                bAnimationsPack = CopybAnimationsPack(tmpbAnimationsPack);

                return -2;
            }
            else
            {
                Array.Resize(ref fileBuffer, ifileBufferSize);
                File.WriteAllBytes(strFileName, fileBuffer);

                return 1;
            }
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ========================================== INTERPOLATION ==========================================
        //  ---------------------------------------------------------------------------------------------------
        public static void InterpolateBattleAnimationsPack(ref BattleSkeleton bSkeleton, ref BattleAnimationsPack bAnimationsPack,
                                                           int numInterpolatedFrames, bool bisLoopQ)
        {
            int ai;
            BattleAnimation tmpbAnimation;

            for (ai = 0; ai < bAnimationsPack.nbSkeletonAnims; ai++)
            {
                if (bAnimationsPack.SkeletonAnimations[ai].numFramesShort > 1)
                {
                    tmpbAnimation = bAnimationsPack.SkeletonAnimations[ai];
                    InterpolateBattleSkeletonAnimation(ref bSkeleton, ref tmpbAnimation, numInterpolatedFrames, bisLoopQ);
                    bAnimationsPack.SkeletonAnimations[ai] = tmpbAnimation;

                    if (ai < bAnimationsPack.nbWeaponAnims && bSkeleton.nWeapons > 0)
                    {
                        tmpbAnimation = bAnimationsPack.WeaponAnimations[ai];
                        InterpolateBattleWeaponAnimation(ref tmpbAnimation, numInterpolatedFrames, bisLoopQ,
                                                         bAnimationsPack.SkeletonAnimations[ai].numFrames, 
                                                         bAnimationsPack.SkeletonAnimations[ai].numFramesShort);
                        bAnimationsPack.WeaponAnimations[ai] = tmpbAnimation;
                    }
                }
            }
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ================================== COPY BATTLE ANIMATIONS PACK ====================================
        //  ---------------------------------------------------------------------------------------------------
        public static BattleAnimationsPack CopybAnimationsPack(BattleAnimationsPack bAnimationsPackIn)
        {
            BattleAnimationsPack bAnimationsPackOut;

            bAnimationsPackOut.nAnimations = bAnimationsPackIn.nAnimations;
            bAnimationsPackOut.nbSkeletonAnims = bAnimationsPackIn.nbSkeletonAnims;
            bAnimationsPackOut.nbWeaponAnims = bAnimationsPackIn.nbWeaponAnims;
            bAnimationsPackOut.IsLimit = bAnimationsPackIn.IsLimit;

            bAnimationsPackOut.SkeletonAnimations = new List<BattleAnimation>();
            foreach (BattleAnimation itmbAnimation in bAnimationsPackIn.SkeletonAnimations) bAnimationsPackOut.SkeletonAnimations.Add(CopybAnimation(itmbAnimation));

            bAnimationsPackOut.WeaponAnimations = new List<BattleAnimation>();
            foreach (BattleAnimation itmbAnimation in bAnimationsPackIn.WeaponAnimations) bAnimationsPackOut.WeaponAnimations.Add(CopybAnimation(itmbAnimation));

            return bAnimationsPackOut;
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ============================================= HELPERS =============================================
        //  ---------------------------------------------------------------------------------------------------
        public static int GetNumBattleBones(string fileAnimFullPath)
        {
            int numBones = 0;
            byte[] fileBuffer;

            // First check if exists
            if (!File.Exists(fileAnimFullPath))
            {
                MessageBox.Show("The Battle Animations Pack file " + Path.GetFileName(fileAnimFullPath) + " does not exists. " +
                                "Animation not loaded for getting the number of bones.",
                                "Error");
                return numBones;
            }

            // Read All *DA Model file into memory for get numBones
            fileBuffer = File.ReadAllBytes(fileAnimFullPath);

            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = 0x4;
                    numBones = memReader.ReadInt32();
                }
            }

            return numBones;
        }

        public static bool SameBattleAnimNumBones(string strFileAnimationsPack, BattleSkeleton bSkeleton)
        {
            int tmpnBonesAnim;

            // The number of animations varies, depending on the type of model loaded.
            // If Model is Enemy    (skeletonType 0?), num bones of the Animations Pack = num bones of Skeleton + 1.
            // If Model is Summon   (skeletonType 0?), num bones of the Animations Pack = num bones of Skeleton + 1.
            // If Model is Location (skeletonType 1?), num bones of the Animations Pack = 0 (no bones). It should not get in here.
            // If Model is PC       (skeletonType 2?), num bones of the Animations Pack = num bones of Skeleton + 1 (because of the weapon).
            tmpnBonesAnim = GetNumBattleBones(strFileAnimationsPack);

            if (tmpnBonesAnim > 1 )
                if (bSkeleton.skeletonType == 0 ||bSkeleton.skeletonType == 2 || modelType == K_MAGIC_SKELETON) 
                    tmpnBonesAnim--;

            return tmpnBonesAnim == bSkeleton.nBones; 
        }

        public static bool IsLimitAnimation (string strbAnimationFileName)
        {
            foreach (STLimitsRegister itmLimit in lstBattleLimitsAnimations)
                if (itmLimit.lstLimitsAnimations.Contains(strbAnimationFileName.ToUpper()))
                    return true;

            return false;
        }


    }
}
