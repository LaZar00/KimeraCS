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


        public void SetOptionsEnabled()
        {
            //chkV_WIREFRAME_OPTION.Enabled = chkV_WIREFRAME.Checked;
            //chkTrueV_TEX.Enabled = chkV_TEXTURE.Checked;
            //chkV_LINEARFILTER_OPTION.Enabled = chkV_LINEARFILTER.Checked;
            //chkV_NOCULL_OPTION.Enabled = chkV_NOCULL.Checked;
            //chkV_CULLFACE_CullFrontFace.Enabled = chkV_CULLFACE.Checked;
            //chkV_DEPTHTEST_OPTION.Enabled = chkV_DEPTHTEST.Checked;
            //chkV_DEPTHMASK_OPTION.Enabled = chkV_DEPTHMASK.Checked;
            //chkV_ALPHABLEND_OPTION.Enabled = chkV_ALPHABLEND.Checked;
            //chkV_SHADEMODE_Lighted.Enabled = chkV_SHADEMODE.Checked;
        }

        public void SetSelectedGroup()
        {
            int iFeatures, iOptions;

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
                    rb25P.Checked = true;
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

            cbSrcBlend.SelectedIndex = EditedPModel.Hundrets[SelectedGroup].srcblend;
            cbDestBlend.SelectedIndex = EditedPModel.Hundrets[SelectedGroup].destblend;

            iFeatures = EditedPModel.Hundrets[SelectedGroup].field_C;
            iOptions = EditedPModel.Hundrets[SelectedGroup].field_8;

            chkV_WIREFRAME.Checked = (iFeatures & 0x01) != 0;
            chkV_TEXTURE.Checked = (iFeatures & 0x02) != 0;
            chkV_LINEARFILTER.Checked = (iFeatures & 0x04) != 0;
            chkV_NOCULL.Checked = (iFeatures & 0x4000) != 0;
            chkV_CULLFACE.Checked = (iFeatures & 0x2000) != 0;
            chkV_DEPTHTEST.Checked = (iFeatures & 0x8000) != 0;
            chkV_DEPTHMASK.Checked = (iFeatures & 0x10000) != 0;
            chkV_ALPHABLEND.Checked = (iFeatures & 0x400) != 0;
            chkV_SHADEMODE.Checked = (iFeatures & 0x020000) != 0;

            chkV_WIREFRAME_OPTION.Checked = (iOptions & 0x1) != 0;
            chkV_TEXTURE_OPTION.Checked = (iOptions & 0x2) != 0;
            chkV_LINEARFILTER_OPTION.Checked = (iOptions & 0x4) != 0;
            chkV_NOCULL_OPTION.Checked = (iOptions & 0x4000) != 0;
            chkV_CULLFACE_CullFrontFace.Checked = (iOptions & 0x2000) != 0;
            chkV_DEPTHTEST_OPTION.Checked = (iOptions & 0x8000) != 0;
            chkV_DEPTHMASK_OPTION.Checked = (iOptions & 0x10000) != 0;
            chkV_ALPHABLEND_OPTION.Checked = (iOptions & 0x400) != 0;
            chkV_SHADEMODE_Lighted.Checked = (iOptions & 0x20000) != 0;

            SetOptionsEnabled();
        }

        private void ChkV_WIREFRAME_CheckedChanged(object sender, EventArgs e)
        {
            //SetOptionsEnabled();
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

            //SetOptionsEnabled();
        }

        private void ChkV_LINEARFILTER_CheckedChanged(object sender, EventArgs e)
        {
            //SetOptionsEnabled();
        }

        private void ChkV_NOCULL_CheckedChanged(object sender, EventArgs e)
        {
            //SetOptionsEnabled();
        }

        private void ChkV_CULLFACE_CheckedChanged(object sender, EventArgs e)
        {
            //SetOptionsEnabled();
        }

        private void ChkV_DEPTHTEST_CheckedChanged(object sender, EventArgs e)
        {
            //SetOptionsEnabled();
        }

        private void ChkV_DEPTHMASK_CheckedChanged(object sender, EventArgs e)
        {
            //SetOptionsEnabled();
        }

        private void ChkV_ALPHABLEND_CheckedChanged(object sender, EventArgs e)
        {
            //SetOptionsEnabled();
        }

        private void ChkV_SHADEMODE_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkV_SHADEMODE.Checked) rb1SM.Checked = true;

            //SetOptionsEnabled();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            int iFeatures, iOptions, mask, invMaskFull;

            if (rbAverage.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 0;
            if (rbAdditive.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 1;
            if (rbSubstractive.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 2;
            if (rb25P.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 3;
            if (rbNone.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 4;

            if (rb1.Checked) EditedPModel.Groups[SelectedGroup].polyType = 1;
            if (rb2.Checked) EditedPModel.Groups[SelectedGroup].polyType = 2;
            if (rb3.Checked) EditedPModel.Groups[SelectedGroup].polyType = 3;

            if (rb1SM.Checked) EditedPModel.Hundrets[SelectedGroup].shademode = 1;
            if (rb2SM.Checked) EditedPModel.Hundrets[SelectedGroup].shademode = 2;

            if (EditedPModel.Groups[SelectedGroup].polyType == 1) rb1.Checked = true;
            else if (EditedPModel.Groups[SelectedGroup].polyType == 2) rb2.Checked = true;
            else rb3.Checked = true;

            EditedPModel.Hundrets[SelectedGroup].srcblend = cbSrcBlend.SelectedIndex;
            EditedPModel.Hundrets[SelectedGroup].destblend = cbDestBlend.SelectedIndex;

            if (chkV_TEXTURE_OPTION.Checked)
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

            iFeatures = EditedPModel.Hundrets[SelectedGroup].field_C;
            iOptions = EditedPModel.Hundrets[SelectedGroup].field_8;

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
            iFeatures = (iFeatures & invMaskFull) | mask;

            mask = (chkV_WIREFRAME_OPTION.Checked ? 0x1 : 0) |
                   (chkV_TEXTURE_OPTION.Checked ? 0x2 : 0) |
                   (chkV_LINEARFILTER_OPTION.Checked ? 0x4 : 0) |
                   (chkV_NOCULL_OPTION.Checked ? 0x4000 : 0) |
                   (chkV_CULLFACE_CullFrontFace.Checked ? 0x2000 : 0) |
                   (chkV_DEPTHTEST_OPTION.Checked ? 0x8000 : 0) |
                   (chkV_DEPTHMASK_OPTION.Checked ? 0x10000 : 0) |
                   (chkV_ALPHABLEND_OPTION.Checked ? 0x400 : 0) |
                   (chkV_SHADEMODE_Lighted.Checked ? 0x20000 : 0);
            iOptions = (iOptions & invMaskFull) | mask;

            EditedPModel.Hundrets[SelectedGroup].field_C = iFeatures;
            EditedPModel.Hundrets[SelectedGroup].field_8 = iOptions;

            // We have to Refresh Group List (in case we changed some TexID)
            frmPEdit.FillGroupsList();

            frmPEdit.ChangeGroupEnable(false);
            frmPEdit.PanelEditorPModel_Paint(null, null);

            Close();
        }
    }
}
