using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using MapCoreLib.Core.Interface;
using MapCoreLib.Core.Util;
using MapCoreLib.Util;
using RMGlib.Core.Utility;

namespace MapCoreLib.Core.Asset
{
    public class ScriptAction: ScriptContent, ICustomXml
    {
        public void toXml(XmlDocument document, XmlElement root, MapDataContext context, object asset)
        {
            var newElement = document.CreateRa3MapElement(contentName);
            var scriptModel = ScriptSpec.actionsSpec.getOrNull(contentName);
            if (scriptModel == null)
            {
                // root.AppendChild(newElement);
                return;
            }

            base.toXml(document, newElement, context, scriptModel);
            root.AppendChild(newElement);
        }

        public void toObj(XmlDocument document, XmlElement childNode, MapDataContext context, object asset)
        {
            var action = new ScriptAction();
            var Name = childNode.LocalName;
            var scriptModel = ScriptSpec.actionsSpec.getOrNull(Name);
            if (scriptModel == null)
            {
                LogUtil.log($"OrCondition | unknown name ${Name}");
                throw new Exception($"非法的脚本动作标签 {Name}");
            }

            base.toObj(document, childNode, context, scriptModel);
        }

        public override void registerSelf(MapDataContext context)
        {
            registerSelf2(context, 3, Ra3MapConst.ASSET_ScriptAction);
        }
        
        public static ScriptAction of(MapDataContext mapDataContext, string commandWord, List<object> args = null)
        {
            if (!ScriptSpec.checkScriptAction(commandWord, args))
            {
                throw new Exception($"ScriptCondition of | 脚本动作参数有误 --- {commandWord}");
            }

            var scriptContent = ScriptSpec.generateScriptContent(mapDataContext, ScriptSpec.actionsSpec, commandWord, args);
            ScriptAction scriptAction = new ScriptAction()
            {
                contentType = scriptContent.contentType,
                assetPropertyType = scriptContent.assetPropertyType,
                contentName = scriptContent.contentName,
                nameIndex = scriptContent.nameIndex,
                enable = scriptContent.enable,
                arguments = scriptContent.arguments,
            };
            return scriptAction;
        }
        
        public static ScriptAction of(MapDataContext mapDataContext, string commandWord, params object[] argsArr)
        {
            var args = argsArr.ToList();
            if (!ScriptSpec.checkScriptAction(commandWord, args))
            {
                throw new Exception($"ScriptCondition of | 脚本动作参数有误 --- {commandWord}");
            }

            var scriptContent = ScriptSpec.generateScriptContent(mapDataContext, ScriptSpec.actionsSpec, commandWord, args);
            ScriptAction scriptAction = new ScriptAction()
            {
                contentType = scriptContent.contentType,
                assetPropertyType = scriptContent.assetPropertyType,
                contentName = scriptContent.contentName,
                nameIndex = scriptContent.nameIndex,
                enable = scriptContent.enable,
                arguments = scriptContent.arguments,
            };
            return scriptAction;
        }
    }
}