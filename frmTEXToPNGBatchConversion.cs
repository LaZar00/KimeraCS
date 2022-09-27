using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KimeraCS
{

    using static GDI32;
    using static FF7TEXTexture;
    using static FileTools;

    public partial class frmTEXToPNGBatchConversion : Form
    {

        public bool bCancelPressed;

        public frmTEXToPNGBatchConversion()
        {
            InitializeComponent();
        }

        private void frmTEXToPNGBatchConversion_Load(object sender, EventArgs e)
        {
            if (strGlobalPathTEX2PNGBatch == "") strGlobalPathTEX2PNGBatch = strGlobalPath;

            txtTEX2PNGBatchPath.Text = strGlobalPathTEX2PNGBatch;

            bCancelPressed = false;
        }

        private void btnSaveOptions_Click(object sender, EventArgs e)
        {
            strGlobalPathTEX2PNGBatch = txtTEX2PNGBatchPath.Text;

            WriteCFGFile();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogEX fbdTEXBatchPath = new FolderBrowserDialogEX();

            // We must select the directory from where to read the files.
            fbdTEXBatchPath.folderBrowser.Description =
                            "Select the source path from where to search the .TEX files:";

            fbdTEXBatchPath.folderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            fbdTEXBatchPath.folderBrowser.ShowNewFolderButton = false;

            if (strGlobalPathTEX2PNGBatch != "")
            {
                fbdTEXBatchPath.folderBrowser.SelectedPath = strGlobalPathTEX2PNGBatch;
            }
            else
            {
                fbdTEXBatchPath.folderBrowser.SelectedPath = strGlobalPath;
            }

            fbdTEXBatchPath.Tmr.Start();
            if (fbdTEXBatchPath.folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (fbdTEXBatchPath.folderBrowser.SelectedPath != "")
                {
                    // Put Global path.
                    strGlobalPathTEX2PNGBatch = fbdTEXBatchPath.folderBrowser.SelectedPath;
                    txtTEX2PNGBatchPath.Text = fbdTEXBatchPath.folderBrowser.SelectedPath;
                }
            }

            fbdTEXBatchPath.Dispose();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {

            lblProcessingT2P.Text = "Processing 0000 / 0000";

            try
            {
                if (Directory.Exists(txtTEX2PNGBatchPath.Text))
                {
                    bCancelPressed = false;
                    btnCancel.Enabled = true;

                    gbProgress.Enabled = true;
                    gbSettings.Enabled = false;

                    ProcessTEX2PNGBatch(txtTEX2PNGBatchPath.Text);
                }
                else
                    MessageBox.Show("The selected folder does not exists.", "Warning");
            }
            catch (Exception ex)
            {
                MessageBox.Show("There has been some exception error while processing TEX to PNG Batch Conversion feature.",
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
        }

        public static Bitmap PutPixelDataIntoBitmap32ARGB(TEX inTEX)
        {
            DirectBitmap tmpDBMP = new DirectBitmap(inTEX.width, inTEX.height);
            int i, iBMPValue, iBMPByteLength, iR, iG, iB, iA;

            iBMPByteLength = inTEX.pixelData.Length / inTEX.bytesPerPixel;

            if (inTEX.hasPal == 1)
            {
                for (i = 0; i < iBMPByteLength; i++)
                {
                    iB = inTEX.palette[inTEX.pixelData[i] * 4];
                    iG = inTEX.palette[inTEX.pixelData[i] * 4 + 1];
                    iR = inTEX.palette[inTEX.pixelData[i] * 4 + 2];
                    iA = inTEX.palette[inTEX.pixelData[i] * 4 + 3];

                    if (inTEX.ColorKeyFlag == 1 && iR == 0 && iG == 0 && iB == 0) iA = 0;

                    iBMPValue = (iA << 24) | (iR << 16) | (iG << 8) | iB;

                    tmpDBMP.Bits[i] = iBMPValue;
                }
            }
            else
            {
                for (i = 0; i < iBMPByteLength; i++)
                {
                    iB = inTEX.pixelData[i * inTEX.bytesPerPixel];
                    iG = inTEX.pixelData[i * inTEX.bytesPerPixel + 1];
                    iR = inTEX.pixelData[i * inTEX.bytesPerPixel + 2];
                    iA = 255;

                    if (inTEX.bitsPerPixel == 32) iA = inTEX.pixelData[i * inTEX.bytesPerPixel + 3];

                    if (inTEX.ColorKeyFlag == 1 && iR == 0 && iG == 0 && iB == 0) iA = 0;

                    iBMPValue = (iA << 24) | (iR << 16) | (iG << 8) | iB;

                    tmpDBMP.Bits[i] = iBMPValue;
                }
            }

            //tmpDBMP.Bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            return tmpDBMP.Bitmap;
        }

        public async void ProcessTEX2PNGBatch(string strMainPath)
        {
            int iCounter;
            TEX tmpTEX;
            Bitmap tmpBMP;
            SearchOption soRecursive;

            string[] strTEXFilesList;
            string strSavePNGFileName;

            try
            {
                if (chkRecursiveSearch.Checked)
                    soRecursive = SearchOption.AllDirectories;
                else
                    soRecursive = SearchOption.TopDirectoryOnly;

                strTEXFilesList = Directory
                    .EnumerateFiles(strMainPath, "*.*", soRecursive)
                    .Where(s => Regex.IsMatch(s.Substring(s.Length - 4, 4).ToUpper(), @".T\d{2}$") ||       // Summon models textures
                                s.ToUpper().EndsWith("AC") || s.ToUpper().EndsWith("AD") ||                 // Battle models textures
                                s.ToUpper().EndsWith("AE") || s.ToUpper().EndsWith("AF") ||
                                s.ToUpper().EndsWith("AG") || s.ToUpper().EndsWith("AH") ||
                                s.ToUpper().EndsWith("AI") || s.ToUpper().EndsWith("AJ") ||
                                s.ToUpper().EndsWith("AK") || s.ToUpper().EndsWith("AL") ||
                                s.ToUpper().EndsWith("TEX")                                                 // Field/World models textures
                    ).ToArray();


                // Init progressBar
                iCounter = 1;
                progBarTEXBatch.Value = iCounter;
                progBarTEXBatch.Maximum = strTEXFilesList.Count() + 1;

                rtbLog.Clear();

                rtbLog.AppendText("===== Processing TEX2PNG Batch Conversion =====\n");
                rtbLog.ScrollToCaret();

                foreach (string strTEXFile in strTEXFilesList)
                {
                    // Check if cancel has been pressed
                    if (bCancelPressed) break;

                    tmpTEX = new TEX();
                    // Load TEX file into variable
                    if (ReadTEXTexture(ref tmpTEX, strTEXFile) == 0)
                    {
                        // Convert to bitmap
                        tmpBMP = PutPixelDataIntoBitmap32ARGB(tmpTEX);

                        // Save to .png
                        if (Path.GetExtension(strTEXFile).Length > 2 &&
                            Path.GetExtension(strTEXFile).ToUpper() != ".TEX")
                        {
                            // Case for .T00
                            strSavePNGFileName = strTEXFile + ".png";
                        }
                        else
                        {
                            // Other cases
                            strSavePNGFileName = Path.GetDirectoryName(strTEXFile) + "\\" +
                                                 Path.GetFileNameWithoutExtension(strTEXFile) +
                                                 ".png";
                        }
                        tmpBMP.Save(strSavePNGFileName, ImageFormat.Png);

                        // Dispose
                        tmpBMP.Dispose();
                        UnloadTexture(ref tmpTEX);

                        if (!chkShowOnlyNoProcessed.Checked)
                            rtbLog.AppendText("PROCESSED. TEX File: " + strTEXFile +
                                              ", Entry: " + iCounter.ToString("0000") + ".\n");
                    }
                    else
                    {
                        rtbLog.AppendText("NOT PROCESSED. TEX File: " + strTEXFile +
                                          ", Entry: " + iCounter.ToString("0000") + ".\n");

                    }

                    //  Increase progress bar...
                    iCounter++;

                    progBarTEXBatch.Value = iCounter;
                    lblProcessingT2P.Text = "Processing... " +
                                            (iCounter - 1).ToString("0000") + " / " +
                                            strTEXFilesList.Count().ToString("0000");
                    lblProcessingT2P.Refresh();

                    rtbLog.ScrollToCaret();

                    await Task.Delay(5);
                }

                rtbLog.AppendText("===== Job Finished =====");

                ActivateSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There has been some exception while doing the TEX2PNG Batch Conversion.",
                                "Error");

                ActivateSettings();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bCancelPressed = true;
            lblProcessingT2P.Text = "Processing 0000 / 0000";
            progBarTEXBatch.Value = 0;

            ActivateSettings();
        }

        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            // Set filter options and filter index.

            saveFile.Title = "Save TEX2PNG Batch Conversion Log";
            saveFile.Filter = "Text files|*.txt|All files|*.*";
            saveFile.FilterIndex = 1;
            saveFile.FileName = "Kimera_TEX2PNG_batch_conversion.txt";

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
                MessageBox.Show("There has been some error saving the TEX2PNG Batch Conversion Log.",
                                "Error", MessageBoxButtons.OK);
            }
        }

        private void frmTEXToPNGBatchConversion_FormClosing(object sender, FormClosingEventArgs e)
        {
            bCancelPressed = true;

            progBarTEXBatch.Value = 0;
            rtbLog.Clear();

            lblProcessingT2P.Text = "Processing... 0000 / 0000";
            gbSettings.Enabled = true;
            gbProgress.Enabled = false;
        }
    }
}
