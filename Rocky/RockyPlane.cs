using Housing;
using RockyScript.Core;
using UnityEngine;

namespace RockyScript
{
    public abstract class RockyPlane : MonoBehaviour
    {
        public abstract void DoMyWindow();
        public abstract void Awake();

        public abstract void Copy<T>(T plane) where T: RockyPlane;

        public static void Debug(string _text)
        {
            PlaneEditor.Debug(_text);
        }
    }
}
