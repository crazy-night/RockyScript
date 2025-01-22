using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using UnityEngine;
using KKAPI.Utilities;
using RockyScript.File;
using System;

namespace RockyScript
{
    public class GifPlane : RockyPlane
    {

        public override void Awake()
        {
            this.material = base.GetComponent<Renderer>().material;
            this.gameObject.layer = 12;
        }
        public override void DoMyWindow()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("fps:" , IMGUIUtils.EmptyLayoutOptions);
            this.fpsstring = GUILayout.TextField(this.fpsstring, 2, IMGUIUtils.EmptyLayoutOptions);
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Load Gif", IMGUIUtils.EmptyLayoutOptions))
            {
                try
                {
                    this.fps = short.Parse(this.fpsstring);
                }
                catch
                {
                    this.fps = 15;
                };
                if (this.fps < 0)
                {
                    this.fps = 0;
                }
                this.SelectGif();
                this.LoadGif();
            }
            GUILayout.EndVertical();
        }

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
            if (LocalDialog.GetOFN(openFileName))
            {
                this.gifpath = openFileName.file;
            }
        }

        public void LoadGif()
        {
            List<Texture2D> list = new List<Texture2D>();
            try
            {
                Image image = Image.FromFile(this.gifpath);
                FrameDimension dimension = new FrameDimension(image.FrameDimensionsList[0]);
                int frameCount = image.GetFrameCount(dimension);
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
                }
            }
            catch
            {
            }
            lock (this.lockObj)
            {
                if (list.Count > 0)
                {
                    this.gifFrames = list;
                }
                
            }
        }

        private void Update()
        {
            int num = -2;
            lock (this.lockObj)
            {
                if (this.gifFrames.Count == 1)
                {
                    num = 1;
                }
                else if (this.gifFrames.Count > 0)
                {
                    this.time += Time.deltaTime;
                    num = (int)(this.time * (float)this.fps) % this.gifFrames.Count;
                }
            }
            if (num >= 0 && num != index && material != null)
            {
                this.material.mainTexture = this.gifFrames[num];
                index = num;
            }

        }

        

        public override void Copy<T>(T plane)
        {
            Debug("Copy gifplane begin!");
            if (plane == null)
            {
                throw new ArgumentNullException("Reference of monplane is null!");
            }
            else if (plane is GifPlane gifPlane)
            {
                this.fps = gifPlane.fps;
                this.gifpath = gifPlane.gifpath;
                this.LoadGif();
            }
            else
            {
                throw new Exception("Not GifPlane!");
            }
            Debug("Copy gifplane end!");
        }


        public short fps = 15;

        private string fpsstring = "15";

        public string gifpath;

        private object lockObj = new object();


        private List<Texture2D> gifFrames = new List<Texture2D>();

        private float time;

        private Material material;

        private int index = -1;

    }
}
