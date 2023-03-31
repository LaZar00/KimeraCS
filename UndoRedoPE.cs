using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KimeraCS
{
    using static FrmPEditor;

    using static FF7Skeleton;
    using static FF7FieldSkeleton;
    using static FF7FieldAnimation;
    using static FF7PModel;

    using static FF7BattleSkeleton;
    using static FF7BattleAnimationsPack;
    using static FF7BattleAnimation;

    using static Utils;

    class UndoRedoPE
    {

        public static int iUndoBufferPECapacity = 20;

        private struct PEState
        {
            public PModel URPEEditedPModel;

            public float URPEpanX, URPEpanY, URPEpanZ, URPEDIST;
            public float URPErszX, URPErszY, URPErszZ;
            public float URPErepX, URPErepY, URPErepZ;

            public float URPErotateAlpha, URPErotateBeta, URPErotateGamma;
            public float URPEalpha, URPEbeta, URPEgamma;

            public bool PalettizedQ;
            public List<Color> colorTablePE;
            public PairIB[] translationTablePolysPE;
            public PairIB[] translationTableVertexPE;
            public byte iThresholdPE;
        }

        private static PEState[] UndoBufferPE;
        private static PEState[] RedoBufferPE;

        public static int UndoCursorPE, RedoCursorPE;

        public static void InitializeUndoRedoPE()
        {
            UndoBufferPE = new PEState[iUndoBufferPECapacity];
            RedoBufferPE = new PEState[iUndoBufferPECapacity];

            UndoCursorPE = 0;
            RedoCursorPE = 0;
        }

        public static void AddStateToBufferPE(FrmPEditor frmPEdit)
        {
            int si;

            if (UndoCursorPE < iUndoBufferPECapacity)
            {
                StoreStatePE(ref UndoBufferPE[UndoCursorPE], frmPEdit);
                UndoCursorPE++;
            }
            else
            {
                for (si = 0; si < iUndoBufferPECapacity - 1; si++)
                {
                    UndoBufferPE[si] = UndoBufferPE[si + 1];
                }

                StoreStatePE(ref UndoBufferPE[UndoCursorPE - 1], frmPEdit);
            }

            if (UndoCursorPE > 0) frmPEdit.undoToolStripMenuItem.Enabled = true;
            //RedoCursorPE = 0;
        }

        public static void RedoPE(FrmPEditor frmPEdit)
        {
            int si;

            if (loadedPModel)
            {
                if (RedoCursorPE > 0)
                {
                    if (UndoCursorPE < iUndoBufferPECapacity)
                    {
                        StoreStatePE(ref UndoBufferPE[UndoCursorPE], frmPEdit);
                        UndoCursorPE++;
                    }
                    else
                    {
                        //  If we've run out of space delete the oldest iteration
                        for (si = 0; si < iUndoBufferPECapacity - 1; si++)
                        {
                            UndoBufferPE[si] = UndoBufferPE[si + 1];
                        }

                        StoreStatePE(ref UndoBufferPE[UndoCursorPE - 1], frmPEdit);
                    }

                    RestoreStatePE(ref RedoBufferPE[RedoCursorPE - 1], frmPEdit);
                    RedoCursorPE--;
                }

                if (RedoCursorPE == 0) frmPEdit.redoToolStripMenuItem.Enabled = false;
                frmPEdit.undoToolStripMenuItem.Enabled = true;
            }
        }

        public static void UndoPE(FrmPEditor frmPEdit)
        {
            int si;

            if (loadedPModel)
            {
                if (UndoCursorPE > 0)
                {
                    if (RedoCursorPE < iUndoBufferPECapacity)
                    {
                        StoreStatePE(ref RedoBufferPE[RedoCursorPE], frmPEdit);
                        RedoCursorPE++;
                    }
                    else
                    {
                        //  If we've run out of space delete the oldest iteration
                        for (si = 0; si < iUndoBufferPECapacity - 1; si++)
                        {
                            RedoBufferPE[si] = RedoBufferPE[si + 1];
                        }

                        StoreStatePE(ref RedoBufferPE[RedoCursorPE - 1], frmPEdit);
                    }

                    RestoreStatePE(ref UndoBufferPE[UndoCursorPE - 1], frmPEdit);
                    UndoCursorPE--;
                }

                frmPEdit.redoToolStripMenuItem.Enabled = true;
                if (UndoCursorPE == 0) frmPEdit.undoToolStripMenuItem.Enabled = false;
            }
        }

        private static void RestoreStatePE(ref PEState sState, FrmPEditor frmPEdit)
        {
            frmPEdit.loadingModifiersQ = true;

            panXPE = sState.URPEpanX;
            panYPE = sState.URPEpanY;
            panZPE = sState.URPEpanZ;

            DISTPE = sState.URPEDIST;

            rszXPE = sState.URPErszX;
            rszYPE = sState.URPErszY;
            rszZPE = sState.URPErszZ;

            repXPE = sState.URPErepX;
            repYPE = sState.URPErepY;
            repZPE = sState.URPErepZ;

            frmPEdit.hsbResizeX.Value = Convert.ToInt32(sState.URPErszX * 100);
            frmPEdit.hsbResizeY.Value = Convert.ToInt32(sState.URPErszY * 100);
            frmPEdit.hsbResizeZ.Value = Convert.ToInt32(sState.URPErszZ * 100);

            frmPEdit.txtResizeX.Text = frmPEdit.hsbResizeX.Value.ToString();
            frmPEdit.txtResizeY.Text = frmPEdit.hsbResizeY.Value.ToString();
            frmPEdit.txtResizeZ.Text = frmPEdit.hsbResizeZ.Value.ToString();

            frmPEdit.hsbRepositionX.Value = Convert.ToInt32(sState.URPErepX * 100);
            frmPEdit.hsbRepositionY.Value = Convert.ToInt32(sState.URPErepY * 100);
            frmPEdit.hsbRepositionZ.Value = Convert.ToInt32(sState.URPErepZ * 100);

            frmPEdit.txtRepositionX.Text = frmPEdit.hsbRepositionX.Value.ToString();
            frmPEdit.txtRepositionY.Text = frmPEdit.hsbRepositionY.Value.ToString();
            frmPEdit.txtRepositionZ.Text = frmPEdit.hsbRepositionZ.Value.ToString();

            frmPEdit.hsbRotateAlpha.Value = Convert.ToInt32(sState.URPErotateAlpha);
            frmPEdit.hsbRotateBeta.Value = Convert.ToInt32(sState.URPErotateBeta);
            frmPEdit.hsbRotateGamma.Value = Convert.ToInt32(sState.URPErotateGamma);

            frmPEdit.txtRotateAlpha.Text = frmPEdit.hsbRotateAlpha.Value.ToString();
            frmPEdit.txtRotateBeta.Text = frmPEdit.hsbRotateBeta.Value.ToString();
            frmPEdit.txtRotateGamma.Text = frmPEdit.hsbRotateGamma.Value.ToString();

            alphaPE = sState.URPEalpha;
            betaPE = sState.URPEbeta;
            gammaPE = sState.URPEgamma;

            EditedPModel = CopyPModel(sState.URPEEditedPModel);

            if (sState.PalettizedQ)
            {
                frmPEdit.chkPaletteMode.Checked = true;

                colorTable = sState.colorTablePE;
                translationTablePolys = sState.translationTablePolysPE;
                translationTableVertex = sState.translationTableVertexPE;
                iThreshold = sState.iThresholdPE;
            }
            else frmPEdit.chkPaletteMode.Checked = false;

            if (frmPEdit.chkPaletteMode.Checked)
            {
                FillColorTable(EditedPModel, ref colorTable, ref translationTableVertex,
                                                             ref translationTablePolys,
                                                             (byte)iThreshold);
            }

            frmPEdit.FillGroupsList();

            frmPEdit.loadingModifiersQ = false;
        }

        private static void StoreStatePE(ref PEState sState, FrmPEditor frmPEdit)
        {
            sState.URPEpanX = panXPE;
            sState.URPEpanY = panYPE;
            sState.URPEpanZ = panZPE;

            sState.URPEDIST = DISTPE;

            sState.URPErszX = rszXPE;
            sState.URPErszY = rszYPE;
            sState.URPErszZ = rszZPE;

            sState.URPErepX = repXPE;
            sState.URPErepY = repYPE;
            sState.URPErepZ = repZPE;

            sState.URPErotateAlpha = frmPEdit.hsbRotateAlpha.Value;
            sState.URPErotateBeta = frmPEdit.hsbRotateBeta.Value;
            sState.URPErotateGamma = frmPEdit.hsbRotateGamma.Value;

            sState.URPEalpha = alphaPE;
            sState.URPEbeta = betaPE;
            sState.URPEgamma = gammaPE;

            sState.URPEEditedPModel = CopyPModel(EditedPModel);
            sState.PalettizedQ = frmPEdit.chkPaletteMode.Checked;

            if (sState.PalettizedQ)
            {
                sState.colorTablePE = new List<Color>(colorTable.ToArray());
                sState.translationTableVertexPE = translationTableVertex;
                sState.translationTablePolysPE = translationTablePolys;
                sState.iThresholdPE = iThreshold;
            }
        }



    }
}
