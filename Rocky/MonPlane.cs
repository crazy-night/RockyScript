using System;
using System.Collections.Generic;
using System.Linq;
using Studio;
using UnityEngine;

namespace RockyScript
{
    // Token: 0x0200000D RID: 13
    public class MonPlane : MonoBehaviour
    {
        // Token: 0x0600005C RID: 92 RVA: 0x000038C0 File Offset: 0x00001AC0
        private void Start()
        {
            this._windowRect = new Rect((float)((double)Screen.width / 2.0 - 50.0), (float)((double)Screen.height / 2.0 - 150.0), 300f, 300f);
            this.m_renderer = base.GetComponent<Renderer>();
            this.planeCam = base.gameObject.GetComponentInChildren<Camera>();
            Vector3 position = base.gameObject.GetComponentInChildren<Camera>().transform.position;
            Quaternion rotation = base.gameObject.GetComponentInChildren<Camera>().transform.rotation;
            this.curTestCameras = Camera.allCameras;
            foreach (Camera camera in this.curTestCameras)
            {
                if (camera.name.Equals("MainCamera"))
                {
                    this.planeCam.CopyFrom(camera);
                    this.planeCam.targetDisplay = 7;
                    break;
                }
            }
            this.planeCam.transform.position = position;
            this.planeCam.transform.rotation = rotation;
            double num = (double)(base.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x * Mathf.Abs(base.gameObject.transform.lossyScale.x));
            double num2 = (double)(base.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.y * Mathf.Abs(base.gameObject.transform.lossyScale.y));
            this.plane_ratio = num2 / num;
            this.cam_width = this.cam_tex;
            this.cam_height = (int)((double)this.cam_tex * this.plane_ratio);
            this.RefreshCam();
            this.curAllCameras = new List<OCICamera>();
        }

        public void DoMyWindow()
        {
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Camera name:", Array.Empty<GUILayoutOption>());
            GUILayout.Label(this.cam_name, Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Fov:", Array.Empty<GUILayoutOption>());
            this.fovstring = GUILayout.TextField(this.fovstring, 3, Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("OrthoSize:", Array.Empty<GUILayoutOption>());
            this.orthostring = GUILayout.TextField(this.orthostring, 3, Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Far:", Array.Empty<GUILayoutOption>());
            this.farstring = GUILayout.TextField(this.farstring, 4, Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("FollowSpeed:", Array.Empty<GUILayoutOption>());
            this.spdstring = GUILayout.TextField(this.spdstring, 3, Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("TextureResolution:", Array.Empty<GUILayoutOption>());
            this.texstring = GUILayout.TextField(this.texstring, 2, Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            if (this._ortho)
            {
                if (GUILayout.Button("Current:OrthoSize used", Array.Empty<GUILayoutOption>()))
                {
                    this._ortho = false;
                }
            }
            else if (GUILayout.Button("Current:Fov used", Array.Empty<GUILayoutOption>()))
            {
                this._ortho = true;
            }
            if (this._showPlane)
            {
                if (GUILayout.Button("Current:ShowPlane", Array.Empty<GUILayoutOption>()))
                {
                    this._showPlane = false;
                }
            }
            else if (GUILayout.Button("Current:HidePlane", Array.Empty<GUILayoutOption>()))
            {
                this._showPlane = true;
            }
            if (GUILayout.Button("RefreshCamList", Array.Empty<GUILayoutOption>()))
            {
                this.curAllCameras.Clear();
                this.curAllCameras.AddRange(Singleton<Studio.Studio>.Instance.dicObjectCtrl.Where(delegate (KeyValuePair<int, ObjectCtrlInfo> kvp)
                {
                    KeyValuePair<int, ObjectCtrlInfo> keyValuePair = kvp;
                    return keyValuePair.Value is OCICamera;
                }).Select(delegate (KeyValuePair<int, ObjectCtrlInfo> kvp)
                {
                    KeyValuePair<int, ObjectCtrlInfo> keyValuePair = kvp;
                    return keyValuePair.Value as OCICamera;
                }));
            }
            if (this.curAllCameras != null && this.curAllCameras.Count > 0)
            {
                this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, new GUILayoutOption[]
                {
                    GUILayout.Width(180f),
                    GUILayout.Height(100f)
                });
                foreach (OCICamera ocicamera in this.curAllCameras)
                {
                    if (GUILayout.Button(ocicamera.cameraInfo.name, Array.Empty<GUILayoutOption>()))
                    {
                        this.cam_dickey = ocicamera.objectInfo.dicKey;
                        this.cam_name = ocicamera.cameraInfo.name;
                    }
                }
                GUILayout.EndScrollView();
            }
            int num = 10;
            if (GUILayout.Button("RefreshPlane", Array.Empty<GUILayoutOption>()))
            {
                try
                {
                    this.cam_fov = (int)short.Parse(this.fovstring);
                    this.cam_far = (int)short.Parse(this.farstring);
                    this.cam_spd = (int)short.Parse(this.spdstring);
                    num = (int)short.Parse(this.texstring);
                    this.cam_ortho = (int)short.Parse(this.orthostring);
                }
                catch
                {
                    this.cam_fov = 23;
                    this.fovstring = "23";
                    this.cam_far = 1000;
                    this.farstring = "1000";
                    this.cam_spd = 30;
                    this.spdstring = "30";
                    this.cam_tex = 1024;
                    this.texstring = "10";
                }
                if (this.cam_fov < 0)
                {
                    this.cam_fov = 23;
                    this.fovstring = "23";
                }
                if (this.cam_far < 0)
                {
                    this.cam_far = 1000;
                    this.farstring = "1000";
                }
                if (this.cam_spd < 0)
                {
                    this.cam_spd = 30;
                    this.spdstring = "30";
                }
                if (num < 0)
                {
                    this.cam_tex = 1;
                    this.texstring = "0";
                }
                else if (num >= 14)
                {
                    this.cam_tex = 16384;
                    this.texstring = "14";
                }
                else
                {
                    this.cam_tex = 1 << num;
                }
                if (this.cam_ortho <= 0)
                {
                    this.cam_ortho = 3;
                    this.orthostring = "3";
                }
                double num2 = (double)(base.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x * Mathf.Abs(base.gameObject.transform.lossyScale.x));
                double num3 = (double)(base.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.y * Mathf.Abs(base.gameObject.transform.lossyScale.y));
                this.plane_ratio = num3 / num2;
                this.cam_width = this.cam_tex;
                this.cam_height = Math.Min(1<<14,(int)((double)this.cam_tex *this.plane_ratio));
                this.RefreshCam();
            }
            GUILayout.EndVertical();
        }

        // Token: 0x0600005F RID: 95 RVA: 0x000040D4 File Offset: 0x000022D4
        public void RefreshCam()
        {
            object obj = MonPlane.lockObj;
            lock (obj)
            {
                if (this.m_renderTexture != null)
                {
                    this.planeCam.targetTexture = null;
                    this.m_renderer.material.mainTexture = null;
                    this.m_renderTexture.Release();
                }
                this.m_renderTexture = new RenderTexture(this.cam_width, this.cam_height, 16, RenderTextureFormat.ARGB32);
                if (this.cam_dickey != -1)
                {
                    try
                    {
                        ObjectCtrlInfo objectCtrlInfo = Singleton<Studio.Studio>.Instance.dicObjectCtrl[this.cam_dickey];
                        if (objectCtrlInfo is OCICamera)
                        {
                            OCICamera ocicamera = (OCICamera)objectCtrlInfo;
                            this.m_camTransform = ocicamera.objectItem.transform;
                            this.planeCam.transform.position = this.m_camTransform.transform.position;
                            this.planeCam.transform.rotation = this.m_camTransform.transform.rotation;
                        }
                    }
                    catch
                    {
                        return;
                    }
                }
                if (this._showPlane)
                {
                    this.planeCam.cullingMask |= this.mask;
                }
                else
                {
                    this.planeCam.cullingMask &= ~this.mask;
                }
                this.planeCam.cullingMask |= 1 << 9;
                this.planeCam.cullingMask &= ~(1 << 14);

                this.planeCam.fieldOfView = (float)this.cam_fov;
                this.planeCam.farClipPlane = (float)this.cam_far;
                this.planeCam.orthographic = this._ortho;
                this.planeCam.orthographicSize = (float)this.cam_ortho;
                this.planeCam.targetTexture = this.m_renderTexture;
                this.m_renderer.material.mainTexture = this.m_renderTexture;
            }
        }

        // Token: 0x06000060 RID: 96 RVA: 0x00004294 File Offset: 0x00002494
        private void LateUpdate()
        {
            if (this.cam_dickey != -1 && this.m_camTransform != null && this.m_camTransform.hasChanged)
            {
                object obj = MonPlane.lockObj;
                lock (obj)
                {
                    this.planeCam.transform.position = Vector3.Lerp(this.planeCam.transform.position, this.m_camTransform.transform.position, (float)this.cam_spd * Time.deltaTime);
                    this.planeCam.transform.rotation = Quaternion.Slerp(this.planeCam.transform.rotation, this.m_camTransform.transform.rotation, (float)this.cam_spd * Time.deltaTime);
                }
            }
        }

        // Token: 0x06000063 RID: 99 RVA: 0x000022DC File Offset: 0x000004DC
        private void OnDestroy()
        {
            this.planeCam.targetTexture = null;
            this.m_renderer.material.mainTexture = null;
            this.m_renderTexture.Release();
        }

        // Token: 0x04000047 RID: 71
        public bool _showPlane;

        // Token: 0x04000048 RID: 72
        public bool _ortho = true;

        // Token: 0x04000049 RID: 73
        public Renderer m_renderer;

        // Token: 0x0400004A RID: 74
        public int cam_dickey = -1;

        // Token: 0x0400004B RID: 75
        private string cam_name = "null";

        // Token: 0x0400004C RID: 76
        public int cam_far = 1000;

        // Token: 0x0400004D RID: 77
        private string farstring = "1000";

        // Token: 0x0400004E RID: 78
        public int cam_fov = 23;

        // Token: 0x0400004F RID: 79
        private string fovstring = "23";

        // Token: 0x04000050 RID: 80
        public int cam_spd = 30;

        // Token: 0x04000051 RID: 81
        private string spdstring = "30";

        // Token: 0x04000052 RID: 82
        public int cam_tex = 1024;

        // Token: 0x04000053 RID: 83
        private string texstring = "10";

        // Token: 0x04000054 RID: 84
        public int cam_ortho = 3;

        // Token: 0x04000055 RID: 85
        private string orthostring = "3";

        // Token: 0x04000056 RID: 86
        public int cam_width;

        // Token: 0x04000057 RID: 87
        public int cam_height;

        // Token: 0x04000058 RID: 88
        public Vector2 scrollPosition;

        // Token: 0x04000059 RID: 89
        public Camera planeCam;

        // Token: 0x0400005A RID: 90
        private static object lockObj = new object();

        // Token: 0x0400005B RID: 91
        public Rect _windowRect;

        // Token: 0x0400005C RID: 92
        public RenderTexture m_renderTexture;

        // Token: 0x0400005D RID: 93
        private List<OCICamera> curAllCameras;

        // Token: 0x0400005E RID: 94
        public Transform m_camTransform;

        // Token: 0x0400005F RID: 95
        public Camera[] curTestCameras;

        // Token: 0x04000060 RID: 96
        public OCIItem monplane;

        // Token: 0x04000061 RID: 97
        public int mask = 536870912;

        // Token: 0x04000062 RID: 98
        public double plane_ratio;
    }
}

