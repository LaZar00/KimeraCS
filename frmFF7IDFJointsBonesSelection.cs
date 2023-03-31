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
    public partial class FrmFF7IDFJointsBonesSelection : Form
    {

        readonly ToolTip toolTip1 = new ToolTip();

        public FrmFF7IDFJointsBonesSelection(List<string> strListJoints)
        {
            InitializeComponent();

            // That's right. Let's put tooltips to two buttons
            DefineToolTips();

            // Let's populate the checked list
            foreach (string strJoint in strListJoints)
            {
                chklbJointsBones.Items.Add(strJoint);
                chklbJointsBones.SetItemChecked(chklbJointsBones.Items.Count - 1, true);
            }

        }

        public void DefineToolTips()
        {
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 1000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 250;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(btnSelectAll, "Select all joints/bones");
            toolTip1.SetToolTip(btnUnselectAll, "Unselect all joints/bones");
        }


        private void BtnCancel_Click(object sender, EventArgs e)
        {
            chklbJointsBones.Items.Clear();
            Close();
        }

        public void SetListItemsState(bool bState)
        {
            int i;

            for (i = 0; i < chklbJointsBones.Items.Count; i++)
            {
                chklbJointsBones.SetItemChecked(i, bState);
            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            SetListItemsState(true);
        }

        private void BtnUnselectAll_Click(object sender, EventArgs e)
        {
            SetListItemsState(false);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
