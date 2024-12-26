using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Video;

namespace RockyScript
{
    // Token: 0x0200000F RID: 15
    public class mp4plane : MonoBehaviour
    {
        // Token: 0x06000067 RID: 103 RVA: 0x000022FD File Offset: 0x000004FD
        private void Start()
        {
            this.vp = base.gameObject.AddComponent<VideoPlayer>();
        }

        // Token: 0x06000068 RID: 104 RVA: 0x000045DC File Offset: 0x000027DC

        // Token: 0x06000069 RID: 105 RVA: 0x00004634 File Offset: 0x00002834
        public void DoMyWindow()
        {
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            bool soundOpen = this._soundOpen;
            if (soundOpen)
            {
                bool flag = GUILayout.Button("Current:Sound On", Array.Empty<GUILayoutOption>());
                if (flag)
                {
                    this._soundOpen = false;
                }
            }
            else
            {
                bool flag2 = GUILayout.Button("Current:Sound off", Array.Empty<GUILayoutOption>());
                if (flag2)
                {
                    this._soundOpen = true;
                }
            }
            bool flag3 = GUILayout.Button("Load mp4", Array.Empty<GUILayoutOption>());
            if (flag3)
            {
                this.Selectmp4();
                this.Loadmp4();
            }
            bool isPlaying = this.vp.isPlaying;
            if (isPlaying)
            {
                bool flag5 = GUILayout.Button("Current:movie is playing", Array.Empty<GUILayoutOption>());
                if (flag5)
                {
                    this.vp.Pause();
                }
            }
            else
            {
                bool flag6 = GUILayout.Button("Current:movie is paused", Array.Empty<GUILayoutOption>());
                if (flag6)
                {
                    this.vp.Play();
                }
                bool flag7 = this.vp.frameCount > 0UL;
                if (flag7)
                {
                    GUILayout.Box(string.Format("Frame:{0:D} / {1:D}", this.vp.frame, this.vp.frameCount), Array.Empty<GUILayoutOption>());
                    this.sliderValue = GUILayout.HorizontalSlider(this.sliderValue, 0f, this.maxSliderValue, Array.Empty<GUILayoutOption>());
                    this.vp.frame = (long)(this.vp.frameCount * this.sliderValue);
                    GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                    GUILayout.Label(string.Format("PlaySpeed:{0:G3}", (float)this.playspeed / 1000f), Array.Empty<GUILayoutOption>());
                    this.spdstring = GUILayout.TextField(this.spdstring, 4, Array.Empty<GUILayoutOption>());
                    bool flag8 = GUILayout.Button("OK", Array.Empty<GUILayoutOption>());
                    if (flag8)
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
                        bool flag9 = this.playspeed < 0;
                        if (flag9)
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

        // Token: 0x0600006A RID: 106 RVA: 0x000048BC File Offset: 0x00002ABC
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

        // Token: 0x0600006B RID: 107 RVA: 0x000049A0 File Offset: 0x00002BA0
        public void Loadmp4()
        {
            bool flag = this.mp4path.Length > 0;
            if (flag)
            {
                this.vp.url = this.mp4path;
                this.vp.SetDirectAudioMute(0, !this._soundOpen);
                this.vp.isLooping = true;
                this.vp.renderMode = VideoRenderMode.MaterialOverride;
                this.vp.targetMaterialRenderer = base.GetComponent<Renderer>();
                this.vp.targetMaterialProperty = "_MainTex";
                this.vp.Prepare();
                this.vp.Play();
            }
        }


        // Token: 0x04000067 RID: 103
        public bool _soundOpen = true;

        // Token: 0x04000068 RID: 104
        public int playspeed = 1000;

        // Token: 0x04000069 RID: 105
        private string spdstring = "1000";

        // Token: 0x0400006A RID: 106
        public string mp4path;

        // Token: 0x0400006B RID: 107
        public VideoPlayer vp;

        // Token: 0x0400006C RID: 108
        private float sliderValue = 0f;

        // Token: 0x0400006D RID: 109
        private float maxSliderValue = 1f;

    }
}
