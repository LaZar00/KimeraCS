using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KimeraCS
{

    using static UndoRedo;
    using static UndoRedoPE;

    public static class FileTools
    {
        private const string CFG_FILE_NAME = "Kimera.cfg";

        public const string CHAR_LGP_FILTER_FILE_NAME = "ifalna.fil";
        public const string BATTLEENEMIES_LGP_FILTER_FILE_NAME = "ifalnab.fil";
        public const string BATTLELOCATIONS_LGP_FILTER_FILE_NAME = "ifalnal.fil";
        public const string BATTLEMAINPCS_LGP_FILTER_FILE_NAME = "ifalnap.fil";
        public const string MAGIC_LGP_FILTER_FILE_NAME = "ifalnam.fil";


        //public const string AERITH_BATTLE_SKELETON = "RVAA";
        //public const string BARRET_BATTLE_SKELETON = "SEAA";
        //public const string CAITSITH_BATTLE_SKELETON = "RYAA";
        //public const string CID_BATTLE_SKELETON = "RZAA";
        //public const string CLOUD_BATTLE_SKELETON = "RTAA";
        //public const string RED_BATTLE_SKELETON = "RWAA";
        //public const string TIFA_BATTLE_SKELETON = "RUAA";
        //public const string YUFFIE_BATTLE_SKELETON = "RXAA";
        //public const string VINCENT_BATTLE_SKELETON = "SGAA";

        //public const string FROG_BATTLE_SKELETON = "RSAA";
        //public const string CLOUD_BATTLE_SKELETON2 = "SIAA";
        //public const string SEPHIROTH_BATTLE_SKELETON = "SAAA";

        public struct STCharLGPRegister
        {
            public string fileName;
            public List<string> lstNames;
            public List<string> lstAnims;
        }

        public struct STBattleLGPRegister
        {
            public string fileName;
            public string modelName;
        }

        public struct STLimitsRegister
        {
            public List<string> lstModelNames;
            public List<string> lstLimitsAnimations;
        }

        public static Hashtable lstCFGKeys = new Hashtable();

        public static int numCharLGPRegisters;
        public static bool bDBLoaded, bDBEnemiesLoaded, bDBLocationsLoaded, bDBMainPCsLoaded, 
                           bDBMagicLoaded;

        public static bool bAdjust3DSImport, bDontCheckRepairPolys;
        public static int iPEFilterIdx;
        public static bool bShowAxesSkeletonWindow;

        public static List<STCharLGPRegister> lstCharLGPRegisters;
        public static List<STBattleLGPRegister> lstBattleEnemiesLGPRegisters;
        public static List<STBattleLGPRegister> lstBattleLocationsLGPRegisters;
        public static List<STBattleLGPRegister> lstBattleMainPCsLGPRegisters;
        public static List<STBattleLGPRegister> lstMagicLGPRegisters;

        public static List<STLimitsRegister> lstBattleLimitsAnimations;

        public static int idefaultFieldInterpFrames = 1;
        public static int idefaultBattleInterpFrames = 3;
        
        public static int isizeWindowWidth = 750;
        public static int isizeWindowHeight = 688;
        public static int iwindowPosX = 0;
        public static int iwindowPosY = 0;

        public static string strCharLGPPathSrc = "", strBattleLGPPathSrc = "", strMagicLGPPathSrc = "";
        public static string strCharLGPPathDest = "", strBattleLGPPathDest = "", strMagicLGPPathDest = "";
        public static string strGlobalPathTEX2PNGBatch = "";

        public static string strGlobalPath = "";
        public static string strGlobalPathPModelFolder = "", strGlobalPModelName = "";
        public static string strGlobalPathRSDResourceFolder = "", strGlobalRSDResourceName = "";
        public static string strGlobalPathTMDModelFolder = "", strGlobalTMDModelName = "";
        public static string strGlobalPath3DSModelFolder = "", strGlobal3DSModelName = "";
        public static string strGlobalPathFieldSkeletonFolder = "", strGlobalFieldSkeletonFileName = "", strGlobalFieldSkeletonName = "", 
                             strGlobalPathFieldAnimationFolder = "", strGlobalFieldAnimationName = "";
        public static string strGlobalPathBattleSkeletonFolder = "", strGlobalBattleSkeletonFileName = "", strGlobalBattleSkeletonName = "", 
                             strGlobalPathBattleAnimationFolder = "", strGlobalBattleAnimationName = "";
        public static string strGlobalPathMagicSkeletonFolder = "", strGlobalMagicSkeletonFileName = "", strGlobalMagicSkeletonName = "",
                             strGlobalPathMagicAnimationFolder = "", strGlobalMagicAnimationName = "";
        public static string strGlobalPathTextureFolder = "", strGlobalTextureName = "";
        public static string strGlobalPathPartModelFolder = "", strGlobalPartModelName = "";    // Add Piece option
        public static string strGlobalPathSaveSkeletonFolder = "", strGlobalPathSaveAnimationFolder = "", 
                             strGlobalPathSaveModelFolder = "", strGlobalPathSaveTMDFolder = "";

        // PEditor
        public static int isizeWindowWidthPE = 736;
        public static int isizeWindowHeightPE = 592;
        public static int iwindowPosXPE = 0;
        public static int iwindowPosYPE = 0;

        public static string strGlobalPathPModelFolderPE = "", strGlobalPModelNamePE = "";
        public static string strGlobalPathSaveModelFolderPE = "";


        public static void PrepareCFGKeys()
        {
            lstCFGKeys.Add("DEFAULT_FIELD_INTERP_FRAMES", "");
            lstCFGKeys.Add("DEFAULT_BATTLE_INTERP_FRAMES", "");

            lstCFGKeys.Add("LGP_BATTLE_PATH", "");
            lstCFGKeys.Add("LGP_BATTLE_PATH_DEST", "");
            lstCFGKeys.Add("LGP_CHAR_PATH", "");
            lstCFGKeys.Add("LGP_CHAR_PATH_DEST", "");
            lstCFGKeys.Add("LGP_MAGIC_PATH", "");
            lstCFGKeys.Add("LGP_MAGIC_PATH_DEST", "");

            lstCFGKeys.Add("PATH_3DSMODEL_FOLDER", "");
            lstCFGKeys.Add("PATH_BATTLESKELETON_FOLDER", "");
            lstCFGKeys.Add("PATH_BATTLEANIMATION_FOLDER", "");
            lstCFGKeys.Add("PATH_FIELDSKELETON_FOLDER", "");
            lstCFGKeys.Add("PATH_FIELDANIMATION_FOLDER", "");
            lstCFGKeys.Add("PATH_MAGICSKELETON_FOLDER", "");
            lstCFGKeys.Add("PATH_MAGICANIMATION_FOLDER", "");
            lstCFGKeys.Add("PATH_PARTMODEL_FOLDER", "");
            lstCFGKeys.Add("PATH_PMODEL_FOLDER", "");
            lstCFGKeys.Add("PATH_TMDMODEL_FOLDER", "");
            lstCFGKeys.Add("PATH_RSDRESOURCE_FOLDER", "");
            lstCFGKeys.Add("PATH_PMODEL_FOLDERPE", "");
            lstCFGKeys.Add("PATH_SAVESKELETON_FOLDER", "");
            lstCFGKeys.Add("PATH_SAVEANIMATION_FOLDER", "");
            lstCFGKeys.Add("PATH_SAVEMODEL_FOLDER", "");
            lstCFGKeys.Add("PATH_SAVETMDMODEL_FOLDER", "");
            lstCFGKeys.Add("PATH_SAVEMODEL_FOLDERPE", "");
            lstCFGKeys.Add("PATH_TEXTURE_FOLDER", "");
            lstCFGKeys.Add("PATH_TEX2PNGBATCH_FOLDER", "");

            lstCFGKeys.Add("WINDOW_POSX", "");
            lstCFGKeys.Add("WINDOW_POSY", "");
            lstCFGKeys.Add("WINDOWSIZE_WIDTH", "");
            lstCFGKeys.Add("WINDOWSIZE_HEIGHT", "");

            lstCFGKeys.Add("WINDOW_POSXPE", "");
            lstCFGKeys.Add("WINDOW_POSYPE", "");
            lstCFGKeys.Add("WINDOWSIZE_WIDTHPE", "");
            lstCFGKeys.Add("WINDOWSIZE_HEIGHTPE", "");

            lstCFGKeys.Add("UNDO_BUFFER_CAPACITY", "");
            lstCFGKeys.Add("UNDO_BUFFERPE_CAPACITY", "");

            lstCFGKeys.Add("ADJUST_3DS_IMPORT", "");
            lstCFGKeys.Add("PELOAD_3DS_FILTER_INDEX", "");

            lstCFGKeys.Add("SHOW_AXES_SKELETON_WINDOW", "");
            lstCFGKeys.Add("DONT_CHECK_REPAIR_POLYS", "");
        }



        //  ---------------------------------------------------------------------------------------------------------
        //  ----------------------------------- Kimera.cfg Functions ------------------------------------------------
        //  ---------------------------------------------------------------------------------------------------------
        public static void ReadCFGFile()
        {
            string strCFGFileName = strGlobalPath + "\\" + CFG_FILE_NAME;
            string[] lines;

            PrepareCFGKeys();
            PrepareLimitsFilterFile();

            // if Kimera.CFG exists, let's read the keys
            if (File.Exists(strCFGFileName))
            {
                lines = File.ReadAllLines(strCFGFileName);

                foreach (string strLine in lines)
                {
                    if (strLine.Length > 13)
                    {
                        string[] strLineSplit = strLine.Split('=');

                        if (strLineSplit.Length > 1)
                        {
                            lstCFGKeys[strLineSplit[0]] = strLineSplit[1];
                        }
                    }
                }

                if (!Int32.TryParse(lstCFGKeys["DEFAULT_FIELD_INTERP_FRAMES"].ToString(), out idefaultFieldInterpFrames)) idefaultFieldInterpFrames = 1;
                if (!Int32.TryParse(lstCFGKeys["DEFAULT_BATTLE_INTERP_FRAMES"].ToString(), out idefaultBattleInterpFrames)) idefaultBattleInterpFrames = 3;

                strCharLGPPathSrc = lstCFGKeys["LGP_CHAR_PATH"].ToString();
                strBattleLGPPathSrc = lstCFGKeys["LGP_BATTLE_PATH"].ToString();
                strMagicLGPPathSrc = lstCFGKeys["LGP_MAGIC_PATH"].ToString();
                strCharLGPPathDest = lstCFGKeys["LGP_CHAR_PATH_DEST"].ToString();
                strBattleLGPPathDest = lstCFGKeys["LGP_BATTLE_PATH_DEST"].ToString();
                strMagicLGPPathDest = lstCFGKeys["LGP_MAGIC_PATH_DEST"].ToString();

                strGlobalPathFieldSkeletonFolder = lstCFGKeys["PATH_FIELDSKELETON_FOLDER"].ToString();
                strGlobalPathFieldAnimationFolder = lstCFGKeys["PATH_FIELDANIMATION_FOLDER"].ToString();
                strGlobalPathBattleSkeletonFolder = lstCFGKeys["PATH_BATTLESKELETON_FOLDER"].ToString();
                strGlobalPathBattleAnimationFolder = lstCFGKeys["PATH_BATTLEANIMATION_FOLDER"].ToString();
                strGlobalPathMagicSkeletonFolder = lstCFGKeys["PATH_MAGICSKELETON_FOLDER"].ToString();
                strGlobalPathMagicAnimationFolder = lstCFGKeys["PATH_MAGICANIMATION_FOLDER"].ToString();
                strGlobalPathPModelFolder = lstCFGKeys["PATH_PMODEL_FOLDER"].ToString();
                strGlobalPathRSDResourceFolder = lstCFGKeys["PATH_RSDRESOURCE_FOLDER"].ToString();
                strGlobalPathPModelFolderPE = lstCFGKeys["PATH_PMODEL_FOLDERPE"].ToString();
                strGlobalPath3DSModelFolder = lstCFGKeys["PATH_3DSMODEL_FOLDER"].ToString();
                strGlobalPathTextureFolder = lstCFGKeys["PATH_TEXTURE_FOLDER"].ToString();
                strGlobalPathPartModelFolder = lstCFGKeys["PATH_PARTMODEL_FOLDER"].ToString();
                strGlobalPathSaveSkeletonFolder = lstCFGKeys["PATH_SAVESKELETON_FOLDER"].ToString();
                strGlobalPathSaveAnimationFolder = lstCFGKeys["PATH_SAVEANIMATION_FOLDER"].ToString();
                strGlobalPathSaveModelFolder = lstCFGKeys["PATH_SAVEMODEL_FOLDER"].ToString();
                strGlobalPathSaveModelFolderPE = lstCFGKeys["PATH_SAVEMODEL_FOLDERPE"].ToString();
                strGlobalPathTMDModelFolder = lstCFGKeys["PATH_TMDMODEL_FOLDER"].ToString();
                strGlobalPathSaveTMDFolder = lstCFGKeys["PATH_SAVETMDMODEL_FOLDER"].ToString();
                strGlobalPathTEX2PNGBatch = lstCFGKeys["PATH_TEX2PNGBATCH_FOLDER"].ToString();
                

                if (!Int32.TryParse(lstCFGKeys["UNDO_BUFFER_CAPACITY"].ToString(), out iUndoBufferCapacity)) iUndoBufferCapacity = 10;
                if (!Int32.TryParse(lstCFGKeys["UNDO_BUFFERPE_CAPACITY"].ToString(), out iUndoBufferPECapacity)) iUndoBufferPECapacity = 20;
                if (iUndoBufferCapacity <= 0) iUndoBufferCapacity = 10;
                if (iUndoBufferPECapacity <= 0) iUndoBufferPECapacity = 20;

                if (!Int32.TryParse(lstCFGKeys["WINDOW_POSX"].ToString(), out iwindowPosX)) iwindowPosX = 0;
                if (!Int32.TryParse(lstCFGKeys["WINDOW_POSY"].ToString(), out iwindowPosY)) iwindowPosY = 0;
                if (!Int32.TryParse(lstCFGKeys["WINDOWSIZE_WIDTH"].ToString(), out isizeWindowWidth)) isizeWindowWidth = 750;
                if (!Int32.TryParse(lstCFGKeys["WINDOWSIZE_HEIGHT"].ToString(), out isizeWindowHeight)) isizeWindowHeight = 688;

                if (!Int32.TryParse(lstCFGKeys["WINDOW_POSXPE"].ToString(), out iwindowPosXPE)) iwindowPosXPE = 0;
                if (!Int32.TryParse(lstCFGKeys["WINDOW_POSYPE"].ToString(), out iwindowPosYPE)) iwindowPosYPE = 0;
                if (!Int32.TryParse(lstCFGKeys["WINDOWSIZE_WIDTHPE"].ToString(), out isizeWindowWidthPE)) isizeWindowWidthPE = 736;
                if (!Int32.TryParse(lstCFGKeys["WINDOWSIZE_HEIGHTPE"].ToString(), out isizeWindowHeightPE)) isizeWindowHeightPE = 592;

                if (!bool.TryParse(lstCFGKeys["ADJUST_3DS_IMPORT"].ToString(), out bAdjust3DSImport)) bAdjust3DSImport = false;
                if (!Int32.TryParse(lstCFGKeys["PELOAD_3DS_FILTER_INDEX"].ToString(), out iPEFilterIdx)) iPEFilterIdx = -1;

                if (!bool.TryParse(lstCFGKeys["SHOW_AXES_SKELETON_WINDOW"].ToString(), out bShowAxesSkeletonWindow)) bShowAxesSkeletonWindow = false;
                if (!bool.TryParse(lstCFGKeys["DONT_CHECK_REPAIR_POLYS"].ToString(), out bDontCheckRepairPolys)) bDontCheckRepairPolys = false;
            }
        }

        public static void WriteCFGFile()
        {
            string strCFGFileName = strGlobalPath + "\\" + CFG_FILE_NAME;
            StringBuilder sbCFGLines = new StringBuilder();

            // Write Kimera.CFG
            lstCFGKeys["DEFAULT_FIELD_INTERP_FRAMES"] = idefaultFieldInterpFrames;
            lstCFGKeys["DEFAULT_BATTLE_INTERP_FRAMES"] = idefaultBattleInterpFrames;

            lstCFGKeys["LGP_CHAR_PATH"] = strCharLGPPathSrc;
            lstCFGKeys["LGP_BATTLE_PATH"] = strBattleLGPPathSrc;
            lstCFGKeys["LGP_MAGIC_PATH"] = strMagicLGPPathSrc;
            lstCFGKeys["LGP_CHAR_PATH_DEST"] = strCharLGPPathDest;
            lstCFGKeys["LGP_BATTLE_PATH_DEST"] = strBattleLGPPathDest;
            lstCFGKeys["LGP_MAGIC_PATH_DEST"] = strMagicLGPPathDest;

            lstCFGKeys["PATH_FIELDSKELETON_FOLDER"] = strGlobalPathFieldSkeletonFolder;
            lstCFGKeys["PATH_FIELDANIMATION_FOLDER"] = strGlobalPathFieldAnimationFolder;
            lstCFGKeys["PATH_BATTLESKELETON_FOLDER"] = strGlobalPathBattleSkeletonFolder;
            lstCFGKeys["PATH_BATTLEANIMATION_FOLDER"] = strGlobalPathBattleAnimationFolder;
            lstCFGKeys["PATH_MAGICSKELETON_FOLDER"] = strGlobalPathMagicSkeletonFolder;
            lstCFGKeys["PATH_MAGICANIMATION_FOLDER"] = strGlobalPathMagicAnimationFolder;
            lstCFGKeys["PATH_PMODEL_FOLDER"] = strGlobalPathPModelFolder;
            lstCFGKeys["PATH_RSDRESOURCE_FOLDER"] = strGlobalPathRSDResourceFolder;
            lstCFGKeys["PATH_TMDMODEL_FOLDER"] = strGlobalPathTMDModelFolder;
            lstCFGKeys["PATH_PMODEL_FOLDERPE"] = strGlobalPathPModelFolderPE;
            lstCFGKeys["PATH_3DSMODEL_FOLDER"] = strGlobalPath3DSModelFolder;
            lstCFGKeys["PATH_TEXTURE_FOLDER"] = strGlobalPathTextureFolder;
            lstCFGKeys["PATH_PARTMODEL_FOLDER"] = strGlobalPathPartModelFolder;
            lstCFGKeys["PATH_SAVESKELETON_FOLDER"] = strGlobalPathSaveSkeletonFolder;
            lstCFGKeys["PATH_SAVEANIMATION_FOLDER"] = strGlobalPathSaveAnimationFolder;
            lstCFGKeys["PATH_SAVEMODEL_FOLDER"] = strGlobalPathSaveModelFolder;
            lstCFGKeys["PATH_SAVETMDMODEL_FOLDER"] = strGlobalPathSaveTMDFolder;
            lstCFGKeys["PATH_SAVEMODEL_FOLDERPE"] = strGlobalPathSaveModelFolderPE;
            lstCFGKeys["PATH_TEX2PNGBATCH_FOLDER"] = strGlobalPathTEX2PNGBatch;


            lstCFGKeys["UNDO_BUFFER_CAPACITY"] = iUndoBufferCapacity;
            lstCFGKeys["UNDO_BUFFERPE_CAPACITY"] = iUndoBufferPECapacity;

            lstCFGKeys["WINDOW_POSX"] = iwindowPosX.ToString();
            lstCFGKeys["WINDOW_POSY"] = iwindowPosY.ToString();
            lstCFGKeys["WINDOWSIZE_WIDTH"] = isizeWindowWidth.ToString();
            lstCFGKeys["WINDOWSIZE_HEIGHT"] = isizeWindowHeight.ToString();

            lstCFGKeys["WINDOW_POSXPE"] = iwindowPosXPE.ToString();
            lstCFGKeys["WINDOW_POSYPE"] = iwindowPosYPE.ToString();
            lstCFGKeys["WINDOWSIZE_WIDTHPE"] = isizeWindowWidthPE.ToString();
            lstCFGKeys["WINDOWSIZE_HEIGHTPE"] = isizeWindowHeightPE.ToString();

            lstCFGKeys["ADJUST_3DS_IMPORT"] = bAdjust3DSImport.ToString();
            lstCFGKeys["PELOAD_3DS_FILTER_INDEX"] = iPEFilterIdx.ToString();

            lstCFGKeys["SHOW_AXES_SKELETON_WINDOW"] = bShowAxesSkeletonWindow.ToString();
            lstCFGKeys["DONT_CHECK_REPAIR_POLYS"] = bDontCheckRepairPolys.ToString();

            // Write Kimera.cfg
            var orderedCFGKeys = lstCFGKeys.Keys.Cast<string>().OrderBy(c => c);

            foreach (string strCFGKey in orderedCFGKeys)
            {
                sbCFGLines.AppendLine(strCFGKey + "=" + lstCFGKeys[strCFGKey]);
            }

            File.WriteAllText(strCFGFileName, sbCFGLines.ToString());
        }



        //  ---------------------------------------------------------------------------------------------------------
        //  -------------------------------------- ilfana Functions -------------------------------------------------
        //  ---------------------------------------------------------------------------------------------------------
        public static int ReadCharFilterFile()
        {
            int iResult = 1;
            string strFileName, strLastFileName;
            string strKey;
            string[] strLinesFilterFile;
            STCharLGPRegister stcLGPReg;

            try
            {
                strFileName = strGlobalPath + "\\" + CHAR_LGP_FILTER_FILE_NAME;                

                if (File.Exists(strFileName))
                {

                    lstCharLGPRegisters = new List<STCharLGPRegister>();
                    stcLGPReg = new STCharLGPRegister();

                    strLinesFilterFile = File.ReadAllLines(strFileName);
                    strLastFileName = "";

                    foreach (string strLineFilter in strLinesFilterFile)
                    {
                        if (strLineFilter.Length > 0)
                        {
                            strFileName = strLineFilter.Substring(0, 4);

                            if (strLastFileName != strFileName)
                            {
                                if (numCharLGPRegisters > 0)
                                {
                                    if (stcLGPReg.lstAnims != null) stcLGPReg.lstAnims.Sort();
                                    lstCharLGPRegisters.Add(stcLGPReg);
                                    stcLGPReg = new STCharLGPRegister();
                                }

                                numCharLGPRegisters += 1;

                                stcLGPReg.fileName = strFileName;
                            }

                            strKey = strLineFilter.Substring(4, 5);

                            if (strKey == "Names")
                            {
                                if (stcLGPReg.lstNames == null) stcLGPReg.lstNames = strLineFilter.Split('=')[1].Split(',').ToList();
                                else stcLGPReg.lstNames.AddRange(strLineFilter.Split('=')[1].Split(',').ToList());
                                
                                stcLGPReg.lstNames.RemoveAt(stcLGPReg.lstNames.Count - 1);
                            }
                            else if (strKey == "Anims")
                            {
                                if (stcLGPReg.lstAnims == null) stcLGPReg.lstAnims = strLineFilter.Split('=')[1].Split(',').ToList();
                                else stcLGPReg.lstAnims.AddRange(strLineFilter.Split('=')[1].Split(',').ToList());
                                
                                stcLGPReg.lstAnims.RemoveAt(stcLGPReg.lstAnims.Count - 1);
                            }

                            strLastFileName = strFileName;
                        }
                    }

                    lstCharLGPRegisters.Add(stcLGPReg);
                    lstCharLGPRegisters.Sort((fN1, fN2) => fN1.fileName.CompareTo(fN2.fileName));
                }
                else
                {
                    iResult = 0;
                }
            }
            catch
            {

                iResult = -1;
            }

            return iResult;
        }

        public static int ReadBattleFilterFile(string fileName, ref List<STBattleLGPRegister> tmpBattleLGPRegister)
        {
            int iResult = 1;
            string strFileName;
            string[] strLinesFilterFile;
            STBattleLGPRegister stcBLGPReg;

            try
            {
                strFileName = strGlobalPath + "\\" + fileName;

                if (File.Exists(strFileName))
                {

                    strLinesFilterFile = File.ReadAllLines(strFileName);
                    tmpBattleLGPRegister = new List<STBattleLGPRegister>();

                    foreach (string strLineFilter in strLinesFilterFile)
                    {
                        if (strLineFilter.Length > 0)
                        {
                            stcBLGPReg = new STBattleLGPRegister()
                            {
                                fileName = strLineFilter.Split('=')[0],
                                modelName = strLineFilter.Split('=')[1],
                            };

                            tmpBattleLGPRegister.Add(stcBLGPReg);
                        }
                    }

                    lstCharLGPRegisters.Sort((fN1, fN2) => fN1.fileName.CompareTo(fN2.fileName));
                }
                else
                {
                    iResult = 0;
                }
            }
            catch
            {

                iResult = -1;
            }

            return iResult;
        }



        //  ---------------------------------------------------------------------------------------------------------
        //  --------------------------- Battle Limits Animations relationship ---------------------------------------
        //  ---------------------------------------------------------------------------------------------------------
        public static void PrepareLimitsFilterFile()
        {
            STLimitsRegister stcLimitsRegister;

            lstBattleLimitsAnimations = new List<STLimitsRegister>();


            // AERITH
            stcLimitsRegister = new STLimitsRegister()
            {
                lstModelNames = new List<string>(),
            };

            stcLimitsRegister.lstModelNames.Add("RVAA");

            stcLimitsRegister.lstLimitsAnimations = new List<string>
            {
                "LIMEA2.A00",
                "LIMEA3.A00",
                "LIMEA4.A00",
                "LIMEA5.A00",
                "LIMEA6.A00",
                "LIMEA7.A00",
                "IYASH.A00",
                "KODO.A00"
            };

            lstBattleLimitsAnimations.Add(stcLimitsRegister);


            // BARRET
            stcLimitsRegister = new STLimitsRegister()
            {
                lstModelNames = new List<string>()
                {
                    "SBAA",
                    "SCAA",
                    "SDAA",
                    "SEAA",
                },

                lstLimitsAnimations = new List<string>()
                {
                    "LIMBR2.A00",
                    "LIMBR3.A00",
                    "LIMBR4.A00",
                    "LIMBR5.A00",
                    "LIMBR6.A00",
                    "LIMBR7.A00",
                    "HVSHOT.A00",
                }
            };

            lstBattleLimitsAnimations.Add(stcLimitsRegister);


            // CAIT SITH
            stcLimitsRegister = new STLimitsRegister()
            {
                lstModelNames = new List<string>()
                {
                    "RYAA",
                },

                lstLimitsAnimations = new List<string>()
                {
                    "DICE.A00",
                }
            };
           
            lstBattleLimitsAnimations.Add(stcLimitsRegister);


            // CID
            stcLimitsRegister = new STLimitsRegister()
            {
                lstModelNames = new List<string>()
                {
                    "RZAA"
                },

                lstLimitsAnimations = new List<string>()
                {
                    "LIMCD2.A00",
                    "LIMCD3.A00",
                    "LIMCD4.A00",
                    "LIMCD5.A00",
                    "LIMCD6.A00",
                }
            };

            lstBattleLimitsAnimations.Add(stcLimitsRegister);


            // CLOUD
            stcLimitsRegister = new STLimitsRegister()
            {
                lstModelNames = new List<string>()
                {
                    "RTAA",
                },

                lstLimitsAnimations = new List<string>()
                {
                    "LIMCL2.A00",
                    "LIMCL3.A00",
                    "LIMCL4.A00",
                    "LIMCL6.A00",
                    "LIMCL7.A00",
                    "BLAVER.A00",
                    "KYOU.A00",
                }
            };

            lstBattleLimitsAnimations.Add(stcLimitsRegister);


            // RED XIII
            stcLimitsRegister = new STLimitsRegister()
            {
                lstModelNames = new List<string>()
                {
                    "RWAA",
                },

                lstLimitsAnimations = new List<string>()
                {
                    "LIMRD3.A00",
                    "LIMRD4.A00",
                    "LIMRD5.A00",
                    "LIMRD6.A00",
                    "LIMRD7.A00",
                    "LIMSLED.A00",
                }
            };

            lstBattleLimitsAnimations.Add(stcLimitsRegister);


            // TIFA
            stcLimitsRegister = new STLimitsRegister()
            {
                lstModelNames = new List<string>()
                {
                    "RUAA",
                },

                lstLimitsAnimations = new List<string>()
                {
                    "LIMFAST.A00",
                }
            };

            lstBattleLimitsAnimations.Add(stcLimitsRegister);


            // YUFFIE
            stcLimitsRegister = new STLimitsRegister()
            {
                lstModelNames = new List<string>()
                {
                    "RXAA",
                },

                lstLimitsAnimations = new List<string>()
                {
                    "LIMYF1.A00",
                    "LIMYF2.A00",
                    "LIMYF3.A00",
                    "LIMYF4.A00",
                    "LIMYF5.A00",
                    "LIMYF6.A00",
                    "LIMYF7.A00",
                }
            };

            lstBattleLimitsAnimations.Add(stcLimitsRegister);
        }

        public static bool CanHaveLimitBreak(string strModelName)
        {
            return lstBattleLimitsAnimations.Exists(x => x.lstModelNames.Contains(strModelName));
        }


    }
}
