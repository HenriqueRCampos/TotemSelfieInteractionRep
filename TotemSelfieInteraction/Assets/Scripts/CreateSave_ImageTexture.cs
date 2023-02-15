using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

public class CreateSave_ImageTexture : MonoBehaviour
{
    [SerializeField] private RectTransform _objToScreenshot;
    [SerializeField] private Button _takeScreenshotButton;
    public GameObject objRenderCameraViewImage, objRenderCameraViewImagetwo, uiPhotoButton, uiSaveButton, uiDeleteButton;
    Texture2D textureImage;
    WebCamTexture webCam;
    bool saveImage;
    int indexImage = 00;

    void Start()
    {
        
        webCam = new();
        objRenderCameraViewImage.GetComponent<RawImage>().texture= webCam;
        objRenderCameraViewImagetwo.GetComponent<RawImage>().texture = webCam;
        Material shaderTexture = objRenderCameraViewImagetwo.GetComponent<RawImage>().material;
        shaderTexture.SetTexture("_Texture2D", webCam);
        CameraManager();
        _takeScreenshotButton.onClick.AddListener(TakeScreenshotAndSaveButton);
    }
    private void TakeScreenshotAndSaveButton()
    {
        StartCoroutine(TakeScreenShotAndSave());
        uiPhotoButton.SetActive(false);
    }
    
    public IEnumerator TakeScreenShotAndSave()
    {
        yield return new WaitForSeconds(5f);

        CameraManager();
        uiSaveButton.SetActive(true);
        uiDeleteButton.SetActive(true);
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
    public void SaveDeleteScreenShotButton(bool button)
    {
        uiSaveButton.SetActive(false);
        uiDeleteButton.SetActive(false);
        uiPhotoButton.SetActive(true);
        if (button)
        {
            SaveDeleteScreenShot(true);
        }
        else
        {
            SaveDeleteScreenShot(false);
        }
    }


    public void SaveDeleteScreenShot(bool saveImage)
    {
        if (saveImage)
        {
            this.saveImage = true;
        }
        else
        {
            StopAllCoroutines();
            CameraManager();
            Debug.Log("foto apagada");
        }
    }
}

