using KKAPI.Utilities;
using RockyScript.File;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Video;

namespace RockyScript
{
    public class mp4plane : RockyPlane
    {
        public override void Start()
        {
            this.vp = base.gameObject.AddComponent<VideoPlayer>();
        }

        public override void DoMyWindow()
        {
            GUILayout.BeginVertical();
            bool soundOpen = this._soundOpen;
            if (soundOpen)
            {
                if (GUILayout.Button("Current:Sound On", IMGUIUtils.EmptyLayoutOptions))
                {
                    this._soundOpen = false;
                }
            }
            else
            {
                if (GUILayout.Button("Current:Sound off", IMGUIUtils.EmptyLayoutOptions))
                {
                    this._soundOpen = true;
                }
            }
            if (GUILayout.Button("Load mp4", IMGUIUtils.EmptyLayoutOptions))
            {
                this.Selectmp4();
                this.LoadMp4();
            }
            if (this.vp.isPlaying)
            {
                if (GUILayout.Button("Current:movie is playing", IMGUIUtils.EmptyLayoutOptions))
                {
                    this.vp.Pause();
                }
            }
            else
            {
                if (GUILayout.Button("Current:movie is paused",IMGUIUtils.EmptyLayoutOptions ))
                {
                    this.vp.Play();
                }
                if (this.vp.frameCount > 0UL)
                {
                    GUILayout.Box(string.Format("Frame:{0:D} / {1:D}", this.vp.frame, this.vp.frameCount),IMGUIUtils.EmptyLayoutOptions );
                    this.sliderValue = GUILayout.HorizontalSlider(this.sliderValue, 0f, this.maxSliderValue, IMGUIUtils.EmptyLayoutOptions);
                    this.vp.frame = (long)(this.vp.frameCount * this.sliderValue);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(string.Format("PlaySpeed:{0:G3}", (float)this.playspeed / 1000f),IMGUIUtils.EmptyLayoutOptions );
                    this.spdstring = GUILayout.TextField(this.spdstring, 4,IMGUIUtils.EmptyLayoutOptions );
                    if (GUILayout.Button("OK" ))
                    {
                        try
                        {
                            this.playspeed = (int)short.Parse(this.spdstring);
                        }
                        catch
                        {
                            this.playspeed = 1000;
                            this.spdstring = "1000";
                        }
                        if (this.playspeed < 0)
                        {
                            this.playspeed = 1000;
                            this.spdstring = "1000";
                        }
                        this.vp.playbackSpeed = (float)this.playspeed / 1000f;
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private void Selectmp4()
        {
            OpenFileName openFileName = new OpenFileName();
            openFileName.structSize = Marshal.SizeOf<OpenFileName>(openFileName);
            openFileName.filter = "All\0*.*\0";
            openFileName.file = new string(new char[256]);
            openFileName.maxFile = openFileName.file.Length;
            openFileName.fileTitle = new string(new char[64]);
            openFileName.maxFileTitle = openFileName.fileTitle.Length;
            bool flag = this.mp4path.Length == 0;
            if (flag)
            {
                openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');
            }
            else
            {
                openFileName.initialDir = this.mp4path.Replace('/', '\\');
            }
            openFileName.title = "支持mp4、avi、asf、wmv、mpg等内部格式为H.264的文件";
            openFileName.flags = 530440;
            bool ofn = LocalDialog.GetOFN(openFileName);
            if (ofn)
            {
                this.mp4path = openFileName.file;
            }
        }

        public void LoadMp4()
        {
            if (this.mp4path.Length > 0)
            {
                this.vp.url = this.mp4path;
                this.vp.SetDirectAudioMute(0, !this._soundOpen);
                this.vp.isLooping = true;
                this.vp.aspectRatio = VideoAspectRatio.FitInside;
                this.vp.renderMode = VideoRenderMode.MaterialOverride;
                this.vp.targetMaterialRenderer = base.GetComponent<Renderer>();
                this.vp.targetMaterialProperty = "_MainTex";
                this.vp.Prepare();
                this.vp.Play();
            }
        }

        public override void Copy<T>(T plane)
        {
            Debug("Copy mp4plane begin!");
            if (plane == null)
            {
                throw new ArgumentNullException("Reference of mp4plane is null!");
            }
            else if (plane is mp4plane mp4Plane)
            {
                this._soundOpen = mp4Plane._soundOpen;
                this.mp4path = mp4Plane.mp4path;
                this.LoadMp4();
            }
            else
            {
                throw new Exception("Not mp4plane!");
            }
            Debug("Copy mp4plane end!");
        }

        public bool _soundOpen = true;

        public int playspeed = 1000;

        private string spdstring = "1000";

        public string mp4path;

        public VideoPlayer vp;

        private float sliderValue = 0f;

        private float maxSliderValue = 1f;

    }
}
