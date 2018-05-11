using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.WSA.WebCam;
using UnityEngine.XR.WSA.Input;

public class HoloLensSnapshotTest : MonoBehaviour
{
    GestureRecognizer m_GestureRecognizer;
    GameObject m_Canvas = null;
    Renderer m_CanvasRenderer = null;
    PhotoCapture m_PhotoCaptureObj;
    CameraParameters m_CameraParameters;
    bool m_CapturingPhoto = false;
    Texture2D m_Texture = null;

    void Start()
    {
        Initialize();
    }

    void SetupGestureRecognizer()
    {
        m_GestureRecognizer = new GestureRecognizer();
        m_GestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        m_GestureRecognizer.TappedEvent += OnTappedEvent;
        m_GestureRecognizer.StartCapturingGestures();

        m_CapturingPhoto = false;
    }

    void Initialize()
    {
        Debug.Log("Initializing...");
        List<Resolution> resolutions = new List<Resolution>(PhotoCapture.SupportedResolutions);
        Resolution selectedResolution = resolutions[0];

        m_CameraParameters = new CameraParameters(WebCamMode.PhotoMode);
        m_CameraParameters.cameraResolutionWidth = selectedResolution.width;
        m_CameraParameters.cameraResolutionHeight = selectedResolution.height;
        m_CameraParameters.hologramOpacity = 0.0f;
        m_CameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

        m_Texture = new Texture2D(selectedResolution.width, selectedResolution.height, TextureFormat.BGRA32, false);

        PhotoCapture.CreateAsync(false, OnCreatedPhotoCaptureObject);
    }

    void OnCreatedPhotoCaptureObject(PhotoCapture captureObject)
    {
        m_PhotoCaptureObj = captureObject;
        m_PhotoCaptureObj.StartPhotoModeAsync(m_CameraParameters, OnStartPhotoMode);
    }

    void OnStartPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        SetupGestureRecognizer();

        Debug.Log("Ready!");
        Debug.Log("Air Tap to take a picture.");
    }

    void OnTappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (m_CapturingPhoto)
        {
            return;
        }

        m_CapturingPhoto = true;
        Debug.Log("Taking picture...");
        m_PhotoCaptureObj.TakePhotoAsync(OnPhotoCaptured);
    }

    void OnPhotoCaptured(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (m_Canvas == null)
        {
            m_Canvas = GameObject.CreatePrimitive(PrimitiveType.Quad);
            m_Canvas.name = "PhotoCaptureCanvas";
            m_CanvasRenderer = m_Canvas.GetComponent<Renderer>() as Renderer;
            m_CanvasRenderer.material = new Material(Shader.Find("AR/HolographicImageBlend"));
        }

        Matrix4x4 cameraToWorldMatrix;
        photoCaptureFrame.TryGetCameraToWorldMatrix(out cameraToWorldMatrix);
        Matrix4x4 worldToCameraMatrix = cameraToWorldMatrix.inverse;

        Matrix4x4 projectionMatrix;
        photoCaptureFrame.TryGetProjectionMatrix(out projectionMatrix);

        photoCaptureFrame.UploadImageDataToTexture(m_Texture);
        m_Texture.wrapMode = TextureWrapMode.Clamp;

        m_CanvasRenderer.sharedMaterial.SetTexture("_MainTex", m_Texture);
        m_CanvasRenderer.sharedMaterial.SetMatrix("_WorldToCameraMatrix", worldToCameraMatrix);
        m_CanvasRenderer.sharedMaterial.SetMatrix("_CameraProjectionMatrix", projectionMatrix);
        m_CanvasRenderer.sharedMaterial.SetFloat("_VignetteScale", 1.0f);

        // Position the canvas object slightly in front
        // of the real world web camera.
        Vector3 position = cameraToWorldMatrix.GetColumn(3) - cameraToWorldMatrix.GetColumn(2);

        // Rotate the canvas object so that it faces the user.
        Quaternion rotation = Quaternion.LookRotation(-cameraToWorldMatrix.GetColumn(2), cameraToWorldMatrix.GetColumn(1));

        m_Canvas.transform.position = position;
        m_Canvas.transform.rotation = rotation;

        Debug.Log("Took picture!");
        m_CapturingPhoto = false;
    }
}