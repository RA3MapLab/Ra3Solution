using System.Collections.Generic;
using System.IO;

namespace MapCoreLib.Core.Asset
{
    public class PostEffectsChunk: MajorAsset
    {
        public List<PostEffect> effects = new List<PostEffect>();

        protected override void saveData(BinaryWriter bw, MapDataContext context)
        {
            bw.Write(effects.Count);
            foreach (var postEffect in effects)
            {
                postEffect.Save(bw);
            }
        }

        public override string getAssetName()
        {
            return Ra3MapConst.ASSET_PostEffectsChunk;
        }

        public override short getVersion()
        {
            return 2;
        }

        public static PostEffectsChunk newInstance(MapDataContext context)
        {
            var postEffectsChunk = new PostEffectsChunk();
            postEffectsChunk.name = Ra3MapConst.ASSET_PostEffectsChunk;
            postEffectsChunk.id = context.MapStruct.RegisterString(postEffectsChunk.name);
            postEffectsChunk.version = postEffectsChunk.getVersion();
            postEffectsChunk.effects.Add(PostEffect.Distortion());
            return postEffectsChunk;
        }
    }
}