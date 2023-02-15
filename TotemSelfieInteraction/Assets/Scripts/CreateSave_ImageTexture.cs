using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

public class CreateSave_ImageTexture : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _objToScreenshot;
    [SerializeField] private Button _takeScreenshotButton;
    [SerializeField] private GameObject shaderMaterial, uiSaveButton, uiDeleteButton, S_sliders;

    [Header("Shader Values Control")]
    [SerializeField] private Slider brightnees;
    [SerializeField] private Slider temperature, contrast;
    private float defoultValue_B = 0.6f, defoultValue_T = 0.1f, defoultValue_C = 3.0f;
    private Texture2D textureImage;
    private Material shaderTexture;
    private WebCamTexture webCam;
    private bool saveImage;
    private int indexImage = 00;

    void Start()
    {
        webCam = new();
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
    }
    
    public IEnumerator TakeScreenShotAndSave()
    {
        yield return new WaitForSeconds(5f);

        CameraManager();
        uiSaveButton.SetActive(true);
        uiDeleteButton.SetActive(true);
        S_sliders.SetActive(true);
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
        string pathDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string fullPath = Path.Combine(pathDocuments, Application.productName);
        DirectoryInfo imageFolder = Directory.CreateDirectory(fullPath);
        
        indexImage++;
        File.WriteAllBytes(fullPath + $"/ScreenShotImage{indexImage}.png", byteArray);
        Destroy(textureImage);
        CameraManager();
        this.saveImage = false;
        S_sliders.SetActive(false);
        brightnees.value = defoultValue_B; temperature.value = defoultValue_T; contrast.value = defoultValue_C;
        Debug.Log("foto salva");
    }

    public void CameraManager()
    {
        if (!webCam.isPlaying)
        {
            webCam.Play();
        }
        else
        {
            webCam.Pause();
        }
    }
    private void TakeScreenshotAndSaveButton()
    {
        StartCoroutine(TakeScreenShotAndSave());
        _takeScreenshotButton.gameObject.SetActive(false);
    }
    public void SaveDeleteScreenShot(bool saveImage)
    {
        uiSaveButton.SetActive(false);
        uiDeleteButton.SetActive(false);
        _takeScreenshotButton.gameObject.SetActive(true);
        if (saveImage)
        {
            this.saveImage = true;
        }
        else
        {
            S_sliders.SetActive(false);
            StopAllCoroutines();
            CameraManager();
            Debug.Log("foto apagada");
        }
    }
}

