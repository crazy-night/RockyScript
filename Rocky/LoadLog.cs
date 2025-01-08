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

namespace RockyScript
{
    internal class LoadLog : SceneCustomFunctionController
    {
        protected override void OnSceneLoad(SceneOperationKind operation, ReadOnlyDictionary<int, ObjectCtrlInfo> loadedItems)
        {
            if (operation != SceneOperationKind.Clear)
            {
                PluginData sceneExtendedData = GetExtendedData();
                if (sceneExtendedData != null)
                {
                    LoadData<StudioGifResolveInfo, GifPlane>(sceneExtendedData, "GifInfo", loadedItems, (info, plane) =>
                    {
                        plane.fps = info.fps;
                        plane.gifpath = info.gifpath;
                        plane.LoadGif();
                    });

                    LoadData<StudioMp4ResolveInfo, mp4plane>(sceneExtendedData, "Mp4Info", loadedItems, (info, plane) =>
                    {
                        plane._soundOpen = info.soundOpen;
                        plane.mp4path = info.mp4path;
                        plane.Loadmp4();
                    });

                    LoadData<StudioMonResolveInfo, MonPlane>(sceneExtendedData, "MonInfo", loadedItems, (info, plane) =>
                    {
                        plane.cam_dickey = info.cam_dickey;
                        plane.cam_far = info.cam_far;
                        plane.cam_fov = info.cam_fov;
                        plane.cam_height = info.cam_height;
                        plane.cam_spd = info.cam_spd;
                        plane.cam_width = info.cam_width;
                        plane.cam_tex = info.cam_tex;
                        plane._ortho = info._ortho;
                        plane.cam_ortho = info.cam_ortho;
                        plane._showPlane = info._showPlane;
                        plane.RefreshCam();
                    });
                }
            }
        }

        protected override void OnSceneSave()
        {
            var dictionary = new Dictionary<string, object>();
            var gifList = new List<StudioGifResolveInfo>();
            var monList = new List<StudioMonResolveInfo>();
            var mp4List = new List<StudioMp4ResolveInfo>();

            var dicObjectCtrl = GetStudio().dicObjectCtrl;

            foreach (var objectCtrlInfo in dicObjectCtrl.Values)
            {
                if (objectCtrlInfo is OCIItem ociItem)
                {
                    var objectItem = ociItem.objectItem;
                    AddToList<GifPlane, StudioGifResolveInfo>(ociItem.objectInfo.dicKey, objectItem, "gifplane", ociItem, gifList);
                    AddToList<mp4plane, StudioMp4ResolveInfo>(ociItem.objectInfo.dicKey, objectItem, "mp4plane", ociItem, mp4List);
                    AddToList<MonPlane, StudioMonResolveInfo>(ociItem.objectInfo.dicKey, objectItem, "MonPlane", ociItem, monList);
                }
            }

            AddToDictionaryIfNotEmpty(dictionary, "GifInfo", gifList.Select(x => x.Serialize()).ToList());
            AddToDictionaryIfNotEmpty(dictionary, "Mp4Info", mp4List.Select(x => x.Serialize()).ToList());
            AddToDictionaryIfNotEmpty(dictionary, "MonInfo", monList.Select(x => x.Serialize()).ToList());

            SetExtendedData(dictionary.Count > 0 ? new PluginData { data = dictionary } : null);
        }

        private void AddToList<TComponent, TInfo>(
            int dicKey,
            GameObject objectItem,
            string componentName,
            OCIItem ociItem,
            List<TInfo> list)
            where TComponent : Component
            where TInfo : class
        {
            if (objectItem.name.Equals(componentName))
            {
                TComponent[] components = objectItem.GetComponentsInChildren<TComponent>();
                if (components != null)
                {
                    foreach (TComponent component in components)
                    {
                        TInfo info = (TInfo)Activator.CreateInstance(typeof(TInfo), new object[] { dicKey, component });
                        if (info != null)
                        {
                            list.Add(info);
                            LogComponentInfo("Saving " + componentName + ": DicKey " + dicKey.ToString());
                        }
                    }
                }
            }
        }

        private void LogComponentInfo(string text)
        {
            BepInEx.Logging.Logger.CreateLogSource("RockyScript").Log(LogLevel.Info, text);
        }

        private void AddToDictionaryIfNotEmpty(Dictionary<string, object> dictionary, string key, List<byte[]> list)
        {
            if (list.Count > 0)
            {
                dictionary.Add(key, list);
            }
        }

        private void LoadData<TInfo, TComponent>(
                PluginData sceneExtendedData,
                string key,
                ReadOnlyDictionary<int, ObjectCtrlInfo> loadedItems,
                Action<TInfo, TComponent> applyData)
                where TInfo : class
                where TComponent : Component
        {
            if (sceneExtendedData.data.ContainsKey(key))
            {
                List<TInfo> list = (from x in (object[])sceneExtendedData.data[key]
                                    select (TInfo)Activator.CreateInstance(typeof(TInfo), new object[] { (byte[])x })).ToList();

                foreach (TInfo info in list)
                {
                    var dicKeyProperty = typeof(TInfo).GetProperty("dicKey");
                    if (dicKeyProperty != null)
                    {
                        int dicKey = (int)dicKeyProperty.GetValue(info);
                        if (loadedItems.TryGetValue(dicKey, out ObjectCtrlInfo objectCtrlInfo) && objectCtrlInfo is OCIItem ociItem)
                        {
                            GameObject objectItem = ociItem.objectItem;
                            TComponent component = objectItem.GetComponentInChildren<TComponent>();
                            if (component != null)
                            {
                                applyData(info, component);
                            }
                        }
                    }
                }
            }
        }
    }
}
