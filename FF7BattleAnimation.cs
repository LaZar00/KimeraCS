using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KimeraCS
{

    using static FF7Skeleton;
    using static FF7BattleSkeleton;

    using static Utils;
    using static OpenGL32;


    public class FF7BattleAnimation
    {

        public struct BattleFrameBone
        {
            public short accumAlphaS;
            public short accumBetaS;
            public short accumGammaS;

            public int accumAlpha;
            public int accumBeta;
            public int accumGamma;

            public float alpha;
            public float beta;
            public float gamma;            

            public BattleFrameBone(float alphaIn, float betaIn, float gammaIn)
            {
                accumAlphaS = 0;
                accumBetaS = 0;
                accumGammaS = 0;

                accumAlpha = 0;
                accumBeta = 0;
                accumGamma = 0;

                alpha = alphaIn;
                beta = betaIn;
                gamma = gammaIn;
            }
        }

        public struct BattleFrame
        {
            public int startX;
            public int startY;
            public int startZ;
            public List<BattleFrameBone> bones;

            //public BattleFrame(byte[] framesRawData, byte key, int blockSizeShort, int nBones, ref ushort numFramesShort)
            //{
            //    startX = 0;
            //    startY = 0;
            //    startZ = 0;

            //    bones = new List<BattleFrameBone>();
            //}
        }

        public struct BattleAnimation
        {

            //  ANIM HEADER
            public int nBones;                //  Number of bones for the model + 1 (root transformation). Unreliable.
            public int numFrames;             //  Number of frames (conservative). Usually wrong (smaller than the actual number).
            public int blockSize;             //  This is sizeOf(blockSizeShort + 5 bytes of ANIM HEADER SHORT + padding bytes (at 4 bytes)).

            //  ANIM HEADER SHORT(Array has Padding to 4 bytes)
            public ushort numFramesShort;     //  Number of frames. Usually wrong (higher than the actual number).
            public ushort blockSizeShort;     //  OLD COMMENT: 'Don't use this field EVER.It would be interpreted as a signed value.
                                              //  OLD VB6: int animationLengthLong;    //    AnimationLengthLong As Long     'This isn't part of the actual structure, it's used just to overcome the lack of unsigned shorts support
            public byte key;
            public byte[] framesRawData;
            public byte[] padding4bytes;

            //public bool missingNumFramesShort;    //  The animation has no secondary frames count. Only RSAA/RSDA seems to use it.
            public List<BattleFrame> frames;
            //public byte[] unknownData;

            //public BattleAnimation(BinaryReader memReader, byte[] fileBuffer, int iVectorBonesLen)
            public BattleAnimation(BinaryReader memReader, int nsSkeletonBones)
            {
                BattleFrame tmpbFrame;
                int offsetBit, fi;

                //  ANIM HEADER
                nBones = memReader.ReadInt32();

                // We will check the number of bones of Skeleton and the number of bones of the animation
                // The number of bones of the skeleton will have precedence if it is one '1'.
                if (nsSkeletonBones == 1) nBones = 1;

                numFrames = memReader.ReadInt32();
                blockSize = memReader.ReadInt32();

                //  ANIM HEADER SHORT(Array has Padding to 4 bytes) - This is partial init of vars. We will read data from fileBuffer.
                numFramesShort = 0;
                blockSizeShort = 0;
                key = 0;

                framesRawData = null;
                padding4bytes = null;

                //missingNumFramesShort = false;
                frames = new List<BattleFrame>();

                if (blockSize > 11)
                {
                    //  ANIM HEADER SHORT(Array has Padding to 4 bytes)
                    numFramesShort = memReader.ReadUInt16();

                    //  Let's use this to correct the missing numFramesShort in vanilla RSAA/Frog Enemy(frame 14) ushort value
                    if (blockSize - 5 == numFramesShort)
                    {
                        blockSizeShort = numFramesShort;
                        blockSize += 2;
                        numFramesShort = (ushort)numFrames;
                    }
                    else
                    {
                        blockSizeShort = memReader.ReadUInt16();
                    }

                    key = memReader.ReadByte();

                    framesRawData = new byte[blockSizeShort];
                    framesRawData = memReader.ReadBytes(blockSizeShort);

                    padding4bytes = new byte[(blockSize - blockSizeShort) - 5];
                    padding4bytes = memReader.ReadBytes((blockSize - blockSizeShort) - 5);

                    frames = new List<BattleFrame>();

                    // Now we need to work with the animRawDataBlock and get the frames
                    tmpbFrame = new BattleFrame();

                    offsetBit = 0;
                    //  Debug.Print "   -First frame at byte" + Str$(offset + 17)
                    //  Debug.Print "   Frame 0"
                    ProcessBattleUncompressedFrame(framesRawData, ref offsetBit, key, nBones, ref tmpbFrame);
                    frames.Add(tmpbFrame);

                    for (fi = 1; fi < numFramesShort; fi++)
                    {
                        //  If we ran out of data while reading the frame, it means this frame doesn't
                        //last_offsetBit = offsetBit;
                        tmpbFrame = new BattleFrame();
                        if (!ProcessBattleFrame(framesRawData, ref offsetBit, key, nBones, ref tmpbFrame, frames[fi - 1]))
                        {
                            numFramesShort = (ushort)fi;
                            break;
                        }
                        frames.Add(tmpbFrame);
                    }
                }
                else
                {
                    framesRawData = new byte[blockSize];
                    framesRawData = memReader.ReadBytes(blockSize);
                }
            }
        }


        //  ---------------------------------------------------------------------------------------------------
        //  =========================== READ COMPRESSED FRAME ANIMATION FUNCTIONS =============================
        //  ---------------------------------------------------------------------------------------------------
        public static int ProcessBattleFrameBoneRotationDelta(byte[] framesRawData, ref int offsetBit, byte key)
        {
            int dLen, iVal, itmpSignVal;
            int iProcessBattleFrameBoneRotationDeltaResult = 0;

            if ((short)GetBitBlockVUnsigned(framesRawData, 1, ref offsetBit) == 1)
            {
                dLen = (short)GetBitBlockVUnsigned(framesRawData, 3, ref offsetBit);

                switch (dLen)
                {
                    case 0:
                        //  Minimum bone rotation decrement
                        iVal = -1;
                        break;

                    case 7:
                        //  Just like the first frame
                        iVal = (short)GetBitBlockV(framesRawData, 12 - key, ref offsetBit);
                        break;

                    default:
                        iVal = (short)GetBitBlockV(framesRawData, dLen, ref offsetBit);

                        //  Invert the value of the last bit
                        itmpSignVal = (int)Math.Pow(2, dLen - 1);

                        if (iVal < 0) iVal -= itmpSignVal;
                        else iVal += itmpSignVal;
                        break;
                }

                //  Convert to 12-bits value
                iVal *= (int)Math.Pow(2, key);
                iProcessBattleFrameBoneRotationDeltaResult = iVal;
            }

            return iProcessBattleFrameBoneRotationDeltaResult;
        }

        //  For bone rotations of all the other frames
        public static void ProcessBattleFrameBone(byte[] framesRawData, ref int offsetBit, byte key, ref BattleFrameBone bBone, BattleFrameBone lastbBone)
        {
            bBone.accumAlphaS = (short)(lastbBone.accumAlphaS + ProcessBattleFrameBoneRotationDelta(framesRawData, ref offsetBit, key));
            bBone.accumBetaS = (short)(lastbBone.accumBetaS + ProcessBattleFrameBoneRotationDelta(framesRawData, ref offsetBit, key));
            bBone.accumGammaS = (short)(lastbBone.accumGammaS + ProcessBattleFrameBoneRotationDelta(framesRawData, ref offsetBit, key));

            bBone.accumAlpha = (bBone.accumAlphaS < 0 ? bBone.accumAlphaS + 0x1000 : bBone.accumAlphaS);
            bBone.accumBeta = (bBone.accumBetaS < 0 ? bBone.accumBetaS + 0x1000 : bBone.accumBetaS);
            bBone.accumGamma = (bBone.accumGammaS < 0 ? bBone.accumGammaS + 0x1000 : bBone.accumGammaS);

            bBone.alpha = GetDegreesFromRaw(bBone.accumAlpha, 0);
            bBone.beta = GetDegreesFromRaw(bBone.accumBeta, 0);
            bBone.gamma = GetDegreesFromRaw(bBone.accumGamma, 0);            
        }

        public static bool ProcessBattleFrame(byte[] framesRawData, ref int offsetBit, byte key, int nBones, ref BattleFrame bFrame, BattleFrame lastbFrame)
        {
            int bi, oi, offsetLen = 0;
            bool bProcessBattleFrameResult;
            BattleFrameBone tmpbFrameBone;

            try
            {
                for (oi = 0; oi < 3; oi++)
                {
                    switch ((short)GetBitBlockV(framesRawData, 1, ref offsetBit) & 1)
                    {
                        case 0:
                            offsetLen = 7;
                            break;

                        case 1:
                            offsetLen = 16;
                            break;

                        default:
                            //  Debug.Print "What?!"
                            break;
                    }

                    switch (oi)
                    {
                        case 0:
                            bFrame.startX = (short)GetBitBlockV(framesRawData, offsetLen, ref offsetBit) + lastbFrame.startX;
                            break;

                        case 1:
                            bFrame.startY = (short)GetBitBlockV(framesRawData, offsetLen, ref offsetBit) + lastbFrame.startY;
                            break;

                        case 2:
                            bFrame.startZ = (short)GetBitBlockV(framesRawData, offsetLen, ref offsetBit) + lastbFrame.startZ;
                            break;

                        default:
                            //  Debug.Print "What?!"
                            break;
                    }
                }

                //  Debug.Print "       Position delta "; Str$(.X_start); ", "; Str$(.Y_start); ", "; Str$(.Z_start)
                bFrame.bones = new List<BattleFrameBone>();
                for (bi = 0; bi < nBones; bi++)
                {
                    //  Debug.Print "       Bone "; Str$(bi)
                    tmpbFrameBone = new BattleFrameBone();
                    ProcessBattleFrameBone(framesRawData, ref offsetBit, key, ref tmpbFrameBone, lastbFrame.bones[bi]);
                    bFrame.bones.Add(tmpbFrameBone);
                }

                //  Debug.Print "diff: "; offsetBit - aux
                bProcessBattleFrameResult = true;
            }
            catch
            {
                bProcessBattleFrameResult = false;
            }

            return bProcessBattleFrameResult;
        }
    


        //  ---------------------------------------------------------------------------------------------------
        //  ========================== READ UNCOMPRESSED FRAME ANIMATION FUNCTIONS ============================
        //  ---------------------------------------------------------------------------------------------------
        //  For raw rotations
        public static short ProcessBattleUncompressedFrameBoneRotation(byte[] framesRawData, ref int offsetBit, byte key)
        {
            int iVal;

            iVal = (short)GetBitBlockV(framesRawData, 12 - key, ref offsetBit);

            //  Convert to 12-bit value
            iVal *= (short)Math.Pow(2, key);

            return (short)iVal;
        }

        //  For bone rotations of the first frame
        public static void ProcessBattleUncompressedFrameBone(byte[] framesRawData, ref int offsetBit, byte key, ref BattleFrameBone bBone)
        {
            bBone.accumAlphaS = ProcessBattleUncompressedFrameBoneRotation(framesRawData, ref offsetBit, key);
            bBone.accumBetaS = ProcessBattleUncompressedFrameBoneRotation(framesRawData, ref offsetBit, key);
            bBone.accumGammaS = ProcessBattleUncompressedFrameBoneRotation(framesRawData, ref offsetBit, key);

            bBone.accumAlpha = (bBone.accumAlphaS < 0 ? bBone.accumAlphaS + 0x1000 : bBone.accumAlphaS);
            bBone.accumBeta = (bBone.accumBetaS < 0 ? bBone.accumBetaS + 0x1000 : bBone.accumBetaS);
            bBone.accumGamma = (bBone.accumGammaS < 0 ? bBone.accumGammaS + 0x1000 : bBone.accumGammaS);

            bBone.alpha = GetDegreesFromRaw(bBone.accumAlpha, 0);
            bBone.beta = GetDegreesFromRaw(bBone.accumBeta, 0);
            bBone.gamma = GetDegreesFromRaw(bBone.accumGamma, 0);
        }

        public static void ProcessBattleUncompressedFrame(byte[] framesRawData, ref int offsetBit, byte key, int nBones, ref BattleFrame bFrame)
        {
            //int bi, tmpOffsetBit;
            int bi;
            BattleFrameBone bBone;

            //tmpOffsetBit = offsetBit;

            //  .BonesVectorLength = BonesVectorLength ' + IIf(NumBones = 1, 0, 1)
            //  NumBones = IIf(NumBones = 2, 1, NumBones)  'Some single bone models have bones counter of 2 instead of 1, so adjust the value for convenience.

            bFrame.startX = (short)GetBitBlockV(framesRawData, 16, ref offsetBit);
            bFrame.startY = (short)GetBitBlockV(framesRawData, 16, ref offsetBit);
            bFrame.startZ = (short)GetBitBlockV(framesRawData, 16, ref offsetBit);

            bFrame.bones = new List<BattleFrameBone>();

            for (bi = 0; bi < nBones; bi++)
            {
                bBone = new BattleFrameBone();
                //  Debug.Print "       Bone "; Str$(bi)
                ProcessBattleUncompressedFrameBone(framesRawData, ref offsetBit, key, ref bBone);
                bFrame.bones.Add(bBone);
            }
            //  Debug.Print "diff: "; offsetBit - aux
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ============================================== SAVING =============================================
        //  ---------------------------------------------------------------------------------------------------
        public static void NormalizeBattleAnimationFrameBoneComponent(ref float fNextValue, float fValue)
        {
            float fDelta;

            fDelta = fNextValue - fValue;

            if (Math.Abs(fDelta) > 180f || fDelta == 180f)
            {
                fDelta = NormalizeAngle180(fDelta);

                fNextValue = fValue + fDelta;

                //if (fNextValue - fValue >= 180f) 
                //    MessageBox.Show("Should not happen! (NormalizeBattleAnimationFrameBoneComponent)", "Error", MessageBoxButtons.OK);
            }
        }

        public static void NormalizeBattleAnimation(ref BattleAnimation bAnimation)
        {
            int fi, bi;
            BattleFrameBone tmpbFrameBone;

            for (fi = 0; fi < bAnimation.numFramesShort - 1; fi++)
            {
                for (bi = 0; bi < bAnimation.frames[fi].bones.Count; bi++)
                {
                    tmpbFrameBone = bAnimation.frames[fi + 1].bones[bi];
                    NormalizeBattleAnimationFrameBoneComponent(ref tmpbFrameBone.alpha, bAnimation.frames[fi].bones[bi].alpha);
                    NormalizeBattleAnimationFrameBoneComponent(ref tmpbFrameBone.beta, bAnimation.frames[fi].bones[bi].beta);
                    NormalizeBattleAnimationFrameBoneComponent(ref tmpbFrameBone.gamma, bAnimation.frames[fi].bones[bi].gamma);
                    bAnimation.frames[fi + 1].bones[bi] = tmpbFrameBone;
                }
            }
        }

        //  For raw/bone rotations of the first frame
        public static void WriteBattleUncompressedFrame(ref byte[] bAnimationBuffer, ref int offsetBit, int key, BattleFrame bFrame)
        {
            int bi;

            //  Debug.Print "       Position delta "; Str$(.X_start); ", "; Str$(.Y_start); ", "; Str$(.Z_start)

            PutBitBlockV(ref bAnimationBuffer, 16, ref offsetBit, bFrame.startX);
            PutBitBlockV(ref bAnimationBuffer, 16, ref offsetBit, bFrame.startY);
            PutBitBlockV(ref bAnimationBuffer, 16, ref offsetBit, bFrame.startZ);

            for (bi = 0; bi < bFrame.bones.Count; bi++)
            {
                //  Debug.Print "       Bone "; Str$(bi)
                //  Debug.Print "           Angle delta (U) "; Str$(.alpha); ", "; Str$(.Beta); ", "; Str$(.Gamma)
                PutBitBlockV(ref bAnimationBuffer, 12 - key, ref offsetBit, GetRawFromDegrees(bFrame.bones[bi].alpha, key));
                PutBitBlockV(ref bAnimationBuffer, 12 - key, ref offsetBit, GetRawFromDegrees(bFrame.bones[bi].beta, key));
                PutBitBlockV(ref bAnimationBuffer, 12 - key, ref offsetBit, GetRawFromDegrees(bFrame.bones[bi].gamma, key));
            }
        }

        public static void WriteBattleFrameBoneRotationDelta(ref byte[] bAnimationBuffer, ref int offsetBit, byte key, int iValue)
        {
            int dLen, iValueOut;
            bool bFoundQ;

            //  Remove sign to prevent bad rounding on shift
            iValueOut = iValue & (int)(Math.Pow(2, 12 - key) - 1);

            if (iValueOut == 0)
            {
                PutBitBlockV(ref bAnimationBuffer, 1, ref offsetBit, 0);
            }
            else
            {
                PutBitBlockV(ref bAnimationBuffer, 1, ref offsetBit, 1);

                if (iValueOut == -(int)Math.Pow(2, key))
                {
                    //  Minimum subtraction given the key precision
                    PutBitBlockV(ref bAnimationBuffer, 3, ref offsetBit, 0);
                }
                else
                {
                    //  Find the lowest data length that can hold the requiered precision.
                    dLen = 1;
                    bFoundQ = false;

                    while (!bFoundQ && dLen < 7)
                    {
                        bFoundQ = (iValueOut & ~((int)Math.Pow(2, dLen) - 1)) == 0;
                        dLen += 1;
                    }
                    dLen = bFoundQ ? dLen - 1 : 7;

                    //  Write data length
                    PutBitBlockV(ref bAnimationBuffer, 3, ref offsetBit, dLen);

                    if (bFoundQ)
                    {
                        //  Write compressed (dLength < 7)
                        iValueOut = InvertBitInteger(iValueOut, dLen - 1);
                        PutBitBlockV(ref bAnimationBuffer, dLen, ref offsetBit, iValueOut);
                    }
                    else
                    {
                        //  Write raw (dLength = 7)
                        PutBitBlockV(ref bAnimationBuffer, 12 - key, ref offsetBit, iValueOut);
                    }
                }
            }
        }

        //  For bone rotations of all the other frames
        public static void WriteBattleFrameBone(ref byte[] bAnimationBuffer, ref int offsetBit, 
                                                byte key, BattleFrameBone bFrameBone, BattleFrameBone bLastFrameBone )  //  , ref Point3D UP3DAnimationCarry);
        {

            int rawDiffX, rawDiffY, rawDiffZ;

            rawDiffX = GetRawFromDegrees(bFrameBone.alpha, key) - GetRawFromDegrees(bLastFrameBone.alpha, key);
            rawDiffY = GetRawFromDegrees(bFrameBone.beta, key) - GetRawFromDegrees(bLastFrameBone.beta, key);
            rawDiffZ = GetRawFromDegrees(bFrameBone.gamma, key) - GetRawFromDegrees(bLastFrameBone.gamma, key);

            WriteBattleFrameBoneRotationDelta(ref bAnimationBuffer, ref offsetBit, key, rawDiffX);
            WriteBattleFrameBoneRotationDelta(ref bAnimationBuffer, ref offsetBit, key, rawDiffY);
            WriteBattleFrameBoneRotationDelta(ref bAnimationBuffer, ref offsetBit, key, rawDiffZ);
        }

        public static void WriteBattleFrame(ref byte[] bAnimationBuffer, ref int offsetBit, byte key, BattleFrame bFrame, BattleFrame bLastFrame) //  , ref Point3D[] UP3DAnimationCarry)
        {
            int bi, oi, offsetLen, offsetDelta;

            //  Debug.Print "       Position delta "; Str$(.X_start - LastFrame.X_start); ", "; Str$(.Y_start - LastFrame.Y_start); ", "; Str$(.Z_start - LastFrame.Z_start)
            offsetDelta = 0;

            for (oi = 0; oi < 3; oi++)
            {
                switch (oi)
                {
                    case 0:
                        offsetDelta = bFrame.startX - bLastFrame.startX;
                        break;

                    case 1:
                        offsetDelta = bFrame.startY - bLastFrame.startY;
                        break;

                    case 2:
                        offsetDelta = bFrame.startZ - bLastFrame.startZ;
                        break;

                    default:
                        MessageBox.Show("What?!", "Error", MessageBoxButtons.OK);
                        break;
                }

                if (offsetDelta < Math.Pow(2, 6) && offsetDelta >= -Math.Pow(2, 6))
                {
                    offsetLen = 7;
                    PutBitBlockV(ref bAnimationBuffer, 1, ref offsetBit, 0);
                }
                else
                {
                    offsetLen = 16;
                    PutBitBlockV(ref bAnimationBuffer, 1, ref offsetBit, 1);
                }

                PutBitBlockV(ref bAnimationBuffer, offsetLen, ref offsetBit, offsetDelta);
            }

            for (bi = 0; bi < bFrame.bones.Count; bi++)
            {
                //  Debug.Print "       Bone "; Str$(bi)
                WriteBattleFrameBone(ref bAnimationBuffer, ref offsetBit, key, bFrame.bones[bi], bLastFrame.bones[bi]);     //  , UP3DAnimationCarry[bi];
            }
        }

        public static bool WriteBattleAnimation(BinaryWriter memWriter, BattleAnimation bAnimation, ref int ifileBufferSize)
        {

            byte[] bAnimationBuffer;          
            int offsetBit, fi, iPadding4Bytes;
            bool bBlockOverSize, bWriteUnknownData;
            BattleAnimation tmpbAnimation;

            tmpbAnimation = bAnimation;
            iPadding4Bytes = 0;
            bBlockOverSize = false;
            bWriteUnknownData = false;

            if (tmpbAnimation.blockSize - tmpbAnimation.blockSizeShort > 5)
            {
                iPadding4Bytes = tmpbAnimation.blockSize - tmpbAnimation.blockSizeShort - 5;
                bWriteUnknownData = true;
            }
            else
            {
                if (bAnimation.padding4bytes != null)
                    iPadding4Bytes = bAnimation.padding4bytes.Length;
            }


            // Begin creating the temporal Battle Animation
            memWriter.Write(tmpbAnimation.nBones);          // num bones
            memWriter.Write(tmpbAnimation.numFrames);       // num frames (this time should be equal to numFramesShort)
            ifileBufferSize += 8;

            if (tmpbAnimation.blockSize < 11 || tmpbAnimation.numFramesShort == 0)
            {
                // The animation is empty of maybe is corrupted
                memWriter.Write(tmpbAnimation.blockSize);
                memWriter.Write(tmpbAnimation.framesRawData);
                ifileBufferSize += 4 + tmpbAnimation.framesRawData.Length;
            }
            else
            {
                //  We don't know yet the blockSize, so write it later
                //  Find highest key without exceding the maximum animation length
                tmpbAnimation.key = 0;

                do
                {
                    offsetBit = 0;

                    // Write 1st frame uncompressed
                    bAnimationBuffer = new byte[1];
                    WriteBattleUncompressedFrame(ref bAnimationBuffer, ref offsetBit, tmpbAnimation.key, tmpbAnimation.frames[0]);

                    // Write rest of frames
                    for (fi = 1; fi < tmpbAnimation.numFramesShort; fi++)
                    {
                        //  Debug.Print "   Frame "; Str$(fi)
                        WriteBattleFrame(ref bAnimationBuffer, ref offsetBit, tmpbAnimation.key, 
                                            tmpbAnimation.frames[fi], tmpbAnimation.frames[fi - 1]);
                    }

                    tmpbAnimation.key += 2;

                } while (bAnimationBuffer.Length > 0xFFFF && tmpbAnimation.key <= 4);

                //  We will check if the length of the animation compressed buffer is higher
                //  than the maximum length of the byte array by FF7 format specification.
                //  It uses Unsigned Integer16 data (65535) in the header for the blockSize.
                if (bAnimationBuffer.Length > 0xFFFF) bBlockOverSize = true;
                else
                {
                    tmpbAnimation.key -= 2;

                    //  Update Battle Animation with AnimationBuffer bytes
                    tmpbAnimation.framesRawData = new byte[bAnimationBuffer.Length];
                    bAnimationBuffer.CopyTo(tmpbAnimation.framesRawData, 0);

                    tmpbAnimation.blockSizeShort = (ushort)bAnimationBuffer.Length;
                    tmpbAnimation.blockSize = bAnimationBuffer.Length + 5;

                    if (bWriteUnknownData)
                    {
                        tmpbAnimation.blockSize += iPadding4Bytes + 5;   // this the the unknown data case
                        tmpbAnimation.padding4bytes = new byte[iPadding4Bytes];
                    }
                    else
                    {
                        //iPadding4Bytes = (4 - (tmpbAnimation.blockSize % 4)) % 4;
                        //tmpbAnimation.blockSize += iPadding4Bytes;   // padding 4bytes
                        //tmpbAnimation.padding4bytes = new byte[iPadding4Bytes];
                        iPadding4Bytes = tmpbAnimation.padding4bytes.Length;
                        tmpbAnimation.blockSize += iPadding4Bytes;   // padding 4bytes
                    }

                    //iPadding4Bytes = (4 - (tmpbAnimation.blockSize % 4)) % 4;
                    //tmpbAnimation.blockSize += iPadding4Bytes;   // padding 4bytes
                    //tmpbAnimation.padding4bytes = new byte[iPadding4Bytes];


                    //  Write frames data to fileBuffer.
                    memWriter.Write(tmpbAnimation.blockSize);
                    memWriter.Write(tmpbAnimation.numFramesShort);
                    memWriter.Write(tmpbAnimation.blockSizeShort);
                    memWriter.Write(tmpbAnimation.key);
                    memWriter.Write(tmpbAnimation.framesRawData);

                    if (tmpbAnimation.padding4bytes.Length > 0)
                        memWriter.Write(tmpbAnimation.padding4bytes);

                    ifileBufferSize += 9 + tmpbAnimation.framesRawData.Length + iPadding4Bytes;


                    // Latest UnknownData (like RTAA/RTDA Battle Animation with the first animation odd data at the end)
                    if (bWriteUnknownData)
                    {
                        memWriter.Write(tmpbAnimation.nBones);
                        memWriter.Write((byte)tmpbAnimation.numFrames);
                        ifileBufferSize += 5;
                    }


                }
            }

            return bBlockOverSize;
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ============================= CREATE EMPTY BATTLE/WEAPON/MAGIC ANIMATION ==========================
        //  ---------------------------------------------------------------------------------------------------
        public static void CreateEmptyBattleAnimation(ref BattleAnimation bAnimation, int nBones)
        {
            BattleFrame tmpbFrame;
            BattleFrameBone tmpbFrameBone;
            int bi;

            bAnimation.nBones = nBones;
            bAnimation.numFrames = 1;
            bAnimation.numFramesShort = 1;
            bAnimation.blockSize = 0;
            bAnimation.blockSizeShort = 0;

            bAnimation.key = 0;

            tmpbFrameBone = new BattleFrameBone()
            {
                accumAlpha = 0,
                accumBeta = 0,
                accumGamma = 0,

                alpha = 0,
                beta = 0,
                gamma = 0,
            };

            tmpbFrame = new BattleFrame()
            {
                startX = 0,
                startY = 0,
                startZ = 0,

                bones = new List<BattleFrameBone>(),
            };

            for (bi = 0; bi < nBones; bi++)
                tmpbFrame.bones.Add(tmpbFrameBone);
            
            bAnimation.frames = new List<BattleFrame>() { tmpbFrame };           
            //bAnimation.frames.Add(tmpbFrame);
        }



        //  ---------------------------------------------------------------------------------------------------
        //  =============================== CREATE COMPATIBLE BATTLE ANIMATION ================================
        //  ---------------------------------------------------------------------------------------------------
        public static void CreateCompatibleBattleAnimationFrame(BattleSkeleton bSkeleton, ref BattleFrame bFrame)
        {
            int bi, jsp, jsp0, stageIndex;
            //string[] joint_stack = new string[bSkeleton.nBones];
            int[] joint_stack = new int[bSkeleton.nBones + 1];
            float hipArmAngle = 0, c1, c2;
            BattleFrameBone tmpbFrameBone;

            jsp = 0;
            jsp0 = 0;

            joint_stack[jsp] = -1;

            stageIndex = 1;

            // 1st Bone Frame
            tmpbFrameBone = new BattleFrameBone()
            {
                alpha = 90,
                beta = 0,
                gamma = 0,

                accumAlpha = 0,
                accumBeta = 0,
                accumGamma = 0,
            };

            bFrame.bones.Add(tmpbFrameBone);

            for (bi = 0; bi < bSkeleton.nBones; bi++)
            {
                tmpbFrameBone = new BattleFrameBone();

                //while (!(bSkeleton.bones[bi].parentBone.ToString() == joint_stack[jsp]) && jsp > 0)
                while (!(bSkeleton.bones[bi].parentBone == joint_stack[jsp]) && jsp > 0)
                {
                    glPopMatrix();
                    jsp--;
                }

                if (jsp0 > jsp) stageIndex++;
                //  Debug.Print obj.Bones(bi + 1).ParentBone, bi, jsp, StageIndex

                switch (stageIndex)
                {
                    case 1:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 0;
                        if (bi == 1) stageIndex = 2;
                        break;
                    case 2:
                        tmpbFrameBone.alpha = -145;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 3;
                        break;
                    case 3:
                        if (jsp > jsp0)
                        {
                            tmpbFrameBone.alpha = 0;
                            tmpbFrameBone.beta = 0;
                            tmpbFrameBone.gamma = 0;
                        }
                        else
                        {
                            tmpbFrameBone.alpha = -180;
                            tmpbFrameBone.beta = 0;
                            tmpbFrameBone.gamma = 180;
                            stageIndex = 5;

                        }
                        break;
                    case 4:
                        tmpbFrameBone.alpha = -180;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 180;
                        stageIndex = 5;
                        break;
                    case 5:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = 90;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 6;
                        break;
                    case 6:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = -60;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 7;
                        break;
                    case 7:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 8;
                        break;
                    case 8:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 9;
                        break;
                    case 9:
                        tmpbFrameBone.alpha = -90;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 10;
                        break;
                    case 10:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 0;
                        break;
                    case 11:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = -90;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 12;
                        break;
                    case 12:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = 60;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 13;
                        break;
                    case 13:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 14;
                        break;
                    case 14:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 15;
                        break;
                    case 15:
                        tmpbFrameBone.alpha = -90;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 16;
                        break;
                    case 16:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 0;
                        break;
                    case 17:
                        c1 = (float)(bSkeleton.bones[bi + 1].len - (bSkeleton.bones[bi].len * 0.01));
                        c2 = (float)(Math.Sqrt(Math.Pow(bSkeleton.bones[bi + 1].len, 2) - Math.Pow(c1, 2)));
                        hipArmAngle = (float)(Math.Atan(c2 / c1) / PI_180);

                        if (float.IsNaN(hipArmAngle) || float.IsInfinity(hipArmAngle)) hipArmAngle = 0;

                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = hipArmAngle;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 18;
                        break;
                    case 18:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = -hipArmAngle - 90;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 19;
                        break;
                    case 19:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 0;
                        break;
                    case 20:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = -hipArmAngle;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 21;
                        break;
                    case 21:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = hipArmAngle + 90;
                        tmpbFrameBone.gamma = 0;
                        stageIndex = 22;
                        break;
                    case 22:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 0;
                        break;
                    case 23:
                        tmpbFrameBone.alpha = 90;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 0;
                        break;

                    default:
                        tmpbFrameBone.alpha = 0;
                        tmpbFrameBone.beta = 0;
                        tmpbFrameBone.gamma = 0;
                        break;
                }

                if (bi == 0) tmpbFrameBone.gamma = 180;
                bFrame.bones.Add(tmpbFrameBone);

                jsp0 = jsp;
                jsp++;

                //joint_stack[jsp] = bi.ToString();
                joint_stack[jsp] = bi;
            }
        }

        public static void CreateCompatibleBattleAnimation(BattleSkeleton bSkeleton, ref BattleAnimation bAnimation)
        {
            BattleFrame tmpbFrame;

            bAnimation.nBones = bSkeleton.nBones;
            bAnimation.numFrames = 1;
            bAnimation.blockSize = 0;
            bAnimation.numFramesShort = 1;
            bAnimation.blockSizeShort = 0;
            bAnimation.key = 0;
            //bAnimation.missingNumFramesShort = false;
            bAnimation.frames = new List<BattleFrame>();

            tmpbFrame = new BattleFrame()
            {
                startX = 0,
                startY = 0,
                startZ = 0,

                bones = new List<BattleFrameBone>(),
            };

            CreateCompatibleBattleAnimationFrame(bSkeleton, ref tmpbFrame);
            bAnimation.frames.Add(tmpbFrame);
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ========================================== INTERPOLATION ==========================================
        //  ---------------------------------------------------------------------------------------------------
        public static void GetTwoBattleFramesWeaponInterpolation(BattleFrame bFrameA, BattleFrame bFrameB,
                                                                 float alpha, ref BattleFrame bFrameOut)
        {
            //BattleFrameBone tmpbFrameBone;

            Quaternion quat_a;
            Quaternion quat_b;
            Quaternion quat_interp;

            Point3D euler_res;
            float alpha_inv;
            double[] mat = new double[16];

            alpha_inv = 1 - alpha;

            bFrameOut.startX = (int)Math.Round(bFrameA.startX * alpha_inv + bFrameB.startX * alpha);
            bFrameOut.startY = (int)Math.Round(bFrameA.startY * alpha_inv + bFrameB.startY * alpha);
            bFrameOut.startZ = (int)Math.Round(bFrameA.startZ * alpha_inv + bFrameB.startZ * alpha);
            //bFrameOut.startX = (int)(bFrameA.startX * alpha_inv + bFrameB.startX * alpha);
            //bFrameOut.startY = (int)(bFrameA.startY * alpha_inv + bFrameB.startY * alpha);
            //bFrameOut.startZ = (int)(bFrameA.startZ * alpha_inv + bFrameB.startZ * alpha);
            bFrameOut.bones  = new List<BattleFrameBone>();

            quat_a = GetQuaternionFromEulerYXZr(bFrameA.bones[0].alpha, bFrameA.bones[0].beta, bFrameA.bones[0].gamma);
            NormalizeQuaternion(ref quat_a);
            quat_b = GetQuaternionFromEulerYXZr(bFrameB.bones[0].alpha, bFrameB.bones[0].beta, bFrameB.bones[0].gamma);
            NormalizeQuaternion(ref quat_b);

            quat_interp = QuaternionSlerp2(ref quat_a, ref quat_b, alpha);
            NormalizeQuaternion(ref quat_interp);
            BuildMatrixFromQuaternion(quat_interp, ref mat);
            euler_res = GetEulerYXZrFromMatrix(mat);

            //  NormalizeEulerAngles(euler_res);
            bFrameOut.bones.Add(new BattleFrameBone(euler_res.y, euler_res.x, euler_res.z));
        }

        public static void GetTwoBattleFramesInterpolation(BattleSkeleton bSkeleton, BattleFrame bFrameA, BattleFrame bFrameB,
                                                           float alpha, ref BattleFrame bFrameOut)
        {
            int bi, jsp;
            BattleFrameBone tmpbFrameBone;

            int[] joint_stack;
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
            int numBones;
            double[] mat = new double[16];

            numBones = bSkeleton.nBones;    // fFrameA.bones.Count

            // Let's check this in design time.
            Debug.Assert(numBones == bFrameA.bones.Count - 1);

            if (numBones == 1)
            {
                GetTwoBattleFramesWeaponInterpolation(bFrameA, bFrameB, alpha, ref bFrameOut);
            }
            else
            {
                alpha_inv = 1 - alpha;
                bFrameOut.startX = (int)Math.Round(bFrameA.startX * alpha_inv + bFrameB.startX * alpha);
                bFrameOut.startY = (int)Math.Round(bFrameA.startY * alpha_inv + bFrameB.startY * alpha);
                bFrameOut.startZ = (int)Math.Round(bFrameA.startZ * alpha_inv + bFrameB.startZ * alpha);
                //bFrameOut.startX = (int)(bFrameA.startX * alpha_inv + bFrameB.startX * alpha);
                //bFrameOut.startY = (int)(bFrameA.startY * alpha_inv + bFrameB.startY * alpha);
                //bFrameOut.startZ = (int)(bFrameA.startZ * alpha_inv + bFrameB.startZ * alpha);

                bFrameOut.bones = new List<BattleFrameBone>();

                joint_stack = new int[numBones + 1];
                rotations_stack_a = new Quaternion[numBones + 1];
                rotations_stack_b = new Quaternion[numBones + 1];
                rotations_stack_acum = new Quaternion[numBones + 1];

                rotations_stack_a[0] = GetQuaternionFromEulerYXZr(bFrameA.bones[0].alpha, bFrameA.bones[0].beta, bFrameA.bones[0].gamma);
                NormalizeQuaternion(ref rotations_stack_a[0]);
                rotations_stack_b[0] = GetQuaternionFromEulerYXZr(bFrameB.bones[0].alpha, bFrameB.bones[0].beta, bFrameB.bones[0].gamma);
                NormalizeQuaternion(ref rotations_stack_b[0]);
                rotations_stack_acum[0] = QuaternionSlerp2(ref rotations_stack_a[0], ref rotations_stack_b[0], alpha);
                NormalizeQuaternion(ref rotations_stack_acum[0]);

                jsp = 0;
                joint_stack[jsp] = -1;

                // Init Bone[0] (weapon)
                bFrameOut.bones.Add(new BattleFrameBone());

                for (bi = 0; bi < numBones; bi++)
                {
                    while (jsp > 0 && bSkeleton.bones[bi].parentBone != joint_stack[jsp] && jsp > 0) jsp--;

                    quat_a = GetQuaternionFromEulerYXZr(bFrameA.bones[bi + 1].alpha, bFrameA.bones[bi + 1].beta, bFrameA.bones[bi + 1].gamma);
                    NormalizeQuaternion(ref quat_a);
                    quat_b = GetQuaternionFromEulerYXZr(bFrameB.bones[bi + 1].alpha, bFrameB.bones[bi + 1].beta, bFrameB.bones[bi + 1].gamma);
                    NormalizeQuaternion(ref quat_b);

                    MultiplyQuaternions(rotations_stack_a[jsp], quat_a, ref quat_acum_a);
                    NormalizeQuaternion(ref quat_acum_a);
                    rotations_stack_a[jsp + 1] = quat_acum_a;

                    MultiplyQuaternions(rotations_stack_b[jsp], quat_b, ref quat_acum_b);
                    NormalizeQuaternion(ref quat_acum_b);
                    rotations_stack_b[jsp + 1] = quat_acum_b;

                    quat_interp = QuaternionSlerp2(ref quat_acum_a, ref quat_acum_b, alpha);
                    rotations_stack_acum[jsp + 1] = quat_interp;
                    quat_acum_inverse = GetQuaternionConjugate(ref rotations_stack_acum[jsp]);
                    MultiplyQuaternions(quat_acum_inverse, quat_interp, ref quat_interp_final);
                    NormalizeQuaternion(ref quat_interp_final);

                    BuildMatrixFromQuaternion(quat_interp_final, ref mat);
                    euler_res = GetEulerYXZrFromMatrix(mat);

                    bFrameOut.bones.Add(new BattleFrameBone(euler_res.y, euler_res.x, euler_res.z));

                    jsp++;
                    joint_stack[jsp] = bi;
                }

                BuildMatrixFromQuaternion(rotations_stack_acum[0], ref mat);
                euler_res = GetEulerYXZrFromMatrix(mat);

                tmpbFrameBone = bFrameOut.bones[0];
                tmpbFrameBone.alpha = euler_res.y;
                tmpbFrameBone.beta = euler_res.x;
                tmpbFrameBone.gamma = euler_res.z;
                bFrameOut.bones[0] = tmpbFrameBone;
            }
        }

        //  ---------------------------------------------------------------------------------------------------
        //  ========================================== INTERPOLATION ==========================================
        //  ---------------------------------------------------------------------------------------------------
        public static void InterpolateBattleSkeletonAnimation(ref BattleSkeleton bSkeleton, ref BattleAnimation bAnimation,
                                                              int numInterpolatedFrames, bool bisLoopQ)
        {
            float alpha, primarySecondaryCountersCoef;
            int nextElemDiff, frameOffset, fi, ifi, baseFinalFrame, nFrames;
            BattleFrame tmpbFrame = new BattleFrame();

            nextElemDiff = numInterpolatedFrames + 1;
            frameOffset = 0;

            if (!bisLoopQ) frameOffset = numInterpolatedFrames;

            //  Numframes1 and NumFrames2 are usually different.
            //  Don't know if this is relevant at all, but keep the balance between them just in case
            primarySecondaryCountersCoef = bAnimation.numFrames / bAnimation.numFramesShort;

            if (bAnimation.numFramesShort == 1) return;

            //  Create new frames
            bAnimation.numFramesShort = (ushort)(bAnimation.numFramesShort * (numInterpolatedFrames + 1) - frameOffset);
            bAnimation.numFrames = (int)(bAnimation.numFramesShort * primarySecondaryCountersCoef);


            nFrames = bAnimation.numFramesShort - bAnimation.frames.Count;
            for (fi = 0; fi < nFrames; fi++) bAnimation.frames.Add(tmpbFrame);

            // Move the original frames into their new positions
            for (fi = bAnimation.numFramesShort - (1 + numInterpolatedFrames - frameOffset); fi >= 0; fi -= nextElemDiff)
                bAnimation.frames[fi] = bAnimation.frames[fi / (numInterpolatedFrames + 1)];

            //  Interpolate the new frames
            for (fi = 0; fi <= bAnimation.numFramesShort - (1 + nextElemDiff + numInterpolatedFrames - frameOffset); fi += nextElemDiff)
            {
                for (ifi = 1; ifi <= numInterpolatedFrames; ifi++)
                {
                    alpha = (float)ifi / (numInterpolatedFrames + 1);
                    if (bSkeleton.nBones > 1)
                    {
                        tmpbFrame = bAnimation.frames[fi + ifi];
                        GetTwoBattleFramesInterpolation(bSkeleton, bAnimation.frames[fi], bAnimation.frames[fi + numInterpolatedFrames + 1],
                                                        alpha, ref tmpbFrame);
                        bAnimation.frames[fi + ifi] = tmpbFrame;
                    }
                    else
                    {
                        tmpbFrame = bAnimation.frames[fi + ifi];
                        GetTwoBattleFramesWeaponInterpolation(bAnimation.frames[fi], bAnimation.frames[fi + numInterpolatedFrames + 1],
                                                        alpha, ref tmpbFrame);
                        bAnimation.frames[fi + ifi] = tmpbFrame;
                    }
                }
            }

            baseFinalFrame = bAnimation.numFramesShort - numInterpolatedFrames - 1;
            if (bisLoopQ)
            {
                for (ifi = 1; ifi <= numInterpolatedFrames; ifi++)
                {
                    alpha = (float)ifi / (numInterpolatedFrames + 1);
                    if (bSkeleton.nBones > 1)
                    {
                        tmpbFrame = bAnimation.frames[baseFinalFrame + ifi];
                        GetTwoBattleFramesInterpolation(bSkeleton, bAnimation.frames[baseFinalFrame], bAnimation.frames[0], alpha, ref tmpbFrame);
                        bAnimation.frames[baseFinalFrame + ifi] = tmpbFrame;
                    }
                    else
                    {
                        tmpbFrame = bAnimation.frames[baseFinalFrame + ifi];
                        GetTwoBattleFramesWeaponInterpolation(bAnimation.frames[baseFinalFrame], bAnimation.frames[0], alpha, ref tmpbFrame);
                        bAnimation.frames[baseFinalFrame + ifi] = tmpbFrame;
                    }
                }
            }

            //  Kimera VB6 Commented
            //  NormalizeBattleAnimation(ref bAnimation)
        }

        public static void InterpolateBattleWeaponAnimation(ref BattleAnimation bAnimation,
                                                            int numInterpolatedFrames, bool bisLoopQ,
                                                            int numFramesSkeleton, ushort numFramesShortSkeleton)
        {
            float alpha;
            int nextElemDiff, frameOffset, fi, ifi, baseFinalFrame, nFrames;
            BattleFrame tmpbFrame = new BattleFrame();

            nextElemDiff = numInterpolatedFrames + 1;
            frameOffset = 0;

            if (!bisLoopQ) frameOffset = numInterpolatedFrames;

            bAnimation.numFramesShort = numFramesShortSkeleton;
            bAnimation.numFrames = numFramesSkeleton;

            nFrames = bAnimation.numFramesShort - bAnimation.frames.Count;
            for (fi = 0; fi < nFrames; fi++) bAnimation.frames.Add(tmpbFrame);

            // Move the original frames into their new positions
            for (fi = bAnimation.numFramesShort - (1 + numInterpolatedFrames - frameOffset); fi >= 0; fi -= nextElemDiff)
                bAnimation.frames[fi] = bAnimation.frames[fi / (numInterpolatedFrames + 1)];

            //  Interpolate the new frames
            for (fi = 0; fi <= bAnimation.numFramesShort - (1 + numInterpolatedFrames + numInterpolatedFrames - frameOffset); fi += nextElemDiff)
            {
                for (ifi = 1; ifi <= numInterpolatedFrames; ifi++)
                {
                    alpha = (float)ifi / (numInterpolatedFrames + 1);

                    tmpbFrame = bAnimation.frames[fi + ifi];
                    GetTwoBattleFramesWeaponInterpolation(bAnimation.frames[fi], bAnimation.frames[fi + nextElemDiff],
                                                            alpha, ref tmpbFrame);
                    bAnimation.frames[fi + ifi] = tmpbFrame;
                }

                baseFinalFrame = bAnimation.numFramesShort - numInterpolatedFrames - 1;

                if (bisLoopQ)
                {
                    for (ifi = 1; ifi <= numInterpolatedFrames; ifi++)
                    {
                        alpha = (float)ifi / (numInterpolatedFrames + 1);

                        tmpbFrame = bAnimation.frames[baseFinalFrame + ifi];
                        GetTwoBattleFramesWeaponInterpolation(bAnimation.frames[baseFinalFrame], bAnimation.frames[0],
                                                              alpha, ref tmpbFrame);
                        bAnimation.frames[baseFinalFrame + ifi] = tmpbFrame;
                    }
                }
            }
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ================================ COPY BATTLE ANIMATION (OR PARTS) =================================
        //  ---------------------------------------------------------------------------------------------------
        public static BattleFrameBone CopybFrameBone(BattleFrameBone bFrameBoneIn)
        {
            BattleFrameBone bFrameBoneOut;

            bFrameBoneOut = new BattleFrameBone()
            {
                accumAlpha = bFrameBoneIn.accumAlpha,
                accumBeta = bFrameBoneIn.accumBeta,
                accumGamma = bFrameBoneIn.accumGamma,

                accumAlphaS = bFrameBoneIn.accumAlphaS,
                accumBetaS = bFrameBoneIn.accumBetaS,
                accumGammaS = bFrameBoneIn.accumGammaS,

                alpha = bFrameBoneIn.alpha,
                beta = bFrameBoneIn.beta,
                gamma = bFrameBoneIn.gamma,
            };

            return bFrameBoneOut;
        }

        public static BattleFrame CopybFrame(BattleFrame bFrameIn)
        {
            BattleFrame bFrameOut;

            bFrameOut = new BattleFrame()
            {
                startX = bFrameIn.startX,
                startY = bFrameIn.startY,
                startZ = bFrameIn.startZ,

                bones = new List<BattleFrameBone>(),
            };

            foreach (BattleFrameBone itmbFrameBone in bFrameIn.bones) 
                                bFrameOut.bones.Add(CopybFrameBone(itmbFrameBone));

            return bFrameOut;
        }

        public static BattleAnimation CopybAnimation(BattleAnimation bAnimationIn)
        {
            BattleAnimation bAnimationOut;

            bAnimationOut = new BattleAnimation()
            {
                blockSize = bAnimationIn.blockSize,
                blockSizeShort = bAnimationIn.blockSizeShort,
                framesRawData = bAnimationIn.framesRawData,
                key = bAnimationIn.key,
                nBones = bAnimationIn.nBones,
                numFrames = bAnimationIn.numFrames,
                numFramesShort = bAnimationIn.numFramesShort,
                padding4bytes = bAnimationIn.padding4bytes,

                frames = new List<BattleFrame>(),
            };

            foreach (BattleFrame itmbFrame in bAnimationIn.frames) 
                            bAnimationOut.frames.Add(CopybFrame(itmbFrame));

            return bAnimationOut;
        }





    }
}
