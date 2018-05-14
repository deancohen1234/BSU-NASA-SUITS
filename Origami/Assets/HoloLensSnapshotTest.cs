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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            TakePhoto();
        }
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
        TakePhoto();
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
        /*if (m_Canvas == null)
        {
            m_Canvas = GameObject.CreatePrimitive(PrimitiveType.Quad);
            m_Canvas.name = "PhotoCaptureCanvas";
            m_CanvasRenderer = m_Canvas.GetComponent<Renderer>() as Renderer;
            m_CanvasRenderer.material = new Material(Shader.Find("AR/HolographicImageBlend"));
        }*/

        Matrix4x4 cameraToWorldMatrix;
        photoCaptureFrame.TryGetCameraToWorldMatrix(out cameraToWorldMatrix);
        Matrix4x4 worldToCameraMatrix = cameraToWorldMatrix.inverse;

        Matrix4x4 projectionMatrix;
        photoCaptureFrame.TryGetProjectionMatrix(out projectionMatrix);

        photoCaptureFrame.UploadImageDataToTexture(m_Texture);
        m_Texture.wrapMode = TextureWrapMode.Clamp;

        /*
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
        */

        m_RawImageBig.texture = m_Texture;
        m_RawImageSmall.texture = m_Texture; 
       // m_RawImage.SetNativeSize();
        Debug.Log("Took picture!");
        m_CapturingPhoto = false;
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
}

/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.WSA.WebCam;
using UnityEngine.XR.WSA.Input;
using UnityEngine.UI;

public class HoloLensSnapshotTest : MonoBehaviour
{
    GestureRecognizer m_GestureRecognizer;
    GameObject m_Canvas = null;
    Renderer m_CanvasRenderer = null;
    PhotoCapture m_PhotoCaptureObj;
    CameraParameters m_CameraParameters;
    bool m_CapturingPhoto = false;
    Texture2D m_Texture = null;

    public RawImage m_RawImage;

    public static HoloLensSnapshotTest m_HoloLensSnapshot;

    void Start()
    {
        m_HoloLensSnapshot = this;
        Initialize();
    }

    void SetupGestureRecognizer()
    {
        m_GestureRecognizer = new GestureRecognizer();
        m_GestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        m_GestureRecognizer.TappedEvent += OnTappedEvent2;
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

        PhotoCapture.CreateAsync(true, OnCreatedPhotoCaptureObject);
        
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

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        Debug.Log("Closing camera");
        // Shutdown the photo capture resource
        m_PhotoCaptureObj.Dispose();
        m_PhotoCaptureObj = null;
        Debug.Log("Camera closed");
    }

    void OnTappedEvent2(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        Debug.Log("Capturing Photo: " + m_CapturingPhoto);
        if (m_CapturingPhoto)
        {
            return;
        }

        m_CapturingPhoto = true;
        Debug.Log("Taking picture...");
        m_PhotoCaptureObj.TakePhotoAsync(OnPhotoCaptured);
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
        if (m_Canvas == null)
        {
            m_Canvas = GameObject.CreatePrimitive(PrimitiveType.Quad);
            m_Canvas.name = "PhotoCaptureCanvas";
            m_CanvasRenderer = m_Canvas.GetComponent<Renderer>() as Renderer;
            m_CanvasRenderer.material = new Material(Shader.Find("Unlit/Texture"));
        }

        Matrix4x4 cameraToWorldMatrix;
        photoCaptureFrame.TryGetCameraToWorldMatrix(out cameraToWorldMatrix);
        Matrix4x4 worldToCameraMatrix = cameraToWorldMatrix.inverse;

        Matrix4x4 projectionMatrix;
        photoCaptureFrame.TryGetProjectionMatrix(out projectionMatrix);

        photoCaptureFrame.UploadImageDataToTexture(m_Texture);
        m_Texture.wrapMode = TextureWrapMode.Clamp;

        m_CanvasRenderer.sharedMaterial.SetTexture("_MainTex", m_Texture);

        m_RawImage.texture = m_Texture;

        // Position the canvas object slightly in front
        // of the real world web camera.
        Vector3 position = cameraToWorldMatrix.GetColumn(3) - cameraToWorldMatrix.GetColumn(2);

        // Rotate the canvas object so that it faces the user.
        Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.position);

        m_Canvas.transform.position = position;
        //m_Canvas.transform.rotation = rotation;

        MenuController menuController = FindObjectOfType<MenuController>();
        menuController.ChangeMenuNonBlender(m_Canvas);

        Debug.Log("Took picture!");
        m_CapturingPhoto = false;

       // m_PhotoCaptureObj.StopPhotoModeAsync(OnStoppedPhotoMode);

        //ServerConnect.S.sendPicture(m_Texture); 
    }
}*/
