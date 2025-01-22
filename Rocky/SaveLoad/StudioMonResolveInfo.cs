using System;
using MessagePack;

namespace RockyScript.SaveLoad
{
    [Serializable]
    public class StudioMonResolveInfo : StudioResolveInfoBase
    {
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

        [Key("follow")]
        public bool _follow { get; set; }

        [Key("downsampling")]
        public int downsampling { get; set; }

        public StudioMonResolveInfo(): base()
        {
            downsampling = 1;
        }

        public StudioMonResolveInfo(int dicKey, bool _showPlane, int cam_dickey, int cam_far, int cam_fov, int cam_spd, int cam_width, int cam_height, int cam_tex, bool _ortho, int cam_ortho, bool _follow = false, int downsampling = 2) : base(dicKey)
        {
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
            this._follow = _follow;
            this.downsampling = downsampling;
        }



        public StudioMonResolveInfo(int dicKey, MonPlane plane) : this(dicKey, plane._showPlane, plane.cam_dickey, plane.cam_far, plane.cam_fov, plane.cam_spd, plane.cam_width, plane.cam_height, plane.cam_tex, plane._ortho, plane.cam_ortho, plane._follow, plane.DownSampling)
        {
        }


        internal override void Info2Plane(RockyPlane plane)
        {
            Debug("MonPlane Loading...");
            if (plane == null)
            {
                throw new ArgumentNullException("Reference of monplane is null!");
            }
            else if (plane is MonPlane monPlane)
            {
                monPlane._showPlane = this._showPlane;
                monPlane.cam_dickey = this.cam_dickey;
                monPlane.cam_far = this.cam_far;
                monPlane.cam_fov = this.cam_fov;
                monPlane.cam_spd = this.cam_spd;
                monPlane.cam_width = this.cam_width;
                monPlane.cam_height = this.cam_height;
                monPlane.cam_tex = this.cam_tex;
                monPlane._ortho = this._ortho;
                monPlane.cam_ortho = this.cam_ortho;
                monPlane._follow = this._follow;
                monPlane.DownSampling = this.downsampling;
                monPlane.RefreshCam();
            }
            else
            {
                throw new Exception("Not MonPlane!");
            }
            Debug("MonPlane Successfully Load");

        }

        internal override byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}
