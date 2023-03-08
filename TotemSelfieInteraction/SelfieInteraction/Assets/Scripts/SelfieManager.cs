using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

public class SelfieManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button _takeScreenshotButton;
    [SerializeField] private RectTransform _objToScreenshot, shaderBar;
    [SerializeField] private GameObject shaderMaterial, objFakeTexture;
    [SerializeField] private GameObject ScrollView, photoTimerAnimation, objPhotoSimulation;
    [SerializeField] private GameObject saveImageButton, deleteImageButton;
    [NonSerialized] public WebCamTexture webCam;
    private Texture2D textureImage, fakeTextureImage;
    private Material shaderTexture;
    private GameUIManager gameUIManager;

    [Header("Shader Values")]
    [SerializeField] private Slider brightnees;
    [SerializeField] private Slider temperature, contrast, saturation;
    [SerializeField] private Scrollbar shaderScrollbar;
    private readonly float defoultValue_B = 0.8f, defoultValue_T = 0.1f, defoultValue_C = 1.5f, defoultValue_S = 1.9f;

    [Header("Camera Resolution")]
    [SerializeField] private int widhtResolution = 1920;
    [SerializeField] private int  heightResolution = 1080;
    private readonly int defoulWidhtResolution = 640;
    private readonly int  defoultHeightResolution = 480;

    [Header("Render Image Size")]
    [SerializeField] private float widhtImageSize = 1920;
    [SerializeField] private float heightImageSize = 1080;
    private readonly float defoultWidhtImageSize = 1920;
    private readonly float defoultHeightImageSize = 1380;

    [Header("Take Photo")]
    [SerializeField] private float timetoTakePhoto = 8f;
    private int indexImage = 00;
    private bool saveImage, webCamStop, resetCamera, isCameraPaused;

    private void Awake()
    {
        gameUIManager = GetComponent<GameUIManager>();
    }
    void Start()
    {
        gameUIManager.SetActiveUiButtons(false, 0);
        webCam = new();
        webCam.requestedFPS = 30;
        shaderTexture = shaderMaterial.GetComponent<RawImage>().material;
        shaderTexture.SetTexture("_MainTexture", webCam);
        CameraManager();
        _takeScreenshotButton.onClick.AddListener(TakeScreenshotAndSaveButton);
    }
    void Update()
    {
        shaderTexture.SetFloat("_Light", brightnees.value);
        shaderTexture.SetFloat("_Temperature", temperature.value);
        shaderTexture.SetFloat("_Contrast_Intensity", contrast.value);
        shaderTexture.SetFloat("_Saturation_Intensity", saturation.value);
    }
    public IEnumerator TakeScreenShotAndSave()
    {
        isCameraPaused = false;
        photoTimerAnimation.SetActive(true);

        yield return new WaitForSeconds(timetoTakePhoto);
        
        yield return CreateFakeImageTexture();
        
        yield return WebCamManagerUpdate(widhtResolution, heightResolution, widhtImageSize, heightImageSize);
        objPhotoSimulation.SetActive(true);
        objFakeTexture.SetActive(false);
        photoTimerAnimation.SetActive(false);
        gameUIManager.SetActiveUiButtons(true, 0);
        ScrollView.SetActive(true);
        
        yield return new WaitUntil(() => this.saveImage);

        yield return new WaitForEndOfFrame();
        Vector3[] corners = new Vector3[4];
        _objToScreenshot.GetWorldCorners(corners);

        int width = (int)corners[3].x - (int)corners[0].x;
        int height = (int)corners[1].y - (int)corners[0].y;
        var startX = corners[0].x;
        var startY = corners[0].y;
        textureImage = new Texture2D(width, height, TextureFormat.RGB48, false);
        textureImage.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
        textureImage.Apply();
        
        indexImage++;
        byte[] byteArray = textureImage.EncodeToPNG();
#if UNITY_EDITOR
        File.WriteAllBytes(Application.dataPath + $"/Resources/ScreenShotImage{indexImage}.png", byteArray);
#else
        string pathDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string directory = pathDocuments + $"/GitHub/TotemSelfieInteractionRep/TotemSelfieInteraction/build/Assets/Resources/ScreenShotImage{indexImage}.png";
        File.WriteAllBytes(directory, byteArray);
#endif
        Destroy(textureImage);
        ScrollView.SetActive(false);
        brightnees.value = defoultValue_B; temperature.value = defoultValue_T; contrast.value = defoultValue_C; saturation.value = defoultValue_S;
       
        yield return WebCamManagerUpdate(defoulWidhtResolution, defoultHeightResolution, defoultWidhtImageSize, defoultHeightImageSize);
        resetCamera = true;
        CameraManager();
        this.saveImage = false;
        
        yield return new WaitUntil(() => !this.saveImage && !webCam.isPlaying);
        Debug.Log("foto salva");
        SceneManager.LoadSceneAsync("Quebra-Cabeca");
    }
    private IEnumerator CreateFakeImageTexture()
    {
        yield return new WaitForEndOfFrame();
        Vector3[] fakeCorners = new Vector3[4];
        _objToScreenshot.GetWorldCorners(fakeCorners);

        int fakeWidth = (int)fakeCorners[3].x - (int)fakeCorners[0].x;
        int fakeHeight = (int)fakeCorners[1].y - (int)fakeCorners[0].y;
        var fakeStartX = fakeCorners[0].x;
        var fakeStartY = fakeCorners[0].y;
        fakeTextureImage = new Texture2D(fakeWidth, fakeHeight, TextureFormat.RGB48, false);
        fakeTextureImage.ReadPixels(new Rect(fakeStartX, fakeStartY, fakeWidth, fakeHeight), 0, 0);
        fakeTextureImage.Apply();

        byte[] fakeByteArray = fakeTextureImage.EncodeToPNG();
        Texture2D result = new(fakeWidth, fakeHeight, TextureFormat.RGB48, false);
        result.filterMode = FilterMode.Point;
        result.LoadImage(fakeByteArray);
        
        objFakeTexture.GetComponent<RawImage>().texture = result;
        objFakeTexture.SetActive(true);
        Destroy(fakeTextureImage);
    }
    public void SetResolution_ImageSize(int Width, int Height, float SizeX, float SizeY)
    {
        webCam.requestedWidth = Width;
        webCam.requestedHeight = Height;

        RectTransform ImageRT = _objToScreenshot;
        RectTransform MaterialRT = shaderMaterial.GetComponent<RectTransform>();
        ImageRT.sizeDelta = new Vector2(SizeX, SizeY);
        MaterialRT.sizeDelta = new Vector2(SizeX, SizeY);
        ImageRT.ForceUpdateRectTransforms();
        MaterialRT.ForceUpdateRectTransforms();
    }
    public void CameraManager()
    {
        if (!resetCamera)
        {
            if (!webCam.isPlaying)
            {
                webCam.Play();
            }
            else
            {
                if (!webCamStop)
                {
                    webCam.Stop();
                    webCamStop = true;
                }
                else
                {
                    webCam.Pause();
                    webCamStop = false;
                    resetCamera = true;
                }
            }
        }
        else
        {
            webCam.Stop();
            resetCamera = false;
        }
    }
    private IEnumerator WebCamManagerUpdate(int camResWidth, int camResHeight, float imageSizeX, float imageSizeY)
    {
        CameraManager();
        SetResolution_ImageSize(camResWidth, camResHeight, imageSizeX, imageSizeY);
        
        yield return new WaitForSeconds(1f);
        CameraManager();
        
        yield return new WaitForSeconds(1.2f);
        if (!isCameraPaused)
        {
            CameraManager();
            isCameraPaused = true;
        }
    }
    private void TakeScreenshotAndSaveButton()
    {
        StartCoroutine(TakeScreenShotAndSave());
        gameUIManager.SetActiveUiButtons(false);
    }
    public void OnClickSaveDeleteButton(bool saveImage)
    {
        shaderScrollbar.value = 0;
        gameUIManager.SetActiveUiButtons(false);
        objPhotoSimulation.SetActive(false);
        if (saveImage)
        {
            this.saveImage = true;
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(WebCamManagerUpdate(defoulWidhtResolution, defoultHeightResolution, defoultWidhtImageSize, defoultHeightImageSize));
            brightnees.value = defoultValue_B; temperature.value = defoultValue_T; contrast.value = defoultValue_C; saturation.value = defoultValue_S;
            ScrollView.SetActive(false);
            gameUIManager.SetActiveUiButtons(true, new List<int>(){1,2});
            Debug.Log("foto apagada");
        }
    }
}