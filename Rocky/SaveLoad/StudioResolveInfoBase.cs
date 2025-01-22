using MessagePack;
using RockyScript.Core;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RockyScript.SaveLoad
{
    [MessagePackObject]
    [Serializable]
    public abstract class StudioResolveInfoBase
    {
        [Key("dicKey")]
        public int dicKey { get; set; }

        public StudioResolveInfoBase()
        {
            this.dicKey = -1;
        }
        

        protected StudioResolveInfoBase(int dicKey)
        {
            this.dicKey = dicKey;
        }

        internal static T Deserialize<T>(byte[] data) where T : StudioResolveInfoBase
        {
            return MessagePackSerializer.Deserialize<T>(data);
        }

        internal abstract byte[] Serialize();
        internal abstract void Info2Plane(RockyPlane plane);

        public static void Debug(string _text)
        {
            PlaneEditor.Debug(_text);
        }


    }
}
