using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundEffect : MonoBehaviour
{
    [SerializeField] private float effectSpeed = 1f;
    private Material effectMaterial;
    private Vector2 effectVector;

    void Start()
    {
        effectMaterial = GetComponent<Image>().material;
        effectMaterial.mainTextureOffset = new Vector2(0, 0);
        effectVector = new Vector2(0, effectSpeed);
    }

    void Update()
    {
        ChangeImageOffset();
    }
    void ChangeImageOffset()
    {
        effectMaterial.mainTextureOffset += effectVector * Time.deltaTime;
        if (effectMaterial.mainTextureOffset.y > 1)
        {
            effectMaterial.mainTextureOffset = new Vector2(0,0);
        }
    }
}
