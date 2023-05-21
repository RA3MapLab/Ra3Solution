using System.Collections.Generic;
using MapCoreLib.Core.Asset;

namespace MapCoreLib.Core.Scripts.ScriptFile
{
    public class BadApple: ScriptInterface
    {
        public void Apply(MapDataContext context)
        {
            
        }

        public void clearMap(MapDataContext context)
        {
            var script = new Script()
            {
                Name = "banInfantry",
                scriptOrConditions = new List<OrCondition>()
                {
                    new OrCondition()
                    {
                        conditions = new List<ScriptCondition>()
                        {
                            ScriptCondition.of(context, "CONDITION_TRUE")
                        }
                    }
                },
                ScriptActionOnTrue = new List<ScriptAction>()
                {
                    ScriptAction.of(context, "", new List<object>())
                }
            };
        }
    }
}