using System;
using System.Runtime.InteropServices;

namespace RockyScript
{
    // Token: 0x02000003 RID: 3
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenFileName
    {
        // Token: 0x04000009 RID: 9
        public int structSize = 0;

        // Token: 0x0400000A RID: 10
        public IntPtr dlgOwner = IntPtr.Zero;

        // Token: 0x0400000B RID: 11
        public IntPtr instance = IntPtr.Zero;

        // Token: 0x0400000C RID: 12
        public string filter = null;

        // Token: 0x0400000D RID: 13
        public string customFilter = null;

        // Token: 0x0400000E RID: 14
        public int maxCustFilter = 0;

        // Token: 0x0400000F RID: 15
        public int filterIndex = 0;

        // Token: 0x04000010 RID: 16
        public string file = null;

        // Token: 0x04000011 RID: 17
        public int maxFile = 0;

        // Token: 0x04000012 RID: 18
        public string fileTitle = null;

        // Token: 0x04000013 RID: 19
        public int maxFileTitle = 0;

        // Token: 0x04000014 RID: 20
        public string initialDir = null;

        // Token: 0x04000015 RID: 21
        public string title = null;

        // Token: 0x04000016 RID: 22
        public int flags = 0;

        // Token: 0x04000017 RID: 23
        public short fileOffset = 0;

        // Token: 0x04000018 RID: 24
        public short fileExtension = 0;

        // Token: 0x04000019 RID: 25
        public string defExt = null;

        // Token: 0x0400001A RID: 26
        public IntPtr custData = IntPtr.Zero;

        // Token: 0x0400001B RID: 27
        public IntPtr hook = IntPtr.Zero;

        // Token: 0x0400001C RID: 28
        public string templateName = null;

        // Token: 0x0400001D RID: 29
        public IntPtr reservedPtr = IntPtr.Zero;

        // Token: 0x0400001E RID: 30
        public int reservedInt = 0;

        // Token: 0x0400001F RID: 31
        public int flagsEx = 0;
    }
}
