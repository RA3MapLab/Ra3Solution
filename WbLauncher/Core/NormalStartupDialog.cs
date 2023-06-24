using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace WbLauncher.Core
{
    public partial class NormalStartupDialog : Form
    {
        private Dictionary<string, string> language = new Dictionary<string, string>()
        {
            {"中文", "cn"},
            {"English", "en"},
        };

        public List<ModInfo> Items;
        private readonly Action<string, string> onSelect;
        private LauncherConfig config;
        private Assembly jsonAssembly = Assembly.LoadFrom(Path.Combine(Directory.GetCurrentDirectory(), "bin", "Newtonsoft.Json.dll"));
        private string preferenceFile;
        private readonly bool waitEvent;

        public NormalStartupDialog(Action<string, string> onSelect, bool waitEvent = false)
        {
            this.onSelect = onSelect;
            InitializeComponent();
            this.waitEvent = waitEvent;
            if (waitEvent)
            {
                modList.Visible = false;
                selectModTv.Visible = false;
            }
            // resources.ApplyResources(c, c.Name, new CultureInfo(lang));
        }

        private void NormalStartupDialog_Load(object sender, EventArgs e)
        {
            preferenceFile = Path.Combine(Directory.GetCurrentDirectory(), "data", "config", "preference.json");
            parseConfig();
            Items = getMods();
            Items.ForEach(item => modList.Items.Add(item.label));
            modList.SelectionMode = SelectionMode.One;
            modList.SelectedIndex = 0;
            
            foreach (var languageKey in language.Keys)
            {
                languageSelect.Items.Add(languageKey);
            }
            
            languageSelect.SelectedIndex = config.language == "cn" ? 0 : 1;
        }

        private void parseConfig()
        {
            var preferenceFile = Path.Combine("data", "config", "preference.json");
            if (File.Exists(preferenceFile))
            {

                Type jsonConvertType = jsonAssembly.GetType("Newtonsoft.Json.JsonConvert");
                MethodInfo deserializeObjectMethod = jsonConvertType.GetMethod("DeserializeObject", new Type[] { typeof(string), typeof(Type) });
                
                string jsonString = File.ReadAllText(preferenceFile);
                try
                { 
                    config = (LauncherConfig)deserializeObjectMethod.Invoke(null, new object[] { jsonString, typeof(LauncherConfig) });
                }
                catch (Exception e)
                {
                    config = new LauncherConfig()
                    {
                        language = Config.isChinese() ? "cn" : "en"
                    };
                    writeConfig();
                }
                
            }
            else
            {
                config = new LauncherConfig()
                {
                    language = Config.isChinese() ? "cn" : "en"
                };
                writeConfig();
            }
        }
        private void writeConfig()
        {
            Type jsonConvertType = jsonAssembly.GetType("Newtonsoft.Json.JsonConvert");
            MethodInfo serializeObjectMethod = jsonConvertType.GetMethod("SerializeObject", new Type[] { typeof(Type) });
            File.WriteAllText(preferenceFile, (string)serializeObjectMethod.Invoke(null, new object[] { config }));
        }

        private void onConfirm(object sender, EventArgs e)
        {
            onSelect(Items[modList.SelectedIndex].skudefPath, language[languageSelect.SelectedText]);
            DialogResult = DialogResult.OK;

            Close();
        }


        private List<ModInfo> getMods()
        {
            var mods =  Directory.EnumerateFiles(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        "Red Alert 3",
                        "Mods"
                    ), "*.skudef", SearchOption.AllDirectories)
                .Select(path =>
                {
                    return new ModInfo()
                    {
                        skudefPath = path,
                        label = Path.GetFileNameWithoutExtension(path)
                    };
                }).ToList();
            mods.Insert(0, new ModInfo()
            {
                skudefPath = "",
                label = "原版(不加载任何mod)"
            });
            return mods;
        }
        
        public class ModInfo
        {
            public string skudefPath;
            public string label;
        }

        private void Launch_Click(object sender, EventArgs e)
        {
            config = new LauncherConfig()
            {
                language = language[(string)languageSelect.SelectedItem]
            };
            writeConfig();
            onSelect(waitEvent ? "" : Items[modList.SelectedIndex].skudefPath, language[(string)languageSelect.SelectedItem]);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}