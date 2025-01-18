using System;
using System.Runtime.InteropServices;

namespace RockyScript.File
{
    public class LocalDialog
    {
        [DllImport("Comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
        public static extern bool GetOpenFileName([In][Out] OpenFileName ofn);

        public static bool GetOFN([In][Out] OpenFileName ofn)
        {
            return LocalDialog.GetOpenFileName(ofn);
        }
    }
}
