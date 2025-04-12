// Building a Better ExtractAssociatedIcon
// Bradley Smith - 2010/07/28
// (updated 2014/11/13)

using Microsoft.UI.Xaml.Media.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WAMFF.Core.Utilities;

/// <summary>
/// Defines a set of utility methods for extracting icons for files and file
/// types.
/// </summary>
public static partial class FileTools
{
    public static FileAttributes GetIconForExtension(string filename) {
        uint attrb = (uint)SHGetFileInfoAttributes.FILE_ATTRIBUTE_NORMAL;
        uint flags = (uint)(SHGetFileInfoFlags.SHGFI_USEFILEATTRIBUTES | SHGetFileInfoFlags.SHGFI_TYPENAME | SHGetFileInfoFlags.SHGFI_ICON | SHGetFileInfoFlags.SHGFI_LARGEICON);

        SHFILEINFO shinfo = new();
        NativeMethods.SHGetFileInfo(filename, attrb, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);

        Icon? icon = null;

        if (shinfo.hIcon.ToInt32() != 0) {
            icon = (Icon)Icon.FromHandle(shinfo.hIcon).Clone();
            NativeMethods.DestroyIcon(shinfo.hIcon);
        }

        BitmapImage? bitmap = null;
        bitmap = icon?.ToBitMapImage();

        return new FileAttributes(shinfo.szTypeName, bitmap);
    }
}

public record FileAttributes(string Type, BitmapImage? Icon);

/// <summary>
/// Flags for SHGetFileInfo
/// </summary>
public enum SHGetFileInfoFlags : uint
{
    SHGFI_ADDOVERLAYS = 0x000000020,
    SHGFI_ATTR_SPECIFIED = 0x000020000,
    SHGFI_ATTRIBUTES = 0x000000800,
    SHGFI_DISPLAYNAME = 0x000000200,
    SHGFI_EXETYPE = 0x000002000,
    SHGFI_ICON = 0x000000100,
    SHGFI_ICONLOCATION = 0x000001000,
    SHGFI_LARGEICON = 0x000000000,
    SHGFI_LINKOVERLAY = 0x000008000,
    SHGFI_OPENICON = 0x000000002,
    SHGFI_OVERLAYINDEX = 0x000000040,
    SHGFI_PIDL = 0x000000008,
    SHGFI_SELECTED = 0x000010000,
    SHGFI_SHELLICONSIZE = 0x000000004,
    SHGFI_SMALLICON = 0x000000001,
    SHGFI_SYSICONINDEX = 0x000004000,
    SHGFI_TYPENAME = 0x000000400,
    SHGFI_USEFILEATTRIBUTES = 0x000000010
}

/// <summary>
/// File attributes for SHGetFileInfo
/// </summary>
public enum SHGetFileInfoAttributes : uint
{
    FILE_ATTRIBUTE_ARCHIVE = 0x20,
    FILE_ATTRIBUTE_HIDDEN = 0x2,
    FILE_ATTRIBUTE_NORMAL = 0x80,
    FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x2000,
    FILE_ATTRIBUTE_OFFLINE = 0x1000,
    FILE_ATTRIBUTE_READONLY = 0x1,
    FILE_ATTRIBUTE_SYSTEM = 0x4,
    FILE_ATTRIBUTE_TEMPORARY = 0x100
}


/// <summary>
/// Structure for retrieving file information with SHGetFileInfo method
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct SHFILEINFO
{
    public readonly IntPtr hIcon;
    public int iIcon;
    public uint dwAttributes;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string szDisplayName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
    public string szTypeName;
}

internal partial class NativeMethods
{
    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern nint SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

    [DllImport("user32.dll")]
    public static extern bool DestroyIcon(nint handle);
}