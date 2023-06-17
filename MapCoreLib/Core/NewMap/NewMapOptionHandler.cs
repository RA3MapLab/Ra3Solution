namespace MapCoreLib.Core.NewMap
{
    public interface NewMapOptionHandler
    {
        void handle(MapDataContext context, NewMapConfig config);

        string optionName();
    }
}