using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Win32;
using Newtonsoft.Json;
using Ra3ModBuilder.Core.Model;
using Ra3ModBuilder.Core.Util;

namespace Ra3ModBuilder.Core
{
    public class ModMaker
    {
        private static string[] streamReferences = new string[3] {"audio.xml", "global.xml", "static.xml"};

        private ModDataContext context = new ModDataContext();

        public ModMaker(string modName, string version)
        {
            // mModName = modName;
            // mVersion = version;
            // initPaths();
        }

        public void configMaker(string configPath)
        {
            var configStr = File.ReadAllText("config.json");
            ModMakerConfigBean configBean = JsonConvert.DeserializeObject<ModMakerConfigBean>(configStr);
            context.config.configSDK(configBean);
        }

        public void buildMod(string modName, string version)
        {
            context.config.configMod(modName, version);
            doBuildMod();
        }

        #region buidlMod
        
        private void GenerateBABProjFile()
                {
                    new BABProj(context.config.mModXml, Path.Combine(context.config.mBuiltModsPath, "sagexml"))
                        .serialize(context.config.mModPath);
                }

        private void doBuildMod()
        {
            GenerateBABProjFile();

            List<Action> buildModSteps = new List<Action>
            {
                new Action(() => { ClearBuiltMod(); }),
                new Action(() => { ClearCache(); }),
                new Action(() => { BuildGlobalData(); }),
                new Action(() => { BuildStaticData(); }),
                new Action(() => { MergeAssets(); }),
                new Action(() => { CopyingAdditionalFiles(); }),
                new Action(() => { CreatingBigFile(); }),
                new Action(() => { CreatingSkudefFile(); })
            };
            
            buildModSteps.ForEach(delegate(Action action) { action.Invoke(); });
        }

        private void BuildGlobalData()
        {
            executeCmdSync(String.Format("/C (@echo off) & (cd /D \"{0}\")"
                                         + " & (for /R \"{1}\\additionalmaps\" %I in (\"mapmetadata_*\") do (del \"%I\" /F /Q))"
                                         + " & (for /R \"{3}\\additionalmaps\" %I in (\"mapmetadata_*.xml\") do ("
                                         + "(\"{2}\" \"%I\" /od:\"{4}\" /iod:\"{4}\" /ls:true /osh:false /pc:true /res:true /slowclean:true /ss:true /audio:\".\\mods\\{5}\\audio;.\\audio\" /art:\".\\mods\\{5}\\art;.\\art\" /data:\".;.\\mods;.\\mods\\{5}\\data;.\\sagexml\")"
                                         + "))",
                context.config.mSDKDirectory, 
                context.config.mBuiltModDataPath, 
                context.config.mBinaryAssetBuilder, 
                context.config.mModDataPath, 
                context.config.mBuiltModsPath, 
                context.config.mModName));
        }

        private void BuildStaticData()
        {
            executeCmdSync(String.Format("/C (@echo off) & (cd /D \"{0}\")"
                                         + " & (if exist \"{1}\\mod.bin\" (del \"{1}\\mod.bin\" /F /Q))"
                                         + " & (if exist \"{1}\\mod.imp\" (del \"{1}\\mod.imp\" /F /Q))"
                                         + " & (if exist \"{1}\\mod.manifest\" (del \"{1}\\mod.manifest\" /F /Q))"
                                         + " & (if exist \"{1}\\mod.relo\" (del \"{1}\\mod.relo\" /F /Q))"
                                         + " & (if exist \"{1}\\mod.version\" (del \"{1}\\mod.version\" /F /Q))"
                                         + " & (for /R \"{1}\" %I in (\"mod_*\") do (del \"%I\" /F /Q))"
                                         + " & (\"{2}\" \"{3}\" /od:\"{4}\" /iod:\"{4}\" /ls:true /osh:false /pc:true /res:true /slowclean:true /ss:true /audio:\".\\mods\\{5}\\audio;.\\audio\" /art:\".\\mods\\{5}\\art;.\\art\" /data:\".;.\\mods;.\\mods\\{5}\\data;.\\sagexml\")",
                context.config.mSDKDirectory, 
                context.config.mBuiltModDataPath, 
                context.config.mBinaryAssetBuilder, 
                context.config.mModBabProj, 
                context.config.mBuiltModsPath, 
                context.config.mModName));
        }

        private void MergeAssets()
        {
            var command = String.Format("/V:ON /C  & (cd /D \"{0}\")"
                                        + " & (for /R \"{1}\" %I in (\"\") do ("
                                        + "(set assets=%~dpI)"
                                        + " & (if exist \"!assets!*.asset\" (\"{2}\" \"{3}\\mod\" \"!assets:~0,-1!\"))"
                                        + "))",
                context.config.mSDKDirectory, 
                context.config.mModAssetsPath, 
                context.config.mAssetMerger, 
                context.config.mBuiltModDataPath);
            Console.WriteLine($"MergeAssets {command}");
            executeCmdSync(command);
        }

        private void CopyingAdditionalFiles()
        {
            executeCmdSync(String.Format("/C (@echo off) & (cd /D \"{0}\")",
                context.config.mSDKDirectory));
            if (Directory.Exists(context.config.mSDKDirectory + "\\additional"))
                FileUtil.CopyFolders(context.config.mSDKDirectory + "\\additional", context.config.mBuiltModPath);
            if (Directory.Exists(context.config.mModAdditionalFilesPath))
            {
                FileUtil.CopyFolders(context.config.mModAdditionalFilesPath, context.config.mBuiltModPath);
            }
        }

        private void CreatingBigFile()
        {
            executeCmdSync(String.Format("/C (@echo off) & (cd /D \"{0}\")"
                                         + " & (\"{1}\" -f \"{2}\" -x:*.asset -o:\"{3}\\{4}\")"
                                         + " & (if exist \"{3}\\{5}\" (del \"{3}\\{5}\" /F /Q))"
                                         + " & (cd /D \"{3}\")"
                                         + " & (echo mod-game {6}>\"{5}\")"
                                         + " & (echo add-big {4}>>\"{5}\")",
                context.config.mSDKDirectory, 
                context.config.mMakeBig, 
                context.config.mBuiltModPath, 
                context.config.mModInstallPath, 
                context.config.mModBig, 
                context.config.mSDKDirectory, 
                1.12));
        }

        private void CreatingSkudefFile()
        {
            executeCmdSync(String.Format("/C (@echo off) & (cd /D \"{0}\")"
                                         + " & (\"{1}\" -f \"{2}\" -x:*.asset -o:\"{3}\\{4}\")"
                                         + " & (if exist \"{3}\\{5}\" (del \"{3}\\{5}\" /F /Q))"
                                         + " & (cd /D \"{3}\")"
                                         + " & (echo mod-game {6}>\"{5}\")"
                                         + " & (echo add-big {4}>>\"{5}\")",
                context.config.mSDKDirectory, 
                context.config.mSDKDirectory, 
                context.config.mBuiltModPath, 
                context.config.mModInstallPath, 
                context.config.mModBig, 
                context.config.mModSkudef, 
                1.12));
        }
        
        private void ClearBuiltMod()
        {
            executeCmdSync(String.Format("/C (@echo off) & (cd /D \"{0}\")"
                                         + " & (for /R \"{1}\" %I in (\"*.*\") do ("
                                         + "(if not \"%~xI\" == \".asset\" (del \"%I\" /F /Q))"
                                         + "))",
                context.config.mSDKDirectory, 
                context.config.mBuiltModPath));
        }
        
        private void ClearCache()
        {
            executeCmdSync(String.Format("/C (@echo off) & (cd /D \"{0}\")"
                                         + " & (if exist \"{1}\\builtmods\" (rd \"{1}\\builtmods\" /S /Q))"
                                         + " & (if exist \"{1}\\cache\" (rd \"{1}\\cache\" /S /Q))"
                                         + " & (for /R \"{2}\" %I in (\"*.asset\") do (del \"%I\" /F /Q))"
                                         + " & (if exist \"{1}\\binaryassetbuilder.sessioncache.xml.deflate\" (del \"{1}\\binaryassetbuilder.sessioncache.xml.deflate\" /F /Q))"
                                         + " & (if exist \"{1}\\binaryassetbuilder.sessioncache.xml.deflate.old\" (del \"{1}\\binaryassetbuilder.sessioncache.xml.deflate.old\" /F /Q))"
                                         + " & (if exist \"{1}\\stringhashes.xml\" (del \"{1}\\stringhashes.xml\" /F /Q))",
                context.config.mSDKDirectory, 
                context.config.mBuiltModsPath, 
                context.config.mBuiltModPath));
        }

        private void executeCmdSync(String command)
        {
            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = command;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.EnableRaisingEvents = true;
            process.Start();
            
            process.StandardInput.WriteLine("&exit");
            process.StandardInput.AutoFlush=true;
            string strOuput = process.StandardOutput.ReadToEnd();

            process.WaitForExit();
            process.Close();
            Console.WriteLine(strOuput);
            // process.Exited += buildMod();
        }

        #endregion

    }
}