using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KKAPI;
using KKAPI.Studio;
using KKAPI.Studio.SaveLoad;
using UnityEngine;
using RockyScript.SaveLoad;
using System;
using RockyScript.Hook;

namespace RockyScript.Core
{
    [BepInPlugin("PhoeniX.RockyScript.HS2", "RockyPlane Editor", "1.1.1")]
    [BepInDependency(KoikatuAPI.GUID, "1.4")]
    [BepInProcess("StudioNEOV2.exe")]
    public class PlaneEditor : BaseUnityPlugin
    {
        public static PlaneEditor Instance { get; private set; }

        public static ConfigEntry<KeyboardShortcut> KeyShowUI { get; private set; }

        public static ConfigEntry<bool> VerboseMessage { get; private set; }

        public static ConfigEntry<int> UIXPosition { get; private set; }

        public static ConfigEntry<int> UIYPosition { get; private set; }

        public static ConfigEntry<int> UIWidth { get; private set; }

        public static ConfigEntry<int> UIHeight { get; private set; }
        private void Awake()
        {
            if (!StudioAPI.InsideStudio)
            {
                return;
            }
            PlaneEditor.Instance = this;
            PlaneEditor.Logger = base.Logger;
            PlaneEditor.KeyShowUI = base.Config.Bind<KeyboardShortcut>("General", "RockyPlane Editor UI shortcut key", new KeyboardShortcut(KeyCode.M, new KeyCode[]
            {
                KeyCode.LeftShift
            }), "Toggles the main UI on and off.");
            PlaneEditor.VerboseMessage = base.Config.Bind<bool>("Debug", "Print verbose info", false, "Print more debug info to console.");
            PlaneEditor.UIXPosition = base.Config.Bind<int>("GUI", "Main GUI X position", 50, "X offset from left in pixel");
            PlaneEditor.UIYPosition = base.Config.Bind<int>("GUI", "Main GUI Y position", 300, "Y offset from top in pixel");
            PlaneEditor.UIWidth = base.Config.Bind<int>("GUI", "Main GUI window width", 300, "Main window width, minimum 200, set it when UI is hided.");
            PlaneEditor.UIHeight = base.Config.Bind<int>("GUI", "Main GUI window height", 500, "Main window height, minimum 100, set it when UI is hided.");

            StudioSaveLoadApi.RegisterExtraBehaviour<LoadLog>("RockyScript");

            GameObject gameObject = new GameObject("RockyPlane Editor");
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            PlaneEditorMgr.Install(gameObject);

            GraphicHook.Initialize();
        }

        public static void Debug(string _text)
        {
            if (PlaneEditor.VerboseMessage.Value)
            {
                Console.WriteLine(Name + ": " + _text);
                //PlaneEditor.Logger.LogDebug(Name + ": " + _text);
            }
        }

        public const string GUID = "PhoeniX.RockyScript.HS2";

        public const string Name = "RockyPlane Editor";

        public const string Version = "1.1.1";

        internal new static ManualLogSource Logger;
    }
}
