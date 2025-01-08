using System;
using MessagePack;

namespace RockyScript
{
    // Token: 0x02000008 RID: 8
    [MessagePackObject(false)]
    [Serializable]
    public class StudioMonResolveInfo
    {
        [Key("dicKey")]
        public int dicKey { get; set; }

        [Key("_showPlane")]
        public bool _showPlane { get; set; }

        [Key("cam_dickey")]
        public int cam_dickey { get; set; }

        [Key("cam_far")]
        public int cam_far { get; set; }

        [Key("cam_fov")]
        public int cam_fov { get; set; }

        [Key("cam_spd")]
        public int cam_spd { get; set; }

        [Key("cam_width")]
        public int cam_width { get; set; }

        [Key("cam_height")]
        public int cam_height { get; set; }

        [Key("cam_tex")]
        public int cam_tex { get; set; }

        [Key("_ortho")]
        public bool _ortho { get; set; }

        [Key("cam_ortho")]
        public int cam_ortho { get; set; }
        internal static StudioMonResolveInfo Deserialize(byte[] data)
        {
            return MessagePackSerializer.Deserialize<StudioMonResolveInfo>(data);
        }
        internal byte[] Serialize()
        {
            return MessagePackSerializer.Serialize<StudioMonResolveInfo>(this);
        }

        public StudioMonResolveInfo(byte[] data):this(Deserialize(data))
        {
        }
        public StudioMonResolveInfo(StudioMonResolveInfo instance):this(instance.dicKey, instance._showPlane, instance.cam_dickey, instance.cam_far, instance.cam_fov, instance.cam_spd, instance.cam_width, instance.cam_height, instance.cam_tex, instance._ortho, instance.cam_ortho)
        {
        }
        public StudioMonResolveInfo(int dicKey, MonPlane plane):this(dicKey, plane._showPlane, plane.cam_dickey, plane.cam_far, plane.cam_fov, plane.cam_spd, plane.cam_width, plane.cam_height, plane.cam_tex, plane._ortho, plane.cam_ortho)
        {
        }
        public StudioMonResolveInfo(int dicKey, bool _showPlane, int cam_dickey, int cam_far, int cam_fov, int cam_spd, int cam_width, int cam_height, int cam_tex, bool _ortho, int cam_ortho)
        {
            this.dicKey = dicKey;
            this._showPlane = _showPlane;
            this.cam_dickey = cam_dickey;
            this.cam_far = cam_far;
            this.cam_fov = cam_fov;
            this.cam_spd = cam_spd;
            this.cam_width = cam_width;
            this.cam_height = cam_height;
            this.cam_tex = cam_tex;
            this._ortho = _ortho;
            this.cam_ortho = cam_ortho;
        }
        public StudioMonResolveInfo()
        {
            this.dicKey = -1;
        }
    }
}
