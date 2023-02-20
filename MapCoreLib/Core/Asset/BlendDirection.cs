using System;

namespace MapCoreLib.Core.Asset
{
    [Flags]
    public enum BlendDirection : byte
    {
        BottomRight = 0x28,
        Bottom = 0x2,
        BottomLeft = 0x24,
        Right = 0x11,
        Left = 0x1,
        TopRight = 0x38,
        Top = 0x12,
        TopLeft = 0x34,
        ExceptBottomRight = 0x14,
        ExceptBottomLeft = 0x18,
        ExceptTopRight = 0x4,
        ExceptTopLeft = 0x8
    }
}