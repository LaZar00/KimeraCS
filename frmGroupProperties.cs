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
    using static frmPEditor;

    public partial class frmGroupProperties : Form
    {

        public int SelectedGroup;

        public frmGroupProperties(int iGroupIdx)
        {
            InitializeComponent();

            SelectedGroup = iGroupIdx;
            SetSelectedGroup();
        }

        public void SetRSValuesEnabled()
        {
            chkTrueV_WIRE.Enabled = chkV_WIREFRAME.Checked;
            chkTrueV_TEX.Enabled = chkV_TEXTURE.Checked;
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

            changeRenderStateValues = EditedPModel.Hundrets[SelectedGroup].field_8;
            renderStateValues = EditedPModel.Hundrets[SelectedGroup].field_C;

            chkV_WIREFRAME.Checked = (changeRenderStateValues & 0x01) == 0 ? false : true;
            chkV_TEXTURE.Checked = (changeRenderStateValues & 0x02) == 0 ? false : true;
            chkV_LINEARFILTER.Checked = (changeRenderStateValues & 0x04) == 0 ? false : true;
            chkV_NOCULL.Checked = (changeRenderStateValues & 0x4000) == 0 ? false : true;
            chkV_CULLFACE.Checked = (changeRenderStateValues & 0x2000) == 0 ? false : true;
            chkV_DEPTHTEST.Checked = (changeRenderStateValues & 0x8000) == 0 ? false : true;
            chkV_DEPTHMASK.Checked = (changeRenderStateValues & 0x10000) == 0 ? false : true;
            chkV_ALPHABLEND.Checked = (changeRenderStateValues & 0x400) == 0 ? false : true;
            chkV_SHADEMODE.Checked = (changeRenderStateValues & 0x020000) == 0 ? false : true;

            chkTrueV_WIRE.Checked = (renderStateValues & 0x1) == 0 ? false : true;
            chkTrueV_TEX.Checked = (renderStateValues & 0x2) == 0 ? false : true;
            chkTrueV_LINEAR.Checked = (renderStateValues & 0x4) == 0 ? false : true;
            chkTrueV_NOCULL.Checked = (renderStateValues & 0x4000) == 0 ? false : true;
            chkCullBackFacing.Checked = (renderStateValues & 0x2000) == 0 ? false : true;
            chkTrueV_DEPTHTEST.Checked = (renderStateValues & 0x8000) == 0 ? false : true;
            chkTrueV_DEPTHMASK.Checked = (renderStateValues & 0x10000) == 0 ? false : true;
            chkTrueV_ALPHA.Checked = (renderStateValues & 0x400) == 0 ? false : true;
            chkLighted.Checked = (renderStateValues & 0x20000) == 0 ? false : true;

            SetRSValuesEnabled();
        }

        private void chkV_WIREFRAME_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void chkV_TEXTURE_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void chkV_LINEARFILTER_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void chkV_NOCULL_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void chkV_CULLFACE_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void chkV_DEPTHTEST_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void chkV_DEPTHMASK_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void chkV_ALPHABLEND_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void chkV_SHADEMODE_CheckedChanged(object sender, EventArgs e)
        {
            SetRSValuesEnabled();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            int changeRenderStateValues, renderStateValues, mask, invMaskFull;

            if (rbAverage.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 0;
            if (rbAdditive.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 1;
            if (rbSubstractive.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 2;
            if (rbUnknown.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 3;
            if (rbNone.Checked) EditedPModel.Hundrets[SelectedGroup].blend_mode = 4;

            if (EditedPModel.Groups[SelectedGroup].texFlag ==  1)
            {
                EditedPModel.Groups[SelectedGroup].texID = (int)nudTextureID.Value;
                EditedPModel.Hundrets[SelectedGroup].texID = (int)nudTextureID.Value;
            }

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

            Close();


            // We have changed the logic and close the Group Properties window when
            // accepting changes. So the next lines are not needed.

            //chkV_WIREFRAME.Checked = (changeRenderStateValues & 0x01) == 0 ? false : true;
            //chkV_TEXTURE.Checked = (changeRenderStateValues & 0x02) == 0 ? false : true;
            //chkV_LINEARFILTER.Checked = (changeRenderStateValues & 0x04) == 0 ? false : true;
            //chkV_NOCULL.Checked = (changeRenderStateValues & 0x4000) == 0 ? false : true;
            //chkV_CULLFACE.Checked = (changeRenderStateValues & 0x2000) == 0 ? false : true;
            //chkV_DEPTHTEST.Checked = (changeRenderStateValues & 0x8000) == 0 ? false : true;
            //chkV_DEPTHMASK.Checked = (changeRenderStateValues & 0x10000) == 0 ? false : true;
            //chkV_ALPHABLEND.Checked = (changeRenderStateValues & 0x400) == 0 ? false : true;
            //chkV_SHADEMODE.Checked = (changeRenderStateValues & 0x020000) == 0 ? false : true;

            //SetRSValuesEnabled();

            //chkTrueV_WIRE.Checked = (renderStateValues & 0x1) == 0 ? false : true;
            //chkTrueV_TEX.Checked = (renderStateValues & 0x2) == 0 ? false : true;
            //chkTrueV_LINEAR.Checked = (renderStateValues & 0x4) == 0 ? false : true;
            //chkTrueV_NOCULL.Checked = (renderStateValues & 0x4000) == 0 ? false : true;
            //chkCullBackFacing.Checked = (renderStateValues & 0x2000) == 0 ? false : true;
            //chkTrueV_DEPTHTEST.Checked = (renderStateValues & 0x8000) == 0 ? false : true;
            //chkTrueV_DEPTHMASK.Checked = (renderStateValues & 0x10000) == 0 ? false : true;
            //chkTrueV_ALPHA.Checked = (renderStateValues & 0x400) == 0 ? false : true;
            //chkLighted.Checked = (renderStateValues & 0x20000) == 0 ? false : true; 
        }
    }
}
