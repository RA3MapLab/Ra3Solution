using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using MapCoreLib.Core.Asset;
using MapCoreLib.Core.Util;
using MapCoreLib.Util;

namespace MapCoreLib.Core
{
    public class MiniMap
    {
        private static Dictionary<int, Color[]> styleColors = new Dictionary<int, Color[]>()
        {
            //草原
            {
                0, new[]
                {
                    Color.FromArgb(94, 95, 53),
                    Color.FromArgb(151, 130, 85),
                    Color.FromArgb(176, 158, 113),
                    Color.FromArgb(181, 186, 145)
                }
            },

            //雪地
            {
                1, new[]
                {
                    Color.FromArgb(145, 146, 151),
                    Color.FromArgb(162, 165, 174),
                    Color.FromArgb(183, 188, 191)
                }
            },

            //浮岛要塞
            {
                2, new[]
                {
                    Color.FromArgb(88, 98, 123),
                    Color.FromArgb(116, 131, 152)
                }
            },

            //古堡
            {
                3, new[]
                {
                    Color.FromArgb(150, 140, 85),
                    Color.FromArgb(101, 74, 38),
                    Color.FromArgb(89, 78, 63)
                }
            },

            //帝国花园
            {
                4, new[]
                {
                    Color.FromArgb(52, 65, 0),
                    Color.FromArgb(120, 98, 4),
                    Color.FromArgb(162, 150, 116)
                }
            },

            //沙漠
            {
                5, new[]
                {
                    Color.FromArgb(119, 84, 54),
                    Color.FromArgb(151, 130, 86),
                    Color.FromArgb(164, 149, 102)
                }
            },

            //温热泉
            {
                6, new[]
                {
                    Color.FromArgb(89, 113, 14),
                    Color.FromArgb(129, 154, 11),
                    Color.FromArgb(174, 168, 110)
                }
            },

            //热带雨林
            {
                7, new[]
                {
                    Color.FromArgb(35, 71, 0),
                    Color.FromArgb(129, 129, 0),
                    Color.FromArgb(154, 123, 45)
                }
            },

            //荒凉鬼村
            {
                8, new[]
                {
                    Color.FromArgb(156, 121, 77),
                    Color.FromArgb(154, 123, 45),
                    Color.FromArgb(195, 164, 84)
                }
            },

            //火山岛
            {
                9, new[]
                {
                    Color.FromArgb(94, 94, 52),
                    Color.FromArgb(51, 53, 28),
                    Color.FromArgb(140, 140, 100)
                }
            },
        };

        public static void genMiniMap(string mapName, int style, bool edge)
        {
            var ra3Map = new Ra3Map(PathUtil.defaultMapPath(mapName));
            ra3Map.parse();
            var bitmap = doGenMiniMap(ra3Map, style, edge);
            var tempMiniMapPath = PathUtil.defaulTempMiniMapPath(mapName);
            if (File.Exists(tempMiniMapPath))
            {
                File.Delete(tempMiniMapPath);
            }

            bitmap.Save(tempMiniMapPath, ImageFormat.Png);
        }

        private static Bitmap doGenMiniMap(Ra3Map ra3Map, int style, bool edge)
        {
            double waterHeight = 200.0;
            var mapWidth = ra3Map.getContext().mapWidth;
            var mapHeight = ra3Map.getContext().mapHeight;
            var border = ra3Map.getContext().border;
            var playableWidth = ra3Map.getContext().mapWidth - 2 * ra3Map.getContext().border;
            var playableHeight = ra3Map.getContext().mapHeight - 2 * ra3Map.getContext().border;
            var heightMapDataRaw = ra3Map.getAsset<HeightMapData>(Ra3MapConst.ASSET_HeightMapData).elevations;

            bool[,] impassableGenRaw = ra3Map.getAsset<BlendTileData>(Ra3MapConst.ASSET_BlendTileData).getImpassible();

            var heightMapData = new float[mapHeight - 2 * border, mapHeight - 2 * border];
            for (int y = 0; y < heightMapData.GetLength(1); y++)
            {
                for (int x = 0; x < heightMapData.GetLength(0); x++)
                {
                    heightMapData[x, y] = heightMapDataRaw[x + border, y + border];
                }
            }

            var impassableGen = new bool[mapHeight - 2 * border, mapHeight - 2 * border];
            for (int y = 0; y < impassableGen.GetLength(1); y++)
            {
                for (int x = 0; x < impassableGen.GetLength(0); x++)
                {
                    impassableGen[x, y] = impassableGenRaw[x + border, y + border];
                }
            }

            if (edge)
            {
                impassableGen = expand(impassableGen);
            }


            Bitmap bitmap = new Bitmap(playableWidth, playableHeight, PixelFormat.Format32bppArgb);
            Color water = Color.FromArgb(68, 94, 106);
            Color[] colors = styleColors[style];
            float[] heights = findHeights(heightMapData);
            if (heights.Length == 0)
            {
                throw new Exception("陆地高度有问题,海面高度必须是200,陆地高度必须高于200");
            }
            for (int i = 0; i < playableHeight; i++)
            {
                for (int j = 0; j < playableWidth; j++)
                {
                    if (heightMapData[i, j] - waterHeight < 0.00001)
                    {
                        bitmap.SetPixel(j, playableHeight - i - 1, water);
                        continue;
                    }

                    if (impassableGen[i, j])
                    {
                        bitmap.SetPixel(j, playableHeight - i - 1, Color.Black);
                        continue;
                    }

                    //一些超高最高陆地层的特殊情况
                    if (heightMapData[i, j].CompareTo(heights[heights.Length - 1]) == 1)
                    {
                        bitmap.SetPixel(j, playableHeight - i - 1, colors[Math.Min(heights.Length - 1, colors.Length - 1)]);
                        continue;
                    }

                    //平坦陆地着色
                    bool notColored = true;
                    for (int k = 0; k < heights.Length && k < colors.Length; k++)
                    {
                        if (heightMapData[i, j].Equals(heights[k]))
                        {
                            bitmap.SetPixel(j, playableHeight - i - 1, colors[k]);
                            notColored = false;
                            break;
                        }
                        // else if (heightMapData[i, j] - heights[0] > 0.0001 && k < heights.Length - 1 &&
                        //          !heightMapData[i, j].Equals(heights[k + 1]))
                        // {
                        //     Color median = IOUtility.interpolateColor(heightMapData[i, j], heights[k], heights[k + 1],
                        //         colors[k], colors[k + 1]);
                        //     bitmap.SetPixel(j, playableHeight - i - 1, median);
                        //     notColored = false;
                        //     break;
                        // }
                        else if (heightMapData[i, j].CompareTo(heights[0]) == 1 &&
                                 needInterpolate(heightMapData[i, j], heights))
                        {
                            int index = findNearHeight(heightMapData[i, j], heights);
                            if (index < colors.Length - 1)
                            {
                                Color median = interpolateColor(heightMapData[i, j], heights[index], heights[index + 1],
                                    colors[index], colors[index + 1]);
                                bitmap.SetPixel(j, playableHeight - i - 1, median);
                                notColored = false;
                                break;
                            }
                        }
                    }

                    if (notColored)
                    {
                        bitmap.SetPixel(j, playableHeight - i - 1, Color.White);
                    }
                }
            }


            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bitmap;
        }

        public static Color interpolateColor(double height, double height1, double height2, Color color1, Color color2)
        {
            double gap = (height2 - height1);
            int diffR = color2.R - color1.R;
            int diffG = color2.G - color1.G;
            int diffB = color2.B - color1.B;
            double ratio = (height - height1) / gap;
            int resR = (int)(ratio * diffR) + color1.R;
            int resG = (int)(ratio * diffG) + color1.G;
            int resB = (int)(ratio * diffB) + color1.B;
            // if (resR > 256)
            // {
            //     resR = 30;
            // }
            return Color.FromArgb(255, resR, resG, resB);
        }

        public static int findNearHeight(float target, float[] heights)
        {
            float min = Math.Abs(target - heights[0]);
            int index = 0;
            for (int i = 0; i < heights.Length; i++)
            {
                if (Math.Abs(target - heights[i]).CompareTo(min) == -1)
                {
                    min = Math.Abs(target - heights[i]);
                    index = i;
                }
            }

            if (target.CompareTo(heights[index]) == -1)
            {
                index--;
            }

            return index;
        }

        public static bool needInterpolate(float target, float[] heights)
        {
            for (int i = 0; i < heights.Length; i++)
            {
                if (target.Equals(heights[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static float[] findHeights(float[,] heightMapData)
        {
            ArrayList heights = new ArrayList();
            float[] data = new float[heightMapData.Length];
            for (int i = 0; i < heightMapData.GetLength(0); i++)
            {
                for (int j = 0; j < heightMapData.GetLength(1); j++)
                {
                    data[j + i * heightMapData.GetLength(1)] = heightMapData[i, j];
                }
            }

            // Buffer.BlockCopy(heightMapData,0,data,0,heightMapData.Length);
            double max = data.Max();
            IEnumerable<IGrouping<float, float>> groupBy = data.GroupBy(e => e)
                .Where(e => e.Key > 200.0)
                .Where(e => e.Key - (int)e.Key < 0.001)
                .Where(e => e.Count() > 100);
            foreach (IGrouping<float, float> grouping in groupBy)
            {
                heights.Add(grouping.Key);
            }

            float[] result = (float[])heights.ToArray(typeof(float));
            Array.Sort(result);
            return result;
        }


        private static bool[,] expand(bool[,] array)
        {
            int[,] array1 = new int[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j])
                    {
                        array1[i, j] = 1;
                    }
                    else
                    {
                        array1[i, j] = 0;
                    }
                }
            }

            int[] kernel = new int[] { -1, 0, 1 };
            for (int i = 1; i < array1.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < array1.GetLength(1) - 1; j++)
                {
                    if (array1[i, j] == 1)
                    {
                        for (int k = 0; k < kernel.Length; k++)
                        {
                            for (int l = 0; l < kernel.Length; l++)
                            {
                                int x1 = i + k;
                                int y1 = j + l;
                                if (x1 > 0 && x1 < array1.GetLength(0) && y1 > 0 && y1 < array1.GetLength(1))
                                {
                                    array1[x1, y1] = 2;
                                }
                            }
                        }
                    }
                }
            }

            bool[,] result = new bool[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array1[i, j] == 2 || array1[i, j] == 1)
                    {
                        result[i, j] = true;
                    }
                }
            }

            return result;
        }
    }
}