using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class Script : MajorAsset
    {
        [XmlAttribute] [DefaultValue("")] public string Name;
        [XmlAttribute] [DefaultValue("")] public string Conmment;
        [XmlAttribute] [DefaultValue("")] public string ConditionComment;
        [XmlAttribute] [DefaultValue("")] public string ActionComment;

        [XmlAttribute] [DefaultValue(false)] public bool IsSubroutine;

        [XmlAttribute] [DefaultValue(true)] public bool isActive;

        [XmlAttribute] [DefaultValue(true)] public bool DeactivateUponSuccess;
        [XmlAttribute] [DefaultValue(true)] public bool ActiveInEasy;
        [XmlAttribute] [DefaultValue(true)] public bool ActiveInMedium;
        [XmlAttribute] [DefaultValue(true)] public bool ActiveInHard;

        [XmlAttribute] [DefaultValue(0)] public int EvaluationInterval;
        [XmlAttribute] [DefaultValue(false)] public bool ActionsFireSequentially;
        [XmlAttribute] [DefaultValue(false)] public bool LoopActions;
        [XmlAttribute] [DefaultValue(0)] public int LoopCount;
        [XmlAttribute] [DefaultValue((byte)1)] public byte SequentialTargetType;
        [XmlAttribute] [DefaultValue("")] public string SequentialTargetName;
        [XmlIgnore] public string Unknown = "";

        [XmlArray("If")] 
        public List<OrCondition> scriptOrConditions = new List<OrCondition>();
        [XmlArray("Then")] public List<ScriptAction> ScriptActionOnTrue = new List<ScriptAction>();
        [XmlArray("Else")] public List<ScriptActionFalse> ScriptActionOnFalse = new List<ScriptActionFalse>();

        public override MajorAsset fromStream(BinaryReader binaryReader, MapDataContext context)
        {
            base.fromStream(binaryReader, context);

            Name = binaryReader.readDefaultString();
            Conmment = binaryReader.readDefaultString();
            ConditionComment = binaryReader.readDefaultString();
            ActionComment = binaryReader.readDefaultString();

            isActive = binaryReader.ReadBoolean();
            DeactivateUponSuccess = binaryReader.ReadBoolean();
            ActiveInEasy = binaryReader.ReadBoolean();
            ActiveInMedium = binaryReader.ReadBoolean();
            ActiveInHard = binaryReader.ReadBoolean();
            IsSubroutine = binaryReader.ReadBoolean();
            EvaluationInterval = binaryReader.ReadInt32();
            ActionsFireSequentially = binaryReader.ReadBoolean();
            LoopActions = binaryReader.ReadBoolean();

            LoopCount = binaryReader.ReadInt32();
            SequentialTargetType = binaryReader.ReadByte();
            SequentialTargetName = binaryReader.readDefaultString();
            Unknown = binaryReader.readDefaultString();

            while (binaryReader.BaseStream.Position - dataStartPos < dataSize)
            {
                var id = binaryReader.ReadInt32();
                var assetName = context.MapStruct.findStringByIndex(id);
                binaryReader.BaseStream.Position -= 4;
                switch (assetName)
                {
                    case Ra3MapConst.ASSET_OrCondition:
                        scriptOrConditions.Add((OrCondition) new OrCondition().fromStream(binaryReader, context));
                        break;
                    case Ra3MapConst.ASSET_ScriptAction:
                        ScriptActionOnTrue.Add((ScriptAction) new ScriptAction().fromStream(binaryReader, context));
                        break;
                    case Ra3MapConst.ASSET_ScriptActionFalse:
                        ScriptActionOnFalse.Add(
                            (ScriptActionFalse) new ScriptActionFalse().fromStream(binaryReader, context));
                        break;
                    default:
                        LogUtil.log($"unknown assetName: {assetName}");
                        break;
                }
            }

            return this;
        }

        protected override void saveData(BinaryWriter binaryWriter, MapDataContext context)
        {
            binaryWriter.writeDefaultString(Name);
            binaryWriter.writeDefaultString(Conmment);
            binaryWriter.writeDefaultString(ConditionComment);
            binaryWriter.writeDefaultString(ActionComment);

            binaryWriter.Write(isActive);
            binaryWriter.Write(DeactivateUponSuccess);
            binaryWriter.Write(ActiveInEasy);
            binaryWriter.Write(ActiveInMedium);
            binaryWriter.Write(ActiveInHard);
            binaryWriter.Write(IsSubroutine);

            binaryWriter.Write(EvaluationInterval);
            binaryWriter.Write(ActionsFireSequentially);
            binaryWriter.Write(LoopActions);
            binaryWriter.Write(LoopCount);
            binaryWriter.Write(SequentialTargetType);
            binaryWriter.writeDefaultString(SequentialTargetName);
            binaryWriter.writeDefaultString(Unknown);

            foreach (var action in ScriptActionOnTrue)
            {
                action.Save(binaryWriter, context);
            }

            foreach (var condition in scriptOrConditions)
            {
                condition.Save(binaryWriter, context);
            }

            foreach (var action in ScriptActionOnFalse)
            {
                action.Save(binaryWriter, context);
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_Script;
        }

        public override short getVersion()
        {
            return 4;
        }

        public override void registerSelf(MapDataContext context)
        {
            base.registerSelf(context);
            //TODO 注册还需要补齐scriptContent的
            scriptOrConditions.ForEach(item => item.registerSelf(context));
            ScriptActionOnTrue.ForEach(item => item.registerSelf(context));
            ScriptActionOnFalse.ForEach(item => item.registerSelf(context));
        }
        
        public static void addTo(MapDataContext context, Script script, string path = "")
        {
            PlayerScriptsList playerScriptsList = context.getAsset<PlayerScriptsList>(Ra3MapConst.ASSET_PlayerScriptsList);
            SidesList sidesList = context.getAsset<SidesList>(Ra3MapConst.ASSET_SidesList);
            if (path == "")
            {
                playerScriptsList.scriptLists[0].scripts.Add(script);
                script.registerSelf(context);
                return;
            }
            var dirs = path.Trim().Split('/');
            int index = sidesList.findPlayerIndex(dirs[0]);
            if (index < 0)
            {
                throw new Exception($"addTo | 脚本路径的第一个文件夹名称有错误 --- {path}");
            }

            if (dirs.Length == 1)
            {
                var scripts = playerScriptsList.scriptLists[index].scripts;
                scripts.Add(script);
            }
            else
            {
                var scriptGroups = playerScriptsList.scriptLists[index].scriptGroups;

                ScriptGroup group = null;
                for (var i = 1; i < dirs.Length; i++)
                {
                    var dirName = dirs[i];
                    if (dirName == "")
                    {
                        throw new Exception($"addTo | 文件夹名字不能为空，检查你是不是多写了斜杠 --- {path}");
                    }

                    group = scriptGroups.Find(scriptGroup => scriptGroup.Name == dirName);
                    if (group == null)
                    {
                        group = ScriptGroup.of(context, dirName);
                        scriptGroups.Add(group);
                    }

                    scriptGroups = group.scriptGroups;
                }
                
                group?.scripts.Add(script);
            }
            
            script.registerSelf(context);
        }
    }
}