using HarmonyLib;
using RockyScript.Core;
using System;
using UnityEngine;

namespace RockyScript.Hook
{
    public class GraphicHook
    {
        public static void Initialize()
        {
            Harmony harmony = new Harmony("Plane.GraphicHook");
            try
            {
                SSS = AccessTools.TypeByName("Graphics.SSSMirrorHooks");
            }
            catch
            {
            }
            if (SSS != null)
            {
                harmony.Patch(AccessTools.Method(SSS, "AddMonPlaneSSSComponent", null, null), null, new HarmonyMethod(typeof(GraphicHook), "GetSSSComponent", null), null, null, null);
                Debug("Initialize Patch Successfully!");
            }
        }
        public static void GetSSSComponent(object __0)
        {
            Debug("Patch SSS for MonPlane begin!");
            MonPlane monPlane = (MonPlane)__0;
            Component component = monPlane.planeCam.gameObject.GetComponent("SSS");
            if (monPlane != null && component != null)
            {
                monPlane.sss = new GraphicHook(component, monPlane.DownSampling);
                Debug("Patch SSS for MonPlane Successfully!");
            }
        }

        public GraphicHook(Component component, int downsampling)
        {
            Traverse sssTraverse = Traverse.Create(component);
            this.DownSampling = sssTraverse.Property("Downsampling");
            if (!this.DownSampling.PropertyExists())
            {
                throw new Exception("Can not get Downsampling from SSS");
            }
            else
            {
                this.SetDownSampling(downsampling);
            }
        }

        public void SetDownSampling(int downsampling)
        {
            if (this.DownSampling != null && this.DownSampling.PropertyExists())
            {
                this.DownSampling.SetValue((float)downsampling);
            }
        }


        private static void Debug(string message)
        {
            PlaneEditor.Debug(message);
        }


        private static Type SSS;

        public Traverse DownSampling;

    }
}
