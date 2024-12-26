using System;
using MessagePack;

namespace RockyScript
{
    // Token: 0x02000009 RID: 9
    [MessagePackObject(false)]
    [Serializable]
    public class StudioMp4ResolveInfo
    {
        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000044 RID: 68 RVA: 0x00002238 File Offset: 0x00000438
        // (set) Token: 0x06000045 RID: 69 RVA: 0x00002240 File Offset: 0x00000440
        [Key("dicKey")]
        public int dicKey { get; set; }

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x06000046 RID: 70 RVA: 0x00002249 File Offset: 0x00000449
        // (set) Token: 0x06000047 RID: 71 RVA: 0x00002251 File Offset: 0x00000451
        [Key("soundOpen")]
        public bool soundOpen { get; set; }

        // Token: 0x17000011 RID: 17
        // (get) Token: 0x06000048 RID: 72 RVA: 0x0000225A File Offset: 0x0000045A
        // (set) Token: 0x06000049 RID: 73 RVA: 0x00002262 File Offset: 0x00000462
        [Key("mp4path")]
        public string mp4path { get; set; }

        // Token: 0x0600004A RID: 74 RVA: 0x000038FC File Offset: 0x00001AFC
        internal static StudioMp4ResolveInfo Deserialize(byte[] data)
        {
            return MessagePackSerializer.Deserialize<StudioMp4ResolveInfo>(data);
        }

        // Token: 0x0600004B RID: 75 RVA: 0x00003914 File Offset: 0x00001B14
        internal byte[] Serialize()
        {
            return MessagePackSerializer.Serialize<StudioMp4ResolveInfo>(this);
        }
    }
}
