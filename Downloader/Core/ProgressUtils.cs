using System;

namespace Downloader.Core;

internal static class ProgressUtils
{
    private class NoOpProgress<T> : IProgress<T>
    {
        public void Report(T value)
        { }
    }

    public static IProgress<T> NoOp<T>() => new NoOpProgress<T>();

    public static IProgress<T> OnlyOnChanged<T>(Action<T> original, T initial)
    {
        var previous = initial;
        return new Progress<T>(t =>
        {
            if (!Equals(t, previous))
            {
                previous = t;
                original(t);
            }
        });
    }

    public static IProgress<T> OnlyOnChanged<T>(IProgress<T> original, T initial)
    {
        return OnlyOnChanged(original.Report, initial);
    }

    public static IProgress<string?> WithPrefix(Action<string> original, string prefix)
    {
        return OnlyOnChanged(new Progress<string?>(s =>
        {
            var current = prefix;
            if (!string.IsNullOrWhiteSpace(s))
            {
                current += $" {s}";
            }
            original(current);
        }), null);
    }

    public static IProgress<string?> WithPrefix(IProgress<string> original, string prefix)
    {
        return WithPrefix(original.Report, prefix);
    }
}