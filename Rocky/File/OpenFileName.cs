﻿using System;
using System.Runtime.InteropServices;

namespace RockyScript.File
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenFileName
    {
        public int structSize = 0;

        public IntPtr dlgOwner = IntPtr.Zero;

        public IntPtr instance = IntPtr.Zero;

        public string filter = null;

        public string customFilter = null;

        public int maxCustFilter = 0;

        public int filterIndex = 0;

        public string file = null;

        public int maxFile = 0;

        public string fileTitle = null;

        public int maxFileTitle = 0;

        public string initialDir = null;

        public string title = null;

        public int flags = 0;

        public short fileOffset = 0;

        public short fileExtension = 0;

        public string defExt = null;

        public IntPtr custData = IntPtr.Zero;

        public IntPtr hook = IntPtr.Zero;

        public string templateName = null;

        public IntPtr reservedPtr = IntPtr.Zero;

        public int reservedInt = 0;

        public int flagsEx = 0;
    }
}
