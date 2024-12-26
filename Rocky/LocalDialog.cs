using System;
using System.Runtime.InteropServices;

namespace RockyScript
{
    // Token: 0x02000004 RID: 4
    public class LocalDialog
    {
        // Token: 0x0600000A RID: 10
        [DllImport("Comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
        public static extern bool GetOpenFileName([In][Out] OpenFileName ofn);

        // Token: 0x0600000B RID: 11 RVA: 0x00002A60 File Offset: 0x00000C60
        public static bool GetOFN([In][Out] OpenFileName ofn)
        {
            return LocalDialog.GetOpenFileName(ofn);
        }
    }
}
