
using System;
using System.Collections.Generic;
using System.Text;

namespace Compress
{
    public class CompressionLevel
    {
        public static readonly CompressionLevel Max = new CompressionLevel(1, 1, 10, 64);

        public readonly int BlockInterval;
        public readonly int SearchLength;
        public readonly int PrequeueLength;
        public readonly int QueueLength;
        public readonly int SameValToTrack;
        public readonly int BruteForceLength;

        public CompressionLevel(int blockInterval,
                                int searchLength,
                                int prequeueLength,
                                int queueLength,
                                int sameValToTrack,
                                int bruteForceLength)
        {
            this.BlockInterval = blockInterval;
            this.SearchLength = searchLength;
            this.PrequeueLength = prequeueLength;
            this.QueueLength = queueLength;
            this.SameValToTrack = sameValToTrack;
            this.BruteForceLength = bruteForceLength;
        }

        public CompressionLevel(int blockInterval, int searchLength, int sameValToTrack, int bruteForceLength)
        {
            this.BlockInterval = blockInterval;
            this.SearchLength = searchLength;
            this.PrequeueLength = this.SearchLength / this.BlockInterval;
            this.QueueLength = 131000 / this.BlockInterval - this.PrequeueLength;
            this.SameValToTrack = sameValToTrack;
            this.BruteForceLength = bruteForceLength;
        }
    }

    public class Compression
    {
        public static bool Compress(byte[] input, out byte[] output)
        {
            return Compress(input, out output, CompressionLevel.Max);
        }

        public static bool Compress(byte[] input, out byte[] output, CompressionLevel level)
        {
            if (input.LongLength >= 0xFFFFFFFF)
            {
                throw new InvalidOperationException("input data is too large");
            }

            var endIsValid = false;
            var compressedChunks = new List<byte[]>();
            var compressedIndex = 0;
            var compressedLength = 0;
            output = null;

            if (input.Length < 16)
            {
                return false;
            }

            var blockTrackingQueue = new Queue<KeyValuePair<int, int>>();
            var blockPretrackingQueue = new Queue<KeyValuePair<int, int>>();

            // So lists aren't being freed and allocated so much
            var unusedLists = new Queue<List<int>>();
            var latestBlocks = new Dictionary<int, List<int>>();
            var lastBlockStored = 0;

            while (compressedIndex < input.Length)
            {
                while (compressedIndex > lastBlockStored + level.BlockInterval && input.Length - compressedIndex > 16)
                {
                    if (blockPretrackingQueue.Count >= level.PrequeueLength)
                    {
                        var tmppair = blockPretrackingQueue.Dequeue();
                        blockTrackingQueue.Enqueue(tmppair);

                        List<int> valueList;

                        if (latestBlocks.TryGetValue(tmppair.Key, out valueList) == false)
                        {
                            valueList = unusedLists.Count > 0 ? unusedLists.Dequeue() : new List<int>();
                            latestBlocks[tmppair.Key] = valueList;
                        }

                        if (valueList.Count >= level.SameValToTrack)
                        {
                            var earliestIndex = 0;
                            var earliestValue = valueList[0];

                            for (int loop = 1; loop < valueList.Count; loop++)
                            {
                                if (valueList[loop] < earliestValue)
                                {
                                    earliestIndex = loop;
                                    earliestValue = valueList[loop];
                                }
                            }

                            valueList[earliestIndex] = tmppair.Value;
                        }
                        else
                        {
                            valueList.Add(tmppair.Value);
                        }

                        if (blockTrackingQueue.Count > level.QueueLength)
                        {
                            var tmppair2 = blockTrackingQueue.Dequeue();
                            valueList = latestBlocks[tmppair2.Key];

                            for (int loop = 0; loop < valueList.Count; loop++)
                            {
                                if (valueList[loop] == tmppair2.Value)
                                {
                                    valueList.RemoveAt(loop);
                                    break;
                                }
                            }

                            if (valueList.Count == 0)
                            {
                                latestBlocks.Remove(tmppair2.Key);
                                unusedLists.Enqueue(valueList);
                            }
                        }
                    }

                    var newBlock = new KeyValuePair<int, int>(BitConverter.ToInt32(input, lastBlockStored),
                                                              lastBlockStored);
                    lastBlockStored += level.BlockInterval;
                    blockPretrackingQueue.Enqueue(newBlock);
                }

                if (input.Length - compressedIndex < 4)
                {
                    // Just copy the rest
                    var chunk = new byte[input.Length - compressedIndex + 1];
                    chunk[0] = (byte)(0xFC | (input.Length - compressedIndex));
                    Array.Copy(input, compressedIndex, chunk, 1, input.Length - compressedIndex);

                    compressedChunks.Add(chunk);
                    compressedIndex += chunk.Length - 1;
                    compressedLength += chunk.Length;

                    // int toRead = 0;
                    // int toCopy2 = 0;
                    // int copyOffset = 0;

                    endIsValid = true;
                    continue;
                }

                // Search ahead the next 3 bytes for the "best" sequence to copy
                var sequenceStart = 0;
                var sequenceLength = 0;
                var sequenceIndex = 0;
                var isSequence = false;

                if (FindSequence(input,
                                 compressedIndex,
                                 ref sequenceStart,
                                 ref sequenceLength,
                                 ref sequenceIndex,
                                 latestBlocks,
                                 level))
                {
                    isSequence = true;
                }
                else
                {
                    // Find the next sequence
                    for (int loop = compressedIndex + 4;
                         isSequence == false && loop + 3 < input.Length;
                         loop += 4)
                    {
                        if (FindSequence(input,
                                         loop,
                                         ref sequenceStart,
                                         ref sequenceLength,
                                         ref sequenceIndex,
                                         latestBlocks,
                                         level))
                        {
                            sequenceIndex += loop - compressedIndex;
                            isSequence = true;
                        }
                    }

                    if (sequenceIndex == int.MaxValue)
                    {
                        sequenceIndex = input.Length - compressedIndex;
                    }

                    // Copy all the data skipped over
                    while (sequenceIndex >= 4)
                    {
                        int toCopy = (sequenceIndex & ~3);
                        if (toCopy > 112)
                        {
                            toCopy = 112;
                        }

                        var chunk = new byte[toCopy + 1];
                        chunk[0] = (byte)(0xE0 | ((toCopy >> 2) - 1));
                        Array.Copy(input, compressedIndex, chunk, 1, toCopy);
                        compressedChunks.Add(chunk);
                        compressedIndex += toCopy;
                        compressedLength += chunk.Length;
                        sequenceIndex -= toCopy;

                        // int toRead = 0;
                        // int toCopy2 = 0;
                        // int copyOffset = 0;
                    }
                }

                if (isSequence)
                {
                    /*
                     * 00-7F  0oocccpp oooooooo
                     *   Read 0-3
                     *   Copy 3-10
                     *   Offset 0-1023
                     *   
                     * 80-BF  10cccccc ppoooooo oooooooo
                     *   Read 0-3
                     *   Copy 4-67
                     *   Offset 0-16383
                     *   
                     * C0-DF  110cccpp oooooooo oooooooo cccccccc
                     *   Read 0-3
                     *   Copy 5-1028
                     *   Offset 0-131071
                     *   
                     * E0-FC  111ppppp
                     *   Read 4-128 (Multiples of 4)
                     *   
                     * FD-FF  111111pp
                     *   Read 0-3
                     */
                    if (FindRunLength(input, sequenceStart, compressedIndex + sequenceIndex) < sequenceLength)
                    {
                        break;
                    }

                    while (sequenceLength > 0)
                    {
                        int thisLength = sequenceLength;
                        if (thisLength > 1028)
                        {
                            thisLength = 1028;
                        }

                        sequenceLength -= thisLength;
                        int offset = compressedIndex - sequenceStart + sequenceIndex - 1;

                        byte[] chunk;
                        if (thisLength > 67 || offset > 16383)
                        {
                            chunk = new byte[sequenceIndex + 4];
                            chunk[0] =
                                (byte)
                                (0xC0 | sequenceIndex | (((thisLength - 5) >> 6) & 0x0C) | ((offset >> 12) & 0x10));
                            chunk[1] = (byte)((offset >> 8) & 0xFF);
                            chunk[2] = (byte)(offset & 0xFF);
                            chunk[3] = (byte)((thisLength - 5) & 0xFF);
                        }
                        else if (thisLength > 10 || offset > 1023)
                        {
                            chunk = new byte[sequenceIndex + 3];
                            chunk[0] = (byte)(0x80 | ((thisLength - 4) & 0x3F));
                            chunk[1] = (byte)(((sequenceIndex << 6) & 0xC0) | ((offset >> 8) & 0x3F));
                            chunk[2] = (byte)(offset & 0xFF);
                        }
                        else
                        {
                            chunk = new byte[sequenceIndex + 2];
                            chunk[0] =
                                (byte)
                                ((sequenceIndex & 0x3) | (((thisLength - 3) << 2) & 0x1C) | ((offset >> 3) & 0x60));
                            chunk[1] = (byte)(offset & 0xFF);
                        }

                        if (sequenceIndex > 0)
                        {
                            Array.Copy(input, compressedIndex, chunk, chunk.Length - sequenceIndex, sequenceIndex);
                        }

                        compressedChunks.Add(chunk);
                        compressedIndex += thisLength + sequenceIndex;
                        compressedLength += chunk.Length;

                        // int toRead = 0;
                        // int toCopy = 0;
                        // int copyOffset = 0;

                        sequenceStart += thisLength;
                        sequenceIndex = 0;
                    }
                }
            }

            if (compressedLength + 6 < input.Length)
            {
                int chunkPosition;

                output = new byte[compressedLength + 14 + (endIsValid ? 0 : 1)];
                // "EAR\0"
                Array.Copy(Encoding.ASCII.GetBytes("EAR\0"), 0, output, 0, 4);
                // 未压缩长度（小端）
                Array.Copy(getIntByteArray(input.Length), 0, output, 4, 4);
                // 标志位
                //统一用0x80大文件标志
                output[8] = 0x80;
                output[9] = 0xFB;
                // 大端
                output[10] = (byte)(input.Length >> 24);
                output[11] = (byte)(input.Length >> 16);
                output[12] = (byte)(input.Length >> 8);
                output[13] = (byte)(input.Length);
                chunkPosition = 14;

                foreach (byte[] t in compressedChunks)
                {
                    Array.Copy(t, 0, output, chunkPosition, t.Length);
                    chunkPosition += t.Length;
                }

                if (!endIsValid)
                {
                    output[output.Length - 1] = 0xFC;
                }

                return true;
            }

            return false;
        }

        private static bool FindSequence(byte[] data,
                                         int offset,
                                         ref int bestStart,
                                         ref int bestLength,
                                         ref int bestIndex,
                                         Dictionary<int, List<int>> blockTracking,
                                         CompressionLevel level)
        {
            int start;
            int end = -level.BruteForceLength;

            if (offset < level.BruteForceLength)
            {
                end = -offset;
            }

            if (offset > 4)
            {
                start = -3;
            }
            else
            {
                start = offset - 3;
            }

            bool foundRun = false;
            if (bestLength < 3)
            {
                bestLength = 3;
                bestIndex = int.MaxValue;
            }

            var search = new byte[data.Length - offset > 4 ? 4 : data.Length - offset];

            for (int loop = 0; loop < search.Length; loop++)
            {
                search[loop] = data[offset + loop];
            }

            while (start >= end && bestLength < 1028)
            {
                byte currentByte = data[start + offset];

                for (int loop = 0; loop < search.Length; loop++)
                {
                    if (currentByte != search[loop] || start >= loop || start - loop < -131072)
                    {
                        continue;
                    }

                    int len = FindRunLength(data, offset + start, offset + loop);

                    if ((len > bestLength || len == bestLength && loop < bestIndex) &&
                        (len >= 5 ||
                         len >= 4 && start - loop > -16384 ||
                         len >= 3 && start - loop > -1024))
                    {
                        foundRun = true;
                        bestStart = offset + start;
                        bestLength = len;
                        bestIndex = loop;
                    }
                }

                start--;
            }

            if (blockTracking.Count > 0 && data.Length - offset > 16 && bestLength < 1028)
            {
                for (int loop = 0; loop < 4; loop++)
                {
                    var thisPosition = offset + 3 - loop;
                    var adjust = loop > 3 ? loop - 3 : 0;
                    var value = BitConverter.ToInt32(data, thisPosition);
                    List<int> positions;

                    if (blockTracking.TryGetValue(value, out positions))
                    {
                        foreach (var trypos in positions)
                        {
                            int localadjust = adjust;

                            if (trypos + 131072 < offset + 8)
                            {
                                continue;
                            }

                            int length = FindRunLength(data, trypos + localadjust, thisPosition + localadjust);

                            if (length >= 5 && length > bestLength)
                            {
                                foundRun = true;
                                bestStart = trypos + localadjust;
                                bestLength = length;
                                if (loop < 3)
                                {
                                    bestIndex = 3 - loop;
                                }
                                else
                                {
                                    bestIndex = 0;
                                }
                            }

                            if (bestLength > 1028)
                            {
                                break;
                            }
                        }
                    }

                    if (bestLength > 1028)
                    {
                        break;
                    }
                }
            }

            return foundRun;
        }

        private static int FindRunLength(byte[] data, int source, int destination)
        {
            int endSource = source + 1;
            int endDestination = destination + 1;

            while (endDestination < data.Length && data[endSource] == data[endDestination] &&
                   endDestination - destination < 1028)
            {
                endSource++;
                endDestination++;
            }

            return endDestination - destination;
        }

        private static byte[] getIntByteArray(int intValue)
        {
            byte[] intBytes = BitConverter.GetBytes(intValue);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            return intBytes;
        }
    }
}
