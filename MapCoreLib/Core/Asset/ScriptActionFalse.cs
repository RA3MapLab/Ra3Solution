namespace MapCoreLib.Core.Asset
{
    public class ScriptActionFalse: ScriptAction
    {
        public override void registerSelf(MapDataContext context)
        {
            registerSelf2(context, 3, Ra3MapConst.ASSET_ScriptActionFalse);
        }
    }
}