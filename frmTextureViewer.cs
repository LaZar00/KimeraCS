using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KimeraCS
{
    using static frmSkeletonEditor;

    using static FF7Skeleton;
    using static FF7FieldRSDResource;
    using static FF7PModel;

    using static Utils;
    using static GDI32;

    public partial class frmTextureViewer : Form
    {

        private frmSkeletonEditor frmSkelEdit;

        public PModel TexViewModel;

        public frmTextureViewer(frmSkeletonEditor frmSkelEdit, PModel modelIn)
        {
            InitializeComponent();

            this.frmSkelEdit = frmSkelEdit;
            Owner = frmSkelEdit;

            //tmpfRSDResource = CopyRSDResource(fRSDResourceIn);
            TexViewModel = CopyPModel(modelIn);
        }


        /////////////////////////////////////////////////////////////
        // ToolTip Helpers:
        // Create the ToolTip and associate with the Form container.
        ToolTip toolTip1 = new ToolTip();

        public void DefineToolTips()
        {
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 1000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 250;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(btnFlipH, "Flip UVs Horizontal");
            toolTip1.SetToolTip(btnFlipV, "Flip UVs Vertical");
            toolTip1.SetToolTip(btnRotateUV, "Rotate UVs");
        }

        private void frmTextureViewer_Load(object sender, EventArgs e)
        {
            Text = "Texture Coordinates(UVs) Viewer" + " - Model: " + TexViewModel.fileName +
                                                       " - Tex: " + frmSkelEdit.cbTextureSelect.Items[frmSkelEdit.cbTextureSelect.SelectedIndex];

            // Assign tooltips.
            DefineToolTips();

            DrawUVs();
        }

        public void DrawUVs()
        {
            Bitmap tmpBMP = null;

            int iGroupIdx, iPolyIdx, iVertCounter, iWidth, iHeight, iTexID;
            int iU, iV;
            float fU, fV, fSecondU, fSecondV, fFirstU, fFirstV, fRadius;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;

            switch(modelType)
            {
                case K_HRC_SKELETON:
                    tmpBMP = new Bitmap(Image.FromHbitmap(fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece].textures[frmSkelEdit.cbTextureSelect.SelectedIndex].HBMP),
                                        new Size(512, 512));
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    tmpBMP = new Bitmap(Image.FromHbitmap(bSkeleton.textures[frmSkelEdit.cbTextureSelect.SelectedIndex].HBMP),
                                        new Size(512, 512));
                    break;
            }


            // Get the size available
            iWidth = tmpBMP.Width;
            iHeight = tmpBMP.Height;

            fSecondU = 0;
            fSecondV = 0;
            fFirstU = 0;
            fFirstV = 0;
            fRadius = 3;

            using (Graphics g = Graphics.FromImage(tmpBMP))
            {
                g.InterpolationMode = InterpolationMode.Default;
                g.PixelOffsetMode = PixelOffsetMode.Half;

                for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
                {
                    if (TexViewModel.Groups[iGroupIdx].texFlag == 1 && TexViewModel.Groups[iGroupIdx].texID == iTexID)
                    {
                        for (iPolyIdx = TexViewModel.Groups[iGroupIdx].offsetPoly; 
                             iPolyIdx < TexViewModel.Groups[iGroupIdx].offsetPoly +
                                        TexViewModel.Groups[iGroupIdx].numPoly; iPolyIdx++)
                        {
                            // Get the 2D point (X,Y pos) of each of the points of the poly and draw the UV coordiantes with triangle shape
                            for (iVertCounter = 0; iVertCounter < 3; iVertCounter++)
                            {
                                fU = TexViewModel.TexCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter] +
                                                            TexViewModel.Groups[iGroupIdx].offsetTex].x;

                                iU = (int)(fU * 100000);
                                if (iU > 100000) fU -= (float)Math.Floor(fU);
                                if (iU < 0)
                                {
                                    if (iU > -100000) fU = 1.0f;
                                    else fU += (float)Math.Floor(Math.Abs(fU));
                                }

                                fU *= iWidth;

                                fV = TexViewModel.TexCoords[TexViewModel.Polys[iPolyIdx].Verts[iVertCounter] +
                                                            TexViewModel.Groups[iGroupIdx].offsetTex].y;

                                iV = (int)(fV * 100000);
                                if (iV > 100000) fV -= (float)Math.Floor(fV);
                                if (iV < 0)
                                {
                                    if (iV > -100000) fV = 1.0f;
                                    else fV += (float)Math.Floor(Math.Abs(fV));
                                }

                                fV *= iHeight;

                                // Draw the texture coordinates
                                using (Brush tmpBrush = new SolidBrush(Color.White))
                                {
                                    g.FillEllipse(tmpBrush, fU - fRadius, fV - fRadius, 2 * fRadius, 2 * fRadius);

                                    switch (iVertCounter)
                                    {
                                        case 0:
                                            fFirstU = fU;
                                            fFirstV = fV;
                                            break;

                                        case 1:
                                            fSecondU = fU;
                                            fSecondV = fV;

                                            break;

                                        case 2:
                                            using (Pen tmpPen = new Pen(tmpBrush))
                                            {
                                                g.DrawLine(tmpPen, new PointF(fU, fV), new PointF(fFirstU, fFirstV));
                                                g.DrawLine(tmpPen, new PointF(fFirstU, fFirstV), new PointF(fSecondU, fSecondV));
                                                g.DrawLine(tmpPen, new PointF(fSecondU, fSecondV), new PointF(fU, fV));
                                            }

                                            break;
                                    }
                                }
                            }
                        }
                    }
                }

            }

            pbTextureView.Image = tmpBMP;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            FieldRSDResource tmpfRSDResource;
            PModel tmpPModel;

            switch (modelType)
            {
                case K_HRC_SKELETON:
                    tmpfRSDResource = CopyRSDResource(fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece]);
                    tmpfRSDResource.Model.TexCoords = TexViewModel.TexCoords;
                    fSkeleton.bones[SelectedBone].fRSDResources[SelectedBonePiece] = CopyRSDResource(tmpfRSDResource);
                    break;

                case K_AA_SKELETON:
                case K_MAGIC_SKELETON:
                    //bSkeleton.bones[SelectedBone].Models[SelectedBonePiece] = CopyPModel(Model);
                    tmpPModel = CopyPModel(bSkeleton.bones[SelectedBone].Models[SelectedBonePiece]);
                    tmpPModel.TexCoords = TexViewModel.TexCoords;
                    bSkeleton.bones[SelectedBone].Models[SelectedBonePiece] = CopyPModel(tmpPModel);
                    break;
            }

            frmSkelEdit.panelModel_Paint(null, null);
        }

        private void btnFlipH_Click(object sender, EventArgs e)
        {
            int iGroupIdx, iVertCounter, iTexID;
            int iV;
            float fV;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {
                if (TexViewModel.Groups[iGroupIdx].texFlag == 1 && TexViewModel.Groups[iGroupIdx].texID == iTexID)
                {
                    // Now for each texture coordinate (UV) from offsetTex to numVert we must invert the Y (V) position for horizontal.
                    for (iVertCounter = TexViewModel.Groups[iGroupIdx].offsetTex;
                         iVertCounter < TexViewModel.Groups[iGroupIdx].offsetTex +
                                        TexViewModel.Groups[iGroupIdx].numVert; iVertCounter++)
                    {
                        // First we need to normalize the coordinate.
                        fV = TexViewModel.TexCoords[iVertCounter].y;
                        iV = (int)(fV * 100000);
                        if (iV > 100000) fV -= (float)Math.Floor(fV);
                        if (iV < 0)
                        {
                            if (iV > -100000) fV = 1.0f;
                            else fV += (float)Math.Floor(Math.Abs(fV));
                        }

                        TexViewModel.TexCoords[iVertCounter].y = 1.0f - fV;
                    }
                }
            }

            DrawUVs();
        }

        private void btnFlipV_Click(object sender, EventArgs e)
        {
            int iGroupIdx, iVertCounter, iTexID;
            int iU;
            float fU;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {
                if (TexViewModel.Groups[iGroupIdx].texFlag == 1 && TexViewModel.Groups[iGroupIdx].texID == iTexID)
                {
                    // Now for each texture coordinate (UV) from offsetTex to numVert we must invert the Y (V) position for horizontal.
                    for (iVertCounter = TexViewModel.Groups[iGroupIdx].offsetTex;
                         iVertCounter < TexViewModel.Groups[iGroupIdx].offsetTex +
                                        TexViewModel.Groups[iGroupIdx].numVert; iVertCounter++)
                    {
                        // First we need to normalize the coordinate.
                        fU = TexViewModel.TexCoords[iVertCounter].x;
                        iU = (int)(fU * 100000);
                        if (iU > 100000) fU -= (float)Math.Floor(fU);
                        if (iU < 0)
                        {
                            if (iU > -100000) fU = 1.0f;
                            else fU += (float)Math.Floor(Math.Abs(fU));
                        }

                        TexViewModel.TexCoords[iVertCounter].x = 1.0f - fU;
                    }
                }
            }

            DrawUVs();
        }

        private void btnRotateUV_Click(object sender, EventArgs e)
        {

            int iGroupIdx, iVertCounter, iTexID;
            int iU, iV;
            float fU, fV;

            iTexID = frmSkelEdit.cbTextureSelect.SelectedIndex;

            for (iGroupIdx = 0; iGroupIdx < TexViewModel.Header.numGroups; iGroupIdx++)
            {
                if (TexViewModel.Groups[iGroupIdx].texFlag == 1 && TexViewModel.Groups[iGroupIdx].texID == iTexID)
                {
                    // Now for each texture coordinate (UV) from offsetTex to numVert we must invert the Y (V) position for horizontal.
                    for (iVertCounter = TexViewModel.Groups[iGroupIdx].offsetTex;
                         iVertCounter < TexViewModel.Groups[iGroupIdx].offsetTex +
                                        TexViewModel.Groups[iGroupIdx].numVert; iVertCounter++)
                    {
                        // First we need to normalize the coordinate.
                        fU = TexViewModel.TexCoords[iVertCounter].x;
                        iU = (int)(fU * 100000);
                        if (iU > 100000) fU -= (float)Math.Floor(fU);
                        if (iU < 0)
                        {
                            if (iU > -100000) fU = 1.0f;
                            else fU += (float)Math.Floor(Math.Abs(fU));
                        }

                        fV = TexViewModel.TexCoords[iVertCounter].y;
                        iV = (int)(fV * 100000);
                        if (iV > 100000) fV -= (float)Math.Floor(fV);
                        if (iV < 0)
                        {
                            if (iV > -100000) fV = 1.0f;
                            else fV += (float)Math.Floor(Math.Abs(fV));
                        }

                        TexViewModel.TexCoords[iVertCounter].x = 1.0f - fV;
                        TexViewModel.TexCoords[iVertCounter].y = fU;

                    }
                }
            }

            DrawUVs();
        }
    }

}
