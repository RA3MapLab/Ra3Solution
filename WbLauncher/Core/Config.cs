using System;
using System.IO;

namespace WbLauncher.Core
{
    public static class Config
    {
        public static string NEW_RA3_WB = "WU_RA3_WB";
        public static string RA3_ROOT = "RA3_ROOT";
        
        public static void CreateWbCfgIfNeeded(string ra3Root)
        {
            var ra3RootData = Path.Combine(ra3Root, "Data");
            
            var wbCfg = Path.Combine(Directory.GetCurrentDirectory(), "bin", "RA3_wb_1.12.cfg");
            if (!File.Exists(wbCfg))
            {
                File.WriteAllText(wbCfg,
                    $@"

set-search-path big:;.
add-big {ra3RootData}\MapsCampaign.big
add-big {ra3RootData}\MapsMultiplayer.big
add-big {ra3RootData}\MapsTutorial.big
add-big {ra3RootData}\English.big
add-big {ra3RootData}\Korean.big
add-big {ra3RootData}\Czech.big
add-big {ra3RootData}\Thai.big
add-big {ra3RootData}\ChineseT.big
add-big {ra3RootData}\Polish.big
add-big {ra3RootData}\Hungarian.big
add-big {ra3RootData}\Italian.big
add-big {ra3RootData}\Spanish.big
add-big {ra3RootData}\Russian.big
add-big {ra3RootData}\German.big
add-big {ra3RootData}\French.big
add-big {ra3RootData}\EnglishAudio.big
add-big {ra3RootData}\FrenchAudio.big
add-big {ra3RootData}\GermanAudio.big
add-big {ra3RootData}\RussianAudio.big
add-big {ra3RootData}\SpanishAudio.big
add-big {ra3RootData}\ItalianAudio.big
add-big {ra3RootData}\EnglishMovieAudio.big
add-big {ra3RootData}\FrenchMovies.big
add-big {ra3RootData}\GermanMovies.big
add-big {ra3RootData}\RussianMovies.big
add-big {ra3RootData}\SpanishMovies.big
add-big {ra3RootData}\ItalianMovies.big
add-big {ra3RootData}\Apt.big
add-big {ra3RootData}\Terrain.big
add-big {ra3RootData}\WBStatic_12.big
add-big {ra3RootData}\WBGlobal_12.big
add-big {ra3RootData}\Libraries.big
add-big {ra3RootData}\Shaders.big
add-big {ra3RootData}\WBStringhashes_12.big
add-big {ra3RootData}\Misc.big
add-big {ra3RootData}\WBData_12.big
");
            }
        }
    }
}