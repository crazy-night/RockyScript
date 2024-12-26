using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KKAPI.Studio;
using UnityEngine;

namespace RockyScript
{
    // Token: 0x02000010 RID: 16
    [BepInPlugin("PhoeniX.RockyScript.HS2", "Plane Editor", "1.1.0")]
    [BepInDependency("marco.kkapi", ">=1.4")]
    [BepInProcess("StudioNEOV2.exe")]
    public class PlaneEditor : BaseUnityPlugin
    {
        // Token: 0x17000016 RID: 22
        // (get) Token: 0x0600006E RID: 110 RVA: 0x00002311 File Offset: 0x00000511
        // (set) Token: 0x0600006F RID: 111 RVA: 0x00002318 File Offset: 0x00000518
        public static PlaneEditor Instance { get; private set; }

        // Token: 0x17000017 RID: 23
        // (get) Token: 0x06000070 RID: 112 RVA: 0x00002320 File Offset: 0x00000520
        // (set) Token: 0x06000071 RID: 113 RVA: 0x00002327 File Offset: 0x00000527
        public static ConfigEntry<KeyboardShortcut> KeyShowUI { get; private set; }

        // Token: 0x17000018 RID: 24
        // (get) Token: 0x06000072 RID: 114 RVA: 0x0000232F File Offset: 0x0000052F
        // (set) Token: 0x06000073 RID: 115 RVA: 0x00002336 File Offset: 0x00000536
        public static ConfigEntry<bool> VerboseMessage { get; private set; }

        // Token: 0x17000019 RID: 25
        // (get) Token: 0x06000074 RID: 116 RVA: 0x0000233E File Offset: 0x0000053E
        // (set) Token: 0x06000075 RID: 117 RVA: 0x00002345 File Offset: 0x00000545
        public static ConfigEntry<int> UIXPosition { get; private set; }

        // Token: 0x1700001A RID: 26
        // (get) Token: 0x06000076 RID: 118 RVA: 0x0000234D File Offset: 0x0000054D
        // (set) Token: 0x06000077 RID: 119 RVA: 0x00002354 File Offset: 0x00000554
        public static ConfigEntry<int> UIYPosition { get; private set; }

        // Token: 0x1700001B RID: 27
        // (get) Token: 0x06000078 RID: 120 RVA: 0x0000235C File Offset: 0x0000055C
        // (set) Token: 0x06000079 RID: 121 RVA: 0x00002363 File Offset: 0x00000563
        public static ConfigEntry<int> UIWidth { get; private set; }

        // Token: 0x1700001C RID: 28
        // (get) Token: 0x0600007A RID: 122 RVA: 0x0000236B File Offset: 0x0000056B
        // (set) Token: 0x0600007B RID: 123 RVA: 0x00002372 File Offset: 0x00000572
        public static ConfigEntry<int> UIHeight { get; private set; }

        // Token: 0x0600007C RID: 124 RVA: 0x00004B18 File Offset: 0x00002D18
        private void Awake()
        {
            if (!StudioAPI.InsideStudio)
            {
                return;
            }
            PlaneEditor.Instance = this;
            PlaneEditor.Logger = base.Logger;
            PlaneEditor.KeyShowUI = base.Config.Bind<KeyboardShortcut>("General", "Plane Editor UI shortcut key", new KeyboardShortcut(KeyCode.M, new KeyCode[]
            {
                KeyCode.LeftShift
            }), "Toggles the main UI on and off.");
            PlaneEditor.VerboseMessage = base.Config.Bind<bool>("Debug", "Print verbose info", false, "Print more debug info to console.");
            PlaneEditor.UIXPosition = base.Config.Bind<int>("GUI", "Main GUI X position", 50, "X offset from left in pixel");
            PlaneEditor.UIYPosition = base.Config.Bind<int>("GUI", "Main GUI Y position", 300, "Y offset from top in pixel");
            PlaneEditor.UIWidth = base.Config.Bind<int>("GUI", "Main GUI window width", 600, "Main window width, minimum 600, set it when UI is hided.");
            PlaneEditor.UIHeight = base.Config.Bind<int>("GUI", "Main GUI window height", 400, "Main window height, minimum 400, set it when UI is hided.");
            UnityEngine.Object.DontDestroyOnLoad(new GameObject("Plane Editor"));
            PlaneEditorMgr.Install(base.gameObject);
        }

        // Token: 0x04000076 RID: 118
        public const string GUID = "PhoeniX.RockyScript.HS2";

        // Token: 0x04000077 RID: 119
        public const string Name = "Plane Editor";

        // Token: 0x04000078 RID: 120
        public const string Version = "1.1.0";

        // Token: 0x04000079 RID: 121
        internal new static ManualLogSource Logger;
    }
}
