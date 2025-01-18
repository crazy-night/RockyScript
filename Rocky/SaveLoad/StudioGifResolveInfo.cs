using System;
using MessagePack;

namespace RockyScript.SaveLoad
{
    [Serializable]
    public class StudioGifResolveInfo : StudioResolveInfoBase
    {
        [Key("fps")]
        public short fps { get; set; }

        [Key("gifpath")]
        public string gifpath { get; set; }

        public StudioGifResolveInfo() : base() { }

        public StudioGifResolveInfo(int dicKey, short fps, string gifpath) : base(dicKey)
        {
            this.fps = fps;
            this.gifpath = gifpath;
        }


        public StudioGifResolveInfo(int dicKey, GifPlane gifPlane) : this(dicKey, gifPlane.fps, gifPlane.gifpath)
        {
        }

        

        internal override void Info2Plane<T>(T plane)
        {
            Debug("GifPlane Loading...");
            if (plane == null)
            {
                throw new ArgumentNullException("Reference of monplane is null!");
            }
            else if (plane is GifPlane gifPlane)
            {
                gifPlane.fps = this.fps;
                gifPlane.gifpath = this.gifpath;
                gifPlane.LoadGif();
            }
            else
            {
                throw new Exception("Not GifPlane!");
            }
            Debug("GifPlane Successfully Load");
        }

        internal override byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}
