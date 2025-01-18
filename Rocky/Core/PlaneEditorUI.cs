using System;
using System.Collections.Generic;
using Studio;
using UnityEngine;

namespace RockyScript.Core
{
    internal class PlaneEditorUI : MonoBehaviour
    {

        public bool VisibleGUI { get; set; }


        public void ResetGui()
        {
            this.ociTarget = null;
        }


        private void Start()
        {
            this.largeLabel = new GUIStyle("label");
            this.largeLabel.fontSize = 16;
            this.btnstyle = new GUIStyle("button");
            this.btnstyle.fontSize = 16;
        }


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

                RockyPlane plane = PlaneEditorMgr.GetPlane(ociTarget);
                if (plane == null)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("<color=#00ffff>" + "Please select a plane to edit." + "</color>", largeLabel);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();
                }
                else
                {
                    plane.DoMyWindow();
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


        

        private void OnSelectChange(TreeNodeObject newSel)
        {
            lastSelectedTreeNode = newSel;
            ociTarget = GetOCIItemFromNode(newSel);
        }

        protected TreeNodeObject GetCurrentSelectedNode()
        {
            return Studio.Studio.Instance.treeNodeCtrl.selectNode;
        }

        

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
                OCIItem ociItem = objectCtrlInfo as OCIItem;
                if (ociItem.objectItem != null)
                {
                    if (NAMES.Contains(ociItem.objectItem.name))
                    {
                        return ociItem;
                    }
                }
                return null;
            }
            return null;
        }


        private readonly int windowID = 1313810679;


        private readonly string windowTitle = "Studio RockyPlane Editor";


        private Rect windowRect = new Rect(0f, 300f, 300f, 500f);


        private bool mouseInWindow;


        private GUIStyle largeLabel;


        private GUIStyle btnstyle;


        private OCIItem ociTarget;


        private TreeNodeObject lastSelectedTreeNode;




        private readonly HashSet<String> NAMES = new HashSet<String>()
        {
            "MonPlane",
            "gifplane",
            "mp4plane"
        };
        
    }
}
