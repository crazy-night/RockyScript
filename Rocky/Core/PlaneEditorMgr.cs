using System;
using System.Collections;
using System.Collections.Generic;
using KKAPI.Studio;
using Studio;
using UnityEngine;

namespace RockyScript.Core
{
    internal class PlaneEditorMgr : MonoBehaviour
    {
        public static PlaneEditorMgr Instance { get; private set; }
        public static Dictionary<OCIItem, RockyPlane> PlaneDict = new Dictionary<OCIItem, RockyPlane>();
        private static readonly HashSet<String> NAMES = new HashSet<String>()
        {
            "MonPlane",
            "gifplane",
            "mp4plane"
        };


        public static PlaneEditorMgr Install(GameObject container)
        {
            if (PlaneEditorMgr.Instance == null)
            {
                PlaneEditorMgr.Instance = container.AddComponent<PlaneEditorMgr>();
            }
            return PlaneEditorMgr.Instance;
        }

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

        private void Awake()
        {
        }

        private void Start()
        {
            base.StartCoroutine(this.LoadingCo());
        }

        private IEnumerator LoadingCo()
        {
            yield return new WaitUntil(() => StudioAPI.StudioLoaded);
            yield return null;
            this.gui = new GameObject("GUI").AddComponent<PlaneEditorUI>();
            this.gui.transform.parent = base.transform;
            this.gui.VisibleGUI = false;
            yield break;
        }

        
        public void ResetGUI()
        {
            this.gui.ResetGui();
        }
        internal static RockyPlane GetPlane(ObjectCtrlInfo ociTarget)
        {
            OCIItem ociItem = ociTarget as OCIItem;
            if (ociItem == null)
            {
                return null;
            }
            if (!PlaneDict.ContainsKey(ociItem))
            {
                if (ociItem.objectItem == null || ociItem.objectItem.name == null)
                {
                    return null;
                }
                if (NAMES.Contains(ociItem.objectItem.name))
                {
                    RockyPlane plane = ociItem.objectItem.GetComponentInChildren<RockyPlane>();
                    if (plane == null)
                    {
                        throw new Exception("The component of the plane does not exist!");
                    }
                    PlaneDict.Add(ociItem, plane);
                }
                else return null;
            }
            return PlaneDict[ociItem];
        }

        internal static void Clear()
        {
            PlaneDict.Clear();
        }

        internal static void Remove(OCIItem ociItem)
        {
            if (PlaneDict.ContainsKey(ociItem))
            {
                PlaneDict.Remove(ociItem);
            }
        }

        public PlaneEditorUI gui;
    }
}
