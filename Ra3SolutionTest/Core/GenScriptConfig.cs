using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace NewMapParser.Core
{
    public class GenScriptConfig
    {
        public static void Run()
        {
            var listConENOrig = File.ReadAllLines("脚本条件英文图.md").ToList();
            var listConEN = listConENOrig.Where(line => line.Contains('[')).ToList();
            listConEN.Sort((line1, line2) =>
            {
                return Int32.Parse(line1.Substring(line1.IndexOf('[') + 1,
                           line1.IndexOf(']') - line1.IndexOf('[') - 1)) -
                       Int32.Parse(line2.Substring(line2.IndexOf('[') + 1,
                           line2.IndexOf(']') - line2.IndexOf('[') - 1));
            });
            
            var listConChs = File.ReadAllLines("脚本条件中文图.md").Where(line => line.Contains('[')).ToList();
            listConChs.Sort((line1, line2) =>
            {
                return Int32.Parse(line1.Substring(line1.IndexOf('[') + 1,
                           line1.IndexOf(']') - line1.IndexOf('[') - 1)) -
                       Int32.Parse(line2.Substring(line2.IndexOf('[') + 1,
                           line2.IndexOf(']') - line2.IndexOf('[') - 1));
            });

            var listActENOrig = File.ReadAllLines("脚本动作英文图.md").ToList();
            var listActEN = listActENOrig.Where(line => line.Contains('[')).ToList();
            listActEN.Sort((line1, line2) =>
            {
                return Int32.Parse(line1.Substring(line1.IndexOf('[') + 1,
                           line1.IndexOf(']') - line1.IndexOf('[') - 1)) -
                       Int32.Parse(line2.Substring(line2.IndexOf('[') + 1,
                           line2.IndexOf(']') - line2.IndexOf('[') - 1));
            });
            
            var listActChs = File.ReadAllLines("脚本动作中文图.md").Where(line => line.Contains('[')).ToList();
            listActChs.Sort((line1, line2) =>
            {
                return Int32.Parse(line1.Substring(line1.IndexOf('[') + 1,
                           line1.IndexOf(']') - line1.IndexOf('[') - 1)) -
                       Int32.Parse(line2.Substring(line2.IndexOf('[') + 1,
                           line2.IndexOf(']') - line2.IndexOf('[') - 1));
            });
            
            for (var i = 0; i < listConENOrig.Count; i++)
            {
                var str = listConENOrig[i];
                if (str.Contains('['))
                {
                    var number = Int32.Parse(str.Substring(str.IndexOf('[') + 1,
                        str.IndexOf(']') - str.IndexOf('[') - 1));
                    var trans = listConChs[number].Substring(listConChs[number].IndexOf(' ') + 1,
                        listConChs[number].IndexOf('[') - listConChs[number].IndexOf(' ') - 1);
                    listConENOrig[i] += trans;
                }
            }
            
            listConENOrig.ForEach(Console.WriteLine);
            Console.WriteLine("--------------------------------");
            
            for (var i = 0; i < listActENOrig.Count; i++)
            {
                var str = listActENOrig[i];
                if (str.Contains('['))
                {
                    var number = Int32.Parse(str.Substring(str.IndexOf('[') + 1,
                        str.IndexOf(']') - str.IndexOf('[') - 1));
                    var trans = listActChs[number].Substring(listActChs[number].IndexOf(' ') + 1,
                        listActChs[number].IndexOf('[') - listActChs[number].IndexOf(' ') - 1);
                    listActENOrig[i] += trans;
                }
            }
            
            listActENOrig.ForEach(Console.WriteLine);
        }

        public static void GenScriptConfigJson()
        {
            var conScriptDir = getConScriptDir("脚本条件翻译混合图.md");
            var actScriptDir = getConScriptDir("脚本动作翻译混合图.md");
            File.WriteAllText("脚本条件图.json", JsonConvert.SerializeObject(conScriptDir.scriptDirs));
            File.WriteAllText("脚本动作图.json", JsonConvert.SerializeObject(actScriptDir.scriptDirs));
        }

        private static ScriptDir getConScriptDir(string file)
        {
            var scriptCon = File.ReadAllText(file);
            var headings = ParseHeadings(scriptCon);
            var tree = GenerateTree(headings);
            var scriptDir = new ScriptDir();
            traverseTree(tree, scriptDir);
            return scriptDir;
        }

        private static void traverseTree(Node tree, ScriptDir scriptDir)
        {
            for (var i = 0; i < tree.Children.Count; i++)
            {
                var child = tree.Children[i];
                var title = child.Heading.Title;
                
                if (child.Children != null && child.Children.Count > 0)
                {
                    scriptDir.scriptDirs.Add(new ScriptDir()
                    {
                        content = new Content()
                        {
                            origin = title.Substring(0, title.IndexOf('[')),
                            chinese = title.Substring(title.IndexOf(']') + 1, title.Length - title.IndexOf(']') - 1)
                        }
                    });
                    traverseTree(child, scriptDir.scriptDirs[scriptDir.scriptDirs.Count - 1]);
                }
                else
                {
                    scriptDir.scriptItems.Add(new Content()
                    {
                        origin = title.Substring(0, title.IndexOf('[')),
                        chinese = title.Substring(title.IndexOf(']') + 1, title.Length - title.IndexOf(']') - 1)
                    });
                }
            }
        }

        static List<Heading> ParseHeadings(string markdown)
        {
            var headings = new List<Heading>();

            // Split markdown into lines
            var lines = markdown.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                if (line.StartsWith("#"))
                {
                    int level = 1;
                    while (line.Length >= level + 1 && line[level] == '#')
                    {
                        level++;
                    }

                    // Trim any leading/trailing white space characters
                    string title = line.Substring(level).Trim();

                    // Add new heading to the list
                    headings.Add(new Heading(level, title));
                }
            }

            return headings;
        }
        
        static Node GenerateTree(List<Heading> headings)
        {
            var root = new Node(null, null);
            var currentNodes = new List<Node> { root };

            foreach (var heading in headings)
            {
                var newNode = new Node(heading, null);

                // Find the parent node by going back in the current node list
                Node parentNode = null;
                for (int i = currentNodes.Count - 1; i >= 0; i--)
                {
                    var currentNode = currentNodes[i];
                    if (currentNode.Heading != null && currentNode.Heading.Level < heading.Level)
                    {
                        parentNode = currentNode;
                        break;
                    }
                }

                if (parentNode == null)
                {
                    parentNode = root;
                }

                parentNode.Children.Add(newNode);

                currentNodes.Add(newNode);
            }

            return root;
        }

        public static void genScriptTransMap()
        {
            List<ScriptDir> scriptCon = JsonConvert.DeserializeObject<List<ScriptDir>>(File.ReadAllText("脚本条件图.json"));
            List<ScriptDir> scriptAct = JsonConvert.DeserializeObject<List<ScriptDir>>(File.ReadAllText("脚本动作图.json"));
            var scriptTransMap = new Dictionary<string, string>();
            scriptCon.ForEach(item =>
            {
                traverseScriptTree(item, item.content.origin, $"{item.content.origin}_{item.content.chinese}" , scriptTransMap);
            });
            scriptAct.ForEach(item =>
            {
                traverseScriptTree(item, item.content.origin, $"{item.content.origin}_{item.content.chinese}", scriptTransMap);
            });
            File.WriteAllText("scriptTrans.json", JsonConvert.SerializeObject(scriptTransMap));
        }

        private static void traverseScriptTree(ScriptDir root, string origin, string trans, Dictionary<string, string> scriptTransMap)
        {
            if (root.scriptDirs.Count == 0)
            {
                root.scriptItems.ForEach(item =>
                {
                    scriptTransMap.Add($"{origin}/{item.origin}", $"{trans}/{item.origin}_{item.chinese}");
                });
            }
            else
            {
                root.scriptDirs.ForEach(item =>
                {
                    traverseScriptTree(item, $"{origin}/{item.content.origin}", $"{trans}/{item.content.origin}_{item.content.chinese}", scriptTransMap);
                });
                root.scriptItems.ForEach(item =>
                {
                    scriptTransMap.Add($"{origin}/{item.origin}", $"{trans}/{item.origin}_{item.chinese}");
                });
            }
        }

        class ScriptDir
        {
            public Content content = new Content();
            public List<ScriptDir> scriptDirs = new List<ScriptDir>();
            public List<Content> scriptItems = new List<Content>();
        }

        class Content
        {
            public string origin = "";
            public string chinese = "";
        }
        
        class Heading
        {
            public int Level { get; set; }
            public string Title { get; set; }

            public Heading(int level, string title)
            {
                Level = level;
                Title = title;
            }
        }

        class Node
        {
            public Heading Heading { get; set; }
            public List<Node> Children { get; set; }

            public Node(Heading heading, List<Node> children)
            {
                Heading = heading;
                Children = children ?? new List<Node>();
            }
        }
    }
}