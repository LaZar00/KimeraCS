using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KimeraCS
{
    using static FrmSkeletonEditor;

    using static FF7Skeleton;
    using static FF7FieldSkeleton;
    using static FF7FieldAnimation;
    using static FF7PModel;

    using static FF7BattleSkeleton;
    using static FF7BattleAnimationsPack;
    using static FF7BattleAnimation;

    class UndoRedo
    {

        public static int iUndoBufferCapacity = 10;

        private struct SkeletonState
        {
            public PModel URModel;
            public FieldSkeleton URfSkeleton;
            public BattleSkeleton URbSkeleton;

            public FieldAnimation URfAnimation;
            public BattleAnimationsPack URbAnimationsPack;

            public int frameIndex, battleAnimIndex, weaponIndex, textureIndex;
            public int URSelectedBone, URSelectedBonePiece;

            public double URalpha, URbeta, URgamma, URDIST;
            public float URpanX, URpanY, URpanZ;           
        }

        private static SkeletonState[] UndoBuffer;
        private static SkeletonState[] RedoBuffer;

        public static int UndoCursor, RedoCursor;

        public static void InitializeUndoRedo()
        {
            UndoBuffer = new SkeletonState[iUndoBufferCapacity];
            RedoBuffer = new SkeletonState[iUndoBufferCapacity];

            UndoCursor = 0;
            RedoCursor = 0;
        }

        public static void AddStateToBuffer(FrmSkeletonEditor frmSkEditor)
        {
            int si;

            if (UndoCursor < iUndoBufferCapacity)
            {
                StoreState(ref UndoBuffer[UndoCursor], frmSkEditor);
                UndoCursor++;
            }
            else 
            {
                for (si = 0; si < iUndoBufferCapacity - 1; si++)
                {
                    UndoBuffer[si] = UndoBuffer[si + 1];                    
                }

                StoreState(ref UndoBuffer[UndoCursor - 1], frmSkEditor);
            }

            if (UndoCursor > 0) frmSkEditor.undoToolStripMenuItem.Enabled = true;
            //RedoCursor = 0;
        }

        public static void Redo(FrmPEditor frmIn, FrmSkeletonEditor frmSkEditor)
        {
            int si;

            if (frmIn != null && frmIn.Visible)
            {
                MessageBox.Show("Can't Redo while the editor window is visible.", "Information", MessageBoxButtons.OK);
                return;
            }

            if (bLoaded)
            {
                if (RedoCursor > 0)
                {
                    if (UndoCursor < iUndoBufferCapacity)
                    {
                        StoreState(ref UndoBuffer[UndoCursor], frmSkEditor);
                        UndoCursor++;
                    }
                    else
                    {
                        //  If we've run out of space delete the oldest iteration
                        for (si = 0; si < iUndoBufferCapacity - 1; si++)
                        {
                            UndoBuffer[si] = UndoBuffer[si + 1];
                        }

                        StoreState(ref UndoBuffer[UndoCursor - 1], frmSkEditor);
                    }

                    RestoreState(ref RedoBuffer[RedoCursor - 1], frmSkEditor);
                    RedoCursor--;
                }
            }

            if (RedoCursor == 0) frmSkEditor.redoToolStripMenuItem.Enabled = false;
            frmSkEditor.undoToolStripMenuItem.Enabled = true;

        }

        public static void Undo(FrmPEditor frmIn, FrmSkeletonEditor frmSkEditor)
        {
            int si;

            if (frmIn != null && frmIn.Visible)
            {
                MessageBox.Show("Can't Undo while the editor window is visible.", "Information", MessageBoxButtons.OK);
                return;
            }

            if (bLoaded)
            {
                if (UndoCursor > 0)
                {
                    if (RedoCursor < iUndoBufferCapacity)
                    {
                        StoreState(ref RedoBuffer[RedoCursor], frmSkEditor);
                        RedoCursor++;
                    }
                    else
                    {
                        //  If we've run out of space delete the oldest iteration
                        for (si = 0; si < iUndoBufferCapacity - 1; si++)
                        {
                            RedoBuffer[si] = RedoBuffer[si + 1];
                        }

                        StoreState(ref RedoBuffer[RedoCursor - 1], frmSkEditor);
                    }

                    RestoreState(ref UndoBuffer[UndoCursor - 1], frmSkEditor);
                    UndoCursor--;
                }

                frmSkEditor.redoToolStripMenuItem.Enabled = true;
                if (UndoCursor == 0) frmSkEditor.undoToolStripMenuItem.Enabled = false;
            }
        }
        
        private static void RestoreState(ref SkeletonState sState, FrmSkeletonEditor frmSkEditor)
        {
            SelectedBone = sState.URSelectedBone;
            SelectedBonePiece = sState.URSelectedBonePiece;

            alpha = sState.URalpha;
            beta = sState.URbeta;
            gamma = sState.URgamma;
            DIST = sState.URDIST;
            panX = sState.URpanX;
            panY = sState.URpanY;
            panZ = sState.URpanZ;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    fSkeleton = CopyfSkeleton(sState.URfSkeleton);
                    fAnimation = CopyfAnimation(sState.URfAnimation);

                    if (SelectedBone > -1)
                    {
                        frmSkEditor.SetBoneModifiers();

                        if (SelectedBonePiece > - 1)
                        {
                            frmSkEditor.SetBonePieceModifiers();
                            frmSkEditor.SetTextureEditorFields();
                        }
                    }

                    frmSkEditor.SetTextureEditorFields();
                    frmSkEditor.SetFrameEditorFields();

                    if (frmSkEditor.cbTextureSelect.Items.Count > 0)
                        frmSkEditor.cbTextureSelect.SelectedIndex = sState.textureIndex;

                    if (sState.frameIndex > fAnimation.nFrames) 
                        frmSkEditor.tbCurrentFrameScroll.Value = fAnimation.nFrames - 1;
                    else
                        frmSkEditor.tbCurrentFrameScroll.Value = sState.frameIndex;

                    frmSkEditor.tbCurrentFrameScroll.Maximum = fAnimation.nFrames - 1;

                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    bSkeleton = CopybSkeleton(sState.URbSkeleton);
                    bAnimationsPack = CopybAnimationsPack(sState.URbAnimationsPack);

                    if (SelectedBone > -1)
                    {
                        frmSkEditor.SetBoneModifiers();

                        if (SelectedBonePiece > -1) frmSkEditor.SetBonePieceModifiers();
                    }

                    frmSkEditor.SetTextureEditorFields();
                    frmSkEditor.SetFrameEditorFields();

                    if (frmSkEditor.cbBattleAnimation.Visible) frmSkEditor.cbBattleAnimation.SelectedIndex = sState.battleAnimIndex;

                    if (sState.frameIndex > sState.URbAnimationsPack.SkeletonAnimations[sState.battleAnimIndex].numFramesShort)
                        frmSkEditor.tbCurrentFrameScroll.Value = sState.URbAnimationsPack.SkeletonAnimations[sState.battleAnimIndex].numFramesShort - 1;
                    else
                        frmSkEditor.tbCurrentFrameScroll.Value = sState.frameIndex;


                    frmSkEditor.tbCurrentFrameScroll.Maximum = sState.URbAnimationsPack.SkeletonAnimations[sState.battleAnimIndex].numFramesShort - 1;

                    if (frmSkEditor.cbWeapon.Visible) frmSkEditor.cbWeapon.SelectedIndex = sState.weaponIndex;

                    frmSkEditor.cbTextureSelect.SelectedIndex = sState.textureIndex;
                    break;

                case K_P_FIELD_MODEL:
                case K_P_BATTLE_MODEL:
                case K_P_MAGIC_MODEL:
                case K_3DS_MODEL:
                    fPModel = CopyPModel(sState.URModel);
                    break;
            }
        }

        private static void StoreState(ref SkeletonState sState, FrmSkeletonEditor frmSkEditor)
        {
            sState.URSelectedBone = SelectedBone;
            sState.URSelectedBonePiece = SelectedBonePiece;

            sState.URalpha = alpha;
            sState.URbeta = beta;
            sState.URgamma = gamma;
            sState.URDIST = DIST;
            sState.URpanX = panX;
            sState.URpanY = panY;
            sState.URpanZ = panZ;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    sState.URfSkeleton = CopyfSkeleton(fSkeleton);
                    sState.URfAnimation = CopyfAnimation(fAnimation);

                    sState.textureIndex = frmSkEditor.cbTextureSelect.SelectedIndex;
                    sState.frameIndex = frmSkEditor.tbCurrentFrameScroll.Value;
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    sState.URbSkeleton = CopybSkeleton(bSkeleton);
                    sState.URbAnimationsPack = CopybAnimationsPack(bAnimationsPack);

                    sState.frameIndex = frmSkEditor.tbCurrentFrameScroll.Value;
                    sState.textureIndex = frmSkEditor.cbTextureSelect.SelectedIndex;
                    
                    if (frmSkEditor.cbBattleAnimation.Visible) sState.battleAnimIndex = ianimIndex;
                    if (frmSkEditor.cbWeapon.Visible) sState.weaponIndex = ianimWeaponIndex;
                    break;

                case K_P_FIELD_MODEL:
                case K_P_BATTLE_MODEL:
                case K_P_MAGIC_MODEL:
                case K_3DS_MODEL:
                    sState.URModel = CopyPModel(fPModel);
                    break;
            }
        }



    }

}
