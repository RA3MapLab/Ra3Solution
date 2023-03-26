using System.Collections.Generic;
using MapCoreLib.Core.Asset;

namespace MapCoreLib.Core.Scripts.ScriptFile
{
    public class OnlyInfantry: ScriptInterface
    {
        public void Apply(MapDataContext context)
        {

            var All_Player = new List<string>()
            {
                "Player_1",
                "Player_2",
                "Player_3",
                "Player_4",
                "Player_5",
                "Player_6",
                "PlyrCivilian",
                "PlyrCreeps"
            };

            var disallowBuildings = new List<string>()
            {
                "SovietAirfield",
                "SovietWarFactory",
                "SovietNavalYard",
                "JapanWarFactoryEgg",
                "JapanNavalYardEgg",
                "AlliedWarFactory",
                "AlliedNavalYard",
                "AlliedAirfield",
                "CelestialNavalYard_Fake",
                "CelestialWarFactory_Fake",
                "CelestialAirfield_Fake",

                "SovietBaseDefenseAir",
                "SovietBaseDefenseGround",
                "SovietBaseDefenseAdvanced",
                "JapanBaseDefenseEgg",
                "JapanBaseDefenseAdvancedEgg",
                "AlliedBaseDefense",
                "AlliedBaseDefenseAdvanced",
                "CelestialBaseDefense_Fake",
                "CelestialBaseDefenseAdvanced_Fake",
                "CelestialBaseDefenseAir_Fake",
                "CelestialLaserTower_Fake"
            };

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
                }
            };
            
            All_Player.ForEach(player =>
            {
                disallowBuildings.ForEach(building =>
                {
                    script.ScriptActionOnTrue.Add(
                        ScriptAction.of(context, "ALLOW_DISALLOW_ONE_BUILDING", new List<object>(){player, building, false}));
                });
            });
            
            Script.addTo(context,
                script,
                "");
        }
    }
}