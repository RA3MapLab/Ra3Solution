using System;
using Microsoft.Win32;

namespace Ra3ModBuilder.Core
{
    public class ModDataConfig
    {
        public string mModName;
        public string mVersion;

        public string mSDKDirectory;
        public string mModPath;
        public string mModAdditionalFilesPath;
        public string mModAssetsPath;
        public string mModDataPath;

        public string mBuiltModsPath;
        public string mBuiltModPath;
        public string mBuiltModDataPath;
        public string mModInstallPath;
        public string mModBabProj;
        public string mModXml;
        public string mModBig;
        public string mModSkudef;

        public string mPersonalDirectory = Environment.ExpandEnvironmentVariables(Registry
            .GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\User Shell Folders",
                "Personal", "").ToString());

        public string mUserDataLeafName;
        
        // exe程序
        public string mBinaryAssetBuilder;
        public string mAssetResolver;
        public string mHashFix;
        public string mLoDStreamBuilder;
        public string mMakeBig;
        public string mAssetMerger;

        public void configSDK(ModMakerConfigBean configBeanBean)
        {
            mSDKDirectory = configBeanBean.sdkPath;
            initUserDataLeafName();
            
            mBuiltModsPath = mSDKDirectory + "\\builtmods";
            mBinaryAssetBuilder = mSDKDirectory + "\\tools\\binaryassetbuilder.exe";
            mAssetResolver = mSDKDirectory + "\\tools\\modassetresolver.exe";
            mHashFix = mSDKDirectory + "\\tools\\hashfix.exe";
            mLoDStreamBuilder = mSDKDirectory + "\\tools\\lodstreambuilder.exe";
            mMakeBig = mSDKDirectory + "\\tools\\makebig.exe";
            mAssetMerger = mSDKDirectory + "\\tools\\assetmerge.exe";
        }

        public void configMod(string modName, string version)
        {
            mModPath = mSDKDirectory + "\\mods\\" + mModName;
            mModAdditionalFilesPath = mModPath + "\\additional";
            mModAssetsPath = mModPath + "\\assets";
            mModDataPath = mModPath + "\\data";
            mBuiltModPath = mBuiltModsPath + "\\mods\\" + mModName;
            mBuiltModDataPath = mBuiltModPath + "\\data";
            mModInstallPath = mPersonalDirectory + "\\" + mUserDataLeafName + "\\Mods\\" + mModName;
            mModBabProj = mModPath + "\\mod.babproj";
            mModXml = mModDataPath + "\\mod.xml";
            mModBig = mModName + "_" + mVersion + ".big";
            mModSkudef = mModName + "_" + mVersion + ".skudef";
        }
        
        private void initUserDataLeafName()
        {
            var Reg = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Electronic Arts\\Electronic Arts\\Red Alert 3",
                "UserDataLeafName", null);
            if (Reg == null)
            {
                Reg = Registry.GetValue(
                    "HKEY_LOCAL_MACHINE\\Software\\Wow6432Node\\Electronic Arts\\Electronic Arts\\Red Alert 3",
                    "UserDataLeafName", null);
                if (Reg == null)
                {
                    mUserDataLeafName = "Red Alert 3";
                }
                else
                {
                    mUserDataLeafName = Reg.ToString();
                }
            }
            else
            {
                mUserDataLeafName = Reg.ToString();
            }
        }
    }
}