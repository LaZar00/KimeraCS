using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KimeraCS
{
    using static FrmPEditor;

    public partial class FrmGroupProperties : Form
    {

        readonly private FrmPEditor frmPEdit;

        public int SelectedGroup;

        public FrmGroupProperties(FrmPEditor frmPEdit, int iGroupIdx)
        {
            InitializeComponent();

            this.frmPEdit = frmPEdit;
            Owner = frmPEdit;

            DefineToolTips();

            SelectedGroup = iGroupIdx;
            SetSelectedGroup();
        }


        /////////////////////////////////////////////////////////////
        // ToolTip Helpers:
        // Create the ToolTip and associate with the Form container.
        readonly ToolTip toolTip1 = new ToolTip();

        public void DefineToolTips()
        {
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 250;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(gbPolygonType, "This tells which type of model you have. 1 - No texture, 2 - Textured with normals,  3 - Textured WITHOUT normals");
            toolTip1.SetToolTip(gbShadeMode, "This tells the Shade Mode for rendering to OpenGL. 1 - FLAT, 2 - SMOOTH. Not very relevant if we have V_SHADEMODE checked -normally 2-");
        }


        public void SetRSValuesEnabled()
        {
            chkTrueV_WIRE.Enabled = chkV_WIREFRAME.Checked;
            //chkTrueV_TEX.Enabled = chkV_TEXTURE.Checked;
            chkTrueV_LINEAR.Enabled = chkV_LINEARFILTER.Checked;
            chkTrueV_NOCULL.Enabled = chkV_NOCULL.Checked;
            chkCullBackFacing.Enabled = chkV_CULLFACE.Checked;
            chkTrueV_DEPTHTEST.Enabled = chkV_DEPTHTEST.Checked;
            chkTrueV_DEPTHMASK.Enabled = chkV_DEPTHMASK.Checked;
            chkTrueV_ALPHA.Enabled = chkV_ALPHABLEND.Checked;
            chkLighted.Enabled = chkV_SHADEMODE.Checked;
        }

        public void SetSelectedGroup()
        {
            int changeRenderStateValues, renderStateValues;

            Text = "Editing Group " + SelectedGroup.ToString("00");

            nudTextureID.Enabled = (EditedPModel.Groups[SelectedGroup].texFlag == 1);
            nudTextureID.Value = EditedPModel.Groups[SelectedGroup].texID;

            switch (EditedPModel.Hundrets[SelectedGroup].blend_mode)
            {
                case 0:
                    rbAverage.Checked = true;
                    break;

                case 1:
                    rbAdditive.Checked = true;
                    break;

                case 2:
                    rbSubstractive.Checked = true;
                    break;

                case 3:
                    rbUnknown.Checked = true;
                    break;

                case 4:
                    rbNone.Checked = true;
                    break;
            }

            if (EditedPModel.Groups[SelectedGroup].polyType == 1) rb1.Checked = true;
            else if (EditedPModel.Groups[SelectedGroup].polyType == 2) rb2.Checked = true;
            else rb3.Checked = true;

            if (EditedPModel.Hundrets[SelectedGroup].shademode == 1) rb1SM.Checked = true;
            else if (EditedPModel.Hundrets[SelectedGroup].shademode == 2) rb2SM.Checked = true;

            changeRenderStateValues = EditedPModel.Hundrets[SelectedGroup].field_8;
            renderStateValues = EditedPModel.Hundrets[SelectedGroup].field_C;

            chkV_WIREFRAME.Checked = (changeRenderStateValues & 0x01) != 0;
            chkV_TEXTURE.Checked = (changeRenderStateValues & 0x02) != 0;
            chkV_LINEARFILTER.Checked = (changeRenderStateValues & 0x04) != 0;
            chkV_NOCULL.Checked = (changeRenderStateValues & 0x4000) != 0;
            chkV_CULLFACE.Checked = (changeRenderStateValues & 0x2000) != 0;
            chkV_DEPTHTEST.Checked = (changeRenderStateValues & 0x8000) != 0;
            chkV_DEPTHMASK.Checked = (changeRenderStateValues & 0x10000) != 0;
            chkV_ALPHABLEND.Checked = (changeRenderStateValues & 0x400) != 0;
            chkV_SHADEMODE.Checked = (changeRenderStateValues & 0x020000) != 0;

            chkTrueV_WIRE.Checked = (renderStateValues & 0x1) != 0;
            chkTrueV_TEX.Checked = (renderStateValues & 0x2) != 0;
            chkTrueV_LINEAR.Checked = (renderStateValues & 0x4) != 0;
            chkTrueV_NOCULL.Checked = (renderStateValues & 0x4000) != 0;
            chkCullBackFacing.Checked = (renderStateValues & 0x2000) != 0;
            chkTrueV_DEPTHTEST.Checked = (renderStateValues & 0x8000) != 0;
            chkTrueV_DEPTHMASK.Checked = (renderStateValues & 0x10000) != 0;
            chkTrueV_ALPHA.Checked = (renderStateValues & 0x400) != 0;
            chkLighted.Checked = (renderStateValues & 0x20000) != 0;

            SetRSValuesEnabled();
        }

        private void ChkV_WIREFRAME_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void ChkV_TEXTURE_CheckedChanged(object sender, EventArgs e)
        {
            if (chkV_TEXTURE.Checked)
                nudTextureID.Enabled = true;
            else
            {
                nudTextureID.Enabled = false;
                rb1.Checked = true;
            }

            SetRSValuesEnabled();
        }

        private void ChkV_LINEARFILTER_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void ChkV_NOCULL_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void ChkV_CULLFACE_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void ChkV_DEPTHTEST_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void ChkV_DEPTHMASK_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void ChkV_ALPHABLEND_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void ChkV_SHADEMODE_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkV_SHADEMODE.Checked) rb1SM.Checked = true;

            SetRSValuesEnabled();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            int changeRenderStateValues, renderStateValues, mask, invMaskFull;

            if (rbAverage.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 0;
            if (rbAdditive.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 1;
            if (rbSubstractive.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 2;
            if (rbUnknown.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 3;
            if (rbNone.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 4;

            if (rb1.Checked) EditedPModel.Groups[SelectedGroup].polyType = 1;
            if (rb2.Checked) EditedPModel.Groups[SelectedGroup].polyType = 2;
            if (rb3.Checked) EditedPModel.Groups[SelectedGroup].polyType = 3;

            if (rb1SM.Checked) EditedPModel.Hundrets[SelectedGroup].shademode = 1;
            if (rb2SM.Checked) EditedPModel.Hundrets[SelectedGroup].shademode = 2;

            if (EditedPModel.Groups[SelectedGroup].polyType == 1) rb1.Checked = true;
            else if (EditedPModel.Groups[SelectedGroup].polyType == 2) rb2.Checked = true;
            else rb3.Checked = true;

            if (chkV_TEXTURE.Checked)
            {
                EditedPModel.Groups[SelectedGroup].texFlag = 1;
                EditedPModel.Groups[SelectedGroup].texID = (int)nudTextureID.Value;
                EditedPModel.Hundrets[SelectedGroup].texID = (int)nudTextureID.Value;
            }
            else
            {
                EditedPModel.Groups[SelectedGroup].texFlag = 0;
                EditedPModel.Groups[SelectedGroup].texID = 0;
                EditedPModel.Hundrets[SelectedGroup].texID = 0;
            }

            // Changed from KimeraVB with previous code.
            //if (EditedPModel.Groups[SelectedGroup].texFlag ==  1)
            //{
            //    EditedPModel.Groups[SelectedGroup].texID = (int)nudTextureID.Value;
            //    EditedPModel.Hundrets[SelectedGroup].texID = (int)nudTextureID.Value;
            //}

            changeRenderStateValues = EditedPModel.Hundrets[SelectedGroup].field_8;
            renderStateValues = EditedPModel.Hundrets[SelectedGroup].field_C;

            invMaskFull = ~(0x1 | 0x2 | 0x4 | 0x4000 | 0x2000 | 0x8000 | 0x10000 | 0x400 | 0x20000);

            mask = (chkV_WIREFRAME.Checked ? 0x1 : 0) |
                   (chkV_TEXTURE.Checked ? 0x2 : 0) |
                   (chkV_LINEARFILTER.Checked ? 0x4 : 0) |
                   (chkV_NOCULL.Checked ? 0x4000 : 0) |
                   (chkV_CULLFACE.Checked ? 0x2000 : 0) |
                   (chkV_DEPTHTEST.Checked ? 0x8000 : 0) |
                   (chkV_DEPTHMASK.Checked ? 0x10000 : 0) |
                   (chkV_ALPHABLEND.Checked ? 0x400 : 0) |
                   (chkV_SHADEMODE.Checked ? 0x20000 : 0);
            changeRenderStateValues = (changeRenderStateValues & invMaskFull) | mask;

            mask = (chkTrueV_WIRE.Checked ? 0x1 : 0) |
                   (chkTrueV_TEX.Checked ? 0x2 : 0) |
                   (chkTrueV_LINEAR.Checked ? 0x4 : 0) |
                   (chkTrueV_NOCULL.Checked ? 0x4000 : 0) |
                   (chkCullBackFacing.Checked ? 0x2000 : 0) |
                   (chkTrueV_DEPTHTEST.Checked ? 0x8000 : 0) |
                   (chkTrueV_DEPTHMASK.Checked ? 0x10000 : 0) |
                   (chkTrueV_ALPHA.Checked ? 0x400 : 0) |
                   (chkLighted.Checked ? 0x20000 : 0);
            renderStateValues = (renderStateValues & invMaskFull) | mask;
            
            EditedPModel.Hundrets[SelectedGroup].field_8 = changeRenderStateValues;
            EditedPModel.Hundrets[SelectedGroup].field_C = renderStateValues;

            // We have to Refresh Group List (in case we changed some TexID)
            frmPEdit.FillGroupsList();

            frmPEdit.ChangeGroupEnable(false);
            frmPEdit.PanelEditorPModel_Paint(null, null);

            Close();
        }
    }
}
