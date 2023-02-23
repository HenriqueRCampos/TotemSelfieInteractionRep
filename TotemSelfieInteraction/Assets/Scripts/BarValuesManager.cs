using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarValuesManager : MonoBehaviour
{
    
    private Text currentShaderbarValue;
    private int[] activeEffects;
    void Start()
    {
        activeEffects = new int[transform.childCount];
    }

    void Update()
    {
        
    }
}
