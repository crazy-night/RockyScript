using System;
using MessagePack;

namespace RockyScript
{
    // Token: 0x02000007 RID: 7
    [MessagePackObject(false)]
    [Serializable]
    public class StudioGifResolveInfo
    {
        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000022 RID: 34 RVA: 0x0000214A File Offset: 0x0000034A
        // (set) Token: 0x06000023 RID: 35 RVA: 0x00002152 File Offset: 0x00000352
        [Key("dicKey")]
        public int dicKey { get; set; }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000024 RID: 36 RVA: 0x0000215B File Offset: 0x0000035B
        // (set) Token: 0x06000025 RID: 37 RVA: 0x00002163 File Offset: 0x00000363
        [Key("fps")]
        public short fps { get; set; }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000026 RID: 38 RVA: 0x0000216C File Offset: 0x0000036C
        // (set) Token: 0x06000027 RID: 39 RVA: 0x00002174 File Offset: 0x00000374
        [Key("gifpath")]
        public string gifpath { get; set; }

        // Token: 0x06000028 RID: 40 RVA: 0x0000389C File Offset: 0x00001A9C
        internal static StudioGifResolveInfo Deserialize(byte[] data)
        {
            return MessagePackSerializer.Deserialize<StudioGifResolveInfo>(data);
        }

        // Token: 0x06000029 RID: 41 RVA: 0x000038B4 File Offset: 0x00001AB4
        internal byte[] Serialize()
        {
            return MessagePackSerializer.Serialize<StudioGifResolveInfo>(this);
        }
    }
}
