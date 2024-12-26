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
            List<StudioGifResolveInfo> list = new List<StudioGifResolveInfo>();
            List<StudioMonResolveInfo> list2 = new List<StudioMonResolveInfo>();
            List<StudioMp4ResolveInfo> list3 = new List<StudioMp4ResolveInfo>();
            Dictionary<int, ObjectCtrlInfo> dicObjectCtrl = Singleton<Studio.Studio>.Instance.dicObjectCtrl;
            foreach (ObjectCtrlInfo objectCtrlInfo in dicObjectCtrl.Values)
            {
                bool flag = objectCtrlInfo is OCIItem;
                if (flag)
                {
                    GameObject objectItem = ((OCIItem)objectCtrlInfo).objectItem;
                    bool flag2 = objectItem.name.Equals("gifplane");
                    if (flag2)
                    {
                        GifPlane[] componentsInChildren = objectItem.GetComponentsInChildren<GifPlane>();
                        bool flag3 = componentsInChildren != null;
                        if (flag3)
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
                                list.Add(studioGifResolveInfo);
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
                    bool flag4 = objectItem.name.Equals("mp4plane");
                    if (flag4)
                    {
                        mp4plane[] componentsInChildren2 = objectItem.GetComponentsInChildren<mp4plane>();
                        bool flag5 = componentsInChildren2 != null;
                        if (flag5)
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
                                list3.Add(studioMp4ResolveInfo);
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
                        bool flag6 = objectItem.name.Equals("MonPlane");
                        if (flag6)
                        {
                            MonPlane[] componentsInChildren3 = objectItem.GetComponentsInChildren<MonPlane>();
                            bool flag7 = componentsInChildren3 != null;
                            if (flag7)
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
                                    list2.Add(studioMonResolveInfo);
                                    string text3 = "SaveMonInfo!dickey:" + studioMonResolveInfo.cam_dickey.ToString();
                                    LoadLog.Debug(text3);
                                }
                            }
                        }
                    }
                }
            }
            bool flag8 = list != null;
            if (flag8)
            {
                bool flag9 = list.Count != 0;
                if (flag9)
                {
                    dictionary.Add("GifInfo", (from x in list
                                               select x.Serialize()).ToList<byte[]>());
                }
            }
            bool flag10 = list3 != null;
            if (flag10)
            {
                bool flag11 = list3.Count != 0;
                if (flag11)
                {
                    dictionary.Add("Mp4Info", (from x in list3
                                               select x.Serialize()).ToList<byte[]>());
                }
            }
            bool flag12 = list2 != null;
            if (flag12)
            {
                bool flag13 = list2.Count != 0;
                if (flag13)
                {
                    dictionary.Add("MonInfo", (from x in list2
                                               select x.Serialize()).ToList<byte[]>());
                }
            }
            bool flag14 = dictionary.Count == 0;
            if (flag14)
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
            PluginData sceneExtendedDataById = ExtendedSave.GetSceneExtendedDataById("RockyScript");
            Dictionary<int, ObjectCtrlInfo> dicObjectCtrl = Singleton<Studio.Studio>.Instance.dicObjectCtrl;
            bool flag = sceneExtendedDataById != null && sceneExtendedDataById.data.ContainsKey("GifInfo");
            if (flag)
            {
                List<StudioGifResolveInfo> list = (from x in (object[])sceneExtendedDataById.data["GifInfo"]
                                                   select StudioGifResolveInfo.Deserialize((byte[])x)).ToList<StudioGifResolveInfo>();
                foreach (StudioGifResolveInfo studioGifResolveInfo in list)
                {
                    ObjectCtrlInfo objectCtrlInfo = dicObjectCtrl[studioGifResolveInfo.dicKey];
                    bool flag2 = objectCtrlInfo is OCIItem;
                    if (flag2)
                    {
                        GameObject objectItem = ((OCIItem)objectCtrlInfo).objectItem;
                        bool flag3 = objectItem.name.Equals("gifplane");
                        if (flag3)
                        {
                            GifPlane componentInChildren = objectItem.GetComponentInChildren<GifPlane>();
                            bool flag4 = componentInChildren != null;
                            if (flag4)
                            {
                                componentInChildren.fps = studioGifResolveInfo.fps;
                                componentInChildren.gifpath = studioGifResolveInfo.gifpath;
                                componentInChildren.LoadGif();
                            }
                        }
                    }
                }
            }
            bool flag5 = sceneExtendedDataById != null && sceneExtendedDataById.data.ContainsKey("Mp4Info");
            if (flag5)
            {
                List<StudioMp4ResolveInfo> list2 = (from x in (object[])sceneExtendedDataById.data["Mp4Info"]
                                                    select StudioMp4ResolveInfo.Deserialize((byte[])x)).ToList<StudioMp4ResolveInfo>();
                foreach (StudioMp4ResolveInfo studioMp4ResolveInfo in list2)
                {
                    ObjectCtrlInfo objectCtrlInfo2 = dicObjectCtrl[studioMp4ResolveInfo.dicKey];
                    bool flag6 = objectCtrlInfo2 is OCIItem;
                    if (flag6)
                    {
                        GameObject objectItem2 = ((OCIItem)objectCtrlInfo2).objectItem;
                        bool flag7 = objectItem2.name.Equals("mp4plane");
                        if (flag7)
                        {
                            mp4plane componentInChildren2 = objectItem2.GetComponentInChildren<mp4plane>();
                            bool flag8 = componentInChildren2 != null;
                            if (flag8)
                            {
                                componentInChildren2._soundOpen = studioMp4ResolveInfo.soundOpen;
                                componentInChildren2.mp4path = studioMp4ResolveInfo.mp4path;
                                componentInChildren2.Loadmp4();
                            }
                        }
                    }
                }
            }
            bool flag9 = sceneExtendedDataById != null && sceneExtendedDataById.data.ContainsKey("MonInfo");
            if (flag9)
            {
                List<StudioMonResolveInfo> list3 = (from x in (object[])sceneExtendedDataById.data["MonInfo"]
                                                    select StudioMonResolveInfo.Deserialize((byte[])x)).ToList<StudioMonResolveInfo>();
                foreach (StudioMonResolveInfo studioMonResolveInfo in list3)
                {
                    ObjectCtrlInfo objectCtrlInfo3 = dicObjectCtrl[studioMonResolveInfo.dicKey];
                    bool flag10 = objectCtrlInfo3 is OCIItem;
                    if (flag10)
                    {
                        GameObject objectItem3 = ((OCIItem)objectCtrlInfo3).objectItem;
                        bool flag11 = objectItem3.name.Equals("MonPlane");
                        if (flag11)
                        {
                            MonPlane componentInChildren3 = objectItem3.GetComponentInChildren<MonPlane>();
                            bool flag12 = componentInChildren3 != null;
                            if (flag12)
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
            PluginData sceneExtendedDataById = ExtendedSave.GetSceneExtendedDataById("RockyScript");
            Dictionary<int, ObjectInfo> dicObject = Singleton<Studio.Studio>.Instance.sceneInfo.dicObject;
            Dictionary<int, ObjectInfo> dicImport = Singleton<Studio.Studio>.Instance.sceneInfo.dicImport;
            int num = dicObject.Count - dicImport.Count;
            Dictionary<int, ObjectCtrlInfo> dicObjectCtrl = Singleton<Studio.Studio>.Instance.dicObjectCtrl;
            bool flag = sceneExtendedDataById != null && sceneExtendedDataById.data.ContainsKey("GifInfo");
            if (flag)
            {
                List<StudioGifResolveInfo> list = (from x in (object[])sceneExtendedDataById.data["GifInfo"]
                                                   select StudioGifResolveInfo.Deserialize((byte[])x)).ToList<StudioGifResolveInfo>();
                foreach (StudioGifResolveInfo studioGifResolveInfo in list)
                {
                    ObjectCtrlInfo objectCtrlInfo = dicObjectCtrl[studioGifResolveInfo.dicKey + num];
                    bool flag2 = objectCtrlInfo is OCIItem;
                    if (flag2)
                    {
                        GameObject objectItem = ((OCIItem)objectCtrlInfo).objectItem;
                        bool flag3 = objectItem.name.Equals("gifplane");
                        if (flag3)
                        {
                            GifPlane componentInChildren = objectItem.GetComponentInChildren<GifPlane>();
                            bool flag4 = componentInChildren != null;
                            if (flag4)
                            {
                                componentInChildren.fps = studioGifResolveInfo.fps;
                                componentInChildren.gifpath = studioGifResolveInfo.gifpath;
                                componentInChildren.LoadGif();
                            }
                        }
                    }
                }
            }
            bool flag5 = sceneExtendedDataById != null && sceneExtendedDataById.data.ContainsKey("Mp4Info");
            if (flag5)
            {
                List<StudioMp4ResolveInfo> list2 = (from x in (object[])sceneExtendedDataById.data["Mp4Info"]
                                                    select StudioMp4ResolveInfo.Deserialize((byte[])x)).ToList<StudioMp4ResolveInfo>();
                foreach (StudioMp4ResolveInfo studioMp4ResolveInfo in list2)
                {
                    ObjectCtrlInfo objectCtrlInfo2 = dicObjectCtrl[studioMp4ResolveInfo.dicKey + num];
                    bool flag6 = objectCtrlInfo2 is OCIItem;
                    if (flag6)
                    {
                        GameObject objectItem2 = ((OCIItem)objectCtrlInfo2).objectItem;
                        bool flag7 = objectItem2.name.Equals("mp4plane");
                        if (flag7)
                        {
                            mp4plane componentInChildren2 = objectItem2.GetComponentInChildren<mp4plane>();
                            bool flag8 = componentInChildren2 != null;
                            if (flag8)
                            {
                                componentInChildren2._soundOpen = studioMp4ResolveInfo.soundOpen;
                                componentInChildren2.mp4path = studioMp4ResolveInfo.mp4path;
                                componentInChildren2.Loadmp4();
                            }
                        }
                    }
                }
            }
            bool flag9 = sceneExtendedDataById != null && sceneExtendedDataById.data.ContainsKey("MonInfo");
            if (flag9)
            {
                List<StudioMonResolveInfo> list3 = (from x in (object[])sceneExtendedDataById.data["MonInfo"]
                                                    select StudioMonResolveInfo.Deserialize((byte[])x)).ToList<StudioMonResolveInfo>();
                foreach (StudioMonResolveInfo studioMonResolveInfo in list3)
                {
                    ObjectCtrlInfo objectCtrlInfo3 = dicObjectCtrl[studioMonResolveInfo.dicKey + num];
                    bool flag10 = objectCtrlInfo3 is OCIItem;
                    if (flag10)
                    {
                        GameObject objectItem3 = ((OCIItem)objectCtrlInfo3).objectItem;
                        bool flag11 = objectItem3.name.Equals("MonPlane");
                        if (flag11)
                        {
                            MonPlane componentInChildren3 = objectItem3.GetComponentInChildren<MonPlane>();
                            bool flag12 = componentInChildren3 != null;
                            if (flag12)
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
