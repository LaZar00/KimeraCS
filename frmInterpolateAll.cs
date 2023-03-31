using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KimeraCS
{
    using static FF7Skeleton;
    using static FF7FieldSkeleton;
    using static FF7FieldAnimation;

    using static FF7BattleSkeleton;
    using static FF7BattleAnimationsPack;
    using static FF7BattleAnimation;

    using static FileTools;
    using static Utils;
    using static GDI32;

    public partial class FrmInterpolateAll : Form
    {

        public struct STSkeletonAnimationNames
        {
            public string strAnimation;
            public string strSkeleton;
        }

        private const int UNIQUE_CHAR_ANIMS_COUNT = 3209;
        private const int UNIQUE_BATTLE_ANIMS_COUNT = 391;
        private const int UNIQUE_MAGIC_ANIMS_COUNT = 79;

        public bool bCancelPressed;
        public int numTotalAnims;

        public FrmInterpolateAll()
        {
            InitializeComponent();          
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmInterpolateAll_Load(object sender, EventArgs e)
        {
            if (strCharLGPPathSrc == "") strCharLGPPathSrc = strGlobalPath;
            if (strCharLGPPathDest == "") strCharLGPPathDest = strGlobalPath;

            if (strBattleLGPPathSrc == "") strBattleLGPPathSrc = strGlobalPath;
            if (strBattleLGPPathDest == "") strBattleLGPPathDest = strGlobalPath;

            if (strMagicLGPPathSrc == "") strMagicLGPPathSrc = strGlobalPath;
            if (strMagicLGPPathDest == "") strMagicLGPPathDest = strGlobalPath;

            txtExtractedCharLGPSrc.Text = strCharLGPPathSrc;
            txtExtractedCharLGPDest.Text = strCharLGPPathDest;

            txtExtractedBattleLGPSrc.Text = strBattleLGPPathSrc;
            txtExtractedBattleLGPDest.Text = strBattleLGPPathDest;

            txtExtractedMagicLGPSrc.Text = strMagicLGPPathSrc;
            txtExtractedMagicLGPDest.Text = strMagicLGPPathDest;

            //  If ifalna.fil file not found, we will disable char.lgp animations processing.
            //  I prefer do this because the char.lgp animations are based in the ilfana.fil file included lists.
            //  If the file is not found, I can not know the relationship of skeleton/animation to do the interpolation.
            //  Another choice is to open flevel.lgp, but this would be more complicated to program.
            if (!bDBLoaded) chkProcessCharLGP.Enabled = false;

            bCancelPressed = false;

        }

        private void FrmInterpolateAll_Shown(object sender, EventArgs e)
        {
            nudInterpFrameField.Value = idefaultFieldInterpFrames;
            nudInterpFrameBattleMagic.Value = idefaultBattleInterpFrames;
        }

        private void BtnSaveOptions_Click(object sender, EventArgs e)
        {
            strCharLGPPathSrc = txtExtractedCharLGPSrc.Text;
            strCharLGPPathDest = txtExtractedCharLGPDest.Text;

            strBattleLGPPathSrc = txtExtractedBattleLGPSrc.Text;
            strBattleLGPPathDest = txtExtractedBattleLGPDest.Text;

            strMagicLGPPathSrc = txtExtractedMagicLGPSrc.Text;
            strMagicLGPPathDest = txtExtractedMagicLGPDest.Text;

            idefaultFieldInterpFrames = (int)nudInterpFrameField.Value;
            idefaultBattleInterpFrames = (int)nudInterpFrameBattleMagic.Value;

            WriteCFGFile();
        }

        private void BtnExtCharLGPSrc_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogEX fbdCharSrcDirectory = new FolderBrowserDialogEX();

            // We must select the directory from where to read the files.
            fbdCharSrcDirectory.folderBrowser.Description =
                            "Select the source path where all the CHAR.LGP files are:";

            fbdCharSrcDirectory.folderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            fbdCharSrcDirectory.folderBrowser.ShowNewFolderButton = false;

            if (strCharLGPPathSrc != "")
            {
                fbdCharSrcDirectory.folderBrowser.SelectedPath = strCharLGPPathSrc;
            }
            else
            {
                fbdCharSrcDirectory.folderBrowser.SelectedPath = strGlobalPath;
            }

            fbdCharSrcDirectory.Tmr.Start();
            if (fbdCharSrcDirectory.folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (fbdCharSrcDirectory.folderBrowser.SelectedPath != "")
                {
                    // Put Global folder for input unswizzled.
                    strCharLGPPathSrc = fbdCharSrcDirectory.folderBrowser.SelectedPath;
                    txtExtractedCharLGPSrc.Text = fbdCharSrcDirectory.folderBrowser.SelectedPath;
                }
            }

            fbdCharSrcDirectory.Dispose();
        }

        private void BtnExtCharLGPDst_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogEX fbdCharDestDirectory = new FolderBrowserDialogEX();

            // We must select the directory from where to read the files.
            fbdCharDestDirectory.folderBrowser.Description =
                            "Select the destination path where all the CHAR.LGP animation files will be put:";

            fbdCharDestDirectory.folderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            fbdCharDestDirectory.folderBrowser.ShowNewFolderButton = false;

            if (strCharLGPPathDest != "")
            {
                fbdCharDestDirectory.folderBrowser.SelectedPath = strCharLGPPathDest;
            }
            else
            {
                fbdCharDestDirectory.folderBrowser.SelectedPath = strGlobalPath;
            }

            fbdCharDestDirectory.Tmr.Start();
            if (fbdCharDestDirectory.folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (fbdCharDestDirectory.folderBrowser.SelectedPath != "")
                {
                    // Put Global folder for input unswizzled.
                    strCharLGPPathDest = fbdCharDestDirectory.folderBrowser.SelectedPath;
                    txtExtractedCharLGPDest.Text = fbdCharDestDirectory.folderBrowser.SelectedPath;
                }
            }

            fbdCharDestDirectory.Dispose();
        }

        private void BtnExtBattleLGPSrc_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogEX fbdBattleSrcDirectory = new FolderBrowserDialogEX();

            // We must select the directory from where to read the files.
            fbdBattleSrcDirectory.folderBrowser.Description =
                            "Select the source path where all the BATTLE.LGP files are:";

            fbdBattleSrcDirectory.folderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            fbdBattleSrcDirectory.folderBrowser.ShowNewFolderButton = false;

            if (strBattleLGPPathSrc != "")
            {
                fbdBattleSrcDirectory.folderBrowser.SelectedPath = strBattleLGPPathSrc;
            }
            else
            {
                fbdBattleSrcDirectory.folderBrowser.SelectedPath = strGlobalPath;
            }

            fbdBattleSrcDirectory.Tmr.Start();
            if (fbdBattleSrcDirectory.folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (fbdBattleSrcDirectory.folderBrowser.SelectedPath != "")
                {
                    // Put Global folder for input unswizzled.
                    strBattleLGPPathSrc = fbdBattleSrcDirectory.folderBrowser.SelectedPath;
                    txtExtractedBattleLGPSrc.Text = fbdBattleSrcDirectory.folderBrowser.SelectedPath;
                }
            }

            fbdBattleSrcDirectory.Dispose();
        }

        private void BtnExtBattleLGPDst_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogEX fbdBattleDestDirectory = new FolderBrowserDialogEX();

            // We must select the directory from where to read the files.
            fbdBattleDestDirectory.folderBrowser.Description =
                            "Select the destination path where all the BATTLE.LGP animation files will be put:";

            fbdBattleDestDirectory.folderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            fbdBattleDestDirectory.folderBrowser.ShowNewFolderButton = false;

            if (strBattleLGPPathDest != "")
            {
                fbdBattleDestDirectory.folderBrowser.SelectedPath = strBattleLGPPathDest;
            }
            else
            {
                fbdBattleDestDirectory.folderBrowser.SelectedPath = strGlobalPath;
            }

            fbdBattleDestDirectory.Tmr.Start();
            if (fbdBattleDestDirectory.folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (fbdBattleDestDirectory.folderBrowser.SelectedPath != "")
                {
                    // Put Global folder for input unswizzled.
                    strBattleLGPPathDest = fbdBattleDestDirectory.folderBrowser.SelectedPath;
                    txtExtractedBattleLGPDest.Text = fbdBattleDestDirectory.folderBrowser.SelectedPath;
                }
            }

            fbdBattleDestDirectory.Dispose();
        }

        private void BtnExtMagicLGPSrc_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogEX fbdMagicSrcDirectory = new FolderBrowserDialogEX();

            // We must select the directory from where to read the files.
            fbdMagicSrcDirectory.folderBrowser.Description =
                            "Select the source path where all the BATTLE.LGP files are:";

            fbdMagicSrcDirectory.folderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            fbdMagicSrcDirectory.folderBrowser.ShowNewFolderButton = false;

            if (strMagicLGPPathSrc != "")
            {
                fbdMagicSrcDirectory.folderBrowser.SelectedPath = strMagicLGPPathSrc;
            }
            else
            {
                fbdMagicSrcDirectory.folderBrowser.SelectedPath = strGlobalPath;
            }

            fbdMagicSrcDirectory.Tmr.Start();
            if (fbdMagicSrcDirectory.folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (fbdMagicSrcDirectory.folderBrowser.SelectedPath != "")
                {
                    // Put Global folder for input unswizzled.
                    strMagicLGPPathSrc = fbdMagicSrcDirectory.folderBrowser.SelectedPath;
                    txtExtractedMagicLGPSrc.Text = fbdMagicSrcDirectory.folderBrowser.SelectedPath;
                }
            }

            fbdMagicSrcDirectory.Dispose();
        }

        private void BtnExtMagicLGPDst_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogEX fbdMagicDestDirectory = new FolderBrowserDialogEX();

            // We must select the directory from where to read the files.
            fbdMagicDestDirectory.folderBrowser.Description =
                            "Select the destination path where all the BATTLE.LGP animation files will be put:";

            fbdMagicDestDirectory.folderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            fbdMagicDestDirectory.folderBrowser.ShowNewFolderButton = false;

            if (strMagicLGPPathDest != "")
            {
                fbdMagicDestDirectory.folderBrowser.SelectedPath = strMagicLGPPathDest;
            }
            else
            {
                fbdMagicDestDirectory.folderBrowser.SelectedPath = strGlobalPath;
            }

            fbdMagicDestDirectory.Tmr.Start();
            if (fbdMagicDestDirectory.folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (fbdMagicDestDirectory.folderBrowser.SelectedPath != "")
                {
                    // Put Global folder for input unswizzled.
                    strMagicLGPPathDest = fbdMagicDestDirectory.folderBrowser.SelectedPath;
                    txtExtractedMagicLGPDest.Text = fbdMagicDestDirectory.folderBrowser.SelectedPath;
                }
            }

            fbdMagicDestDirectory.Dispose();
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            bCancelPressed = false;
            btnCancel.Enabled = true;

            lblProcessing.Text = "Processing 0000 / 0000";
            gbProgress.Enabled = true;
            gbSettings.Enabled = false;

            try
            {
                InterpolateAllAnimations();
            }
            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;

                MessageBox.Show("There has been some exception error with Interpolate All Animations feature.",
                                "Error.", MessageBoxButtons.OK);

                //  Reactivate some object to continue working even with an exception
                bCancelPressed = false;

                ActivateSettings();
            }
        }

        public void ActivateSettings()
        {
            gbSettings.Enabled = true;
            gbProgress.Enabled = false;
            //lblProcessing.Text = "Processing 0000 / 0000";

            //progBarIntAllAnim.Value = 0;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            bCancelPressed = true;
            lblProcessing.Text = "Processing 0000 / 0000";
            progBarIntAllAnim.Value = 0;

            ActivateSettings();
        }

        public void ListAllUniqueCharAnimations(ref List<STSkeletonAnimationNames> lstUniqueCharSkAnim)
        {
            HashSet<string> hsUniqueCharAnims = new HashSet<string>();

            // Let's create a HashSet list (no duplicated animations) to help create the
            // Animation/Skeleton list to avoid interpolate duplicate animations.
            foreach (STCharLGPRegister itmRegister in lstCharLGPRegisters)
            {
                foreach (string itmAnim in itmRegister.lstAnims)
                {
                    if (hsUniqueCharAnims.Add(itmAnim))
                    {
                        STSkeletonAnimationNames itmSkAnim = new STSkeletonAnimationNames()
                        {
                            strAnimation = itmAnim + ".A",
                            strSkeleton = itmRegister.fileName + ".HRC",
                        };

                        lstUniqueCharSkAnim.Add(itmSkAnim);
                    }
                }
            }

            hsUniqueCharAnims.Clear();

            // Finally let's sort the list by animations
            lstUniqueCharSkAnim.Sort((x1, x2) => {
                                          int ret = x1.strSkeleton.CompareTo(x2.strSkeleton);
                                          return ret != 0 ? ret : x1.strAnimation.CompareTo(x2.strAnimation);
                                     });
        }

        public async void InterpolateAllAnimations()
        {
            int iCounter;

            //  Initialize some vars
            bCancelPressed = false;

            //  Initialize progress bar. Set Maximum of animations to process. It will depend on checked checkboxes
            numTotalAnims = chkProcessCharLGP.Checked ? UNIQUE_CHAR_ANIMS_COUNT : 0;
            numTotalAnims += chkProcessBattleLGP.Checked ? UNIQUE_BATTLE_ANIMS_COUNT : 0;
            numTotalAnims += chkProcessMagicLGP.Checked ? UNIQUE_MAGIC_ANIMS_COUNT : 0;

            iCounter = 1;
            progBarIntAllAnim.Value = 0;
            progBarIntAllAnim.Maximum = numTotalAnims;

            rtbLog.Clear();

            //  Let's process Char/Battle/Magic animations
            //  Char
            if (chkProcessCharLGP.Checked)
            {
                //  Declare some vars
                FieldSkeleton fSkeleton;
                FieldAnimation fAnimation;
                string oldSkeletonName;

                rtbLog.AppendText("===== PROCESSING CHAR.LGP ANIMATIONS =====\n");
                rtbLog.ScrollToCaret();

                //  Prepare an animation list with duples <ANIMATION, SKELETON> with unique animations
                List<STSkeletonAnimationNames> lstUniqueCharSkAnim = new List<STSkeletonAnimationNames>();
                ListAllUniqueCharAnimations(ref lstUniqueCharSkAnim);
                
                // This line helps to check a concrete field animation.
                //lstUniqueCharSkAnim.Add(new stSkeletonAnimationNames {strAnimation = "HBCF.A", strSkeleton = "HAGB.HRC" });

                //  Now for each skeleton, we have to load it, and:
                //     - load the animation and interpolate it.
                //     - get next item. if skeleton is different, load new skeleton, if not, load next animation.
                fSkeleton = new FieldSkeleton();
                fAnimation = new FieldAnimation();

                oldSkeletonName = "";

                foreach (STSkeletonAnimationNames itmSkAnim in lstUniqueCharSkAnim) 
                {
                    // Check if cancel has been pressed
                    if (bCancelPressed) break;

                    //  Read Skeleton (we need to check if we have to load a new skeleton
                    if (oldSkeletonName != itmSkAnim.strSkeleton)
                    {
                        if (File.Exists(txtExtractedCharLGPSrc.Text + "\\" + itmSkAnim.strSkeleton))
                        {
                            fSkeleton = new FieldSkeleton(txtExtractedCharLGPSrc.Text + "\\" + itmSkAnim.strSkeleton, false);
                        }
                        else
                        {
                            rtbLog.AppendText("NOT PROCESSED. Skeleton: " + itmSkAnim.strSkeleton +
                                              ", Animation: " + itmSkAnim.strAnimation +
                                              ", Entry: " + iCounter.ToString("0000") + ".\n");
                        }
                    }


                    if (File.Exists(txtExtractedCharLGPSrc.Text + "\\" + itmSkAnim.strAnimation))
                    {
                        //  Read Animation and fix it (Kimera VB6 have the fix processing here too)
                        fAnimation = new FieldAnimation(fSkeleton,
                                                        txtExtractedCharLGPSrc.Text + "\\" + itmSkAnim.strAnimation,
                                                        true);

                        //  Interpolate Animation
                        if (fAnimation.nBones == fSkeleton.nBones || fAnimation.nBones == 0)
                        {
                            await Task.Run(() => InterpolateFieldAnimation(ref fSkeleton, ref fAnimation, (int)nudInterpFrameField.Value, false));
                            Task.WaitAll();
                            await Task.Run(() => WriteFieldAnimation(fAnimation, strCharLGPPathDest + "\\" + itmSkAnim.strAnimation));
                            Task.WaitAll();

                            if (!chkShowOnlyNoProcessed.Checked)
                                rtbLog.AppendText("DONE. Skeleton: " + itmSkAnim.strSkeleton +
                                                  ", Animation: " + itmSkAnim.strAnimation +
                                                  ", Entry: " + iCounter.ToString("0000") + ".\n");
                        }
                        else
                        {
                            rtbLog.AppendText("NOT PROCESSED. Skeleton: " + itmSkAnim.strSkeleton +
                                              ", Animation: " + itmSkAnim.strAnimation +
                                              ", Entry: " + iCounter.ToString("0000") + ".\n");
                        }
                    }
                    else
                    {
                        rtbLog.AppendText("NOT PROCESSED. Skeleton: " + itmSkAnim.strSkeleton +
                                          ", Animation: " + itmSkAnim.strAnimation +
                                          ", Entry: " + iCounter.ToString("0000") + ".\n");
                    }


                    //  Last vars. Increase progress bar, save last animation name...
                    progBarIntAllAnim.PerformStep();
                    lblProcessing.Text = "Processing... " + iCounter.ToString("0000") + " / " +
                                                            numTotalAnims.ToString("0000");

                    oldSkeletonName = itmSkAnim.strSkeleton;
                    iCounter++;
                    
                    rtbLog.ScrollToCaret();
                    //Application.DoEvents();
                }

                if (chkProcessBattleLGP.Checked || chkProcessMagicLGP.Checked) rtbLog.AppendText("\n\n\n");
            }


            //  Battle
            if (chkProcessBattleLGP.Checked && !bCancelPressed)
            {
                //  Declare some vars
                BattleSkeleton bSkeleton;
                BattleAnimationsPack bAnimationsPack;
                string[] lstbAnimationsPackFiles;
                string baseBattleAnimationFileName;
                string strbSkeletonFullFileName = "";
                string strbAnimationsPackFullFileName = "";
                string strbAnimationsPackFullWriteFileName = "";

                rtbLog.AppendText("===== PROCESSING BATTLE.LGP ANIMATIONS =====\n");

                //  Get all the Battle Animation files (*DA)
                lstbAnimationsPackFiles = Directory.GetFiles(strBattleLGPPathSrc, "*DA", SearchOption.TopDirectoryOnly);

                // This line helps to check a concrete field animation.
                //lstbAnimationsPackFiles = new string[1] { "CCDA" };

                // Let's set the global var modelType to battle skeletons
                modelType = K_AA_SKELETON;

                foreach (string itmBattleAnimation in lstbAnimationsPackFiles)
                {
                    //  Check if cancel has been pressed
                    if (bCancelPressed) break;

                    //  Prepare file vars
                    baseBattleAnimationFileName = Path.GetFileName(itmBattleAnimation).ToUpper();
                    strbSkeletonFullFileName = strBattleLGPPathSrc + "\\" + baseBattleAnimationFileName[0] +
                                                                            baseBattleAnimationFileName[1] + "AA";
                    strbAnimationsPackFullFileName = strBattleLGPPathSrc + "\\" + baseBattleAnimationFileName;
                    strbAnimationsPackFullWriteFileName = strBattleLGPPathDest + "\\" + baseBattleAnimationFileName;

                    //  Load Battle Skeleton and Animations Pack files
                    bSkeleton = new BattleSkeleton(strbSkeletonFullFileName,
                                                   CanHaveLimitBreak(Path.GetFileNameWithoutExtension(strbSkeletonFullFileName).ToUpper()),
                                                   true);

                    //  Interpolate Animation
                    if (SameBattleAnimNumBones(strbAnimationsPackFullFileName, bSkeleton))
                    {
                        bAnimationsPack = new BattleAnimationsPack(bSkeleton, strbAnimationsPackFullFileName);

                        await Task.Run(() => InterpolateBattleAnimationsPack(ref bSkeleton, ref bAnimationsPack, (int)nudInterpFrameBattleMagic.Value, false));
                        Task.WaitAll();
                        await Task.Run(() => WriteBattleAnimationsPack(ref bAnimationsPack, strbAnimationsPackFullWriteFileName));
                        Task.WaitAll();

                        if (!chkShowOnlyNoProcessed.Checked)
                            rtbLog.AppendText("DONE. Skeleton: " + baseBattleAnimationFileName[0] +
                                                                    baseBattleAnimationFileName[1] + "AA" +
                                                ", Animation: " + baseBattleAnimationFileName +
                                                ", Entry: " + iCounter.ToString("0000") + ".\n");
                    }
                    else
                    {
                        rtbLog.AppendText("NOT PROCESSED. Skeleton: " + baseBattleAnimationFileName[0] +
                                                                    baseBattleAnimationFileName[1] + "AA" +
                                                ", Animation: " + baseBattleAnimationFileName +
                                                ", Entry: " + iCounter.ToString("0000") + ".\n");
                    }


                    //  Increase progress bar...
                    progBarIntAllAnim.PerformStep();
                    lblProcessing.Text = "Processing... " + iCounter.ToString("0000") + " / " +
                                                            numTotalAnims.ToString("0000");

                    iCounter++;

                    rtbLog.ScrollToCaret();

                }

                if (chkProcessMagicLGP.Checked) rtbLog.AppendText("\n\n\n");

            }


            //  Magic
            if (chkProcessMagicLGP.Checked && !bCancelPressed)
            {
                //  Declare some vars
                BattleSkeleton bSkeleton;
                BattleAnimationsPack bAnimationsPack;
                string[] lstbAnimationsPackFiles;
                string baseMagicAnimationFileName;
                string strbSkeletonFullFileName = "";
                string strbAnimationsPackFullFileName = "";
                string strbAnimationsPackFullWriteFileName = "";

                rtbLog.AppendText("===== PROCESSING MAGIC.LGP ANIMATIONS =====\n");

                //  Get all the Battle Animation files (*DA)
                lstbAnimationsPackFiles = Directory.GetFiles(strMagicLGPPathSrc, "*.A00", SearchOption.TopDirectoryOnly);

                // This line helps to check a concrete field animation.
                //lstbAnimationsPackFiles = new string[1] { "CCDA" };

                // Let's set the global var modelType to battle skeletons
                modelType = K_MAGIC_SKELETON;

                foreach (string itmMagicAnimation in lstbAnimationsPackFiles)
                {
                    //  Check if cancel has been pressed
                    if (bCancelPressed) break;

                    //  Prepare file vars
                    baseMagicAnimationFileName = Path.GetFileNameWithoutExtension(itmMagicAnimation).ToUpper();
                    strbSkeletonFullFileName = strMagicLGPPathSrc + "\\" + baseMagicAnimationFileName + ".D";
                    strbAnimationsPackFullFileName = strMagicLGPPathSrc + "\\" + baseMagicAnimationFileName + ".A00";
                    strbAnimationsPackFullWriteFileName = strMagicLGPPathDest + "\\" + baseMagicAnimationFileName + ".A00";

                    //  Load Battle Skeleton and Animations Pack files
                    if (File.Exists(strbSkeletonFullFileName))
                    {
                        bSkeleton = new BattleSkeleton(strbSkeletonFullFileName, true);

                        //  Interpolate Animation
                        if (SameBattleAnimNumBones(strbAnimationsPackFullFileName, bSkeleton))
                        {
                            bAnimationsPack = new BattleAnimationsPack(bSkeleton, strbAnimationsPackFullFileName);

                            await Task.Run(() => InterpolateBattleAnimationsPack(ref bSkeleton, ref bAnimationsPack, (int)nudInterpFrameBattleMagic.Value, false));
                            Task.WaitAll();
                            await Task.Run(() => WriteBattleAnimationsPack(ref bAnimationsPack, strbAnimationsPackFullWriteFileName));
                            Task.WaitAll();

                            if (!chkShowOnlyNoProcessed.Checked)
                                rtbLog.AppendText("DONE. Skeleton: " + baseMagicAnimationFileName + ".D" +
                                                  ", Animation: " + baseMagicAnimationFileName + ".A00" +
                                                  ", Entry: " + iCounter.ToString("0000") + ".\n");
                        }
                        else
                        {
                                rtbLog.AppendText("NOT PROCESSED. Skeleton: " + baseMagicAnimationFileName + ".D" +
                                                  ", Animation: " + baseMagicAnimationFileName + ".A00" +
                                                  ", Entry: " + iCounter.ToString("0000") + ".\n");
                        }
                    }
                    else
                    {
                        if (IsLimitAnimation(baseMagicAnimationFileName + ".A00"))
                            rtbLog.AppendText("LIMIT BREAK. Skeleton: " + baseMagicAnimationFileName + ".D" +
                                              ", Animation: " + baseMagicAnimationFileName + ".A00" +
                                              ", Entry: " + iCounter.ToString("0000") + ".\n");
                    }

                    //  Increase progress bar...
                    //progBarIntAllAnim.PerformStep();
                    progBarIntAllAnim.Value++;
                    lblProcessing.Text = "Processing... " + iCounter.ToString("0000") + " / " +
                                                            numTotalAnims.ToString("0000");

                    iCounter++;

                    rtbLog.ScrollToCaret();
                }
            }

            if (chkProcessMagicLGP.Checked) rtbLog.AppendText("===== JOB FINISHED =====");

            ActivateSettings();

            //            Private Sub InterpolateAllAnimations()

            //    If MagicLGPDataDirCheck.value = vbChecked And Not OperationCancelled Then
            //        anims_pack_filename = Dir(MAGIC_LGP_PATH + "\*.a00")

            //        PI = 0
            //        Do While anims_pack_filename > ""
            //            magic_anims_packs_names(PI) = anims_pack_filename
            //            anims_pack_filename = Dir()
            //            PI = PI + 1
            //        Loop

            //        For PI = 0 To UNIQUE_MAGIC_ANIMS_COUNT -1
            //            If OperationCancelled Then
            //                Exit For
            //            End If

            //            UpdateProgressBar base_percentage + (PI / UNIQUE_MAGIC_ANIMS_COUNT) / num_anim_groups, magic_anims_packs_names(PI)
            //            DoEvents
            //            Refresh

            //            anims_pack_filename = MAGIC_LGP_PATH + "\" + magic_anims_packs_names(PI)
            //            battle_skeleton_filename = Left$(anims_pack_filename, Len(anims_pack_filename) - 3) + "d"
            //            limit_owner_skeleton_filename = GetLimitCharacterFileName(magic_anims_packs_names(PI))
            //            If limit_owner_skeleton_filename<> "" Then
            //               ReadAASkeleton BATTLE_LGP_PATH + "\" + limit_owner_skeleton_filename, aa_sk, True, False
            //                ReadDAAnimationsPack anims_pack_filename, aa_sk.NumBones, 8, 8, da_anims_pack
            //            Else
            //                ReadMagicSkeleton battle_skeleton_filename, aa_sk, False
            //                ReadDAAnimationsPack anims_pack_filename, aa_sk.NumBones, aa_sk.NumBodyAnims, aa_sk.NumWeaponAnims, da_anims_pack
            //            End If

            //            InterpolateDAAnimationsPack aa_sk, da_anims_pack, NumInterpFramesBattleUpDown.value, False

            //            WriteDAAnimationsPack MAGIC_LGP_PATH_DEST + "\" + magic_anims_packs_names(PI), da_anims_pack
            //        Next
            //    End If

            //    If OperationCancelled Then
            //        MsgBox "Operation cancelled.", vbOKOnly, "Cancelled"
            //    Else
            //        UpdateProgressBar 1#, "Finished!"
            //        DoEvents
            //        Refresh
            //        MsgBox "Operation completed.", vbOKOnly, "Finished"
            //    End If
            //    Hide
            //End Sub
        }

        private void FrmInterpolateAll_FormClosing(object sender, FormClosingEventArgs e)
        {
            bCancelPressed = true;

            progBarIntAllAnim.Value = 0;
            rtbLog.Clear();

            lblProcessing.Text = "Processing... 0000 / 0000";
            gbSettings.Enabled = true;
            gbProgress.Enabled = false;
        }

        private void BtnSaveLog_Click(object sender, EventArgs e)
        {
            // Set filter options and filter index.
            
            saveFile.Title = "Save Interpolate All Animations Log";
            saveFile.Filter = "Text files|*.txt|All files|*.*";
            saveFile.FilterIndex = 1;
            saveFile.FileName = "Kimera_interpolate_all_animations.txt";

            // Initial Directory
            saveFile.InitialDirectory = strGlobalPath;

            try
            {
                // Process input if the user clicked OK.
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFile.FileName, rtbLog.Text);
                }
            }
            catch
            {
                MessageBox.Show("There has been some error saving the Interpolate All Animations Log.",
                                "Error", MessageBoxButtons.OK);
            }
        }





  
    }
}
