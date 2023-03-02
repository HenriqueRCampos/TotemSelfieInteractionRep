using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

public class SelfieManager : MonoBehaviour 
{
    [Header("References")]
    [SerializeField] private RectTransform _objToScreenshot;
    [SerializeField] private GameObject shaderMaterial;
    [SerializeField] private Button _takeScreenshotButton;
    [SerializeField] private GameObject uiSaveButton, uiDeleteButton, ScrollView, photoTimerAnimation;
    [SerializeField] private RectTransform shaderBar;

    [Header("Shader Values Control")]
    [SerializeField] private Slider brightnees;
    [SerializeField] private Slider temperature, contrast, saturation;

    [HideInInspector]public static WebCamTexture webCam;
    private float defoultValue_B = 0.8f, defoultValue_T = 0.1f, defoultValue_C = 1.5f, defoultValue_S = 1.9f;
    private Texture2D textureImage;
    private Material shaderTexture;
    private bool saveImage;
    private bool webCamStop;
    private bool resetCamera;
    private bool isCameraPaused;
    private int indexImage = 00;
    private float shaderbarX;

    void Start()
    {
        webCam = new ();
        webCam.requestedFPS = 30;
        shaderTexture = shaderMaterial.GetComponent<RawImage>().material;
        shaderTexture.SetTexture("_Texture2D", webCam);
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
        yield return new WaitForSeconds(2f);
        yield return WebCamManagerUpdate(1920,1080,1920,1080);
        photoTimerAnimation.SetActive(false);
        uiSaveButton.SetActive(true);
        uiDeleteButton.SetActive(true);
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
        

        byte[] byteArray = textureImage.EncodeToPNG();
      //  string pathDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
       // string fullPath = Path.Combine(pathDocuments, Application.productName);
       // DirectoryInfo imageFolder = Directory.CreateDirectory(fullPath);
        
       // File.WriteAllBytes(fullPath + $"/ScreenShotImage{indexImage}.png", byteArray);
        indexImage++;
        File.WriteAllBytes(Application.dataPath + $"/Resources/ScreenShotImage{indexImage}.png", byteArray);
        Destroy(textureImage);
        ScrollView.SetActive(false);
        brightnees.value = defoultValue_B; temperature.value = defoultValue_T; contrast.value = defoultValue_C; saturation.value = defoultValue_S;
        yield return WebCamManagerUpdate(640,480,1920,1380);
        resetCamera = true;
        CameraManager();
        this.saveImage = false;
        yield return new WaitUntil(() => !this.saveImage && !webCam.isPlaying);
        Debug.Log("foto salva");
        SceneManager.LoadSceneAsync("Quebra-Cabeca");
    }
    public void SetWebCamResolution(int Width, int Height, float SizeX, float SizeY)
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
                print("camera ON");
            }
            else
            {
                if (!webCamStop)
                {
                    webCam.Stop();
                    webCamStop = true;
                    print("camera OFF");
                }
                else
                {
                    webCam.Pause();
                    webCamStop = false;
                    resetCamera = true;
                    print("camera PAUSE");
                }
            }
        }
        else
        {
            webCam.Stop();
            resetCamera = false;
            print("camera OFF");
        }
    }
    private IEnumerator WebCamManagerUpdate(int camResWidth, int camResHeight, float imageSizeX, float imageSizeY)
    {
        CameraManager();
        SetWebCamResolution(camResWidth, camResHeight, imageSizeX, imageSizeY);
        yield return new WaitForSeconds(1f);
        CameraManager();
        yield return new WaitForSeconds(1.3f);
        if (!isCameraPaused)
        {
            CameraManager();
            isCameraPaused = true;
        }
    }
    private void TakeScreenshotAndSaveButton()
    {
        StartCoroutine(TakeScreenShotAndSave());
        _takeScreenshotButton.gameObject.SetActive(false);
    }
    public void SaveDeleteScreenShot(bool saveImage)
    {
        float shaderbarXvalue = shaderBar.position.x;
        shaderbarXvalue = 0;
        shaderBar.ForceUpdateRectTransforms();
        uiSaveButton.SetActive(false);
        uiDeleteButton.SetActive(false);
        _takeScreenshotButton.gameObject.SetActive(true);
        if (saveImage)
        {
            this.saveImage = true;
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(WebCamManagerUpdate(640, 480, 1920, 1380));
            brightnees.value = defoultValue_B; temperature.value = defoultValue_T; contrast.value = defoultValue_C; saturation.value = defoultValue_S;
            ScrollView.SetActive(false);
            Debug.Log("foto apagada");
        }
    }
}


