using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            label1.Text = Config.isChinese() ? "选择语言" : "Language:";
            selectModTv.Text = Config.isChinese() ? "选择Mod:" : "Select Mod:";
            this.waitEvent = waitEvent;
            if (waitEvent)
            {
                modList.Visible = false;
                selectModTv.Visible = false;
            }
            //when double click item of modList
            modList.DoubleClick += (sender, args) =>
            {
                Launch();
            };
            //when press enter key
            modList.KeyDown += (sender, args) =>
            {
                if (args.KeyCode == Keys.Enter)
                {
                    Launch();
                }
            };
            // resources.ApplyResources(c, c.Name, new CultureInfo(lang));
            qqgroup.Click += (sender, args) =>
            {
                System.Diagnostics.Process.Start(
                    "http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=Uhe3By7ZOj2-LWr6cJEsq6TpNQiS_U06&authKey=2UptCtgDoo8b3EumQMAwK9Ihi7gbZnQ8NMRVZHkVIA5ySLs3%2Fe14oC0wNKdtaYoC&noverify=0&group_code=613550502");
            };
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

        // private void onConfirm(object sender, EventArgs e)
        // {
        //     onSelect(Items[modList.SelectedIndex].skudefPath, language[languageSelect.SelectedText]);
        //     DialogResult = DialogResult.OK;
        //
        //     Close();
        // }


        private List<ModInfo> getMods()
        {
            var ModsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Red Alert 3",
                "Mods"
            );
            if (!Directory.Exists(ModsDir))
            {
                //如果mods文件夹不存在，创建mods文件夹
                Directory.CreateDirectory(ModsDir);
            }
            var mods =  Directory.EnumerateFiles(
                    ModsDir, "*.skudef", SearchOption.AllDirectories)
                .Select(path =>
                {
                    //支持WB版本配置文件
                    //get skudefWB extension of same name file
                    var skudefWB = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + ".skudefWB");
                    if (File.Exists(skudefWB))
                    {
                        return new ModInfo()
                        {
                            skudefPath = skudefWB,
                            label = Path.GetFileNameWithoutExtension(path)
                        };
                    }
                    return new ModInfo()
                    {
                        skudefPath = path,
                        label = Path.GetFileNameWithoutExtension(path)
                    };
                }).ToList();
            mods.Insert(0, new ModInfo()
            {
                skudefPath = "",
                label = Config.isChinese() ? "原版(不加载任何mod)" : "original (Do not load any mods)"
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
            Launch();
        }

        private void Launch()
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

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //open url
            // System.Diagnostics.Process.Start("explorer.exe",
            //     "http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=cArWBpG1xV-z1k4JV2wXahd9NZ_1y2U5&authKey=YXo97c0YJV4q7hBm1rDdaWvKPJ23nwyAa9slLz%2FBRTo%2FbUiCVcXgMps%2BEyTtMVtD&noverify=0&group_code=613550502");
            
            System.Diagnostics.Process.Start(
                "http://www.baidu.com");
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }
    }
}