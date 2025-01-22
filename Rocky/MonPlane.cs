using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Studio;
using UnityEngine;
using KKAPI.Utilities;
using UnityEngine.Rendering.PostProcessing;
using RockyScript.Hook;

namespace RockyScript
{
    public class MonPlane : RockyPlane
    {
        public override void Awake()
        {
            curAllCameras = new List<OCICamera>();
            m_renderer = base.GetComponent<Renderer>();
            planeCam = base.gameObject.GetComponentInChildren<Camera>();


            cam_dickey = -1;
            cam_name = "null";
            DownSampling = 1;

            Initialize();
            _ortho = planeCam.orthographic;


            double num = (double)(base.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x * Mathf.Abs(base.gameObject.transform.lossyScale.x));
            double num2 = (double)(base.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.y * Mathf.Abs(base.gameObject.transform.lossyScale.y));
            double plane_ratio = num2 / num;
            cam_tex = 1024;
            cam_width = cam_tex;
            cam_height = (int)((double)cam_tex * plane_ratio);

        }

        public void Start()
        {
            if (cam_dickey == -1)
            {
                Vector3 position = planeCam.transform.position;
                Quaternion rotation = planeCam.transform.rotation;

                if (PostProcessHook.MainCamera != null)
                {
                    planeCam.CopyFrom(PostProcessHook.MainCamera);
                    planeCam.targetDisplay = 7;
                    cam_name = "Main Camera";
                }
                else
                {
                    throw new Exception("Cannot find main camera");
                }

                planeCam.transform.position = position;
                planeCam.transform.rotation = rotation;


            }
            if (PostProcessHook.Resources != null)
            {
                PostProcessLayer postProcessLayer = planeCam.gameObject.GetOrAddComponent<PostProcessLayer>();

                postProcessLayer.Init(PostProcessHook.Resources);
                postProcessLayer.volumeTrigger = planeCam.transform;
                postProcessLayer.volumeLayer = PostProcessHook.volumeLayer;
                Debug("Init PostProcessLayer Successfully!");
            }

            RefreshCam();
        }


        public override void DoMyWindow()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Camera name: ", IMGUIUtils.EmptyLayoutOptions);
            GUILayout.FlexibleSpace();
            GUILayout.Label(cam_name, IMGUIUtils.EmptyLayoutOptions);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (_ortho)
            {

                if (GUILayout.Button("Current: Orthographic used", IMGUIUtils.EmptyLayoutOptions))
                {
                    _ortho = false;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Ortho Size: ", IMGUIUtils.EmptyLayoutOptions);
                GUILayout.FlexibleSpace();
                orthostring = GUILayout.TextField(orthostring, 3, GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }
            else
            {
                if (GUILayout.Button("Current: Perspective used", IMGUIUtils.EmptyLayoutOptions))
                {
                    _ortho = true;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Field of View: ", IMGUIUtils.EmptyLayoutOptions);
                GUILayout.FlexibleSpace();
                fovstring = GUILayout.TextField(fovstring, 3, GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }


            GUILayout.BeginHorizontal();
            GUILayout.Label("Far Clip Plane: ", IMGUIUtils.EmptyLayoutOptions);
            GUILayout.FlexibleSpace();
            farstring = GUILayout.TextField(farstring, 5, GUILayout.Width(100));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Follow Speed: ", IMGUIUtils.EmptyLayoutOptions);
            GUILayout.FlexibleSpace();
            spdstring = GUILayout.TextField(spdstring, 3, GUILayout.Width(100));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Texture Resolution: ", IMGUIUtils.EmptyLayoutOptions);
            GUILayout.FlexibleSpace();
            if (cam_tex > 128 && GUILayout.Button("-", GUILayout.Width(25), GUILayout.Height(25)))
            {
                cam_tex >>= 1;
            }
            GUILayout.Label(cam_tex.ToString(), GUILayout.Height(25));
            if (cam_tex >= 16384)
            {
                GUILayout.Space(25);
            }
            else if (GUILayout.Button("+", GUILayout.Width(25), GUILayout.Height(25)))
            {
                cam_tex <<= 1;
            }
            GUILayout.EndHorizontal();

            if (sss != null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("SSS Downsampling: ", IMGUIUtils.EmptyLayoutOptions);
                GUILayout.FlexibleSpace();
                if (DownSampling > 1 && GUILayout.Button("-", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    DownSampling >>= 1;
                    sss.SetDownSampling(DownSampling);
                }
                GUILayout.Label(DownSampling.ToString(), GUILayout.Height(25));
                if (DownSampling >= 32)
                {
                    GUILayout.Space(25);
                }
                else if (GUILayout.Button("+", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    DownSampling <<= 1;
                    sss.SetDownSampling(DownSampling);
                }
                GUILayout.EndHorizontal();
            }


            if (_showPlane)
            {
                if (GUILayout.Button("Current: Show Plane", IMGUIUtils.EmptyLayoutOptions))
                {
                    _showPlane = false;
                }
            }
            else if (GUILayout.Button("Current: Hide Plane", IMGUIUtils.EmptyLayoutOptions))
            {
                _showPlane = true;
            }

            if (_follow)
            {
                if (GUILayout.Button("Current: Follow Camera", IMGUIUtils.EmptyLayoutOptions))
                {
                    _follow = false;
                }
            }
            else if (GUILayout.Button("Current: Not Follow Camera", IMGUIUtils.EmptyLayoutOptions))
            {
                _follow = true;
            }

            if (GUILayout.Button("Refresh Camera List", IMGUIUtils.EmptyLayoutOptions))
            {
                curAllCameras.Clear();
                curAllCameras.AddRange(Singleton<Studio.Studio>.Instance.dicObjectCtrl.Where(delegate (KeyValuePair<int, ObjectCtrlInfo> kvp)
               {
                   KeyValuePair<int, ObjectCtrlInfo> keyValuePair = kvp;
                   return keyValuePair.Value is OCICamera;
               }).Select(delegate (KeyValuePair<int, ObjectCtrlInfo> kvp)
               {
                   KeyValuePair<int, ObjectCtrlInfo> keyValuePair = kvp;
                   return keyValuePair.Value as OCICamera;
               }));
            }
            if (curAllCameras != null && curAllCameras.Count > 0)
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, new GUILayoutOption[]
               {
                    GUILayout.Width(280f),
                    GUILayout.Height(150f)
               });
                foreach (OCICamera ocicamera in curAllCameras)
                {
                    if (GUILayout.Button(ocicamera.cameraInfo.name, IMGUIUtils.EmptyLayoutOptions))
                    {
                        cam_dickey = ocicamera.objectInfo.dicKey;
                    }
                }
                GUILayout.EndScrollView();
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Refresh Plane", IMGUIUtils.EmptyLayoutOptions))
            {
                try
                {
                    cam_fov = (int)short.Parse(fovstring);
                    cam_far = (int)short.Parse(farstring);
                    cam_spd = (int)short.Parse(spdstring);
                    cam_ortho = (int)short.Parse(orthostring);
                }
                catch
                {
                    Initialize();
                }
                
                double num2 = (double)(base.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x * Mathf.Abs(base.gameObject.transform.lossyScale.x));
                double num3 = (double)(base.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.y * Mathf.Abs(base.gameObject.transform.lossyScale.y));
                double plane_ratio = num3 / num2;
                cam_width = cam_tex;
                cam_height = Math.Min(1 << 14, (int)((double)cam_tex * plane_ratio));
                RefreshCam();
            }
            GUILayout.EndVertical();
        }

        public void RefreshCam()
        {
            lock (lockObj)
            {
                Debug("Refresh Camera");
                if (m_renderTexture != null)
                {
                    planeCam.targetTexture = null;
                    m_renderer.material.mainTexture = null;
                    m_renderTexture.Release();
                }
                m_renderTexture = new RenderTexture(cam_width, cam_height, 16, RenderTextureFormat.ARGB32);
                if (cam_dickey != -1)
                {
                    ObjectCtrlInfo objectCtrlInfo;
                    Singleton<Studio.Studio>.Instance.dicObjectCtrl.TryGetValue(cam_dickey, out objectCtrlInfo);
                    if (objectCtrlInfo != null && objectCtrlInfo is OCICamera ocicamera)
                    {
                        m_camTransform = ocicamera.objectItem.transform;
                        cam_name = ocicamera.cameraInfo.name;
                        planeCam.transform.position = m_camTransform.transform.position;
                        planeCam.transform.rotation = m_camTransform.transform.rotation;
                    }
                }
                if (_showPlane)
                {
                    planeCam.cullingMask |= mask;
                }
                else
                {
                    planeCam.cullingMask &= ~mask;
                }
                planeCam.cullingMask |= 1 << 9;
                planeCam.cullingMask &= ~(1 << 14);

                planeCam.fieldOfView = (float)cam_fov;
                planeCam.farClipPlane = (float)cam_far;
                planeCam.orthographic = _ortho;
                planeCam.orthographicSize = (float)cam_ortho;
                planeCam.targetTexture = m_renderTexture;
                m_renderer.material.mainTexture = m_renderTexture;

                SetString();
            }
        }
        private void LateUpdate()
        {
            if (_follow && cam_dickey != -1 && m_camTransform != null && m_camTransform.hasChanged)
            {
                lock (lockObj)
                {
                    planeCam.transform.position = Vector3.Lerp(planeCam.transform.position, m_camTransform.transform.position, (float)cam_spd * Time.deltaTime);
                    planeCam.transform.rotation = Quaternion.Slerp(planeCam.transform.rotation, m_camTransform.transform.rotation, (float)cam_spd * Time.deltaTime);
                }
            }
        }

        private void OnDestroy()
        {
            planeCam.targetTexture = null;
            m_renderer.material.mainTexture = null;
            m_renderTexture.Release();
        }

        public override void Copy<T>(T plane)
        {
            Debug("Copy monplane begin!");
            if (plane == null)
            {
                throw new ArgumentNullException("Reference of monplane is null!");
            }
            else if (plane is MonPlane monPlane)
            {
                _showPlane = monPlane._showPlane;
                cam_dickey = monPlane.cam_dickey;
                cam_far = monPlane.cam_far;
                cam_fov = monPlane.cam_fov;
                cam_spd = monPlane.cam_spd;
                cam_width = monPlane.cam_width;
                cam_height = monPlane.cam_height;
                cam_tex = monPlane.cam_tex;
                _ortho = monPlane._ortho;
                cam_ortho = monPlane.cam_ortho;
                _follow = monPlane._follow;
                DownSampling = monPlane.DownSampling;
                RefreshCam();
            }
            else
            {
                throw new Exception("Not MonPlane!");
            }
            Debug("Copy monplane end!");
        }

        private void Initialize()
        {
            cam_far = 1500;
            cam_fov = 23;
            cam_spd = 30;
            cam_ortho = 10;
        }
        private void SetString()
        {
            farstring = cam_far.ToString();
            fovstring = cam_fov.ToString();
            spdstring = cam_spd.ToString();
            orthostring = cam_ortho.ToString();
        }

        public bool _showPlane;

        public bool _ortho;

        public Renderer m_renderer;

        public int cam_dickey;

        private string cam_name;

        public int cam_width;

        public int cam_height;

        public Vector2 scrollPosition;

        public Camera planeCam;

        private object lockObj = new object();

        public RenderTexture m_renderTexture;

        private List<OCICamera> curAllCameras;

        public Transform m_camTransform;


        public OCIItem monplane;

        public readonly int mask = 536870912;


        public bool _follow;

        public GraphicHook sss;

        private int downsampling;

        private int FarClipPlane;
        private int FieldOfView;
        private int Speed;
        private int OrthoSize;

        private string farstring;
        private string fovstring;
        private string spdstring;
        private string orthostring;

        public int cam_far
        {
            get
            {
                return FarClipPlane;
            }
            set
            {
                if (value <= 0)
                {
                    FarClipPlane = 15000;
                }
                else
                    FarClipPlane = value;
            }
        }

        public int cam_fov
        {
            get
            {
                return FieldOfView;
            }
            set
            {
                if (value <= 0)
                {
                    FieldOfView = 23;
                }
                else
                    FieldOfView = value;
            }
        }

        public int cam_spd
        {
            get
            {
                return Speed;
            }
            set
            {
                if (value <= 0)
                {
                    Speed = 30;
                }
                else
                    Speed = value;
            }
        }


        public int cam_ortho
        {
            get
            {
                return OrthoSize;
            }
            set
            {
                if (value <= 0)
                {
                    OrthoSize = 10;
                }
                else
                    OrthoSize = value;
            }
        }


        private int TexResolution;
        public int cam_tex
        {
            get
            {
                return TexResolution;
            }
            set
            {
                if (value < 128 || value > 16384)
                {
                    TexResolution = 1024;
                }
                else
                    TexResolution = value;
            }
        }

        public int DownSampling
        {
            get { return downsampling; }
            set
            {
                if (value <= 0)
                {
                    downsampling = 1;
                }
                else if ((value & (value - 1)) != 0)
                {
                    downsampling = value + 1;
                }
                else
                    downsampling = value;
            }
        }
    }
}

