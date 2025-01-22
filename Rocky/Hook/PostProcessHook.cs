using HarmonyLib;
using RockyScript.Core;
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace RockyScript.Hook
{
    public class PostProcessHook
    {
        public static void Debug(string message)
        {
            PlaneEditor.Debug(message);
        }
        public static Camera MainCamera
        {
            get
            {
                if (_camera == null && Camera.allCameras.Length != 0)
                {
                    foreach (Camera camera in Camera.allCameras)
                    {
                        Debug("Checking Camera: " + camera.name);
                        if (camera.name.Equals("VRGIN_Camera (eye)"))
                        {
                            _camera = camera;
                        }
                    }
                    if (_camera == null)
                    {
                        _camera = Camera.main;
                    }
                    if (_camera != null)
                    {
                        Debug("Using Camera: " + _camera.name);
                    }
                }
                return _camera;
            }
        }

        public static PostProcessResources Resources
        {
            get
            {
                if (_resources == null && MainPostProcessLayer != null)
                {
                    Traverse PPL = Traverse.Create(MainPostProcessLayer);
                    Traverse m_Resources = PPL.Field("m_Resources");
                    if (!m_Resources.FieldExists())
                    {
                        throw new InvalidOperationException("_resource not found in Post Process MainPostProcessLayer of Camera");
                    }
                    _resources = (PostProcessResources)m_Resources.GetValue();
                    Debug("Get PostProcessResources Successfully!");

                }
                return _resources;
            }
        }

        public static PostProcessLayer MainPostProcessLayer
        {
            get
            {
                if (_postProcessLayer == null)
                {
                    _postProcessLayer = MainCamera.GetComponent<PostProcessLayer>();
                    Debug("Get PostProcessLayer Successfully!");
                }
                return _postProcessLayer;
            }
        }

        public static LayerMask volumeLayer
        {
            get
            {
                if (_volumeLayer == 0)
                {
                    _volumeLayer = MainPostProcessLayer == null ? (LayerMask)(0) : MainPostProcessLayer.volumeLayer;
                    Debug("Get VolumeLayer Successfully!");
                }
                return _volumeLayer;
            }
        }

        private static Camera _camera;

        private static PostProcessResources _resources;

        private static PostProcessLayer _postProcessLayer;

        private static LayerMask _volumeLayer;
    }
}
