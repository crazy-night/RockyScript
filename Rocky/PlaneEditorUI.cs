using System;
using System.Collections.Generic;
using RootMotion;
using Studio;
using UnityEngine;

namespace RockyScript
{
    // Token: 0x02000014 RID: 20
    internal class PlaneEditorUI : MonoBehaviour
    {
        // Token: 0x17000021 RID: 33
        // (get) Token: 0x06000091 RID: 145 RVA: 0x00002428 File Offset: 0x00000628
        // (set) Token: 0x06000092 RID: 146 RVA: 0x00002430 File Offset: 0x00000630
        public bool VisibleGUI { get; set; }

        // Token: 0x06000093 RID: 147 RVA: 0x00002439 File Offset: 0x00000639
        public void ResetGui()
        {
            this.ociTarget = null;
            this.guiMode = PlaneEditorUI.GuiModeType.None;
        }

        // Token: 0x06000094 RID: 148 RVA: 0x00004CFC File Offset: 0x00002EFC
        private void Start()
        {
            this.largeLabel = new GUIStyle("label");
            this.largeLabel.fontSize = 16;
            this.btnstyle = new GUIStyle("button");
            this.btnstyle.fontSize = 16;
        }

        // Token: 0x06000095 RID: 149 RVA: 0x00004D50 File Offset: 0x00002F50
        private void Update()
        {
            if (PlaneEditor.KeyShowUI.Value.IsDown())
            {
                this.VisibleGUI = !this.VisibleGUI;
                if (this.VisibleGUI)
                {
                    this.windowRect = new Rect((float)PlaneEditor.UIXPosition.Value, (float)PlaneEditor.UIYPosition.Value, (float)Math.Max(300, PlaneEditor.UIWidth.Value), (float)Math.Max(500, PlaneEditor.UIHeight.Value));
                }
                else
                {
                    PlaneEditor.UIXPosition.Value = (int)this.windowRect.x;
                    PlaneEditor.UIYPosition.Value = (int)this.windowRect.y;
                    PlaneEditor.UIWidth.Value = (int)this.windowRect.width;
                    PlaneEditor.UIHeight.Value = (int)this.windowRect.height;
                }
            }
            if (this.VisibleGUI)
            {
                TreeNodeObject currentSelectedNode = this.GetCurrentSelectedNode();
                if (currentSelectedNode != this.lastSelectedTreeNode)
                {
                    this.OnSelectChange(currentSelectedNode);
                }
            }
        }

        // Token: 0x06000096 RID: 150 RVA: 0x00004E54 File Offset: 0x00003054
        private void OnGUI()
        {
            if (this.VisibleGUI)
            {
                try
                {
                    GUIStyle style = new GUIStyle(GUI.skin.window);
                    this.windowRect = GUI.Window(this.windowID, this.windowRect, new GUI.WindowFunction(this.FuncWindowGUI), this.windowTitle, style);
                    this.mouseInWindow = this.windowRect.Contains(Event.current.mousePosition);
                    if (this.mouseInWindow)
                    {
                        Singleton<Studio.Studio>.Instance.cameraCtrl.noCtrlCondition = (() => this.mouseInWindow && this.VisibleGUI);
                        Input.ResetInputAxes();
                    }
                }
                catch (Exception value)
                {
                    Console.WriteLine(value);
                }
            }
        }

        // Token: 0x06000097 RID: 151 RVA: 0x00004F04 File Offset: 0x00003104
        private void FuncWindowGUI(int winID)
        {
            try
            {
                int hotControl = GUIUtility.hotControl;
                if (Event.current.type == EventType.MouseDown)
                {
                    GUI.FocusControl("");
                    GUI.FocusWindow(winID);
                }
                GUI.enabled = true;
                switch (this.guiMode)
                {
                    case PlaneEditorUI.GuiModeType.None:
                        GUILayout.FlexibleSpace();
                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("<color=#00ffff>" + "Please select a plane to edit." + "</color>", largeLabel);
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                        GUILayout.FlexibleSpace();
                        break;
                    case PlaneEditorUI.GuiModeType.MON:
                        this.GUIMon();
                        break;
                    case PlaneEditorUI.GuiModeType.GIF:
                        this.GUIGif();
                        break;
                    case PlaneEditorUI.GuiModeType.MP4:
                        this.GUIMp4();
                        break;
                    default:
                        throw new Exception("Unknown gui mode");
                }

                // close btn
                Rect cbRect = new Rect(windowRect.width - 16, 3, 13, 13);
                Color oldColor = GUI.color;
                GUI.color = Color.red;
                if (GUI.Button(cbRect, ""))
                {
                    VisibleGUI = false;
                }
                GUI.color = oldColor;

                GUI.DragWindow();
            }
            catch (Exception value)
            {
                Console.WriteLine(value);
                this.ResetGui();
            }
        }

        // Token: 0x06000098 RID: 152 RVA: 0x00004FB8 File Offset: 0x000031B8
        private void GUIMon()
        {
            MonPlane monPlaneComponent = this.ociTarget.objectItem.GetComponentInChildren<MonPlane>();
            if (monPlaneComponent != null)
            {
                monPlaneComponent.DoMyWindow();
            }
        }

        // Token: 0x06000099 RID: 153 RVA: 0x00002449 File Offset: 0x00000649
        private void GUIGif()
        {
            GifPlane gifPlaneComponent = this.ociTarget.objectItem.GetComponentInChildren<GifPlane>();
            if (gifPlaneComponent != null)
            {
                gifPlaneComponent.DoMyWindow();
            }
        }

        // Token: 0x0600009A RID: 154 RVA: 0x0000245C File Offset: 0x0000065C
        private void GUIMp4()
        {
            mp4plane mp4PlaneComponent = this.ociTarget.objectItem.GetComponentInChildren<mp4plane>();
            if (mp4PlaneComponent != null)
            {
                mp4PlaneComponent.DoMyWindow();
            }
        }

        // Token: 0x0600009B RID: 155 RVA: 0x0000246F File Offset: 0x0000066F
        private void OnSelectChange(TreeNodeObject newSel)
        {
            this.lastSelectedTreeNode = newSel;
            this.ociTarget = this.GetOCIItemFromNode(newSel);
        }

        // Token: 0x0600009C RID: 156 RVA: 0x00002485 File Offset: 0x00000685
        protected TreeNodeObject GetCurrentSelectedNode()
        {
            return Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectNode;
        }

        // Token: 0x0600009D RID: 157 RVA: 0x00004FE8 File Offset: 0x000031E8
        protected OCIItem GetOCIItemFromNode(TreeNodeObject node)
        {
            if (node == null)
            {
                return null;
            }
            Dictionary<TreeNodeObject, ObjectCtrlInfo> dicInfo = Singleton<Studio.Studio>.Instance.dicInfo;
            if (!dicInfo.ContainsKey(node))
            {
                return null;
            }
            ObjectCtrlInfo objectCtrlInfo = dicInfo[node];
            if (objectCtrlInfo is OCIItem)
            {
                OCIItem ociitem = objectCtrlInfo as OCIItem;
                if (ociitem.objectItem != null)
                {
                    this.dict.TryGetValue(ociitem.objectItem.name, out this.guiMode);
                    if (this.guiMode != PlaneEditorUI.GuiModeType.None)
                    {
                        return ociitem;
                    }
                }
                return null;
            }
            return null;
        }

        // Token: 0x04000081 RID: 129
        private readonly int windowID = 1313810679;

        // Token: 0x04000082 RID: 130
        private readonly string windowTitle = "Studio Plane Editor";

        // Token: 0x04000083 RID: 131
        private Rect windowRect = new Rect(0f, 300f, 300f, 500f);

        // Token: 0x04000084 RID: 132
        private bool mouseInWindow;

        // Token: 0x04000085 RID: 133
        private GUIStyle largeLabel;

        // Token: 0x04000086 RID: 134
        private GUIStyle btnstyle;

        // Token: 0x04000087 RID: 135
        private OCIItem ociTarget;

        // Token: 0x04000088 RID: 136
        private TreeNodeObject lastSelectedTreeNode;

        // Token: 0x04000089 RID: 137
        private PlaneEditorUI.GuiModeType guiMode;

        // Token: 0x0400008A RID: 138
        private Dictionary<string, PlaneEditorUI.GuiModeType> dict = new Dictionary<string, PlaneEditorUI.GuiModeType>
        {
            {
                "MonPlane",
                PlaneEditorUI.GuiModeType.MON
            },
            {
                "gifplane",
                PlaneEditorUI.GuiModeType.GIF
            },
            {
                "mp4plane",
                PlaneEditorUI.GuiModeType.MP4
            }
        };

        // Token: 0x02000015 RID: 21
        private enum GuiModeType
        {
            // Token: 0x0400008D RID: 141
            None,
            // Token: 0x0400008E RID: 142
            MON,
            // Token: 0x0400008F RID: 143
            GIF,
            // Token: 0x04000090 RID: 144
            MP4
        }
    }
}
