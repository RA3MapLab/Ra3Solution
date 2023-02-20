using System.Collections.Generic;
using MapCoreLib.Core.Asset;

namespace MapCoreLib.Core
{
    public abstract class MapListener
    {

        public abstract List<string> interestedIn();

        public void visitStringPool(MapDataContext context, Dictionary<string, int> stringPool)
        {
            
        }
        
        #region 脚本相关
        public virtual void visitScriptList(MapDataContext context, ScriptList scriptList, int index)
        {
            
        }
        
        public virtual void visitScriptGroup(MapDataContext context, ScriptGroup scriptGroup)
        {
            
        }
        
        public virtual void visitScript(MapDataContext context, Script script)
        {
            
        }
        
        public virtual void visitScriptCondition(MapDataContext context, ScriptCondition scriptCondition)
        {
            
        }
        
        public virtual void visitScriptAction(MapDataContext context, ScriptAction scriptAction)
        {
            
        }
        
        public virtual void visitScriptActionFalse(MapDataContext context, ScriptAction scriptActionFalse)
        {
            
        }
        #endregion

        #region player相关

        public virtual void visitPlayer(MapDataContext context, Player player)
        {
            
        }
        
        public virtual void visitPlayerProperty(MapDataContext context, AssetProperty assetProperty, Player player)
        {
            
        }

        #endregion

        #region Team相关

        public virtual void visitTeam(Team team)
        {
            
        }
        
        public virtual void visitTeamProperty(MapDataContext context, AssetProperty assetProperty, Team team)
        {
            
        }

        #endregion

        #region MapObject相关

        public virtual void visitMO_Start(MapDataContext context, ObjectsList objectsList)
        {
            
        }

        public virtual void visitMO_Plain(MapDataContext context, MapObject mapObject)
        {
            
        }
        
        public virtual void visitMO_Waypoint(MapDataContext context, MapObject mapObject)
        {
            
        }
        
        public virtual void visitMO_PlayerStart(MapDataContext context, MapObject mapObject)
        {
            
        }

        #endregion
        
    }
}