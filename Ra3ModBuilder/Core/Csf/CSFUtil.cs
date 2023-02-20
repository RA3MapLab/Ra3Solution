using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ra3ModBuilder.Core
{
    public class CSFUtil
    {
        public static readonly char[] headerFlag = " FSC".ToCharArray();
        public static readonly int version = 3;
        public static readonly int language = 0;
        public static readonly char[] labelFlag = " LBL".ToCharArray();
        public static readonly char[] contentFlag = " RTS".ToCharArray();
        
        public Dictionary<string, string> entries = new Dictionary<string, string>();

        public CSFUtil()
        {
        }

        public bool parse(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }
            using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
            {
                var fileFlag = reader.ReadChars(4);
                if (!fileFlag.SequenceEqual(" FSC".ToCharArray()))
                {
                    return false;
                }

                reader.ReadUInt32(); // 版本号
                var labelNameCount = reader.ReadUInt32();
                var labelContentCount = reader.ReadUInt32();
                reader.ReadUInt32();  // 语言
                reader.ReadUInt32();

                for (int i = 0; i < labelNameCount; i++)
                {
                    var csfEntry = new CSFEntry().parse(reader);
                    entries.Add(csfEntry.name, csfEntry.content);
                }
            }

            return true;
        }

        public void mergeCsv2Csf(string csvPath, string srcCsfPath, string destCsfPath)
        {
            if (".csv".Equals(Path.GetExtension(csvPath)))
            {
                parse(srcCsfPath);
                var allLines = File.ReadAllLines(csvPath);
                foreach (var line in allLines)
                {
                    var strings = line.Trim().Split(',').Select(s => s.Trim()).ToArray();
                    if (strings.Length == 2)
                    {
                        mergeEntry(strings[0], strings[1]);
                    }
                }

                generateCsfFromEntry(destCsfPath);
            }
        }

        private void generateCsfFromEntry(string csfPath)
        {
            using (BinaryWriter writer1 = new BinaryWriter(File.OpenWrite(csfPath)))
            {
                writeCSfHeader(writer1);
                foreach (var pair in entries)
                {
                    writeEntry(writer1, pair.Key, pair.Value);
                }
            }
        }
        
        private static void writeEntry(BinaryWriter writer1, string key, string value)
        {
            writer1.Write(labelFlag);
            writer1.Write(1);
            writer1.writeUInt32PrefixedAsciiString(key);
            writer1.Write(contentFlag);
            writer1.writeUInt32PrefixedNegatedUnicodeString(value);
        }

        private void writeCSfHeader(BinaryWriter writer1)
        {
            writer1.Write(headerFlag);
            writer1.Write(version);
            writer1.Write(entries.Count);
            writer1.Write(entries.Count);
            writer1.Write(language);
            writer1.Write(0);
        }

        private void mergeEntry(string label, string content)
        {
            if (entries.ContainsKey(label))
            {
                entries[label] = content;
            }
            else
            {
                entries.Add(label, content);
            }
        }

        // public void csf2CSV(string path)
        // {
        //     if (parse(path))
        //     {
        //         var directoryName = Path.GetDirectoryName(Path.GetFullPath(path));
        //         var csvPath = Path.Combine(directoryName, Path.GetFileNameWithoutExtension(path) + ".csv");
        //         var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
        //         
        //         if (File.Exists(csvPath))
        //         {
        //             var backupFile = Path.Combine(directoryName, fileNameWithoutExtension + ".csv.backup");
        //             if (File.Exists(csvPath))
        //             {
        //                 File.Delete(backupFile);
        //                 File.Copy(csvPath, backupFile);
        //             }
        //             File.Delete(csvPath);
        //         }
        //
        //         
        //         using (var sw = new StreamWriter(File.OpenWrite(csvPath)))
        //         {
        //             
        //             entries.ForEach(delegate(CSFEntry entry)
        //             {
        //                 sw.WriteLine($"{entry.name},\"{entry.content}\"");
        //             });
        //         }
        //     }
        // }
        
        // public void csv2csf(string path)
        // {
        //     if (".csv".Equals(Path.GetExtension(path)))
        //     {
        //         var directoryName = Path.GetDirectoryName(Path.GetFullPath(path));
        //         var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
        //         var csfPath1 = Path.Combine(directoryName, fileNameWithoutExtension + "_1.csf");
        //         var csfPath2 = Path.Combine(directoryName, fileNameWithoutExtension + "_2.csf");
        //         
        //         if (File.Exists(csfPath1))
        //         {
        //             var backupFile = Path.Combine(directoryName, fileNameWithoutExtension + ".csf.backup1");
        //             if (File.Exists(csfPath1))
        //             {
        //                 File.Delete(backupFile);
        //                 File.Copy(csfPath1, backupFile);
        //             }
        //             File.Delete(csfPath1);
        //         }
        //         
        //         if (File.Exists(csfPath2))
        //         {
        //             var backupFile = Path.Combine(directoryName, fileNameWithoutExtension + ".csf.backup2");
        //             if (File.Exists(csfPath2))
        //             {
        //                 File.Delete(backupFile);
        //                 File.Copy(csfPath2, backupFile);
        //             }
        //             File.Delete(csfPath2);
        //         }
        //         
        //         using (BinaryWriter writer1 = new BinaryWriter(File.OpenWrite(csfPath1)))
        //         {
        //             using (BinaryWriter writer2 = new BinaryWriter(File.OpenWrite(csfPath2)))
        //             {
        //                 var allLines = File.ReadAllLines(path);
        //                 
        //                 writeCSfHeader(writer1, allLines);
        //                 writeCSfHeader(writer2, allLines);
        //
        //                 int count = 0;
        //                 foreach (var line in allLines)
        //                 {
        //                     var strings = line.Trim().Split(',').Select(s => s.Trim()).ToArray();
        //                     if (strings.Length == 2)
        //                     {
        //                         count++;
        //                         Array.Resize(ref strings, 3);
        //                         strings[2] = "";
        //                         writeEntry(writer1, strings, writer2);
        //                     } else if (strings.Length == 3)
        //                     {
        //                         count++;
        //                         writeEntry(writer1, strings, writer2);
        //                     }
        //                 }
        //
        //                 rewriteCount(writer1, count);
        //                 rewriteCount(writer2, count);
        //             }
        //         }
        //     }
        // }
        //
        // private static void writeCSfHeader(BinaryWriter writer1, string[] allLines)
        // {
        //     writer1.Write(headerFlag);
        //     writer1.Write(version);
        //     writer1.Write(allLines.Length);
        //     writer1.Write(allLines.Length);
        //     writer1.Write(language);
        //     writer1.Write(0);
        // }
        //
        // private static void rewriteCount(BinaryWriter writer1, int count)
        // {
        //     writer1.Seek(8, SeekOrigin.Begin);
        //     writer1.Write(count);
        //     writer1.Write(count);
        // }
        //
        // private static void writeEntry(BinaryWriter writer1, string[] strings, BinaryWriter writer2)
        // {
        //     writer1.Write(labelFlag);
        //     writer1.Write(1);
        //     writer1.writeUInt32PrefixedAsciiString(strings[0]);
        //     writer1.Write(contentFlag);
        //     var realContent = strings[1];
        //     if (strings[1].Length > 2 && strings[1].StartsWith("\"") && strings[1].EndsWith("\""))
        //     {
        //         realContent = strings[1].Substring(1, strings[1].Length - 2);
        //     }
        //
        //     writer1.writeUInt32PrefixedNegatedUnicodeString(realContent);
        //
        //
        //     writer2.Write(labelFlag);
        //     writer2.Write(1);
        //     writer2.writeUInt32PrefixedAsciiString(strings[0]);
        //     writer2.Write(contentFlag);
        //     realContent = strings[2];
        //     if (strings[2].Length > 2 && strings[2].StartsWith("\"") && strings[2].EndsWith("\""))
        //     {
        //         realContent = strings[2].Substring(1, strings[2].Length - 2);
        //     }
        //
        //     writer2.writeUInt32PrefixedNegatedUnicodeString(realContent);
        // }
        //
        // public static bool isCSF(string path)
        // {
        //     bool isRealCSF = false;
        //     using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        //     {
        //         var fileFlag = reader.ReadChars(4);
        //         isRealCSF = fileFlag.SequenceEqual(" FSC".ToCharArray());
        //     }
        //
        //     return ".csf".Equals(Path.GetExtension(path)) && isRealCSF;
        // }
        //
        // public static bool isCSV(string path)
        // {
        //     return ".csv".Equals(Path.GetExtension(path));
        // }
    }
}