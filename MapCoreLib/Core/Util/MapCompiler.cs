using System.IO;

namespace MapCoreLib.Core.Util
{
    public class MapCompiler
    {
        public void compile(MapCompileConfig config)
        {
            //如果modsdkPath下的mods里的maps文件夹不存在，递归创建
            var sdkMapCopyDir = Path.Combine(config.modsdkPath, "mods", "maps",
                Path.GetFileNameWithoutExtension(config.mapPath));
            if (!Directory.Exists(sdkMapCopyDir))
            {
                Directory.CreateDirectory(sdkMapCopyDir);
            }
            //config.mapPath所在文件夹里的文件，除了manifest后缀的，都复制到sdkMapCopyDir里
            var mapDir = Path.GetDirectoryName(config.mapPath);
            foreach (var file in Directory.GetFiles(mapDir))
            {
                if (Path.GetExtension(file) != ".manifest")
                {
                    File.Copy(file, Path.Combine(sdkMapCopyDir, Path.GetFileName(file)), true);
                }
            }
            
            
            //清除上次留下的编译文件（bin,manifest, relo, imp, version）
            //用binaryassetbuilder编译map.xml(TODO 容忍错误，处理map.xml里的错误，增加贴图是都编译的选项)
            //用assetmerge合并asset
            //用hashfix修复hash
            //把产物复制回地图文件夹（注意有些文件不需要复制）
            
        }
    }
    
    public class MapCompileConfig
    {
        public string mapPath;
        public string modsdkPath;
    }
}