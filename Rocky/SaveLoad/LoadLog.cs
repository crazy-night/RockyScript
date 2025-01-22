using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using ExtensibleSaveFormat;
using Studio;
using UnityEngine;
using KKAPI.Studio.SaveLoad;
using KKAPI.Studio;
using KKAPI.Utilities;
using RockyScript.Core;
using AIProject.Animal;

namespace RockyScript.SaveLoad
{
    internal class LoadLog : SceneCustomFunctionController
    {
        private string GifKey = "GifInfo";
        private string Mp4Key = "Mp4Info";
        private string MonKey = "MonInfo";

        public static void Debug(string _text)
        {
            PlaneEditor.Debug(_text);
        }

        protected override void OnSceneLoad(SceneOperationKind operation, ReadOnlyDictionary<int, ObjectCtrlInfo> loadedItems)
        {
            if (operation == SceneOperationKind.Clear)
            {
                PlaneEditorMgr.Clear();
                return;
            }
            else if (operation == SceneOperationKind.Load)
            {
                PlaneEditorMgr.Clear();
            }
            PluginData sceneExtendedData = GetExtendedData();
            if (sceneExtendedData != null)
            {
                LoadData<StudioGifResolveInfo, GifPlane>(sceneExtendedData, GifKey, loadedItems);

                LoadData<StudioMp4ResolveInfo, mp4plane>(sceneExtendedData, Mp4Key, loadedItems);

                LoadData<StudioMonResolveInfo, MonPlane>(sceneExtendedData, MonKey, loadedItems);
            }

        }

        protected override void OnSceneSave()
        {
            var dictionary = new Dictionary<string, object>();
            List<StudioGifResolveInfo> gifList = new List<StudioGifResolveInfo>();
            List<StudioMonResolveInfo> monList = new List<StudioMonResolveInfo>();
            List<StudioMp4ResolveInfo> mp4List = new List<StudioMp4ResolveInfo>();



            foreach (KeyValuePair<OCIItem, RockyPlane> kvp in PlaneEditorMgr.PlaneDict)
            {
                RockyPlane plane = kvp.Value;
                int dicKey = kvp.Key.objectInfo.dicKey;
                if (plane is GifPlane gifPlane)
                {
                    StudioGifResolveInfo info = new StudioGifResolveInfo(dicKey, gifPlane);
                    gifList.Add(info);
                    Debug("Saving GifPlane" + ", DicKey = " + dicKey.ToString());
                }
                else if (plane is MonPlane monPlane)
                {
                    StudioMonResolveInfo info = new StudioMonResolveInfo(dicKey, monPlane);
                    monList.Add(info);
                    Debug("Saving MonPlane" + ", DicKey = " + dicKey.ToString());
                }
                else if (plane is mp4plane mp4Plane)
                {
                    StudioMp4ResolveInfo info = new StudioMp4ResolveInfo(dicKey, mp4Plane);
                    mp4List.Add(info);
                    Debug("Saving Mp4Plane" + ", DicKey = " + dicKey.ToString());

                }
                else
                {
                    throw new Exception("Unknown Plane");
                }

            }
            AddToDictionary(dictionary, GifKey, gifList.Select(x => x.Serialize()).ToList());
            AddToDictionary(dictionary, Mp4Key, mp4List.Select(x => x.Serialize()).ToList());
            AddToDictionary(dictionary, MonKey, monList.Select(x => x.Serialize()).ToList());


            SetExtendedData(dictionary.Count > 0 ? new PluginData { data = dictionary } : null);
        }
        protected override void OnObjectDeleted(ObjectCtrlInfo objectCtrlInfo)
        {
            if (objectCtrlInfo is OCIItem ociItem)
            {
                PlaneEditorMgr.Remove(ociItem);
            }
        }
        protected override void OnObjectsCopied(ReadOnlyDictionary<int, ObjectCtrlInfo> copiedItems)
        {
            foreach (KeyValuePair<int, ObjectCtrlInfo> kvp in copiedItems)
            {
                if (kvp.Value is OCIItem ociDST)
                {
                    if (GetStudio().dicObjectCtrl.TryGetValue(kvp.Key, out ObjectCtrlInfo oci))
                    {
                        OCIItem ociSRC = oci as OCIItem;
                        RockyPlane planeSRC = PlaneEditorMgr.GetPlane(ociSRC);
                        if (planeSRC != null)
                        {
                            RockyPlane planeDST = PlaneEditorMgr.GetPlane(ociDST);
                            planeDST.Copy<RockyPlane>(planeSRC);
                            Debug("Copy Successfully!");
                        }
                    }
                }
            }
        }

        private void AddToDictionary(Dictionary<string, object> dictionary, string key, List<byte[]> list)
        {
            if (list.Count > 0)
            {
                dictionary.Add(key, list);
                Debug(key + " Add to Dictionary");
            }
        }

        private void LoadData<TInfo, TPlane>(
                PluginData sceneExtendedData,
                string key,
                ReadOnlyDictionary<int, ObjectCtrlInfo> loadedItems)
                where TInfo : StudioResolveInfoBase
                where TPlane : RockyPlane
        {
            if (sceneExtendedData.data.ContainsKey(key))
            {
                List<TInfo> list = (from x in (object[])sceneExtendedData.data[key]
                                    select StudioResolveInfoBase.Deserialize<TInfo>((byte[])x)).ToList();

                foreach (StudioResolveInfoBase info in list)
                {

                    int dicKey = info.dicKey;
                    Debug("Loading " + key + ", DicKey = " + dicKey.ToString());
                    if (dicKey != -1 && loadedItems.TryGetValue(dicKey, out ObjectCtrlInfo objectCtrlInfo) && objectCtrlInfo is OCIItem ociItem)
                    {
                        TPlane plane = (TPlane)PlaneEditorMgr.GetPlane(ociItem);
                        if (plane != null)
                        {
                            Debug("Get Plane Successfully!");
                            if (info is StudioMonResolveInfo monInfo && monInfo.cam_dickey != -1)
                            {
                                monInfo.cam_dickey = StudioObjectExtensions.GetSceneId(loadedItems[monInfo.cam_dickey]);
                            }
                            info.Info2Plane(plane);
                        }
                    }
                    Debug("Loading Successfully!");
                }

            }
        }
    }
}
