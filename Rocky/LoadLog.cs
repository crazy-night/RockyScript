using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using ExtensibleSaveFormat;
using Studio;
using UnityEngine;

namespace RockyScript
{
    // Token: 0x02000005 RID: 5
    [BepInProcess("StudioNEOV2")]
    [BepInPlugin("Rocky.LoadLog", "LoadLog", "0.0.2")]
    public class LoadLog : BaseUnityPlugin
    {
        public enum Mode { Load = 0, Import = 1 };
        // Token: 0x0600000D RID: 13 RVA: 0x00002A78 File Offset: 0x00000C78
        public void Awake()
        {
            LoadLog.Debug("Loading RockyScript!");
            ExtendedSave.SceneBeingLoaded += LoadLog.ExtendedSceneLoad;
            ExtendedSave.SceneBeingImported += LoadLog.ExtendedSceneImport;
            ExtendedSave.SceneBeingSaved += LoadLog.ExtendedSceneSave;
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002068 File Offset: 0x00000268
        public static void Debug(string _text)
        {
            BepInEx.Logging.Logger.CreateLogSource("RockyScript").Log(LogLevel.Info, _text);
        }

        // Token: 0x0600000F RID: 15 RVA: 0x00002AC8 File Offset: 0x00000CC8
        internal static void ExtendedSceneSave(string path)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            List<StudioGifResolveInfo> gifList = new List<StudioGifResolveInfo>();
            List<StudioMonResolveInfo> monList = new List<StudioMonResolveInfo>();
            List<StudioMp4ResolveInfo> mp4List = new List<StudioMp4ResolveInfo>();
            Dictionary<int, ObjectCtrlInfo> dicObjectCtrl = Singleton<Studio.Studio>.Instance.dicObjectCtrl;
            foreach (ObjectCtrlInfo objectCtrlInfo in dicObjectCtrl.Values)
            {
                bool flag = objectCtrlInfo is OCIItem;
                if (flag)
                {
                    GameObject objectItem = ((OCIItem)objectCtrlInfo).objectItem;
                    if (objectItem.name.Equals("gifplane"))
                    {
                        GifPlane[] componentsInChildren = objectItem.GetComponentsInChildren<GifPlane>();

                        if (componentsInChildren != null)
                        {
                            GifPlane[] array = componentsInChildren;
                            int num = 0;
                            if (num < array.Length)
                            {
                                GifPlane gifPlane = array[num];
                                StudioGifResolveInfo studioGifResolveInfo = new StudioGifResolveInfo();
                                studioGifResolveInfo.dicKey = objectCtrlInfo.objectInfo.dicKey;
                                studioGifResolveInfo.fps = gifPlane.fps;
                                studioGifResolveInfo.gifpath = gifPlane.gifpath;
                                gifList.Add(studioGifResolveInfo);
                                string text = string.Concat(new string[]
                                {
                                    "SaveGifInfo!dickey:",
                                    studioGifResolveInfo.dicKey.ToString(),
                                    "\tfps:",
                                    studioGifResolveInfo.fps.ToString(),
                                    "\tgifpath:",
                                    studioGifResolveInfo.gifpath
                                });
                                LoadLog.Debug(text);
                            }
                        }
                    }
                    if (objectItem.name.Equals("mp4plane"))
                    {
                        mp4plane[] componentsInChildren2 = objectItem.GetComponentsInChildren<mp4plane>();

                        if (componentsInChildren2 != null)
                        {
                            mp4plane[] array2 = componentsInChildren2;
                            int num2 = 0;
                            if (num2 < array2.Length)
                            {
                                mp4plane mp4plane = array2[num2];
                                StudioMp4ResolveInfo studioMp4ResolveInfo = new StudioMp4ResolveInfo();
                                studioMp4ResolveInfo.dicKey = objectCtrlInfo.objectInfo.dicKey;
                                studioMp4ResolveInfo.soundOpen = mp4plane._soundOpen;
                                studioMp4ResolveInfo.mp4path = mp4plane.mp4path;
                                mp4List.Add(studioMp4ResolveInfo);
                                string text2 = string.Concat(new string[]
                                {
                                    "SaveMp4Info!dickey:",
                                    studioMp4ResolveInfo.dicKey.ToString(),
                                    "\tSound Open:",
                                    studioMp4ResolveInfo.soundOpen.ToString(),
                                    "\tmp4path:",
                                    studioMp4ResolveInfo.mp4path
                                });
                                LoadLog.Debug(text2);
                            }
                        }
                    }
                    else
                    {

                        if (objectItem.name.Equals("MonPlane"))
                        {
                            MonPlane[] componentsInChildren3 = objectItem.GetComponentsInChildren<MonPlane>();

                            if (componentsInChildren3 != null)
                            {
                                MonPlane[] array3 = componentsInChildren3;
                                int num3 = 0;
                                if (num3 < array3.Length)
                                {
                                    MonPlane monPlane = array3[num3];
                                    StudioMonResolveInfo studioMonResolveInfo = new StudioMonResolveInfo();
                                    studioMonResolveInfo.dicKey = objectCtrlInfo.objectInfo.dicKey;
                                    studioMonResolveInfo.cam_dickey = monPlane.cam_dickey;
                                    studioMonResolveInfo.cam_far = monPlane.cam_far;
                                    studioMonResolveInfo.cam_fov = monPlane.cam_fov;
                                    studioMonResolveInfo.cam_height = monPlane.cam_height;
                                    studioMonResolveInfo.cam_spd = monPlane.cam_spd;
                                    studioMonResolveInfo.cam_width = monPlane.cam_width;
                                    studioMonResolveInfo.cam_tex = monPlane.cam_tex;
                                    studioMonResolveInfo._ortho = monPlane._ortho;
                                    studioMonResolveInfo.cam_ortho = monPlane.cam_ortho;
                                    studioMonResolveInfo._showPlane = monPlane._showPlane;
                                    monList.Add(studioMonResolveInfo);
                                    string text3 = "SaveMonInfo!dickey:" + studioMonResolveInfo.cam_dickey.ToString();
                                    LoadLog.Debug(text3);
                                }
                            }
                        }
                    }
                }
            }
            if (gifList != null && gifList.Count != 0)
            {
                dictionary.Add("GifInfo", (from x in gifList
                                           select x.Serialize()).ToList<byte[]>());
            }

            if (mp4List != null && mp4List.Count != 0)
            {
                dictionary.Add("Mp4Info", (from x in mp4List
                                           select x.Serialize()).ToList<byte[]>());
            }


            if (monList != null && monList.Count != 0)
            {
                dictionary.Add("MonInfo", (from x in monList
                                           select x.Serialize()).ToList<byte[]>());
            }

            if (dictionary.Count == 0)
            {
                ExtendedSave.SetSceneExtendedDataById("RockyScript", null);
            }
            else
            {
                ExtendedSave.SetSceneExtendedDataById("RockyScript", new PluginData
                {
                    data = dictionary
                });
            }
        }

        // Token: 0x06000010 RID: 16 RVA: 0x0000207E File Offset: 0x0000027E
        internal static void ExtendedSceneLoad(string path)
        {
            Singleton<Studio.Studio>.Instance.DelayToDo(0.5f, delegate
            {
                LoadLog.OnSceneLoad();
            }, false);
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00002FC8 File Offset: 0x000011C8
        internal static void OnSceneLoad()
        {

            LoadInfo(Mode.Load);
        }

        // Token: 0x06000012 RID: 18 RVA: 0x000020B1 File Offset: 0x000002B1
        internal static void ExtendedSceneImport(string path)
        {
            Singleton<Studio.Studio>.Instance.DelayToDo(0.5f, delegate
            {
                LoadLog.OnSceneImport();
            }, false);
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00003414 File Offset: 0x00001614
        internal static void OnSceneImport()
        {
            LoadInfo(Mode.Import);
        }
        internal static void LoadInfo(Mode mode)
        {
            PluginData sceneExtendedDataById = ExtendedSave.GetSceneExtendedDataById("RockyScript");
            Dictionary<int, ObjectCtrlInfo> dicObjectCtrl = Singleton<Studio.Studio>.Instance.dicObjectCtrl;
            Dictionary<int, int> dicReverseChangeKey = new Dictionary<int, int>();
            if (sceneExtendedDataById != null)
            {
                if (mode == Mode.Import)
                {
                    try
                    {
                        dicReverseChangeKey = Singleton<Studio.Studio>.Instance.sceneInfo.dicChangeKey.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                if (sceneExtendedDataById.data.ContainsKey("GifInfo"))
                {
                    List<StudioGifResolveInfo> list = (from x in (object[])sceneExtendedDataById.data["GifInfo"]
                                                       select StudioGifResolveInfo.Deserialize((byte[])x)).ToList<StudioGifResolveInfo>();

                    foreach (StudioGifResolveInfo studioGifResolveInfo in list)
                    {
                        int newDickey = -1;
                        if (mode == Mode.Load)
                        {
                            newDickey = studioGifResolveInfo.dicKey;
                        }
                        else if (mode == Mode.Import)
                        {
                            newDickey = dicReverseChangeKey[studioGifResolveInfo.dicKey];
                        }
                        else
                        {
                            LoadLog.Debug("Gif Plane: Unknown Mode!\n");
                        }
                        ObjectCtrlInfo objectCtrlInfo = dicObjectCtrl[newDickey];

                        if (objectCtrlInfo != null && objectCtrlInfo is OCIItem)
                        {
                            GameObject objectItem = ((OCIItem)objectCtrlInfo).objectItem;
                            if (objectItem.name.Equals("gifplane"))
                            {
                                GifPlane componentInChildren = objectItem.GetComponentInChildren<GifPlane>();

                                if (componentInChildren != null)
                                {
                                    componentInChildren.fps = studioGifResolveInfo.fps;
                                    componentInChildren.gifpath = studioGifResolveInfo.gifpath;
                                    componentInChildren.LoadGif();
                                }
                            }
                        }
                    }
                }
                ;
                if (sceneExtendedDataById.data.ContainsKey("Mp4Info"))
                {
                    List<StudioMp4ResolveInfo> list2 = (from x in (object[])sceneExtendedDataById.data["Mp4Info"]
                                                        select StudioMp4ResolveInfo.Deserialize((byte[])x)).ToList<StudioMp4ResolveInfo>();
                    foreach (StudioMp4ResolveInfo studioMp4ResolveInfo in list2)
                    {
                        int newDickey = -1;
                        if (mode == Mode.Load)
                        {
                            newDickey = studioMp4ResolveInfo.dicKey;
                        }
                        else if (mode == Mode.Import)
                        {
                            newDickey = dicReverseChangeKey[studioMp4ResolveInfo.dicKey];
                        }
                        else
                        {
                            LoadLog.Debug("Mp4 Plane: Unknown Mode!\n");
                        }
                        ObjectCtrlInfo objectCtrlInfo2 = dicObjectCtrl[newDickey];
                        ;
                        if (objectCtrlInfo2 != null && objectCtrlInfo2 is OCIItem)
                        {
                            GameObject objectItem2 = ((OCIItem)objectCtrlInfo2).objectItem;

                            if (objectItem2.name.Equals("mp4plane"))
                            {
                                mp4plane componentInChildren2 = objectItem2.GetComponentInChildren<mp4plane>();

                                if (componentInChildren2 != null)
                                {
                                    componentInChildren2._soundOpen = studioMp4ResolveInfo.soundOpen;
                                    componentInChildren2.mp4path = studioMp4ResolveInfo.mp4path;
                                    componentInChildren2.Loadmp4();
                                }
                            }
                        }
                    }
                }

                if (sceneExtendedDataById.data.ContainsKey("MonInfo"))
                {
                    List<StudioMonResolveInfo> list3 = (from x in (object[])sceneExtendedDataById.data["MonInfo"]
                                                        select StudioMonResolveInfo.Deserialize((byte[])x)).ToList<StudioMonResolveInfo>();
                    foreach (StudioMonResolveInfo studioMonResolveInfo in list3)
                    {
                        int newDickey = -1;
                        if (mode == Mode.Load)
                        {
                            newDickey = studioMonResolveInfo.dicKey;
                        }
                        else if (mode == Mode.Import)
                        {
                            newDickey = dicReverseChangeKey[studioMonResolveInfo.dicKey];
                        }
                        else
                        {
                            LoadLog.Debug("Mp4 Plane: Unknown Mode!\n");
                        }
                        ObjectCtrlInfo objectCtrlInfo3 = dicObjectCtrl[newDickey];

                        if (objectCtrlInfo3 != null && objectCtrlInfo3 is OCIItem)
                        {
                            GameObject objectItem3 = ((OCIItem)objectCtrlInfo3).objectItem;

                            if (objectItem3.name.Equals("MonPlane"))
                            {
                                MonPlane componentInChildren3 = objectItem3.GetComponentInChildren<MonPlane>();
                                if (componentInChildren3 != null)
                                {
                                    componentInChildren3.cam_dickey = studioMonResolveInfo.cam_dickey;
                                    componentInChildren3.cam_far = studioMonResolveInfo.cam_far;
                                    componentInChildren3.cam_fov = studioMonResolveInfo.cam_fov;
                                    componentInChildren3.cam_height = studioMonResolveInfo.cam_height;
                                    componentInChildren3.cam_spd = studioMonResolveInfo.cam_spd;
                                    componentInChildren3.cam_width = studioMonResolveInfo.cam_width;
                                    componentInChildren3.cam_tex = studioMonResolveInfo.cam_tex;
                                    componentInChildren3._ortho = studioMonResolveInfo._ortho;
                                    componentInChildren3.cam_ortho = studioMonResolveInfo.cam_ortho;
                                    componentInChildren3._showPlane = studioMonResolveInfo._showPlane;
                                    componentInChildren3.RefreshCam();
                                }
                            }
                        }
                    }
                }

            }
        }
    }
}
