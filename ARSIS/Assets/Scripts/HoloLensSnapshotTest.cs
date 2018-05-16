using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.WSA.WebCam;
using UnityEngine.XR.WSA.Input;
using UnityEngine.UI;

public class HoloLensSnapshotTest : MonoBehaviour
{
    public static HoloLensSnapshotTest S; 

    public RawImage m_RawImageSmall;
    public RawImage m_RawImageBig; 

    GestureRecognizer m_GestureRecognizer;
    GameObject m_Canvas = null;
    Renderer m_CanvasRenderer = null;
    PhotoCapture m_PhotoCaptureObj;
    CameraParameters m_CameraParameters;
    bool m_CapturingPhoto = false;
    Texture2D m_Texture = null;

    void Start()
    {
        S = this; 
        Initialize();
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

        Debug.Log("Camera Ready!");
    }

    public void TakePhoto()
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
        Matrix4x4 cameraToWorldMatrix;
        photoCaptureFrame.TryGetCameraToWorldMatrix(out cameraToWorldMatrix);
        Matrix4x4 worldToCameraMatrix = cameraToWorldMatrix.inverse;

        Matrix4x4 projectionMatrix;
        photoCaptureFrame.TryGetProjectionMatrix(out projectionMatrix);

        photoCaptureFrame.UploadImageDataToTexture(m_Texture);
        m_Texture.wrapMode = TextureWrapMode.Clamp;

        SetImage(m_Texture); 
        ServerConnect.S.sendPicture(m_Texture);
        // m_RawImage.SetNativeSize();
        Debug.Log("Took picture!");
        m_CapturingPhoto = false;

    }

    public void SetImage(Texture2D text)
    {
        m_RawImageBig.texture = text;
        m_RawImageSmall.texture = text;
    }

    public void ToggleImage()
    {
        if (!m_RawImageBig.gameObject.activeInHierarchy)
        {
            m_RawImageBig.gameObject.SetActive(true);
            m_RawImageSmall.gameObject.SetActive(false); 
        } else
        {
            m_RawImageBig.gameObject.SetActive(false);
            m_RawImageSmall.gameObject.SetActive(true);
        }
    }

    public void ClearImage()
    {
        m_RawImageBig.gameObject.SetActive(false);
        m_RawImageSmall.gameObject.SetActive(false); 
    }

    // For Testing Only 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TakePhoto();
        }
    }

    // For testing only 
    void SetupGestureRecognizer()
    {
        m_GestureRecognizer = new GestureRecognizer();
        m_GestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        m_GestureRecognizer.TappedEvent += OnTappedEvent;
        m_GestureRecognizer.StartCapturingGestures();

        m_CapturingPhoto = false;
    }

    void OnTappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        TakePhoto();
    }
}
