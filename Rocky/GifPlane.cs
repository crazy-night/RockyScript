using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using UnityEngine;

namespace RockyScript
{
    // Token: 0x02000002 RID: 2
    public class GifPlane : MonoBehaviour
    {

        // Token: 0x06000003 RID: 3 RVA: 0x00002394 File Offset: 0x00000594
        public void DoMyWindow()
        {
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("fps:", Array.Empty<GUILayoutOption>());
            this.fpsstring = GUILayout.TextField(this.fpsstring, 2, Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            bool flag = GUILayout.Button("Load Gif", Array.Empty<GUILayoutOption>());
            if (flag)
            {
                try
                {
                    this.fps = short.Parse(this.fpsstring);
                }
                catch
                {
                    this.fps = 15;
                }
                bool flag2 = this.fps < 0;
                if (flag2)
                {
                    this.fps = 0;
                }
                this.SelectGif();
                this.LoadGif();
            }
            GUILayout.EndVertical();
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002458 File Offset: 0x00000658
        private void SelectGif()
        {
            OpenFileName openFileName = new OpenFileName();
            openFileName.structSize = Marshal.SizeOf<OpenFileName>(openFileName);
            openFileName.filter = "All\0*.*\0";
            openFileName.file = new string(new char[256]);
            openFileName.maxFile = openFileName.file.Length;
            openFileName.fileTitle = new string(new char[64]);
            openFileName.maxFileTitle = openFileName.fileTitle.Length;
            openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');
            openFileName.title = "选择Gif图片";
            openFileName.flags = 530440;
            bool ofn = LocalDialog.GetOFN(openFileName);
            if (ofn)
            {
                this.gifpath = openFileName.file;
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002510 File Offset: 0x00000710
        public void LoadGif()
        {
            List<Texture2D> list = new List<Texture2D>();
            try
            {
                Image image = Image.FromFile(this.gifpath);
                FrameDimension dimension = new FrameDimension(image.FrameDimensionsList[0]);
                int frameCount = image.GetFrameCount(dimension);
                int tickCount = Environment.TickCount;
                for (int i = 0; i < frameCount; i++)
                {
                    image.SelectActiveFrame(dimension, i);
                    Bitmap bitmap = new Bitmap(image.Width, image.Height);
                    System.Drawing.Graphics.FromImage(bitmap).DrawImage(image, Point.Empty);
                    Texture2D texture2D = new Texture2D(bitmap.Width, bitmap.Height, TextureFormat.ARGB32, true);
                    for (int j = 0; j < bitmap.Width; j++)
                    {
                        for (int k = 0; k < bitmap.Height; k++)
                        {
                            System.Drawing.Color pixel = bitmap.GetPixel(j, k);
                            texture2D.SetPixel(j, bitmap.Height - 1 - k, new Color32(pixel.R, pixel.G, pixel.B, pixel.A));
                        }
                    }
                    texture2D.Apply();
                    list.Add(texture2D);
                    int tickCount2 = Environment.TickCount;
                }
            }
            catch
            {
            }
            object obj = GifPlane.lockObj;
            lock (obj)
            {
                bool flag2 = list.Count > 0;
                if (flag2)
                {
                    this.gifFrames = list;
                }
            }
        }

        // Token: 0x06000006 RID: 6 RVA: 0x000026E0 File Offset: 0x000008E0
        private void Update()
        {
            int num = -1;
            object obj = GifPlane.lockObj;
            lock (obj)
            {
                bool flag3 = this.gifFrames.Count > 0;
                if (flag3)
                {
                    this.time += Time.deltaTime;
                    num = (int)(this.time * (float)this.fps) % this.gifFrames.Count;
                }
            }
            bool flag4 = num >= 0;
            if (flag4)
            {
                base.GetComponent<Renderer>().material.mainTexture = this.gifFrames[num];
            }

        }


        // Token: 0x04000002 RID: 2
        public short fps = 15;

        // Token: 0x04000003 RID: 3
        private string fpsstring = "15";

        // Token: 0x04000004 RID: 4
        public string gifpath;

        // Token: 0x04000005 RID: 5
        private static object lockObj = new object();


        // Token: 0x04000007 RID: 7
        private List<Texture2D> gifFrames = new List<Texture2D>();

        // Token: 0x04000008 RID: 8
        private float time;
    }
}
