using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyAR
{
    public class QRCodeReader : MonoBehaviour
    {
        public event OnDetectQRCode detectQRCode = delegate {};

        public delegate void OnDetectQRCode(string text);
        //private CameraDeviceBaseBehaviour _cameraDevice;

        void Awake()
        {
           // _cameraDevice = GetComponentInChildren<CameraDeviceBaseBehaviour>();
            var EasyARBehaviour = GetComponent<EasyARBehaviour>();
            EasyARBehaviour.Initialize();
            foreach (var behaviour in ARBuilder.Instance.ARCameraBehaviours)
            {
                behaviour.TextMessage += OnTextMessage;
            }
        }

        private void OnTextMessage(ARCameraBaseBehaviour arcameraBehaviour, string text)
        {
            
            Debug.Log(text);
            detectQRCode(text);
            /*
            if (!GetComponent<AssetManager>().IsDownloadedContent(text))
            {
                _cameraDevice.StopCapture();
                GetComponent<DialogManager>().ShowConfirmDownloadDialog(text);
            }
            */
        }
    }
}