using System;
using System.Collections.Generic;
using System.IO;
using CodeHollow.FeedReader;

namespace Downloader.Core;

public class RssData
{
    public string DirectLink { get; }
    public string BTLink { get; }

    public RssData(string directLink, string bTLink)
    {
        DirectLink = directLink;
        BTLink = bTLink;
    }

    public static RssData? Parse(Feed feed, string id, string targetPath)
    {
        var exceptions = new List<Exception>();
        foreach (var item in feed.Items)
        {
            const string srvRoot = "/srv/";
            const string torrentExtension = ".torrent";
            var textLink = Filter(item.Link);
            if (!textLink.EndsWith(torrentExtension, StringComparison.OrdinalIgnoreCase))
            {
                exceptions.Add(new InvalidDataException($"Unexpected link without {nameof(torrentExtension)}: {textLink}"));
                continue;
            }
            // 去除后缀名
            // textLink = textLink[..^torrentExtension.Length];
            textLink = textLink.Substring(0, textLink.Length - torrentExtension.Length);
            if (!Uri.TryCreate(textLink, UriKind.Absolute, out var link))
            {
                exceptions.Add(new InvalidDataException($"Failed to parse uri: {item.Link}"));
                continue;
            }
            // 检测是否以 srv 起始
            if (!link.AbsolutePath.StartsWith(srvRoot, StringComparison.OrdinalIgnoreCase))
            {
                exceptions.Add(new InvalidDataException($"Unexpected link without {nameof(srvRoot)}: {link.AbsolutePath}"));
                continue;
            }
            // 比较链接
            var path = $"{srvRoot}{id}/{targetPath}";
            if (!path.Equals(link.AbsolutePath, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            // 找到了！
            return new(textLink, Filter(item.Description));
        }

        // 假如找不到任何链接，检查是不是因为出错了
        if (exceptions.Count > 0)
        {
            throw new AggregateException($"Failed to parse feed.", exceptions);
        }
        // 假如不是因为出错，那么返回 null
        return null;
    }

    private static string Filter(string raw) => raw.Trim(' ', '\t', '"');
}
