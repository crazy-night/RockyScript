using System;
using MessagePack;

namespace RockyScript
{
    // Token: 0x02000008 RID: 8
    [MessagePackObject(false)]
    [Serializable]
    public class StudioMonResolveInfo
    {
        // Token: 0x17000004 RID: 4
        // (get) Token: 0x0600002B RID: 43 RVA: 0x0000217D File Offset: 0x0000037D
        // (set) Token: 0x0600002C RID: 44 RVA: 0x00002185 File Offset: 0x00000385
        [Key("dicKey")]
        public int dicKey { get; set; }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x0600002D RID: 45 RVA: 0x0000218E File Offset: 0x0000038E
        // (set) Token: 0x0600002E RID: 46 RVA: 0x00002196 File Offset: 0x00000396
        [Key("_showPlane")]
        public bool _showPlane { get; set; }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x0600002F RID: 47 RVA: 0x0000219F File Offset: 0x0000039F
        // (set) Token: 0x06000030 RID: 48 RVA: 0x000021A7 File Offset: 0x000003A7
        [Key("cam_dickey")]
        public int cam_dickey { get; set; }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000031 RID: 49 RVA: 0x000021B0 File Offset: 0x000003B0
        // (set) Token: 0x06000032 RID: 50 RVA: 0x000021B8 File Offset: 0x000003B8
        [Key("cam_far")]
        public int cam_far { get; set; }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000033 RID: 51 RVA: 0x000021C1 File Offset: 0x000003C1
        // (set) Token: 0x06000034 RID: 52 RVA: 0x000021C9 File Offset: 0x000003C9
        [Key("cam_fov")]
        public int cam_fov { get; set; }

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x06000035 RID: 53 RVA: 0x000021D2 File Offset: 0x000003D2
        // (set) Token: 0x06000036 RID: 54 RVA: 0x000021DA File Offset: 0x000003DA
        [Key("cam_spd")]
        public int cam_spd { get; set; }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x06000037 RID: 55 RVA: 0x000021E3 File Offset: 0x000003E3
        // (set) Token: 0x06000038 RID: 56 RVA: 0x000021EB File Offset: 0x000003EB
        [Key("cam_width")]
        public int cam_width { get; set; }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x06000039 RID: 57 RVA: 0x000021F4 File Offset: 0x000003F4
        // (set) Token: 0x0600003A RID: 58 RVA: 0x000021FC File Offset: 0x000003FC
        [Key("cam_height")]
        public int cam_height { get; set; }

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x0600003B RID: 59 RVA: 0x00002205 File Offset: 0x00000405
        // (set) Token: 0x0600003C RID: 60 RVA: 0x0000220D File Offset: 0x0000040D
        [Key("cam_tex")]
        public int cam_tex { get; set; }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x0600003D RID: 61 RVA: 0x00002216 File Offset: 0x00000416
        // (set) Token: 0x0600003E RID: 62 RVA: 0x0000221E File Offset: 0x0000041E
        [Key("_ortho")]
        public bool _ortho { get; set; }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x0600003F RID: 63 RVA: 0x00002227 File Offset: 0x00000427
        // (set) Token: 0x06000040 RID: 64 RVA: 0x0000222F File Offset: 0x0000042F
        [Key("cam_ortho")]
        public int cam_ortho { get; set; }

        // Token: 0x06000041 RID: 65 RVA: 0x000038CC File Offset: 0x00001ACC
        internal static StudioMonResolveInfo Deserialize(byte[] data)
        {
            return MessagePackSerializer.Deserialize<StudioMonResolveInfo>(data);
        }

        // Token: 0x06000042 RID: 66 RVA: 0x000038E4 File Offset: 0x00001AE4
        internal byte[] Serialize()
        {
            return MessagePackSerializer.Serialize<StudioMonResolveInfo>(this);
        }
    }
}
