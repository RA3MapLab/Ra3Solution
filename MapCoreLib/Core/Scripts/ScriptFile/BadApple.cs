using System.Collections.Generic;
using MapCoreLib.Core.Asset;
using MapCoreLib.Core.Util;

namespace MapCoreLib.Core.Scripts.ScriptFile
{
    public class BadApple: ScriptInterface
    {
        public static string counter = "counter";

        //5秒后开始
        public int startFrame = 5 * 15;
        
        public void Apply(MapDataContext context)
        {
            SetCounter(context);
            
        }

        public void SetCounter(MapDataContext context)
        {
            //初始化计数器
            context.addScript( "", new Script()
            {
                Name = "initCounter",
                scriptOrConditions = ScriptHelper.ConditionTrue(context),
                ScriptActionOnTrue = new List<ScriptAction>()
                {
                    ScriptAction.of(context, "SET_COUNTER", counter, 0)
                }
            });
            
            //每帧加1
            context.addScript( "", new Script()
            {
                Name = "addCounter",
                LoopActions = true,
                scriptOrConditions = ScriptHelper.ConditionTrue(context),
                ScriptActionOnTrue = new List<ScriptAction>()
                {
                    ScriptAction.of(context, "INCREMENT_COUNTER", counter, 1)
                }
            });
        }
        
        
        
    }
}