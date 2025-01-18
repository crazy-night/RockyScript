using HarmonyLib;
using JetBrains.Annotations;
using KKAPI.Utilities;
using RockyScript.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RockyScript
{
    public class SSSHook
    {
        public static void Initialize()
        {
            Harmony harmony = new Harmony("Plane.SSSHook");
            try
            {
                sss = AccessTools.TypeByName("Graphics.SSSMirrorHooks");
            }
            catch
            {
            }
            if (sss != null)
            {
                harmony.Patch(AccessTools.Method(sss, "AddMonPlaneSSSComponent", null, null), null, new HarmonyMethod(typeof(SSSHook), "GetSSSComponent", null), null, null, null);
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
                monPlane.sss = new SSSHook(component, monPlane.DownSampling);
            }
            Debug("Patch SSS for MonPlane Successfully!");
        }

        public SSSHook(Component component, int downsampling)
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


        private static Type sss;

        public Traverse DownSampling;

    }
}
