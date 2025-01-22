using System;
using MessagePack;

namespace RockyScript.SaveLoad
{
    [Serializable]
    public class StudioMp4ResolveInfo : StudioResolveInfoBase
    {
        [Key("soundOpen")]
        public bool soundOpen { get; set; }

        [Key("mp4path")]
        public string mp4path { get; set; }

        
        public StudioMp4ResolveInfo() : base() { }

        public StudioMp4ResolveInfo(int dicKey, bool soundOpen, string mp4path) : base(dicKey)
        {
            this.soundOpen = soundOpen;
            this.mp4path = mp4path;
        }


        

        public StudioMp4ResolveInfo(int dicKey, mp4plane plane) : this(dicKey, plane._soundOpen, plane.mp4path)
        {
        }

        

        internal override void Info2Plane(RockyPlane plane)
        {
            Debug("Mp4Plane Loading...");
            if (plane == null)
            {
                throw new ArgumentNullException("Reference of mp4plane is null!"); 
            }
            else if (plane is mp4plane mp4Plane)
            {
                mp4Plane._soundOpen = this.soundOpen;
                mp4Plane.mp4path = this.mp4path;
                mp4Plane.LoadMp4();
            }
            else
            {
                throw new Exception("Not mp4plane!");
            }
            Debug("Mp4Plane Successfully Load");
        }


        internal override byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}
