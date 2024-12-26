using System;
using System.Collections;
using KKAPI.Studio;
using UnityEngine;

namespace RockyScript
{
    // Token: 0x02000011 RID: 17
    internal class PlaneEditorMgr : MonoBehaviour
    {
        // Token: 0x1700001D RID: 29
        // (get) Token: 0x0600007E RID: 126 RVA: 0x00002382 File Offset: 0x00000582
        // (set) Token: 0x0600007F RID: 127 RVA: 0x00002389 File Offset: 0x00000589
        public static PlaneEditorMgr Instance { get; private set; }

        // Token: 0x06000080 RID: 128 RVA: 0x00002391 File Offset: 0x00000591
        public static PlaneEditorMgr Install(GameObject container)
        {
            if (PlaneEditorMgr.Instance == null)
            {
                PlaneEditorMgr.Instance = container.AddComponent<PlaneEditorMgr>();
            }
            return PlaneEditorMgr.Instance;
        }

        // Token: 0x1700001E RID: 30
        // (get) Token: 0x06000081 RID: 129 RVA: 0x000023B0 File Offset: 0x000005B0
        // (set) Token: 0x06000082 RID: 130 RVA: 0x000023BD File Offset: 0x000005BD
        public bool VisibleGUI
        {
            get
            {
                return this.gui.VisibleGUI;
            }
            set
            {
                this.gui.VisibleGUI = value;
            }
        }

        // Token: 0x06000083 RID: 131 RVA: 0x000022A7 File Offset: 0x000004A7
        private void Awake()
        {
        }

        // Token: 0x06000084 RID: 132 RVA: 0x000023CB File Offset: 0x000005CB
        private void Start()
        {
            base.StartCoroutine(this.LoadingCo());
        }

        // Token: 0x06000085 RID: 133 RVA: 0x000023DA File Offset: 0x000005DA
        private IEnumerator LoadingCo()
        {
            yield return new WaitUntil(() => StudioAPI.StudioLoaded);
            yield return null;
            this.gui = new GameObject("GUI").AddComponent<PlaneEditorUI>();
            this.gui.transform.parent = base.transform;
            this.gui.VisibleGUI = false;
            yield break;
        }

        // Token: 0x06000086 RID: 134 RVA: 0x000023E9 File Offset: 0x000005E9
        public void ResetGUI()
        {
            this.gui.ResetGui();
        }

        // Token: 0x0400007B RID: 123
        public PlaneEditorUI gui;
    }
}
