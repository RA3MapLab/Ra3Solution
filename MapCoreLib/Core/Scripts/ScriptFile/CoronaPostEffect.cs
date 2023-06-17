using System;
using System.Collections.Generic;
using MapCoreLib.Core.Asset;
using MapCoreLib.Core.Util;

namespace MapCoreLib.Core.Scripts.ScriptFile
{
    public class CoronaPostEffect: ScriptInterface
    {
        #region 配置

        //地图里的物体名字
        private string objName = "obj";

        //变化间隔，单位是帧比如每3帧收到1点伤害
        private int interval = 3;
        
        //受伤害总次数，每次受1点伤害(正伤害或者负伤害)，注意不要让物体血量减到低于0，这里的100代表总共受100次伤害
        private int pass = 100;

        //是否是正伤害，true是正伤害（物体扣血），false是负伤害（物体加血）
        private bool positive = true;

        #endregion
        


        #region 代码使用的变量

        private string counter;

        #endregion
            
        public void Apply(MapDataContext context)
        {
            string prefix = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + "_";
            counter = prefix + "counter";
            var playerScriptsList = context.getAsset<PlayerScriptsList>(Ra3MapConst.ASSET_PlayerScriptsList);
            var scriptGroup = ScriptGroup.of(context, "?" + prefix);
            playerScriptsList.scriptLists[0].scriptGroups.Add(scriptGroup);
            scriptGroup.scripts.Add(  new Script()
            {
                Name = prefix + "init",
                scriptOrConditions = ScriptHelper.ConditionTrue(context),
                ScriptActionOnTrue = new List<ScriptAction>()
                {
                    ScriptAction.of(context, "SET_COUNTER", counter, 0),
                    ScriptAction.of(context, "ENABLE_SCRIPT", prefix + "loop")
                }
            });
            
            scriptGroup.scripts.Add( new Script()
            {
                Name = prefix + "loop",
                LoopActions = true,
                isActive = false,
                scriptOrConditions = ScriptHelper.ConditionTrue(context),
                ScriptActionOnTrue = new List<ScriptAction>()
                {
                    ScriptAction.of(context, "INCREMENT_COUNTER",  1, counter)
                }
            });
            
            for (int i = 0; i < pass; i++)
            {
                scriptGroup.scripts.Add(  new Script()
                {
                    Name = prefix + "loop",
                    LoopActions = true,
                    
                    scriptOrConditions = new List<OrCondition>()
                    {
                        new OrCondition()
                        {
                            conditions = new List<ScriptCondition>()
                            {
                                ScriptCondition.of(context, "COUNTER", new List<object>() {counter, 2, i * interval})
                            }
                        }
                    },
                    ScriptActionOnTrue = new List<ScriptAction>()
                    {
                        ScriptAction.of(context, "NAMED_DAMAGE",  objName, 1),
                        
                    }
                });
            }
            scriptGroup.scripts[scriptGroup.scripts.Count - 1].ScriptActionOnTrue.Add(ScriptAction.of(context, "DISABLE_SCRIPT",  prefix + "loop"));
            
            foreach (var script1 in scriptGroup.scripts)
            {
                script1.registerSelf(context);
            }
            
        }
    }
}